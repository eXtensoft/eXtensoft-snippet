using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    [XmlRoot("tag")]
    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
