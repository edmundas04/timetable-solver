using System.Collections.Generic;

namespace TimetableSolver.Mutators
{
    public interface IMutator
    {
        List<int> Mutate();
        void Commit();
        void Rollback();
    }
}
