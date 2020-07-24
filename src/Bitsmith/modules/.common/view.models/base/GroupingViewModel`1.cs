using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bitsmith.ViewModels
{
    public class GroupingViewModel<T> : IDisposable, INotifyPropertyChanged where T : class, new()
    {
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public IEnumerable<T> Items { get; set; }

        private string _Display;
        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }

        public GroupingViewModel(IEnumerable<T> items)
        {
            Items = items;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            this.OnDispose();
        }



        protected virtual void OnDispose()
        {
        }
#if DEBUG

        ~GroupingViewModel()
        {
            string s = Display;
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, s, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }

#endif


        #endregion


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
