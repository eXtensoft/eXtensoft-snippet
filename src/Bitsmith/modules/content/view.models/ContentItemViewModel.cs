using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class ContentItemViewModel : ViewModel<ContentItem>
    {

        public bool IsRemove
        {
            get
            {
                return Model.IsRemove;
            }
            set
            {
                Model.IsRemove = value;
                OnPropertyChanged("IsRemove");
            }
        }


        public bool IsFullTextSearch
        {
            get
            {
				return !String.IsNullOrWhiteSpace(_SearchTerm);
            }
            set { }
        }
		private string _SearchTerm;
		public string SearchTerm
        {
            get
            {
				return _SearchTerm;
            }
            set
            {
				_SearchTerm = value;
            }
        }
		public string ContentType
		{
			get
			{
				return Mime;
			}
			set { }
		}
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
			}
		}

		public string Mime
		{
			get
			{
				return Model.Mime;
			}
			set
			{
				Model.Mime = value;
				OnPropertyChanged("Mime");
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

		public string Body
		{
			get
			{
				return Model.Body;
			}
			set
			{
				Model.Body = value;
				OnPropertyChanged("Body");
			}
		}


		private DateTime _UpdatedAt;
		public DateTime UpdatedAt
		{
			get
			{
				return _UpdatedAt;
			}
			set
			{

				OnPropertyChanged("UpdatedAt");
			}
		}


		public List<Property> Tags
		{
			get
			{
				return Model.Properties.Tags();
			}
			set
			{
				Model.Properties.Coalesce(value);
				OnPropertyChanged("Tags");
			}
		}



		public ContentItemViewModel(ContentItem model)
        {
			Model = model;
			var modified = model.Properties.FirstOrDefault(x => x.Name.Equals($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}"));
            if (modified != null && 
				modified.Value != null && 
				DateTime.TryParse(modified.Value.ToString(), out _UpdatedAt))
            {
			
            }
            else
            {
				_UpdatedAt = DateTime.Now;
            }
			//var found = model.Properties.FirstOrDefault(x => x.Name.Equals($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}"));

		}

    }
}
