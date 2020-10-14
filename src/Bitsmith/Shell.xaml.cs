using Bitsmith.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Bitsmith
{

    public partial class Shell : WindowBase
    {
        public ObservableCollection<NavMenuItem> Menu
        {
            get
            {
                return Workspace.Instance.State.Menu;
            }
            set { }
        }

        private ICommand _NavigateToCommand;
        public ICommand NavigateToCommand
        {
            get
            {
                if (_NavigateToCommand == null)
                {
                    _NavigateToCommand = new RelayCommand<string>(NavigateTo, CanNavigateTo); ;
                }
                return _NavigateToCommand;
            }
        }
        private bool CanNavigateTo(string state)
        {
            return Workspace.Instance.State.CanTransitionTo(state);
        }
        private void NavigateTo(string state)
        {
            Workspace.Instance.State.TransitionTo(state);
        }

        private ICommand _NavigateToContentCommand;
        public ICommand NavigateToContentCommand
        {
            get
            {
                if (_NavigateToContentCommand == null)
                {
                    _NavigateToContentCommand = new RelayCommand(param => 
                    { 
                        Workspace.Instance.State.ExecuteTransition("Content"); 
                    }, 
                    param => true);
                }
                return _NavigateToContentCommand;
            }
        }

        private ICommand _NavigateToTasksCommand;
        public ICommand NavigateToTasksCommand
        {
            get
            {
                if (_NavigateToTasksCommand == null)
                {
                    _NavigateToTasksCommand = new RelayCommand(
                    param => { Workspace.Instance.State.ExecuteTransition("Tasks"); },
                    param => true);
                }
                return _NavigateToTasksCommand;
            }
        }

        private ICommand _NavigateToTimeEntryCommand;
        public ICommand NavigateToTimeEntryCommand
        {
            get
            {
                if (_NavigateToTimeEntryCommand == null)
                {
                    _NavigateToTimeEntryCommand = new RelayCommand(
                    param => { Workspace.Instance.State.ExecuteTransition("TimeEntry"); },
                    param => true);
                }
                return _NavigateToTimeEntryCommand;
            }
        }

        private ICommand _NavigateToSettingsCommand;
        public ICommand NavigateToSettingsCommand
        {
            get
            {
                if (_NavigateToSettingsCommand == null)
                {
                    _NavigateToSettingsCommand = new RelayCommand(
                    param => { Workspace.Instance.State.ExecuteTransition("Settings"); },
                    param => true);
                }
                return _NavigateToSettingsCommand;
            }
        }

        private ICommand _NavigateToCredentialsCommand;
        public ICommand NavigateToCredentialsCommand
        {
            get
            {
                if (_NavigateToCredentialsCommand == null)
                {
                    _NavigateToCredentialsCommand = new RelayCommand(
                    param => { Workspace.Instance.State.ExecuteTransition("Credentials"); },
                    param => true);
                }
                return _NavigateToCredentialsCommand;
            }
        }

        private ChronosView _Chronos;
        public ChronosView Chronos
        {
            get
            {
                if (_Chronos == null)
                {
                    _Chronos = new ChronosView();
                }
                return _Chronos;
            }
        }

        private ContentView _ContentView;
        public ContentView ContentView
        {
            get
            {
                if (_ContentView == null)
                {
                    _ContentView = new ContentView();
                }
                return _ContentView;
            }
        }

        private TasksView _Tasks;
        public TasksView Tasks
        {
            get
            {
                if (_Tasks == null)
                {
                    _Tasks = new TasksView();
                }
                return _Tasks;
            }
        }

        private CredentialsView _Credentials;
        public CredentialsView Credentials
        {
            get
            {
                if (_Credentials == null)
                {
                    _Credentials = new CredentialsView();
                }
                return _Credentials;
            }
        }

        private SettingsView _Settings;
        public SettingsView Settings
        {
            get
            {
                if (_Settings == null)
                {
                    _Settings = new SettingsView();
                }
                return _Settings;
            }
        }

        private ICommand _ExitAppCommand;
        public ICommand ExitAppCommand
        {
            get
            {
                if (_ExitAppCommand == null)
                {
                    _ExitAppCommand = new RelayCommand(
                        param => OnLoggedOff());
                }
                return _ExitAppCommand;
            }
        }

        public Shell()
        {
            InitializeComponent();
            WorkspaceViewModel vm = Workspace.Instance.ViewModel;
            vm.Root = this.grdRoot;
            this.grdRoot.DataContext = vm;
            this.brdMenu.DataContext = this;
            InitializeState();

        }

        private void InitializeState()
        {
            var mgr = Workspace.Instance.State;

            mgr.RegisterEndpointAction(ActivityStateOption.Authenticated.ToString(), EndpointOption.Arrival, new Action[] { () =>
                {
                    grdRoot.Children.Clear();
                    grdRoot.Children.Add(new AuthenticationView());
                }
            });

            mgr.RegisterEndpointAction(ActivityStateOption.Content.ToString(), EndpointOption.Arrival, new Action[] { () => 
                {
                    grdRoot.Children.Clear();
                    grdRoot.Children.Add(ContentView);
                }
            });

            mgr.RegisterEndpointAction(ActivityStateOption.Tasks.ToString(), EndpointOption.Arrival, new Action[] { () =>
                {
                    grdRoot.Children.Clear();
                    grdRoot.Children.Add(Tasks);
                }
            });

            mgr.RegisterEndpointAction(ActivityStateOption.TimeEntry.ToString(), EndpointOption.Arrival, new Action[] { () =>
                {
                    grdRoot.Children.Clear();
                    grdRoot.Children.Add(Chronos);
                }
            });

            mgr.RegisterEndpointAction(ActivityStateOption.Settings.ToString(), EndpointOption.Arrival, new Action[] { () =>
                {
                    grdRoot.Children.Clear();
                    grdRoot.Children.Add(Settings);
                }
            });

            mgr.RegisterEndpointAction(ActivityStateOption.Credentials.ToString(), EndpointOption.Arrival, new Action[] { () =>
                {
                    grdRoot.Children.Clear();
                    grdRoot.Children.Add(Credentials);
                }
            });


            mgr.RegisterEndpointAction(ActivityStateOption.LoggedOff.ToString(), EndpointOption.Arrival, OnLoggedOff);

            this.InputBindings.Add(new KeyBinding(NavigateToCredentialsCommand, new KeyGesture(Key.P, ModifierKeys.Alt)));
            this.InputBindings.Add(new KeyBinding(NavigateToContentCommand, new KeyGesture(Key.C, ModifierKeys.Alt)));
            this.InputBindings.Add(new KeyBinding(NavigateToTasksCommand, new KeyGesture(Key.T, ModifierKeys.Alt)));
            this.InputBindings.Add(new KeyBinding(ExitAppCommand, new KeyGesture(Key.X, ModifierKeys.Alt)));
            this.InputBindings.Add(new KeyBinding(NavigateToSettingsCommand, new KeyGesture(Key.X, ModifierKeys.Alt)));

        }

        private void WindowBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mgr = Workspace.Instance.State;
            if (!mgr.CurrentState.Equals(ActivityStateOption.LoggedOff.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;
                mgr.Machine.ExecuteTransition(TransitionTypeOption.Logoff.ToString());
            }
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.Login.ToString());
        }

        private void OnLoggedOff()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(new LogoffView());
        }

    }
}
