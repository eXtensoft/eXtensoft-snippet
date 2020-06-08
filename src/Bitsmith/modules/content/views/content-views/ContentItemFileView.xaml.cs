using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bitsmith
{
    /// <summary>
    /// Interaction logic for ContentItemFileView.xaml
    /// </summary>
    public partial class ContentItemFileView : UserControl
    {
        public ContentItemFileView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string s = String.Empty;
            ContentItemViewModel viewmodel = DataContext as ContentItemViewModel;
            if (viewmodel != null)
            {
                string directory = Application.Current.Properties[AppConstants.ContentDirectory] as string;
                if (!String.IsNullOrEmpty(directory))
                {
                    string filepath = System.IO.Path.Combine(directory, viewmodel.Body);
                    if (System.IO.File.Exists(filepath))
                    {
                        
                        try
                        {
                            System.Diagnostics.Process.Start(filepath);
                        }
                        catch (Exception ex)
                        {
                            string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                            MessageBox.Show(message);
                        }

                    }
                }
            }
        }
    }
}
