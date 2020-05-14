using System;
using System.Globalization;
using System.Windows.Data;

namespace Bitsmith
{
    public class BooleanToIntConverter : IValueConverter
    {
        public int TrueInt { get; set; } = 0;
        public int FalseInt { get; set; } = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Boolean.TryParse(value.ToString(), out bool isTrue))
            {
                return isTrue ? TrueInt : FalseInt;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
