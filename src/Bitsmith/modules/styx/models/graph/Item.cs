using System;
using System.Collections.Generic;
using System.Text;

namespace Bitsmith.Styx
{
    public class Item
    {
        public Tag Identifier { get; set; }
        public List<Tag> Labels { get; set; }
        public List<Property> Properties { get; set; }
    }
}
