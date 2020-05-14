using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
