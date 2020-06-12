using Bitsmith.Models;
using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{

    public class TaskItemViewModel : ViewModel<TaskItem>
    {

        private List<Disposition> _UrgencySelections;
        public List<Disposition> UrgencySelections 
        {
            get
            {
                if (_UrgencySelections == null)
                {
                    _UrgencySelections = Enum.GetNames(typeof(ScaleOption)).ToDispositions("urgency");
                }
                return _UrgencySelections;
            }
            set { _UrgencySelections = value; }
        }

        private List<Disposition> _ImportanceSelections;

        public List<Disposition> ImportanceSelections 
        {
            get
            {
                if (_ImportanceSelections == null)
                {
                    _ImportanceSelections = Enum.GetNames(typeof(ScaleOption)).ToDispositions("importance");
                }
                return _ImportanceSelections;
            }
            set { _ImportanceSelections = value; }
        
        }

        private List<WorkflowStep> _Selections;
        public List<WorkflowStep> StatusSelections
        {
            get
            {
                return _Selections;
            }
            set { }
        }

        private WorkflowStep _Selected;
        public WorkflowStep SelectedStatus
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (value != null && value.IsTransition)
                {
                    Transition(value.Name);
                }

            }
        }
        private void Transition(string name)
        {
            Machine.ExecuteTransition(name);
            ResolveSelections();
            var disposition = Machine.GetCurrentState().ToDisposition();
            Status = disposition;
        }


        private StateMachine _Machine;
        public StateMachine Machine
        { 
            get
            {
                return _Machine;
            }
            set
            {
                if (value != null)
                {
                    _Machine = value;
                    if (Status != null)
                    {
                        _Machine.SetState(Status.Token);
                    }
                    else
                    {
                        _Machine.SetState();
                    }
                    ResolveSelections();
                }
            }
        }

        public WorkflowStep CurrentState
        {
            get { return Machine.GetCurrentState().ToStep(); }
        }

        public List<WorkflowStep> Transitions
        {
            get
            {
                return Machine.GetTransitions().ToSteps().ToList();
            }

        }


        private void ResolveSelections()
        {
            _Selections = new List<WorkflowStep>();
            _Selections.Add(CurrentState);
            _Selections.AddRange(Transitions);
            _Selected = _Selections.FirstOrDefault(x => x.Name.Equals(Machine.CurrentState));
            OnPropertyChanged("StatusSelections");
            OnPropertyChanged("SelectedStatus");
        }


        public string WorkflowId
        {
            get
            {
                return Model.WorkflowId;
            }
            set
            {
                Model.WorkflowId = value;
                OnPropertyChanged("WorkflowId");
            }
        }


        public string Display
        {
            get
            {
                return Model.Display;
            }
            set
            {
                Model.Display = value;
                OnPropertyChanged("Display");
            }
        }

        public string Identifier
        {
            get
            {
                return Model.Identifier.Display;
            }
            set
            {
                Model.Identifier.Display = value;
                Model.Identifier.Token = value.ToToken();
                OnPropertyChanged("Identifier");
            }
        }

        public DateTime DueOn
        {
            get
            {
                return Model.DueOn;
            }
            set
            {
                Model.DueOn = value;
                OnPropertyChanged("DueOn");
            }
        }



        private Disposition _Status;
        public Disposition Status
        {
            get
            {
                if (_Status == null)
                {
                    _Status = Model.Dispositions.Last(x => x.Key.Equals("status"));
                }
                return _Status;
            }
            set
            {
                value.StartedAt = DateTime.Now;
                _Status = value;
                Model.Dispositions.Add(_Status);
                OnPropertyChanged("Status");
            }
        }

        private Disposition _Urgency;
        public Disposition Urgency
        {
            get
            {
                if (_Urgency == null)
                {
                    var found = Model.Dispositions.OrderBy(y=>y.StartedAt).Last(x => x.Key.Equals("urgency"));
                    if (found != null)
                    {
                        _Urgency = UrgencySelections.First(x => x.Token.Equals(found.Token));
                    }
                    else
                    {
                        _Urgency = UrgencySelections.First();
                    }
                }
                return _Urgency;
            }
            set
            {
                if (value != null)
                {
                    _Urgency = GenericObjectManager.Clone<Disposition>(value);
                    _Urgency.StartedAt = DateTime.Now;
                    Model.Dispositions.Add(_Urgency);
                    OnPropertyChanged("Urgency");
                }

            }
        }

        private Disposition _Importance;
        public Disposition Importance
        {
            get
            {
                if (_Importance == null)
                {
                    var found = Model.Dispositions.OrderBy(y=>y.StartedAt).Last(x => x.Key.Equals("importance"));
                    if (found != null)
                    {
                        _Importance = ImportanceSelections.First(x => x.Token.Equals(found.Token));
                    }
                    else
                    {
                        _Importance = ImportanceSelections.First();
                    }
                }
                return _Importance;
            }
            set
            {
                if (value != null)
                {
                    _Importance = GenericObjectManager.Clone<Disposition>(value);
                    _Importance.StartedAt = DateTime.Now;
                    Model.Dispositions.Add(_Importance);
                    OnPropertyChanged("Importance");
                }
            }
        }


        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _NotesDisplay = "Notes";
        public string NotesDisplay
        {
            get
            {
                return _NotesDisplay;
            }
            set
            {
                _NotesDisplay = value;
                OnPropertyChanged("NotesDisplay");
            }
        }

        private string _FilesDisplay = "Files";
        public string FilesDisplay
        {
            get
            {
                return _FilesDisplay;
            }
            set
            {
                _FilesDisplay = value;
                OnPropertyChanged("FilesDisplay");
            }
        }

        private string _LinksDisplay = "Links";
        public string LinksDisplay
        {
            get
            {
                return _LinksDisplay;
            }
            set
            {
                _LinksDisplay = value;
                OnPropertyChanged("LinksDisplay");
            }
        }



        private ICommand _AddNoteCommand;
        public ICommand AddNoteCommand
        {
            get
            {
                if (_AddNoteCommand == null)
                {
                    _AddNoteCommand = new RelayCommand(
                    param => AddNote(),
                    param => CanAddNote());
                }
                return _AddNoteCommand;
            }
        }
        private bool CanAddNote()
        {
            return true;
        }
        private void AddNote()
        {
            ContentItem item = new ContentItem()
            {
                Id = Guid.NewGuid().ToString().ToLower(),
                Display = "display",
                Scope = ScopeOption.Private,
                Mime = "text", 
                Body = "note"
            };
            item.Properties.DefaultTags();
            item.Properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}", Value = Model.Id });
            Workspace.Instance.ViewModel.Content.AddContent(item);
            RefreshContent();
        }

        private ICommand _AddFileCommand;
        public ICommand AddFileCommand
        {
            get
            {
                if (_AddFileCommand == null)
                {
                    _AddFileCommand = new RelayCommand(
                    param => AddFile(),
                    param => CanAddFile());
                }
                return _AddFileCommand;
            }
        }
        private bool CanAddFile()
        {
            return true;
        }
        private void AddFile()
        {
            MessageBox.Show("Add File");
        }

        private ICommand _AddLinkCommand;
        public ICommand AddLinkCommand
        {
            get
            {
                if (_AddLinkCommand == null)
                {
                    _AddLinkCommand = new RelayCommand(
                    param => AddLink(),
                    param => CanAddLink());
                }
                return _AddLinkCommand;
            }
        }
        private bool CanAddLink()
        {
            return true;
        }
        private void AddLink()
        {
            MessageBox.Show("Add Link");
        }

        private ContentItemViewModel _SelectedItem;
        public ContentItemViewModel SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
                if (value != null)
                {
                    dynamic param = new ExpandoObject();
                    param.Title = _SelectedItem.Display;
                    param.Control = Workspace.Instance.ViewModel.Content.Mimes.Resolve(value);
                    Workspace.Instance.ViewModel.Overlay.SetOverlay(AppConstants.OverlayContent, param);
                }
            }
        }


        private ObservableCollection<ContentItemViewModel> _Items;
        public ObservableCollection<ContentItemViewModel> Items 
        {
            get
            {
                if (_Items == null)
                {
                    RefreshContent();
                }
                return _Items;
            }
            set 
            {
                _Items = value;
                if (value != null)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(Items);
                    view.GroupDescriptions.Add(new PropertyGroupDescription("ContentType"));
                }
                OnPropertyChanged("Items");
            } 
        }


        private void RefreshContent()
        {
            var found = Workspace.Instance.ViewModel.Content.ExecuteTaskSearch(Model.Id);
            if (found != null && found.Count > 0)
            {
                Items = new ObservableCollection<ContentItemViewModel>(from x in found select x);
            }
        }


        private ICommand _ArchiveTaskCommand;
        public ICommand ArchiveTaskCommand
        {
            get
            {
                if (_ArchiveTaskCommand == null)
                {
                    _ArchiveTaskCommand = new RelayCommand(
                    param => ArchiveTask(),
                    param => CanArchiveTask());
                }
                return _ArchiveTaskCommand;
            }
        }
        private bool CanArchiveTask()
        {
            return true;
        }
        private void ArchiveTask()
        {
            Status =  new Disposition().Archive();
        }


        public TaskItemViewModel(TaskItem model)
        {
            Model = model;
        }

        private void Initialize()
        {
            if (_Machine == null)
            {
                if (Workspace.Instance.ViewModel.Settings.Workflows.Contains(WorkflowId))
                {
                    _Machine = Workspace.Instance.ViewModel.Settings.Workflows[WorkflowId].Machine.Clone();
                }
                else
                {
                    _Machine = new StateMachine().Default();
                    WorkflowId = AppConstants.Defaults.WorkflowId;
                }

                if (Status != null)
                {
                    _Machine.SetState(Status.Token);
                }
                else
                {
                    _Machine.SetState();
                }
                ResolveSelections();
            }
        }


    }
}
