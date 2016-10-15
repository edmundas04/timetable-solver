using System.Threading.Tasks;
using TimetableSolver.Models;

namespace TimetableSolver.Solvers
{
    public interface ISolver
    {
        Task<Timetable> Solve();
        void Stop();
    }
}
