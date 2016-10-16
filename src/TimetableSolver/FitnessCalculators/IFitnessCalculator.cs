using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.FitnessCalculators
{
    public interface IFitnessCalculator
    {
        void SetTimetable(Timetable timetable);
        int GetFitness(List<int> modifiedTeachingGroups);
        void Commit();
        void Rollback();
    }
}
