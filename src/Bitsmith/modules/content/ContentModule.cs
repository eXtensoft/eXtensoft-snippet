using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class ContentModule : Module
    {
        public ObservableCollection<DomainViewModel> Domains { get; set; }

        


        private DomainViewModel _SelectedDomain;
        public DomainViewModel SelectedDomain
        {
            get
            {
                return _SelectedDomain;
            }
            set
            {
                _SelectedDomain = value;
                OnPropertyChanged("SelectedDomain");
                //Workspace.Instance.ViewModel.Paths.SelectedDomainId = value.Id;
                //Data.Instance.ViewModel.Paths.SelectedDomainId = value.Id;
            }
        }

        private ICommand _AddDomainCommand;
        public ICommand AddDomainCommand
        {
            get
            {
                if (_AddDomainCommand == null)
                {
                    _AddDomainCommand = new RelayCommand(
                    param => AddDomain(),
                    param => CanAddDomain());
                }
                return _AddDomainCommand;
            }
        }
        private bool CanAddDomain()
        {
            return true;
        }
        private void AddDomain()
        {
            Domains.Add(new DomainViewModel(new Domain().Default(DateTime.Now,Guid.NewGuid().ToString().ToLower())));
        }

        private ICommand _ViewDomainsCommand;
        public ICommand ViewDomainsCommand
        {
            get
            {
                if (_ViewDomainsCommand == null)
                {
                    _ViewDomainsCommand = new RelayCommand(
                    param => ViewDomains(),
                    param => CanViewDomains());
                }
                return _ViewDomainsCommand;
            }
        }
        private bool CanViewDomains()
        {
            return true;
        }
        private void ViewDomains()
        {
            Control ctl = new DomainsView();
            ctl.DataContext = this;
            dynamic param = new System.Dynamic.ExpandoObject();
            param.Title = "Content Domains";
            param.Control = ctl;
            Workspace.Instance.ViewModel.Overlay.SetOverlay(AppConstants.OverlayContent, param);
        }




        public NewContentViewModel Input { get; set; } = new NewContentViewModel();


        private ICommand _AddContentCommand;
        public ICommand AddContentCommand
        {
            get
            {
                if (_AddContentCommand == null)
                {
                    _AddContentCommand = new RelayCommand(
                    param => AddContent(),
                    param => CanAddContent());
                }
                return _AddContentCommand;
            }
        }
        private bool CanAddContent()
        {
            return Input != null ? Input.Validate() : false;
        }
        private void AddContent()
        {
            if (Input.TryBuild(Resolver, 
                SelectedDomain.Model, 
                out ContentItem newContent))
            {
                model.Items.Add(newContent);
            }
        }


        public TagResolver Resolver { get; set; } = new TagResolver();

        public string Id
        {
            get { return model.Id; }
            set { }
        }

        private Content model;
        public Content Content
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                OnPropertyChanged("Content");
            }
        }

        public override string Filepath => FileSystemDataProvider.Filepath<Content>();





        private ICommand _SaveWorkspaceCommand;
        public ICommand SaveWorkspaceCommand
        {
            get
            {
                if (_SaveWorkspaceCommand == null)
                {
                    _SaveWorkspaceCommand = new RelayCommand(
                    param => SaveWorkspace(),
                    param => CanSaveWorkspace());
                }
                return _SaveWorkspaceCommand;
            }
        }
        private bool CanSaveWorkspace()
        {
            return true;
        }
        private void SaveWorkspace()
        {
            SaveData();
        }


        protected override bool LoadData()
        {
            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Content content = new Content().Default();
                if(!FileSystemDataProvider.TryWrite<Content>(content, out string error, filepath))
                {
                    OnFailure(error);
                }               
            }

            bool b = FileSystemDataProvider.TryRead<Content>(Filepath, out model, out string message);
            if (!b)
            {
                OnFailure(message);
            }
            else
            {
                LoadTagResolver();
                Domains = new ObservableCollection<DomainViewModel>(from x in model.Domains select new DomainViewModel(x));
                Domains.CollectionChanged += Domains_CollectionChanged;
                SelectedDomain = Domains[0];
            }
            return b;
        }



        private void Domains_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as DomainViewModel;
                    if (vm != null)
                    {
                        model.Domains.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as DomainViewModel;
                    if (vm != null)
                    {
                        var found = model.Domains.FirstOrDefault(x => x.Id.Equals(vm.Id, StringComparison.OrdinalIgnoreCase));
                        if (found != null)
                        {
                            model.Domains.Remove(found);
                        }
                    }
                }
            }
        }


        public ContentModule()
        {

        }

        protected override bool SaveData()
        {
            if (model !=null && !FileSystemDataProvider.TryWrite<Content>(model, out string message, Filepath))
            {
                OnFailure(message);
                return false;
            }
            return true;
        }



        private void LoadTagResolver()
        {
            Resolver.Load(model.Items);
        }

    }
}
