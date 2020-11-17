using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Indexing
{
    public class TextFileParser : IFileReader
    {
        string IFileReader.Extension => "txt";

        bool IFileReader.TryReadSplit(FileInfo info, out List<string> lines)
        {
            return TryReadSplit(info, out lines);
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
                catch { }
            }
            return b;
        }
    }
}
