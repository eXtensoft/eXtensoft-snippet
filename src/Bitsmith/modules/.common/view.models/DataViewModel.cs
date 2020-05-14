using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class DataViewModel : ViewModel<Data>
    {
        public VirtualPathModule Paths { get; set; }
        public MimeModule Mimes { get; set; }

        public DataViewModel(Data model)
        {
            Model = model;
            Mimes = new MimeModule();
            Mimes.Setup();
            Paths = new VirtualPathModule();
            Paths.Setup();
       }
    }
}
