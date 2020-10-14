using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.NaturalLanguage
{
    public class Processor
    {
        public List<Token> Tokens { get; set; } = new List<Token>();

        public Processor(IEnumerable<Token> tokens)
        {
            Tokens = tokens.ToList();
        }
    }
}
