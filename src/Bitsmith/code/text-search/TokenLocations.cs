using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.FullText
{
    public class TokenLocations : KeyedCollection<string, TokenLocation>
    {
        protected override string GetKeyForItem(TokenLocation item)
        {
            return item.Id;
        }
    }
}
