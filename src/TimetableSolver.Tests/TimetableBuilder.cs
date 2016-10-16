using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;
using TimetableSolver.Models.Contracts;

namespace TimetableSolver.Tests
{
    public class TimetableBuilder
    {
        private List<Models.Contracts.Class> _classes;
        private List<Models.Contracts.Teacher> _teachers;
        private List<Models.Contracts.TeachingGroup> _teachingGroups;
        private List<ClassAssignedTeachingGroup> _classAssignedTeachingGroups;
        private List<TeacherAssignedTeachingGroup> _teacherAssignedTeachingGroups;
        private List<AvailableWeekDay> _availableWeekDays;

        public TimetableBuilder()
        {
            _classes = new List<Models.Contracts.Class>();
            _teachers = new List<Models.Contracts.Teacher>();
            _teachingGroups = new List<Models.Contracts.TeachingGroup>();
            _classAssignedTeachingGroups = new List<ClassAssignedTeachingGroup>();
            _teacherAssignedTeachingGroups = new List<TeacherAssignedTeachingGroup>();
            _availableWeekDays = new List<AvailableWeekDay>();
        }

        public TimetableBuilder AddClass(int idClass)
        {
            _classes.Add(new Models.Contracts.Class { Id = idClass });
            return this;
        }

        public TimetableBuilder AddTeacher(int idTeacher)
        {
            _teachers.Add(new Models.Contracts.Teacher { Id = idTeacher });
            return this;
        }

        public TimetableBuilder AddTeachingGroup(int idTeachingGroup, short lessonsPerWeek, List<int> timetable)
        {
            var timetableElements = timetable.Select(s => new TimetableElement { LessonNumber = s % 10, DayOfWeek = TimetableHelper.GetDayOfWeek((short) (s / 100)) }).ToList();
            _teachingGroups.Add(new Models.Contracts.TeachingGroup { Id = idTeachingGroup, LessonsPerWeek = lessonsPerWeek, Timetable = timetableElements });
            return this;
        }

        public TimetableBuilder AddTeacherAssignment(int idTeacher, int idTeachingGroup)
        {
            _teacherAssignedTeachingGroups.Add(new TeacherAssignedTeachingGroup { IdTeacher = idTeacher, IdTeachingGroup = idTeachingGroup });
            return this;
        }

        public TimetableBuilder AddClassAssignment(int idClass, int idTeachingGroup)
        {
            _classAssignedTeachingGroups.Add(new ClassAssignedTeachingGroup { IdClass = idClass, IdTeachingGroup = idTeachingGroup });
            return this;
        }

        public TimetableBuilder AddAvailableWeekDay(DayOfWeek dayOfWeek, short numberOfLessons)
        {
            _availableWeekDays.Add(new AvailableWeekDay { DayOfWeek = dayOfWeek, NumberOfLessons= numberOfLessons });
            return this;
        }

        public Timetable Build()
        {
            return new Timetable(_classes, _teachers, _teachingGroups, _classAssignedTeachingGroups, _teacherAssignedTeachingGroups, _availableWeekDays);
        }

        public static Timetable GetTimetable()
        {
            var builder = new TimetableBuilder();

            builder.AddClass(101) // 205, 104, 203, 102, 101, 205, 304, 305, 201 //101, 102, 104, 201, 203, 205, 205, 304, 305
            .AddClass(102) //104, 101, 203, 102, 105, 102, 102, 204, 204 //101, 102, 102, 102, 104, 105, 203, 204, 204
            .AddClass(103) //101, 103, 105, 301, 202, 105, 304, 103, 102 //101, 102, 103, 103, 105, 105, 202, 301, 304
            .AddClass(104); //101, 204, 301, 105, 302, 204, 101, 205, 301 //101, 101, 105, 204, 204, 205, 301, 301, 302

            builder.AddTeacher(201) //205, 104, 203, 104, 101, 203, 101, 103, 105, 101, 204, 301 
                                    //101, 101, 101, 103, 104, 104, 105, 203, 203, 204, 205, 301
            .AddTeacher(202) //102, 101, 205, 102, 105, 102, 301, 202, 105, 105, 302, 204
                             //101, 102, 102, 102, 105, 105, 105, 202, 204, 205, 301, 302
            .AddTeacher(203); //304, 305, 201, 102, 204, 204, 304, 103, 102, 101, 205, 301
                              //101, 102, 102, 103, 201, 204, 204, 205, 301, 304, 304, 305

            builder.AddTeachingGroup(301, 3, new List<int> { 205, 104, 203 })
            .AddTeachingGroup(302, 3, new List<int> { 104, 101, 203 })
            .AddTeachingGroup(303, 3, new List<int> { 101, 103, 105 })
            .AddTeachingGroup(304, 3, new List<int> { 101, 204, 301 })

            .AddTeachingGroup(305, 3, new List<int> { 102, 101, 205 })
            .AddTeachingGroup(306, 3, new List<int> { 102, 105, 102 })
            .AddTeachingGroup(307, 3, new List<int> { 301, 202, 105 })
            .AddTeachingGroup(308, 3, new List<int> { 105, 302, 204 })

            .AddTeachingGroup(309, 3, new List<int> { 304, 305, 201 })
            .AddTeachingGroup(310, 3, new List<int> { 102, 204, 204 })
            .AddTeachingGroup(311, 3, new List<int> { 304, 103, 102 })
            .AddTeachingGroup(312, 3, new List<int> { 101, 205, 301 });

            builder.AddClassAssignment(104, 304)
               .AddClassAssignment(104, 308)
               .AddClassAssignment(104, 312)
               .AddTeacherAssignment(201, 304)
               .AddTeacherAssignment(202, 308)
               .AddTeacherAssignment(203, 312);

            builder.AddClassAssignment(103, 303)
                .AddClassAssignment(103, 307)
                .AddClassAssignment(103, 311)
                .AddTeacherAssignment(201, 303)
                .AddTeacherAssignment(202, 307)
                .AddTeacherAssignment(203, 311);

            builder.AddClassAssignment(102, 302)
                .AddClassAssignment(102, 306)
                .AddClassAssignment(102, 310)
                .AddTeacherAssignment(201, 302)
                .AddTeacherAssignment(202, 306)
                .AddTeacherAssignment(203, 310);

            builder.AddClassAssignment(101, 301)
                .AddClassAssignment(101, 305)
                .AddClassAssignment(101, 309)
                .AddTeacherAssignment(201, 301)
                .AddTeacherAssignment(202, 305)
                .AddTeacherAssignment(203, 309);

            builder.AddAvailableWeekDay(DayOfWeek.Monday, 5)
                .AddAvailableWeekDay(DayOfWeek.Tuesday, 5)
                .AddAvailableWeekDay(DayOfWeek.Wednesday, 5);

            return builder.Build();
        }
    }
}
