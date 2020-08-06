using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.FullText
{
    public class TextIndexes : KeyedCollection<string, TextIndex>
    {
        protected override string GetKeyForItem(TextIndex item)
        {
            return item.Value;
        }

        internal void Post(string token, string id, int posX, int posY)
        {
            if (!Contains(token.ToLower()))
            {
                Add(new TextIndex() { Value = token.ToLower() });
            }
            this[token.ToLower()].AddToken(token, id, posX, posY);
        }
    }
}
