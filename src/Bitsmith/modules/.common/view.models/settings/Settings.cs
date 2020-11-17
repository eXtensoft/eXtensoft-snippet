using Bitsmith.BusinessProcess;
using Bitsmith.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bitsmith
{
    public class Settings
    {
        [XmlElement("Domain")]
        public List<Domain> Domains { get; set; } = new List<Domain>();
        [XmlElement("Workflow")]

        public List<Workflow> Workflows { get; set; }

        [XmlElement("Preferences")]
        public List<UserSettings> UserPreferences { get; set; }

        public Settings()
        {

        }

    }
}
