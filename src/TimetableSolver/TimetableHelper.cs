using System;
using System.Collections.Generic;

namespace TimetableSolver
{
    public static class TimetableHelper
    {
        private static Dictionary<DayOfWeek, short> _dayOfWeekWeekNumberMap = new Dictionary<DayOfWeek, short>
            {
                { DayOfWeek.Monday, 1 },
                { DayOfWeek.Tuesday, 2 },
                { DayOfWeek.Wednesday, 3 },
                { DayOfWeek.Thursday, 4 },
                { DayOfWeek.Friday, 5 },
                { DayOfWeek.Saturday, 6 },
                { DayOfWeek.Sunday, 7 }
            };
        private static Dictionary<short, DayOfWeek> _weekNumberDayOfWeekMap = new Dictionary<short, DayOfWeek>
            {
                { 1, DayOfWeek.Monday },
                { 2, DayOfWeek.Tuesday },
                { 3, DayOfWeek.Wednesday },
                { 4, DayOfWeek.Thursday },
                { 5, DayOfWeek.Friday },
                { 6, DayOfWeek.Saturday },
                { 7, DayOfWeek.Sunday }
            };


        public static List<int> AvailableTimes(List<KeyValuePair<short, short>> availableWeekDays)
        {
            var result = new List<int>();
            foreach (var availableWeekDay in availableWeekDays)
            {
                for (int i = 1; i <= availableWeekDay.Value; i++)
                {
                    result.Add(availableWeekDay.Key * 100 + i);
                }
            }

            return result;
        }

        public static DayOfWeek GetDayOfWeek(short weekNumber)
        {
            return _weekNumberDayOfWeekMap[weekNumber];
        }

        public static short GetWeekNumber(DayOfWeek dayOfWeek)
        {
            return _dayOfWeekWeekNumberMap[dayOfWeek];
        }
    }
}
