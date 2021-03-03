using Bitsmith.Models;
using Bitsmith.Parsing.MongoDB;
using System;
using System.IO;

namespace Bitsmith.Parsing
{
    public class DataParser : IDataParser
    {
        private readonly IDataParserFactory _Factory;
        public DataParser(IDataParserFactory factory)
        {
            _Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        bool IDataParser.TryParse(FileInfo info, out TabularData tabularData)
        {
            tabularData = new TabularData().Default(info);
            IParser parser = _Factory.Create(info);
            parser.Parse(tabularData);
            return tabularData.IsOkay;
        }

        bool IDataParser.TryParse(string body, out TabularData tabularData)
        {
            tabularData = new TabularData().Default(body);
            IParser parser = _Factory.Create(body);
            parser.Parse(tabularData);
            return tabularData.IsOkay;
        }




    }
}
