using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public static class BibleManager
    {



        public static IEnumerable<ContentItem> Ingest(string domainId, string version)
        {
            var now = DateTime.Now;
            List<ContentItem> items = new List<ContentItem>();

            List<string> books = new List<string>();
            HashSet<string> hs = new HashSet<string>();

            List<string> list = new List<string>(Resources.kjvdat.Split(new char[] { '\n','\r'}, StringSplitOptions.RemoveEmptyEntries));
            list.ForEach(line => {
                string[] parts = line.Split(new char[] { '|' });
                string book = parts[0];
                int chapter = Int32.Parse(parts[1]);
                int verse = Int32.Parse(parts[2]);
                string text = parts[3].Trim();

                if (hs.Add(parts[0]))
                {
                    books.Add(parts[0]);
                }
                ContentItem contentItem = new ContentItem()
                {
                    Language = "en",
                    Id = Guid.NewGuid().ToString(),
                    Mime = "text",
                    Scope = ScopeOption.None,
                    Body = text,
                    Display = $"{book} {chapter}:{verse} ({version})"
                };
                contentItem.Properties.Add(new Property() { Name = "x-domain", Value = domainId });
                contentItem.Properties.Add(new Property() { Name = "x-created-at", Value = now });
                contentItem.Paths.Add($"bible/{version}/{book}");
                contentItem.Properties.Add(new Property() { Name = "book", Value = book });
                contentItem.Properties.Add(new Property() { Name = "version", Value = version });

                //contentItem.Properties.Add(new Property() { Name = "", Value = "" });
                //contentItem.Properties.Add(new Property() { Name = "", Value = "" });
                //contentItem.Properties.Add(new Property() { Name = "", Value = "" });
                items.Add(contentItem);
                
            });

            return items;
        }
    }
}
