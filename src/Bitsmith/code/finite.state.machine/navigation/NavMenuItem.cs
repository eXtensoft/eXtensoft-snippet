using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bitsmith
{
    public class NavMenuItem : INotifyPropertyChanged
    {
        #region style properties





        #endregion

        public string Display { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        
        public bool IsShow { get; set; }
        public int SortOrder { get; set; }

        private bool _IsCurrent;
        public bool IsCurrent
        {
            get
            {
                return _IsCurrent;
            }
            set
            {
                _IsCurrent = value;
                OnPropertyChanged("IsCurrent");
            }
        }

        private bool _CanNavigateTo;
        public bool CanNavigateTo
        {
            get
            {
                return _CanNavigateTo;
            }
            set
            {
                _CanNavigateTo = value;
                OnPropertyChanged("CanNavigateTo");
            }
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
