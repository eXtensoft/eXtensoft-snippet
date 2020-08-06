using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Modules.Bible.Model
{
    public class Book
    {
        public int Index { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
