using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class DataField
    {
        public int Position { get; set; }
        public string Display { get; set; }
        public string Name { get; set; }
        public string FieldType { get; set; }
        public object Value { get; set; }
    }
}
