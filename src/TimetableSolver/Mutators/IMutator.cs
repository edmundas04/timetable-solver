using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators
{
    public interface IMutator
    {
        void SetTimetable(Timetable timetable);
        List<int> Mutate();
        void Commit();
        void Rollback();
    }
}
