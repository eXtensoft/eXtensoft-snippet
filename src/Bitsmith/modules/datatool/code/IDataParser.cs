using Bitsmith.Models;
using System.IO;

namespace Bitsmith.Parsing
{
    public interface IDataParser
    {
        bool TryParse(FileInfo info, out TabularData tabularData);
        bool TryParse(string body, out TabularData tabularData);
    }
}
