using System.ComponentModel;
using System.Windows;

namespace Bitsmith.ViewModels
{
    public class Module : INotifyPropertyChanged
    {

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
                    ApplyPreferences(_UserPreferences);
                }
            }
        }

        protected virtual void ApplyPreferences(UserSettings userPreferences)
        {
        }

        internal virtual void SetPreferences()
        {

        }

        public virtual string Filepath { get; set; }
        public bool IsInitialized { get; set; }

        public void Setup()
        {
            IsInitialized = LoadData();
            var s = this.GetType().Name;
            Initialize();

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
