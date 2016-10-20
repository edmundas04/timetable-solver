using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Randomizers;

namespace TimetableSolver.Tests
{
    [Ignore]
    [TestClass]
    public class Performance
    {
        [TestMethod]
        public void PerformanceTest()
        {
            //var timetable = TimetableBuilder.GetRandomTimetable(40, 22, 22, 7, new Random(1991));
            //var randomizer = new Randomizer();
            //randomizer.Randomize(timetable, new Random(8));


            //var firstFitenesCalculator = new FitnessCalculator(10, 10, 10, 10, 10);
            //var secondFitenesCalculator = new NewFitnessCalculator(10, 10, 10, 10, 10);
            //firstFitenesCalculator.SetTimetable(timetable);
            //secondFitenesCalculator.SetTimetable(timetable);

            //for (int i = 0; i < 1000; i++)
            //{
            //    firstFitenesCalculator.GetFitness();
            //    secondFitenesCalculator.GetFitness();
            //}

            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            //stopwatch.Stop();
            //stopwatch.Reset();
            //Thread.Sleep(2000);
            //stopwatch.Start();
            //for (int i = 0; i < 30000; i++)
            //{
            //    firstFitenesCalculator.GetFitness();
            //}
            //stopwatch.Stop();
            //var firstFitenesCalculatorTime = stopwatch.ElapsedTicks;
            //stopwatch.Reset();
            //Thread.Sleep(2000);
            //stopwatch.Start();
            //for (int i = 0; i < 30000; i++)
            //{
            //    secondFitenesCalculator.GetFitness();
            //}
            //stopwatch.Stop();
            //var secondFitenesCalculatorTime = stopwatch.ElapsedTicks;
            
            //if(secondFitenesCalculatorTime > firstFitenesCalculatorTime)
            //{
            //    //first won
            //    var diff = (decimal) secondFitenesCalculatorTime - firstFitenesCalculatorTime;
            //    var percent = diff / ((decimal)secondFitenesCalculatorTime);
            //}
            //else
            //{
            //    //second won
            //    var diff = (decimal)firstFitenesCalculatorTime - secondFitenesCalculatorTime;
            //    var percent = diff / ((decimal)firstFitenesCalculatorTime);
            //}
        }
    }
}
