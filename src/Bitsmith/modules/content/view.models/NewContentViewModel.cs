using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class NewContentViewModel : INotifyPropertyChanged
    {
        private static Dictionary<string, Type> _ColumnTypeMaps = new Dictionary<string, Type>()
        {
            { "Text", typeof(string) },
            { "Date" ,typeof(DateTime) },
            { "Integer" ,typeof(Int32) },
            { "Decimal" ,typeof(decimal) },
            { "Yes/No" ,typeof(bool) },
        };

        private ICommand _SaveTabularDataCommand;
        public ICommand SaveTabularDataCommand
        {
            get
            {
                if (_SaveTabularDataCommand == null)
                {
                    _SaveTabularDataCommand = new RelayCommand(
                    param => SaveTabularData(),
                    param => CanSaveTabularData());
                }
                return _SaveTabularDataCommand;
            }
        }
        private bool CanSaveTabularData()
        {
            return TabularData != null && TabularData.Rows.Count > 0;
        }
        private void SaveTabularData()
        {
            Body = TabularData.ToCsv();
        }


        private bool _IsTabularDataDirty = true;
        public bool IsTabularDataDirty
        {
            get { return _IsTabularDataDirty; }
            set
            {
                _IsTabularDataDirty = value;
                OnPropertyChanged("IsTabularDataDirty");
            }
        }

        private ICommand _AddColumnCommand;
        public ICommand AddColumnCommand
        {
            get
            {
                if (_AddColumnCommand == null)
                {
                    _AddColumnCommand = new RelayCommand(
                    param => AddColumn(),
                    param => CanAddColumn());
                }
                return _AddColumnCommand;
            }
        }
        private bool CanAddColumn()
        {
            return SelectedColumnType != null && !String.IsNullOrWhiteSpace(ColumnName);
        }
        private void AddColumn()
        {
            TabularData.Columns.Add(new DataColumn(ColumnName, _ColumnTypeMaps[SelectedColumnType.Key]));
            ColumnName = string.Empty;
            RefreshTabularData();
        }

        private void RefreshTabularData()
        {
            var dt = TabularData;
            TabularData = null;
            OnPropertyChanged("TabularData");
            TabularData = dt;
            OnPropertyChanged("TabularData");
            RefreshColumns();   

        }
        private void RefreshColumns()
        {
            Columns.Clear();
            foreach (DataColumn item in TabularData.Columns)
            {
                Columns.Add(new DataColumnViewModel(item) { RefreshData = RefreshTabularData });
            }
        }


        private string _ColumnName;
        public string ColumnName
        {
            get
            {
                return _ColumnName;
            }
            set
            {
                _ColumnName = value;
                OnPropertyChanged("ColumnName");
            }
        }


        private TypedItem _SelectedColumnType;
        public TypedItem SelectedColumnType
        {
            get { return _SelectedColumnType; }
            set
            {
                _SelectedColumnType = value;
                OnPropertyChanged("SelectedColumnType");
            }
        }
        public ObservableCollection<TypedItem> ColumnTypes { get; set; }

        public DataColumnCollection DataColumns
        {
            get
            {
                return TabularData.Columns;
            }
        }

        public ObservableCollection<DataColumnViewModel> Columns { get; set; } = new ObservableCollection<DataColumnViewModel>();
        public DataTable TabularData { get; set; } = new DataTable();



        private SchemaBuilderViewModel _SelectedSchema;
        public SchemaBuilderViewModel SelectedSchema
        {
            get { return _SelectedSchema; }
            set
            {
                _SelectedSchema = value;
                OnPropertyChanged("SelectedSchema");
            }
        }
       
        public void EnsureSelectedSchema(SchemaBuilderViewModel selectedSchema)
        {
            if (SelectedSchema == null || 
                !SelectedSchema.Token.Equals(selectedSchema.Token, StringComparison.OrdinalIgnoreCase))
            {
                SelectedSchema = selectedSchema;
            }
        }

        private bool _IsEnableContentSchemas = false;
        public bool IsEnableContentSchemas
        {
            get
            {
                return _IsEnableContentSchemas;
            }
            set
            {
                _IsEnableContentSchemas = value;
                if (!value && ContentType == ContentTypeOption.Schema)
                {
                    ContentType = ContentTypeOption.Text;
                    OnPropertyChanged("IsText");
                }
                OnPropertyChanged("IsEnableContentSchemas");
                OnPropertyChanged("IsShowSchemas");
            }
        }


        private  int _MaxLength = 300;
        public int MaxLength
        {
            get { return _MaxLength; }
            set
            {
                _MaxLength = value;
                OnPropertyChanged("MaxLength");
            }
        }

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

        private ICommand _CycleTagsCommand;
        public ICommand CycleTagsCommand
        {
            get
            {
                if (_CycleTagsCommand == null)
                {
                    _CycleTagsCommand = new RelayCommand(
                    param => CycleTags(),
                    param => CanCycleTags());
                }
                return _CycleTagsCommand;
            }
        }
        private bool CanCycleTags()
        {
            return true;
        }
        private void CycleTags()
        {
            _TagsIndex = _TagsIndex < _TagsMax ? _TagsIndex + 1 : 0;
            OnPropertyChanged("IsTagsRecent");
            OnPropertyChanged("IsTagsPopular");
            OnPropertyChanged("TagsExpander");
        }

        private int _TagsMax = 2;
        private int _TagsIndex = 0;

        private static Dictionary<int, string> tagmaps = new Dictionary<int, string>()
        {
            {0, "../../../content/icons/tag-black-basic.png" },
            {1, "../../../content/icons/tag-black-list.png" },
            {2, "../../../content/icons/tag-black-star.png" },
        };
        public string TagsExpander
        {
            get
            {
                return tagmaps[_TagsIndex];
            }
            set { }
        }
        

        public Visibility IsTagsRecent
        {
            get
            {
                return _TagsIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            }

        }

        public Visibility IsTagsPopular
        {
            get
            {
                return _TagsIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
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


        
        private string _Path;
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                OnPropertyChanged("Path");
            }
        }

        private List<string> _Paths;
        public List<string> Paths
        {
            get
            {
                return _Paths;
            }
            set
            {
                _Paths = value;
                OnPropertyChanged("Paths");
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
        public ContentTypeOption ContentType 
        {
            get { return _ContentType; } 
            set
            {
                _ContentType = value;
                if (_ContentType == ContentTypeOption.Link)
                {
                    _Mime = "url";
                }
            }
        } 

        public bool IsLink
        {
            get
            {
                return ContentType == ContentTypeOption.Link;
            }
            set
            {
                if (value)
                {
                    ContentType = ContentTypeOption.Link;
                    _Mime = "url";
                }
            }
        }

        public bool IsFile
        {
            get
            {
                return ContentType == ContentTypeOption.File;
            }
            set
            {
                if (value)
                {
                    ContentType = ContentTypeOption.File;
                }
            }
        }

        public bool IsText
        {
            get
            {
                return ContentType == ContentTypeOption.Text;
            }
            set
            {
                if (value)
                {
                    ContentType = ContentTypeOption.Text;
                    Mime = "text";
                }
            }
        }

        public bool IsSchema
        {
            get
            {
                return ContentType == ContentTypeOption.Schema;
            }
            set
            {
                if (value)
                {
                    ContentType = ContentTypeOption.Schema;
                }
            }
        }

        public bool IsTabular
        {
            get
            {
                return ContentType == ContentTypeOption.TabularData;
            }
            set
            {
                if (value)
                {
                    ContentType = ContentTypeOption.TabularData;
                    Mime = "tab";
                }
            }
        }

        private bool _IsShowSchemas = true;
        public bool IsShowSchemas
        {
            get { return _IsEnableContentSchemas && _IsShowSchemas; }
            set
            {
                _IsShowSchemas = value;
                OnPropertyChanged("IsShowSchemas");
            }
        }

        public NewContentViewModel()
        {
            Paths = new List<string>(new string[] { $"/content" });
            ColumnTypes = new ObservableCollection<TypedItem>(from key in _ColumnTypeMaps.Keys select new TypedItem(key, key));
            SelectedColumnType = ColumnTypes[0];

            RefreshTabularData();
        }

        internal void Refresh(TagMapViewModel tag, ContentTypeOption contentType)
        {
            Paths = new List<string>(new string[] { $"/content" });
            Filepath = string.Empty;
            Display = string.Empty;
            Body = string.Empty;
            Tags = new List<string>();
            AddTag(tag);
            ContentType = contentType;
        }

        public bool Validate()
        {
            bool b = true;
            b = b ? !string.IsNullOrWhiteSpace(Display) : b;
            b = b ? !string.IsNullOrWhiteSpace(Body) : b;
            b = b ? !string.IsNullOrWhiteSpace(Mime) : b;

            if (Body.Length > MaxLength)
            {

            }



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
