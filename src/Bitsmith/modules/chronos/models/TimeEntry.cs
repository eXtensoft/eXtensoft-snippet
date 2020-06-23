using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class TimeEntry
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("masterId")]
        public string MasterId { get; set; }
        public TagIdentifier Task { get; set; }
        public TagIdentifier Actor { get; set; }
        public TagIdentifier Role { get; set; }
        public TagIdentifier Activity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Started { get; set; }
        public int Minutes { get; set; }
        public string Comment { get; set; }

    }
}
