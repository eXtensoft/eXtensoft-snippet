using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class VirtualPathModule : Module<DomainPathMap>
    {

        private ICommand _ViewPathDomainsCommand;
		public ICommand ViewPathDomainsCommand
		{
			get
			{
				if (_ViewPathDomainsCommand == null)
				{
					_ViewPathDomainsCommand = new RelayCommand(
					param => ViewPathDomains(),
					param => CanViewPathDomains());
				}
				return _ViewPathDomainsCommand;
			}
		}
		private bool CanViewPathDomains()
		{
			return true;
		}
		private void ViewPathDomains()
		{
			Control ctl = new PathsView();
			ctl.DataContext = this;
			dynamic param = new System.Dynamic.ExpandoObject();
			param.Title = "Content Domains";
			param.Control = ctl;
			Workspace.Instance.ViewModel.Overlay.SetOverlay(AppConstants.OverlayContent, param);
		}



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

		private IPathNode _SelectedNode;
		public IPathNode SelectedNode
        {
            get { return _SelectedNode; }
            set
            {
				_SelectedNode = value;
				OnPropertyChanged("SelectedNode");
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

        public VirtualPathModule(IDataService dataService)
        {

			DataService = dataService;
        }
        public override string Filepath()
        {
            return Path.Combine(AppConstants.SettingsDirectory, DataService.Filepath<DomainPathMap>());
        }

        protected override bool LoadData()
        {
			//if (!File.Exists(Filepath))
   //         {
   //             List<DomainPathMap> list = new List<DomainPathMap>().Default();
   //             DataService.TryWrite<DomainPathMap>(list, out string message, Filepath);
   //         }

            return base.LoadData();
        }

        public override void Initialize()
        {
            Items = new ObservableCollection<DomainPathMapViewModel>( from x in Models select new DomainPathMapViewModel(x));
			Items.CollectionChanged += Items_CollectionChanged;
            if (Items.Any())
            {
				SelectedDomain = Items[0];
            }
			
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

		internal void Build(SettingsModule settings, List<ContentItem> items)
		{
			List<DomainViewModel> list = new List<DomainViewModel>();
			List<DomainPathMapViewModel> additions = new List<DomainPathMapViewModel>();
			foreach (var domain in settings.Settings.Domains)
			{
				var item = Items.FirstOrDefault(p => p.Id.Equals(domain.Id, StringComparison.OrdinalIgnoreCase));
				if (item == null)
				{
					item = new DomainPathMapViewModel(new DomainPathMap()
					{
						Id = domain.Id,
						Display = domain.Name,
						Slug = domain.Name.ToLower(),
						Path = domain.Name.ToLower(),
						Items = new List<PathNode>().Default()
					});
					additions.Add(item);
				}
				list.Add(new DomainViewModel(domain, item));
			}
			additions.ForEach(x => { Items.Add(x); });
			Build(items);
			settings.Initialize(list);			
		}

        internal void Build(List<ContentItem> items)
		{
			Dictionary<string, List<string>> d = new Dictionary<string, List<string>>();
			Dictionary<string, HashSet<string>> hs = new Dictionary<string, HashSet<string>>();
			foreach (var item in items.Where(y => !y.Mime.Equals("text/credential")))
			{
                if (item.HasFile())
                {
					var p = $"/{AppConstants.Paths.Files}/{item.Mime.Trim(new char[] { '.' })}";
                    if (!item.Paths.Contains(p))
                    {
						item.Paths.Add(p);
                    }
                }
				string domain = item.Domain();
				if (!d.ContainsKey(domain))
				{
					d.Add(domain, new List<string>());
					hs.Add(domain, new HashSet<string>());
				}
				foreach (var path in item.Paths)
				{
					string s = path.Trim().ToLower();
					if (hs[domain].Add(s))
					{
						d[domain].Add(s);
					}
				}

				if (item.HasFile())
				{
					var filetype = item.Mime.Trim(new char[] { '.' });
					var p = $"/{AppConstants.Paths.Files}/{filetype}";
                    if (hs[domain].Add(p))
                    {
						d[domain].Add(p);
                    }
				}

			}
			foreach (var domain in d.Keys)
			{
				var found = Items.FirstOrDefault(x => x.Id.Equals(domain));
				if (found != null)
				{
					d[domain].Sort();
					foreach (var path in d[domain])
					{
						found.EnsurePath(path);
					}
				}
			}

		}


		internal void Ensure(ContentItem item)
        {
			string domain = item.Domain();
			var found = Items.FirstOrDefault(x => x.Id.Equals(domain));
			if (found != null)
			{
				foreach (var path in item.Paths)
				{
					found.EnsurePath(path);
				}
			}

        }
    
	
	}
}
