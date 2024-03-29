﻿using System.ComponentModel;
using System.Windows;

namespace XTool.ViewModels
{
    public class Module : INotifyPropertyChanged
    {
        //public IDataService DataService { get; set; }
        //protected virtual string ModuleKey 
        //{ 
        //    get { return this.GetType().Name.CamelToKebab(); } 

        //}

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
        //        }
        //    }
        //}

        //protected virtual void ApplyPreferences(UserSettings userPreferences)
        //{
        //}

        //internal virtual void SetPreferences()
        //{

        //}

        internal virtual string Filepath { get; set; }


        public bool IsInitialized { get; set; }

        public void Setup()
        {
            //if (DataService != null)
            //{
            //    IsInitialized = LoadData();
            //    Initialize();
            //}
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
