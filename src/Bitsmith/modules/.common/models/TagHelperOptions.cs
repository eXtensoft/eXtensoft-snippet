using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    [Flags]
    public enum TagHelperOptions
    {
        None = 0,
        Recent = 1,
        Popular = 2,
        All = 3,
    }
}
