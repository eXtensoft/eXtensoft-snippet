using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.NaturalLanguage
{
    public class ParserCollection : KeyedCollection<string, Parser>
    {
        protected override string GetKeyForItem(Parser item)
        {
            return item.Language;
        }
    }
}
