using Bitsmith.DataServices.Abstractions;
using Bitsmith.ProjectManagement;
using Bitsmith.Styx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class StyxModule : Module
    {
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


        public GraphDesigner Model { get; set; }

        public ObservableCollection<GraphDesignViewModel> Designs { get; set; }

        private GraphDesignViewModel _SelectedDesign;
        public GraphDesignViewModel SelectedDesign
        {
            get { return _SelectedDesign; }
            set
            {
                _SelectedDesign = value;
                OnPropertyChanged("SelectedDesign");
            }
        }
        public ObservableCollection<GraphTemplateViewModel> Templates { get; set; }

        private GraphTemplateViewModel _SelectedTemplate;
        public GraphTemplateViewModel SelectedTemplate
        {
            get
            {
                return _SelectedTemplate;
            }
            set
            {
                _SelectedTemplate = value;
                OnPropertyChanged("SelectedTemplate");
            }
        }



        public StyxModule(IDataService dataService)
        {
            DataService = dataService;
            //Filepath = Path.Combine(AppConstants.StyxDirectory, DataService.Filepath<GraphDesigner>());
        }

        protected override string Filepath()
        {
            return Path.Combine(AppConstants.StyxDirectory, DataService.Filepath<GraphDesigner>());
        }
        public override void Initialize()
        {
            Designs = new ObservableCollection<GraphDesignViewModel>(from x in Model.Designs select new GraphDesignViewModel(x));
            Designs.CollectionChanged += Designs_CollectionChanged;

            Templates = new ObservableCollection<GraphTemplateViewModel>(from x in Model.Templates select new GraphTemplateViewModel(x));
            Designs.CollectionChanged += Templates_CollectionChanged;
        }

        private void Templates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as GraphTemplateViewModel;
                    if (vm != null)
                    {
                        Model.Templates.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as GraphTemplateViewModel;
                    if (vm != null)
                    {
                        Model.Templates.Remove(vm.Model);
                    }
                }
            }
        }

        private void Designs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as GraphDesignViewModel;
                    if (vm != null)
                    {
                        Model.Designs.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as GraphDesignViewModel;
                    if (vm != null)
                    {
                        Model.Designs.Remove(vm.Model);
                    }
                }
            }
        }

        //public bool CanSaveWorkspace()
        //{
        //    return Model != null;
        //}

        //public void SaveWorkspace()
        //{
        //    SaveData();
        //}

        protected override bool LoadData()
        {
            string filepath = Filepath();
            if (!File.Exists(filepath))
            {
                GraphDesigner designer = new GraphDesigner().Default();
                if (!DataService.TryWrite<GraphDesigner>(designer, out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = DataService.TryRead<GraphDesigner>(filepath, out GraphDesigner model, out string message);
            if (!b)
            {
                OnFailure(message);
            }
            else
            {
                Model = model;
            }

            return b;
        }

        protected override bool SaveData()
        {
            bool b = true;
            if (Model != null)
            {
                if (!DataService.TryWrite<GraphDesigner>(Model, out string message, Filepath()))
                {
                    OnFailure(message);
                    b = false;
                }
            }
            return b;
        }

    }
}
