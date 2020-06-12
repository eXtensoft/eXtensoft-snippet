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
            List<Property> list = new List<Property>();

            if (value != null)
            {
                string s = value.ToString().Trim().Trim(',');
                if (!String.IsNullOrEmpty(s))
                {
                    string[] t = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in t)
                    {
                        string[] parts = item.Trim().Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
                        var prop = new Property() { Name = parts[0].Trim() };
                        if (parts.Length == 2)
                        {
                            bool b = false;
                            if (!b && DateTime.TryParse(parts[1], out DateTime dte))
                            {
                                prop.Value = dte.ToShortDateString();
                                b = true;
                            }
                            else if(!b && Decimal.TryParse(parts[1], out decimal dec))
                            {
                                prop.Value = dec;
                                b = true;
                            }
                            else if(!b && Int32.TryParse(parts[1], out int i))
                            {
                                prop.Value = i;
                                b = true;
                            }
                            else
                            {
                                prop.Value = parts[1];
                                b = true;
                            }
                           
                        }
                        list.Add(prop);
                    }
                }
            }
            return list;
        }
    }
}
