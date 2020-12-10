using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bitsmith.BusinessProcess;

namespace Bitsmith.ViewModels
{
    public class WorkflowViewModel : ViewModel<Workflow>
    {

        private string _DslBody;
        public string DslBody
        {
            get
            {
                return _DslBody;
            }
            set
            {
                _DslBody = value;
                OnPropertyChanged("DslBody");
            }
        }

        private ICommand _ToDslCommand;
        public ICommand ToDslCommand
        {
            get
            {
                if (_ToDslCommand == null)
                {
                    _ToDslCommand = new RelayCommand(
                    param => ToDsl(),
                    param => CanToDsl());
                }
                return _ToDslCommand;
            }
        }
        private bool CanToDsl()
        {
            return true;
        }
        private void ToDsl()
        {
            if (Validate())
            {

            }
            DslBody = Model.Machine.ToDsl(Model);
        }

        private ICommand _FromDslCommand;
        public ICommand FromDslCommand
        {
            get
            {
                if (_FromDslCommand == null)
                {
                    _FromDslCommand = new RelayCommand(
                    param => FromDsl(),
                    param => CanFromDsl());
                }
                return _FromDslCommand;
            }
        }
        private bool CanFromDsl()
        {
            return !string.IsNullOrWhiteSpace(_DslBody);
        }
        private void FromDsl()
        {
            if (DslBody.TryBuildWorkflow(out Workflow workflow))
            {

            }


        }

        //private WorkflowBuilder TryParse(string textToParse)
        //{
        //    WorkflowBuilder builder = new WorkflowBuilder() { TextToParse = "" , IsSuccess = false};

        //    builder.Workflow = new Workflow();
        //    return builder;
        //}


        //public class WorkflowBuilder
        //{
        //    public bool IsSuccess { get; set; }
        //    public Workflow Workflow { get; set; }
        //    public string TextToParse { get; set; }

           
        //}


        public ObservableCollection<StateViewModel> States { get; set; }

        public ObservableCollection<TransitionViewModel> Transitions { get; set; }

        private ICommand _AddStateCommand;
        public ICommand AddStateCommand
        {
            get
            {
                if (_AddStateCommand == null)
                {
                    _AddStateCommand = new RelayCommand(
                    param => AddState(),
                    param => CanAddState());
                }
                return _AddStateCommand;
            }
        }
        private bool CanAddState()
        {
            return Model.Machine != null && Model.Machine.States != null;
        }
        private void AddState()
        {
            States.Add(new StateViewModel(new State().Default(),this));
        }


        private ICommand _AddTransitionCommand;
        public ICommand AddTransitionCommand
        {
            get
            {
                if (_AddTransitionCommand == null)
                {
                    _AddTransitionCommand = new RelayCommand(
                    param => AddTransition(),
                    param => CanAddTransition());
                }
                return _AddTransitionCommand;
            }
        }
        private bool CanAddTransition()
        {
            return Model.Machine != null && Model.Machine.Transitions != null;
        }
        private void AddTransition()
        {
            Transitions.Add(new TransitionViewModel(new Transition().Default(),this));
        }



        public string Id
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
                OnPropertyChanged("Id");
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
                OnPropertyChanged("Display");
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



        private string _StateDisplay;
        public string StateDisplay
        {
            get
            {
                return $"{Model.Machine.Display}:{Model.Machine.CurrentState}";
            }
            set
            {
                _StateDisplay = value;
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
            //_Selections.AddRange(Transitions);
            _Selected = _Selections.FirstOrDefault(x => x.Name.Equals(Model.Machine.CurrentState));
        }



        public WorkflowStep CurrentState
        {
            get { return Model.Machine.GetCurrentState().ToStep(); }
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
            Model.Machine.ExecuteTransition(name);
            ResolveSelections();
            History += "\r\n\t" + Model.Machine.CurrentState;
            if (Model.Machine.IsEnd())
            {
                History += "\r\n" + "end";
            }
        }

        //public List<WorkflowStep> Transitions
        //{
        //    get 
        //    { 
        //        return Model.Machine.GetTransitions().ToSteps().ToList(); 
        //    }

        //}



        private WorkflowViewModel(StateMachine machine, string currentState = "")
        {
            ////Machine = machine;
            ////Machine.SetState(currentState);
            ////ResolveSelections();
            ////History = "begin";
            ////History += "\r\n\t" + Machine.CurrentState;
        }
        public WorkflowViewModel(Workflow model)
        {
            Model = model;
            States = new ObservableCollection<StateViewModel>(from x in Model.Machine.States select new StateViewModel(x,this));
            Transitions = new ObservableCollection<TransitionViewModel>(from x in Model.Machine.Transitions select new TransitionViewModel(x, this));

            States.CollectionChanged += States_CollectionChanged;
            Transitions.CollectionChanged += Transitions_CollectionChanged;

            //Machine.SetState("");
            //ResolveSelections();

        }

        private void States_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as StateViewModel;
                    if (vm != null)
                    {
                        Model.Machine.States.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as StateViewModel;
                    if (vm != null)
                    {
                        Model.Machine.States.Remove(vm.Model);
                    }
                }
            }
        }

        private void Transitions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as TransitionViewModel;
                    if (vm != null)
                    {
                        Model.Machine.Transitions.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as TransitionViewModel;
                    if (vm != null)
                    {
                        Model.Machine.Transitions.Remove(vm.Model);
                    }
                }
            }
        }





    }
}
