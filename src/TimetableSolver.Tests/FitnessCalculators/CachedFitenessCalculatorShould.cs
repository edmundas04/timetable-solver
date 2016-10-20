using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;
using TimetableSolver.Randomizers;

namespace TimetableSolver.Tests.FitnessCalculators
{
    [TestClass]
    public class CachedFitenessCalculatorShould
    {
        private Timetable _timetable;

        [TestInitialize]
        public void Initialize()
        {
            _timetable = TimetableBuilder.GetTimetable();
        }

        [TestMethod]
        public void CalculateTeacherCollisions()
        {
            var fintnessCalcaulator = new CachedFitnessCalculator(1, 0, 0, 0, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness(new List<int>()).Should().Be(11);
        }

        [TestMethod]
        public void CalculateTeacherWindows()
        {
            var fintnessCalcaulator = new CachedFitnessCalculator(0, 1, 0, 0, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness(new List<int>()).Should().Be(8);
        }

        [TestMethod]
        public void CalculateClassCollisions()
        {
            var fintnessCalcaulator = new CachedFitnessCalculator(0, 0, 1, 0, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness(new List<int>()).Should().Be(9);
        }

        [TestMethod]
        public void CalculateClassWindows()
        {
            var fintnessCalcaulator = new CachedFitnessCalculator(0, 0, 0, 1, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness(new List<int>()).Should().Be(10);
        }

        [TestMethod]
        public void CalculateClassFrontWindows()
        {
            var fintnessCalcaulator = new CachedFitnessCalculator(0, 0, 0, 0, 1);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness(new List<int>()).Should().Be(9);
        }

        [TestMethod]
        public void CalculateClassWindowsAndFrontWindows()
        {
            var fintnessCalcaulator = new CachedFitnessCalculator(0, 0, 0, 1, 1);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness(new List<int>()).Should().Be(19);
        }

        [TestMethod]
        public void CachedFitenesCalculatorWorkflowTest()
        {
            var random = new Random(1991);
            var timetable = TimetableBuilder.GetRandomTimetable(40, 22, 22, 7, random);
            var randomizer = new Randomizer();
            randomizer.Randomize(timetable);

            var simpleFitnessCalculator = new FitnessCalculator(14, 16, 21, 20, 13);
            var cachedFitenesCalculator = new CachedFitnessCalculator(14, 16, 21, 20, 13);
            var mutator = new Mutator(new List<IMutation> { new Mutation() }, random);
            simpleFitnessCalculator.SetTimetable(timetable);
            cachedFitenesCalculator.SetTimetable(timetable);
            mutator.SetTimetable(timetable);
            
            var simpleFitness = simpleFitnessCalculator.GetFitness();
            var cachedFitness = cachedFitenesCalculator.GetFitness();
            simpleFitness.Should().Be(cachedFitness);


            for (int i = 0; i < 50; i++)
            {
                if(i / 3  == 0)
                {
                    //commit
                    var changes = mutator.Mutate();
                    simpleFitness = simpleFitnessCalculator.GetFitness(changes);
                    cachedFitness = cachedFitenesCalculator.GetFitness(changes);
                    simpleFitness.Should().Be(cachedFitness);
                    mutator.Commit();
                    cachedFitenesCalculator.Commit();

                }
                else
                {
                    //rollback
                    var changes = mutator.Mutate();
                    simpleFitness = simpleFitnessCalculator.GetFitness(changes);
                    cachedFitness = cachedFitenesCalculator.GetFitness(changes);
                    simpleFitness.Should().Be(cachedFitness);
                    mutator.Rollback();
                    cachedFitenesCalculator.Rollback();
                }
            }


        }
    }
}
