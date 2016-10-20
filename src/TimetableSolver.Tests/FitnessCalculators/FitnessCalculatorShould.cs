using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;

namespace TimetableSolver.Tests.FitnessCalculators
{
    [TestClass]
    public class FitnessCalculatorShould
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
            var fintnessCalcaulator = new FitnessCalculator(1, 0, 0, 0, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness().Should().Be(11);
        }

        [TestMethod]
        public void CalculateTeacherWindows()
        {
            var fintnessCalcaulator = new FitnessCalculator(0, 1, 0, 0, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness().Should().Be(8);
        }

        [TestMethod]
        public void CalculateClassCollisions()
        {
            var fintnessCalcaulator = new FitnessCalculator(0, 0, 1, 0, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness().Should().Be(9);
        }

        [TestMethod]
        public void CalculateClassWindows()
        {
            var fintnessCalcaulator = new FitnessCalculator(0, 0, 0, 1, 0);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness().Should().Be(10);
        }

        [TestMethod]
        public void CalculateClassFrontWindows()
        {
            var fintnessCalcaulator = new FitnessCalculator(0, 0, 0, 0, 1);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness().Should().Be(9);
        }

        [TestMethod]
        public void CalculateClassWindowsAndFrontWindows()
        {
            var fintnessCalcaulator = new FitnessCalculator(0, 0, 0, 1, 1);
            fintnessCalcaulator.SetTimetable(_timetable);
            fintnessCalcaulator.GetFitness().Should().Be(19);
        }
    }
}
