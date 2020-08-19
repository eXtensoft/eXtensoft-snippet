using Bitsmith.BusinessProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class SettingsModule : Module<Settings>
    {

        public string Display => "App Settings";

        private ICommand _AddWorkflowCommand;
        public ICommand AddWorkflowCommand
        {
            get
            {
                if (_AddWorkflowCommand == null)
                {
                    _AddWorkflowCommand = new RelayCommand(
                    param => AddWorkflow(),
                    param => CanAddWorkflow());
                }
                return _AddWorkflowCommand;
            }
        }
        private bool CanAddWorkflow()
        {
            return true;
        }
        private void AddWorkflow()
        {
            var model = new Workflow().Default();
            var vm = new WorkflowViewModel(model);
            Workflows.Add(vm);
        }

        private ICommand _RemoveWorkflowCommand;
        public ICommand RemoveWorkflowCommand
        {
            get
            {
                if (_RemoveWorkflowCommand == null)
                {
                    _RemoveWorkflowCommand = new RelayCommand(
                    param => RemoveWorkflow(),
                    param => CanRemoveWorkflow());
                }
                return _RemoveWorkflowCommand;
            }
        }
        private bool CanRemoveWorkflow()
        {
            return _SelectedWorkflow != null;
        }
        private void RemoveWorkflow()
        {

        }


        private WorkflowViewModel _SelectedWorkflow;
        public WorkflowViewModel SelectedWorkflow
        {
            get
            {
                return _SelectedWorkflow;
            }
            set
            {
                _SelectedWorkflow = value;
                OnPropertyChanged("SelectedWorkflow");
            }
        }



        public ObservableCollection<WorkflowViewModel> Workflows { get; set; }


        private Settings _Settings;
        public Settings Settings
        { 
            get { return _Settings; }
            set
            {
                _Settings = value;
            }
        }

        public SettingsModule()
        {
            Filepath = Path.Combine(AppConstants.SettingsDirectory, base.Filepath);
        }


        protected override bool LoadData()
        {

            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Models.Add(new Settings().Default());
                
                if (!FileSystemDataProvider.TryWrite<Settings>(Models, out string error, filepath))
                {
                    OnFailure(error);
                }
            }

            bool b = FileSystemDataProvider.TryRead<Settings>(Filepath, out List<Settings> list, out string message);
            if (b)
            {
                Models = list;
            }
            else
            {
                OnFailure(message);
            }
            if (Models != null && Models.Count >= 1)
            {
                _Settings = Models[0];
            }
            if (_Settings.UserPreferences == null)
            {
                _Settings.UserPreferences = new List<UserSettings>();
                var current = new UserSettings().Default();
                _Settings.UserPreferences.Add(current);
            }
            var preferences = _Settings.UserPreferences.FirstOrDefault(x => x.Username.Equals(Environment.UserName));
            if (preferences == null)
            {
                preferences = new UserSettings().Default();
                _Settings.UserPreferences.Add(preferences);
            }
            UserPreferences = preferences;
            return b;
        }

        public override void Initialize()
        {
            Workflows = new ObservableCollection<WorkflowViewModel>(from x in _Settings.Workflows select new WorkflowViewModel(x));
            Workflows.CollectionChanged += Workflows_CollectionChanged;



        }

        private void Workflows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as WorkflowViewModel;
                    if (vm != null)
                    {
                        _Settings.Workflows.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as WorkflowViewModel;
                    if (vm != null)
                    {
                        _Settings.Workflows.Remove(vm.Model);
                    }
                }
            }
        }

        protected override void SaveData()
        {
            base.SaveData();
        }


        protected virtual void OnFailure(string message)
        {
            MessageBox.Show(message);
        }

    }
}
