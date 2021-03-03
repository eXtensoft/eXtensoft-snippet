using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Profiling
{
    public class Profiler
    {
        public int Count { get; set; }

        public ProfileFields Fields { get; set; } = new ProfileFields();

        public void Profile<T>(IEnumerable<T> items, Action<T, Profiler> extract)
        {
            foreach (var item in items)
            {
                Count++;
                extract(item, this);
            }
        }

        public void Profile(string fieldName, string value)
        {
            Fields.Profile(fieldName, value);
        }

        public Profiler()
        {
        }

        public bool TryDiscover(IEnumerable<string> lines, out DataTable dt)
        {
            bool b = false;
            dt = new DataTable();


            return b;
        }

    }
}
