using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public static class RolodexExtension
    {
        public static List<Card> Default(this List<Card> list)
        {
            Card item = new Card().Default();
            list.Add(item);
            return list;
        }

        public static Card Default(this Card model)
        {
            model.LastName = "nelson";
            model.FirstName = "R Todd";
            model.Id = Guid.NewGuid().ToString();
            return model;
        }
    }
}
