using System;
using TimetableSolver.Models;

namespace TimetableSolver.Randomizers
{
    public interface IRandomizer
    {
        void Randomize(Timetable timetable, Random random = null);
    }
}
