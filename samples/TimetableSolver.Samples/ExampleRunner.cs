using System;
using System.Threading;
using System.Threading.Tasks;
using TimetableSolver.Models;
using TimetableSolver.Samples.Models;
using TimetableSolver.Solvers;

namespace TimetableSolver.Samples
{
    public static class ExampleRunner
    {
        public static void Run(TimetableInfo timetableInfo, ISolverDiagnostics solver, int? timeToExecute = null)
        {
            timeToExecute = timeToExecute ?? 1;

            var environmentName = HtmlExportHelper.PrepareEnvironment();

            HtmlExportHelper.ExportHtml(timetableInfo, environmentName, "before");

            Console.WriteLine("Optimization started");

            Task<Timetable> optimization = null;
            var index = 0;

            while (index != timeToExecute)
            {
                optimization = solver.Solve();
                Thread.Sleep(1000);
                index++;
                solver.Stop();
                optimization.Wait();
                Console.Write($"{index}. ");
                InfoPrinter.PrintTimetableInfo(optimization.Result, Penalties.DefaultPenalties(), solver.Iterations);
            }

            Console.WriteLine("OptimizationEnded");

            if (optimization != null)
            {
                timetableInfo.UpdateTimetable(optimization.Result);
            }

            HtmlExportHelper.ExportHtml(timetableInfo, environmentName, "after");
        }
    }
}
