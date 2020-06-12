using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.BusinessProcess
{
    public class Builder
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<string> EndStates { get; set; }
        public string BeginState { get; set; }

        public List<State> States { get; set; }

        public List<Transition> Transitions { get; set; }
    }
}
