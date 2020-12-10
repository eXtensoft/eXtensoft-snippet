using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.Models
{
    public class Query
    {       
        [XmlAttribute("type")]
        public QueryTypeOption QueryType { get; set; }
        [XmlAttribute("domain")]
        public string Domain { get; set; }
        [XmlAttribute("op")]
        public QueryOperatorOption Operator { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("display")]
        public string Display { get; set; }
        [XmlElement("TokenQuery")]
        public List<TokenQuery> TokenQueries { get; set; } = new List<TokenQuery>();

        public override string ToString()
        {
            if (!String.IsNullOrWhiteSpace(Display))
            {
                return Display;
            }
            else
            {
                var first = TokenQueries.FirstOrDefault();
                if (first != null)
                {
                    return $"{QueryType.ToString()} {first.Token}";
                }
                else
                {
                    return QueryType.ToString();
                }
            }

        }
    }
}
