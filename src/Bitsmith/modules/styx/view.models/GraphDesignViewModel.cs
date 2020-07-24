using Bitsmith.Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class GraphDesignViewModel : ViewModel<GraphDesign>
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

        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
                OnPropertyChanged("Description");
            }
        }



        public GraphDesignViewModel(GraphDesign model)
        {
            Model = model;
        }
    }
}
