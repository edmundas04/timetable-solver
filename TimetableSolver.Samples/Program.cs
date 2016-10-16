using System;

namespace TimetableSolver.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. SimpleSolver example\n");
            Console.Write("Enter example number: ");
            var exampleNumber = Console.ReadLine();
            switch (exampleNumber)
            {
                case "1":
                    SimpleSolverExample.Run();
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }        
    }
}
