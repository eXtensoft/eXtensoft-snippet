using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Bitsmith
{
    public class BooleanToBrushConverter : IValueConverter
    {

        public Brush TrueBrush { get; set; } = Brushes.Green;

        public Brush FalseBrush { get; set; } = Brushes.Red;

        public Brush NotSetBrush { get; set; } = Brushes.Gray;


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = NotSetBrush;
            if (value != null)
            {
                var b = value as bool?;
                if (b != null)
                {
                    if (b.HasValue)
                    {
                        return b.Value ? TrueBrush : FalseBrush;
                    }

                }
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
