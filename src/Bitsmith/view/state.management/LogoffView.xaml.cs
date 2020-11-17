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
    /// Interaction logic for LogoffView.xaml
    /// </summary>
    public partial class LogoffView : UserControl
    {
        System.Timers.Timer _Timer = null;

        public LogoffView()
        {
            InitializeComponent();
            _Timer = new System.Timers.Timer(500.00);
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            _Timer.Enabled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _Timer.Start();
        }

        void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Stop();

            Dispatcher.Invoke((Action)(() =>
            {
                if (Workspace.Instance.Settings.IsSaveOnClose)
                {
                    Workspace.Instance.ViewModel.Save();
                }
                //WorkspaceViewModel vm = Application.Current.Properties[AppConstants.CURRENTVIEWMODEL] as WorkspaceViewModel;
                //vm.PerformPreSaveCleanup();
                //string s = Application.Current.Properties[AppConstants.LASTOPENFILEDIALOGFOLDERPATH] as string;
                //List<Tuple<string, string>> list = Application.Current.Properties[AppConstants.SETTINGS] as List<Tuple<string, string>>;
                //if (list != null && list.Count > 0)
                //{
                //    var found = list.First(x => x.Item1.Equals(AppConstants.LASTOPENFILEDIALOGFOLDERPATH));
                //    if (found != null)
                //    {
                //        list.Remove(found);
                //    }


                //}
                //list.Add(new Tuple<string, string>(AppConstants.LASTOPENFILEDIALOGFOLDERPATH, s));
                //LocalStorage.SaveSettings(list);
                //GenericObjectManager.WriteGenericItem<Workspace>(_Workspace, _Filepath);
                Application.Current.Shutdown();
            }));


        }
    }
}
