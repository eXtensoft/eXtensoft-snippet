using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Concurrent;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Bitsmith
{
    public class StateManager : INotifyPropertyChanged
    {
        private bool _IsInitialized = false;
        private List<string> _TransitionNames = new List<string>();
        private ConcurrentDictionary<string, bool> _CanTransitionFlags = new ConcurrentDictionary<string, bool>();

        public bool IsShowNavMenu { get; set; }
        //private Dictionary<string, UserControl> _Views = new Dictionary<string, UserControl>();

        public ObservableCollection<NavMenuItem> Menu { get; set; }

        #region Commands

        private ICommand _ExecuteTransitionCommand;

        public ICommand ExecuteTransitionCommand
        {
            get
            {
                if (_ExecuteTransitionCommand == null)
                {
                    _ExecuteTransitionCommand = new RelayCommand<string>(ExecuteTransition, CanExecuteTransition);
                }
                return _ExecuteTransitionCommand;
            }
        }

        #endregion

        #region CurrentState (string)
        /// <summary>
        /// Gets or sets the string value for CurrentState
        /// </summary>
        /// <value> The string value.</value>
        private string _CurrentState;
        public string CurrentState
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(_CurrentState))
                {
                    _CurrentState = Machine.GetCurrentState().Display;

                    // this is the key to it all;  if ShowNavigation
                    // then mark all NavMenuItems appropriately
                }
                return _CurrentState;
            }
            set
            {
                _CurrentState = value;
                OnPropertyChanged("CurrentState");
            }
        }

        #endregion

        #region Transitions (List<Transition>)
        /// <summary>
        /// Gets or sets the List<Transition> value for Transitions
        /// </summary>
        /// <value> The List<Transition> value.</value>

        public List<Transition> Transitions
        {
            get { return Machine.GetTransitions(); }

        }

        #endregion

        //public Transition[] EndingTransition
        //{
        //    get { return Machine.EndingTransitions; }
        //}


        public StateMachine Machine { get; set; }

        public StateManager() { }


        #region helper methods

        public void ExecuteTransition(string transitionName)
        {
            Machine.ExecuteTransition(transitionName);
            if (_IsInitialized)
            {
                RefreshTransitionFlags();
            }
            var currentstate = Machine.GetCurrentState();
            foreach (var menuitem in Menu)
            {
                menuitem.IsCurrent = menuitem.Display.Equals(currentstate.Name, StringComparison.OrdinalIgnoreCase);
            }
            CurrentState = string.Empty;
            OnPropertyChanged("CurrentState");
            OnPropertyChanged("Transitions");
            OnPropertyChanged("EndingTransition");
        }

        public void TransitionTo(string stateName)
        {
            var transition = Machine.GetTransitions().FirstOrDefault(t => t.DestinationState.Equals(stateName, StringComparison.OrdinalIgnoreCase));        
            if (transition != null)
            {
                ExecuteTransition(transition.Name);
            }
        }

        public bool CanTransitionTo(string stateName)
        {
            var transition = Machine.GetTransitions().FirstOrDefault(t => t.DestinationState.Equals(stateName, StringComparison.OrdinalIgnoreCase));
            return transition != null;
        }

        private bool CanExecuteTransition(string transitionName)
        {
            return Machine.CanExecuteTransition(transitionName);
        }

        #endregion

        #region instance methods

        public void RegisterEndpointAction(string stateKey, EndpointOption option, params Action[] actions)
        {
            var found = Machine.States.FirstOrDefault(x => x.Name.Equals(stateKey, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                if (found.EndpointActions == null)
                {
                    found.EndpointActions = new List<IEndpointAction>();
                }
                found.EndpointActions.Add(new EndpointAction(option, actions));
            }
        }

        public void RegisterTransitionAction(string transitionKey, EndpointOption option, params Action[] actions)
        {
            var found = Machine.States.FirstOrDefault(x => x.Name.Equals(transitionKey, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                if (found.EndpointActions == null)
                {
                    found.EndpointActions = new List<IEndpointAction>();
                }
                found.EndpointActions.Add(new EndpointAction(option, actions));
            }
        }

        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Initialize()
        {
            if (!_IsInitialized)
            {
                Machine.Transitions.DistinctBy(x => x.Name).ToList().ForEach(t =>
                {
                    _CanTransitionFlags.TryAdd(t.Name, false);
                    _TransitionNames.Add(t.Name);
                });
                RefreshTransitionFlags();
                _IsInitialized = true;
            }
        }

        private void RefreshTransitionFlags()
        {
            // re-assign flag values based upon transitions 
            // available given the current state
            var available = from a in Machine.GetTransitions() select a.Name;
            _TransitionNames.ForEach(t =>
            {
                bool b = available.Contains(t);
                _CanTransitionFlags.AddOrUpdate(t, b,(key,oldValue) => b);
            });          
        }


        public bool CanTransition(string transitionName)
        {
            //return true;
            //bool b = false;
            //if (_IsInitialized &&
            //    !string.IsNullOrWhiteSpace(transitionName) &&
            //    _CanTransitionFlags.TryGetValue(transitionName, out bool canTransition))
            //{
            //    if (canTransition)
            //    {

            //    }
            //    b = canTransition;
            //}

            //return b;
            return Machine.CanExecuteTransition(transitionName);
        }

        #endregion
    }
}
