using System.Collections.Generic;
using System.IO;

namespace Bitsmith.Indexing
{
    public interface IFileReader
    {
        string Extension { get; }
        bool TryReadSplit(FileInfo info, out List<string> lines);
    }
}
