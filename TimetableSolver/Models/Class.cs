using System.Collections.Generic;

namespace TimetableSolver.Models
{
    public class Class
    {
        public int Id { get; set; }
        public List<TeachingGroup> TeachingGroups { get; set; }
        public List<int> GetTimetable()
        {
            var result = new List<int>();

            foreach (var teachingGroup in TeachingGroups)
            {
                result.AddRange(teachingGroup.Timetable);
            }

            return result;
        }
    }
}
