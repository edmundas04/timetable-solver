using System;
using System.Collections.Generic;
using TimetableSolver.Models;

namespace TimetableSolver.Mutators.Mutations
{
    public interface IMutation
    {
        List<MutationHistory> Mutate(List<TeachingGroup> teachingGroups, List<KeyValuePair<short, short>> availableWeekDays, Random random = null);
    }
}
