﻿using Bitsmith.BusinessProcess;
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
                Model.OriginState = value.Name;
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
                Model.DestinationState = value.Name;
                Name = value.Name;
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


        private WorkflowViewModel _Workflow;
        public TransitionViewModel(Transition model, WorkflowViewModel workflow)
        {
            Model = model;
            States = workflow.States;
            var from = States.FirstOrDefault(x => x.Name.Equals(Model.OriginState));
            if (from != null)
            {
                _From = from;
            }
            var to = States.FirstOrDefault(x => x.Name.Equals(Model.DestinationState));
            if (to != null)
            {
                _To = to;
            }
            _Workflow = workflow;
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
