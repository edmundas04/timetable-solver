using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Samples.Models;
using TimetableSolver.Samples.TimetableInfoGenerators;

namespace TimetableSolver.Samples
{
    public class TimetableInfoBuilder
    {
        private List<ClassInfo> _classes;
        private List<TeacherInfo> _teachers;
        private List<TeachingGroupInfo> _teachingGroups;
        private List<ClassAssignedTeachingGroup> _classAssignedTeachingGroups;
        private List<TeacherAssignedTeachingGroup> _teacherAssignedTeachingGroups;
        private List<AvailableWeekDayInfo> _availableWeekDays;

        public TimetableInfoBuilder()
        {
            _classes = new List<ClassInfo>();
            _teachers = new List<TeacherInfo>();
            _teachingGroups = new List<TeachingGroupInfo>();
            _classAssignedTeachingGroups = new List<ClassAssignedTeachingGroup>();
            _teacherAssignedTeachingGroups = new List<TeacherAssignedTeachingGroup>();
            _availableWeekDays = new List<AvailableWeekDayInfo>();
        }

        public TimetableInfoBuilder AddClass(int idClass, string name)
        {
            _classes.Add(new ClassInfo { IdClass = idClass,  Name = name});
            return this;
        }

        public TimetableInfoBuilder AddTeacher(int idTeacher, string firstName, string lastName)
        {
            _teachers.Add(new TeacherInfo { IdTeacher = idTeacher, FirstName = firstName, LastName = lastName });
            return this;
        }

        public TimetableInfoBuilder AddTeachingGroup(int idTeachingGroup, int lessonsPerWeek, string name, string subject, List<int> timetable = null)
        {
            timetable = timetable ?? new List<int>();
            var timetableElements = timetable.Select(s => new TimetableElement { LessonNumber = s % 10, DayOfWeek = TimetableHelper.GetDayOfWeek((short)(s / 100)) }).ToList();
            _teachingGroups.Add(new TeachingGroupInfo
            {
                IdTeachingGroup = idTeachingGroup,
                LessonsPerWeek = lessonsPerWeek,
                Name = name,
                Subject = subject,
                Timetable = timetableElements
            });
            return this;
        }

        public TimetableInfoBuilder AddTeacherAssignment(int idTeacher, int idTeachingGroup)
        {
            _teacherAssignedTeachingGroups.Add(new TeacherAssignedTeachingGroup { IdTeacher = idTeacher, IdTeachingGroup = idTeachingGroup });
            return this;
        }

        public TimetableInfoBuilder AddClassAssignment(int idClass, int idTeachingGroup)
        {
            _classAssignedTeachingGroups.Add(new ClassAssignedTeachingGroup { IdClass = idClass, IdTeachingGroup = idTeachingGroup });
            return this;
        }

        public TimetableInfoBuilder AddAvailableWeekDay(DayOfWeek dayOfWeek, short numberOfLessons)
        {
            _availableWeekDays.Add(new AvailableWeekDayInfo { DayOfWeek = dayOfWeek, NumberOfLessons = numberOfLessons });
            return this;
        }

        public TimetableInfo Build()
        {
            var result = new TimetableInfo
            {
                Classes = _classes,
                Teachers = _teachers,
                TeachingGroups = _teachingGroups,
                ClassAssignedTeachingGroups = _classAssignedTeachingGroups,
                AvailableWeekDays = _availableWeekDays,
                TeacherAssignedTeachingGroups = _teacherAssignedTeachingGroups
            };

            _classes = new List<ClassInfo>();
            _teachers = new List<TeacherInfo>();
            _teachingGroups = new List<TeachingGroupInfo>();
            _classAssignedTeachingGroups = new List<ClassAssignedTeachingGroup>();
            _teacherAssignedTeachingGroups = new List<TeacherAssignedTeachingGroup>();
            _availableWeekDays = new List<AvailableWeekDayInfo>();

            return result;
        }

        public static TimetableInfo GetTimetableInfo()
        {
            var builder = new TimetableInfoBuilder();

            builder.AddClass(101, "10A")
            .AddClass(102, "10B")
            .AddClass(103, "10C")
            .AddClass(104, "10D");

            builder.AddTeacher(201, "Albert", "Einstein")
            .AddTeacher(202, "Charles", "Darwin")
            .AddTeacher(203, "John", "von Neumann");

            builder.AddTeachingGroup(301, 3, "PHY1", "Physics")
            .AddTeachingGroup(302, 3, "PHY2", "Physics")
            .AddTeachingGroup(303, 3, "PHY3", "Physics")
            .AddTeachingGroup(304, 3, "PHY4", "Physics")

            .AddTeachingGroup(305, 3, "BIO1", "Biology")
            .AddTeachingGroup(306, 3, "BIO2", "Biology")
            .AddTeachingGroup(307, 3, "BIO3", "Biology")
            .AddTeachingGroup(308, 3, "BIO4", "Biology")

            .AddTeachingGroup(309, 3, "COMP1", "Computer science")
            .AddTeachingGroup(310, 3, "COMP2", "Computer science")
            .AddTeachingGroup(311, 3, "COMP3", "Computer science")
            .AddTeachingGroup(312, 3, "COMP4", "Computer science");

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

        public static TimetableInfo GetRandomTimetableInfo(int classCount, int lessonsPerWeekForClass, int lessonsPerWeekForTeacher, int numberOfLessons, Random random = null)
        {
            var availableWeekDays = new List<AvailableWeekDayInfo>
            {
                new AvailableWeekDayInfo
                {
                    DayOfWeek = DayOfWeek.Monday,
                    NumberOfLessons = numberOfLessons
                },
                new AvailableWeekDayInfo
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    NumberOfLessons = numberOfLessons
                },
                new AvailableWeekDayInfo
                {
                    DayOfWeek = DayOfWeek.Wednesday,
                    NumberOfLessons = numberOfLessons
                },
                new AvailableWeekDayInfo
                {
                    DayOfWeek = DayOfWeek.Thursday,
                    NumberOfLessons = numberOfLessons
                },
                new AvailableWeekDayInfo
                {
                    DayOfWeek = DayOfWeek.Friday,
                    NumberOfLessons = numberOfLessons
                }
            };
            var timetableInfoGenrator = new TimetableInfoByClassGenerator(classCount, lessonsPerWeekForClass, lessonsPerWeekForTeacher, availableWeekDays, random ?? new Random());
            return timetableInfoGenrator.Generate();
        }
    }
}
