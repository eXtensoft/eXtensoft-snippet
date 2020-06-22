using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class ChronosModule : Module
    {

        public Chronos Model { get; set; }

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
            return true;
        }
        private void AddItem()
        {

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
            }
        }


        public ChronosModule()
        {
            Filepath = Path.Combine(AppConstants.ChronosDirectory, FileSystemDataProvider.Filepath<Chronos>());
        }


        public override void Initialize()
        {
            Activities = new List<TagIdentifier>().Activities();
            _Minutes = 60;
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
                Model = model;
                OnFailure(message);
            }

            return b;
        }


    }
}
