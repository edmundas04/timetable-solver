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
                foreach (var dayTime in teachingGroup.Timetable)
                {
                    result.Add(dayTime);
                }
            }

            return result;
        }

        public HashSet<int> GetTimetableHashSet()
        {
            var result = new HashSet<int>();
            foreach (var teachingGroup in TeachingGroups)
            {
                foreach (var dayTime in teachingGroup.Timetable)
                {
                    result.Add(dayTime);
                }
            }
            return result;
        }

        public Class Copy(List<TeachingGroup> teachingGroups)
        {
            return new Class
            {
                Id = Id,
                TeachingGroups = teachingGroups
            };
        }
    }
}
