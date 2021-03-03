using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class Card
    {
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
