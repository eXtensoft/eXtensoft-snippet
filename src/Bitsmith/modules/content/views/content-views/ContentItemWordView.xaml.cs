using Bitsmith.Models;
using Bitsmith.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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
