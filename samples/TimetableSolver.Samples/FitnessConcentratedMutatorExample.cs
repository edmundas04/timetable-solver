using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;
using TimetableSolver.Randomizers;
using TimetableSolver.Samples.Models;
using TimetableSolver.Solvers;

namespace TimetableSolver.Samples
{
    public static class FitnessConcentratedMutatorExample
    {
        public static void Run(Random random = null)
        {
            random = random ?? new Random();
            Console.Write("Enter execution time in seconds: ");
            var timeToExecute = int.Parse(Console.ReadLine());

            //The information for timetable is retrieved. It could be database or some other source
            var timetableInfo = TimetableInfoBuilder.GetRandomTimetableInfo(50, 24, 20, 7, random);
            var solver = BuildSolver(timetableInfo);

            ExampleRunner.Run(timetableInfo, solver, timeToExecute);

            Console.Read();
        }

        private static Solver BuildSolver(TimetableInfo timetableInfo, Random random = null)
        {
            random = random ?? new Random();

            //Timetable information is transformed to timetable object used for optimization
            var timetable = timetableInfo.ToTimetable();

            //If timetable is empty random timetable generated
            var randomizer = new Randomizer();
            randomizer.Randomize(timetable, random);

            //Updated timetable information object with new timtable values
            timetableInfo.UpdateTimetable(timetable);

            var penalties = Penalties.DefaultPenalties();

            //Created object responsible for calculating quality of timetable during optimization
            var fitnessCalculator = new CachedFitnessCalculator(penalties.TeacherCollisionPenalty, penalties.TeacherWindowPenalty, penalties.ClassCollisionPenalty, penalties.ClassWindowPenalty, penalties.ClassFrontWindowPenalty);

            //Created object responsible for making random changes for timetable during optimization
            var mutator = new FitnessConcentratedMutator(new List<IMutation> { new Mutation() }, fitnessCalculator, 2000, random);

            //Solver is created
            var solver = new Solver(mutator, fitnessCalculator, timetable);
            InfoPrinter.PrintTimetableInfo(timetable, penalties, solver.Iterations);
            return solver;
        }
    }
}
