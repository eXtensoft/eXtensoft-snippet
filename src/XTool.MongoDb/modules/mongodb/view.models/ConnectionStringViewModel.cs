using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTool.MongoDb;

namespace XTool.ViewModels
{
    public class ConnectionStringViewModel : ViewModel<ConnectionString>
    {
        private bool _CanConnect;
        public bool CanConnect
        {
            get
            {
                return _CanConnect;
            }
            set
            {
                _CanConnect = value;
                OnPropertyChanged("CanConnect");
            }
        }

        public DateTime ConnectedOn
        {
            get
            {
                return Model.ConnectedOn;
            }
            set
            {
                Model.ConnectedOn = value;
                OnPropertyChanged("ConnectedOn");
            }
        }


        public string Key
        {
            get
            {
                return Model.Key;
            }
            set
            {
                Model.Key = value;
                OnPropertyChanged("Key");
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

        public string Text
        {
            get
            {
                return Model.Text;
            }
            set
            {
                Model.Text = value;
                OnPropertyChanged("Text");
            }
        }

        public ConnectionStringViewModel(ConnectionString model)
        {
            Model = model;
        }
    }
}
