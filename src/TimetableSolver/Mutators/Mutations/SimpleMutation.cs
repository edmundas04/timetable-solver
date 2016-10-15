using System;
using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public class SimpleMutation: IMutation
    {
        private Random _random;

        public SimpleMutation(): this(new Random()) { }

        public SimpleMutation(Random random)
        {
            _random = random;
        }

        public List<ChangeHistoryElement> Mutate(Timetable timetable)
        {
            var result = new List<ChangeHistoryElement>();
            var changeHistoryElement = new ChangeHistoryElement();
            var teachingGroupIndex = _random.Next(0, timetable.TeachingGroups.Count);
            var teachingGroup = timetable.TeachingGroups[teachingGroupIndex];
            var timetableElementIndex = _random.Next(0, teachingGroup.Timetable.Count);
            var timetableElement = teachingGroup.Timetable[timetableElementIndex];
            var weekDayIndex = _random.Next(0, timetable.AvailableWeekDays.Count);
            var weekDay = timetable.AvailableWeekDays[weekDayIndex].Key;
            var newTimetableElement = weekDay * 100 + _random.Next(1, timetable.AvailableWeekDays[weekDayIndex].Value + 1);
            teachingGroup.Change(timetableElement, newTimetableElement);
            changeHistoryElement.IdTeachingGroup = teachingGroup.Id;
            changeHistoryElement.OldValue = timetableElement;
            changeHistoryElement.NewValue = newTimetableElement;
            result.Add(changeHistoryElement);
            return result;
        }
    }
}
