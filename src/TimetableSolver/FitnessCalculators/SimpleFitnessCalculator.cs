using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;

namespace TimetableSolver.FitnessCalculators
{
    public class SimpleFitnessCalculator : IFitnessCalculator
    {
        private int _teacherCollisionPenalty;
        private int _teacherWindowPenalty;
        private int _classCollisionPenalty;
        private int _classWindowPenalty;
        private int _classFrontWindowPenalty;

        private Timetable _timetable;

        private List<Teacher> _teachers { get; set; }
        private List<Class> _classes { get; set; }

        public SimpleFitnessCalculator(int teacherCollisionPenalty, 
            int teacherWindowPenalty, int classCollisionPenalty, int classWindowPenalty, 
            int classFrontWindowPenalty)
        {
            _teacherCollisionPenalty = teacherCollisionPenalty;
            _teacherWindowPenalty = teacherWindowPenalty;
            _classCollisionPenalty = classCollisionPenalty;
            _classWindowPenalty = classWindowPenalty;
            _classFrontWindowPenalty = classFrontWindowPenalty;
        }

        public void SetTimetable(Timetable timetable)
        {
            _timetable = timetable;
            _teachers = timetable.Teachers;
            _classes = timetable.Classes;
        }

        public int GetFitness(List<int> modifiedTeachingGroups = null)
        {
            var result = 0;

            if(_teacherCollisionPenalty > 0)
            {
                result += TeacherCollisions() * _teacherCollisionPenalty;
            }

            if(_teacherWindowPenalty > 0)
            {
                result += TeacherWindows() * _teacherWindowPenalty;
            }

            if(_classCollisionPenalty > 0)
            {
                result += ClassCollisions() * _classCollisionPenalty;
            }

            if(_classWindowPenalty > 0)
            {
                result += ClassWindows() * _classWindowPenalty;
            }

            if(_classFrontWindowPenalty > 0)
            {
                result += ClassFrontWindows() * _classFrontWindowPenalty;
            }

            return result;
        }

        public int TeacherCollisions()
        {
            int result = 0;

            for (int i = 0; i < _teachers.Count; i++)
            {
                var teacher = _teachers[i];
                var timetable = teacher.GetTimetable();
                result += timetable.Count - timetable.Distinct().Count();
            }

            return result;
        }

        //Must be optimized in the future
        public int TeacherWindows()
        {
            int result = 0;

            for (int i = 0; i < _teachers.Count; i++)
            {
                var teacher = _teachers[i];
                var groupedByDayOfWeek = teacher.GetTimetable().GroupBy(x => x / 100).ToList();
                for (int j = 0; j < groupedByDayOfWeek.Count; j++)
                {
                    var dayOfWeekGroup = groupedByDayOfWeek[j].Distinct().ToList();
                    if (dayOfWeekGroup.Count <= 1)
                    {
                        continue;
                    }

                    var min = dayOfWeekGroup.Min();
                    var max = dayOfWeekGroup.Max();
                    result += max - min + 1 - dayOfWeekGroup.Count;
                }
            }

            return result;
        }

        public int ClassCollisions()
        {
            int result = 0;

            foreach (var @class in _classes)
            {
                var timetable = @class.GetTimetable();
                result += timetable.Count - timetable.Distinct().Count();
            }

            return result;
        }

        //Must be optimized in the future
        public int ClassWindows()
        {
            int result = 0;

            for (int i = 0; i < _classes.Count; i++)
            {
                var @class = _classes[i];
                var groupedByDayOfWeek = @class.GetTimetable().GroupBy(x => x / 100).ToList();
                for (int j = 0; j < groupedByDayOfWeek.Count; j++)
                {
                    var dayOfWeekGroup = groupedByDayOfWeek[j].Distinct().ToList();
                    if (dayOfWeekGroup.Count <= 1)
                    {
                        continue;
                    }

                    var min = dayOfWeekGroup.Min();
                    var max = dayOfWeekGroup.Max();
                    result += max - min + 1 - dayOfWeekGroup.Count;
                }
            }

            return result;
        }

        //Must be optimized in the future
        public int ClassFrontWindows()
        {
            int result = 0;

            for (int i = 0; i < _classes.Count; i++)
            {
                var @class = _classes[i];
                var groupedByDayOfWeek = @class.GetTimetable().GroupBy(x => x / 100).ToList();
                for (int j = 0; j < groupedByDayOfWeek.Count; j++)
                {
                    var min = groupedByDayOfWeek[j].Min();
                    result += min % 10 - 1;
                }
            }
            
            return result;
        }

        private void CheckTimetableSet()
        {
            if(_timetable == null)
            {
                throw new Exception("Timetable is not set");
            }
        }

        void IFitnessCalculator.Commit()
        {
        }

        void IFitnessCalculator.Rollback()
        {
        }        
    }
}
