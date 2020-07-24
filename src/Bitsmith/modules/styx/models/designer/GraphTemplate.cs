using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Styx
{
    public class GraphTemplate
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt;
        [XmlAttribute("display")]
        public string Display { get; set; }
        public TagIdentifier Identifier { get; set; }
        public string Description { get; set; }
        public Graph Template { get; set; }

    }
}
