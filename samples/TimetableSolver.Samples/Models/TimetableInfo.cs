using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;
using TimetableSolver.Models.Contracts;

namespace TimetableSolver.Samples.Models
{
    public class TimetableInfo
    {
        public List<ClassInfo> Classes { get; set; }
        public List<TeacherInfo> Teachers { get; set; }
        public List<TeachingGroupInfo> TeachingGroups { get; set; }
        public List<ClassAssignedTeachingGroup> ClassAssignedTeachingGroups { get; set; }
        public List<TeacherAssignedTeachingGroup> TeacherAssignedTeachingGroups { get; set; }
        public List<AvailableWeekDayInfo> AvailableWeekDays { get; set; }

        public Timetable ToTimetable()
        {
            var classes = Classes.Select(s => new ClassContract { Id = s.IdClass }).ToList();
            var teachers = Teachers.Select(s => new TeacherContract { Id = s.IdTeacher }).ToList();
            var teachingGroups = TeachingGroups.Select(s => new TeachingGroupContract
            {
                Id = s.IdTeachingGroup,
                LessonsPerWeek = s.LessonsPerWeek,
                Timetable = s.Timetable.Select(x => new TimetableElementContract { DayOfWeek = x.DayOfWeek, LessonNumber = x.LessonNumber }).ToList()
            }).ToList();
            var classAssignedTeachingGroups = ClassAssignedTeachingGroups.Select(s => new ClassAssignedTeachingGroupContract { IdClass = s.IdClass, IdTeachingGroup = s.IdTeachingGroup  }).ToList();
            var teacherAssignedTeachingGroups = TeacherAssignedTeachingGroups.Select(s => new TeacherAssignedTeachingGroupContract { IdTeacher = s.IdTeacher, IdTeachingGroup = s.IdTeachingGroup }).ToList();
            var availableWeekDays = AvailableWeekDays.Select(s => new AvailableWeekDayContract { DayOfWeek = s.DayOfWeek, NumberOfLessons = (short)s.NumberOfLessons }).ToList();

            var result = new Timetable(classes, teachers, teachingGroups, classAssignedTeachingGroups, teacherAssignedTeachingGroups, availableWeekDays);
            return result;
        }

        public void UpdateTimetable(Timetable timetable)
        {
            foreach (var teachingGroupInfo in TeachingGroups)
            {
                teachingGroupInfo.Timetable.Clear();
                var teachingGroup = timetable.TeachingGroups.Single(s => s.Id == teachingGroupInfo.IdTeachingGroup);
                foreach (var dayTime in teachingGroup.Timetable)
                {
                    teachingGroupInfo.Timetable.Add(new TimetableElement
                    {
                        DayOfWeek = TimetableHelper.GetDayOfWeek((short)(dayTime / 100)),
                        LessonNumber = dayTime % 10
                    });
                }
            }
        }
    }
}
