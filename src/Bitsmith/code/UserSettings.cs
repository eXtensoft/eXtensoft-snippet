using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    [Serializable]
    public class UserSettings
    {
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
        public string Machine { get; set; }
        public string Application { get; set; }
        public List<TypedItem> Items { get; set; } = new List<TypedItem>();

    }
}
