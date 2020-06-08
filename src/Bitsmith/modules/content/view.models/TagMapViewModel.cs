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


        public int Count
        {
            get
            {
                return Model.Count;
            }
            set
            {
                Model.Count = value;
                OnPropertyChanged("Count");
            }
        }


        public TagMapViewModel(TagMap model)
        {
            Model = model;
        }
    }
}
