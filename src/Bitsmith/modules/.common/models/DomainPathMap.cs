using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class DomainPathMap : PathNode
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

    }
}
