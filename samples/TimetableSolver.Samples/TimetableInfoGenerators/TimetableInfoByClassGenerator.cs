using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableSolver.Samples.Models;

namespace TimetableSolver.Samples.TimetableInfoGenerators
{
    public class TimetableInfoByClassGenerator
    {
        private int _classCount;
        private int _lessonsPerWeekForClass;
        private int _lessonsPerWeekForTeacher;
        private List<AvailableWeekDayInfo> _availableWeekDays;
        private Random _random;
        private Dictionary<int, int> _teachersPool;
        private HashSet<string> _classAssignedTeachers;
        private int _idClassStartIndex;
        private int _idTeacherStartIndex;
        private int _idTeachingGroupIndex;

        public TimetableInfoByClassGenerator(int classCount, int lessonsPerWeekForClass, int lessonsPerWeekForTeacher, List<AvailableWeekDayInfo> availableWeekDays, Random random = null)
        {
            _classCount = classCount;
            _lessonsPerWeekForClass = lessonsPerWeekForClass;
            _lessonsPerWeekForTeacher = lessonsPerWeekForTeacher;
            _availableWeekDays = availableWeekDays;
            _random = random ?? new Random();

            _teachersPool = new Dictionary<int, int>();
            _classAssignedTeachers = new HashSet<string>();
            _idClassStartIndex = 10000;
            _idTeacherStartIndex = 20000;
            _idTeachingGroupIndex = 30000;
        }

        public TimetableInfo Generate()
        {
            var builder = new TimetableInfoBuilder();
            var classes = CreateClasses(builder);

            foreach (var idClass in classes)
            {
                var assignedLessons = 0;

                while(assignedLessons != _lessonsPerWeekForClass)
                {
                    var idTeacher = GetTeacher(builder, idClass);
                    _classAssignedTeachers.Add(idClass.ToString() + idTeacher.ToString());
                    var teachinGroup = GetTeacingGroup(builder, _lessonsPerWeekForClass - assignedLessons, _lessonsPerWeekForTeacher - _teachersPool[idTeacher]);
                    _teachersPool[idTeacher] += teachinGroup.Value;
                    assignedLessons += teachinGroup.Value;
                    builder.AddClassAssignment(idClass, teachinGroup.Key);
                    builder.AddTeacherAssignment(idTeacher, teachinGroup.Key);
                }
            }

            foreach (var availableWeekDay in _availableWeekDays)
            {
                builder.AddAvailableWeekDay(availableWeekDay.DayOfWeek, (short) availableWeekDay.NumberOfLessons);
            }

            return builder.Build();
        }

        private KeyValuePair<int, int> GetTeacingGroup(TimetableInfoBuilder builder, int lessonsToDistributeForClass, int lessonsToDistributeForTeacher)
        {
            var minLessonsToDistribute = Enumerable.Min(new List<int> { lessonsToDistributeForClass, lessonsToDistributeForTeacher, 4 });

            var lessonsPerWeek = _random.Next(1, minLessonsToDistribute + 1);
            var idTeachingGroup = _idTeachingGroupIndex;
            
            builder.AddTeachingGroup(idTeachingGroup, lessonsPerWeek, GetTeachingGroupName(idTeachingGroup), GetLetter());
            _idTeachingGroupIndex++;
            return new KeyValuePair<int, int>(idTeachingGroup, lessonsPerWeek);
        }

        private string GetLetter()
        {
            int num = _random.Next(0, 26);
            char let = (char)('a' + num);
            return let.ToString();
        }

        private int GetTeacher(TimetableInfoBuilder builder, int idClass)
        {
            var teachers = _teachersPool.Where(x => x.Value != _lessonsPerWeekForTeacher && !_classAssignedTeachers.Contains(idClass.ToString() + x.Key.ToString())).ToList();

            if (teachers.Any())
            {
                return teachers[_random.Next(0, teachers.Count)].Key;
            }
            else
            {
                var idTeacher = _idTeacherStartIndex;
                _teachersPool.Add(idTeacher, 0);
                builder.AddTeacher(idTeacher, GetTeacherName(idTeacher), "");
                _idTeacherStartIndex++;
                return idTeacher;
            }
        }

        private string GetTeachingGroupName(int idTeachingGroup)
        {
            return "GR" + ((idTeachingGroup + 1) % 1000);
        }

        private string GetTeacherName(int idTeacher)
        {
            return "Teacher" + ((idTeacher + 1) % 1000);
        } 

        private string GetClassName(int idClass)
        {
            return "Class" + ((idClass + 1) % 1000);
        }

        private List<int> CreateClasses(TimetableInfoBuilder builder)
        {
            var result = new List<int>();

            for (int i = 0; i < _classCount; i++)
            {
                var idClass = _idClassStartIndex;
                result.Add(idClass);
                builder.AddClass(idClass, GetClassName(idClass));
                _idClassStartIndex++;
            }

            return result;
        }
    }
}
