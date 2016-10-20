using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;
using TimetableSolver.Mutators.Mutations;

namespace TimetableSolver.Mutators
{
    public class Mutator : IMutator
    {
        private List<IMutation> _mutations;
        private Timetable _timetable;
        private Random _random;
        private List<MutationHistory> _pendingChanges;
        private Dictionary<int, TeachingGroup> _teachingGroups;

        public Mutator(List<IMutation> mutations) :this(mutations, new Random()) { }

        public Mutator(List<IMutation> mutations, Random random)
        {
            _mutations = mutations;
            _random = random;
            _pendingChanges = new List<MutationHistory>();
            
        }

        public void SetTimetable(Timetable timetable)
        {
            _timetable = timetable;
            _teachingGroups = timetable.TeachingGroups.ToDictionary(x => x.Id);
        }

        public List<int> Mutate()
        {
            if(_timetable == null)
            {
                throw new Exception("Timetable is not set");
            }

            if(_pendingChanges.Count > 0)
            {
                throw new Exception("Commit or rollback changes before mutation");
            }

            var result = new List<int>();
            var randomMutationIndex = _random.Next(0, _mutations.Count);
            var randomMutation = _mutations[randomMutationIndex];
            _pendingChanges.AddRange(randomMutation.Mutate(_timetable, _random));
            return _pendingChanges.Select(s => s.IdTeachingGroup).ToList();
        }

        public void Commit()
        {
            _pendingChanges.Clear();
        }

        public void Rollback()
        {
            for (int i = _pendingChanges.Count - 1; i >= 0 ; i--)
            {
                var changeHistoryElement = _pendingChanges[i];
                var teachingGroup = _teachingGroups[changeHistoryElement.IdTeachingGroup];
                teachingGroup.ChangeDayTime(changeHistoryElement.NewValue, changeHistoryElement.OldValue);
            }

            _pendingChanges.Clear();
        }
    }
}
