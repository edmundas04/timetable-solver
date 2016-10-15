using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;
using TimetableSolver.Mutators.Mutations;

namespace TimetableSolver.Mutators
{
    public class SimpleMutator : IMutator
    {
        private List<IMutation> _mutations;
        private Timetable _timetable;
        private Random _random;
        private List<ChangeHistoryElement> _pendingChanges;
        private Dictionary<int, TeachingGroup> _teachingGroups;

        public SimpleMutator(List<IMutation> mutations, Timetable timetable) :this(mutations, timetable, new Random()) { }

        public SimpleMutator(List<IMutation> mutations, Timetable timetable, Random random)
        {
            _mutations = mutations;
            _timetable = timetable;
            _random = random;
            _pendingChanges = new List<ChangeHistoryElement>();
            _teachingGroups = timetable.TeachingGroups.ToDictionary(x => x.Id);
        }

        public List<int> Mutate()
        {
            if(_pendingChanges.Count > 0)
            {
                throw new Exception("Commit or rollback changes before mutation");
            }

            var result = new List<int>();
            var randomMutationIndex = _random.Next(0, _mutations.Count);
            var randomMutation = _mutations[randomMutationIndex];
            _pendingChanges.AddRange(randomMutation.Mutate(_timetable));
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
                teachingGroup.Change(changeHistoryElement.NewValue, changeHistoryElement.OldValue);
            }

            _pendingChanges.Clear();
        }
    }
}
