using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Profiling
{
    public class ProfileFields : KeyedCollection<string, ProfileField>
    {
        protected override string GetKeyForItem(ProfileField item)
        {
            return item.Name;
        }

        public void Profile(string fieldName, string value)
        {
            if (!Contains(fieldName))
            {
                Add(new ProfileField(fieldName, value));

            }
            else
            {
                this[fieldName].Profile(fieldName, value);
            }
        }

        public override string ToString()
        {
            return $"{this.Count()}";
        }
    }
}
