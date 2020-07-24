using Bitsmith.BusinessProcess;
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
                _Workflow.SetBeginState(Model.Name);
            }
        }

        public void SetBegin(bool isBegin)
        {
            if (_IsBegin != isBegin)
            {
                _IsBegin = isBegin;
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
                if (_IsEnd != value)
                {
                    _IsEnd = value;
                    if (value)
                    {

                        _Workflow.AddEndState(Model.Name);
                    }
                    else
                    {
                        _Workflow.RemoveEndState(Model.Name);
                    }

                    OnPropertyChanged("IsEnd");
                }
            }
        }
        public void SetEnd(bool isEnd)
        {
            if (_IsEnd != isEnd)
            {
                _IsEnd = isEnd;
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






        private WorkflowViewModel _Workflow;

        public StateViewModel(State model, WorkflowViewModel workflow)
        {
            Model = model;
            _Workflow = workflow;
            if (Model.Name.Equals(_Workflow.Model.Machine.BeginState))
            {
                _IsBegin = true;
            }
            if (_Workflow.Model.Machine.EndStates.Contains(Model.Name))
            {
                _IsEnd = true;
            }
        }

        public StateViewModel(State model)
        {
            Model = model;
        }
    }
}
