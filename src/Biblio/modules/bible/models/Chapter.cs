using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Modules.Bible.Model
{
    public class Chapter
    {     
        public int Index { get; set; }
        public List<Verse> Verses { get; set; }
    }
}
