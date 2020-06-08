using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class ListItem
    {
        public string Id { get; set; }
        public TagIdentifier Identifier { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }

        public int Sort { get; set; }
        public int Count { get; set; }
        

    }
}
