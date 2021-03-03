using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public interface IParser
    {
        string Extensions { get; }
        void Parse(TabularData tabularData);
    }
}
