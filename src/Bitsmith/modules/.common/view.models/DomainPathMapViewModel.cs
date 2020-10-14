using Bitsmith.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
	public class DomainPathMapViewModel : ViewModel<DomainPathMap>, IPathNode
    {

		public string Id
		{
			get
			{
				return Model.Id;
			}
			set
			{
				Model.Id = value;
				OnPropertyChanged("Id");
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
				Path = value;
				OnPropertyChanged("Slug");
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
			Items.Add(new PathNodeViewModel(new PathNode() { Display = "Path", Slug = "path", Path = $"path" })); ;
		}



		public DomainPathMapViewModel(DomainPathMap model)
        {
            Model = model;
			if (model.Items != null)
			{
				Items = new ObservableCollection<PathNodeViewModel>(from x in model.Items select new PathNodeViewModel(x));
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

        internal void EnsurePath(string path)
        {
			var pattern = string.Empty;
			var next = path;
            if (path.StripSlug(out string slug, out next) && 
				slug.Equals(AppConstants.Paths.Files))
            {
				pattern = $"/{AppConstants.Paths.Files}";
			}
            else if(path.Equals($"/{AppConstants.Paths.Content}"))
            {
				pattern = $"/{AppConstants.Paths.Content}";
			}
            else
            {
				pattern = $"/{AppConstants.Paths.Default}";
				next = path;
            }

            
			var found = Items.FirstOrDefault(y => y.Path.Equals(pattern));
            if (found == null)
            {
				found = new PathNodeViewModel(new PathNode() 
				{ 
					Path = pattern, 
					Slug = pattern.TrimStart('/'), 
					Display = pattern.TrimStart('/').ToTitleCase() 
				});
				Items.Add(found);
            }
            if (!string.IsNullOrWhiteSpace(next))
            {
				found.EnsurePath(next);
            }
            

        }
    }
}
