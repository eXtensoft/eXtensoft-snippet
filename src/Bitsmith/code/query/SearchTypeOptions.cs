using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    [Flags]
    public enum SearchTypeOptions
    {
        None = 0,
        Tag = 1,
        FullText = 2,
        Path = 4,
        File = 8,
        Fuzzy = 16,
        Recent = 32,
    }
}
