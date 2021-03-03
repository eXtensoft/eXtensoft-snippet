using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Profiling
{
    public class ProfileField : KeyedCollection<string,ProfileItem>
    {
        public string Name { get { return this[0].FieldName; } }

        private readonly Func<string, string> _Resolver;

        public ProfileField(string fieldName, string value,Func<string,string> resolver)
        {
            _Resolver = resolver;
            Profile(fieldName, value);
        }
        public ProfileField(string fieldName, string value)
        {
            Profile(fieldName, value);
        }
        public ProfileField(string fieldName)
        {
            Profile(fieldName,string.Empty);
        }

        protected virtual string ResolveValue(string value)
        {
            if (_Resolver != null)
            {
                return _Resolver(value);
            }
            else
            {
                return value;
            }
            
        }

        protected override string GetKeyForItem(ProfileItem item)
        {
            return item.Value;
        }

        public void Profile(string fieldName, string value)
        {
            var resolved = ResolveValue(value);
            if (!Contains(value))
            {
                
                Add(new ProfileItem() { FieldName = fieldName, Value = resolved, Count = 1 });
            }
            else
            {
                this[resolved].Count++;
            }
        }

        
        public override string ToString()
        {
            return $"{Name}: {Count}";
        }


    }
}
