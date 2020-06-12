using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.ProjectManagement
{
    [Serializable]
    public class Disposition
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("display")]
        public string Display { get; set; }
        [XmlAttribute("token")]
        public string Token { get; set; }
        [XmlAttribute("start")]
        public DateTime StartedAt { get; set; }
    }
}
