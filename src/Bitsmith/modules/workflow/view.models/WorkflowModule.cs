using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bitsmith.BusinessProcess;
using Newtonsoft.Json;

namespace Bitsmith.ViewModels
{
    public class WorkflowModule : Module
    {

        private WorkflowViewModel _Workflow;
        public WorkflowViewModel Workflow
        {
            get
            {
                return _Workflow;
            }
            set
            {
                _Workflow = value;
                OnPropertyChanged("Workflow");
            }
        }


        private StateMachine _Machine;
        public StateMachine Machine 
        {
            get { return _Machine; }
            
            set
            {
                _Machine = value;
                OnPropertyChanged("Machine");
            }
        }

        private ICommand _UploadStateMachineCommand;
        public ICommand UploadStateMachineCommand
        {
            get
            {
                if (_UploadStateMachineCommand == null)
                {
                    _UploadStateMachineCommand = new RelayCommand(
                    param => UploadStateMachine(),
                    param => CanUploadStateMachine());
                }
                return _UploadStateMachineCommand;
            }
        }
        private bool CanUploadStateMachine()
        {
            return true;
        }
        private void UploadStateMachine()
        {
            bool b = false;
            if (FileSystemDataProvider.TryLocateFile(out FileInfo info))
            {
                try
                {
                    if (info.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        Machine = GenericObjectManager.ReadGenericItem<StateMachine>(info.FullName);
                        b = true;
                    }
                    else if (info.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        string text = File.ReadAllText(info.FullName);
                        Machine = JsonConvert.DeserializeObject<StateMachine>(text);
                        b = true;
                    }
                    if (b)
                    {
                        //Machine.SetState();
                        //Workflow = new WorkflowViewModel(Machine);
                    }

                }
                catch (Exception ex)
                {
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    MessageBox.Show(message);
                }
            }

        }


        private ICommand _InloadStateMachineCommand;
        public ICommand InloadStateMachineCommand
        {
            get
            {
                if (_InloadStateMachineCommand == null)
                {
                    _InloadStateMachineCommand = new RelayCommand(
                    param => InloadStateMachine(),
                    param => CanInloadStateMachine());
                }
                return _InloadStateMachineCommand;
            }
        }
        private bool CanInloadStateMachine()
        {
            return Machine == null;
        }
        private void InloadStateMachine()
        {
            bool b = false;
            if (FileSystemDataProvider.TryLocateFile(out FileInfo info))
            {
                try
                {
                    if (info.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        Machine = GenericObjectManager.ReadGenericItem<StateMachine>(info.FullName);
                        b = true;
                    }
                    else if (info.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        string text = File.ReadAllText(info.FullName);
                        Machine = JsonConvert.DeserializeObject<StateMachine>(text);
                        b = true;
                    }
                    if (b)
                    {
                        Machine.SetState();
                        Workflow = new WorkflowViewModel(Machine);
                    }

                }
                catch (Exception ex)
                {
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    MessageBox.Show(message);
                }                
            }

        }

        public string Display { get; set; } = "workflow-module";

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
            return Item != null && Item.Validate();
        }
        private void ToDsl()
        {
            Machine = Item.Build();
            Workflow = new WorkflowViewModel(Machine);
            DslBody = Newtonsoft.Json.JsonConvert.SerializeObject(Machine, Newtonsoft.Json.Formatting.Indented);
            GenericObjectManager.WriteGenericItem<StateMachine>(Machine, $"sm.{Guid.NewGuid().ToString().Substring(0,4)}.xml");
            File.WriteAllText($"sm.{Guid.NewGuid().ToString().Substring(0,4)}.json", DslBody);
        }


        private BuilderViewModel _Item;
        public BuilderViewModel Item 
        {
            get { return _Item; }            
            set
            {
                _Item = value;
                OnPropertyChanged("Item");
            }
        
        }

        private ICommand _AddItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new RelayCommand(
                    param => AddItem(),
                    param => CanAddItem());
                }
                return _AddItemCommand;
            }
        }
        private bool CanAddItem()
        {
            return true;
        }
        private void AddItem()
        {
            var builder = new BuilderViewModel();
            Item = builder;
        }

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
            return Item != null && Item.States != null;
        }
        private void AddState()
        {
            Item.States.Add(new StateViewModel(new State().Default()));
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
            return Item != null && Item.Transitions != null;
        }
        private void AddTransition()
        {
            Item.Transitions.Add(new TransitionViewModel(new Transition().Default(),Item.States));
        }


    }
}
