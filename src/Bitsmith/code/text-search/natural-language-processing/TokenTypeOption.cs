using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.NaturalLanguage
{
    public enum TokenTypeOption
    {
        None = 0,
        Root = 1,
        Prefix = 2,
        Suffix = 3,
        Stop = 4,
        Whitelist = 5,
    }
}
