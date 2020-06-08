using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class MimeModule : Module<MimeMap>
    {

        public ObservableCollection<MimeMapViewModel> Items { get; set; }

        private MimeMapViewModel _SelectedItem;
        public MimeMapViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        private ICommand _AddItemCommand;
        public ICommand AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new RelayCommand(
                    param => AddItem(),
                    param => CanAddItem());
                }
                return _AddItemCommand;
            }
        }
        private bool CanAddItem()
        {
            return true;
        }
        private void AddItem()
        {
            Items.Add(new MimeMapViewModel(new MimeMap()));
        }


        protected override bool LoadData()
        {
            if (!File.Exists(Filepath))
            {
                List<MimeMap> list = new List<MimeMap>();

                list.Add(new MimeMap() { Id = "bmp", Extension = ".bmp", Display = "bmp", Name = "Bitmap", Mime = "image/bmp", View = "Bitsmith.ContentItemImageView" });
                list.Add(new MimeMap() { Id = "gif", Extension = ".gif", Display = "gif", Name = "GIF", Mime = "image/gif", View = "Bitsmith.ContentItemView" });
                list.Add(new MimeMap() { Id = "png", Extension = ".png", Display = "png", Name = "PNG", Mime = "image/png", View = "Bitsmith.ContentItemImageView" });
                list.Add(new MimeMap() { Id = "tiff", Extension = ".tiff", Display = "tiff", Name = "TIFF", Mime = "image/tiff", View = "Bitsmith.ContentItemImageView" });
                list.Add(new MimeMap() { Id = "jpg", Extension = ".jpg", Display = "jpg", Name = "JPG", Mime = "image/jpg", View = "Bitsmith.ContentItemImageView" });
                list.Add(new MimeMap() { Id = "pdf", Extension = ".pdf", Display = "pdf", Name = "PDF", Mime = "application/pdf", View = "Bitsmith.ContentItemFileView" });
                list.Add(new MimeMap() { Id = "xlsx", Extension = ".xlsx", Display = "xlsx", Name = "MS-Excel", Mime = "application/vnd.ms-excel", View = "Bitsmith.ContentItemFileView" });
                list.Add(new MimeMap() { Id = "xls", Extension = ".xls", Display = "xls", Name = "MS-Excel", Mime = "application/vnd.ms-excel", View = "Bitsmith.ContentItemFileView" });
                list.Add(new MimeMap() { Id = "docx", Extension = ".docx", Display = "docx", Name = "MS-Word", Mime = "application/msword", View = "Bitsmith.ContentItemFileView" });
                list.Add(new MimeMap() { Id = "doc", Extension = ".doc", Display = "doc", Name = "MS-Word", Mime = "application/msword", View = "Bitsmith.ContentItemFileView" });
                list.Add(new MimeMap() { Id = "csv", Extension = ".csv", Display = "csv", Name = "Comma Delimited", Mime = "text/plain", View = "Bitsmith.ContentItemTextfileView" });
                list.Add(new MimeMap() { Id = "xml", Extension = ".xml", Display = "xml", Name = "XML", Mime = "text/xml", View = "Bitsmith.ContentItemTextfileView" });
                list.Add(new MimeMap() { Id = "json", Extension = ".json", Display = "json", Name = "JSON", Mime = "application/json", View = "Bitsmith.ContentItemJsonView" });
                list.Add(new MimeMap() { Id = "js", Extension = ".js", Display = "js", Name = "Javascript", Mime = "text/javascript", View = "Bitsmith.ContentItemJavascriptView" });
                list.Add(new MimeMap() { Id = "java", Extension = ".java", Display = "java", Name = "Java", Mime = "text/java", View = "Bitsmith.ContentItemJavaView" });
                list.Add(new MimeMap() { Id = "c#", Extension = ".cs", Display = "c#", Name = "CSharp", Mime = "text/cs", View = "Bitsmith.ContentItemCSharpView" });
                list.Add(new MimeMap() { Id = "cshtml", Extension = ".cshtml", Display = "cshtml", Name = "CSHTML", Mime = "text/cshtml", View = "Bitsmith.ContentItemCSHTMLView" });
                list.Add(new MimeMap() { Id = "html", Extension = ".html", Display = "html", Name = "HTML", Mime = "text/html", View = "Bitsmith.ContentItemHTMLView" });
                list.Add(new MimeMap() { Id = "css", Extension = ".css", Display = "css", Name = "CSS", Mime = "text/css", View = "Bitsmith.ContentItemCSSView" });
                list.Add(new MimeMap() { Id = "vue", Extension = ".vue", Display = "vue", Name = "VUE", Mime = "text/vue", View = "Bitsmith.ContentItemVueJSView" });
                list.Add(new MimeMap() { Id = "sql", Extension = ".sql", Display = "sql", Name = "SQL", Mime = "text/sql", View = "Bitsmith.ContentItemSQLView" });
                list.Add(new MimeMap() { Id = "txt", Extension = ".txt", Display = "txt", Name = "TXT", Mime = "text/plain", View = "Bitsmith.ContentItemTextfileView" });
                list.Add(new MimeMap() { Id = "text", Extension = ".text", Display = "text", Name = "TEXT", Mime = "text/plain", View = "Bitsmith.ContentItemTextView" });
                list.Add(new MimeMap() { Id = "note", Extension = ".note", Display = "note", Name = "Note", Mime = "text/plain", View = "Bitsmith.ContentItemTextView" });
                list.Add(new MimeMap() { Id = "url", Extension = ".url", Display = "url", Name = "Url", Mime = "text/x-url", View = "Bitsmith.ContentItemUrlView" });


                FileSystemDataProvider.TryWrite<MimeMap>(list, out string message, Filepath);
            }

            return base.LoadData();
        }

        public override void Initialize()
        {
            Items = new ObservableCollection<MimeMapViewModel>(from x in Models select new MimeMapViewModel(x));
            Items.CollectionChanged += Maps_CollectionChanged1;
        }

        private void Maps_CollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as MimeMapViewModel;
                    if (vm != null)
                    {
                        Models.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as MimeMapViewModel;
                    if (vm != null)
                    {
                        Models.Remove(vm.Model);
                    }
                }
            }
        }



    }
}
