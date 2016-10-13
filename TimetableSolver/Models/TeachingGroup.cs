using System.Collections.Generic;

namespace TimetableSolver.Models
{
    public class TeachingGroup
    {
        public int Id { get; set; }
        public int LessonsPerWeek { get; set; }
        public List<int> Timetable { get; set; }
    }
}
