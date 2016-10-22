using System.Collections.Generic;

namespace TimetableSolver.Models.Contracts
{
    public class TeachingGroupContract
    {
        public int Id { get; set; }
        public int LessonsPerWeek { get; set; }
        public List<TimetableElementContract> Timetable { get; set; }
    }
}
