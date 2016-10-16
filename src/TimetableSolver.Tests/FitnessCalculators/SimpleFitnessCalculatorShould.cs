using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;

namespace TimetableSolver.Tests.FitnessCalculators
{
    [TestClass]
    public class SimpleFitnessCalculatorShould
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
            var fintnessCalcaulator = new SimpleFitnessCalculator(_timetable, 1, 0, 0, 0, 0);
            fintnessCalcaulator.GetFitness().Should().Be(11);
        }

        [TestMethod]
        public void CalculateTeacherWindows()
        {
            var fintnessCalcaulator = new SimpleFitnessCalculator(_timetable, 0, 1, 0, 0, 0);
            fintnessCalcaulator.GetFitness().Should().Be(8);
        }

        [TestMethod]
        public void CalculateClassCollisions()
        {
            var fintnessCalcaulator = new SimpleFitnessCalculator(_timetable, 0, 0, 1, 0, 0);
            fintnessCalcaulator.GetFitness().Should().Be(9);
        }

        [TestMethod]
        public void CalculateClassWindows()
        {
            var fintnessCalcaulator = new SimpleFitnessCalculator(_timetable, 0, 0, 0, 1, 0);
            fintnessCalcaulator.GetFitness().Should().Be(10);
        }

        [TestMethod]
        public void CalculateClassFrontWindows()
        {
            var fintnessCalcaulator = new SimpleFitnessCalculator(_timetable, 0, 0, 0, 0, 1);
            fintnessCalcaulator.GetFitness().Should().Be(9);
        }
    }
}
