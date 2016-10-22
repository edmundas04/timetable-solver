using System;

namespace TimetableSolver.Models.Contracts
{
    public class AvailableWeekDayContract
    {
        public DayOfWeek DayOfWeek { get; set; }
        public short NumberOfLessons { get; set; }
    }
}
