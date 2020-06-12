using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.BusinessProcess
{
    public class Workflow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public StateMachine Machine { get; set; }
    }
}
