using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class MimeMapViewModel : ViewModel<MimeMap>
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

		public string Extension
		{
			get
			{
				return Model.Extension;
			}
			set
			{
				Model.Extension = value;
				OnPropertyChanged("Extension");
			}
		}

		public string GroupName
		{
			get
			{
				return Model.GroupName;
			}
			set
			{
				Model.GroupName = value;
				OnPropertyChanged("GroupName");
			}
		}

		public string Image
		{
			get
			{
				return Model.Image;
			}
			set
			{
				Model.Image = value;
				OnPropertyChanged("Image");
			}
		}

		public string View
		{
			get
			{
				return Model.View;
			}
			set
			{
				Model.View = value;
				OnPropertyChanged("View");
			}
		}


		public MimeMapViewModel(MimeMap model)
        {
            Model = model;
        }
    }
}
