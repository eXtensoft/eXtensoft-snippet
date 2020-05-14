using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
	public class VirtualPathModule : Module<DomainPathMap>
    {

		public string SelectedDomainId
		{
			get
			{
				return _SelectedDomain.Id;
			}
			set
			{
				var found = Items.FirstOrDefault(x => x.Id.Equals(value, StringComparison.OrdinalIgnoreCase));
				if (found != null)
				{
					SelectedDomain = found;
				}
				//SetSelectedDomain(value);
				//string id = value;
				//var found = Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
				//if (found == null)
				//{
				//	id = Guid.NewGuid().ToString().ToLower();
				//	AddItem(id);
				//	_SelectedDomain = Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
				//}
				//else
				//{
				//	_SelectedDomain = found;

				//}
			}
		}


		private DomainPathMapViewModel _SelectedDomain;
		public DomainPathMapViewModel SelectedDomain
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


		public ObservableCollection<DomainPathMapViewModel> Items { get; set; }


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
			AddItem(Guid.NewGuid().ToString());
		}
		private void AddItem(string id)
		{
			var domainMapPath = new DomainPathMap() { Id = id, Slug = "slug", Display = "Display", Path = "slug" };
			Items.Add(new DomainPathMapViewModel(domainMapPath));
			OnPropertyChanged("Items");
		}

		private void AddPath()
		{

		}


		protected override bool LoadData()
        {
            if (!File.Exists(Filepath))
            {
                List<DomainPathMap> list = new List<DomainPathMap>().Default();
                FileSystemDataProvider.TryWrite<DomainPathMap>(list, out string message, Filepath);
            }

            return base.LoadData();
        }

        public override void Initialize()
        {

			//var domainMapPath = new DomainPathMap() { Id = Guid.NewGuid().ToString(), Slug = "slug", Display = "Display", Path = "slug" };
			//Models.Add(domainMapPath);
            Items = new ObservableCollection<DomainPathMapViewModel>( from x in Models select new DomainPathMapViewModel(x));
			Items.CollectionChanged += Items_CollectionChanged;
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems)
				{
					var vm = item as DomainPathMapViewModel;
					if (vm != null)
					{
						Models.Add(vm.Model);
					}
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (var item in e.OldItems)
				{
					var vm = item as DomainPathMapViewModel;
					if (vm != null)
					{
						Models.Remove(vm.Model);
					}
				}
			}
		}


	}
}
