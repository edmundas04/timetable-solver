using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators;

namespace TimetableSolver.Solvers
{
    public class Solver : ISolverDiagnostics
    {
        private IMutator _mutator;
        private IFitnessCalculator _fitnessCalculator;
        private Timetable _currentTimetable;
        private bool _end;
        private List<KeyValuePair<int, List<int>>> _bestGenes;

        public Timetable BestTimetable
        {
            get
            {
                var result = _currentTimetable.Copy();
                result.ChangeGenes(_bestGenes);
                return result;
            }
        }
        public int BestFitness { get; private set; }
        public int Iterations { get; private set; }

        public Solver(IMutator mutator, IFitnessCalculator fitnessCalculator, Timetable timetable)
        {

            _mutator = mutator;
            _fitnessCalculator = fitnessCalculator;
            _currentTimetable = timetable.Copy();
            CheckTimetable(_currentTimetable);

            _end = false;

            _mutator.SetTimetable(_currentTimetable);
            _fitnessCalculator.SetTimetable(_currentTimetable);

            _bestGenes = _currentTimetable.CopyGenes();
            BestFitness = fitnessCalculator.GetFitness(null);
            Iterations = 0;
        }

        private void CheckTimetable(Timetable timetable)
        {
            foreach (var teachingGroup in timetable.TeachingGroups)
            {
                if(teachingGroup.Timetable.Count != teachingGroup.LessonsPerWeek)
                {
                    throw new Exception("Teaching group timetable must have correct number of values");
                }
            }

            var allTimetables = timetable.TeachingGroups.SelectMany(s => s.Timetable).Distinct();
            var allowedValues = TimetableHelper.AvailableDayTimes(timetable.AvailableWeekDays);
            if(!allTimetables.All(x => allowedValues.Contains(x)))
            {
                throw new Exception("Some timetable values are incorrect");
            }
        }

        public async Task<Timetable> Solve()
        {
            return await Task.Factory.StartNew(() => Start());
        }

        private Timetable Start()
        {
            int newFitness;
            while (true)
            {
                if (_end)
                {
                    _end = false;
                    break;
                }

                Iterations++;

                newFitness = _fitnessCalculator.GetFitness(_mutator.Mutate());

                if(newFitness <= BestFitness)
                {
                    _mutator.Commit();
                    _fitnessCalculator.Commit();
                    _bestGenes = _currentTimetable.CopyGenes();
                    BestFitness = newFitness;
                }
                else
                {
                    _mutator.Rollback();
                    _fitnessCalculator.Rollback();
                }
            }

            return BestTimetable;
        }

        public void Stop()
        {
            _end = true;
        }
    }
}
