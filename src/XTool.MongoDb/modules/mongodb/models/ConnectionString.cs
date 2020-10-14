using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XTool.MongoDb
{
    public class ConnectionString
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("connectedOn")]
        public DateTime ConnectedOn { get; set; }
        [XmlAttribute("display")]
        public string Display { get; set; }
        [XmlAttribute("text")]
        public string Text { get; set; }
        
    }
}
