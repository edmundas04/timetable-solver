using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators.Mutations;

namespace TimetableSolver.Mutators
{
    public class FitnessConcentratedMutator : IMutator
    {
        private List<IMutation> _mutations;
        private CachedFitnessCalculator _fitnessCalculator;
        private int _refreshRate;
        private Timetable _timetable;
        private Random _random;

        private List<MutationHistory> _pendingChanges;
        private Dictionary<int, TeachingGroup> _teachingGroups;
        private Dictionary<int, Teacher> _teachers;
        private Dictionary<int, Class> _classes;
        private List<TeachingGroup> _teachingGroupsToProcess;
        private int _currentIteration;


        public FitnessConcentratedMutator(List<IMutation> mutations, CachedFitnessCalculator fitnessCalculator, int refreshRate)
            :this(mutations, fitnessCalculator, refreshRate, new Random()) { }

        public FitnessConcentratedMutator(List<IMutation> mutations, CachedFitnessCalculator fitnessCalculator, int refreshRate, Random random)
        {
            _mutations = mutations;
            _fitnessCalculator = fitnessCalculator;
            _refreshRate = refreshRate;
            _random = random;
            _pendingChanges = new List<MutationHistory>();
        }

        public void SetTimetable(Timetable timetable)
        {
            _timetable = timetable;
            _teachingGroups = timetable.TeachingGroups.ToDictionary(x => x.Id);
            _classes = timetable.Classes.ToDictionary(x => x.Id);
            _teachers = timetable.Teachers.ToDictionary(x => x.Id);
            _teachingGroupsToProcess = timetable.TeachingGroups.ToList();
            _currentIteration = 1;
        }

        public List<int> Mutate()
        {
            if (_timetable == null)
            {
                throw new Exception("Timetable is not set");
            }

            if (_pendingChanges.Count > 0)
            {
                throw new Exception("Commit or rollback changes before mutation");
            }

            if(_currentIteration % _refreshRate == 0)
            {
                SetTeachingGroupToProcess();
            }

            _currentIteration++;

            var result = new List<int>();
            var randomMutationIndex = _random.Next(0, _mutations.Count);
            var randomMutation = _mutations[randomMutationIndex];
            _pendingChanges.AddRange(randomMutation.Mutate(_teachingGroupsToProcess, _timetable.AvailableWeekDays, _random));
            return _pendingChanges.Select(s => s.IdTeachingGroup).ToList();
        }

        public void SetTeachingGroupToProcess()
        {
            var result = new List<TeachingGroup>();
            var selectedTeachingGroups = new HashSet<int>();
            _teachingGroupsToProcess.Clear();

            foreach (var teacherFitness in _fitnessCalculator.TeacherFitnessMap)
            {
                if (teacherFitness.Value == 0)
                {
                    continue;
                }

                var teacher = _teachers[teacherFitness.Key];

                foreach (var teachingGroup in teacher.TeachingGroups)
                {
                    if (!selectedTeachingGroups.Contains(teachingGroup.Id))
                    {
                        selectedTeachingGroups.Add(teachingGroup.Id);
                        _teachingGroupsToProcess.Add(teachingGroup);
                    }
                }
            }

            foreach (var classFitness in _fitnessCalculator.ClassFitnessMap)
            {
                if (classFitness.Value == 0)
                {
                    continue;
                }

                var @class = _classes[classFitness.Key];

                foreach (var teachingGroup in @class.TeachingGroups)
                {
                    if (!selectedTeachingGroups.Contains(teachingGroup.Id))
                    {
                        selectedTeachingGroups.Add(teachingGroup.Id);
                        _teachingGroupsToProcess.Add(teachingGroup);
                    }
                }
            }
        }

        public void Commit()
        {
            _pendingChanges.Clear();
        }

        public void Rollback()
        {
            for (int i = _pendingChanges.Count - 1; i >= 0; i--)
            {
                var changeHistoryElement = _pendingChanges[i];
                var teachingGroup = _teachingGroups[changeHistoryElement.IdTeachingGroup];
                teachingGroup.ChangeDayTime(changeHistoryElement.NewValue, changeHistoryElement.OldValue);
            }

            _pendingChanges.Clear();
        }
    }
}
