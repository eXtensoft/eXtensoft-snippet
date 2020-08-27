﻿using Bitsmith.Models;
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



        private StyxModule _Styx;
        public StyxModule Styx
        {
            get
            {
                if (_Styx == null)
                {
                    _Styx = new StyxModule();
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
                    _Chronos = new ChronosModule();
                    _Chronos.Setup();
                }
                return _Chronos;
            }
            
        }
        public CredentialsModule Credentials { get; set; }
        public SettingsModule Settings { get; set; }
        public ContentModule Content { get; set; }

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
            Overlay.RegisterOverlay(AppConstants.OverlayContent, ShowContentOverlay);
            Settings = new SettingsModule();
            Settings.Setup();

            Paths = new VirtualPathModule();
            Paths.Setup();

            Mimes = new MimeModule();
            Mimes.Setup();

            Content = new ContentModule() { UserPreferences = Settings.UserPreferences };
            Content.Setup(Paths,Mimes,Settings);
            //Content.UserPreferences = Settings.UserPreferences;

            Credentials = new CredentialsModule();
            Credentials.Setup();

            Project = new TasksModule();
            Project.Setup();
            Project.UserPreferences = Settings.UserPreferences;

            Workflow = new WorkflowModule();



        }

        internal void Save()
        {
            if (Settings.CanSaveWorkspace())
            {
                
                Settings.SaveWorkspace();
            }
            if (Content.CanSaveWorkspace())
            {
                Content.SetPreferences();
                Content.SaveWorkspace();
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
            if (Settings.CanSaveWorkspace())
            {
                Settings.SaveWorkspace();
            }

        }
    }
}
