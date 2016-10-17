using System.Collections.Generic;
using System.Linq;

namespace TimetableSolver.Models
{
    public class TeachingGroup
    {
        public int Id { get; set; }
        public int LessonsPerWeek { get; set; }
        public List<int> Timetable { get; set; }

        public void ChangeDayTime(int from, int to)
        {
            var elementIndex = Timetable.IndexOf(from);
            Timetable[elementIndex] = to;
        }

        public void AddDayTime(int dayTime)
        {
            Timetable.Add(dayTime);
        }

        public TeachingGroup Copy()
        {
            return new TeachingGroup
            {
                Id = Id,
                LessonsPerWeek = LessonsPerWeek,
                Timetable = Timetable.Select(s => s).ToList()
            };
        }
    }
}
