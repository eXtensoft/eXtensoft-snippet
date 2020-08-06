using Biblio.Modules.Bible.Model;
using Biblio.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio
{
    public class VerseRepo : KeyedCollection<string, TermViewModel>
    {
        protected override string GetKeyForItem(TermViewModel item)
        {
            return item.Id;
        }

        public void AddRange(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                foreach (var chapter in book.Chapters)
                {
                    foreach (var verse in chapter.Verses)
                    {
                        if (!Contains(verse.Id))
                        {
                            TermViewModel vm = new TermViewModel(verse) { Book = book.Name, Chapter = chapter.Index, };
                            Add(vm);
                        }
                    }
                }
            }
        }
    }
}
