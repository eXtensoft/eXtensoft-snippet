using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Profiling
{
    public class ProfileItem
    {
        public string FieldName { get; set; }
        public string Value { get; set; }       
        public int Count { get; set; }
        public override string ToString()
        {
            return $"{Value}: {Count}";
        }
    }

}
