using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bitsmith
{
    public class TruncateStringConverter : IValueConverter
    {
        public int MaxLength { get; set; } = 30;
        public string Suffix { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string s = string.Empty;
                int max = MaxLength - (!String.IsNullOrWhiteSpace(Suffix) ? Suffix.Length : 0);                
                var tags = value as List<Property>;
                if (tags != null)
                {
                    var list = new List<string>();
                    foreach (var tag in tags)
                    {
                        list.Add(tag.ToString());
                    }
                    s = string.Join(", ", list);
                }
                else
                {
                    s = value.ToString().Trim();
                }
                
                if (s.Length <= MaxLength)
                {
                    return s;
                }
                else
                {
                    string t = string.Empty;
                    int last = s.Substring(0,max).LastIndexOf(' ');
                    if (last < 1)
                    {
                        t = s.Substring(0, max);
                    }
                    else
                    {
                        t  = s.Substring(0, last).TrimEnd(',');
                        
                    }
                    return $"{t}{Suffix}";

                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
