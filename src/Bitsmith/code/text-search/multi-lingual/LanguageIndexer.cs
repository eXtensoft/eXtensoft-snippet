using Bitsmith.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitsmith.Indexing
{
    public class LanguageIndexer : ILanguageIndexer
    {

        public LanguageSettings LanguageSettings { get; set; }
        public List<char> Whitelist { get; set; } = new List<char>();
        public List<string> Exclusions { get; set; } = new List<string>();

        string ILanguageIndexer.Sanitize(string input)
        {
            return Sanitize(input);
        }

        IEnumerable<string> ILanguageIndexer.Tokenize(string input)
        {
            return Tokenize(input);
        }

        public LanguageIndexer(LanguageSettings languageSettings)
        {
            LanguageSettings = languageSettings;
            Exclusions = (from item in LanguageSettings.Tokens.Where(y => y.Type == TokenTypeOption.Stop) select item.Content).ToList();

            HashSet<char> hs = new HashSet<char>();
            if (LanguageSettings.Tokens.Any(y=>y.Type.Equals(TokenTypeOption.Whitelist)))
            {
                foreach (var item in LanguageSettings.Tokens.Where(x => x.Type.Equals(TokenTypeOption.Whitelist)))
                {
                    foreach (var c in item.Content.ToCharArray())
                    {
                        if (hs.Add(c))
                        {
                            Whitelist.Add(c);
                        }                    
                    }
                }
            }
            else
            {
                Whitelist.AddRange(Span(65, 90));
                Whitelist.AddRange(Span(97, 122));
            }
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

        protected virtual string Sanitize(string input)
        {
            StringBuilder sb = new StringBuilder(input.Length);
            foreach (char c in input.Trim().ToCharArray())
            {
                if (Whitelist.Contains(c))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString().Trim(new char[] { '\\', '/', ';', '.', '@', ' ' });
        }

        protected virtual IEnumerable<string> Tokenize(string input)
        {
            List<string> list = new List<string>();

            string[] parts = input.Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                string token = part;
                if (!Exclusions.Contains(token.ToLower()) && token.Length > 3)
                {
                    list.Add(token);
                }
            }
            return list;
        }

    }
}
