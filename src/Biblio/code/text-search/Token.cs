using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.FullText
{
    public class Token
    {
        public string Value { get; set; }
        //public string Id { get; set; }

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
