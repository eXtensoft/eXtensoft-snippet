using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Styx
{
    public class GraphDesigner
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }
        public string Display { get; set; } = "Graphmatic";
        public List<GraphTemplate> Templates { get; set; } = new List<GraphTemplate>();
        public List<GraphDesign> Designs { get; set; } = new List<GraphDesign>();
    }
}
