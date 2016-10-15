using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public interface IMutation
    {
        List<ChangeHistoryElement> Mutate(Timetable timetable);
    }
}
