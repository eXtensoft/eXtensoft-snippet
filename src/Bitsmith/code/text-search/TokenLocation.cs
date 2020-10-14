using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.FullText
{
    public class TokenLocation
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlElement("p")]
        public List<TokenPosition> Positions { get; set; } = new List<TokenPosition>();
    }
}
