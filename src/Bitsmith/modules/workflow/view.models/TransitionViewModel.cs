using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class TransitionViewModel : ViewModel<Transition>
    {
        public ObservableCollection<StateViewModel> States { get; set; }

        private StateViewModel _From;
        public StateViewModel From
        {
            get
            {
                return _From;
            }
            set
            {
                _From = value;
                Display = ToString();
                OnPropertyChanged("From");
            }
        }

        private StateViewModel _To;
        public StateViewModel To
        {
            get { return _To; }
            set
            {
                _To = value;
                Name = _To.Name;
                Display = ToString();
                OnPropertyChanged("To");
                OnPropertyChanged("Monker");
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
                OnPropertyChanged("Moniker");
            }
        }

        public string Moniker
        {
            get { return ToString(); }
            set { }
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


        public string OriginState
        {
            get
            {
                return Model.OriginState;
            }
            set
            {
                Model.OriginState = value;
                OnPropertyChanged("OriginState");
            }
        }

        public string DestinationState
        {
            get
            {
                return Model.DestinationState;
            }
            set
            {
                Model.DestinationState = value;
                OnPropertyChanged("DestinationState");
            }
        }


        public int SortOrder
        {
            get
            {
                return Model.SortOrder;
            }
            set
            {
                Model.SortOrder = value;
                OnPropertyChanged("SortOrder");
            }
        }



        public TransitionViewModel(Transition model, ObservableCollection<StateViewModel> states)
        {
            Model = model;
            States = states;
        }

        public override string ToString()
        {
            var from = _From != null ? _From.Name : "{from}";
            var to = _To != null ? _To.Name : "{to}";
            return $"{from}->{to}";
        }
    }
}
