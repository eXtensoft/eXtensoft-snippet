using System;
using System.Collections.Generic;
using System.Text;

namespace Bitsmith.Styx
{
    public class Link
    {
        public string Display { get; set; }
        public Link(string from, string to)
        {
            Display = $"{from}->{to}";
        }
        public Link()
        {

        }
    }
}
