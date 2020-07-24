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
using System.Windows.Shapes;

namespace Bitsmith
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : WindowBase
    {
        private StyxView _Styx;
        public StyxView Styx
        {
            get 
            {
                if (_Styx == null)
                {
                    _Styx = new StyxView();
                }
                return _Styx; 
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

        private WorkflowBuilderView _Workflow;
        public WorkflowBuilderView Workflow
        {
            get
            {
                if (_Workflow == null)
                {
                    _Workflow = new WorkflowBuilderView();
                }
                return _Workflow;
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

        private ICommand _ToggleSettingsCommand;
        public ICommand ToggleSettingsCommand
        {
            get
            {
                if (_ToggleSettingsCommand == null)
                {
                    _ToggleSettingsCommand = new RelayCommand(
                    param => ToggleSettings());
                }
                return _ToggleSettingsCommand;
            }
        }

        


        private ICommand _ToggleListsCommand;
        public ICommand ToggleListsCommand
        {
            get
            {
                if (_ToggleListsCommand == null)
                {
                    _ToggleListsCommand = new RelayCommand(
                    param => ToggleLists());
                }
                return _ToggleListsCommand;
            }
        }




        private ICommand _ToggleProjectsCommand;
        public ICommand ToggleProjectsCommand
        {
            get
            {
                if (_ToggleProjectsCommand == null)
                {
                    _ToggleProjectsCommand = new RelayCommand(
                    param => ToggleProjects());
                }
                return _ToggleProjectsCommand;
            }
        }

        private ICommand _ToggleTasksCommand;
        public ICommand ToggleTasksCommand
        {
            get
            {
                if (_ToggleTasksCommand == null)
                {
                    _ToggleTasksCommand = new RelayCommand(
                    param => ToggleTasks());
                }
                return _ToggleTasksCommand;
            }
        }

        private ICommand _ToggleStyxCommand;
        public ICommand ToggleStyxCommand
        {
            get
            {
                if (_ToggleStyxCommand == null)
                {
                    _ToggleStyxCommand = new RelayCommand(
                        param => ToggleStyx());
                }
                return _ToggleStyxCommand;
            }
        }

        private ICommand _ToggleCredentialsCommand;
        public ICommand ToggleCredentialsCommand
        {
            get
            {
                if (_ToggleCredentialsCommand == null)
                {
                    _ToggleCredentialsCommand = new RelayCommand(
                        param => ToggleCredentials());
                }
                return _ToggleCredentialsCommand;
            }
        }

        private ICommand _ToggleTimeEntryCommand;
        public ICommand ToggleTimeEntryCommand
        {
            get
            {
                if (_ToggleTimeEntryCommand == null)
                {
                    _ToggleTimeEntryCommand = new RelayCommand(
                        param => ToggleTimeEntry());
                }
                return _ToggleTimeEntryCommand;
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
            this.DataContext = vm;
            InitializeState();

        }

        private void InitializeState()
        {
            var mgr = Workspace.Instance.State;
            mgr.RegisterEndpointAction(ActivityStateOption.Authenticated.ToString(), EndpointOption.Arrival, OnAuthenticated);
            mgr.RegisterEndpointAction(ActivityStateOption.Authorized.ToString(), EndpointOption.Arrival, OnAuthorized);
            mgr.RegisterEndpointAction(ActivityStateOption.LoggedOff.ToString(), EndpointOption.Arrival, OnLoggedOff);
            mgr.RegisterEndpointAction(ActivityStateOption.Unauthorized.ToString(), EndpointOption.Arrival, OnUnauthorized);
            mgr.RegisterEndpointAction(ActivityStateOption.Error.ToString(), EndpointOption.Arrival, OnError);
            mgr.RegisterEndpointAction(ActivityStateOption.Lists.ToString(), EndpointOption.Arrival, OnToggleLists);
            mgr.RegisterEndpointAction(ActivityStateOption.Settings.ToString(), EndpointOption.Arrival, OnToggleSettings);
            mgr.RegisterEndpointAction(ActivityStateOption.Credentials.ToString(), EndpointOption.Arrival, OnToggleCredentials);
            mgr.RegisterEndpointAction(ActivityStateOption.Projects.ToString(), EndpointOption.Arrival, OnToggleProjects);
            mgr.RegisterEndpointAction(ActivityStateOption.TimeEntry.ToString(), EndpointOption.Arrival, OnToggleTimeEntry);
            mgr.RegisterEndpointAction(ActivityStateOption.Tasks.ToString(), EndpointOption.Arrival, OnToggleTasks);
            mgr.RegisterEndpointAction(ActivityStateOption.Styx.ToString(), EndpointOption.Arrival, OnToggleStyx);

            AddToggleCommands();
        }

        private void AddToggleCommands()
        {
            this.InputBindings.Add(new KeyBinding(ToggleListsCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ToggleSettingsCommand, new KeyGesture(Key.U, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ToggleProjectsCommand, new KeyGesture(Key.P, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ToggleTasksCommand, new KeyGesture(Key.T, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ExitAppCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ToggleCredentialsCommand, new KeyGesture(Key.Y, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ToggleTimeEntryCommand, new KeyGesture(Key.E, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ToggleStyxCommand, new KeyGesture(Key.Q, ModifierKeys.Control)));
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

        private void OnLoggedOff()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(new LogoffView());
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.Login.ToString());
        }

        private void OnAuthenticated()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(new AuthenticationView());
        }

        private void OnAuthorized()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(ContentView);
        }


        private void OnUnauthorized()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(new UnauthorizedView());
        }

        private void OnError()
        {

            grdRoot.Children.Clear();
            grdRoot.Children.Add(new ErrorView());
        }

        private void OnToggleProjects()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Projects);
        }

        private void OnToggleTasks()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Tasks);
        }

        private void OnToggleStyx()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Styx);
        }

        private void OnToggleTimeEntry()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Chronos);
        }

        private void OnToggleCredentials()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Credentials);
        }

        private void ToggleTimeEntry()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleTimeEntry.ToString());
        }

        private void ToggleProjects()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleProjects.ToString());
        }

        private void ToggleTasks()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleTasks.ToString());
        }


        private void ToggleStyx()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleStyx.ToString());
        }

        private void ToggleCredentials()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleCredentials.ToString());
        }
        private void OnToggleLists()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Workflow);
        }

        private void ToggleSettings()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleSettings.ToString());
        }

        private void OnToggleSettings()
        {
            grdRoot.Children.Clear();
            grdRoot.Children.Add(Settings);
        }

        private void ToggleLists()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleLists.ToString());
        }

    }
}
