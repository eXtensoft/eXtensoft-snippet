using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Bitsmith
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : WindowBase
    {
        private ICommand _SomeCommand;
        public ICommand SomeCommand
        {
            get
            {
                if (_SomeCommand == null)
                {
                    _SomeCommand = new RelayCommand(
                    param => Some(),
                    param => CanSome());
                }
                return _SomeCommand;
            }
        }
        private bool CanSome()
        {
            return true;
        }
        private void Some()
        {

        }

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
        private bool CanNavigateToCredentials()
        {
            return true;
        }
        private void NavigateToCredentials()
        {

        }


        //private StyxView _Styx;
        //public StyxView Styx
        //{
        //    get 
        //    {
        //        if (_Styx == null)
        //        {
        //            _Styx = new StyxView();
        //        }
        //        return _Styx; 
        //    }
        //}

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

        private ProjectsView _Projects;
        public ProjectsView Projects
        {
            get
            {
                if (_Projects == null)
                {
                    _Projects = new ProjectsView();
                }
                return _Projects;
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

        //private WorkflowBuilderView _Workflow;
        //public WorkflowBuilderView Workflow
        //{
        //    get
        //    {
        //        if (_Workflow == null)
        //        {
        //            _Workflow = new WorkflowBuilderView();
        //        }
        //        return _Workflow;
        //    }
        //}

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

        //private ICommand _ToggleSettingsCommand;
        //public ICommand ToggleSettingsCommand
        //{
        //    get
        //    {
        //        if (_ToggleSettingsCommand == null)
        //        {
        //            _ToggleSettingsCommand = new RelayCommand(
        //                param => ToggleSettings(),
        //                param => CanTransition(TransitionTypeOption.ToggleSettings.ToString()));
        //        }
        //        return _ToggleSettingsCommand;
        //    }
        //}




        //private ICommand _ToggleListsCommand;
        //public ICommand ToggleListsCommand
        //{
        //    get
        //    {
        //        if (_ToggleListsCommand == null)
        //        {
        //            _ToggleListsCommand = new RelayCommand(
        //                param => ToggleLists(),
        //                param => CanTransition(TransitionTypeOption.ToggleLists.ToString()));
        //        }
        //        return _ToggleListsCommand;
        //    }
        //}




        //private ICommand _ToggleProjectsCommand;
        //public ICommand ToggleProjectsCommand
        //{
        //    get
        //    {
        //        if (_ToggleProjectsCommand == null)
        //        {
        //            _ToggleProjectsCommand = new RelayCommand(
        //                param => ToggleProjects(),
        //                param => CanTransition(TransitionTypeOption.ToggleProjects.ToString()));
        //        }
        //        return _ToggleProjectsCommand;
        //    }
        //}

        //private ICommand _ToggleTasksCommand;
        //public ICommand ToggleTasksCommand
        //{
        //    get
        //    {
        //        if (_ToggleTasksCommand == null)
        //        {
        //            _ToggleTasksCommand = new RelayCommand(
        //                param => ToggleTasks(),
        //                param => CanTransition(TransitionTypeOption.ToggleTasks.ToString()));
        //        }
        //        return _ToggleTasksCommand;
        //    }
        //}




        //private ICommand _ToggleStyxCommand;
        //public ICommand ToggleStyxCommand
        //{
        //    get
        //    {
        //        if (_ToggleStyxCommand == null)
        //        {
        //            _ToggleStyxCommand = new RelayCommand(
        //                param => ToggleStyx(),
        //                param => CanTransition(TransitionTypeOption.ToggleStyx.ToString()));
        //        }
        //        return _ToggleStyxCommand;
        //    }
        //}

        //private ICommand _ToggleCredentialsCommand;
        //public ICommand ToggleCredentialsCommand
        //{
        //    get
        //    {
        //        if (_ToggleCredentialsCommand == null)
        //        {
        //            _ToggleCredentialsCommand = new RelayCommand(
        //                param => ToggleCredentials(),
        //                param => CanTransition(TransitionTypeOption.ToggleCredentials.ToString()));
        //        }
        //        return _ToggleCredentialsCommand;
        //    }
        //}




        //private ICommand _ToggleTimeEntryCommand;
        //public ICommand ToggleTimeEntryCommand
        //{
        //    get
        //    {
        //        if (_ToggleTimeEntryCommand == null)
        //        {
        //            _ToggleTimeEntryCommand = new RelayCommand(
        //                param => ToggleTimeEntry(),
        //                param => CanTransition(TransitionTypeOption.ToggleTimeEntry.ToString()));
        //        }
        //        return _ToggleTimeEntryCommand;
        //    }
        //}

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



        //private bool CanTransition(string transitionName)
        //{
        //    bool b = Workspace.Instance.State.CanTransition(transitionName);
        //    return b;
        //}

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

        //private void OnAuthenticated()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(new AuthenticationView());
        //}

        //private void OnAuthorized()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(ContentView);
        //}


        //private void OnUnauthorized()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(new UnauthorizedView());
        //}

        //private void OnError()
        //{

        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(new ErrorView());
        //}

        //private void OnToggleProjects()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(Projects);
        //}

        //private void OnToggleTasks()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(Tasks);
        //}

        //private void OnToggleStyx()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(Styx);
        //}

        //private void OnToggleTimeEntry()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(Chronos);
        //}

        private void OnToggleCredentials()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Credentials);
        }

        //private void ToggleTimeEntry()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleTimeEntry.ToString());
        //}

        //private void ToggleProjects()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleProjects.ToString());
        //}

        //private void ToggleTasks()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleTasks.ToString());
        //}


        //private void ToggleStyx()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleStyx.ToString());
        //}

        //private void ToggleCredentials()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleCredentials.ToString());
        //}

        //private void OnToggleLists()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(Workflow);
        //}

        //private void ToggleSettings()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleSettings.ToString());
        //}

        //private void OnToggleSettings()
        //{
        //    grdRoot.Children.Clear();
        //    grdRoot.Children.Add(Settings);
        //}

        //private void ToggleLists()
        //{
        //    Workspace.Instance.State.ExecuteTransition(TransitionTypeOption.ToggleLists.ToString());
        //}

    }
}
