using System.Collections.Generic;

namespace TimetableSolver.Models.Contracts
{
    public class TeachingGroup
    {
        public int Id { get; set; }
        public int LessonsPerWeek { get; set; }
        public List<TimetableElement> Timetable { get; set; }
    }
}
