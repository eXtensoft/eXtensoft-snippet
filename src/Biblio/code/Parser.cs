using Biblio.Modules.Bible.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Parsing
{
    public static class Parser
    {
        public static List<Book> Parse(string version, string language, string text)
        {

            List<Book> books = new List<Book>();

            if (!String.IsNullOrWhiteSpace(text))
            {
                HashSet<string> hsBooks = new HashSet<string>();
                int i = 1;

                List<string> lines = new List<string>(from line in text.Split(new char[] { '\r','\n' },StringSplitOptions.RemoveEmptyEntries) select line.TrimEnd('~'));
                lines.ForEach((line) => {
                    var parts = line.Split(new char[] { '|' });
                    if (parts.Count().Equals(4) &&
                        !string.IsNullOrWhiteSpace(parts[0]) &&
                        Int32.TryParse(parts[1], out int chapterNumber) &&
                        Int32.TryParse(parts[2], out int verseNumber) &&
                        !string.IsNullOrWhiteSpace(parts[3]))
                    {
                        VerseText verseText = new VerseText() { Text = parts[3], VersionId = version };
                        var book = books.FirstOrDefault(b => b.Name.Equals(parts[0]));
                        if (book == null)
                        {
                            book = new Book() { Index = i++, Name = parts[0], Chapters = new List<Chapter>() };
                            books.Add(book);
                        }
                        var chapter = book.Chapters.Find(c => c.Index.Equals(chapterNumber));
                        if (chapter == null)
                        {
                            chapter = new Chapter() { Index = chapterNumber, Verses = new List<Verse>() };
                            book.Chapters.Add(chapter);
                        }
                        var verse = chapter.Verses.FirstOrDefault(v => v.Index.Equals(verseNumber));
                        if (verse == null)
                        {
                            verse = new Verse() { Id = $"{book.Index}-{chapter.Index}-{verseNumber}",  Index = verseNumber, Text = new List<VerseText>() };
                            chapter.Verses.Add(verse);
                        }
                        verse.Text.Add(verseText);
                    }
                });
            }
            return books;
        }
    }
}

//Gen|1|1| In the beginning God created the heaven and the earth.~
//Gen|1|2| And the earth was without form, and void; and darkness was upon the face of the deep.And the Spirit of God moved upon the face of the waters.~
//Gen|1|3| And God said, Let there be light: and there was light.~
