using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;
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
            var solver = BuildSimpleSolver();

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

            Console.WriteLine("OptimizationEnded");
            Console.WriteLine($"Iterations: {solver.Iterations}");
            Console.Read();
        }

        private static SimpleSolver BuildSimpleSolver()
        {
            var penalties = Penalties.DefaultPenalties();
            var timetable = TimetableBuilder.GetTimetable();

            var fitnessCalculator = new SimpleFitnessCalculator(timetable, penalties.TeacherCollisionPenalty, penalties.TeacherWindowPenalty, penalties.ClassCollisionPenalty, penalties.ClassWindowPenalty, penalties.ClassFrontWindowPenalty);
            var mutator = new SimpleMutator(new List<IMutation> { new SimpleMutation() }, timetable);

            var solver = new SimpleSolver(mutator, fitnessCalculator, timetable);
            InfoPrinter.PrintTimetableInfo(timetable, penalties);
            return solver;
        }
    }
}
