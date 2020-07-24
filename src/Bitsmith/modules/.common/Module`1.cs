using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class Module<T>  where T : class, new()
    {
        protected List<T> Models { get; set; } = new List<T>();

        public virtual string Filepath { get; set; } = FileSystemDataProvider.Filepath<T>();

        private ICommand _SaveItemsCommand;
        public ICommand SaveItemsCommand
        {
            get
            {
                if (_SaveItemsCommand == null)
                {
                    _SaveItemsCommand = new RelayCommand(
                        param => SaveItems(),
                        param => CanSaveItems());
                }
                return _SaveItemsCommand;
            }
        }

        private bool CanSaveItems()
        {
            return Models.Count > 0;
        }
        private void SaveItems()
        {
            SaveData();
        }

        public virtual bool CanSaveWorkspace()
        {
            return Models != null && Models.Count > 0;
        }

        public virtual void SaveWorkspace()
        {
            SaveData();
        }

        protected virtual void SaveData()
        {
            if(!FileSystemDataProvider.TryWrite<T>(Models, out string message, Filepath))
            {
                OnFailure("save-data", message);
            }
        }

        protected virtual bool LoadData()
        {
            bool b = FileSystemDataProvider.TryRead<T>(out List<T> items, out string message, Filepath);

            if(!b)
            {
                OnFailure("load-data",message);
            }
            else
            {
                Models = items;
            }
            return b;
        }

        protected virtual void OnFailure(string actionName, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                System.Windows.MessageBox.Show(message);
            }
            
        }


        public bool IsInitialized { get; set; }

        public virtual void Initialize() { }

        public void Setup()
        {
            IsInitialized = LoadData();
            Initialize();
        }
        public void Cleanup()
        {
            SaveData();
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
