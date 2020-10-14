using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.FullText
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

        internal void Delete(string id)
        {
            foreach (TextIndex item in this)
            {
                item.Tokens.Delete(id);
            }
        }
   

        public void AddRange(IEnumerable<TextIndex> list)
        {
            foreach (var item in list)
            {
                if (!Contains(item))
                {
                    Add(item);
                }
            }
        }

    }
}
