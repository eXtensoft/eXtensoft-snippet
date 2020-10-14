using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.FullText
{
    public class FullTextIndex
    {
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }

        [XmlElement("i")]
        public List<TextIndex> Indexes { get; set; }
    }
}
