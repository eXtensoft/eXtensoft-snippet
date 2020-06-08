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

        private ICommand _ExitAppCommand;
        public ICommand ExitAppCommand
        {
            get
            {
                if (_ExitAppCommand == null)
                {
                    _ExitAppCommand = new RelayCommand(param => ExitApp());
                }
                return _ExitAppCommand;
            }
        }

        private void ExitApp()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.Logoff.ToString());
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
            //mgr.RegisterEndpointAction(ActivityStateOption.Administration.ToString(), EndpointOption.Arrival, OnToggleAdmin);
            //mgr.RegisterEndpointAction(ActivityStateOption.Lists.ToString(), EndpointOption.Arrival, OnToggleLists);
            //mgr.RegisterEndpointAction(ActivityStateOption.Contacts.ToString(), EndpointOption.Arrival, OnToggleContacts);
            //mgr.RegisterEndpointAction(ActivityStateOption.Credentials.ToString(), EndpointOption.Arrival, OnToggleCredentials);
            //mgr.RegisterEndpointAction(ActivityStateOption.Security.ToString(), EndpointOption.Arrival, OnToggleSecurity);
            //mgr.RegisterEndpointAction(ActivityStateOption.Todos.ToString(), EndpointOption.Arrival, OnToggleTodos);
            //mgr.RegisterEndpointAction(ActivityStateOption.Remote.ToString(), EndpointOption.Arrival, OnToggleRemote);
            mgr.RegisterEndpointAction(ActivityStateOption.Projects.ToString(), EndpointOption.Arrival, OnToggleProjects);

            AddToggleCommands();
        }

        private void AddToggleCommands()
        {
            //KeyGesture toggleProjects = new KeyGesture(Key.T, ModifierKeys.Control);
            //KeyBinding toggleProjectsBinding = new KeyBinding(ToggleProjectsCommand, toggleProjects);
            //this.InputBindings.Add(toggleProjectsBinding);
            this.InputBindings.Add(new KeyBinding(ToggleProjectsCommand, new KeyGesture(Key.T, ModifierKeys.Control)));
            this.InputBindings.Add(new KeyBinding(ExitAppCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
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
            //Application.Current.Shutdown();
            //HtmlPage.Window.Navigate(new Uri("http://www.google.com",UriKind.RelativeOrAbsolute));
            // navigate to a configurable web page
            //HtmlPage.Window.Invoke("close");
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
        private void ToggleProjects()
        {
            Workspace.Instance.State.Machine.ExecuteTransition(TransitionTypeOption.ToggleProjects.ToString());
        }

    }
}
