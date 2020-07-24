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
    /// Interaction logic for WorkEffortsView.xaml
    /// </summary>
    public partial class WorkEffortsView : UserControl
    {
        public Grid Workeffort 
        {
            get { return this.grdWorkeffort; }
            set
            {
                grdWorkeffort = value;
            }
        }
        public WorkEffortsView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
