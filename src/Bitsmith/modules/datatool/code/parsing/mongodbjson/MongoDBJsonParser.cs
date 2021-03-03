using Bitsmith.Models;
using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing.MongoDB
{
    public class MongoDBJsonParser : Parser
    {
        public override string Extension => ".mjson";

        public FieldParserFactory Factory { get; set; }


        public override void Parse(TabularData tabularData)
        {
            List<string> lines = new List<string>();
            if (tabularData.Info != null)
            {
                lines = tabularData.Info.AllLines();
            }
            else if (!string.IsNullOrWhiteSpace(tabularData.Body))
            {
                lines = new List<string>(tabularData.Body.AllLines());
            }

            FieldParserFactory factory = new FieldParserFactory();
            if (SetupDataTable(factory, tabularData, lines, out FieldParserCollection parsers))
            {
                Execute(parsers, lines, tabularData);
                tabularData.IsOkay = true;
            }
            else
            {
                tabularData.IsOkay = false;
            }
        }

        private static void Execute(FieldParserCollection parsers, List<string> lines, TabularData tabularData)
        {
            int index = 0;
            DataRow row = tabularData.Data.NewRow();

            bool isArray = false;
            var sb = new StringBuilder();
            string arrayKey = string.Empty;

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string s = line.Trim();
                    if (isArray)
                    {
                        sb.Append(s);
                        if (s[0].Equals(']'))
                        {
                            var data = sb.ToString();
                            if (parsers.Contains(arrayKey))
                            {
                                parsers[arrayKey].Parse(data.TrimEnd(','), row);
                            }
                            sb = new StringBuilder();
                            arrayKey = string.Empty;
                            isArray = false;
                        }
                    }
                    else
                    {
                        if (s.EndsWith("["))
                        {
                            isArray = true;
                            sb.Append("[");
                            int pos = line.IndexOf(':');
                            arrayKey = line.Substring(0, pos).Trim().Trim('"');
                        }
                        else
                        {
                            if (line.StartsWith("/*"))
                            {
                                var parts = line.Trim().Split(new char[] { ' ' });
                                if (parts.Length == 3 && Int32.TryParse(parts[1], out int i))
                                {
                                    index = i;
                                }
                            }
                            else if(line.Equals("{"))
                            {
                                row = tabularData.Data.NewRow();
                                row[0] = index;
                            }
                            else if (line.Equals("}"))
                            {
                                tabularData.Data.Rows.Add(row);
                            }
                            else
                            {
                                int pos = line.IndexOf(':');
                                if (pos > 0)
                                {
                                    string key = line.Substring(0, pos).Trim().Trim('"');
                                    if (s.EndsWith("["))
                                    {
                                        isArray = true;
                                        arrayKey = key;
                                        sb.Append(s);
                                    }
                                    else
                                    {
                                        string data = line.Substring(pos + 1).Trim().TrimEnd('"');
                                        if (parsers.Contains(key))
                                        {
                                            parsers[key].Parse(data.TrimEnd(',').Trim('"'), row);
                                        }
                                    }
                                }


                            }
                        }
                    }


                }
            }


        }

        private static bool SetupDataTable(FieldParserFactory factory, TabularData tabularData, List<string> lines, out FieldParserCollection parsers)
        {
            parsers = new FieldParserCollection();
            tabularData.Data = new DataTable();

            bool b = false;
            int index = 0;
            bool isArray = false;
            string arrayKey = string.Empty;
            tabularData.Data.Columns.Add(new DataColumn("seq", typeof(Int32)));
            for (int i = 0; !b && i < lines.Count; i++)
            {
                string line = lines[i];
                string s = line.Trim();
                if (isArray)
                {
                    if (line.EndsWith("],") || line.EndsWith("]"))
                    {
                        var parser = factory.Create(index, arrayKey, "array");
                        tabularData.Data.Columns.Add(parser.ToDataColumn());
                        tabularData.Fields.Add(parser.ToField(index));
                        parsers.Add(parser);
                        isArray = false;
                        arrayKey = string.Empty;
                    }
                }
                else
                {
                    if (line.Equals("}"))
                    {
                        b = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("/*") && !line.Equals("{"))
                    {
                        index++;
                        int pos = line.IndexOf(':');
                        if (pos > 0)
                        {
                            string key = line.Substring(0, pos).Trim().Trim('"');
                            if(key.Equals("_id"))
                            {
                                var parser = new ObjectIdFieldParser() { Key = key, Position = index };
                                tabularData.Data.Columns.Add(parser.ToDataColumn());
                                tabularData.Fields.Add(parser.ToField(index));
                                parsers.Add(parser);
                            }
                            else if (!s.EndsWith("["))
                            {
                                string data = line.Substring(pos + 1).Trim().TrimEnd(',').Trim('"');
                                var parser = factory.Create(index, key, data);
                                tabularData.Data.Columns.Add(parser.ToDataColumn());
                                tabularData.Fields.Add(parser.ToField(index));
                                parsers.Add(parser);
                            }
                            else
                            {
                                isArray = true;
                                arrayKey = key;
                            }
                        }
                    }
                }            
            }

            return b;
        }
    }
}
