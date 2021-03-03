using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public class PipeDelimitedParser : DelimitedParser
    {
        public override char Delimiter => '|';


    }
}
