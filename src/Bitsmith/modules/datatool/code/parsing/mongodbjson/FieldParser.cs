using Bitsmith.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bitsmith.Parsing.MongoDB
{


    public class FieldParserCollection : KeyedCollection<string, FieldParser>
    {
        protected override string GetKeyForItem(FieldParser item)
        {
            return item.Key;
        }
    }

    public class FieldParser
    {
        public int Position { get; set; }
        public string Key { get; set; }
        public virtual void Parse(string data, DataRow row)
        {
            if (Position < row.ItemArray.Length)
            {
                row[Position] = data.ToString();
            }
        }

        public virtual DataField ToField(int position)
        {
            return new DataField() { Name = Key, Display = Key, FieldType = typeof(string).Name, Position = position };
        }

        public virtual DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(string));
        }

        protected void OnError(Exception ex)
        {
            var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            MessageBox.Show(message);
        }
    }

    public class JsonArrayFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            if (Position < row.ItemArray.Length && !string.IsNullOrWhiteSpace(data))
            {
                try
                {
                    var unescaped = data.Replace("\\", "");
                    var json = JsonConvert.DeserializeObject(unescaped);
                    var jsontext = JsonConvert.SerializeObject(json, Formatting.None);
                    row[Position] = jsontext;
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }
        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(string));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(string).Name };
        }
    }


    public class JsonFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            if (Position < row.ItemArray.Length && !string.IsNullOrWhiteSpace(data))
            {
                try
                {
                    var unescaped = data.Replace("\\", "");
                    var json = JsonConvert.DeserializeObject(unescaped);
                    var jsontext = JsonConvert.SerializeObject(json, Formatting.None);
                    row[Position] = jsontext;
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }
        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(string));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(string).Name };
        }
    }

    public class ObjectIdFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            var id = data.Substring(data.IndexOf('"') + 1).TrimEnd(')').TrimEnd('"');
            row[Position] = id;
        }
        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(string));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = "ObjectId"};
        }
    }

    public class DateTimeFieldParser: FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            if (DateTime.TryParse(data, out DateTime datetime))
            {
                row[Position] = datetime;
            }
        }
        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key,typeof(DateTime));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(DateTime).Name };
        }
    }

    public class IsoDateFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            string datestring = data.Substring(data.IndexOf('"') + 1).TrimEnd(')').TrimEnd('"');
            DateTime dte = DateTime.Parse(datestring, null, System.Globalization.DateTimeStyles.RoundtripKind);
            row[Position] = dte.AddHours(-5);
        }

        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(DateTime));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(DateTime).Name };
        }
    }

    public class NumberFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            base.Parse(data, row);
        }
    }

    public class BooleanFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            if (Boolean.TryParse(data, out bool b))
            {
                row[Position] = b;
            }
        }
        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(bool));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(bool).Name };
        }
    }

    public class DecimalFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            if (Decimal.TryParse(data, out decimal d))
            {
                row[Position] = d;
            }
            else
            {
                base.Parse(data, row);
            }
        }

        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(decimal));
        }
        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(decimal).Name };
        }
    }


    public class IntegerFieldParser : FieldParser
    {
        public override void Parse(string data, DataRow row)
        {
            if (Int32.TryParse(data, out Int32 i))
            {
                row[Position] = i;
            }
            else
            {
                base.Parse(data, row);
            }
        }

        public override DataColumn ToDataColumn()
        {
            return new DataColumn(Key, typeof(Int32));
        }

        public override DataField ToField(int position)
        {
            return new DataField() { Display = Key, Name = Key, Position = position, FieldType = typeof(Int32).Name };
        }
    }




}
