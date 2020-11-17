using Bitsmith.FullText;
using Bitsmith.Models;
using Bitsmith.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bitsmith.Indexing
{
    public class Indexer : IContentIndexer
    {
        public Dictionary<string, IFileReader> FileParsers { get; set; } = new Dictionary<string, IFileReader>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, ILanguageIndexer> Languages { get; set; } = new Dictionary<string, ILanguageIndexer>(StringComparer.OrdinalIgnoreCase);

        public DirectoryInfo Directory { get; set; }

        public TextIndexes Indexes { get; set; } = new TextIndexes();
        public int Delta { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsInitialized { get; private set; } = false;


        public Indexer(DirectoryInfo directoryInfo, IEnumerable<IFileReader> fileParsers, IEnumerable<LanguageSettings> languageSettings)
        {
            Directory = directoryInfo;
            HashSet<string> hs = new HashSet<string>();
            foreach (var fileparser in fileParsers)
            {
                if (hs.Add(fileparser.Extension))
                {
                    FileParsers.Add(fileparser.Extension, fileparser);
                }
            }
            HashSet<string> hsSettings = new HashSet<string>();
            foreach (var setting in languageSettings)
            {
                if (hsSettings.Add(setting.Language))
                {
                    Languages.Add(setting.Language, new LanguageIndexer(setting));
                }
            }
        }

        void IContentIndexer.Index(ContentItem contentItem)
        {
            Indexes.Delete(contentItem.Id);
            Index(contentItem);
        }

        void IContentIndexer.Index(IEnumerable<ContentItem> contentItems)
        {
            foreach (var item in contentItems)
            {
                Index(item);
            }
        }

        IEnumerable<string> IContentIndexer.Query(IEnumerable<string> tokens, string language)
        {
            HashSet<string> hs = new HashSet<string>();
            List<string> list = new List<string>();
            if (tokens != null && tokens.Count() > 0)
            {
                foreach (var token in tokens)
                {
                    foreach (var item in Query(token, language))
                    {
                        if (hs.Add(item))
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        IEnumerable<string> IContentIndexer.Query(string token, string language)
        {
            return Query(token, language);
        }
      
        protected IEnumerable<string> Query(string token, string language)
        {
            HashSet<string> hs = new HashSet<string>();
            List<string> list = new List<string>();
            if (Indexes.Contains(token))
            {
                TextIndex index = Indexes[token];
                foreach (FullText.Token item in index.Tokens)
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
            return list;
        }

        protected virtual void Index(ContentItem contentItem)
        {
            List<string> lines = new List<string>();
            if (contentItem.Mime.Equals("text") && TrySplit(contentItem.Body, out lines))
            {
                Index(contentItem, lines);
            }
            else if(FileParsers.ContainsKey(contentItem.Mime))
            {
                FileInfo info = new FileInfo(Path.Combine(Directory.FullName, contentItem.Body));
                if (info.Exists)
                {
                    FileParsers[contentItem.Mime].TryReadSplit(info, out lines);
                }
                Index(contentItem, lines);
            }
        }

        private void Index(ContentItem contentItem, List<string> lines)
        {
            ILanguageIndexer language = Languages.ContainsKey(contentItem.Language) ? Languages[contentItem.Language] : Languages[AppConstants.Languages.English];
            for (int i = 0; i < lines.Count; i++)
            {
                var line = language.Sanitize(lines[i]);
                if (!String.IsNullOrWhiteSpace(line))
                {
                    var tokens = language.Tokenize(line);
                    int pos = 0;
                    foreach (var token in tokens)
                    {
                        pos++;
                        Index(contentItem, token, i + 1, pos);
                    }
                }
            }
        }

        protected void Index(ContentItem contentItem, string token, int lineNumber, int tokenPosition)
        {
            Indexes.Post(token, contentItem.Id, tokenPosition, lineNumber);
        }

        private static bool TrySplit(string content, out List<string> lines)
        {
            bool b = false;
            lines = new List<string>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                lines = new List<string>(content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                if (lines.Count > 0)
                {
                    b = true;
                }
            }
            return b;
        }

    }
}
