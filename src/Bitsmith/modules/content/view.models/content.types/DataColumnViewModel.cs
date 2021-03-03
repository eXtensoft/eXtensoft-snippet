using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class DataColumnViewModel : ViewModel<DataColumn>
    {
        public string ColumnName
        {
            get
            {
                return Model.ColumnName;
            }
            set
            {               
                Model.ColumnName = value;
                OnPropertyChanged("ColumnName");
                if (RefreshData != null)
                {
                    RefreshData();
                }
            }
        }

        public string Datatype
        {
            get
            {
                return Model.DataType.Name;
            }
            set
            {
                Type t = typeof(string);
                switch (value)
                {
                    case "Int32":
                        t = typeof(Int32);
                        break;
                    case "string":
                    default:
                        break;
                }
                Model.DataType = t;
                OnPropertyChanged("Datatype");
            }
        }

        public Action RefreshData { get; set; }

        public DataColumnViewModel(DataColumn model)
        {
            Model = model;
        }

    }
}
