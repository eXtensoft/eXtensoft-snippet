using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class PathNodeViewModel : ViewModel<PathNode>
    {
		public PathNodeViewModel Master { get; set; }

		public string Path
		{
			get
			{
				return Model.Path;
			}
			set
			{
				Model.Path = value;
				OnPropertyChanged("Path");
				foreach (PathNodeViewModel item in Items)
				{
					item.Path = $"{Model.Path}/{item.Slug}";
				}
			}
		}

		public string Slug
		{
			get
			{
				return Model.Slug;
			}
			set
			{
				Model.Slug = value;
				Path = (Master != null) ? $"{Master.Path}/{Model.Slug}" : value;
				OnPropertyChanged("Slug");
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
				Slug = Model.Display.ToKebab();
			}
		}

		public ObservableCollection<PathNodeViewModel> Items { get; set; }


		private ICommand _AddPathCommand;
		public ICommand AddPathCommand
		{
			get
			{
				if (_AddPathCommand == null)
				{
					_AddPathCommand = new RelayCommand(
					param => AddPath(),
					param => CanAddPath());
				}
				return _AddPathCommand;
			}
		}
		private bool CanAddPath()
		{
			return true;
		}
		private void AddPath()
		{
			Items.Add(new PathNodeViewModel(new PathNode() { Display = "Path", Slug = "path", Path = $"path" },this)); ;
		}



		public PathNodeViewModel(PathNode model)
        {
            Model = model;
			if (model.Items != null)
			{
				Items = new ObservableCollection<PathNodeViewModel>(from x in model.Items select new PathNodeViewModel(x, this));
				Items.CollectionChanged += Items_CollectionChanged;
			}
        }

		public PathNodeViewModel(PathNode model,PathNodeViewModel master)
		{
			Master = master;
			model.Path = $"{master.Slug}/{model.Slug}";
			Model = model;
			if (model.Items != null)
			{
				Items = new ObservableCollection<PathNodeViewModel>(from x in model.Items select new PathNodeViewModel(x, this));
				Items.CollectionChanged += Items_CollectionChanged;
			}
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems)
				{
					var vm = item as PathNodeViewModel;
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
					var vm = item as PathNodeViewModel;
					if (vm != null)
					{
						Model.Items.Remove(vm.Model);
					}
				}
			}
		}

		public void RefreshPath()
		{

		}

	}
}
