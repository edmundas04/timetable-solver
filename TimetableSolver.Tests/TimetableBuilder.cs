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
            var dayOfWeekWeekNumberMap = new Dictionary<short, DayOfWeek>
            {
                { 1, DayOfWeek.Monday },
                { 2, DayOfWeek.Tuesday },
                { 3, DayOfWeek.Wednesday },
                { 4, DayOfWeek.Thursday },
                { 5, DayOfWeek.Friday },
                { 6, DayOfWeek.Saturday },
                { 7, DayOfWeek.Sunday }
            };

            var timetableElements = timetable.Select(s => new TimetableElement { LessonNumber = s % 10, DayOfWeek = dayOfWeekWeekNumberMap[(short) (s / 100)] }).ToList();
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
    }
}
