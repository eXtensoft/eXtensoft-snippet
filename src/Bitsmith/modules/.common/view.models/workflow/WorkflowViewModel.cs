using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bitsmith.BusinessProcess;

namespace Bitsmith.ViewModels
{
    public class WorkflowViewModel : INotifyPropertyChanged
    {

        private string _History;
        public string History
        {
            get
            {
                return _History;
            }
            set
            {
                _History = value;
                OnPropertyChanged("History");
            }
        }

        private string _Display;
        public string Display
        {
            get
            {
                return  $"{Machine.Display}:{Machine.CurrentState}";
            }
            set
            {
                _Display = value;

            }
        }

        private List<WorkflowStep> _Selections;
        public List<WorkflowStep> Selections
        {
            get
            {
                return _Selections;
            }
            set { }
        }

        private void ResolveSelections()
        {
            _Selections = new List<WorkflowStep>();
            _Selections.Add(CurrentState);
            _Selections.AddRange(Transitions);
            _Selected = _Selections.FirstOrDefault(x => x.Name.Equals(Machine.CurrentState));
        }


        public StateMachine Machine { get; set; }


        public WorkflowStep CurrentState
        {
            get { return Machine.GetCurrentState().ToStep(); }
        }


        private WorkflowStep _Selected;
        public WorkflowStep Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (value != null && value.IsTransition)
                {
                    Transition(value.Name);
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("Display");
                    OnPropertyChanged("Selections");
                }                

            }
        }

        private void Transition(string name)
        {
            Machine.ExecuteTransition(name);
            ResolveSelections();
            History += "\r\n\t" + Machine.CurrentState;
            if (Machine.IsEnd())
            {
                History += "\r\n" + "end";
            }
        }

        public List<WorkflowStep> Transitions
        {
            get 
            { 
                return Machine.GetTransitions().ToSteps().ToList(); 
            }

        }

        

        public WorkflowViewModel(StateMachine machine, string currentState = "")
        {
            Machine = machine;
            Machine.SetState(currentState);
            ResolveSelections();
            History = "begin";
            History += "\r\n\t" + Machine.CurrentState;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
