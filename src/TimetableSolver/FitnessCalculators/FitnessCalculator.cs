using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;

namespace TimetableSolver.FitnessCalculators
{
    public class FitnessCalculator : IFitnessCalculator
    {
        private int _teacherCollisionPenalty;
        private int _teacherWindowPenalty;
        private int _classCollisionPenalty;
        private int _classWindowPenalty;
        private int _classFrontWindowPenalty;

        private Timetable _timetable;

        private List<Teacher> _teachers { get; set; }
        private List<Class> _classes { get; set; }
        private List<int> _weekDayTimes;
        private List<int> _weekDayNumbers;

        public FitnessCalculator(int teacherCollisionPenalty, 
            int teacherWindowPenalty, int classCollisionPenalty, int classWindowPenalty, 
            int classFrontWindowPenalty)
        {
            _teacherCollisionPenalty = teacherCollisionPenalty;
            _teacherWindowPenalty = teacherWindowPenalty;
            _classCollisionPenalty = classCollisionPenalty;
            _classWindowPenalty = classWindowPenalty;
            _classFrontWindowPenalty = classFrontWindowPenalty;
        }

        public void SetTimetable(Timetable timetable)
        {
            _timetable = timetable;
            _teachers = timetable.Teachers;
            _classes = timetable.Classes;
            _weekDayTimes = TimetableHelper.AvailableDayTimes(timetable.AvailableWeekDays).OrderBy(x => x).ToList();
            _weekDayNumbers = _weekDayTimes.Select(s => (s / 100) * 100).Distinct().OrderBy(x => x).ToList();
        }

        public int GetFitness(List<int> modifiedTeachingGroups = null)
        {
            CheckTimetableSet();

            var result = 0;

            if(_teacherCollisionPenalty > 0)
            {
                result += TeacherCollisions() * _teacherCollisionPenalty;
            }

            if(_teacherWindowPenalty > 0)
            {
                result += TeacherWindows() * _teacherWindowPenalty;
            }

            if(_classCollisionPenalty > 0)
            {
                result += ClassCollisions() * _classCollisionPenalty;
            }

            if (_classWindowPenalty > 0 && _classFrontWindowPenalty > 0)
            {
                result += ClassWindowsAndFrontWindowsFitness();
            }
            else
            {
                if (_classWindowPenalty > 0)
                {
                    result += ClassWindows() * _classWindowPenalty;
                }

                if (_classFrontWindowPenalty > 0)
                {
                    result += ClassFrontWindows() * _classFrontWindowPenalty;
                }
            }

            return result;
        }

        public int TeacherCollisions()
        {
            int result = 0;

            for (int i = 0; i < _teachers.Count; i++)
            {
                var teacher = _teachers[i];
                var timetable = teacher.GetTimetable();
                result += timetable.Count - timetable.Distinct().Count();
            }

            return result;
        }
        
        public int TeacherWindows()
        {
            int result = 0;

            foreach (var teacher in _teachers)
            {
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
                        result += windowsCount - windowsSinceLast;
                        foundCount = 0;
                        windowsCount = 0;
                        windowsSinceLast = 0;
                    }

                    if (timetableHashSet.Contains(weekDayTime))
                    {
                        foundCount++;
                        windowsSinceLast = 0;
                    }

                    if (foundCount > 0 && !timetableHashSet.Contains(weekDayTime))
                    {
                        windowsCount++;
                        windowsSinceLast++;
                    }
                }
                result += windowsCount - windowsSinceLast;
            }

            return result;
        }

        public int ClassCollisions()
        {
            int result = 0;

            foreach (var @class in _classes)
            {
                var timetable = @class.GetTimetable();
                result += timetable.Count - timetable.Distinct().Count();
            }

            return result;
        }
        
        public int ClassWindows()
        {
            int result = 0;

            foreach (var @class in _classes)
            {
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
                        result += windowsCount - windowsSinceLast;
                        foundCount = 0;
                        windowsCount = 0;
                        windowsSinceLast = 0;
                    }

                    if (timetableHashSet.Contains(weekDayTime))
                    {
                        foundCount++;
                        windowsSinceLast = 0;
                    }

                    if (foundCount > 0 && !timetableHashSet.Contains(weekDayTime))
                    {
                        windowsCount++;
                        windowsSinceLast++;
                    }
                }
                result += windowsCount - windowsSinceLast;
            }

            return result;
        }
        
        public int ClassFrontWindows()
        {
            int result = 0;

            foreach (var @class in _classes)
            {
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
                        result += weekDayTime - weekDayNumber - 1;
                    }
                }
            }

            return result;
        }

        public int ClassWindowsAndFrontWindowsFitness()
        {
            int classWindwos = 0;
            int classFrontWindows = 0;

            foreach (var @class in _classes)
            {
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
                        classWindwos += windowsCount - windowsSinceLast;
                        foundCount = 0;
                        windowsCount = 0;
                        windowsSinceLast = 0;
                    }

                    if (timetableHashSet.Contains(weekDayTime))
                    {
                        foundCount++;
                        if (foundCount == 1)
                        {
                            classFrontWindows += weekDayTime - weekDayNumber - 1;
                        }

                        windowsSinceLast = 0;
                    }

                    if (foundCount > 0 && !timetableHashSet.Contains(weekDayTime))
                    {
                        windowsCount++;
                        windowsSinceLast++;
                    }
                }
                classWindwos += windowsCount - windowsSinceLast;
            }

            return classWindwos * _classWindowPenalty + classFrontWindows * _classFrontWindowPenalty;
        }

        private void CheckTimetableSet()
        {
            if(_timetable == null)
            {
                throw new Exception("Timetable is not set");
            }
        }

        void IFitnessCalculator.Commit()
        {
        }

        void IFitnessCalculator.Rollback()
        {
        }        
    }
}
