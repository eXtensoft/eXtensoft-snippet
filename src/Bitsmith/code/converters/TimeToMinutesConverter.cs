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
            TimeSpan ts = TimeSpan.FromMinutes(30);
            if (value != null && !String.IsNullOrWhiteSpace(value.ToString()))
            {
                string s = value.ToString().Trim();
                string[] parts = s.Split(new char[] { ':' });
                if (parts.Length == 1)
                {
                    var part = parts[0];
                    if (Double.TryParse(part, out double d))
                    {
                        if (d <= 8)
                        {
                            ts = TimeSpan.FromHours(d);
                        }
                        else
                        {
                            ts = TimeSpan.FromMinutes(Math.Floor(d));
                        }
                    }
                    else
                    {
                        var last = part[part.Length - 1];
                        if (!Char.IsNumber(last))
                        {
                            var stripped = part.Substring(0,part.Length-1);
                            if (Double.TryParse(stripped, 
                                out double time))
                            {
                                if (last.ToString().Equals("h", StringComparison.OrdinalIgnoreCase))
                                {
                                    ts = TimeSpan.FromHours(time);
                                }
                                else if (last.ToString().Equals("m", StringComparison.OrdinalIgnoreCase))
                                {
                                    ts = TimeSpan.FromMinutes(Math.Floor(time));
                                }
                            }
                        }
                    }
                }
                else if (parts.Length == 2)
                {
                    if (Int32.TryParse(parts[0], out int hours) &&
                        Int32.TryParse(parts[1], out int min))
                    {
                        int total = (hours * 60) + min;
                        ts = TimeSpan.FromMinutes(total);
                    }
                }
            }
            minutes = (int)ts.TotalMinutes;

            return minutes;
        }
    }
}
