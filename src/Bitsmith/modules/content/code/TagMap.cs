﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class TagMap
    {
        public List<Counter> Counters { get; set; } = new List<Counter>();
        public string Key { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return string.Join(",", from x in Counters select x.Domain);
        }
    }
}
