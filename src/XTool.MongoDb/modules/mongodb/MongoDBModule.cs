using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using XTool.DataServices.Abstractions;
using XTool.MongoDb;
using XTool.MongoDb.Schema;

namespace XTool.ViewModels
{
    public class MongoDBModule : Module<ConnectionString>
    {

        private ICollectionView _Servers;
        public ICollectionView Servers
        {
            get
            {
                if (Contexts != null && _Servers == null)
                {
                    _Servers = CollectionViewSource.GetDefaultView(Contexts);
                    _Servers.Filter = FilterServers;
                }
                return _Servers;
            }
            set { }
           
        }

        private bool FilterServers(object obj)
        {
            var ctx = obj as MongoContext;
            if (ctx != null)
            {
                return true;
            }
            return false;
        }

        private List<MongoContext> Contexts { get; set; } = new List<MongoContext>();

        public ObservableCollection<ConnectionStringViewModel> Connections { get; set; }

        private ConnectionStringViewModel _SelectedConnection;
        public ConnectionStringViewModel SelectedConnection
        {
            get { return _SelectedConnection; }
            set
            {
                _SelectedConnection = value;
                OnPropertyChanged("SelectedConnection");
            }
        }

        private ICommand _AddConnectionCommand;
        public ICommand AddConnectionCommand
        {
            get
            {
                if (_AddConnectionCommand == null)
                {
                    _AddConnectionCommand = new RelayCommand(
                    param => AddConnection(),
                    param => CanAddConnection());
                }
                return _AddConnectionCommand;
            }
        }
        private bool CanAddConnection()
        {
            return true;
        }
        private void AddConnection()
        {
            var cn = new ConnectionString().Default();
            Connections.Add(new ConnectionStringViewModel(cn));
        }

        private ICommand _TryConnectCommand;
        public ICommand TryConnectCommand
        {
            get
            {
                if (_TryConnectCommand == null)
                {
                    _TryConnectCommand = new RelayCommand(
                    param => TryConnect(),
                    param => CanTryConnect());
                }
                return _TryConnectCommand;
            }
        }
        private bool CanTryConnect()
        {
            return SelectedConnection != null && !SelectedConnection.CanConnect;
        }
        private void TryConnect()
        {
            if (SelectedConnection.Text.TryConnect(out MongoContext context))
            {
                SelectedConnection.CanConnect = true;
                SelectedConnection.ConnectedOn = DateTime.Now;
                context.Key = SelectedConnection.Key;

                Contexts.Add(context);
                Servers.Refresh();
            }
        }

        

        public MongoDBModule(IDataService dataService)
        {
            DataService = dataService;
        }


        public override void Initialize()
        {
            Connections = new ObservableCollection<ConnectionStringViewModel>(from x in Models select new ConnectionStringViewModel(x));
        }

        protected override bool LoadData()
        {
            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Models.Add(new ConnectionString().Default());

                if (!FileSystemDataProvider.TryWrite<ConnectionString>(Models, out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = FileSystemDataProvider.TryRead<ConnectionString>(Filepath, out List<ConnectionString> list, out string message);
            if (b)
            {
                Models = list;
            }
            else
            {
                OnFailure(message);
            }
            return b;
        }

        

        protected virtual void OnFailure(string message)
        {
            MessageBox.Show(message);
        }
    }
}
