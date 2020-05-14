using System;
using System.Globalization;
using System.Windows.Data;

namespace Bitsmith
{
    public class BooleanToStringConverter : IValueConverter
    {
        public string TrueText { get; set; } = "true";
        public string FalseText { get; set; } = "false";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && Boolean.TryParse(value.ToString(), out bool isTrue))
            {
                return isTrue ? TrueText : FalseText;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
