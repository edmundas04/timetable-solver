using System.Collections.Generic;

namespace TimetableSolver.FitnessCalculators
{
    public interface IFitnessCalculator
    {
        int GetFitness(List<int> modifiedTeachingGroups);
        void Commit();
        void Rollback();
    }
}
