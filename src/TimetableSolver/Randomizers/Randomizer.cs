using System;
using TimetableSolver.Models;

namespace TimetableSolver.Randomizers
{
    public class Randomizer : IRandomizer
    {
        public void Randomize(Timetable timetable, Random random = null)
        {
            random = random ?? new Random();
            var availableDayTimes = TimetableHelper.AvailableDayTimes(timetable.AvailableWeekDays);
            foreach (var teachingGroup in timetable.TeachingGroups)
            {
                while(teachingGroup.LessonsPerWeek > teachingGroup.Timetable.Count)
                {
                    teachingGroup.AddDayTime(availableDayTimes[random.Next(0, availableDayTimes.Count)]);
                }
            }
        }
    }
}
