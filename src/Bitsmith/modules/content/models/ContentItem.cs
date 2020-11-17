using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class ContentItem
    {
        [XmlIgnore]
        public bool IsRemove { get; set; }
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("display")]
        public string Display { get; set; }
        [XmlAttribute("mime")]
        public string Mime { get; set; }
        [XmlAttribute("scope")]
        public ScopeOption Scope { get; set; }
        [XmlAttribute("lang")]
        public string Language { get; set; }
        [XmlElement("Path")]
        public List<string> Paths { get; set; } = new List<string>() { };
        [XmlElement("Tag")]
        public List<Property> Properties { get; set; } = new List<Property>();
        public string Body { get; set; }
    }
}
