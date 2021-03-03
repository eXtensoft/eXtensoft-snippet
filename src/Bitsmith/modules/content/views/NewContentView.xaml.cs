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
    /// Interaction logic for NewContentView.xaml
    /// </summary>
    public partial class NewContentView : UserControl
    {
        public NewContentView()
        {
            InitializeComponent();
        }


        private void ItemsControl_Checked(object sender, RoutedEventArgs e)
        {
            var rdo = e.OriginalSource as RadioButton;
            if (rdo != null && rdo.IsChecked.HasValue && rdo.IsChecked.Value && rdo.Tag != null)
            {
                var vm = rdo.Tag as SchemaBuilderViewModel;
                if (vm != null)
                {
                    Workspace.Instance.ViewModel.Content.Input.EnsureSelectedSchema(vm);
                }
            }
        }
    }
}
