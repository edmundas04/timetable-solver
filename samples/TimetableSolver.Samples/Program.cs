using System;

namespace TimetableSolver.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Simple Solver example");
            Console.WriteLine("2. Solver with cached fitness calculator example");
            Console.WriteLine("3. Solver with cached fitness calculator and three types of mutations");
            Console.WriteLine("4. Solver with cached fitness calculator, fitness concentrated mutator and three types of mutations");
            Console.WriteLine();
            Console.Write("Enter example number: ");
            var exampleNumber = Console.ReadLine();
            switch (exampleNumber)
            {
                case "1":
                    //Ordinary Solver witch uses ordinary FitnessCalculator, and ordinary Mutator with one added mutation operation called Mutation
                    SimpleSolverExample.Run(new Random(1991));
                    break;
                case "2":
                    //Ordinary Solver witch uses CachedFitnessCalculator, and ordinary Mutator with one added mutation operation called Mutation
                    CachedFitnessSolverExample.Run(new Random(1991));
                    break;
                case "3":
                    //Ordinary Solver witch uses CachedFitnessCalculator, and ordinary Mutator with three added mutation operation called Mutation, SwapMutation, HalfSwapMutation
                    MoreMutationsSolverExample.Run(new Random(1991));
                    break;
                case "4":
                    //Ordinary Solver witch uses CachedFitnessCalculator, and FitnessConcentrateMutator with three added mutation operation called Mutation, SwapMutation, HalfSwapMutation
                    FitnessConcentratedMutatorExample.Run(new Random(1991));
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }        
    }
}
