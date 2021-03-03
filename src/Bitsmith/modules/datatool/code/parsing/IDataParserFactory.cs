using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public interface IDataParserFactory
    {
        IParser Create(FileInfo fileInfo);
        IParser Create(string body);
    }
}
