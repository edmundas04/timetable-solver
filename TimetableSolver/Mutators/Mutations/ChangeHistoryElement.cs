using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableSolver.Mutators.Mutations
{
    public class ChangeHistoryElement
    {
        public int IdTeachingGroup { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }
    }
}
