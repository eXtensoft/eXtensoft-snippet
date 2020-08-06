using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Bitsmith
{
    public class NavMenuItemStyleConverter : IValueConverter
    {
        public Style Default { get; set; }
        public Style CurrentState { get; set; }

        public Style CanNavigateTo { get; set; }

        public Style CannotNavigateTo { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style style = null;
            var item = value as NavMenuItem;
            if (item != null)
            {
                if (item.IsCurrent)
                {
                    style = CurrentState;
                }
                else if (item.CanNavigateTo)
                {
                    style = CanNavigateTo;
                }
                else
                {
                    style = CannotNavigateTo;
                }
            }
            if (style == null)
            {
                style = Default;
            }

            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
