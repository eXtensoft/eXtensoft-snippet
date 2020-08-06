using Biblio.FullText;
using Biblio.Modules.Bible.Model;
using Biblio.Parsing;
using Biblio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Biblio.Modules
{
    public class BibleModule : Module
    {
        public TextIndexer Indexer { get; set; } = new TextIndexer();

        public List<Book> Books { get; set; }

        public VerseRepo Verses { get; set; } = new VerseRepo();

        private List<TermViewModel> _SearchResults;
        public List<TermViewModel> SearchResults
        {
            get
            {
                return _SearchResults;
            }
            set
            {
                _SearchResults = value;
                _SearchResults.ForEach(r => r.Set(_Terms, _SelectedVersion));
                OnPropertyChanged("SearchResults");
            }
        }

        private List<string> _Terms = new List<string>();
        public string Terms
        {
            get { return string.Join(", ",_Terms); }
            set
            {
                _Terms.Clear();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var parts = value.Split(new char[] { ' ',',',';','\r','\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        _Terms.Add(part.Trim());
                    }
                }
                OnPropertyChanged("Terms");
            }
        }

        private string _SelectedVersion = "KJV";
        public string SelectedVersion
        {
            get { return _SelectedVersion; }
            set
            {
                _SelectedVersion = value;
                OnPropertyChanged("SelectedVersion");
            }
        }



        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new RelayCommand(
                    param => Search(),
                    param => CanSearch());
                }
                return _SearchCommand;
            }
        }
        private bool CanSearch()
        {
            return _Terms != null && _Terms.Count > 0;
        }

        private void ImportCommand()
        {
            Search();
        }


        private void Search()
        {
            Search(_Terms);
        }
        private void Search(List<string> list)
        {
            IEnumerable<string> ids = Indexer.Query(list);
            SearchResults = Verses.Where(v => ids.Contains(v.Id)).ToList();
       }

        public override void Initialize()
        {
            this.IndexContent();
        }

        protected override bool LoadData()
        {
            var text = Scripture.kjvdat;
            var language = "english";
            var version = _SelectedVersion;
            Books = Parser.Parse(version, language, text);
            Verses.AddRange(Books);
            //GenericObjectManager.WriteGenericList<Book>(books, @"books-of-the-bible.xml");
            return true;
        }
    }
}
