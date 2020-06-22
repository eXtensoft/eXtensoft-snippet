using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class TagIdentifier
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("display")]
        public string Display { get; set; }
        [XmlAttribute("token")]
        public string Token { get; set; }
        [XmlAttribute("masterId")]
        public string MasterId { get; set; }
    }
}
