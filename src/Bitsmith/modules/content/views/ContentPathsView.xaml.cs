using Bitsmith.Models;
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
    /// Interaction logic for ContentPathsView.xaml
    /// </summary>
    public partial class ContentPathsView : UserControl
    {
        public ContentPathsView()
        {
            InitializeComponent();
            //DataContext = Workspace.Instance.ViewModel.Paths;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var vm = DataContext as WorkspaceViewModel;
            if (vm != null)
            {
                IPathNode selected = e.NewValue as IPathNode;
                if (selected != null)
                {
                    vm.Content.ExecuteQuery(selected);
                }
            }

        }
    }
}
