using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;
using TimetableSolver.Randomizers;
using TimetableSolver.Solvers;

namespace TimetableSolver.Tests
{
    [Ignore]
    [TestClass]
    public class Performance
    {
        [TestMethod]
        public void PerformanceTest()
        {
            var simpleFitnessCalculator = new FitnessCalculator(100, 3, 100, 3, 1);
            var random1 = new Random(1991);
            var timetable1 = TimetableBuilder.GetRandomTimetable(200, 22, 22, 7, random1);
            var randomizer1 = new Randomizer();
            randomizer1.Randomize(timetable1, random1);
            var mutator1 = new Mutator(new List<IMutation> { new Mutation() }, random1);
            var solver1 = new Solver(mutator1, simpleFitnessCalculator, timetable1);


            var cachedFitenessCalculator = new CachedFitnessCalculator(100, 3, 100, 3, 1);
            var random2 = new Random(1991);
            var timetable2 = TimetableBuilder.GetRandomTimetable(200, 22, 22, 7, random2);
            var randomizer2 = new Randomizer();
            randomizer1.Randomize(timetable2, random2);
            var mutator2 = new Mutator(new List<IMutation> { new Mutation() }, random2);
            var solver2 = new Solver(mutator2, cachedFitenessCalculator, timetable2);

            Thread.Sleep(2000);

            var optimization1 = solver1.Solve();
            Thread.Sleep(5000);
            solver1.Stop();
            optimization1.Wait();


            var optimization2 = solver2.Solve();
            Thread.Sleep(5000);
            solver2.Stop();
            optimization2.Wait();


            if(solver1.Iterations > solver2.Iterations)
            {
                //simple won
                var diff = (double)solver1.Iterations - solver2.Iterations;
                var percentage = diff / solver1.Iterations;
            }
            else
            {
                //cached won
                var diff = (double)solver2.Iterations - solver1.Iterations;
                var percentage = diff / solver2.Iterations;
            }


        }
    }
}
