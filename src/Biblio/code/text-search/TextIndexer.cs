using Biblio.Models;
using Biblio.Modules.Bible.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Biblio.FullText
{
    public class TextIndexer
    {
        public List<string> Exclusions { get; set; } = new List<string>();
        public TextIndexes Indexes { get; set; } = new TextIndexes();

        private void Index(string id, string token, int y, int x)
        {
            Indexes.Post(token, id, x, y);
        }
        public Dictionary<string, int> Profiler { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public List<char> Whitelist { get; set; } = new List<char>();
        public TextIndexer()
        {
            PopulateWhitelist();
            Exclusions = new List<string>()
            {
            "of",
            "the",
            "and",
            "when",
            "if",
            "or",
            "not",
            "to",
            "in",
            "with",
            "each",
            "item",
            "shall",
            "have",
            };
        }


        private void PopulateWhitelist()
        {
            //Whitelist.AddRange(Span(47, 57));
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
        public TextIndexer(IEnumerable<string> exclusions)
        {
            Exclusions = new List<string>(exclusions);
            PopulateWhitelist();
        }

        public void Index(string id, string rawText)
        {
            if (TextIndexLoader.TrySplit(rawText, out List<string> list))
            {
                Index(id, list);
            }
        }

        internal void Index(Book book, Chapter chapter, Verse verse, VerseText text)
        {
            IndexText(verse.Id, text.Text);
        }

        public void IndexText(string id, string text)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                string s = Substitute(text);
                if (!String.IsNullOrWhiteSpace(s))
                {
                    var tokens = Tokenize(s);
                    int j = 0;
                    foreach (var token in tokens)
                    {
                        j++;
                        Index(id, token, 0, j);
                        if (!Profiler.ContainsKey(token))
                        {
                            Profiler.Add(token.ToLower(), 0);
                        }
                        Profiler[token]++;
                    }
                }

            }
        }

        public void Index(string id, List<string> lines)
        {
            for (int i = 1; i <= lines.Count; i++)
            {
                var line = lines[i - 1];
                if (!String.IsNullOrWhiteSpace(line))
                {
                    string s = Substitute(line);
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        var tokens = Tokenize(s);
                        int j = 0;
                        foreach (var token in tokens)
                        {
                            j++;
                            Index(id, token, i, j);
                            if (!Profiler.ContainsKey(token))
                            {
                                Profiler.Add(token.ToLower(), 0);
                            }
                            Profiler[token]++;
                        }
                    }

                }
            }
        }

        internal void Index(string id, FileInfo info)
        {
            List<string> lines = new List<string>();
            switch (info.Extension)
            {
                case ".txt":
                    if (TextIndexLoader.TryReadSplit(info, out lines))
                    {
                        Index(id, lines);
                    }
                    break;
                default:
                    break;
            }
        }

        private string Substitute(string line)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in line.ToCharArray())
            {
                if (Whitelist.Contains(c))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(" ");
                }
                //if (Char.IsLetterOrDigit(c))
                //{
                //    sb.Append(c);
                //}
                //else
                //{
                //    sb.Append(" ");
                //}
            }

            return sb.ToString().Trim(new char[] { '\\','/',';','.','@' });
        }


        private List<string> Tokenize(string line)
        {
            List<string> list = new List<string>();
            
            string[] parts = line.Split(new char[] { ' ', ',','\t' }, StringSplitOptions.RemoveEmptyEntries);
            list = new List<string>(parts);
            //foreach (var part in parts)
            //{
            //    string token = Scrub(part);
            //    if (!Exclusions.Contains(token.ToLower()) && token.Length > 3)
            //    {
            //        list.Add(token);
            //    }
            //}
            return list;
        }

        private string Scrub(string part)
        {

            return part;
            //StringBuilder sb = new StringBuilder();
            //foreach (Char c in part.ToCharArray())
            //{
            //    if (Whitelist.Contains(c))
            //    {
            //        sb.Append(c);
            //    }
            //    //if (Char.IsSeparator(c) | Char.IsLetterOrDigit(c))
            //    //{
            //    //    sb.Append(c);
            //    //}
            //}
            //return sb.ToString();
        }

        internal IEnumerable<string> Query(List<string> tokens)
        {
            HashSet<string> hs = new HashSet<string>();
            List<string> list = new List<string>();
            if (tokens != null && tokens.Count > 0)
            {
                var token = tokens[0].Trim().ToLower();
                if (Indexes.Contains(token))
                {
                    TextIndex index = Indexes[token];
                    foreach (Token item in index.Tokens)
                    {
                        foreach (TokenLocation location in item.Locations)
                        {
                            if (hs.Add(location.Id))
                            {
                                list.Add(location.Id);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
