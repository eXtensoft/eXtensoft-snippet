using Bitsmith.Models;
using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class ProjectModule : Module
    {
        public string Display { get; set; } = "Projectus Maximus";

        public ObservableCollection<TaskItemViewModel> Items { get; set; }
        public ObservableCollection<DomainViewModel> Domains { get; set; }


        private DomainViewModel _SelectedDomain;
        public DomainViewModel SelectedDomain
        {
            get
            {
                return _SelectedDomain;
            }
            set
            {
                _SelectedDomain = value;
                OnPropertyChanged("SelectedDomain");
            }
        }

        private ICommand _AddDomainCommand;
        public ICommand AddDomainCommand
        {
            get
            {
                if (_AddDomainCommand == null)
                {
                    _AddDomainCommand = new RelayCommand(
                    param => AddDomain(),
                    param => CanAddDomain());
                }
                return _AddDomainCommand;
            }
        }

        private bool CanAddDomain()
        {
            return true;
        }
        private void AddDomain()
        {
            Domains.Add(new DomainViewModel(new Domain().Default(DateTime.Now, Guid.NewGuid().ToString().ToLower())));
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
            var model = new TaskItem().Default(SelectedDomain.Model);
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
        internal bool CanSaveWorkspace()
        {
            return true;
        }
        internal void SaveWorkspace()
        {
            SaveData();
        }





        public override string Filepath => FileSystemDataProvider.Filepath<Project>();

        protected override bool LoadData()
        {
            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Project project = new Project().Default();
                if (!FileSystemDataProvider.TryWrite<Project>(project, out string error, filepath))
                {
                    OnFailure(error);
                }
            }

            bool b = FileSystemDataProvider.TryRead<Project>(Filepath, out model, out string message);
            if (!b)
            {
                OnFailure(message);
            }

            return b;
        }

        protected override bool SaveData()
        {
            bool b = true;
            var filepath = FileSystemDataProvider.Filepath<TaskItem>("archive");
            var removals = (from x in Items where x.Status.Token.Equals("archived") select x.Model.Id).ToList();
            
            if (removals.Count > 0)
            {
                var toArchive = model.Items.Where(x => removals.Contains(x.Id)).ToList();
                if (FileSystemDataProvider.TryRead<TaskItem>(filepath,out List<TaskItem> previouslyArchived, out string message))
                {
                    toArchive.AddRange(previouslyArchived);
                }
                if (!FileSystemDataProvider.TryWrite<TaskItem>(toArchive, out string error, filepath))
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
                if (!FileSystemDataProvider.TryWrite<Project>(model, out string message))
                {
                    OnFailure(message);
                    return false;
                }
            }
            return true;
        }

        public override void Initialize()
        {
            
            if (model != null)
            {
                Domains = new ObservableCollection<DomainViewModel>(from x in model.Domains select new DomainViewModel(x));
                SelectedDomain = Domains.FirstOrDefault();
                Items = new ObservableCollection<TaskItemViewModel>(from x in model.Items select new TaskItemViewModel(x));
                Items.CollectionChanged += Items_CollectionChanged;

            }
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

        private Project model;
        public Project Project
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


        public ProjectModule()
        {
            var projectdirectory = "project-files";
            Application.Current.Properties[AppConstants.ProjectDirectory] = projectdirectory;
            
        }

    }
}
