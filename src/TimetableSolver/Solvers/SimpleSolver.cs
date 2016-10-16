using System.Threading.Tasks;
using TimetableSolver.FitnessCalculators;
using TimetableSolver.Models;
using TimetableSolver.Mutators;

namespace TimetableSolver.Solvers
{
    public class SimpleSolver : ISolver
    {
        private IMutator _mutator;
        private IFitnessCalculator _fitnessCalculator;
        private Timetable _currentTimetable;
        private bool _end;

        public Timetable BestTimetable { get; private set; }
        public int BestFitness { get; private set; }
        public int Iterations { get; private set; }

        public SimpleSolver(IMutator mutator, IFitnessCalculator fitnessCalculator, Timetable timetable)
        {
            _mutator = mutator;
            _fitnessCalculator = fitnessCalculator;
            _currentTimetable = timetable.Copy();
            _end = false;

            _mutator.SetTimetable(_currentTimetable);
            _fitnessCalculator.SetTimetable(_currentTimetable);

            BestTimetable = timetable.Copy();
            BestFitness = fitnessCalculator.GetFitness(null);
            Iterations = 0;
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

                if(newFitness < BestFitness)
                {
                    _mutator.Commit();
                    _fitnessCalculator.Commit();
                    BestTimetable = _currentTimetable.Copy();
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
