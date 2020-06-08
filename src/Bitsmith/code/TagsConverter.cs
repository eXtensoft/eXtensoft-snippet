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
    public class TagsConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<string> list = new List<string>();

            if (value != null)
            {
                var tags = value as List<Property>;
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        list.Add(tag.ToString());
                    }
                }
            }
            return string.Join(", ",list);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<string> list = new List<string>();

            if (value != null)
            {
                string s = value.ToString().Trim().Trim(',');
                if (!String.IsNullOrEmpty(s))
                {
                    string[] t = s.Split(',');
                    foreach (var item in t)
                    {
                        list.Add(item.Trim());
                    }
                }
            }
            return list;
        }
    }
}
