using Bitsmith.Models;
using System;

namespace Bitsmith.ViewModels
{
    public class QueryViewModel : ViewModel<Query>
    {
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
