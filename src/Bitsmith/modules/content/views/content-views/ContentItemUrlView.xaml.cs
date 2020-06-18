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
    /// Interaction logic for ContentItemUrlView.xaml
    /// </summary>
    public partial class ContentItemUrlView : UserControl
    {
        public ContentItemUrlView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ContentItemViewModel viewmodel = DataContext as ContentItemViewModel;
            if (viewmodel != null)
            {
                if (!string.IsNullOrWhiteSpace(viewmodel.Body))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(viewmodel.Body);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


            }
        }
    }
}
