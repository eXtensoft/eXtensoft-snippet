using System;
using System.Collections.Generic;

namespace Bitsmith.Parsing.MongoDB
{
    public class FieldParserFactory
    {
        Dictionary<string, Func<int, string, string, FieldParser>> _FactoryMaps = new Dictionary<string, Func<int, string, string, FieldParser>>();
        public FieldParserFactory()
        {
            _FactoryMaps.Add("array", (pos, key, data) => { return new JsonArrayFieldParser() { Key = key, Position = pos }; });
            _FactoryMaps.Add("requestbody", (pos, key, data) => { return new JsonFieldParser() { Key = key, Position = pos }; });            
        }

        public FieldParser Create(int position, string key, string data)
        {
            if (_FactoryMaps.ContainsKey(key))
            {
                return _FactoryMaps[key](position,key,data);
            }
            else if(TryResoleParser(position,key,data, out FieldParser parser))
            {
                return parser;
            }
            else
            { 
                return new FieldParser() { Key = key, Position = position };

            }
        }

        private bool TryResoleParser(int position, string key, string data, out FieldParser parser)
        {
            parser = null;

            if (DateTime.TryParse(data, out DateTime date))
            {
                parser = new DateTimeFieldParser() { Key = key, Position = position };
            }
            else if (Int32.TryParse(data, out int i))
            {
                parser = new IntegerFieldParser() { Key = key, Position = position };
            }
            else if (Decimal.TryParse(data, out decimal d))
            {
                parser = new DecimalFieldParser() { Key = key, Position = position };
            }
            else if (Boolean.TryParse(data, out bool b ))
            {
                parser = new BooleanFieldParser() { Key = key, Position = position };
            }
            return parser != null;
        }
    }
}
