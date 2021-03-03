using Bitsmith.BusinessProcess;
using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class TasksModule : Module
    {
        public string Display { get; set; } = "";

        public SettingsModule Settings { get; set; }

        public ObservableCollection<TaskItemViewModel> Items { get; set; }

        private ICollectionView _DomainWorkflows;
        public ICollectionView DomainWorkflows
        {
            get
            {
                if (_DomainWorkflows == null)
                {
                    _DomainWorkflows = CollectionViewSource.GetDefaultView(Workflows);
                    _DomainWorkflows.Filter = FilterForDomain;
                }
                return _DomainWorkflows;
            }
        }

        private bool FilterForDomain(object o)
        {
            bool b = false;
            var vm = o as WorkflowViewModel;
            if (vm != null)
            {
                var found = Settings.SelectedDomain.DomainWorkflowSelections.FirstOrDefault(x => x.Id.Equals(vm.Id, StringComparison.OrdinalIgnoreCase));
                if (found != null && found.IsSelected)
                {
                    b = true;
                }               
            }
            return b;
        }

        public ObservableCollection<WorkflowViewModel> Workflows { get; set; }

        private WorkflowViewModel _SelectedWorkflow;
        public WorkflowViewModel SelectedWorkflow
        {
            get
            {
                if (_SelectedWorkflow == null)
                {
                    _SelectedWorkflow = Workspace.Instance.ViewModel.Settings.Workflows[0];
                }
                return _SelectedWorkflow;
            }
            set
            {
                _SelectedWorkflow = value;
                OnPropertyChanged("SelectedWorkflow");
            }
        }

        public bool IsItemSelected
        {
            get
            {
                return _SelectedItem != null;
            }
        }

        private TaskItemViewModel _SelectedItem;
        public TaskItemViewModel SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("IsItemSelected");
            }
        }



        public NewTaskViewModel Input { get; set; } = new NewTaskViewModel();


        private ICommand _AddItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new RelayCommand(
                    param => AddItem(),
                    param => CanAddItem());
                }
                return _AddItemCommand;
            }
        }
        private bool CanAddItem()
        {
            return true;
        }
        private void AddItem()
        {
            var model = new TaskItem().Default(Settings.SelectedDomain.Model);
            if (SelectedWorkflow != null)
            {
                model.WorkflowId = _SelectedWorkflow.Id;
            }
            var vm = new TaskItemViewModel(model);
            Items.Add(vm);
        }


        private ICommand _SaveWorkspaceCommand;
        public ICommand SaveWorkspaceCommand
        {
            get
            {
                if (_SaveWorkspaceCommand == null)
                {
                    _SaveWorkspaceCommand = new RelayCommand(
                    param => SaveWorkspace(),
                    param => CanSaveWorkspace());
                }
                return _SaveWorkspaceCommand;
            }
        }


        protected override bool LoadData()
        {
            string filepath = Filepath();
            if (!File.Exists(filepath))
            {
                TaskManager project = new TaskManager().Default();
                if (!DataService.TryWrite<TaskManager>(project, out string error, filepath))
                {
                    OnFailure(error);
                }
            }

            bool b = DataService.TryRead<TaskManager>(filepath, out model, out string message);
            if (!b)
            {
                OnFailure(message);
            }

            return b;
        }

        protected override bool SaveData()
        {
            bool b = true;
            var filepath = Path.Combine(AppConstants.TasksDirectory, DataService.Filepath<TaskItem>("archive"));
            var removals = (from x in Items where x.Status.Token.Equals("archived") select x.Model.Id).ToList();
            
            if (removals.Count > 0)
            {
                var toArchive = model.Items.Where(x => removals.Contains(x.Id)).ToList();
                if (DataService.TryRead<TaskItem>(filepath,out List<TaskItem> previouslyArchived, out string message))
                {
                    toArchive.AddRange(previouslyArchived);
                }
                if (!DataService.TryWrite<TaskItem>(toArchive, out string error, filepath))
                {
                    b = false;
                }
                else
                {
                    foreach (var id in removals)
                    {
                        var found = Items.First(x => x.Model.Id.Equals(id));
                        Items.Remove(found);
                    }
                }

            }

            if (model != null)
            {
                if (!DataService.TryWrite<TaskManager>(model, out string message,Filepath()))
                {
                    OnFailure(message);
                    return false;
                }
            }
            return true;
        }

        protected override void ApplyPreferences()
        {
            if (UserPreferences.TryGet<string>(ModuleKey, "selected-workflow", out string workflow))
            {
                var found = Workflows.FirstOrDefault(x => x.Id.Equals(workflow));
                if (found != null)
                {
                    SelectedWorkflow = found;
                }
                else
                {
                    SelectedWorkflow = Workflows.FirstOrDefault();
                }
            }
            // tasks-domain-workflow FOR EACH DOMAIN
            RefreshDomainWorkflows();
        }

        private void RefreshDomainWorkflows()
        {
            
            //if (UserPreferences.TryGetAny<string>($"{ModuleKey}.domain-workflow",SelectedDomain.Id, out List<string> list))
            //{

            //}
            //HashSet<string> hs = new HashSet<string>();
            //foreach (var item in list)
            //{
            //    string[] parts = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (parts.Length == 2)
            //    { 
            //        var workflow = Workflows.FirstOrDefault(x => x.Id.Equals(parts[1]));
            //        var domain = Domains.FirstOrDefault(x => x.Id.Equals(parts[0]));
            //        if (workflow != null && domain != null && hs.Add(item))
            //        {
            //            domain.DomainWorkflowSelections.Add(new ListItemViewModel(new ListItem() { Identifier = new TagIdentifier() { Display = workflow.Display, Id = workflow.Id, MasterId = domain.Id, Token = $"{domain.Id}|{workflow.Id}" }  }) { IsSelected = true }); ;
            //        }
            //    }               
            //}

            //foreach (var domain in Domains)
            //{
            //    foreach (var workflow in Workflows)
            //    {
            //        var found = domain.DomainWorkflowSelections.FirstOrDefault(x => x.MasterId.Equals(domain.Id) && x.Id.Equals(workflow.Id));
            //        if (found == null)
            //        {
            //            domain.DomainWorkflowSelections.Add(new ListItemViewModel(new ListItem() { Identifier = new TagIdentifier() { Display = workflow.Display, Id = workflow.Id, MasterId = domain.Id, Token = $"{domain.Id}|{workflow.Id}" } }));
            //        }
            //    }
            //}

        }


        internal override void SetPreferences()
        {
            UserPreferences.EnsurePreference(ModuleKey, "selected-workflow", SelectedWorkflow.Id);
            UserPreferences.EnsurePreference(ModuleKey, "selected-domain", Settings.SelectedDomain.Model.Id);
        }

        public override void Initialize()
        {                        
            if (model != null)
            {
                Items = new ObservableCollection<TaskItemViewModel>(from x in model.Items select new TaskItemViewModel(x));
                Items.CollectionChanged += Items_CollectionChanged;
            }
            ApplyPreferences();
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as TaskItemViewModel;
                    if (vm != null)
                    {
                        model.Items.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as TaskItemViewModel;
                    if (vm != null)
                    {
                        model.Items.Remove(vm.Model);
                    }
                }
            }
        }


        private void InitializeSelections()
        {
            //StatusSelections = new ObservableCollection<Disposition>(Enum.GetNames(typeof(StatusOption)).ToDispositions("status"));
            //UrgencySelections = new ObservableCollection<Disposition>(Enum.GetNames(typeof(ScaleOption)).ToDispositions("urgency"));
            //ImportanceSelections = new ObservableCollection<Disposition>(Enum.GetNames(typeof(ScaleOption)).ToDispositions("importance"));
        }

        private TaskManager model;
        public TaskManager Project
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                OnPropertyChanged("Project");
            }
        }


        public TasksModule(IDataService dataService, SettingsModule settings)
        {
            DataService = dataService;
            UserPreferences = settings.UserPreferences;
            Workflows = settings.Workflows;
            Settings = settings;
        }

        protected override string Filepath()
        {
            return Path.Combine(AppConstants.TasksDirectory, DataService.Filepath<TaskManager>());
        }
    }
}
