using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class TokenQuery
    {        
        [XmlAttribute("token")]
        public string Token { get; set; }
        [XmlAttribute("searchType")]
        public SearchTypeOptions SearchType { get; set; }
        public List<string> Ids { get; set; } = new List<string>();

    }
}
