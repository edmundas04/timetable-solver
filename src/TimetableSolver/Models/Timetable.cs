using System.Collections.Generic;
using System.Linq;

namespace TimetableSolver.Models
{
    public class Timetable
    {
        public List<Teacher> Teachers { get; set; }
        public List<Class> Classes { get; set; }
        public List<TeachingGroup> TeachingGroups { get; set; }
        public List<KeyValuePair<short, short>> AvailableWeekDays { get; set; }

        public Timetable() { }

        public Timetable(List<Contracts.Class> classes, 
            List<Contracts.Teacher> teachers, 
            List<Contracts.TeachingGroup> teachingGroups,
            List<Contracts.ClassAssignedTeachingGroup> classAssignedTeachingGroups, 
            List<Contracts.TeacherAssignedTeachingGroup> teacherAssignedTeachingGroups,
            List<Contracts.AvailableWeekDay> availableWeekDays)
        {
            TeachingGroups = TransformTeachingGroups(teachingGroups);
            Teachers = TransformTeachers(teachers, teacherAssignedTeachingGroups);
            Classes = TransformClasses(classes, classAssignedTeachingGroups);
            AvailableWeekDays = TransformAvailableWeekDays(availableWeekDays);
        }

        private List<KeyValuePair<short, short>> TransformAvailableWeekDays(List<Contracts.AvailableWeekDay> availableWeekDays)
        {
            var result = new List<KeyValuePair<short, short>>();

            foreach (var availableWeekDay in availableWeekDays)
            {
                result.Add(new KeyValuePair<short, short>(TimetableHelper.GetWeekNumber(availableWeekDay.DayOfWeek), availableWeekDay.NumberOfLessons));
            }

            return result;
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
