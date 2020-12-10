using Bitsmith.DataServices.Abstractions;
using Bitsmith.FullText;
using Bitsmith.Indexing;
using Bitsmith.Models;
using Bitsmith.NaturalLanguage;
using Bitsmith.Search;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xaml;
using System.Xml.Linq;

namespace Bitsmith.ViewModels
{
    public class ContentModule : Module
    {

        private ICommand _AddBibleCommand;
        public ICommand AddBibleCommand
        {
            get
            {
                if (_AddBibleCommand == null)
                {
                    _AddBibleCommand = new RelayCommand(
                    param => AddBible(),
                    param => CanAddBible());
                }
                return _AddBibleCommand;
            }
        }
        private bool CanAddBible()
        {
            return true;
        }
        private void AddBible()
        {
            var verses = BibleManager.Ingest(Settings.SelectedDomain.Id, "kjvdat");
            foreach (var verse in verses)
            {
                AddContent(verse);
            }
        }


        public VirtualPathModule Paths { get; set; }

        public ObservableCollection<QueryViewModel> AllQueries { get; set; }

        private void PublishQuery(Query query)
        {
            query.QueryType = QueryTypeOption.Recent;
            var hash = query.GetHash();
            if (!AllQueries.Any(x => x.Hash.Equals(hash)))
            {
                AllQueries.Insert(0, new QueryViewModel(query));
                //AllQueries.Add(new QueryViewModel(query));
            }
        }

        private bool _IsRecentQueries = true;
        public bool IsRecentQueries
        {
            get { return _IsRecentQueries; }
            set
            {
                _IsRecentQueries = value;
                OnPropertyChanged("IsRecentQueries");
                Queries.Refresh();
            }
        }

        private ICollectionView _Queries;
        public ICollectionView Queries
        {
            get
            {
                if (AllQueries != null && _Queries == null)
                {
                    _Queries = CollectionViewSource.GetDefaultView(AllQueries);
                    _Queries.Filter = FilterQueries;
                }
                return _Queries;
            }
        }


        private bool FilterQueries(object o)
        {
            
            bool b = false;
            var vm = o as QueryViewModel;
            if (vm != null)
            {
                if (_IsRecentQueries && vm.Model.QueryType == QueryTypeOption.Recent)
                {
                    b = true;
                }
                else if(!_IsRecentQueries && vm.Model.QueryType != QueryTypeOption.Recent)
                {
                    b = true;
                }
            }
            return b;
        }

        private void AllQueries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Queries.Refresh();
        }


        private QueryViewModel _SelectedQuery;
        public QueryViewModel SelectedQuery
        {
            get { return _SelectedQuery; }
            set
            {
                _SelectedQuery = value;                     
                OnPropertyChanged("SelectedQuery");
                if (_SelectedQuery != null)
                {
                    SetSearch(_SelectedQuery);                   
                    ExecuteQuery();
                }
            }
        }

        private void SetSearch(QueryViewModel query)
        {
            QueryText = query.QueryText;
            SearchTypeOptions options = SearchTypeOptions.None;
            foreach (var item in query.Model.TokenQueries)
            {
                if (!options.HasFlag(item.SearchType))
                {
                    options = options | item.SearchType;
                }
            }
            if (options == SearchTypeOptions.Tag)
            {
                IsTagSearch = true;
            }
            else if(options == SearchTypeOptions.None)
            {
                IsTagSearch = true;
            }
            else if (options == SearchTypeOptions.FullText)
            {
                IsTagSearch = false;
            }
        }


        public IContentIndexer Indexer { get; set; }

        internal ContentManager ContentManager { get; set; }
        public ObservableCollection<MimeMapViewModel> Mimes { get; set; }


        private bool _IsTagsExpanded = false;
        public bool IsTagsExpanded
        {
            get
            {
                return _IsTagsExpanded;
            }
            set
            {
                _IsTagsExpanded = value;
                OnPropertyChanged("IsTagsExpanded");
            }
        }


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
                    _SelectedItem.Model.LastViewed();
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
            {// TODO, why did this null check become necessary?
                if (value != null)
                {
                    _SelectedTag = value;
                    OnPropertyChanged("SelectedTag");
                    SelectedIndex = 1;
                    ExecuteQuery(_SelectedTag.Key);
                }

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
        }
        private void Import()
        {
            int totalfiles = 0;
            int foundfiles = 0;
            List<string> orphanfiles = new List<string>();
            List<ContentItem> list = new List<ContentItem>();
            var xdoc = XDocument.Load(@"c:\data\snippets.xml", LoadOptions.None);
            foreach (XElement el in xdoc.Descendants("ContentItem"))
            {
                ContentItem item = new ContentItem() { Language = AppConstants.Languages.English };
                
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
                            if (ContentManager.TryInload(info, out string filename))
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
                    Value = Settings.SelectedDomain.Id
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
            var searchtype = IsTagSearch ? SearchTypeOptions.Tag : SearchTypeOptions.FullText;
            var query = new Query().Parse(_QueryText,searchtype,QueryOperatorOption.Or, Settings.SelectedDomain.Id);
            ExecuteQuery(query);
        }

        internal void ExecuteQuery(IPathNode node)
        {
            
            var query = new Query().Parse(node,Settings.SelectedDomain.Id);
            ExecuteQuery(query);
        }

        private void ExecuteQuery(string tag)
        {
            //if (!IsTagSearch && !IsTagsExpanded)
            //{
            //    IsTagsExpanded = true;
            //}
            var query = new Query().Parse(tag, SearchTypeOptions.Tag, QueryOperatorOption.Or, Settings.SelectedDomain.Id);
            ExecuteQuery(query);
        }

        private void ExecuteQuery(Query query)
        {
            SearchResults = query.Execute(model.Items, Indexer);
            PublishQuery(query);
        }

        /*
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

        }
        */

        public List<ContentItemViewModel> ExecuteTaskSearch(string taskId)
        {
            var list = new List<string> { $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}:{taskId}" };
            var found = model.Items.Where(x => x.Includes((from s in list select new QueryExpression(s)).ToList())).OrderBy(z => z.Display).ToList();
            return new List<ContentItemViewModel>(from f in found select new ContentItemViewModel(f));
        }

        public void HandleDomainSelected(DomainViewModel selected)
        {
            if (selected != null)
            {
                Resolver.Domain = selected;
                // clear selected tag
                // clear results;
                SearchResults = null;
                SelectedTag = null;
                Paths.SelectedDomainId = selected.Id;
            }
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
            //ctl.DataContext = this;
            ctl.DataContext = Workspace.Instance.ViewModel.Settings;
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
                Settings.SelectedDomain.Model, Mimes, 
                ContentManager, 
                out ContentItem newContent))
            {
                newContent.Language = GetSelectedLanguage();
                //newContent.Language = SelectedLanguage != null ? SelectedLanguage.Language : AppConstants.Languages.English;
                AddContent(newContent);
            }
        }


        public void AddContent(ContentItem item)
        {
            var domain = item.Properties.FirstOrDefault(x => x.Name.Equals($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}"));
            if (domain == null)
            {
                item.Properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}", Value = Settings.SelectedDomain.Id });
            }           
            if (Indexer != null)
            {
                Indexer.Index(item);
            }
            if (Paths != null)
            {
                Paths.Ensure(item);
            }
            model.Items.Add(item);

        }


        public TagResolver Resolver { get; set; }

        private ICommand _RemoveRecentTagCommand;
        public ICommand RemoveRecentTagCommand
        {
            get
            {
                if (_RemoveRecentTagCommand == null)
                {
                    _RemoveRecentTagCommand = new RelayCommand<string>(RemoveRecentTag);
                }
                return _RemoveRecentTagCommand;
            }
        }
        private bool CanRemoveRecentTag()
        {
            return true;
        }
        private void RemoveRecentTag(string tag)
        {
            var found = Resolver.RecentTags.FirstOrDefault(x => x.Key.Equals(tag));
            if (found != null)
            {
                var counter = found.Model.Counters.FirstOrDefault(y => y.Domain.Equals(Settings.SelectedDomain.Id));
                if (counter != null)
                {
                    found.Model.Counters.Remove(counter);
                }
                //Resolver.DomainExclusions[found.Domain].Add(found.Key);
                Resolver.RecentTags.Remove(found);
                
            }
        }


        private ICommand _RemovePopularTagCommand;
        public ICommand RemovePopularTagCommand
        {
            get
            {
                if (_RemovePopularTagCommand == null)
                {
                    _RemovePopularTagCommand = new RelayCommand<string>(RemovePopularTag);
                }
                return _RemovePopularTagCommand;
            }
        }

        private void RemovePopularTag(string tag)
        {
            var found = Resolver.PopularTags.FirstOrDefault(x => x.Key.Equals(tag));
            if (found != null)
            {
                var counter = found.Model.Counters.FirstOrDefault(y => y.Domain.Equals(Settings.SelectedDomain.Id));
                if (counter != null)
                {
                    found.Model.Counters.Remove(counter);
                }
                Resolver.Exclusions[found.Domain].Add(found.Key);
                Resolver.PopularTags.Remove(found);
            }
        }

        private bool _IsTagsRecentChecked;
        public bool IsTagsRecentChecked
        {
            get
            {
                return _IsTagsRecentChecked;
            }
            set
            {
                _IsTagsRecentChecked = value;
                OnPropertyChanged("IsTagsRecentChecked");
            }
        }




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




        protected override bool LoadData()
        {

            if (!File.Exists(Filepath()))
            {
                Content content = new Content().Default();
                if(!DataService.TryWrite<Content>(content, out string error, Filepath()))
                {
                    OnFailure(error);
                }               
            }

            bool b = DataService.TryRead<Content>(Filepath(), out model, out string message);
            if (!b)
            {
                OnFailure(message);
            }
            if (model != null)
            {
                model.Items.EnsurePaths();
            }

            EnsureLanguage(model.Items);
            return b;
        }

        private void EnsureLanguage(List<ContentItem> items)
        {
            items.ForEach((x) => {

                if (string.IsNullOrWhiteSpace(x.Language))
                {
                    x.Language = AppConstants.Languages.English;
                }
            });
        }

        public SettingsModule Settings { get; set; }

        private void HandleDomainAdded(Domain model)
        {            
            if(!Resolver.Exclusions.ContainsKey(model.Id))
            {
                var global = new List<string>().TagExclusions();
                Resolver.Exclusions.Add(model.Id, global);
            }
        }

        public ObservableCollection<LanguageSettingsViewModel> Languages { get; set; }


        private string GetSelectedLanguage()
        {
            var found = Languages.FirstOrDefault(v => v.IsSelected);
            if (found == null)
            {
                found = Languages.First();
            }
            return found != null ? found.Language : AppConstants.Languages.English;
        }

        public bool AreLanguagesVisible
        {
            get
            {
                return Languages.Count() > 1;
            }
        }

        public ContentModule(IDataService dataService, 
            SettingsModule settings, 
            IndexerModule indexer,
            VirtualPathModule paths)
        {
            DataService = dataService;
            Settings = settings;
            settings.DomainSelected = HandleDomainSelected;
            settings.DomainAdded = HandleDomainAdded;
            UserPreferences = Settings.UserPreferences;
            Indexer = indexer.Indexer;
            Languages = indexer.Languages;
            indexer.LanguageAdded = HandleLanguageAdded;
            Paths = paths;
            ContentManager = new ContentManager(Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles));
        }
        protected override string Filepath()
        {
            return Path.Combine(AppConstants.ContentDirectory, DataService.Filepath<Content>());
        }

        private void HandleLanguageAdded(LanguageSettings settings)
        {
            OnPropertyChanged("AreLanguagesVisible");
            SetSelectedLanguage(settings.Language);
        }

        private void SetSelectedLanguage(string language)
        {
            var found = Languages.FirstOrDefault(x => x.Language.Equals(language));
            if (found != null)
            {
                found.IsSelected = true;
            }
        }

        internal void Setup(MimeModule mimes)
        {
            base.Setup();

            if (model.Queries == null || model.Queries.Count == 0)
            {
                model.Queries = new List<Query>().Default(Settings.Settings.Domains);
            }
            AllQueries = new ObservableCollection<QueryViewModel>(from x in model.Queries select new QueryViewModel(x));

            Mimes = mimes.Items;
            Paths.Build(Settings, model.Items);
            ApplyPreferences();

            if (Indexer != null &&
                !Indexer.IsInitialized)
            {
                Indexer.Index(model.Items);
            }

            Resolver.Load(model.Items);
            
            if (UserPreferences.TryGet<string>(ModuleKey, "recent-tags", out string recentTags))
            {
                var tags = recentTags.Split(new char[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                Resolver.SetRecentTags(tags);
            }
        }


        protected override void ApplyPreferences()
        {
            var exclusions = new Dictionary<string, List<string>>();
            var global = new List<string>().TagExclusions();

            foreach (var domain in Settings.Domains)
            {
                List<string> domainexclusions = new List<string>();
                domainexclusions.AddRange(global);
                if (UserPreferences.TryGet<string>(ModuleKey,$"tag-exclusions:{domain.Id}",out string tagExclusions))
                {
                    if (!string.IsNullOrWhiteSpace(tagExclusions))
                    {
                        
                        foreach (var tag in tagExclusions.Split(new char[] { ';' },StringSplitOptions.RemoveEmptyEntries))
                        {
                            domainexclusions.Add(tag);
                        }
                    }
                }
                exclusions.Add(domain.Id, domainexclusions);
            }
            Resolver = new TagResolver(exclusions);

            if (UserPreferences.TryGet<ContentTypeOption>(ModuleKey,"content-type", out ContentTypeOption option))
            {
                Input.ContentType = option;
            }
            if (UserPreferences.TryGet<string>(ModuleKey,"selected-domain",out string id))
            {
                var found = Settings.Domains.FirstOrDefault(x => x.Id.Equals(id));
                if (found != null)
                {
                    Settings.SelectedDomain = found;
                }
                else
                {
                    Settings.SelectedDomain = Settings.Domains[0];
                }
            }
            else
            {
                Settings.SelectedDomain = Settings.Domains[0];
            }
            if (UserPreferences.TryGet<string>(ModuleKey,"last-opened", out string location) && Directory.Exists(location))
            {
                Application.Current.Properties[AppConstants.LastOpenedFileDialogFolderpath] = location;
            }
            if (UserPreferences.TryGet<string>(ModuleKey,"selected-language", out string language))
            {
                SetSelectedLanguage(language);
            }

            if (UserPreferences.TryGet<bool>(ModuleKey,"tags-expanded", out bool isTagsExpanded))
            {
                IsTagsExpanded = isTagsExpanded;
            }
            if (UserPreferences.TryGet<bool>(ModuleKey, "recent-tags-checked", out bool isRecentTagsChecked))
            {
                IsTagsRecentChecked = isRecentTagsChecked;
            }
            
        }

        internal override void SetPreferences()
        {
            var global = new List<string>().TagExclusions();
            foreach (var domain in Settings.Domains)
            {
                if (Resolver.Exclusions.ContainsKey(domain.Id))
                {
                    var domainexclusions = Resolver.Exclusions[domain.Id].Where(x => !global.Contains(x));
                    if (domainexclusions.Any())
                    {
                        UserPreferences.EnsurePreference(ModuleKey, $"tag-exclusions:{domain.Id}", String.Join(";",domainexclusions));
                    }
                }
            }
            if (Resolver.RecentTags.Count > 0)
            {
                var recenttags = string.Join(";", Resolver.RecentTags.Select(t => t.Model.Key));
                UserPreferences.EnsurePreference(ModuleKey, "recent-tags", recenttags);
            }


            UserPreferences.EnsurePreference(ModuleKey, "content-type", Input.ContentType);
            UserPreferences.EnsurePreference(ModuleKey, "selected-domain", Settings.SelectedDomain.Model.Id);
            var location = Application.Current.Properties[AppConstants.LastOpenedFileDialogFolderpath] as string;
            if (!String.IsNullOrWhiteSpace(location) && Directory.Exists(location))
            {
                UserPreferences.EnsurePreference(ModuleKey, "last-opened", location);
            }
            UserPreferences.EnsurePreference(ModuleKey, "tags-expanded", IsTagsExpanded);
            UserPreferences.EnsurePreference(ModuleKey, "recent-tags-checked", IsTagsRecentChecked);

            var language = GetSelectedLanguage();
            UserPreferences.EnsurePreference(ModuleKey, "selected-language", language);


        }

        protected override bool SaveData()
        {

            if (model !=null)
            {
                model.Items.CleansePaths();
                var removals = model.Items.Where(x => x.IsRemove).ToList();
                if (removals.Count > 0)
                {
                    string directory = Path.Combine(AppConstants.ContentDirectory,AppConstants.ContentFiles);
                    foreach (var removal in removals)
                    {
                        if (!removal.Mime.Equals("text") && 
                            !String.IsNullOrWhiteSpace(removal.Mime) && 
                            !removal.Mime.Equals("url") && 
                            !removal.Mime.Equals("text/credential"))
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

                if (!DataService.TryWrite<Content>(model, out string message, Filepath()))
                {
                    OnFailure(message);
                    return false;
                }
            }
            return true;
        }

    }
}
