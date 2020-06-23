using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class NewContentViewModel : INotifyPropertyChanged
    {

        private TagMapViewModel _SelectedTag;
        public TagMapViewModel SelectedTag
        {
            get
            {
                return _SelectedTag;
            }
            set
            {
                _SelectedTag = value;
                
                OnPropertyChanged("SelectedTag");
                AddTag(_SelectedTag);                
            }
        }

        public void AddTag(TagMapViewModel vm)
        {
            if (vm != null && !Tags.Contains(vm.Key))
            {
                Tags.Add(vm.Key);
                OnPropertyChanged("Tags");
            }
        }

        private ICommand _ClearTagsCommand;
        public ICommand ClearTagsCommand
        {
            get
            {
                if (_ClearTagsCommand == null)
                {
                    _ClearTagsCommand = new RelayCommand(
                    param => ClearTags(),
                    param => CanClearTags());
                }
                return _ClearTagsCommand;
            }
        }
        private bool CanClearTags()
        {
            return Tags.Count > 0;
        }
        private void ClearTags()
        {
            Tags.Clear();
            OnPropertyChanged("Tags");
            SelectedTag = null;
        }

        public bool HasFile
        {
            get
            {
                return !String.IsNullOrWhiteSpace(_Filepath) && File.Exists(_Filepath);
            }
            set
            {

            }
        }

        private string _Filepath;
        public string Filepath
        {
            get
            {
                return _Filepath;
            }
            set
            {
                _Filepath = value;
                OnPropertyChanged("Filepath");
                OnPropertyChanged("HasFile");
            }
        }


        private string _Display = string.Empty;
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

        private string _Body = string.Empty;
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
                    _Mime = "url";
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

        public void SetFile(FileInfo info)
        {
            Filepath = info.FullName;
            Display = info.Name;
            Mime = "file";
            Body = info.Name;
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
