using Bitsmith.Parsing.MongoDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public class DataParserFactory : IDataParserFactory
    {
        private readonly IDataFormatResolver _Resolver;
        private readonly IDictionary<string, IParser> _Parsers = new Dictionary<string, IParser>(StringComparer.OrdinalIgnoreCase);
        
        public DataParserFactory(IDataFormatResolver resolver, IEnumerable<IParser> parsers)
        {
            _Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            HashSet<string> hs = new HashSet<string>();
            foreach (var parser in parsers)
            {
                if (hs.Add(parser.Extensions))
                {
                    _Parsers.Add(parser.Extensions, parser);
                }
            }
        }

        IParser IDataParserFactory.Create(FileInfo fileInfo)
        {
            var key = _Resolver.Resolve(fileInfo);
            if (_Parsers.ContainsKey(key))
            {
                return _Parsers[key];
            }
            return new MongoDBJsonParser();
        }

        IParser IDataParserFactory.Create(string body)
        {
            var key = _Resolver.Resolve(body);
            if (_Parsers.ContainsKey(key))
            {
                return _Parsers[key];
            }
            return null;
        }
    }
}
