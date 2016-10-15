using System.Collections.Generic;

namespace TimetableSolver.Models
{
    public class Teacher
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

        public Teacher Copy(List<TeachingGroup> teachingGroups)
        {
            return new Teacher
            {
                Id = Id,
                TeachingGroups = teachingGroups
            };
        }
    }
}
