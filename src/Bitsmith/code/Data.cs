using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public class Data : Module
    {
        private static Data _Instance;
        public static Data Instance
        {
            get
            {
                return _Instance;
            }
        }

        private DataViewModel _ViewModel;
        public DataViewModel ViewModel
        {
            get
            {
                if (_ViewModel == null)
                {
                    _ViewModel = new DataViewModel(Instance);
                }
                return _ViewModel;
            }
        }

        static Data()
        {
            _Instance = Bootstrapper.Data();
        }
    }
}
