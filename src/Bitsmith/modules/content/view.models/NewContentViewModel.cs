using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bitsmith.ViewModels
{
    public class NewContentViewModel : INotifyPropertyChanged
    {

        private string _Display = "display";
        public string Display
        {
            get
            {
                return _Display;
            }
            set
            {
                _Display = value;
                OnPropertyChanged("Display");
            }
        }

        private string _Body = "body";
        public string Body
        {
            get
            {
                return _Body;
            }
            set
            {
                _Body = value;
                OnPropertyChanged("Body");
            }
        }

        private string _Mime = "text";
        public string Mime
        {
            get
            {
                return _Mime;
            }
            set
            {
                _Mime = value;
                OnPropertyChanged("Mime");
            }
        }

        private List<string> _Tags = new List<string>();
        public List<string> Tags
        {
            get
            {
                return _Tags;
            }
            set
            {
                _Tags = value;
                OnPropertyChanged("Tags");
            }
        }



        private string _Path = $"/content/{Environment.UserName}";
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                OnPropertyChanged("Path");
            }
        }

        private ScopeOption _Scope;
        public ScopeOption Scope
        {
            get
            {
                return _Scope;
            }
            set
            {
                _Scope = value;
                OnPropertyChanged("Scope");
            }
        }

        private ContentTypeOption _ContentType = ContentTypeOption.Text;
        public bool IsLink
        {
            get
            {
                return _ContentType == ContentTypeOption.Link;
            }
            set
            {
                if (value)
                {
                    _ContentType = ContentTypeOption.Link;
                }
            }
        }

        public bool IsFile
        {
            get
            {
                return _ContentType == ContentTypeOption.File;
            }
            set
            {
                if (value)
                {
                    _ContentType = ContentTypeOption.File;
                }
            }
        }

        public bool IsText
        {
            get
            {
                return _ContentType == ContentTypeOption.Text;
            }
            set
            {
                if (value)
                {
                    _ContentType = ContentTypeOption.Text;
                    Mime = "text";
                }
            }
        }

        public bool Validate()
        {
            bool b = true;
            b = b ? !string.IsNullOrWhiteSpace(Display) : b;
            b = b ? !string.IsNullOrWhiteSpace(Body) : b;
            b = b ? !string.IsNullOrWhiteSpace(Mime) : b;

            return b;
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
