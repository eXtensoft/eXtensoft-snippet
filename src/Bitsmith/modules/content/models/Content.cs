using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class Content
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }
        [XmlElement("Domain")]
        public List<Domain> Domains { get; set; }
        [XmlElement("ContentItem")]
        public List<ContentItem> Items { get; set; }
    }
}
