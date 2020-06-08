using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Bitsmith
{
    public class DateTimeConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String s = value.ToString();
            if (value is DateTime)
            {
                DateTime now = DateTime.Now;
                DateTime target = (DateTime)value;
                TimeSpan ts = new TimeSpan(now.Ticks - target.Ticks);
                if (ts.TotalHours < 24)
                {
                    if (ts.TotalHours < 1)
                    {
                        s = String.Format("{0} minutes ago", ts.Minutes);
                    }
                    else
                    {
                        s = String.Format("{0} hours ago", ts.Hours);
                    }
                }
                else if (now.Date.Equals(target.Date))
                {
                    s = "Today";
                }
                else if (now.AddDays(-1).Date.Equals(target.Date))
                {
                    s = "Yesterday";
                }
                else
                {
                    s = target.ToString("yyyy-MM-dd");
                }
            }
            return s;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
