using Bitsmith.Models;
using Bitsmith.ViewModels;
using DocumentFormat.OpenXml.EMMA;
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
    /// Interaction logic for ContentItemWordView.xaml
    /// </summary>
    public partial class ContentItemWordView : UserControl
    {
        public ContentItemWordView()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ContentItemViewModel vm = DataContext as ContentItemViewModel;
            if (vm != null)
            {
                string filepath = System.IO.Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles, vm.Body);
                FileInfo info = new FileInfo(filepath);

                if (info.Exists && info.TryBuildFlowDocument(out FlowDocument flowDocument, vm.SearchTerms))
                {
                    fldViewer.Document = flowDocument;
                }
            }
        }

        private void OpenWord()
        {
            ContentItemViewModel vm = DataContext as ContentItemViewModel;
            if (vm != null)
            {
                string filepath = System.IO.Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles, vm.Body);
                FileInfo info = new FileInfo(filepath);
                if (info.Exists)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(info.FullName);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
