using Bitsmith.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Indexing
{
    public interface ILanguageIndexer
    {
        LanguageSettings LanguageSettings { get; set; }
        List<char> Whitelist { get; set; }
        List<string> Exclusions { get; set; }
        string Sanitize(string input);
        IEnumerable<string> Tokenize(string input);
    }
}
