using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class StateViewModel : ViewModel<State>
    {

        private bool _IsBegin;
        public bool IsBegin
        {
            get
            {
                return _IsBegin;
            }
            set
            {
                _IsBegin = value;
                OnPropertyChanged("IsBegin");
            }
        }

        private bool _IsEnd;
        public bool IsEnd
        {
            get
            {
                return _IsEnd;
            }
            set
            {
                _IsEnd = value;
                OnPropertyChanged("IsEnd");
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


        public string Display
        {
            get
            {
                return Model.Display;
            }
            set
            {
                Model.Display = value;
                Name = value.ToKebab();
                OnPropertyChanged("Display");
            }
        }










        public StateViewModel(State model)
        {
            Model = model;
        }
    }
}
