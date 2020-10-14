using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace Bitsmith
{
    public static class AppExtensions
    {

        public static string Guids(this int max)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < max; i++)
            {
                sb.AppendLine(Guid.NewGuid().ToString().ToLower());
            }

            return sb.ToString();
        }
        public static void EnsureDirectories(this IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                DirectoryInfo info = new DirectoryInfo(item);
                if (!info.Exists)
                {
                    info.Create();
                }
            }
        }

        public static FlowDocument ToFlowDocument(this string text, IEnumerable<string> terms, Brush background, bool IsCaseSensitive = false)
        {
            StringComparison comparison = IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            string body = text;
            string term;
            int pos = body.IndexOfNext(terms, comparison, out term);
            do
            {
                if (pos == 0)
                {
                    p.Inlines.Add(new Run(body.Substring(0, term.Length)) { Background = background });
                    body = body.Substring(term.Length);
                }
                else if(pos > 0)
                {
                    p.Inlines.Add(new Run(body.Substring(0, pos)));
                    body = body.Substring(pos);
                }
                pos = body.IndexOfNext(terms, comparison, out term);
            } while (pos > -1);
            p.Inlines.Add(new Run(body));
            doc.Blocks.Add(p);
            return doc;
        }
        public static FlowDocument ToFlowDocument(this string text, string term, Brush background, bool IsCaseSensitive = false)
        {
            StringComparison comparison = IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            string body = text;
            int pos = body.IndexOf(term,comparison);
            int len = term.Length;
            do
            {
                if (pos == 0)
                {
                    p.Inlines.Add(new Run(body.Substring(0, len)) { Background = background });
                    body = body.Substring(len);
                }
                else if (pos > 0)
                {
                    p.Inlines.Add(new Run(body.Substring(0, pos)));
                    body = body.Substring(pos);
                }
                pos = body.IndexOf(term, comparison);
            } while (pos > -1);
            p.Inlines.Add(new Run(body));
            doc.Blocks.Add(p);

            return doc;
        }

        private static int IndexOfNext(this string body, IEnumerable<string> terms, StringComparison comparison, out string nextTerm)
        {
            int pos = -1;
            nextTerm = string.Empty;
            foreach (var term in terms)
            {
                int x = body.IndexOf(term, comparison);
                if (x > -1)
                {
                    if (pos == -1 || x < pos)
                    {
                        pos = x;
                        nextTerm = term;
                    }
                }
            }
            return pos;
        }

        public static string ToText(this TimeSpan timespan, string format = @"dd\:hh\:mm")
        {

            if (string.IsNullOrWhiteSpace(format))
            {
                StringBuilder sb = new StringBuilder();
                TimeSpan ts = TimeSpan.FromMinutes(timespan.TotalMinutes);
                if (ts.TotalHours > 24)
                {
                    var days = Math.Floor(ts.TotalHours / 24);
                    ts = ts.Subtract(TimeSpan.FromDays(days));
                    sb.Append($"{Math.Floor(days)}d");

                }
                if (ts.Hours >= 1)
                {
                    var hours = Math.Floor(ts.TotalHours);
                    ts = ts.Subtract(TimeSpan.FromHours(hours));
                    sb.Append($"{hours}h");
                
                }
                if(ts.TotalMinutes > 0 && ts.TotalMinutes < 60)
                {
                    sb.Append($"{ts.TotalMinutes}m");
                }
                return sb.ToString();
            }
            else
            {
                return timespan.ToString(format);
            }

        }

        public static UserSettings Ensure(this UserSettings model)
        {
            if (string.IsNullOrWhiteSpace(model.Application))
            {
                model.Application = "Bitsmith";
            }
            if (string.IsNullOrWhiteSpace(model.Machine))
            {
                model.Machine = Environment.MachineName;
            }
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                model.Username = Environment.UserName;
            }
            if (model.CreatedAt == DateTime.MinValue)
            {
                model.CreatedAt = DateTime.Now;
            }
            return model;
        }
        public static List<string> SplitTrimLower(this string p)
        {
            List<string> list = new List<string>();
            if (!String.IsNullOrEmpty(p))
            {
                string[] t = p.Split(new char[] {',',' ',';','\r','\n','\t'},StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in t)
                {
                    list.Add(s.Trim().ToLower());
                }
            }
            return list;
        }

        public static string Delimit(this List<string> list, string delimiter)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(String.Format("{0} ",delimiter));
                }
                sb.Append(list[i]);
            }

            return sb.ToString();
        }

        public static string Expand(this string input)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            if (!String.IsNullOrWhiteSpace(input))
            {
                foreach (var c in input.ToCharArray())
                {
                    if (i++ > 0 && Char.IsUpper(c))
                    {
                        sb.Append($"_{c.ToString().ToLower()}");
                    }
                    else
                    {
                        sb.Append(c.ToString().ToLower());
                    }
                }
            }
            return sb.ToString();
        }
        public static string ToToken(this string input)
        {

            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(input))
            {
                var text = input.Trim().Replace(" ", "-").ToCharArray();
                for (int j = 0; j < text.Length; j++)
                {
                    var c = text[j];
                    if (char.IsUpper(c))
                    {
                        if (j == 0)
                        {
                            sb.Append(c.ToString().ToLower());
                        }
                        else
                        {
                            char previous = text[j - 1];
                            if(Char.IsUpper(previous))
                            {
                                sb.Append(c.ToString().ToLower());
                            }
                            else if (previous == '-')
                            {
                                sb.Append(c.ToString().ToLower());
                            }
                            else
                            {
                                sb.Append($"_{c.ToString().ToLower()}");
                            }
                        }
                    }
                    else
                    {
                        sb.Append(c.ToString().ToLower());
                    }
                }
            }
            return sb.ToString();
        }

        public static string UpTo(this string text, int maxlength = 30)
        {
            string output = string.Empty;
            if (!String.IsNullOrWhiteSpace(text))
            {
                output = text.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ').Replace("  ", " ");
                if (output.Length > maxlength)
                {
                    string part = output.Substring(0, maxlength);
                    int pos = part.LastIndexOf(' ');
                    if (pos > 0)
                    {
                        output = part.Substring(0, part.LastIndexOf(' '));
                    }
                }
            }
            return output;
        }

        public static DirectoryInfo EnsureDirectory(this DirectoryInfo info)
        {
            if (!info.Exists)
            {
                info.Create();
            }
            return info;
        }

        public static bool PeerDirectoryExists(this FileInfo info, string directoryName)
        {
            bool b = false;
            DirectoryInfo directoryInfo = info.Directory;
            var found = directoryInfo.GetDirectories().Where(x => x.Name.EndsWith(directoryName, StringComparison.OrdinalIgnoreCase));
            if (found != null && found.Count() == 1)
            {
                b = true;
            }
            return b;
        }

        public static bool EnsurePeerDirectory(this FileInfo info, string directoryName, out DirectoryInfo contentDirectory)
        {

            if (!info.PeerDirectoryExists(directoryName))
            {
                info.Directory.CreateSubdirectory(directoryName);
            }
            contentDirectory = new DirectoryInfo(Path.Combine(info.Directory.FullName, directoryName));
            return true;
        }

        public static int PowerOf(this int bas, int exp)
        {
            return Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b);
        }
        public static Dictionary<int, int[]> ToMaps(this int cnt)
        {
            Dictionary<int, int[]> maps = new Dictionary<int, int[]>();
            int max = 2.PowerOf(cnt);
            for (int i = 1; i < max; i++)
            {
                BitArray b = new BitArray(new int[] { i });
                List<int> list = new List<int>();
                for (int bitIndex = 0; bitIndex < b.Length; bitIndex++)
                {
                    if (b[bitIndex])
                    {
                        list.Add(bitIndex + 1);
                    }
                }
                if (list.Count > 0)
                {
                    maps.Add(i, list.ToArray());
                }
            }
            return maps;
        }

        public static FileInfo ToFileInfo(this DirectoryInfo directoryInfo, FileInfo fileInfo, bool isAppendName = false)
        {
            string filename = !isAppendName ? fileInfo.Name :
                fileInfo.Name.Insert(fileInfo.Name.LastIndexOf('.'), $".{Guid.NewGuid().ToString().Substring(0, 4)}");
            var filepath = Path.Combine(directoryInfo.FullName, filename);
            return new FileInfo(filepath);
        }

        public static FileInfo ToFileInfo(this DirectoryInfo directoryInfo, string filename)
        {
            var filepath = Path.Combine(directoryInfo.FullName, filename);
            return new FileInfo(filepath);
        }

        public static void ToCsv(this DataTable dt, string filepath, bool includeHeaders = true)
        {
            FileInfo info = new FileInfo(filepath);
            if (info.Directory.Exists)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(info.FullName))
                    {
                        dt.ToCsv(writer, includeHeaders);
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    System.Windows.MessageBox.Show(message);
                }
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource,TKey>(this IEnumerable<TSource> source, Func<TSource,TKey> keySelector)
        {
            HashSet<TKey> keys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (keys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }

        private static void ToCsv(this DataTable dt, TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                IEnumerable<string> headers = dt.Columns.OfType<DataColumn>().Select(column => Quote(column.ColumnName));
                writer.WriteLine(string.Join(",", headers));
            }
            IEnumerable<string> list = null;
            foreach (DataRow row in dt.Rows)
            {
                list = row.ItemArray.Select(o => Quote(o?.ToString() ?? string.Empty));
                writer.WriteLine(string.Join(",", list));
            }
        }

        private static string Quote(string data)
        {
            return string.Concat("\"", data.Replace("\"", "\"\""), "\"");
        }

       
    }
}
