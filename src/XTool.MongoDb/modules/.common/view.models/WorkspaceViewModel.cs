
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XTool.DataServices;
using XTool.DataServices.Abstractions;
using XTool.MongoDb;

namespace XTool.ViewModels
{
    public class WorkspaceViewModel
    {
        public Grid Root { get; set; }

        public IDataService DataService { get; set; }


        public MongoDBModule MongoDB { get; set; }


        #region overlay

        //public OverlayManager Overlay { get; set; } = new OverlayManager();
        //private void ShowContentOverlay(dynamic args)
        //{
        //    OverlayView overlay = new OverlayView() { Close = RemoveOverlay };
        //    overlay.grdOverlay.Children.Add(args.Control);
        //    overlay.SetTitle((string)args.Title);
        //    Root.Children.Add(overlay);
        //}
        private void RemoveOverlay()
        {
            Root.Children.RemoveAt(Root.Children.Count - 1);
        }


        #endregion


        public WorkspaceViewModel(Workspace model)
        {
            DataService = new XmlDataService();
            //DataService = new JsonDataService();

            MongoDB = new MongoDBModule(DataService);
            MongoDB.Setup();

        }

        internal void Save()
        {
            if (MongoDB.CanSaveWorkspace())
            {
                MongoDB.SaveWorkspace();
            }

        }
    }
}
