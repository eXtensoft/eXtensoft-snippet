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
                return Model.QueryType.ToString();
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



        public QueryViewModel(Query model)
        {
            Model = model;
        }


    }
}
