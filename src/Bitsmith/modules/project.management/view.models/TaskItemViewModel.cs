﻿using Bitsmith.Models;
using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Bitsmith.Models;

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



        private ICommand _AddContentCommand;
        public ICommand AddContentCommand
        {
            get
            {
                if (_AddContentCommand == null)
                {
                    _AddContentCommand = new RelayCommand(
                    param => AddContent(),
                    param => CanAddContent());
                }
                return _AddContentCommand;
            }
        }
        private bool CanAddContent()
        {
            return true;
        }
        private void AddContent()
        {
            ContentItem item = new ContentItem()
            {
                Id = Guid.NewGuid().ToString().ToLower(),
                Scope = ScopeOption.Private,
            };
            item.Properties.DefaultTags();
            item.Properties.Add(new Property() 
            { 
                Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}", 
                Value = Model.Id 
            });

            bool b = true;
            switch (_ContentType)
            {
                case ContentTypeOption.None:
                    break;
                case ContentTypeOption.File:
                    var content = Workspace.Instance.ViewModel.Content;
                    if (TryLocateFile(out FileInfo info) && 
                        content.ContentManager.TryInload(info, out string filename))
                    {
                        item.Display = info.Name;
                        item.Mime = content.Mimes.Resolve(info);
                        item.Body = filename;
                        item.Properties.Add(new Property()
                        {
                            Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}",
                            Value = item.Mime
                        });
                    }
                    else
                    {
                        b = false;
                    }
                    break;
                case ContentTypeOption.Link:
                    item.Mime = "url";
                    break;
                case ContentTypeOption.Text:
                    item.Display = "new note";
                    item.Mime = "text";
                    break;
                default:
                    break;
            }
            if (b)
            {
                Workspace.Instance.ViewModel.Content.AddContent(item);
                RefreshContent();
            }
        }

        private bool TryLocateFile(out FileInfo info)
        {
            bool b = false;
            info = null;

            string candidate = String.Empty;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            string directory = Application.Current.Properties[AppConstants.LastOpenedFileDialogFolderpath] as string;
            if (!String.IsNullOrEmpty(directory))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    if (dir != null && dir.Exists)
                    {
                        directory = dir.FullName;
                    }
                    else
                    {
                        directory = String.Empty;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    directory = String.Empty;
                }

            }
            dialog.InitialDirectory = ((!String.IsNullOrEmpty(directory) && Directory.Exists(directory))) ?
                directory : AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                info = new FileInfo(dialog.FileName);
                Application.Current.Properties[AppConstants.LastOpenedFileDialogFolderpath] = info.Directory.FullName;
                b = true;
            }

            return b;
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




        private ContentTypeOption _ContentType = ContentTypeOption.Text;
        public bool IsLink
        {
            get
            {
                return _ContentType == ContentTypeOption.Link;
            }
            set
            {
                if (value)
                {
                    _ContentType = ContentTypeOption.Link;
                }
            }
        }

        public bool IsFile
        {
            get
            {
                return _ContentType == ContentTypeOption.File;
            }
            set
            {
                if (value)
                {
                    _ContentType = ContentTypeOption.File;
                }
            }
        }

        public bool IsText
        {
            get
            {
                return _ContentType == ContentTypeOption.Text;
            }
            set
            {
                if (value)
                {
                    _ContentType = ContentTypeOption.Text;
                }
            }
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
