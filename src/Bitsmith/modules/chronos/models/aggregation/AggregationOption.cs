using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public enum AggregationOption
    {
        None = 0,
        Actor = 1,
        Day = 2,
        Week = 4,
        Month = 8,
        Sprint = 16,
        Project = 32,
        System = 64,
    }
}
