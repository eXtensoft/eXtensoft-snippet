using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class ListItemViewModel : ViewModel<ListItem>
    {
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public string DatastoreId
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
                OnPropertyChanged("DatastoreId");
            }
        }


        public string Id
        {
            get
            {
                return Model.Identifier.Id;
            }
            set
            {
                Model.Identifier.Id = value;
                OnPropertyChanged("Id");
            }
        }


        public string Display
        {
            get
            {
                return Model.Identifier.Display;
            }
            set
            {
                Model.Identifier.Display = value;
                OnPropertyChanged("Display");
            }
        }


        public string Token
        {
            get
            {
                return Model.Identifier.Token;
            }
            set
            {
                Model.Identifier.Token = value;
                OnPropertyChanged("Token");
            }
        }


        public string MasterId
        {
            get
            {
                return Model.Identifier.MasterId;
            }
            set
            {
                Model.Identifier.MasterId = value;
                OnPropertyChanged("MasterId");
            }
        }



        public string Group
        {
            get
            {
                return Model.Group;
            }
            set
            {
                Model.Group = value;
                OnPropertyChanged("Group");
            }
        }

        public string Key
        {
            get
            {
                return Model.Key;
            }
            set
            {
                Model.Key = value;
                OnPropertyChanged("Key");
            }
        }

        private string _Value;
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                OnPropertyChanged("Value");
            }
        }

        public int Sort
        {
            get
            {
                return Model.Sort;
            }
            set
            {
                Model.Sort = value;
                OnPropertyChanged("Sort");
            }
        }





        public ListItemViewModel(ListItem model)
        {
            Model = model;
        }
    }
}
