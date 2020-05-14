using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public static class TypeResolver
    {
        public static object Resolve(string stringValue)
        {
            if (Int32.TryParse(stringValue, out int i))
            {
                return i;
            }
            else if (Double.TryParse(stringValue, out double dbl))
            {
                return dbl;
            }
            else if(DateTime.TryParse(stringValue, out DateTime dte))
            {
                return dte;
            }

            return stringValue.Trim();
        }
    }
}
