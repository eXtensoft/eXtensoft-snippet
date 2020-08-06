using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Modules.Bible.Model
{
    public class Verse
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public List<VerseText> Text { get; set; }
    }
}
