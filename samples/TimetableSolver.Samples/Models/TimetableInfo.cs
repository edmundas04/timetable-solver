﻿using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;

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
            var classes = Classes.Select(s => new TimetableSolver.Models.Contracts.Class { Id = s.IdClass }).ToList();
            var teachers = Teachers.Select(s => new TimetableSolver.Models.Contracts.Teacher { Id = s.IdTeacher }).ToList();
            var teachingGroups = TeachingGroups.Select(s => new TimetableSolver.Models.Contracts.TeachingGroup
            {
                Id = s.IdTeachingGroup,
                LessonsPerWeek = s.LessonsPerWeek,
                Timetable = s.Timetable.Select(x => new TimetableSolver.Models.Contracts.TimetableElement { DayOfWeek = x.DayOfWeek, LessonNumber = x.LessonNumber }).ToList()
            }).ToList();
            var classAssignedTeachingGroups = ClassAssignedTeachingGroups.Select(s => new TimetableSolver.Models.Contracts.ClassAssignedTeachingGroup { IdClass = s.IdClass, IdTeachingGroup = s.IdTeachingGroup  }).ToList();
            var teacherAssignedTeachingGroups = TeacherAssignedTeachingGroups.Select(s => new TimetableSolver.Models.Contracts.TeacherAssignedTeachingGroup { IdTeacher = s.IdTeacher, IdTeachingGroup = s.IdTeachingGroup }).ToList();
            var availableWeekDays = AvailableWeekDays.Select(s => new TimetableSolver.Models.Contracts.AvailableWeekDay { DayOfWeek = s.DayOfWeek, NumberOfLessons = (short)s.NumberOfLessons }).ToList();

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
