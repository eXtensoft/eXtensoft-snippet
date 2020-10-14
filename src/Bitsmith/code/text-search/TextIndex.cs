using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.FullText
{
    public class TextIndex
    {
        [XmlAttribute("v")]
        public string Value { get; set; }
        [XmlElement("t")]
        public TokenCollection Tokens { get; set; } = new TokenCollection();

        internal void AddToken(string token, int posX, int posY)
        {
            if (!Tokens.Contains(token))
            {
                Tokens.Add(new Token() { Value = token });
            }
            //Tokens[token].Locations.Add(tokenLocation);
        }

        public override string ToString()
        {
            return $"{Value}: ({Tokens.Count}) {Tokens}";
        }

        internal void AddToken(string token, string id, int posX, int posY)
        {
            if (!Tokens.Contains(token))
            {
                Tokens.Add(new Token() { Value = token });
            }
            Tokens[token].AddLocationPosition(id, posX, posY);
        }
    }
}
