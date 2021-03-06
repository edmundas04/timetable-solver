﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using TimetableSolver.Models;
using TimetableSolver.Mutators;
using TimetableSolver.Mutators.Mutations;

namespace TimetableSolver.Tests.Mutators
{
    [TestClass]
    public class MutatorShould
    {
        private Timetable _timetable;
        private IMutation _mutation;
        private Mutator _simpleMutator;
        private List<MutationHistory> _changeHistoryElements;


        [TestInitialize]
        public void Initialize()
        {
            _timetable = TimetableBuilder.GetTimetable();
            _mutation = Substitute.For<IMutation>();
            FillChangeHistoryElements();
            _mutation.Mutate(Arg.Any<List<TeachingGroup>>(), Arg.Any<List<KeyValuePair<short, short>>>(), Arg.Any<Random>()).Returns(_changeHistoryElements);
            _simpleMutator = new Mutator(new List<IMutation> { _mutation });
            _simpleMutator.SetTimetable(_timetable);
        }

        private void FillChangeHistoryElements()
        {
            _changeHistoryElements = new List<MutationHistory>
            {
                new MutationHistory
                {
                    IdTeachingGroup = 302,
                    NewValue = 101,
                    OldValue = 205
                },
                new MutationHistory
                {
                    IdTeachingGroup = 304,
                    NewValue = 204,
                    OldValue = 302
                },
                new MutationHistory
                {
                    IdTeachingGroup = 307,
                    NewValue = 301,
                    OldValue = 203
                },
                new MutationHistory
                {
                    IdTeachingGroup = 310,
                    NewValue = 102,
                    OldValue = 101
                }
            };
        }

        [TestMethod]
        public void MuatateWithRollback()
        {
            var result = _simpleMutator.Mutate();
            result.Should().NotBeNull();
            result.Should().HaveCount(4);
            result.ShouldBeEquivalentTo(_changeHistoryElements.Select(s => s.IdTeachingGroup));
            _simpleMutator.Rollback();
            var teachingGroup1 = _timetable.TeachingGroups.Single(s => s.Id == _changeHistoryElements[0].IdTeachingGroup);
            teachingGroup1.Timetable.ShouldBeEquivalentTo(new List<int> { 104, 205, 203 });
            var teachingGroup2 = _timetable.TeachingGroups.Single(s => s.Id == _changeHistoryElements[1].IdTeachingGroup);
            teachingGroup2.Timetable.ShouldBeEquivalentTo(new List<int> { 101, 302, 301 });
            var teachingGroup3 = _timetable.TeachingGroups.Single(s => s.Id == _changeHistoryElements[2].IdTeachingGroup);
            teachingGroup3.Timetable.ShouldBeEquivalentTo(new List<int> { 203, 202, 105 });
            var teachingGroup4 = _timetable.TeachingGroups.Single(s => s.Id == _changeHistoryElements[3].IdTeachingGroup);
            teachingGroup4.Timetable.ShouldBeEquivalentTo(new List<int> { 101, 204, 204 });
        }
    }
}
