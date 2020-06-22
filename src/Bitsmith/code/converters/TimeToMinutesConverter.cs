using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bitsmith
{
    public class TimeToMinutesConverter : IValueConverter
    {
        public string Format { get; set; }
        public int DefaultMinutes { get; set; } = 30;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Int32.TryParse(value.ToString(), out int minutes))
            {
                TimeSpan ts = TimeSpan.FromMinutes(minutes);
                return !string.IsNullOrWhiteSpace(Format) ? ts.ToText(Format) : ts.ToText();
            }
            return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int minutes = 30;
            if (value != null && !String.IsNullOrWhiteSpace(value.ToString()))
            {
                TimeSpan ts;
                string s = value.ToString().Trim();
                string[] parts = s.Split(new char[] { ':' });
                if (parts.Length == 1)
                {
                    if (Double.TryParse(parts[0], out double d))
                    {
                        if (d <= 12)
                        {
                            ts = TimeSpan.FromHours(d);
                        }
                        else
                        {
                            ts = TimeSpan.FromMinutes(Math.Floor(d));
                        }
                        minutes = (int)ts.TotalMinutes;
                    }
                    else if (TimeSpan.TryParse(parts[0], out ts))
                    {
                        minutes = (int)ts.TotalMinutes;
                    }
                }
                else if (parts.Length == 2)
                {
                    if (Int32.TryParse(parts[0], out int hours) &&
                        Int32.TryParse(parts[1], out int min))
                    {
                        int total = (hours * 60) + min;
                        ts = TimeSpan.FromMinutes(total);
                        minutes = (int)ts.TotalMinutes;
                    }
                }
            }
            return minutes;
        }
    }
}
