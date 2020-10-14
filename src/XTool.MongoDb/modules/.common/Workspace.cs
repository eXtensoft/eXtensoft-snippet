using XTool.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class Workspace : Module
    {

        private static Workspace _Instance;
        public static Workspace Instance
        {
            get
            {
                return _Instance;
            }
        }

        private WorkspaceViewModel _ViewModel;
        public WorkspaceViewModel ViewModel
        {
            get
            {
                if (_ViewModel == null)
                {
                    _ViewModel = new WorkspaceViewModel(Instance);
                }
                return _ViewModel;
            }
        }


        static Workspace()
        {
            _Instance = Bootstrapper.Workspace();
        }
        public DateTime CreatedAt { get; set; }
        public FileInfo Info { get; set; }

        private string windowtitle;

        public string WindowTitle
        {
            get { return windowtitle; }
            set
            {
                windowtitle = value;
                OnPropertyChanged("WindowTitle");
            }
        }


    }
}
