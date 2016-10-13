using System.Collections.Generic;

namespace TimetableSolver.Models
{
    internal class TeachingGroup
    {
        internal int Id { get; set; }
        internal int LessonsPerWeek { get; set; }
        internal List<int> Timetable { get; set; }
    }
}
