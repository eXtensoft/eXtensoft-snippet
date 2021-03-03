using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class DataFieldViewModel : ViewModel<DataField>
    {
        private bool _IsPipeline;
        public bool IsPipeline
        {
            get
            {
                return _IsPipeline;
            }
            set
            {
                _IsPipeline = value;
                OnPropertyChanged("IsPipeline");
            }
        }


        public int Position
        {
            get
            {
                return Model.Position;
            }
            set
            {
                Model.Position = value;
                OnPropertyChanged("Position");
            }
        }


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

        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                Model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string FieldType
        {
            get
            {
                return Model.FieldType;
            }
            set
            {

                OnPropertyChanged("FieldType");
            }
        }


        public DataFieldViewModel(DataField model)
        {
            Model = model;
            IsPipeline = model.IsPipelineByDefault();
        }

    }
}
