using System.Collections.Generic;
using System.Linq;

namespace TimetableSolver.Samples.Models
{
    public class TimetableInfoViewModel
    {
        public List<WeekDayViewModel> WeekDays { get; set; }
        public List<TimetableMemberViewModel> TimetableMembers { get;set; }


        public TimetableInfoViewModel(TimetableInfo timetable)
        {
            TransformAvailableWeekDays(timetable.AvailableWeekDays);
            TransformTimetableMembers(timetable.Classes, timetable.Teachers, timetable.TeachingGroups, timetable.ClassAssignedTeachingGroups, timetable.TeacherAssignedTeachingGroups);
        }

        private void TransformTimetableMembers(List<ClassInfo> classes, List<TeacherInfo> teachers, List<TeachingGroupInfo> teachingGroups, 
            List<ClassAssignedTeachingGroup> classAssignedTeachingGroup, List<TeacherAssignedTeachingGroup> teacherAssignedTeachingGroup)
        {
            TimetableMembers = new List<TimetableMemberViewModel>();

            foreach (var @class in classes)
            {
                var classTeachinGroups = GetTeachingGroupsByClass(@class.IdClass, teachingGroups, classAssignedTeachingGroup);
                var elements = new List<ElementViewModel>();

                foreach (var teachingGroup in classTeachinGroups)
                {
                    elements.AddRange(teachingGroup.Timetable.Select(s =>
                    new ElementViewModel
                    {
                        IdTeachingGroup = teachingGroup.IdTeachingGroup,
                        DayTime = s.LessonNumber,
                        DayOfWeek = TimetableHelper.GetWeekNumber(s.DayOfWeek),
                        ShortName = teachingGroup.Subject[0].ToString().ToUpper(),
                        SubjectName = teachingGroup.Subject,
                        TeachingGroupName = teachingGroup.Name
                    }));
                }

                TimetableMembers.Add(new TimetableMemberViewModel
                {
                    Id = @class.IdClass,
                    Type = "class",
                    Name = @class.Name,
                    Elements = elements
                });
            }

            foreach (var teacher in teachers)
            {
                var teacherTeachinGroups = GetTeachingGroupsByTeacher(teacher.IdTeacher, teachingGroups, teacherAssignedTeachingGroup);
                var elements = new List<ElementViewModel>();

                foreach (var teachingGroup in teacherTeachinGroups)
                {
                    elements.AddRange(teachingGroup.Timetable.Select(s =>
                    new ElementViewModel
                    {
                        IdTeachingGroup = teachingGroup.IdTeachingGroup,
                        DayTime = s.LessonNumber,
                        DayOfWeek = TimetableHelper.GetWeekNumber(s.DayOfWeek),
                        ShortName = teachingGroup.Subject[0].ToString().ToUpper(),
                        SubjectName = teachingGroup.Subject,
                        TeachingGroupName = teachingGroup.Name
                    }));
                }

                TimetableMembers.Add(new TimetableMemberViewModel
                {
                    Id = teacher.IdTeacher,
                    Type = "teacher",
                    Name = $"{teacher.FirstName} {teacher.LastName}",
                    Elements = elements
                });
            }
        }

        private List<TeachingGroupInfo> GetTeachingGroupsByTeacher(int idTeacher, List<TeachingGroupInfo> teachingGroups, List<TeacherAssignedTeachingGroup> teacherAssignedTeachingGroup)
        {
            var teachingGroupIds = teacherAssignedTeachingGroup.Where(x => x.IdTeacher == idTeacher).Select(s => s.IdTeachingGroup);
            return teachingGroups.Where(x => teachingGroupIds.Contains(x.IdTeachingGroup)).ToList();
        }

        private List<TeachingGroupInfo> GetTeachingGroupsByClass(int inClass, List<TeachingGroupInfo> teachingGroups, List<ClassAssignedTeachingGroup> classAssignedTeachingGroup)
        {
            var teachingGroupIds = classAssignedTeachingGroup.Where(x => x.IdClass == inClass).Select(s => s.IdTeachingGroup);
            return teachingGroups.Where(x => teachingGroupIds.Contains(x.IdTeachingGroup)).ToList();
        }

        private void TransformAvailableWeekDays(List<AvailableWeekDayInfo> availableWeekDays)
        {
            WeekDays = availableWeekDays.Select(s =>
            new WeekDayViewModel
            {
                LessonsPerDay = s.NumberOfLessons,
                Name = s.DayOfWeek.ToString(),
                WeekDayNumber = TimetableHelper.GetWeekNumber(s.DayOfWeek)
            }).ToList();
        }
    }

    public class WeekDayViewModel
    {
        public int WeekDayNumber { get; set; }
        public string Name { get; set; }
        public int LessonsPerDay { get; set; }
    }

    public class TimetableMemberViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<ElementViewModel> Elements { get; set; }
    }

    public class ElementViewModel
    {
        public int IdTeachingGroup { get; set; }
        public string TeachingGroupName { get; set; }
        public string ShortName { get; set; }
        public string SubjectName { get; set; }
        public int DayOfWeek { get; set; }
        public int DayTime { get; set; }
    }
}
