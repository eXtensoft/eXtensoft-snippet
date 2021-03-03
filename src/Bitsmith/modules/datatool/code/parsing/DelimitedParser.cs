using Bitsmith.Models;
using Bitsmith.Profiling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public abstract class DelimitedParser : Parser
    {
        public override string Extension => ".pipe";

        public abstract char Delimiter { get; }

        public override void Parse(TabularData tabularData)
        {
            List<string> lines = new List<string>();
            if (tabularData.Info != null)
            {
                lines = tabularData.Info.AllLines();
            }
            else if (!string.IsNullOrWhiteSpace(tabularData.Body))
            {
                lines = new List<string>(tabularData.Body.AllLines());
            }
            if (SetupDataTable(tabularData,lines))
            {
                tabularData.IsOkay = Execute(tabularData, lines);
            }
        }

        protected virtual bool SetupDataTable(TabularData tabularData, List<string> lines)
        {
            bool b = false;
            Profiler profiler = new Profiler();
            if (profiler.TryDiscover(lines, out DataTable dt))
            {
                tabularData.Data = dt;
                b = true;
            }
            return b;
        }

        public virtual bool Execute(TabularData tabularData, List<string> lines)
        {
            bool b = false;

            return b;
        }

    }
}
