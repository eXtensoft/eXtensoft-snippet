using System.ComponentModel;
using System.Windows;

namespace Bitsmith.ViewModels
{
    public class Module : INotifyPropertyChanged
    {


        public virtual string Filepath => "...";
        public bool IsInitialized { get; set; }

        public void Setup()
        {
            IsInitialized = LoadData();
            Initialize();

        }

        public virtual void Initialize() { }
        protected virtual bool LoadData() { return true; }

        protected virtual bool SaveData() { return true; }

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
