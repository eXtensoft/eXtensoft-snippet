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
    public class PathNodeViewModel : ViewModel<PathNode>, IPathNode
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
				//foreach (PathNodeViewModel item in Items)
				//{
				//	item.Path = $"{Model.Path}/{item.Slug}";
				//}
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
				//Path = (Master != null) ? $"{Master.Path}/{Model.Slug}" : value;
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
			//model.Path = $"{master.Slug}/{model.Slug}";
			Model = model;
			if (model.Items != null)
			{
				Items = new ObservableCollection<PathNodeViewModel>(from x in model.Items select new PathNodeViewModel(x, this));
				Items.CollectionChanged += Items_CollectionChanged;
			}
		}


		public PathNodeViewModel(PathNodeViewModel master, string path, string slug = "")
        {
			Master = master;
			Items = new ObservableCollection<PathNodeViewModel>();
			Items.CollectionChanged += Items_CollectionChanged;
			var token = !String.IsNullOrWhiteSpace(slug) ? slug : path.TrimStart('/');
			Model = new PathNode()
			{
				Display = token.ToTitleCase(),
				Slug = token,
				Path = $"{master.Slug}/{token}"
			};
            if (!String.IsNullOrWhiteSpace(slug))
            {
				if (path.StripSlug(out string nextSlug, out string next))
				{
					//Items.Add(new PathNodeViewModel(string.Empty,))
				}
				else
				{
					Items.Add(new PathNodeViewModel(this, path));
				}
            }


        }

		internal void EnsurePath(string path)
		{
			if (path.StripSlug(out string slug, out string next))
			{
				var found = Items.FirstOrDefault(y => y.Slug.Equals(slug));
                if (found == null)
                {
					var token = Path.Equals("/virtual") ? $"/{slug}" : $"{Path}/{slug}";

					found = new PathNodeViewModel(new PathNode() 
					{ 
						Display = slug,
						Path = token,
						Slug = slug
					},this);
					Items.Add(found);
                }
				found.EnsurePath(next);
			}
            else if (!Path.Equals(path, StringComparison.OrdinalIgnoreCase))
			{
				var p = path.TrimStart('/');
				var t = Path.Equals("/virtual") ? path : $"{Path}/{p}";
				var token = Master != null && !Master.Path.Equals("/virtual") ? $"{Master.Path}/{Slug}/{p}" : t;
                Items.Add(new PathNodeViewModel(new PathNode()
				{
					Display = p,
					Path = token,
					Slug = p
				},this)); 
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





	}
}
