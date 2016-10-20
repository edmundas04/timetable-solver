using System;
using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public class Mutation: IMutation
    {
        public List<MutationHistory> Mutate(Timetable timetable, Random random = null)
        {
            random = random ?? new Random();

            var result = new List<MutationHistory>();
            var changeHistoryElement = new MutationHistory();
            var teachingGroupIndex = random.Next(0, timetable.TeachingGroups.Count);
            var teachingGroup = timetable.TeachingGroups[teachingGroupIndex];
            var timetableElementIndex = random.Next(0, teachingGroup.Timetable.Count);
            var timetableElement = teachingGroup.Timetable[timetableElementIndex];
            var weekDayIndex = random.Next(0, timetable.AvailableWeekDays.Count);
            var weekDay = timetable.AvailableWeekDays[weekDayIndex].Key;
            var newTimetableElement = weekDay * 100 + random.Next(1, timetable.AvailableWeekDays[weekDayIndex].Value + 1);
            teachingGroup.ChangeDayTime(timetableElement, newTimetableElement);
            changeHistoryElement.IdTeachingGroup = teachingGroup.Id;
            changeHistoryElement.OldValue = timetableElement;
            changeHistoryElement.NewValue = newTimetableElement;
            result.Add(changeHistoryElement);
            return result;
        }
    }
}
