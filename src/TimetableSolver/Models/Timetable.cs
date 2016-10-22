using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models.Contracts;

namespace TimetableSolver.Models
{
    public class Timetable
    {
        public List<Teacher> Teachers { get; set; }
        public List<Class> Classes { get; set; }
        public List<TeachingGroup> TeachingGroups { get; set; }
        public List<KeyValuePair<short, short>> AvailableWeekDays { get; set; }

        public Timetable() { }

        public Timetable(List<ClassContract> classes, 
            List<TeacherContract> teachers, 
            List<TeachingGroupContract> teachingGroups,
            List<ClassAssignedTeachingGroupContract> classAssignedTeachingGroups, 
            List<TeacherAssignedTeachingGroupContract> teacherAssignedTeachingGroups,
            List<AvailableWeekDayContract> availableWeekDays)
        {
            Check(classes, teachers, teachingGroups, classAssignedTeachingGroups, teacherAssignedTeachingGroups, availableWeekDays);
            TeachingGroups = TransformTeachingGroups(teachingGroups);
            Teachers = TransformTeachers(teachers, teacherAssignedTeachingGroups);
            Classes = TransformClasses(classes, classAssignedTeachingGroups);
            AvailableWeekDays = TransformAvailableWeekDays(availableWeekDays);
        }

        private void Check(List<ClassContract> classes,
            List<TeacherContract> teachers, List<TeachingGroupContract> teachingGroups, 
            List<ClassAssignedTeachingGroupContract> classAssignedTeachingGroups,
            List<TeacherAssignedTeachingGroupContract> teacherAssignedTeachingGroups,
            List<AvailableWeekDayContract> availableWeekDays)
        {
            if (!classAssignedTeachingGroups.All(x => classes.Any(c => c.Id == x.IdClass)))
            {
                throw new ArgumentException("Some classAssignedTeachingGroups has no reference to class");
            }

            if (!classAssignedTeachingGroups.All(x => teachingGroups.Any(c => c.Id == x.IdTeachingGroup)))
            {
                throw new ArgumentException("Some classAssignedTeachingGroups has no reference to teaching group");
            }

            if (!teacherAssignedTeachingGroups.All(x => teachers.Any(c => c.Id == x.IdTeacher)))
            {
                throw new ArgumentException("Some teacherAssignedTeachingGroups has no reference to teacher");
            }

            if (!teacherAssignedTeachingGroups.All(x => teachingGroups.Any(c => c.Id == x.IdTeachingGroup)))
            {
                throw new ArgumentException("Some teacherAssignedTeachingGroups has no reference to teaching group");
            }

            if(!classes.All(c => classAssignedTeachingGroups.Any(a => a.IdClass == c.Id)))
            {
                throw new ArgumentException("Some classes has no reference to teaching group");
            }

            if (!teachers.All(t => teacherAssignedTeachingGroups.Any(a => a.IdTeacher == t.Id)))
            {
                throw new ArgumentException("Some classes has no reference to teaching group");
            }

            if (availableWeekDays.Count != availableWeekDays.Select(s => s.DayOfWeek).Distinct().Count())
            {
                throw new ArgumentException("Some availableWeekDays duplicated");
            }

            if(!availableWeekDays.All(x => x.NumberOfLessons > 0))
            {
                throw new ArgumentException("NumberOfLessons must be greater than 0");
            }

            var allTimetables = teachingGroups.SelectMany(s => TransformTimetableElements(s.Timetable)).Distinct();
            var allowedValues = TimetableHelper.AvailableDayTimes(TransformAvailableWeekDays(availableWeekDays));
            if (!allTimetables.All(x => allowedValues.Contains(x)))
            {
                throw new ArgumentException("Some timetable values are incorrect");
            }
        }

        private List<KeyValuePair<short, short>> TransformAvailableWeekDays(List<AvailableWeekDayContract> availableWeekDays)
        {
            var result = new List<KeyValuePair<short, short>>();

            foreach (var availableWeekDay in availableWeekDays)
            {
                result.Add(new KeyValuePair<short, short>(TimetableHelper.GetWeekNumber(availableWeekDay.DayOfWeek), availableWeekDay.NumberOfLessons));
            }

            return result;
        }

        private List<Class> TransformClasses(List<ClassContract> classes, List<ClassAssignedTeachingGroupContract> classAssignedTeachingGroups)
        {
            var result = new List<Class>();
            foreach (var @class in classes)
            {
                var teacherTeachingGroupIds = classAssignedTeachingGroups.Where(x => x.IdClass == @class.Id).Select(s => s.IdTeachingGroup).ToList();
                var timetableClass = new Class(@class.Id, TeachingGroups.Where(x => teacherTeachingGroupIds.Contains(x.Id)).ToList());
                result.Add(timetableClass);
            }

            return result;
        }

        private List<Teacher> TransformTeachers(List<TeacherContract> teachers, List<TeacherAssignedTeachingGroupContract> teacherAssignedTeachingGroups)
        {
            var result = new List<Teacher>();
            foreach (var teacher in teachers)
            {
                var teacherTeachingGroupIds = teacherAssignedTeachingGroups.Where(x => x.IdTeacher == teacher.Id).Select(s => s.IdTeachingGroup).ToList();
                var timetableTeacher = new Teacher(teacher.Id, TeachingGroups.Where(x => teacherTeachingGroupIds.Contains(x.Id)).ToList());
                result.Add(timetableTeacher);
            }

            return result;
        }

        private List<TeachingGroup> TransformTeachingGroups(List<TeachingGroupContract> teachingGroups)
        {
            return teachingGroups.Select(s => new TeachingGroup(s.Id, s.LessonsPerWeek, TransformTimetableElements(s.Timetable))).ToList();
        }

        private List<int> TransformTimetableElements(List<TimetableElementContract> timetableElements)
        {
            return timetableElements.Select(s => TransformTimetableElement(s)).ToList();
        }

        private int TransformTimetableElement(TimetableElementContract timetableElement)
        {
            return TimetableHelper.GetWeekNumber(timetableElement.DayOfWeek) * 100 + timetableElement.LessonNumber;
        }

        public Timetable Copy()
        {
            var teachingGroups = TeachingGroups.Select(s => s.Copy()).ToList();

            return new Timetable
            {
                AvailableWeekDays = AvailableWeekDays.Select(s => new KeyValuePair<short, short>(s.Key, s.Value)).ToList(),
                TeachingGroups = teachingGroups,
                Classes = Classes.Select(s => s.Copy(GetClassTeachingGroups(teachingGroups, s))).ToList(),
                Teachers = Teachers.Select(s => s.Copy(GetTeacherTeachingGroups(teachingGroups, s))).ToList()
            };
        }

        public List<KeyValuePair<int, List<int>>> CopyGenes()
        {
            var result = new List<KeyValuePair<int, List<int>>>();

            foreach (var teachingGroup in TeachingGroups)
            {
                result.Add(new KeyValuePair<int, List<int>>(teachingGroup.Id, teachingGroup.Timetable.ToList()));
            }

            return result;
        }

        public void ChangeGenes(List<KeyValuePair<int, List<int>>> genes)
        {
            if(!genes.All(x => TeachingGroups.Any(a => a.Id == x.Key)))
            {
                throw new ArgumentException("Some genes does not have teaching group");
            }

            foreach (var gene in genes)
            {
                var teachingGroup = TeachingGroups.Single(s => s.Id == gene.Key);
                teachingGroup.ChangeTimetable(gene.Value);
            }
        }


        private List<TeachingGroup> GetClassTeachingGroups(List<TeachingGroup> teachingGroups, Class @class)
        {
            var teachingGroupIds = @class.TeachingGroups.Select(s => s.Id);
            return teachingGroups.Where(x => teachingGroupIds.Contains(x.Id)).ToList();
        }

        private List<TeachingGroup> GetTeacherTeachingGroups(List<TeachingGroup> teachingGroups, Teacher teacher)
        {
            var teachingGroupIds = teacher.TeachingGroups.Select(s => s.Id);
            return teachingGroups.Where(x => teachingGroupIds.Contains(x.Id)).ToList();
        }
    }
}
