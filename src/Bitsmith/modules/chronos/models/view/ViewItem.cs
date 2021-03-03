using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models.Views
{
    public class ViewItem
    {
        public string TipDisplay { get { return ToString(); } set { } }
        public ViewItem Master { get; set; }
        public string Id { get; set; }
        public string Display { get; set; }
        public int Level { get; set; }
    }
}
