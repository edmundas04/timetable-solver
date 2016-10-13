using System.Collections.Generic;

namespace TimetableSolver.Models
{
    internal class Class
    {
        internal int Id { get; set; }
        internal List<TeachingGroup> TeachingGroups { get; set; }
    }
}
