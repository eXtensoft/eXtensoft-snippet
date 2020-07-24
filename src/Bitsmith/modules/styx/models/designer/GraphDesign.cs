using Bitsmith.Models;
using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Styx
{
    public class GraphDesign
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }
        [XmlAttribute("display")]
        public string Display { get; set; }
        public string Description { get; set; }
        public TagIdentifier Identifier { get; set; }
        public List<Disposition> Dispositions { get; set; }
        public string GraphText { get; set; }
        public Graph Graph { get; set; }
    }
}
