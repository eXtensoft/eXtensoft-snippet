using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class Chronos
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }
        [XmlAttribute("masterId")]
        public string MasterId { get; set; }

        [XmlElement("Entry")]
        public List<TimeEntry> Items { get; set; } = new List<TimeEntry>();

    }
}
