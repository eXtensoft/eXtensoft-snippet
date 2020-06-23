using Bitsmith.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

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
