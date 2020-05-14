using Bitsmith.Models;
using System;

namespace Bitsmith.ViewModels
{
	public class DomainViewModel : ViewModel<Domain>
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

		public DomainViewModel(Domain model)
		{
			Model = model;
		}

	}
}
