using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;
using TimetableSolver.Randomizers;
using TimetableSolver.Samples.Models;
using TimetableSolver.Solvers;

namespace TimetableSolver.Samples
{
    public static class SimpleSolverExample
    {
        public static void Run()
        {
            Console.Write("Enter execution time in seconds: ");
            var timeToExecute = int.Parse(Console.ReadLine());
            var index = 0;

            //The information for timetable is retrieved. It could be database or some other source
            var timetableInfo = TimetableInfoBuilder.GetTimetableInfo();
            var solver = BuildSimpleSolver(timetableInfo);

            ExportHtml(timetableInfo, "before");

            Console.WriteLine("Optimization started");

            Task<Timetable> optimization;
            while (index != timeToExecute)
            {
                optimization = solver.Solve();
                Thread.Sleep(1000);
                index++;
                solver.Stop();
                optimization.Wait();
                Console.Write($"{index}. ");
                InfoPrinter.PrintTimetableInfo(optimization.Result, Penalties.DefaultPenalties());
            }

            ExportHtml(timetableInfo, "after");

            Console.WriteLine("OptimizationEnded");
            Console.WriteLine($"Iterations: {solver.Iterations}");
            Console.Read();
        }

        private static Solver BuildSimpleSolver(TimetableInfo timetableInfo)
        {
            //Timetable information is transformed to timetable object used for optimization
            var timetable = timetableInfo.ToTimetable();

            //If timetable is empty random timetable generated
            var randomizer = new Randomizer();
            randomizer.Randomize(timetable);

            //Updated timetable information object with new timtable values
            timetableInfo.UpdateTimetable(timetable);

            var penalties = Penalties.DefaultPenalties();

            //Created object responsible for calculating quality of timetable during optimization
            var fitnessCalculator = new FitnessCalculator(penalties.TeacherCollisionPenalty, penalties.TeacherWindowPenalty, penalties.ClassCollisionPenalty, penalties.ClassWindowPenalty, penalties.ClassFrontWindowPenalty);

            //Created object responsible for making random changes for timetable during optimization
            var mutator = new Mutator(new List<IMutation> { new Mutation() });

            //Solver is created
            var solver = new Solver(mutator, fitnessCalculator, timetable);
            InfoPrinter.PrintTimetableInfo(timetable, penalties);
            return solver;
        }

        private static void ExportHtml(TimetableInfo timetableInfo, string fileName)
        {
            //Not implemented yet
        }
    }
}
