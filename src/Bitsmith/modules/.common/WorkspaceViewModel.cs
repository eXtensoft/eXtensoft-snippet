using Bitsmith.DataServices;
using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using Bitsmith.NaturalLanguage;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    _Chronos = new ChronosModule( DataService );
                    _Chronos.Setup();
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


        public WorkspaceViewModel(Workspace model)
        {
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

            Content = new ContentModule(DataService, Settings, Indexer, Paths);
            Content.Setup(Mimes);

            Credentials = new CredentialsModule(DataService) { };
            Credentials.Setup();


            Workflow = new WorkflowModule();

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
                Mimes.DataService = _DataService;
                Mimes.SaveWorkspace();
            }
            if (Content.CanSaveWorkspace())
            {
                Content.SetPreferences();
                Content.SaveWorkspace();
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
            if (_Chronos != null && Chronos.CanSaveWorkspace())
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
