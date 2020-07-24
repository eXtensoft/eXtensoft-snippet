using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class WorkEffortViewModel : GroupingViewModel<TimeEntry>
    {
        public WorkEffortViewModel(IEnumerable<TimeEntry> items)
        :base(items){
            Display = items.FirstOrDefault().Task.Display;
        }
    }
}
