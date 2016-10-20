using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public interface IMutation
    {
        List<MutationHistory> Mutate(Timetable timetable);
    }
}
