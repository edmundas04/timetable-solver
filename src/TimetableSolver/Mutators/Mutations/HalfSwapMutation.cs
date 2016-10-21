using System;
using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public class HalfSwapMutation : IMutation
    {
        public List<MutationHistory> Mutate(List<TeachingGroup> teachingGroups, List<KeyValuePair<short, short>> availableWeekDays, Random random = null)
        {
            if (teachingGroups.Count < 1)
            {
                throw new ArgumentException("At least one taching group is required");
            }

            random = random ?? new Random();

            var result = new List<MutationHistory>();

            var teachingGroup1 = teachingGroups[random.Next(0, teachingGroups.Count)];
            var teachingGroup2 = teachingGroups[random.Next(0, teachingGroups.Count)];
            var element1 = teachingGroup1.Timetable[random.Next(0, teachingGroup1.Timetable.Count)];
            var element2 = teachingGroup2.Timetable[random.Next(0, teachingGroup2.Timetable.Count)];

            var weekDay = availableWeekDays[random.Next(0, availableWeekDays.Count)];
            var newTimetableElement = weekDay.Key * 100 + random.Next(1, weekDay.Value + 1);

            teachingGroup1.ChangeDayTime(element1, element2);
            teachingGroup2.ChangeDayTime(element2, newTimetableElement);

            result.Add(new MutationHistory { IdTeachingGroup = teachingGroup1.Id, OldValue = element1, NewValue = element2 });
            result.Add(new MutationHistory { IdTeachingGroup = teachingGroup2.Id, OldValue = element2, NewValue = newTimetableElement });

            return result;
        }
    }
}
