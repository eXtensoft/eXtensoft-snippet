using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Bitsmith
{
    public class UriToBitmapConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string foldername = Application.Current.Properties[AppConstants.ContentDirectory] as string;
            if (!String.IsNullOrEmpty(foldername))
            {
                DirectoryInfo directory = new DirectoryInfo(foldername);
                string filepath = System.IO.Path.Combine(directory.FullName, value.ToString());
                BitmapImage image = new BitmapImage();
                if (System.IO.File.Exists(filepath))
                {
                    image.BeginInit();
                    //image.DecodePixelWidth = 512;
                    image.CacheOption = BitmapCacheOption.OnLoad;

                    image.UriSource = new Uri(filepath);
                    image.EndInit();
                }
                return image;
            }
            else
            {
                return value;
            }

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
