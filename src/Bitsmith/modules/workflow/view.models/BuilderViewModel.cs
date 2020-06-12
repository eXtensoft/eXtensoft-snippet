using Bitsmith.BusinessProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class BuilderViewModel : ViewModel<Builder>
    {
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


        

        public ObservableCollection<StateViewModel> States { get; set; }

        public ObservableCollection<TransitionViewModel> Transitions { get; set; }



        public BuilderViewModel()
        {
            Model = new Builder().Default();
            States = new ObservableCollection<StateViewModel>();
            Transitions = new ObservableCollection<TransitionViewModel>();
            Setup();
        }

        public BuilderViewModel(Builder model)
        {
            States = new ObservableCollection<StateViewModel>(from x in model.States select new StateViewModel(x));
            Transitions = new ObservableCollection<TransitionViewModel>(from x in model.Transitions select new TransitionViewModel(x,States));
            Setup();
        }

        private void Setup()
        {
            States.CollectionChanged += States_CollectionChanged;
            Transitions.CollectionChanged += Transitions_CollectionChanged;
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
                        Model.States.Add(vm.Model);
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
                        Model.States.Remove(vm.Model);
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
                        Model.Transitions.Add(vm.Model);
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
                        Model.Transitions.Remove(vm.Model);
                    }
                }
            }
        }



        public override bool Validate()
        {
            bool b = base.Validate();
            b = b ? States.Count >= 3 : b;
            b = b ? Transitions.Count >= 2 : b;
            if (b)
            {
                foreach (var t in Transitions)
                {
                    if (t.From != null && t.To != null)
                    {

                    }
                    else
                    {
                        b = false;
                        break;
                    }
                }
            }

            
            return b;
        }
    }
}
