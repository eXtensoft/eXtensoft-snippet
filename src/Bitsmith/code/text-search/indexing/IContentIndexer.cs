using Bitsmith.FullText;
using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Indexing
{
    public interface IContentIndexer
    {
        int Delta { get; set; }
        bool IsInitialized { get;  }
        void Index(ContentItem contentItem); // contentItem.Properties contains a language tag
        void Index(IEnumerable<ContentItem> contentItems);

        IEnumerable<string> Query(IEnumerable<string> tokens, string language = "en");

        IEnumerable<string> Query(string token, string language = "en");

        TextIndexes Indexes { get; }

    }
}
