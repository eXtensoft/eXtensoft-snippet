using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bitsmith.Parsing
{
    public class XmlParser : Parser
    {
        public override string Extension => ".xml";

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

        }
    }
}
