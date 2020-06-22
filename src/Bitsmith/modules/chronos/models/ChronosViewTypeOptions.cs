using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    [Flags]
    public enum ChronosViewTypeOptions
    {
        None = 0,
        Actor = 1,
        Day = 2,
        Week = 4,
        Sprint = 8,
        Project = 16,
        System = 32,
    }
}
