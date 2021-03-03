using System.Collections.Generic;
using System.IO;

namespace Bitsmith.Parsing
{
    public interface IDataFormatResolver
    {
        string Resolve(FileInfo fileInfo);
        string Resolve(IEnumerable<string> lines);
        string Resolve(string text);

    }
}
