using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;

namespace TimetableSolver.FitnessCalculators
{
    public class CachedFitnessCalculator : IFitnessCalculator
    {
        private int _teacherCollisionPenalty;
        private int _teacherWindowPenalty;
        private int _classCollisionPenalty;
        private int _classWindowPenalty;
        private int _classFrontWindowPenalty;

        private Timetable _timetable;

        private List<Teacher> _teachers { get; set; }
        private List<Class> _classes { get; set; }
        private List<TeachingGroup> _teachingGroups { get; set; }
        private List<int> _weekDayTimes;
        private List<int> _weekDayNumbers;

        private Dictionary<int, List<Teacher>> _teachingGroupTeachersMap;
        private Dictionary<int, List<Class>> _teachingGroupClassesMap;
        private Dictionary<int, int> _teacherFitnessMap;
        private Dictionary<int, int> _classFitnessMap;
        private Dictionary<int, int> _newTeacherFitnessMap;
        private Dictionary<int, int> _newClassFitnessMap;
        private List<Teacher> _teachersToProcess;
        private List<Class> _classesToProcess;
        private int _lastFitness;
        private int? _newFitness;

        public CachedFitnessCalculator(int teacherCollisionPenalty,
            int teacherWindowPenalty, int classCollisionPenalty, int classWindowPenalty,
            int classFrontWindowPenalty)
        {
            _teacherCollisionPenalty = teacherCollisionPenalty;
            _teacherWindowPenalty = teacherWindowPenalty;
            _classCollisionPenalty = classCollisionPenalty;
            _classWindowPenalty = classWindowPenalty;
            _classFrontWindowPenalty = classFrontWindowPenalty;
        }

        private void MapTeachingGroupTeachers()
        {
            _teachingGroupTeachersMap = new Dictionary<int, List<Teacher>>();
            foreach (var teachingGroup in _teachingGroups)
            {
                var teachers = _teachers.Where(x => x.TeachingGroups.Any(a => a.Id == teachingGroup.Id)).ToList();
                _teachingGroupTeachersMap.Add(teachingGroup.Id, teachers);
            }
        }

        private void MapTeachingGroupClasses()
        {
            _teachingGroupClassesMap = new Dictionary<int, List<Class>>();
            foreach (var teachingGroup in _teachingGroups)
            {
                var classes = _classes.Where(x => x.TeachingGroups.Any(a => a.Id == teachingGroup.Id)).ToList();
                _teachingGroupClassesMap.Add(teachingGroup.Id, classes);
            }
        }

        private void SetFirstFitness()
        {
            _teachersToProcess = _teachers.Select(s => s).ToList();
            _classesToProcess = _classes.Select(s => s).ToList();
            _newTeacherFitnessMap = _teachersToProcess.ToDictionary(x => x.Id, x => 0);
            _newClassFitnessMap = _classesToProcess.ToDictionary(x => x.Id, x => 0);
            CalculateTeacherCollisionFitness();
            CalculateTeacherWindowsFitness();
            CalculateClassCollisionsFitness();
            CalculateClassWindowsAndFrontWindowsFitness();
            _lastFitness = 0;
            _lastFitness += _newTeacherFitnessMap.Sum(s => s.Value);
            _lastFitness += _newClassFitnessMap.Sum(s => s.Value);
            _newFitness = _lastFitness;
        }

        public void SetTimetable(Timetable timetable)
        {
            _timetable = timetable;
            _teachers = timetable.Teachers;
            _classes = timetable.Classes;
            _teachingGroups = timetable.TeachingGroups;
            _weekDayTimes = TimetableHelper.AvailableDayTimes(timetable.AvailableWeekDays).OrderBy(x => x).ToList();
            _weekDayNumbers = _weekDayTimes.Select(s => (s / 100) * 100).Distinct().OrderBy(x => x).ToList();
            _teacherFitnessMap = _teachers.ToDictionary(x => x.Id, x => 0);
            _classFitnessMap = _classes.ToDictionary(x => x.Id, x => 0);
            MapTeachingGroupTeachers();
            MapTeachingGroupClasses();
            SetFirstFitness();
            Commit();
        }

        public int GetFitness(List<int> modifiedTeachingGroups = null)
        {
            if (_newFitness.HasValue)
            {
                return _newFitness.Value;
            }

            if (modifiedTeachingGroups == null)
            {
                return _lastFitness;
            }

            foreach (var idTeachingGroup in modifiedTeachingGroups)
            {
                var _teachingGroupTeachers = _teachingGroupTeachersMap[idTeachingGroup];
                var _teachingGroupClasses = _teachingGroupClassesMap[idTeachingGroup];

                foreach (var teacher in _teachingGroupTeachers)
                {
                    if (!_newTeacherFitnessMap.ContainsKey(teacher.Id))
                    {
                        _newTeacherFitnessMap.Add(teacher.Id, 0);
                        _teachersToProcess.Add(teacher);
                    }
                }

                foreach (var @class in _teachingGroupClasses)
                {
                    if (!_newClassFitnessMap.ContainsKey(@class.Id))
                    {
                        _newClassFitnessMap.Add(@class.Id, 0);
                        _classesToProcess.Add(@class);
                    }
                }
            }

            if (_teacherCollisionPenalty > 0)
            {
                CalculateTeacherCollisionFitness();
            }

            if(_teacherWindowPenalty > 0)
            {
                CalculateTeacherWindowsFitness();
            }
            
            if(_classCollisionPenalty > 0)
            {
                CalculateClassCollisionsFitness();
            }

            if (_classWindowPenalty > 0 && _classFrontWindowPenalty > 0)
            {
                CalculateClassWindowsAndFrontWindowsFitness();
            }
            else
            {
                if (_classWindowPenalty > 0)
                {
                    CalculateClassWindowsFitness();
                }

                if (_classFrontWindowPenalty > 0)
                {
                    CalculateClassFrontWindowsFitness();
                }
            }

            _newFitness = _lastFitness;

            foreach (var teacher in _teachersToProcess)
            {
                _newFitness += -_teacherFitnessMap[teacher.Id] + _newTeacherFitnessMap[teacher.Id];
            }

            foreach (var @class in _classesToProcess)
            {
                _newFitness += -_classFitnessMap[@class.Id] + _newClassFitnessMap[@class.Id];
            }

            return _newFitness.Value;
        }        

        public void Commit()
        {
            if (!_newFitness.HasValue)
            {
                throw new Exception("No pending changes exists");
            }

            foreach (var teacher in _teachersToProcess)
            {
                _teacherFitnessMap[teacher.Id] = _newTeacherFitnessMap[teacher.Id];
            }

            foreach (var @class in _classesToProcess)
            {
                _classFitnessMap[@class.Id] = _newClassFitnessMap[@class.Id];
            }

            _lastFitness = _newFitness.Value;
            _newFitness = null;
            _teachersToProcess.Clear();
            _classesToProcess.Clear();
            _newTeacherFitnessMap.Clear();
            _newClassFitnessMap.Clear();
        }        

        public void Rollback()
        {
            if (!_newFitness.HasValue)
            {
                throw new Exception("No pending changes exists");
            }

            _newFitness = null;
            _teachersToProcess.Clear();
            _classesToProcess.Clear();
            _newTeacherFitnessMap.Clear();
            _newClassFitnessMap.Clear();
        }

        public List<TeachingGroup> GetTeachingGroupCausingPenalties()
        {
            var result = new List<TeachingGroup>();
            var selectedTeachingGroups = new HashSet<int>();

            foreach (var teacher in _teachers)
            {
                if(_teacherFitnessMap[teacher.Id] == 0)
                {
                    continue;
                }

                foreach (var teachingGroup in teacher.TeachingGroups)
                {
                    if (!selectedTeachingGroups.Contains(teachingGroup.Id))
                    {
                        selectedTeachingGroups.Add(teachingGroup.Id);
                        result.Add(teachingGroup);
                    }
                }
            }

            foreach (var @class in _classes)
            {
                if (_classFitnessMap[@class.Id] == 0)
                {
                    continue;
                }

                foreach (var teachingGroup in @class.TeachingGroups)
                {
                    if (!selectedTeachingGroups.Contains(teachingGroup.Id))
                    {
                        selectedTeachingGroups.Add(teachingGroup.Id);
                        result.Add(teachingGroup);
                    }
                }
            }

            return result;
        }

        private void CalculateTeacherCollisionFitness()
        {
            foreach (var teacher in _teachersToProcess)
            {
                var timetable = teacher.GetTimetable();
                _newTeacherFitnessMap[teacher.Id] += (timetable.Count - timetable.Distinct().Count()) * _teacherCollisionPenalty;
            }
        }

        private void CalculateTeacherWindowsFitness()
        {
            foreach (var teacher in _teachersToProcess)
            {
                var allWindowsCount = 0;
                var timetableHashSet = teacher.GetTimetableHashSet();
                var weekDayNumberIndex = 0;
                var weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                var foundCount = 0;
                var windowsCount = 0;
                var windowsSinceLast = 0;

                foreach (var weekDayTime in _weekDayTimes)
                {
                    if ((weekDayTime - weekDayNumber) > 100)
                    {
                        weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                        allWindowsCount += windowsCount - windowsSinceLast;
                        foundCount = 0;
                        windowsCount = 0;
                        windowsSinceLast = 0;
                    }

                    var contains = timetableHashSet.Contains(weekDayTime);

                    if (contains)
                    {
                        foundCount++;
                        windowsSinceLast = 0;
                    }

                    if (foundCount > 0 && !contains)
                    {
                        windowsCount++;
                        windowsSinceLast++;
                    }
                }
                allWindowsCount += windowsCount - windowsSinceLast;

                _newTeacherFitnessMap[teacher.Id] += allWindowsCount * _teacherWindowPenalty;
            }
        }

        private void CalculateClassCollisionsFitness()
        {
            foreach (var @class in _classesToProcess)
            {
                var timetable = @class.GetTimetable();
                _newClassFitnessMap[@class.Id] += (timetable.Count - timetable.Distinct().Count()) * _classCollisionPenalty;
            }
        }

        private void CalculateClassWindowsFitness()
        {
            foreach (var @class in _classesToProcess)
            {
                var allWindowsCount = 0;
                var timetableHashSet = @class.GetTimetableHashSet();
                var weekDayNumberIndex = 0;
                var weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                var foundCount = 0;
                var windowsCount = 0;
                var windowsSinceLast = 0;

                foreach (var weekDayTime in _weekDayTimes)
                {
                    if ((weekDayTime - weekDayNumber) > 100)
                    {
                        weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                        allWindowsCount += windowsCount - windowsSinceLast;
                        foundCount = 0;
                        windowsCount = 0;
                        windowsSinceLast = 0;
                    }

                    var contains = timetableHashSet.Contains(weekDayTime);

                    if (contains)
                    {
                        foundCount++;
                        windowsSinceLast = 0;
                    }

                    if (foundCount > 0 && !contains)
                    {
                        windowsCount++;
                        windowsSinceLast++;
                    }
                }
                allWindowsCount += windowsCount - windowsSinceLast;
                _newClassFitnessMap[@class.Id] += allWindowsCount * _classWindowPenalty;
            }
        }

        private void CalculateClassFrontWindowsFitness()
        {
            foreach (var @class in _classesToProcess)
            {
                var frontWindows = 0;
                var timetableHashSet = @class.GetTimetableHashSet();
                var weekDayNumberIndex = 0;
                var weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                var firstFound = false;

                foreach (var weekDayTime in _weekDayTimes)
                {
                    if ((weekDayTime - weekDayNumber) > 100)
                    {
                        weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                        firstFound = false;
                    }

                    if (firstFound)
                    {
                        continue;
                    }

                    if (timetableHashSet.Contains(weekDayTime))
                    {
                        firstFound = true;
                        frontWindows += weekDayTime - weekDayNumber - 1;
                    }
                }

                _newClassFitnessMap[@class.Id] += frontWindows * _classFrontWindowPenalty;
            }
        }

        private void CalculateClassWindowsAndFrontWindowsFitness()
        {
            foreach (var @class in _classesToProcess)
            {
                int allClassWindwos = 0;
                int classFrontWindows = 0;
                var timetableHashSet = @class.GetTimetableHashSet();
                var weekDayNumberIndex = 0;
                var weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                var foundCount = 0;
                var windowsCount = 0;
                var windowsSinceLast = 0;

                foreach (var weekDayTime in _weekDayTimes)
                {
                    if ((weekDayTime - weekDayNumber) > 100)
                    {
                        weekDayNumber = _weekDayNumbers[weekDayNumberIndex++];
                        allClassWindwos += windowsCount - windowsSinceLast;
                        foundCount = 0;
                        windowsCount = 0;
                        windowsSinceLast = 0;
                    }

                    var contains = timetableHashSet.Contains(weekDayTime);

                    if (contains)
                    {
                        foundCount++;
                        if (foundCount == 1)
                        {
                            classFrontWindows += weekDayTime - weekDayNumber - 1;
                        }

                        windowsSinceLast = 0;
                    }

                    if (foundCount > 0 && !contains)
                    {
                        windowsCount++;
                        windowsSinceLast++;
                    }
                }
                allClassWindwos += windowsCount - windowsSinceLast;

                _newClassFitnessMap[@class.Id] += (allClassWindwos * _classWindowPenalty) + (classFrontWindows * _classFrontWindowPenalty);
            }
        }

    }
}
