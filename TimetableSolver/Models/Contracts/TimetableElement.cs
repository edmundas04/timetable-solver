using System;

namespace TimetableSolver.Models.Contracts
{
    public class TimetableElement
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int LessonNumber { get; set; }
    }
}
