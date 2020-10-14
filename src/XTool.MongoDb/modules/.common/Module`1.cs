
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using XTool.DataServices.Abstractions;
using XTool.Models;

namespace XTool.ViewModels
{

    public class Module<T>  where T : class, new()
    {
        public IDataService DataService { get; set; }

        protected virtual string ModuleKey
        {
            get { return this.GetType().Name.CamelToKebab(); }

        }

        //private UserSettings _UserPreferences;
        //public UserSettings UserPreferences
        //{
        //    get
        //    {
        //        return _UserPreferences;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _UserPreferences = value;
        //            ApplyPreferences(_UserPreferences);
        //        }
        //    }
        //}

        //protected virtual void ApplyPreferences(UserSettings userPreferences)
        //{
        //}

        //internal virtual void SetPreferences()
        //{

        //}

        protected List<T> Models { get; set; } = new List<T>();

        private string _Filepath;
        public virtual string Filepath 
        { 
            get
            {
                if (String.IsNullOrWhiteSpace(_Filepath) && DataService != null)
                {
                    _Filepath = DataService.Filepath<T>();
                }
                return _Filepath;
            }
            set { _Filepath = value; } 
        }

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
            if (DataService != null)
            {
                IsInitialized = LoadData();
                Initialize();
            }
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
