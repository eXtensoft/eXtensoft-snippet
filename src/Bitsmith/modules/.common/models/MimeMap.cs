using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class MimeMap
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string Name { get; set; }
        public string Mime { get; set; }
        public string Extension { get; set; }
        public string GroupName { get; set; } = "None";
        public string Image { get; set; } = "none";
        public List<Property> Properties { get; set; } = new List<Property>();


    }
}
