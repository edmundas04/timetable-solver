using System;
using System.Collections.Generic;
using System.Linq;

namespace TimetableSolver.Models
{
    public class TeachingGroup
    {
        public int Id { get; set; }
        public int LessonsPerWeek { get; set; }
        public List<int> Timetable { get; set; }

        public void ChangeDayTime(int from, int to)
        {
            var elementIndex = Timetable.IndexOf(from);
            Timetable[elementIndex] = to;
        }

        public void AddDayTime(int dayTime)
        {
            Timetable.Add(dayTime);
        }

        public void ChangeTimetable(List<int> newTimetable)
        {
            if(newTimetable.Count != LessonsPerWeek)
            {
                throw new ArgumentException("newTimetable has incorrect number of elements");
            }

            Timetable.Clear();
            Timetable.AddRange(newTimetable);
        }

        public TeachingGroup Copy()
        {
            return new TeachingGroup(Id, LessonsPerWeek, Timetable.ToList());
        }

        public TeachingGroup(int id, int lessonsPerWeek, List<int> timetable)
        {
            Id = id;
            LessonsPerWeek = lessonsPerWeek;
            Timetable = timetable;
        }
    }
}
