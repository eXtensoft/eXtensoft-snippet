using Bitsmith.FullText;
using Bitsmith.Models;
using Bitsmith.Styx;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bitsmith.Indexing
{
    public class Indexer : IContentIndexer
    {
        public DirectoryInfo Directory { get; set; }
        
        public TextIndexes Indexes { get; set; } = new TextIndexes();
        public List<string> Exclusions { get; set; } = new List<string>();
        public List<char> Whitelist { get; set; } = new List<char>();

        public int Delta { get; set; }
        public bool IsInitialized { get;  set; }


        int IContentIndexer.Delta { get => Delta; set { Delta = value; } }

        bool IContentIndexer.IsInitialized => IsInitialized;

        void IContentIndexer.Index(ContentItem contentItem)
        {
            Index(contentItem,true);
        }

        void IContentIndexer.Index(IEnumerable<ContentItem> items)
        {
            foreach (var item in items)
            {
                Index(item, false);
            }
        }

        IEnumerable<string> IContentIndexer.Query(IEnumerable<string> tokens, string language)
        {
            return Query(tokens, language);
        }

        IEnumerable<string> IContentIndexer.Query(string token, string language)
        {
            return Query(token, language);
        }


        protected virtual IEnumerable<string> Query(IEnumerable<string> tokens, string language)
        {
            HashSet<string> hs = new HashSet<string>();
            List<string> list = new List<string>();
            if (tokens != null && tokens.Count() > 0)
            {
                foreach (var token in tokens)
                {
                    foreach (var item in Query(token,language))
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

        protected virtual IEnumerable<string> Query(string token, string language)
        {
            HashSet<string> hs = new HashSet<string>();
            List<string> list = new List<string>();
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
            return list;
        }

        protected virtual void Index(ContentItem contentItem, bool isReplace)
        {
            List<string> lines = new List<string>();
            switch (contentItem.Mime)
            {
                case "text":
                    if (TrySplit(contentItem.Body, out lines))
                    {
                        Index(contentItem, lines, isReplace);
                    }
                    break;
                case "txt":
                    var filepath = System.IO.Path.Combine(Directory.FullName, contentItem.Body);
                    FileInfo info = new FileInfo(filepath);
                    if (TryReadSplit(info, out lines))
                    {
                        Index(contentItem, lines, isReplace);
                    }
                    break;
                case "docx":
                    break;
                default:
                    break;
            }

        }

        private void Index(ContentItem contentItem, List<string> lines, bool isReplace = false)
        {
            if (isReplace)
            {
                Indexes.Delete(contentItem.Id);
            }
            
            for (int i = 0; i < lines.Count; i++)
            {
                var line = Sanitize(lines[i]);
                if (!String.IsNullOrWhiteSpace(line))
                {
                    var tokens = Tokenize(line);
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

        protected virtual string Sanitize(string line)
        {
            StringBuilder sb = new StringBuilder(line.Length);
            foreach (char c in line.Trim().ToCharArray())
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


        protected virtual List<string> Tokenize(string line)
        {
            List<string> list = new List<string>();

            string[] parts = line.Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
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

        public static bool TrySplit(string content, out List<string> lines)
        {
            bool b = false;
            lines = new List<string>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                lines = new List<string>(content.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries));
                if (lines.Count > 0)
                {
                    b = true;
                }
            }

            return b;
        }

        private static bool TryReadSplit(string filepath, out List<string> lines)
        {
            return TryReadSplit(new FileInfo(filepath), out lines);
        }

        private static bool TryReadSplit(FileInfo info, out List<string> lines)
        {
            bool b = false;
            lines = new List<string>();
            if (info.Exists)
            {
                try
                {
                    lines = new List<string>(File.ReadAllLines(info.FullName));
                    b = true;
                }
                catch {}
            }
            return b;
        }


        //public static bool TryReadWord(string filepath, out List<string> lines)
        //{
        //    //var sb = new StringBuilder();
        //    FlowDocumentReader r = new FlowDocumentReader();
        //    FlowDocument fd = new FlowDocument();
        //    bool b = false;
        //    lines = new List<string>();
        //    FileInfo info = new FileInfo(filepath);
        //    if (info.Exists)
        //    {
        //        try
        //        {
        //            using (var doc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(info.FullName,true))
        //            {
        //                var opc = doc.ToFlatOpcString();
        //                string body = doc.MainDocumentPart.Document.Body.InnerText;
        //                foreach (var paragraph in doc.MainDocumentPart.Document.Body)
        //                {
        //                    fd.Blocks.Add(new Paragraph(new Run(paragraph.InnerText)));

        //                    var p = paragraph.InnerText;

        //                    if (!string.IsNullOrWhiteSpace(p))
        //                    {
        //                        lines.Add(p);
        //                        //sb.AppendLine(p);
        //                        //sb.AppendLine(paragraph.InnerXml);
        //                    }
        //                    else
        //                    {
        //                        //sb.AppendLine();
        //                    }
        //                }
        //            }
        //            b = true;
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //    }

        //    //var sbs = sb.ToString();
        //    return b;
        //} 


        //public static bool TryReadPdf(string filepath, out List<string> lines)
        //{
        //    bool b = false;
        //    lines = new List<string>();
        //    FileInfo info = new FileInfo(filepath);
        //    if (info.Exists)
        //    {
        //        try
        //        {
        //            var pdftext = PdfTextExtractor.GetText(info.FullName);
        //            //var pdf = PdfReader.Open(info.FullName, PdfDocumentOpenMode.ReadOnly);
        //            //foreach (var page in pdf.Pages)
        //            //{
        //            //    CObject content = ContentReader.ReadContent(page);
        //            //    var pagelines = ExtractText(content);
        //            //    lines.AddRange(pagelines);
        //            //}
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //    }

        //    return b;
        //}

        //private static IEnumerable<string> ExtractText(CObject obj)
        //{
        //    var list = new List<string>();
        //    if (obj is COperator)
        //    {
        //        var op = obj as COperator;
        //        if (op.OpCode.Name == OpCodeName.TJ.ToString() || 
        //            op.OpCode.Name == OpCodeName.Tj.ToString())
        //        {
        //            foreach (var item in op.Operands)
        //            {
        //                list.AddRange(ExtractText(op));
        //            }
        //        }
        //    }
        //    else if(obj is CSequence)
        //    {
        //        foreach (var item in obj as CSequence)
        //        {
        //            list.AddRange(ExtractText(item));
        //        }
        //    }
        //    else if(obj is CString)
        //    {
        //        list.Add((obj as CString).Value);
        //    }
        //    return list;
        //}

    }
}
