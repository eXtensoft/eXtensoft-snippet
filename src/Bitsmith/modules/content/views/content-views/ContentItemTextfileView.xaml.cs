using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ContentItemTextfileView.xaml
    /// </summary>
    public partial class ContentItemTextfileView : UserControl
    {
        public ContentItemTextfileView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            string s = String.Empty;
            ContentItemViewModel viewmodel = DataContext as ContentItemViewModel;
            if (viewmodel != null)
            {

                string filepath = System.IO.Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles, viewmodel.Body);
                if (System.IO.File.Exists(filepath))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(filepath))
                    {
                        s = reader.ReadToEnd();
                    }
                }
                if (String.IsNullOrWhiteSpace(viewmodel.SearchTerm))
                {
                    txbBody.Text = s;
                }
                else
                {
                    rtbBody.Document = s.ToFlowDocument(viewmodel.SearchTerm, Brushes.Yellow);
                }

            }
        }

        private void OpenRead()
        {

            ContentItemViewModel viewmodel = DataContext as ContentItemViewModel;
            if (viewmodel != null)
            {

                string filepath = System.IO.Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles, viewmodel.Body);
                if (System.IO.File.Exists(filepath))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start("notepad++.exe", filepath);
                        }
                        catch
                        {

                            try
                            {
                                System.Diagnostics.Process.Start("notepad.exe", filepath);
                            }
                            catch
                            {
                            }
                        }

                    }

            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenRead();
        }

    }
}
