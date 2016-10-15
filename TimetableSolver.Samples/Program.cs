using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;
using TimetableSolver.Solvers;

namespace TimetableSolver.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. SimpleSolver example/n");
            Console.Write("Enter example number: ");
            var exampleNumber = Console.ReadLine();
            switch (exampleNumber)
            {
                case "1":
                    SimpleSolverExample();
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }

        private static void SimpleSolverExample()
        {
            Console.Write("Enter execution time in seconds: ");
            var timeToExecute = int.Parse(Console.ReadLine());
            var index = 0;

            var timetable = TimetableBuilder.GetTimetable();
            var fitnessCalculator = new SimpleFitnessCalculator(timetable, 100, 4, 100, 4, 1);
            var mutator = new SimpleMutator(new List<IMutation> { new SimpleMutation() }, timetable);

            var solver = new SimpleSolver(mutator, fitnessCalculator, timetable);
            PrintTimetableInfo(timetable, 100, 4, 100, 4, 1);
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
                PrintTimetableInfo(optimization.Result, 100, 4, 100, 4, 1);
            }

            Console.WriteLine("OptimizationEnded");
            Console.WriteLine($"Iterations: {solver.Iterations}");
            Console.Read();
        }

        private static void PrintTimetableInfo(Timetable timetable, int teacherCollisionPenalty,
            int teacherWindowPenalty, int classCollisionPenalty, int classWindowPenalty,
            int classFrontWindowPenalty)
        {
            var fitnessCalculator = new SimpleFitnessCalculator(timetable, teacherCollisionPenalty, 
                teacherWindowPenalty, classCollisionPenalty, classWindowPenalty, classFrontWindowPenalty);

            Console.WriteLine($@"Fitness: {fitnessCalculator.GetFitness()}, TeacherCollisions: {fitnessCalculator.TeacherCollisions()}, ClassCollisions: {fitnessCalculator.ClassCollisions()}, TeacherWindows: {fitnessCalculator.TeacherWindows()}, ClassWindows: {fitnessCalculator.ClassWindows()}, ClassFrontWindows: {fitnessCalculator.ClassFrontWindows()}");
        }
    }
}
