using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.NaturalLanguage
{
    public class Token
    {
        [XmlAttribute("lang")]
        public string Language { get; set; }
        [XmlAttribute("t")]
        public TokenTypeOption Type { get; set; }
        [XmlAttribute("c")]
        public string Content { get; set; }
    }
}
