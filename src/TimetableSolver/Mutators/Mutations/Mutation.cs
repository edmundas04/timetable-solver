using System;
using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public class Mutation: IMutation
    {
        public List<MutationHistory> Mutate(List<TeachingGroup> teachingGroups, List<KeyValuePair<short, short>> availableWeekDays, Random random = null)
        {
            random = random ?? new Random();

            var result = new List<MutationHistory>();
            var changeHistoryElement = new MutationHistory();
            var teachingGroupIndex = random.Next(0, teachingGroups.Count);
            var teachingGroup = teachingGroups[teachingGroupIndex];
            var timetableElementIndex = random.Next(0, teachingGroup.Timetable.Count);
            var timetableElement = teachingGroup.Timetable[timetableElementIndex];
            var weekDayIndex = random.Next(0, availableWeekDays.Count);
            var weekDay = availableWeekDays[weekDayIndex].Key;
            var newTimetableElement = weekDay * 100 + random.Next(1, availableWeekDays[weekDayIndex].Value + 1);
            teachingGroup.ChangeDayTime(timetableElement, newTimetableElement);
            changeHistoryElement.IdTeachingGroup = teachingGroup.Id;
            changeHistoryElement.OldValue = timetableElement;
            changeHistoryElement.NewValue = newTimetableElement;
            result.Add(changeHistoryElement);
            return result;
        }
    }
}
