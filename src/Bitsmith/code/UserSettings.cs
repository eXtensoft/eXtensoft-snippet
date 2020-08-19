using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith
{
    [Serializable]
    public class UserSettings
    {        
        [XmlAttribute("user")]
        public string Username { get; set; }
        [XmlAttribute("machine")]
        public string Machine { get; set; }
        [XmlAttribute("app")]
        public string Application { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }

        [XmlElement("Property")]
        public List<TypedItem> Items { get; set; } = new List<TypedItem>();

    }
}
