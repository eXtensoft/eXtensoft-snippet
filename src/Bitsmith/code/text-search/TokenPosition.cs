using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.FullText
{

    public class TokenPosition
    {
        [XmlAttribute("x")]
        public int PosX { get; set; }
        [XmlAttribute("y")]
        public int PosY { get; set; }
    }
}
