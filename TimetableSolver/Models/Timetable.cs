using System;
using System.Collections.Generic;
using System.Linq;

namespace TimetableSolver.Models
{
    public class Timetable
    {
        public List<Teacher> Teachers { get; set; }
        public List<Class> Classes { get; set; }
        public List<TeachingGroup> TeachingGroups { get; set; }

        public Timetable(List<Contracts.Class> classes, 
            List<Contracts.Teacher> teachers, 
            List<Contracts.TeachingGroup> teachingGroups,
            List<Contracts.ClassAssignedTeachingGroup> classAssignedTeachingGroups, 
            List<Contracts.TeacherAssignedTeachingGroup> teacherAssignedTeachingGroups)
        {
            TeachingGroups = TransformTeachingGroups(teachingGroups);
            Teachers = TransformTeachers(teachers, teacherAssignedTeachingGroups);
            Classes = TransformClasses(classes, classAssignedTeachingGroups);
        }

        private List<Class> TransformClasses(List<Contracts.Class> classes, List<Contracts.ClassAssignedTeachingGroup> classAssignedTeachingGroups)
        {
            var result = new List<Class>();
            foreach (var @class in classes)
            {
                var timetableClass = new Class { Id = @class.Id };
                var teacherTeachingGroupIds = classAssignedTeachingGroups.Where(x => x.IdClass == timetableClass.Id).Select(s => s.IdTeachingGroup);
                timetableClass.TeachingGroups = TeachingGroups.Where(x => teacherTeachingGroupIds.Contains(x.Id)).ToList();
                result.Add(timetableClass);
            }

            return result;
        }

        private List<Teacher> TransformTeachers(List<Contracts.Teacher> teachers, List<Contracts.TeacherAssignedTeachingGroup> teacherAssignedTeachingGroups)
        {
            var result = new List<Teacher>();
            foreach (var teacher in teachers)
            {
                var timetableTeacher = new Teacher { Id = teacher.Id };
                var teacherTeachingGroupIds = teacherAssignedTeachingGroups.Where(x => x.IdTeacher == timetableTeacher.Id).Select(s => s.IdTeachingGroup);
                timetableTeacher.TeachingGroups = TeachingGroups.Where(x => teacherTeachingGroupIds.Contains(x.Id)).ToList();
                result.Add(timetableTeacher);
            }

            return result;
        }

        private List<TeachingGroup> TransformTeachingGroups(List<Contracts.TeachingGroup> teachingGroups)
        {
            return teachingGroups.Select(s => new TeachingGroup { Id = s.Id, LessonsPerWeek = s.LessonsPerWeek, Timetable = TransformTimetableElements(s.Timetable) }).ToList();
        }

        private List<int> TransformTimetableElements(List<Contracts.TimetableElement> timetableElements)
        {
            return timetableElements.Select(s => TransformTimetableElement(s)).ToList();
        }

        private int TransformTimetableElement(Contracts.TimetableElement timetableElement)
        {
            var dayOfWeekWeekNumberMap = new Dictionary<DayOfWeek, int>
            {
                { DayOfWeek.Monday, 1 },
                { DayOfWeek.Tuesday, 2 },
                { DayOfWeek.Wednesday, 3 },
                { DayOfWeek.Thursday, 4 },
                { DayOfWeek.Friday, 5 },
                { DayOfWeek.Saturday, 6 },
                { DayOfWeek.Sunday, 7 }
            };

            return dayOfWeekWeekNumberMap[timetableElement.DayOfWeek] * 100 + timetableElement.LessonNumber;
        }
    }
}
