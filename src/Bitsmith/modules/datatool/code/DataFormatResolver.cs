using Bitsmith.Profiling;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public class DataFormatResolver : IDataFormatResolver
    {
        private const string MongoDBJsonPattern = "/\\*(?:.|[\\n\\r])*?\\*/";
        private const char TAB = '\t';
        private const char PIPE = '|';
        private const char COMMA = ',';

        private Dictionary<string, string> _ExtensiomMaps = new Dictionary<string, string>() 
        {
            { "foo", "bar" },
        };
        string IDataFormatResolver.Resolve(FileInfo fileInfo)
        {
            var ext = fileInfo.Extension;
            if (_ExtensiomMaps.ContainsKey(ext))
            {
                return _ExtensiomMaps[ext];
            }
            else
            {
                return Resolve(fileInfo);
            }
        }

        string IDataFormatResolver.Resolve(IEnumerable<string> lines)
        {
            return Resolve(lines.ToList());
        }

        string IDataFormatResolver.Resolve(string text)
        {
            return Resolve(text);
        }

        private string Resolve(FileInfo fileInfo)
        {
            var ext = fileInfo.Extension;
            if (_ExtensiomMaps.ContainsKey(ext))
            {
                return _ExtensiomMaps[ext];
            }
            else if(File.Exists(fileInfo.FullName))
            {
                var lines =  new List<string>(File.ReadAllLines(fileInfo.FullName));
                return Resolve(lines);
            }
            return string.Empty;
        }
        private string Resolve(string text)
        {
            // if xml, json, text {hasHeader:Y/N, delimiter, delimiter-per-line}, isFixed
            if (text.StartsWith("{") && text.EndsWith("}") || text.StartsWith("[") && text.EndsWith("}"))
            {
                // look for pattern /*n*/
                if (text.Contains("IsoDate") || text.Contains("/*"))
                {
                    return ".mjson";
                }
                else
                {
                    return ".json";
                }
                
            }
            else if(text.StartsWith("<") && text.EndsWith("/>"))
            {
                return ".xml";
            }
            else
            {
                var lines = new List<string>(text.Split(new char[] { '\r','\n' },StringSplitOptions.RemoveEmptyEntries));
                return Resolve(lines);
            }
        }

        private string Resolve(List<string> lines)
        {
            // we now assume a delimited format, with/without header, and we must determine the delimiter (if any)
            int min = 1;
            int max = lines.Count() > 1000 ? 1000 : lines.Count();

            Dictionary<char, int> delimiters = new Dictionary<char, int>() 
            {
                { COMMA, 0 },
                { TAB, 0 },
                { PIPE, 0 },
            };
            Dictionary<int, int> columncounts = new Dictionary<int, int>();
            for (int i = min; i < max; i++)
            {
                var line = lines[i].Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    int count = 0;
                    foreach (var c in line.ToCharArray())
                    {
                        if (delimiters.ContainsKey(c))
                        {
                            delimiters[c]++;
                            count++;
                        }
                    }
                    if (!columncounts.ContainsKey(count))
                    {
                        columncounts.Add(count,0);
                    }
                    columncounts[count]++;
                }

            }
            char resolved = ResolveDelimiter(delimiters);
            int columns = ResolveColumnCount(columncounts);

            ProfileFields fields = new ProfileFields();
            var headers = lines[0].Split(new char[] { resolved });
            foreach (var header in headers)
            {
                fields.Add(new ProfileField(header,header));
            }
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var data = line.Split(new char[] { resolved }) ;
                for (int j = 1; j < data.Length; j++)
                {
                    if (j < columns)
                    {
                        fields.Profile(headers[j], data[j]);
                    }                    
                }

            }
            var ts = fields.ToString();
            //DataTable dataTable = new DataTable();
            //var headers = lines[0].Split(new char[] { resolved });
            //if (headers.Length >= columns)
            //{
            //    foreach (var header in headers)
            //    {
            //        dataTable.Columns.Add(new DataColumn(header, typeof(string)));

            //    }
            //}
            //for (int j = 1; j < lines.Count; j++)
            //{
            //    var data = lines[j].Split(new char[] { resolved });
            //    var row = dataTable.NewRow();
            //    for (int  i = 0;  i <data.Length;  i++)
            //    {
            //        if (i < dataTable.Columns.Count)
            //        {
            //            row[i] = data[i];
            //        }

            //    }
            //    dataTable.Rows.Add(row);
            //}

            return string.Empty;
        }

        private char ResolveDelimiter(Dictionary<char, int> delimiterFrequencies)
        {
            int max = 0;
            char delimiter = (char)0;
            foreach (var item in delimiterFrequencies)
            {
                if (item.Value > max)
                {
                    max = item.Value;
                    delimiter = item.Key;
                }
            }

            return delimiter;
          
        }

        private int ResolveColumnCount(Dictionary<int, int> columnCounts)
        {
            int max = 0;
            int count = 0;
            foreach (var item in columnCounts)
            {
                if (item.Value > max)
                {
                    count = item.Key;
                }
            }
            return count + 1;

        }


    }
}
