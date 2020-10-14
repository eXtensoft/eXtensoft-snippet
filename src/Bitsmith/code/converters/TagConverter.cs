using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace Bitsmith
{
    public class TagConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = String.Empty;
            List<string> list = value as List<string>;
            if (value != null)
            {
                string t = value.GetType().Name;

            }
            if (list != null && list.Count > 1)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < list.Count - 1; i++)
                {
                    sb.Append(String.Format("{0}, ", list[i]));
                }
                sb.Append(list[list.Count - 1]);
                s = sb.ToString();
            }
            else if (list.Count == 1)
            {
                s = list[0];
            }
            return s;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
                        if (hs.Add(item))
                        {
                            list.Add(item.Trim());
                        }
                    }
                }
            }
            return list;
        }
    }
}
