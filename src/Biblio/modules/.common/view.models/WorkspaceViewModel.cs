using Biblio.Models;
using Biblio.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Biblio.ViewModels
{
    public class WorkspaceViewModel
    {
        public Grid Root { get; set; }



        private BibleModule _Bible;
        public BibleModule Bible
        {
            get
            {
                if (_Bible == null)
                {
                    _Bible = new BibleModule();
                    _Bible.Setup();
                }
                return _Bible;
            }
        }


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



        }

        internal void Save()
        {


        }
    }
}
