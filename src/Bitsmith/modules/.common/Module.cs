using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using System;
using System.CodeDom;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Bitsmith.ViewModels
{
    public abstract class Module : INotifyPropertyChanged
    {
        public IDataService DataService { get; set; }
        protected virtual string ModuleKey 
        { 
            get { return this.GetType().Name.CamelToKebab(); } 

        }

        private UserSettings _UserPreferences;
        public UserSettings UserPreferences
        {
            get
            {
                return _UserPreferences;
            }
            set
            {
                if (value != null)
                {
                    _UserPreferences = value;
                }
            }
        }

        protected virtual void ApplyPreferences()
        {
        }

        internal virtual void SetPreferences()
        {

        }

        protected virtual string Filepath()
        {
            return Path.Combine(AppConstants.SettingsDirectory, DataService.Filepath<TypedItem>());
        }

        internal void SetDataService(IDataService dataService)
        {
            DataService = dataService;
        }

        public bool IsInitialized { get; set; }

        public void Setup()
        {
            if (DataService != null)
            {
                IsInitialized = LoadData();
                Initialize();
            }
        }

        public virtual void Initialize() { }
        protected virtual bool LoadData() { return true; }

        protected virtual bool SaveData() { return true; }

        public virtual bool CanSaveWorkspace()
        {
            return true;
        }

        public virtual void SaveWorkspace()
        {
            SaveData();
        }

        protected virtual void OnFailure(string message)
        {
            MessageBox.Show(message);
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
