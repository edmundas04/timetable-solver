using System.Collections.Generic;

namespace TimetableSolver.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public List<TeachingGroup> TeachingGroups { get; set; }
    }
}
