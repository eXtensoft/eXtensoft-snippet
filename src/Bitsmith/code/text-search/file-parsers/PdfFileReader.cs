using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Indexing
{
    public class PdfFileReader : IFileReader
    {
        string IFileReader.Extension => "pdf";

        bool IFileReader.TryReadSplit(FileInfo info, out List<string> lines)
        {
            lines = new List<string>();
            bool b = false;

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
            return b;
        }

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
