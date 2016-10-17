using System;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Samples.Models;

namespace TimetableSolver.Samples
{
    public static class InfoPrinter
    {
        public  static void PrintTimetableInfo(Timetable timetable, Penalties penalties)
        {
            var fitnessCalculator = new FitnessCalculator(penalties.TeacherCollisionPenalty,
                penalties.TeacherWindowPenalty, penalties.ClassCollisionPenalty, penalties.ClassWindowPenalty, penalties.ClassFrontWindowPenalty);
            fitnessCalculator.SetTimetable(timetable);

            Console.WriteLine($@"Fitness: {fitnessCalculator.GetFitness()}, TeacherCollisions: {fitnessCalculator.TeacherCollisions()}, ClassCollisions: {fitnessCalculator.ClassCollisions()}, TeacherWindows: {fitnessCalculator.TeacherWindows()}, ClassWindows: {fitnessCalculator.ClassWindows()}, ClassFrontWindows: {fitnessCalculator.ClassFrontWindows()}");
        }
    }
}
