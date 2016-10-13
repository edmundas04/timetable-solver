using System.Collections.Generic;

namespace TimetableSolver.Models
{
    public class Teacher
    {
        internal int Id { get; set; }
        internal List<TeachingGroup> TeachingGroups { get; set; }
    }
}
