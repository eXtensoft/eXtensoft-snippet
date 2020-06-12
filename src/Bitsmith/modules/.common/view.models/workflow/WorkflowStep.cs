using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class WorkflowStep
    {
      
        public bool IsTransition { get; set; }
        public string Display { get; set; }
        public string Name { get; set; }
    }
}
