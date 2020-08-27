using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class TagMapViewModel : ViewModel<TagMap>
    {

        private string _Domain;
        public string Domain
        {
            get { return _Domain; }
            set
            {
                _Domain = value;
                OnPropertyChanged("Domain");
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


        private int _Count;
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
                OnPropertyChanged("Count");
            }
        }


        public TagMapViewModel(TagMap model)
        {
            Model = model;
            _Count = model.Counters.Total();
        }


        public bool SetFilter(string domain, int min = 0)
        {
            Domain = domain;
            _Count = 0;
            var counter = Model.Counters.FirstOrDefault(xx => xx.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase));
            if (counter != null)
            {
                Count = counter.Count;
            }
            return _Count > min;
        }

        public override string ToString()
        {
            return Model.ToString();
        }

    }
}
