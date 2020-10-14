using Bitsmith.Indexing;
using Bitsmith.Models;
using Bitsmith.NaturalLanguage;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.FullText
{
    public class SimpleTextIndexer : Indexer
    {
        
        public SimpleTextIndexer(LanguageSettings settings)
        {
            Exclusions = (from item in settings.Tokens.Where(y => y.Type == TokenTypeOption.Stop) select item.Content).ToList();
            Whitelist.AddRange(Span(65, 90));
            Whitelist.AddRange(Span(97, 122));
        }

        private IEnumerable<char> Span(int begin, int end)
        {
            List<char> list = new List<char>();
            for (int i = begin; i <= end; i++)
            {
                var c = (char)i;
                list.Add(c);
            }
            return list;
        }




    }
}
