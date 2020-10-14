using Bitsmith.Styx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bitsmith
{
    public class PathsConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = String.Empty;
            List<string> input = value as List<string>;
            if (input != null)
            {
                HashSet<string> hs = new HashSet<string>();
                List<string> list = new List<string>();
                input.ForEach(item => {
                    var path = (!item.StartsWith("/") ? $"/{item}" : item).TrimEnd(new char[] { '/' });
                    if (hs.Add(path.Trim()))
                    {
                        list.Add(Cleanse(path));
                    }
                });
                s = string.Join(", ", list);
            }
            if (string.IsNullOrEmpty(s))
            {
                s = $"/content/{Environment.UserName}";
            }

            return s;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> list = new List<string>();

            if (value != null)
            {
                string s = value.ToString().Trim().Trim(',');
                if (!String.IsNullOrEmpty(s))
                {
                    HashSet<string> hs = new HashSet<string>();
                    string[] t = s.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in t)
                    {
                        var path = (!item.StartsWith("/") ? $"/{item}" : item).TrimEnd(new char[] { '/' });
                        if (hs.Add(path.Trim()))
                        {
                            list.Add(Cleanse(path));
                        }
                    }
                }
            }
            return list;
        }

        private static string Cleanse(string input)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(input))
            {
                foreach (char c in input.Replace("//","/").Trim().ToCharArray())
                {
                    if (whitelist.Contains(c))
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }


        private static IList<char> whitelist = new List<char>() 
        { 
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            '1','2','3','4','5','6','7','8','9','0','/','-'
        };

    }
}
