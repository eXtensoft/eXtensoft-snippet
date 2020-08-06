using Biblio.Modules.Bible.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.code
{
    public static class ParserExtensions
    {
        public static VerseText Build(this VerseText verseText,string[] parts)
        {
            if (parts.Count().Equals(4) && 
                !string.IsNullOrWhiteSpace(parts[0]) &&
                Int32.TryParse(parts[1],out int chapter) && 
                Int32.TryParse(parts[2], out int verse) &&
                !string.IsNullOrWhiteSpace(parts[3]))
            {
                
            }

            return verseText;
        }
    }
}
