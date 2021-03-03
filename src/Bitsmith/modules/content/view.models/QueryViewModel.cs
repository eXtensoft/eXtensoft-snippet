using Bitsmith.Models;
using System;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class QueryViewModel : ViewModel<Query>
    {

        private ICommand _CycleQueryTypeCommand;
        public ICommand CycleQueryTypeCommand
        {
            get
            {
                if (_CycleQueryTypeCommand == null)
                {
                    _CycleQueryTypeCommand = new RelayCommand(
                    param => CycleQueryType(),
                    param => CanCycleQueryType());
                }
                return _CycleQueryTypeCommand;
            }
        }
        private bool CanCycleQueryType()
        {
            return true;
        }
        private void CycleQueryType()
        {
            switch (Model.QueryType)
            {
                case QueryTypeOption.None:
                    break;
                case QueryTypeOption.Recent:
                    Model.QueryType = QueryTypeOption.Favorite;
                    break;
                case QueryTypeOption.Favorite:
                    Model.QueryType = QueryTypeOption.None;
                    break;
                case QueryTypeOption.Named:
                    break;
                default:
                    break;
            }
            OnPropertyChanged("QueryType");
        }

        public string QueryText
        {
            get
            {
                return Model.ToQueryText();
            }
            set { }
        }

        public string Display
        {
            get
            {
                return Model.ToString();

            }
            set
            {
                Model.Display = value;
                OnPropertyChanged("Display");
            }
        }



        public string QueryType
        {
            get
            {
                return $"querytype-{Model.QueryType.ToString().ToLower()}";
            }
            set
            {

            }
        }

        public string SearchType 
        {
            get
            {
                return $"querytype-search-{(int)Model.GetSearchType()}";
            }
            set { }
        
        }

        public string Operator
        {
            get
            {
                return Model.Operator.ToString();
            }
            set
            {

            }
        }



        public string Domain
        {
            get
            {
                return Model.Domain;
            }
            set
            {
                Model.Domain = value;
                OnPropertyChanged("Domain");
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

        private string _Hash;
        
        public string Hash
        {
            get
            {
                return _Hash;
            }
            set
            {
                _Hash = value;
                OnPropertyChanged("Hash");
            }
        }

        public QueryViewModel(Query model)
        {   
            Model = model;
            _Hash = Model.GetHash();

        }

    }
}
