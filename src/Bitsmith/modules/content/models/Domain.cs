using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class Domain
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("scope")]
        public ScopeOption Scope { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("createdOn")]
        public DateTime CreatedOn { get; set; }
    }
}
