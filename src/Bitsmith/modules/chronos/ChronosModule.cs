using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class ChronosModule : Module
    {

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
            item.Task = new TagIdentifier() { Display = SelectedTask.Display, Id = SelectedTask.Model.Id, Token = SelectedTask.Model.Identifier.Token };
            item.Activity = new TagIdentifier() { Display = SelectedActivity.Display, Id = SelectedActivity.Id, Token = SelectedActivity.Token };
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

        public bool CanSaveWorkspace()
        {
            return Model != null;
        }

        public void SaveWorkspace()
        {
            SaveData();
        }


        public ChronosModule()
        {
            Filepath = Path.Combine(AppConstants.ChronosDirectory, FileSystemDataProvider.Filepath<Chronos>());
        }


        public override void Initialize()
        {
            Activities = new List<TagIdentifier>().Activities();
            _Minutes = 60;
            TimeEntries = new ObservableCollection<TimeEntryViewModel>(from x in Model.Items select new TimeEntryViewModel(x));
            TimeEntries.CollectionChanged += TimeEntries_CollectionChanged;

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
            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Chronos chronos = new Chronos().Default();
                if (!FileSystemDataProvider.TryWrite<Chronos>(chronos, out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = FileSystemDataProvider.TryRead<Chronos>(Filepath, out Chronos model, out string message);
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
                if (!FileSystemDataProvider.TryWrite<Chronos>(Model,out string message, Filepath))
                {
                    OnFailure(message);
                    b = false;
                }
            }
            return b;
        }

       


    }
}
