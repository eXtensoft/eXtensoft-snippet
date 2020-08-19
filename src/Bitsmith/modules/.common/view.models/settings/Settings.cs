using Bitsmith.BusinessProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bitsmith
{
    public class Settings
    {

        public List<Workflow> Workflows { get; set; }

        [XmlElement("Preferences")]
        public List<UserSettings> UserPreferences { get; set; }


        public Settings()
        {

        }

    }
}
