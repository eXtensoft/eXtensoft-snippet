using Bitsmith.FullText;
using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Bitsmith.ViewModels
{
    public class ContentModule : Module
    {

        public TextIndexer Indexer { get; set; } = new TextIndexer();

        private ContentManager _ContentManager;
        public ObservableCollection<MimeMapViewModel> Mimes { get; set; }

        private int _SelectedIndex = 1;
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        private List<string> _SearchTerms = new List<string>();
        public List<string> SearchTerms
        {
            get { return _SearchTerms; }
            set
            {
                _SearchTerms = new List<string>();
                OnPropertyChanged("SearchTerms");
                OnPropertyChanged("SearchResultCount");
                OnPropertyChanged("SearchDisplay");
            }
        }

        private List<ContentItemViewModel> _SearchResults;
        public List<ContentItemViewModel> SearchResults 
        {
            get { return _SearchResults; }
            set
            {
                _SearchResults = value;
                if (value != null)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(SearchResults);
                    view.GroupDescriptions.Add(new PropertyGroupDescription("ContentType"));
                    SelectedIndex = 0;
                }
                OnPropertyChanged("SearchResults");
                OnPropertyChanged("SearchResultsCount");
                OnPropertyChanged("SearchDisplay");
            }
        }

        public int SearchResultsCount
        {
            get
            {
                return _SearchResults != null ? _SearchResults.Count : 0;
            }
            set { }
        }

        public string SearchDisplay
        {
            get
            {
                if (_SearchTerms.Count > 0)
                {
                    string s = IsTagSearch ? "Tag Search" : "Full Text Search";
                    return $"{s}: {SearchResultsCount} results";
                }
                else
                {
                    return string.Empty;
                }
                
            }
            set { }
        }

        private ContentItemViewModel _SelectedItem;
        public ContentItemViewModel SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
                if (value != null)
                {
                    dynamic param = new ExpandoObject();
                    param.Title = _SelectedItem.Display;
                    param.Control = Mimes.Resolve(value);
                    Workspace.Instance.ViewModel.Overlay.SetOverlay(AppConstants.OverlayContent, param);
                }
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
                SelectedIndex = 1;
                ExecuteQuery(_SelectedTag.Key);
            }
        }


        private bool _IsTagSearch = true;
        public bool IsTagSearch
        {
            get
            {
                return _IsTagSearch;
            }
            set
            {
                _IsTagSearch = value;
                OnPropertyChanged("IsTagSearch");
            }
        }

        private string _QueryText;
        public string QueryText
        {
            get
            {
                return _QueryText;
            }
            set
            {
                _QueryText = value;
                OnPropertyChanged("QueryText");
                SearchResults = null;
                if (!String.IsNullOrWhiteSpace(_QueryText) && SelectedIndex == 1)
                {
                    SelectedIndex = 0;
                }
            }
        }


        private ICommand _ClearQueryCommand;
        public ICommand ClearQueryCommand
        {
            get
            {
                if (_ClearQueryCommand == null)
                {
                    _ClearQueryCommand = new RelayCommand(
                    param => ClearQuery(),
                    param => CanClearQuery());
                }
                return _ClearQueryCommand;
            }
        }
        private bool CanClearQuery()
        {
            return !String.IsNullOrWhiteSpace(QueryText);
        }


        private static Dictionary<string, string> cp = new Dictionary<string, string>()
        {
            { "text/plain", "txt"}, // is file based
            { "text/snippet", "text"},
            { "resource/url", "url"},
            { "application/msword", "word" }, //
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel" }, //
            { "application/pdf", "pdf" }, //
            { "image/png", "png" }, //
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "word" }, //
            { "application/vnd.visio", "visio" }, //
            { "image/jpeg","jpg" }, //
            { "text/credential", "cred" },
        };

        private static List<string> exc = new List<string>()
        {
            "28211d7a-a54c-413b-bbc2-0ad6363edc0e",
            "21574fd9-0c7b-4342-9074-5974baf598be",
            "2bdd1d41-59c3-4ff2-b15e-889f5f881219",
            "59ff90bc-6e19-4454-8c92-43114d2128bb",
            "TDS",
            "CreatedBy",
            "updated",
            "Common",
        };
        private void ClearQuery()
        {
            QueryText = string.Empty;
            int totalfiles = 0;
            int foundfiles = 0;
            List<string> orphanfiles = new List<string>();
            List<ContentItem> list = new List<ContentItem>();
            var xdoc = XDocument.Load(@"c:\data\snippets.xml", LoadOptions.None);
            foreach (XElement el in xdoc.Descendants("ContentItem"))
            {
                ContentItem item = new ContentItem();
                
                item.Id = el.Attribute("id").Value;
                var display = el.Element("Title");
                if (display != null)
                {
                    item.Display = display.Value;
                }
                else
                {

                }

                item.Body = el.Element("Text").Value;
                string mime = el.Attribute("mime").Value;
                if (cp.ContainsKey(mime))
                {
                    item.Mime = cp[mime];
                    if (mime == "text/snippet" || mime == "resource/url" || mime == "text/credential")
                    {

                    }
                    else // file based
                    {
                        totalfiles++;
                        string filepath = Path.Combine(@"c:\data\content.directory", item.Body);
                        FileInfo info = new FileInfo(filepath);
                        if (info.Exists)
                        {
                            foundfiles++;
                            if (_ContentManager.TryInload(info, out string filename))
                            {
                                var found = Mimes.FirstOrDefault(x => x.Extension.Equals(info.Extension, StringComparison.OrdinalIgnoreCase));
                                if (found != null)
                                {
                                    item.Mime = found.Id;
                                }
                                else
                                {
                                    item.Mime = info.Extension.TrimStart('.').ToLower();
                                }
                                item.Body = filename;
                                item.Properties.Add(new Property()
                                {
                                    Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}",
                                    Value = item.Mime
                                });
                            }

                        }
                        else
                        {
                            orphanfiles.Add(item.Body);
                        }
                    }
                }
                else
                {

                }

                List<string> tags = new List<string>();
                foreach (var tag in el.Descendants("Tag"))
                {
                    string id = tag.Attribute("id").Value;
                    string key = tag.Attribute("key").Value;

                    if (!exc.Contains(id))
                    {
                        
                        if (tag.HasElements)
                        {
                            key = $"{key}:{tag.Element("Value").Value}";
                        }
                        tags.Add(key);
                        
                    } 
                    else if (id.Equals("28211d7a-a54c-413b-bbc2-0ad6363edc0e")) // created
                    {
                        if (tag.HasElements && DateTime.TryParse(tag.Element("Value").Value,out DateTime created))
                        {
                            item.Properties.Add(new Property() 
                            { 
                                Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}", 
                                Value = created
                            });
                        }
                    }
                    else if (id.Equals("21574fd9-0c7b-4342-9074-5974baf598be")) // created by
                    {
                        if (tag.HasElements)
                        {
                            item.Properties.Add(new Property()
                            {
                                Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}",
                                Value = tag.Element("Value").Value
                            });
                        }
                    }
                    else if(id.Equals("2bdd1d41-59c3-4ff2-b15e-889f5f881219")) // updated
                    {
                        if (tag.HasElements && DateTime.TryParse(tag.Element("Value").Value, out DateTime updated))
                        {
                            item.Properties.Add(new Property()
                            {
                                Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}",
                                Value = updated
                            });
                        }
                    }
                }
                item.Properties.Add(new Property()
                {
                    Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}",
                    Value = SelectedDomain.Id
                });

                item.Properties.AddRange(Resolver.Resolve(tags));

                list.Add(item);
            }
            list.ForEach(x => {
                model.Items.Add(x);
            });

        }

        private ICommand _ExecuteQueryCommand;
        public ICommand ExecuteQueryCommand
        {
            get
            {
                if (_ExecuteQueryCommand == null)
                {
                    _ExecuteQueryCommand = new RelayCommand(
                    param => ExecuteQuery(),
                    param => CanExecuteQuery());
                }
                return _ExecuteQueryCommand;
            }
        }
        private bool CanExecuteQuery()
        {
            return !String.IsNullOrWhiteSpace(QueryText);
        }
        private void ExecuteQuery()
        {
            SearchTerms = new List<string>();

            if (!string.IsNullOrWhiteSpace(_QueryText))
            {
                _SearchTerms = _QueryText.SplitTrimLower().NormalizeQueryKeys();                
            }
            else
            {
                _SearchTerms.Add("all");
            }
            
            SearchResults = Search(_SearchTerms);

        }

        public List<ContentItemViewModel> ExecuteTaskSearch(string taskId)
        {
            return Search(new List<string> { $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}:{taskId}" });
        }


        private void ExecuteQuery(string tag)
        {
            SearchResults = Search(new List<string>() { tag });
        }

        private List<ContentItemViewModel> Search(List<string> list)
        {
            List<ContentItemViewModel> result = null;
            
            if (list.Contains("all"))
            {
                result = new List<ContentItemViewModel>(from x in model.Items select new ContentItemViewModel(x));
            }
            else if(list.Contains("url"))
            {
                
            }
            else if(list.Contains("files"))
            {

            }
            else if(list.Contains("removals"))
            {

            }
            else if(list.Contains("today"))
            {

            }
            else if (list.Contains("recent"))
            {

            }
            else
            {
                if (IsTagSearch)
                {
                    var found = model.Items.Where(x => x.Includes((from s in list select new QueryExpression(s)).ToList())).OrderBy(z => z.Display).ToList();
                    result = new List<ContentItemViewModel>(from f in found select new ContentItemViewModel(f));
                }
                else // is fulltext search
                {
                    IEnumerable<string> ids = Indexer.Query(list);
                    var found = model.Items.Where(x => ids.Contains(x.Id)).OrderBy(z=>z.Display).ToList();
                    result = new List<ContentItemViewModel>(from f in found select new ContentItemViewModel(f) {SearchTerm = list[0] });
                }
                // for a given domain, execute a query;
                // either tag or fulltext
            }

            if (result == null)
            {
                result = new List<ContentItemViewModel>(from x in model.Items select new ContentItemViewModel(x));
            }

            return result;


        }

        


        public ObservableCollection<DomainViewModel> Domains { get; set; }

        private DomainViewModel _SelectedDomain;
        public DomainViewModel SelectedDomain
        {
            get
            {
                return _SelectedDomain;
            }
            set
            {
                _SelectedDomain = value;
                OnPropertyChanged("SelectedDomain");
            }
        }

        private ICommand _AddDomainCommand;
        public ICommand AddDomainCommand
        {
            get
            {
                if (_AddDomainCommand == null)
                {
                    _AddDomainCommand = new RelayCommand(
                    param => AddDomain(),
                    param => CanAddDomain());
                }
                return _AddDomainCommand;
            }
        }

        internal void Setup(VirtualPathModule paths, MimeModule mimes)
        {
            base.Setup();
            Mimes = mimes.Items;
            List<DomainPathMapViewModel> additions = new List<DomainPathMapViewModel>();
            List<DomainViewModel> list = new List<DomainViewModel>();
            foreach (var domain in model.Domains)
            {
                var item = paths.Items.FirstOrDefault(p => p.Id.Equals(domain.Id, StringComparison.OrdinalIgnoreCase));
                if (item == null)
                {
                    item = new DomainPathMapViewModel(new DomainPathMap()
                    {
                        Id = domain.Id,
                        Display = domain.Name,
                        Slug = domain.Name.ToLower(),
                        Path = domain.Name.ToLower()
                    });
                    additions.Add(item);
                }
                list.Add(new DomainViewModel(domain,item));
            }
            additions.ForEach(x => { paths.Items.Add(x); });

            Domains = new ObservableCollection<DomainViewModel>(list);
            Domains.CollectionChanged += Domains_CollectionChanged;
            SelectedDomain = Domains[0];
        }


        private bool CanAddDomain()
        {
            return true;
        }
        private void AddDomain()
        {
            Domains.Add(new DomainViewModel(new Domain().Default(DateTime.Now,Guid.NewGuid().ToString().ToLower())));
        }

        private ICommand _ViewDomainsCommand;
        public ICommand ViewDomainsCommand
        {
            get
            {
                if (_ViewDomainsCommand == null)
                {
                    _ViewDomainsCommand = new RelayCommand(
                    param => ViewDomains(),
                    param => CanViewDomains());
                }
                return _ViewDomainsCommand;
            }
        }
        private bool CanViewDomains()
        {
            return true;
        }
        private void ViewDomains()
        {
            Control ctl = new DomainsView();
            ctl.DataContext = this;
            dynamic param = new System.Dynamic.ExpandoObject();
            param.Title = "Content Domains";
            param.Control = ctl;
            Workspace.Instance.ViewModel.Overlay.SetOverlay(AppConstants.OverlayContent, param);
        }




        public NewContentViewModel Input { get; set; } = new NewContentViewModel();


        private ICommand _SelectFileCommand;
        public ICommand SelectFileCommand
        {
            get
            {
                if (_SelectFileCommand == null)
                {
                    _SelectFileCommand = new RelayCommand(
                    param => SelectFile(),
                    param => CanSelectFile());
                }
                return _SelectFileCommand;
            }
        }
        private bool CanSelectFile()
        {
            return true;
        }
        private void SelectFile()
        {
            string candidate = String.Empty;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            string directory = Application.Current.Properties[AppConstants.LastOpenedFileDialogFolderpath] as string;
            if (!String.IsNullOrEmpty(directory))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    if (dir != null && dir.Exists)
                    {
                        directory = dir.FullName;
                    }
                    else
                    {
                        directory = String.Empty;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    directory = String.Empty;
                }

            }
            dialog.InitialDirectory = ((!String.IsNullOrEmpty(directory) && Directory.Exists(directory))) ? 
                directory : AppDomain.CurrentDomain.BaseDirectory;

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                Input.Filepath = dialog.FileName;
                FileInfo info = new FileInfo(dialog.FileName);
                Input.SetFile(info);
                Application.Current.Properties[AppConstants.LastOpenedFileDialogFolderpath] = info.Directory.FullName;
            }

        }


        private ICommand _AddContentCommand;
        public ICommand AddContentCommand
        {
            get
            {
                if (_AddContentCommand == null)
                {
                    _AddContentCommand = new RelayCommand(
                    param => AddContent(),
                    param => CanAddContent());
                }
                return _AddContentCommand;
            }
        }
        private bool CanAddContent()
        {
            return Input != null ? Input.Validate() : false;
        }
        private void AddContent()
        {
            if (Input.TryBuild(Resolver, 
                SelectedDomain.Model, Mimes, 
                _ContentManager, 
                out ContentItem newContent))
            {
                AddContent(newContent);
            }
        }

        public void AddContent(ContentItem item)
        {
            model.Items.Add(item);
        }


        public TagResolver Resolver { get; set; } = new TagResolver();

        public string Id
        {
            get { return model.Id; }
            set { }
        }

        private Content model;
        public Content Content
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                OnPropertyChanged("Content");
            }
        }

        public override string Filepath => FileSystemDataProvider.Filepath<Content>();





        private ICommand _SaveWorkspaceCommand;
        public ICommand SaveWorkspaceCommand
        {
            get
            {
                if (_SaveWorkspaceCommand == null)
                {
                    _SaveWorkspaceCommand = new RelayCommand(
                    param => SaveWorkspace(),
                    param => CanSaveWorkspace());
                }
                return _SaveWorkspaceCommand;
            }
        }



        internal bool CanSaveWorkspace()
        {
            return true;
        }
        internal void SaveWorkspace()
        {
            SaveData();
        }


        protected override bool LoadData()
        {
            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Content content = new Content().Default();
                if(!FileSystemDataProvider.TryWrite<Content>(content, out string error, filepath))
                {
                    OnFailure(error);
                }               
            }

            bool b = FileSystemDataProvider.TryRead<Content>(Filepath, out model, out string message);
            if (!b)
            {
                OnFailure(message);
            }

            return b;
        }

        public override void Initialize()
        {
            LoadTagResolver();
            this.IndexContent();
        }

        private void Domains_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as DomainViewModel;
                    if (vm != null)
                    {
                        model.Domains.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as DomainViewModel;
                    if (vm != null)
                    {
                        var found = model.Domains.FirstOrDefault(x => x.Id.Equals(vm.Id, StringComparison.OrdinalIgnoreCase));
                        if (found != null)
                        {
                            model.Domains.Remove(found);
                        }
                    }
                }
            }
        }


        public ContentModule()
        {
            var contentdirectory = "content-files";
            Application.Current.Properties[AppConstants.ContentDirectory] = contentdirectory;
            _ContentManager = new ContentManager(contentdirectory);
        }

        protected override bool SaveData()
        {
            if (model !=null)
            {                
                var removals = model.Items.Where(x => x.IsRemove).ToList();
                if (removals.Count > 0)
                {
                    string directory = Application.Current.Properties[AppConstants.ContentDirectory] as string;
                    foreach (var removal in removals)
                    {
                        if (!removal.Mime.Equals("text") && 
                            !String.IsNullOrWhiteSpace(removal.Mime) && 
                            !removal.Mime.Equals("url"))
                        {
                            string filepath = System.IO.Path.Combine(directory, removal.Body);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }                  
                        model.Items.Remove(removal);
                    }
                }

                if (!FileSystemDataProvider.TryWrite<Content>(model, out string message, Filepath))
                {
                    OnFailure(message);
                    return false;
                }
            }
            return true;
        }



        private void LoadTagResolver()
        {
            Resolver.Load(model.Items);
        }

    }
}
