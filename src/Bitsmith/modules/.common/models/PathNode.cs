using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class PathNode 
    {		
        [XmlIgnore]
        public string Path { get; set; }
        [XmlAttribute("slug")]

        public string Slug { get; set; }
        [XmlAttribute("display")]

        public string Display { get; set; }
        [XmlElement("Node")]

        public List<PathNode> Items { get; set; } = new List<PathNode>();

    }
}
