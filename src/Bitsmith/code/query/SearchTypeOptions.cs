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
        TagValue = 2,
        FullText = 4,
        Path = 8,
        File = 16,
        Fuzzy = 32,
        Recent = 64,
    }
}
