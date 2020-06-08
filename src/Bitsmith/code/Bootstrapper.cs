using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Bitsmith
{
    public static class Bootstrapper
    {

        public static Workspace Workspace()
        {
            return new Workspace();
        }

        internal static Data Data()
        {
            return new Data();
        }

        public static StateManager StateMachine()
        {
            XDocument doc = XDocument.Parse(Resources.statemachine);
            return new StateManager()
            {
                Machine = new StateMachine(doc)
            };
        }
    }
}
