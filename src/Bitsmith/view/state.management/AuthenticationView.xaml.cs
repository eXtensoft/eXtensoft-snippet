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
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView : UserControl
    {
        System.Timers.Timer _Timer = null;
        public AuthenticationView()
        {
            InitializeComponent();
            _Timer = new System.Timers.Timer(1500.00);
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Stop();
            Dispatcher.Invoke((Action)(() =>
            {                
                Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.Authorize.ToString());
            }));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Workspace.Instance.Settings.IsAuthenticate)
            {
                _Timer.Enabled = true;
                _Timer.Start();
            }
            
        }
    }
}
