using System.Collections.Generic;

namespace TimetableSolver.Samples.Models
{
    public class TeachingGroupInfo
    {
        public int IdTeachingGroup { get; set; }
        public int LessonsPerWeek { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public List<TimetableElement> Timetable { get; set; }
    }
}