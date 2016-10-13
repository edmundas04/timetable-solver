using System.Collections.Generic;

namespace TimetableSolver.Models
{
    public class Class
    {
        public int Id { get; set; }
        public List<TeachingGroup> TeachingGroups { get; set; }
    }
}
