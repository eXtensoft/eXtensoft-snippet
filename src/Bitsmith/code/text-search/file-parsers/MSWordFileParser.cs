using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Indexing
{
    public class MSWordFileParser : IFileReader
    {
        string IFileReader.Extension => "docx";

        bool IFileReader.TryReadSplit(FileInfo info, out List<string> lines)
        {
            lines = new List<string>();
            bool b = false;
            /*
            FlowDocumentReader r = new FlowDocumentReader();
            FlowDocument fd = new FlowDocument();
            
            
            try
            {
                using (var doc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(info.FullName, true))
                {
                    var opc = doc.ToFlatOpcString();
                    string body = doc.MainDocumentPart.Document.Body.InnerText;
                    foreach (var paragraph in doc.MainDocumentPart.Document.Body)
                    {
                        fd.Blocks.Add(new Paragraph(new Run(paragraph.InnerText)));

                        var p = paragraph.InnerText;

                        if (!string.IsNullOrWhiteSpace(p))
                        {
                            lines.Add(p);
                        }
                    }
                }
                b = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            */
            return b;
        }
    }
}
