using System;

namespace TimetableSolver.Models.Contracts
{
    public class AvailableWeekDay
    {
        public DayOfWeek DayOfWeek { get; set; }
        public short NumberOfLessons { get; set; }
    }
}
