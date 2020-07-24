using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bitsmith.Styx
{
    public class Edge : Item
    {
        public Direction Vector { get; set; }

        [JsonIgnore]
        public Vertex From { get; set; }
        [JsonIgnore]
        public Vertex To { get; set; }
    }
}
