using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.FullText
{
    public class TokenLocation
    {
        public string Id { get; set; }
        public List<TokenPosition> Positions { get; set; } = new List<TokenPosition>();
    }
}
