using Bitsmith.DataServices;
using Bitsmith.DataServices.Abstractions;
using Bitsmith.NaturalLanguage;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;


namespace Bitsmith.ViewModels
{
    public class WorkspaceViewModel : INotifyPropertyChanged
    {
        public Grid Root { get; set; }

        private IDataService _DataService;
        public IDataService DataService 
        {
            get { return _DataService; }
            set
            {
                // must ensurelazy loaded modules are
                // loaded before dataservice can change
                EnsureLazyLoadedModulesSetup();
                _DataService = value;               
                OnPropertyChanged("DataService");
            }
        }


        public ObservableCollection<IDataService> DataServices { get; set; } = new ObservableCollection<IDataService>();

        private DataServiceStrategyOption _DataServiceStrategy;

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



        private StyxModule _Styx;
        public StyxModule Styx
        {
            get
            {
                if (_Styx == null)
                {
                    _Styx = new StyxModule( DataService );
                    _Styx.Setup();
                }
                return _Styx;
            }
        }

        private ChronosModule _Chronos;
        public ChronosModule Chronos
        {
            get
            {
                if (_Chronos == null)
                {
                    _Chronos = new ChronosModule(DataService);
                    _Chronos.Setup();
                    _Chronos.RefreshWorkEffort(Tasks.Project.Items);
                }
                return _Chronos;
            }
        }


        public CredentialsModule Credentials { get; set; }
        public SettingsModule Settings { get; set; }
        public ContentModule Content { get; set; }

        public IndexerModule Indexer { get; set; }


        public TasksModule Tasks  { get; set; }

        public WorkflowModule Workflow { get; set; }

        public VirtualPathModule Paths { get; set; }

        public MimeModule Mimes { get; set; }

        public ProjectsModule Projects { get; set; }

        public SchemaModule Schema { get; set; }

        public RolodexModule Rolodex { get; set; }

        public DatatoolModule Datatool { get; set; } 

        #region overlay

        public OverlayManager Overlay { get; set; } = new OverlayManager();
        private void ShowContentOverlay(dynamic args)
        {
            OverlayView overlay = new OverlayView() { Close = RemoveOverlay };
            overlay.grdOverlay.Children.Add(args.Control);
            overlay.SetTitle((string)args.Title);
            Root.Children.Add(overlay);
        }
        private void RemoveOverlay()
        {
            Root.Children.RemoveAt(Root.Children.Count - 1);
        }


        #endregion

        public Action HandleDataServiceChanged { get; set; }

        public WorkspaceViewModel(Workspace model)
        {
            // first time setting we don't want 
            // 'setup lazy loaded modules to fire
            _DataServiceStrategy = model.DataServiceStrategy;

            DataServices.Add(new XmlDataService());
            DataServices.Add(new JsonDataService());

            var found = DataServices.FirstOrDefault(x => x.Key.Equals(model.DataServiceStrategy));
            if (found != null)
            {
                _DataService = found;
            }

            Overlay.RegisterOverlay(AppConstants.OverlayContent, ShowContentOverlay);
            Settings = new SettingsModule(DataService );
            Settings.Setup();

            Tasks = new TasksModule(DataService, Settings);
            Tasks.Setup();

            Paths = new VirtualPathModule(DataService);
            Paths.Setup();

            Mimes = new MimeModule(DataService);
            Mimes.Setup();

            Indexer = new IndexerModule(DataService, Settings);
            Indexer.Setup();

            Schema = new SchemaModule(DataService, Settings);
            Schema.Setup();

            Content = new ContentModule(DataService, Settings, Indexer, Paths, Schema);
            Content.Setup(Mimes);

            Credentials = new CredentialsModule(DataService) { };
            Credentials.Setup();

            Rolodex = new RolodexModule(DataService);
            Rolodex.Setup();

            Datatool = new DatatoolModule(DataService);
            Datatool.Setup();

            Workflow = new WorkflowModule();

        }

        private void EnsureLazyLoadedModulesSetup()
        {
            var chronos = Chronos;
            var styx = Styx;
        }

        internal void Save()
        {
            if (!DataService.Key.Equals(_DataServiceStrategy))
            {
                Bootstrapper.SetDataStrategy(DataService.Key);
                Settings.DataService = _DataService;
                Tasks.DataService = _DataService;
                Paths.DataService = _DataService;
                Mimes.DataService = _DataService;
                Content.DataService = _DataService;
                Credentials.DataService = _DataService;
                Indexer.DataService = _DataService;
                Chronos.DataService = _DataService;               
                Mimes.DataService = _DataService;
                Schema.DataService = _DataService;
                Mimes.SaveWorkspace();
            }
            if (Content.CanSaveWorkspace())
            {
                Content.SetPreferences();
                Content.SaveWorkspace();
            }

            if (Schema.CanSaveWorkspace())
            {
                Schema.SetPreferences();
                Schema.SaveWorkspace();
            }
            if (Indexer.CanSaveWorkspace())
            {
                Indexer.SaveWorkspace();
            }

            if (Tasks.CanSaveWorkspace())
            {
                Tasks.SetPreferences();
                Tasks.SaveWorkspace();
            }
            if (_Chronos !=null && _Chronos.CanSaveWorkspace())
            {
                _Chronos.SaveWorkspace();
            }
            if (Styx != null && Styx.CanSaveWorkspace())
            {
                Styx.SaveWorkspace();
            }
            // must be last so that preferences are persisted
            if (Settings.CanSaveWorkspace())
            {                
                Settings.SaveWorkspace();
            }
        }
    
    }
}
