using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models   
{
    public class Schema
    {
        public string Id { get; set; }
        public TagIdentifier Identifier { get; set; }

        public string SchemaText { get; set; }

    }
}
