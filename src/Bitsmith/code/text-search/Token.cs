using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith.FullText
{
    public class Token
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("v")]
        public string Value { get; set; }

        [XmlElement("l")]
        public TokenLocations Locations { get; set; } = new TokenLocations();

        internal void AddLocationPosition(string id, int posX, int posY)
        {
            if (!Locations.Contains(id))
            {
                Locations.Add(new TokenLocation() { Id = id });
            }
            Locations[id].Positions.Add(new TokenPosition() { PosX = posX, PosY = posY });
        }
    }
}
