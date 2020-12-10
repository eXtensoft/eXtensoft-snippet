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
				return SearchTerms.Count > 0;
            }
            set { }
        }

		private List<string> _SearchTerms = new List<string>();
		public List<string> SearchTerms
        {
            get
            {
				return _SearchTerms;
            }
            set
            {
				_SearchTerms = value;
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
				Model.LastUpdated();
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
				Model.LastUpdated();
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
				Model.LastUpdated();
			}
		}

		public List<string> Paths
        {
            get
            {
				return Model.Paths;
            }
            set
            {
				Model.Paths = value;
				OnPropertyChanged("Paths");
				Model.LastUpdated();
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
				Model.LastUpdated();
			}
		}


		//private string _HoverDisplay;
		public string HoverDisplay
        {
            get
            {
				StringBuilder sb = new StringBuilder();
				Model.Properties.ForEach(p => { sb.AppendLine(p.ToString()); });
				return sb.ToString();
            }
            set { }
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
