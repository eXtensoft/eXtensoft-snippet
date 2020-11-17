using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bitsmith.ViewModels
{
	public class DomainViewModel : ViewModel<Domain>
    {
		public DomainPathMapViewModel Item { get; set; }


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

		public DateTime CreatedOn
		{
			get
			{
				return Model.CreatedOn;
			}
			set
			{
				Model.CreatedOn = value;
				OnPropertyChanged("CreatedOn");
			}
		}

		public string Name
		{
			get
			{
				return Model.Name;
			}
			set
			{
				Model.Name = value;
				OnPropertyChanged("Name");
			}
		}

		public ScopeOption Scope
		{
			get
			{
				return Model.Scope;
			}
			set
			{
				Model.Scope = value;
				OnPropertyChanged("Scope");
			}
		}

		private int _MinimumTagCount = -1;
        public int MinimumTagCount
        {
            get
            {
                if (_MinimumTagCount == -1)
                {
					var found = Model.Lists.FirstOrDefault(y => y.Name.Equals("minimum-tag-count"));
                    if (found != null && Int32.TryParse(found.Value.ToString(), out _MinimumTagCount))
                    {

                    }
                    else
                    {
						_MinimumTagCount = 0;
                    }
                }
				return _MinimumTagCount;
            }
            set
            {
				_MinimumTagCount = value;
				var found = Model.Lists.FirstOrDefault(y => y.Name.Equals("minimum-tag-count"));
                if (found == null)
                {
					Model.Lists.Add(new Property() { Name = "minimum-tag-count", Value = value });
                }
                else
                {
					found.Value = value;
                }
				
				OnPropertyChanged("MinumumTagCount");
            }
        }


        public string Display
        {
            get
            {
				return $"{Name} ({Scope.ToString()})";
            }
        }
        public DomainPathMapViewModel Paths { get; internal set; }

		private List<ListItem> _DomainWorkflowSelections = new List<ListItem>();
		public ObservableCollection<ListItemViewModel> DomainWorkflowSelections { get; set; } = new ObservableCollection<ListItemViewModel>();


		public DomainViewModel(Domain model)
		{
			Model = model;
		}

		public DomainViewModel(Domain model, DomainPathMapViewModel item) : this(model)
		{
			Item = item;
		}
	}
}
