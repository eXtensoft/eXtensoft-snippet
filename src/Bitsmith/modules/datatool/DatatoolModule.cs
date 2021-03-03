using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class DatatoolModule : Module
    {
        public List<string> FieldDatatypes { get; set; } = new List<string>() { "System.String","System.Integer","System.DateTime" };
        public string Display { get; set; } = "xtool";
        public bool HasSelected
        {
            get { return SelectedItem != null; }
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
            ContentItem model = new ContentItem().Default();
            Items.Add(new TabularDataViewModel(model));
        }


        private TabularDataViewModel _SelectedItem;
        public TabularDataViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        public ObservableCollection<TabularDataViewModel> Items { get; set; }

        public Datatool Model { get; set; }

        public DatatoolModule(IDataService dataService)
        {
            DataService = dataService;
        }

        protected override string Filepath()
        {
            return Path.Combine(AppConstants.DatatoolDirectory, DataService.Filepath<Datatool>());
        }

        public override void Initialize()
        {
            Items = new ObservableCollection<TabularDataViewModel>(from c in Model.Items select new TabularDataViewModel(c));
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as TabularDataViewModel;
                    if (vm != null)
                    {
                        Model.Items.Add(vm.Model);
                    }
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as TabularDataViewModel;
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
                Datatool datatool = new Datatool().Default();
                if (!DataService.TryWrite<Datatool>(datatool, out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = DataService.TryRead<Datatool>(filepath, out Datatool model, out string message);
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
                if (!DataService.TryWrite<Datatool>(Model, out string message, Filepath()))
                {
                    OnFailure(message);
                    b = false;
                }
            }
            return b;
        }


    }
}
