namespace TimetableSolver.Solvers
{
    public interface ISolverDiagnostics: ISolver
    {
        int BestFitness { get; }
        int Iterations { get; }
    }
}
