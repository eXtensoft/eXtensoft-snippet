using Bitsmith.DataServices;
using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using Bitsmith.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bitsmith.ViewModels
{
    public class WorkspaceViewModel
    {
        public Grid Root { get; set; }

        public IDataService DataService { get; set; }


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

        public TasksModule Project { get; set; }

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
            DataService = new XmlDataService();
            //DataService = new JsonDataService();

            Overlay.RegisterOverlay(AppConstants.OverlayContent, ShowContentOverlay);
            Settings = new SettingsModule(DataService );
            Settings.Setup();

            Paths = new VirtualPathModule(DataService);
            Paths.Setup();

            Mimes = new MimeModule(DataService);
            Mimes.Setup();

            Indexer = new IndexerModule(DataService, Settings);
            Indexer.Setup();

            Content = new ContentModule(DataService, Settings, Indexer, Paths);
            Content.Setup(Mimes,Settings);

            Credentials = new CredentialsModule(DataService) { };
            Credentials.Setup();

            Project = new TasksModule(DataService) { UserPreferences = Settings.UserPreferences};
            Project.Setup();

            Workflow = new WorkflowModule();


        }

        internal void Save()
        {
            if (Content.CanSaveWorkspace())
            {
                Content.SetPreferences();
                Content.SaveWorkspace();
            }
            if (Indexer.CanSaveWorkspace())
            {
                Indexer.SaveWorkspace();
            }
            if (Settings.CanSaveWorkspace())
            {                
                Settings.SaveWorkspace();
            }
            if (Project.CanSaveWorkspace())
            {
                Project.SetPreferences();
                Project.SaveWorkspace();
            }
            if (_Chronos != null && Chronos.CanSaveWorkspace())
            {
                _Chronos.SaveWorkspace();
            }
            if (Styx != null && Styx.CanSaveWorkspace())
            {
                Styx.SaveWorkspace();
            }

        }
    }
}
