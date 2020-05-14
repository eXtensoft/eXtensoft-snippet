using Bitsmith.Models;
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
        public ContentModule Content { get; set; }

        //public Data Data { get; set; }

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
            Content = new ContentModule();
            //Content.Setup();
            //Data = new Data();
            //Data.Setup();
            Paths = new VirtualPathModule();
            //Paths.Setup();
            //Mimes = new MimeModule();
            //Mimes.Setup();

        }






    }
}
