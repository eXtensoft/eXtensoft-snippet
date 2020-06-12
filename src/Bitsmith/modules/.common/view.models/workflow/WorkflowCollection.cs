
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.BusinessProcess
{
    public class WorkflowCollection : KeyedCollection<string, Workflow>
    {
        protected override string GetKeyForItem(Workflow item)
        {
            return item.Id;
        }
    }
}
