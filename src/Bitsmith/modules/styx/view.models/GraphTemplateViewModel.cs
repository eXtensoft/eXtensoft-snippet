using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitsmith.Styx;

namespace Bitsmith.ViewModels
{
    public class GraphTemplateViewModel : ViewModel<GraphTemplate>
    {


        public string Display
        {
            get
            {
                return Model.Display;
            }
            set
            {
                Model.Display = value;
                OnPropertyChanged("Display");
            }
        }


        public GraphTemplateViewModel(GraphTemplate model)
        {
            Model = model;
        }
    }
}
