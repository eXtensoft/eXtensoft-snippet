using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using Bitsmith.Models.Views;
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
    public class ChronosModule : Module
    {

        public ObservableCollection<TaskViewItem> TaskViewItems { get; set; }


        private ObservableCollection<WorkEffortViewModel> _Groupings;
        public ObservableCollection<WorkEffortViewModel> Groupings 
        {
            get { return _Groupings; }
            set
            {
                _Groupings = value;
                OnPropertyChanged("Groupings");
            }
        
        }

        public ObservableCollection<TimeEntryViewModel> TimeEntries { get; set; }

        public Chronos Model { get; set; }

        private ChronosViewTypeOptions _SelectedView = ChronosViewTypeOptions.Week;
        public ChronosViewTypeOptions SelectedView
        {
            get { return _SelectedView; }
            set
            {
                _SelectedView = value;
                OnPropertyChanged("SelectedView");
            }
        }

        private readonly Dictionary<ChronosViewTypeOptions, Func<TimeEntryViewModel, string, bool>> _Filters = new Dictionary<ChronosViewTypeOptions, Func<TimeEntryViewModel, string, bool>>() 
        {
            {ChronosViewTypeOptions.None,FilterByNone},
            {ChronosViewTypeOptions.Actor,FilterByActor},
            {ChronosViewTypeOptions.Day,FilterByDay},
            {ChronosViewTypeOptions.Week,FilterByWeek},
            {ChronosViewTypeOptions.Sprint,FilterBySprint},
            {ChronosViewTypeOptions.Project,FilterByProject},
            {ChronosViewTypeOptions.System,FilterSystem},
        };

        private static bool FilterByNone(TimeEntryViewModel item, string arg)
        {
            return item != null;
        }

        private static bool FilterByActor(TimeEntryViewModel item, string arg)
        {
            return !string.IsNullOrWhiteSpace(arg) ? item.Model.Actor.Token.Equals(arg,StringComparison.OrdinalIgnoreCase) : true;
        }

        private static bool FilterByDay(TimeEntryViewModel item, string arg)
        {
            DateTime target;
            if (!string.IsNullOrWhiteSpace(arg) && DateTime.TryParse(arg,out target))
            {
            }
            else
            {
                target = DateTime.Now.Date;
            }
            return item.Model.Started.Date.Equals(target);
        }

        private static bool FilterByWeek(TimeEntryViewModel item, string arg)
        {
            DateTime target;
            if (!string.IsNullOrWhiteSpace(arg) && DateTime.TryParse(arg, out target))
            {
            }
            else
            {
                target = DateTime.Now.Date;
            }
            return true;
        }

        private static bool FilterBySprint(TimeEntryViewModel item, string arg)
        {
            return false;
        }

        private static bool FilterByProject(TimeEntryViewModel item, string arg)
        {
            return false;
        }

        private static bool FilterSystem(TimeEntryViewModel item, string arg)
        {
            return true;
        }


        public List<TagIdentifier> Activities { get; set; }

        private TaskItemViewModel _SelectedTask;
        public TaskItemViewModel SelectedTask
        {
            get
            {
                return _SelectedTask;
            }
            set
            {
                _SelectedTask = value;
                OnPropertyChanged("SelectedTask");
                Message = string.Empty;
            }
        }

        private TagIdentifier _SelectedActivity;
        public TagIdentifier SelectedActivity
        {
            get { return _SelectedActivity; }
            set
            {
                _SelectedActivity = value;
                OnPropertyChanged("SelectedActivity");
                Message = string.Empty;
            }
        }




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
            bool b = true;
            b = b && Minutes > 5;
            b = b && SelectedActivity != null;
            b = b && SelectedTask != null;
            return b;
        }
        private void AddItem()
        {
            TimeEntry item = new TimeEntry().Default(Start);
            item.Task = new TagIdentifier() 
            { 
                Display = SelectedTask.Display, 
                Id = SelectedTask.Model.Id, 
                Token = SelectedTask.Status.Display
            };
            item.Activity = new TagIdentifier() 
            { 
                Display = SelectedActivity.Display, 
                Id = SelectedActivity.Id, 
                Token = SelectedActivity.Token 
            };
            item.Minutes = Minutes;
            item.Comment = Comment; 
            TimeEntryViewModel vm = new TimeEntryViewModel(item);
            TimeEntries.Add(vm);
            RefreshItem();
            Message = item.ToDisplay();
        }

        private ICommand _RefreshItemCommand;
        public ICommand RefreshItemCommand
        {
            get
            {
                if (_RefreshItemCommand == null)
                {
                    _RefreshItemCommand = new RelayCommand(
                    param => RefreshItem(),
                    param => CanRefreshItem());
                }
                return _RefreshItemCommand;
            }
        }
        private bool CanRefreshItem()
        {
            return true;
        }
        private void RefreshItem()
        {
            SelectedTask = null;
            SelectedActivity = null;
            Start = DateTime.Now;
            Comment = string.Empty;
            Minutes = 15;
            Message = string.Empty;
        }

        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private DateTime _Start = DateTime.Now;
        public DateTime Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
                OnPropertyChanged("Start");
                Message = string.Empty;
            }
        }

        private int _Minutes;
        public int Minutes
        {
            get { return _Minutes; }
            set
            {
                _Minutes = value;
                OnPropertyChanged("Minutes");
                Message = string.Empty;
            }
        }

        private string _Comment;
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                _Comment = value;
                OnPropertyChanged("Comment");
                Message = string.Empty;
            }
        }

        public override bool CanSaveWorkspace()
        {
            return Model != null;
        }


        public ChronosModule(IDataService dataService)
        {
            DataService = dataService;
        }

        public void RefreshWorkEffort(List<ProjectManagement.TaskItem> tasks)
        {
            TaskViewItems = new ObservableCollection<TaskViewItem>(tasks.Build(Model.Items));
        }


        private ICollectionView _View;
        public ICollectionView View
        {
            get
            {
                if (TimeEntries != null && _View == null)
                {
                    _View = CollectionViewSource.GetDefaultView(TimeEntries);
                    _View.Filter = FilterEntry;
                }
                return _View;
            }
        }

        public override void Initialize()
        {
            Activities = new List<TagIdentifier>().Activities();
            _Minutes = 60;
            BuildWorkEfforts();
            TimeEntries = new ObservableCollection<TimeEntryViewModel>(from x in Model.Items select new TimeEntryViewModel(x));
            TimeEntries.CollectionChanged += TimeEntries_CollectionChanged;
            _View = (CollectionView)CollectionViewSource.GetDefaultView(TimeEntries);
            _View.Filter = FilterEntry;

        }
        protected override string Filepath()
        {
            return Path.Combine(AppConstants.ChronosDirectory, DataService.Filepath<Chronos>());
        }

        private void BuildWorkEfforts()
        {
            var grps = from t in Model.Items
                       group t by t.Task.Id into grp
                       select grp;
            if (grps.Count() > 0)
            {
                List<WorkEffortViewModel> list = new List<WorkEffortViewModel>(from g in grps select new WorkEffortViewModel(g));
                Groupings = new ObservableCollection<WorkEffortViewModel>(list);
            }

        }


        private bool FilterEntry(object item)
        {
            bool b = false;
            var vm = item as TimeEntryViewModel;
            if (vm != null)
            {
                switch (SelectedView)
                {
                    case ChronosViewTypeOptions.None:
                        b = FilterByNone(vm,null);
                        break;
                    case ChronosViewTypeOptions.Actor:
                        b = false;
                        break;
                    case ChronosViewTypeOptions.Day:
                        b = FilterByDay(vm, DateTime.Now.ToString());
                        break;
                    case ChronosViewTypeOptions.Week:
                        b = FilterByDay(vm, DateTime.Now.ToString());
                        break;
                    case ChronosViewTypeOptions.Sprint:
                        break;
                    case ChronosViewTypeOptions.Project:
                        break;
                    case ChronosViewTypeOptions.System:
                        b = true;
                        break;
                    default:
                        break;
                }
            }

            return b;
        }

        private void TimeEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as TimeEntryViewModel;
                    if (vm != null)
                    {
                        Model.Items.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as TimeEntryViewModel;
                    if (vm != null)
                    {
                        Model.Items.Remove(vm.Model);
                    }
                }
            }
        }



        protected override bool LoadData()
        {
            string filepath = Filepath();
            if (!File.Exists(filepath))
            {
                Chronos chronos = new Chronos().Default();
                if (!DataService.TryWrite<Chronos>(chronos, out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = DataService.TryRead<Chronos>(filepath, out Chronos model, out string message);
            if (!b)
            {
                OnFailure(message);
            }
            else
            {
                Model = model;
            }

            return b;
        }

        protected override bool SaveData()
        {
            bool b = true;
            if (Model != null)
            {
                if (!DataService.TryWrite<Chronos>(Model,out string message, Filepath()))
                {
                    OnFailure(message);
                    b = false;
                }
            }
            return b;
        }

       


    }
}
