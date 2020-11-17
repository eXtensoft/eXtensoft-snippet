using Bitsmith.DataServices.Abstractions;
using Bitsmith.FullText;
using Bitsmith.Indexing;
using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Bitsmith.NaturalLanguage
{
    public class IndexerModule : Module<LanguageSettings>
    {
        private DataTable _IndexProfile;
        public DataTable IndexProfile
        {
            get
            {
                if (_IndexProfile == null)
                {
                     _IndexProfile = Indexer.Indexes.ToProfile();
                }
                return _IndexProfile;
            }
        }

        private TypedItem _SelectedLanguageOption;
        public TypedItem SelectedLanguageOption
        {
            get
            {
                return _SelectedLanguageOption;
            }
            set
            {
                _SelectedLanguageOption = value;
                OnPropertyChanged("SelectedLanguageOption");
            }
        }
       private ObservableCollection<TypedItem> LanguageOptions { get; set; }

        private ICollectionView _LanguageSelections;
        public ICollectionView LanguageSelections
        {
            get
            {
                if (_LanguageSelections == null)
                {
                    _LanguageSelections = CollectionViewSource.GetDefaultView(LanguageOptions);
                    _LanguageSelections.Filter = FilterTokens;
                }
                return _LanguageSelections;
            }
        }

        private bool FilterTokens(object obj)
        {
            bool b = false;
            var vm = obj as TypedItem;
            if (vm != null)
            {
                b = !Languages.Any(x => x.Language.Equals(vm.Key));
            }
            return b;
        }

        public Action<LanguageSettings> LanguageAdded { get; set; }

        public ObservableCollection<LanguageSettingsViewModel> Languages { get; set; }

        private LanguageSettingsViewModel _SelectedLanguage;
        public LanguageSettingsViewModel SelectedLanguage
        {
            get
            {
                return _SelectedLanguage;
            }
            set
            {
                _SelectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
            }
        }

        private ICommand _RemoveLanguageCommand;
        public ICommand RemoveLanguageCommand
        {
            get
            {
                if (_RemoveLanguageCommand == null)
                {
                    _RemoveLanguageCommand = new RelayCommand<LanguageSettingsViewModel>(
                    new Action<LanguageSettingsViewModel>(RemoveLanguage));
                }
                return _RemoveLanguageCommand;
            }
        }
        private void RemoveLanguage(LanguageSettingsViewModel viewModel)
        {
            Languages.Remove(viewModel);
        }


        private ICommand _AddLanguageCommand;
        public ICommand AddLanguageCommand
        {
            get
            {
                if (_AddLanguageCommand == null)
                {
                    _AddLanguageCommand = new RelayCommand(
                    param => AddLanguage(),
                    param => CanAddLanguage());
                }
                return _AddLanguageCommand;
            }
        }
        private bool CanAddLanguage()
        {
            return _SelectedLanguageOption != null;
        }
        private void AddLanguage()
        {
            var language = _SelectedLanguageOption.Key;
            var settings = new LanguageSettings().Default(language);
            LanguageSettingsViewModel vm = new LanguageSettingsViewModel(settings);
            Languages.Add(vm);

            if (LanguageAdded != null)
            {
                LanguageAdded(settings);
            }
        }


        public IContentIndexer Indexer { get; set; }

        private SettingsModule _Settings;
        

        public IndexerModule(IDataService dataService, SettingsModule settings)
        {
            DataService = dataService;
            _Settings = settings;
            AssembleLanguageSelections();
        }

        public override string Filepath()
        {
            return Path.Combine(AppConstants.SettingsDirectory, DataService.Filepath<LanguageSettings>());
        }

        private void AssembleLanguageSelections()
        {
            List<TypedItem> list = new List<TypedItem>();
            list.Add(new TypedItem() { Key = "en", Value = "English" });
            list.Add(new TypedItem() { Key = "fr", Value = "French" });
            list.Add(new TypedItem() { Key = "es", Value = "Spanish" });
            list.Add(new TypedItem() { Key = "de", Value = "German" });
            list.Add(new TypedItem() { Key = "it", Value = "Italian" });
            list.Add(new TypedItem() { Key = "zh", Value = "Chinese" });
            list.Add(new TypedItem() { Key = "el", Value = "Greek" });
            LanguageOptions = new ObservableCollection<TypedItem>(list);
        }

        public override void Initialize()
        {
            var path = Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles);
            DirectoryInfo directory = new DirectoryInfo(path);
            var readers = GetFileReaders();
            Indexer = new Indexer(directory, readers, Models) ;

            // okay, now setup the languages so that we can adjust language settings
            Languages = new ObservableCollection<LanguageSettingsViewModel>(from x in Models select new LanguageSettingsViewModel(x));
            Languages.CollectionChanged += Languages_CollectionChanged;


            //this is where the IContentIndexer Indexer should be built
            //var found = Models.FirstOrDefault(y => y.Language.Equals(AppConstants.Languages.English, StringComparison.OrdinalIgnoreCase));
            //if (found != null)
            //{
            //    var directory = Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles);
            //    var simple = new Indexer(found) { Directory = new DirectoryInfo(directory) };
            //    if (DataService != null)
            //    {
            //        string filepath = Path.Combine(AppConstants.ContentDirectory, DataService.Filepath<FullTextIndex>());
            //        if (File.Exists(filepath) && 
            //            DataService.TryRead<FullTextIndex>(filepath, out FullTextIndex model, out string message))
            //        {
            //            IsInitialized = true;
            //            simple.IsInitialized = true;
            //            simple.Indexes.Load(model.Indexes);
            //        }
            //        else
            //        {
            //            IsInitialized = false;
            //        }
            //    }
            //    Indexer = simple;

            //}          
        }

        private void Languages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as LanguageSettingsViewModel;
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
                    var vm = item as LanguageSettingsViewModel;
                    if (vm != null)
                    {
                        Models.Remove(vm.Model);
                    }
                }
            }
        }

        private List<IFileReader> GetFileReaders()
        {
            List<IFileReader> list = new List<IFileReader>();
            list.Add(new TextFileParser());
            list.Add(new MSWordFileParser());
            list.Add(new PdfFileReader());
            return list;
        }

        protected override bool LoadData()
        {
            bool b = false;
            var filepath = Filepath();
            if (!File.Exists(filepath))
            {
                Models.Add(new LanguageSettings().Default());
                if (!DataService.TryWrite<LanguageSettings>(Models, out string error, filepath))
                {
                    OnFailure("load Language Settings", error);
                }
            }
            b = DataService.TryRead<LanguageSettings>(filepath, out List<LanguageSettings> list, out string message);
            if (b)
            {
                if (!list.Any())
                {
                    list.Add(new LanguageSettings().Default());
                }
                Models = list;
            }
            else
            {
                OnFailure("", message);
            }
            return b;
        }

        protected override void SaveData()
        {
            base.SaveData();

            //if (DataService != null)
            //{
            //    FullTextIndex fti = new FullTextIndex()
            //    {
            //        Indexes = Indexer.Indexes.ToList(),
            //        CreatedAt = DateTime.Now
            //    };
            //    if (fti.Indexes != null &&
            //        fti.Indexes.Count > 0)
            //    {
            //        string filepath = Path.Combine(AppConstants.ContentDirectory, DataService.Filepath<FullTextIndex>());
            //        DataService.TryWrite<FullTextIndex>(fti, out string message, filepath);
            //    }
            //}
        }
    }
}
