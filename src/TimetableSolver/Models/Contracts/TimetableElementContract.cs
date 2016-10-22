using System;

namespace TimetableSolver.Models.Contracts
{
    public class TimetableElementContract
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int LessonNumber { get; set; }
    }
}
