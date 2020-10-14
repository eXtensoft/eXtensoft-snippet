using Bitsmith.code.pdf;
using Bitsmith.FullText;
using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Bitsmith
{
    public class TextIndexLoader
    {
        public static void Load(TextIndexer indexer, IEnumerable<ContentItem> contentItems, string directory)
        {
            foreach (var item in contentItems)
            {
                Load(indexer, item, directory);
            }
            indexer.IsInitialized = true;
            indexer.NewIndexCount = 0;
        }

        public static void Load(TextIndexer indexer, ContentItem item, string directory)
        {
            List<string> lines = new List<string>();
            var mime = item.Mime;

            switch (mime)
            {
                case "text":
                    //if (TrySplit(item.Body, out lines))
                    //{
                    //    indexer.Index(item.Id.ToString(), lines);
                    //}
                    break;
                case "txt":

                    //if (TryReadSplit(System.IO.Path.Combine(directory, item.Body), out lines))
                    //{
                    //    indexer.Index(item.Id.ToString(), lines);
                    //}
                    break;
                case "docx":
                    //if (TryReadWord(System.IO.Path.Combine(directory, item.Body), out lines))
                    //{
                    //    indexer.Index(item.Id.ToString(), lines);
                    //}
                    break;
                case "pdf":
                    //if (TryReadPdf(System.IO.Path.Combine(directory,item.Body), out lines))
                    //{
                    //    indexer.Index(item.Id.ToString(), lines);
                    //}
                    break;
                case "text/credential":
                case "resource/url":
                    break;
                default:
                    break;
            }
            if (indexer.IsInitialized)
            {
                indexer.NewIndexCount++;
            }
        }


        public static bool TryReadSplit(FileInfo info, out List<string> lines)
        {
            return TryReadSplit(info.FullName, out lines);
        }

        public static bool TryReadSplit(string filepath, out List<string> lines)
        {
            bool b = false;
            lines = new List<string>();
            if (File.Exists(filepath))
            {
                try
                {
                    lines = new List<string>(File.ReadAllLines(filepath));
                    if (lines.Count > 0)
                    {
                        b = true;
                    }
                }
                catch { }
            }
            return b;
        }
        public static bool TrySplit(string content, out List<string> lines)
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
