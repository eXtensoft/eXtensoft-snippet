using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace Bitsmith
{
    public static class ModuleExtensions
    {
        public static void EnsurePreference(this UserSettings settings, string groupName, string key, object value)
        {
            var found = settings.Items.FirstOrDefault((x) => 
            {
                return x.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase) &&
                x.Key.Equals(key, StringComparison.OrdinalIgnoreCase);
            });
            if (found != null)
            {
                found.Value = value;
                found.Effective = DateTime.Now;
            }
            else
            {
                settings.Items.Add(new TypedItem() { Key = key, Group = groupName, Effective = DateTime.Now, Value = value });
            }
        }

        public static bool TryGet<T>(this UserSettings settings, string groupName, string key, out T value)
        {
            bool b = false;
            value = default(T);
            if (settings.TryGet(groupName,key, out object obj))
            {
                try
                {
                    Type type = typeof(T);
                    if (type.IsEnum && type.IsEnumDefined(obj))
                    {
                        value = (T)Enum.Parse(type, obj.ToString());
                        b = true;
                    }
                    else
                    {
                        value = (T)obj;
                        b = true;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return b;
        }

        public static bool TryGet(this UserSettings settings, string groupName, string key, out object value)
        {
            bool b = false;
            value = null;
            var found = settings.Items.FirstOrDefault((x) => 
            { 
                return x.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase) && 
                x.Key.Equals(key, StringComparison.OrdinalIgnoreCase); 
            });
            if (found != null)
            {
                value = found.Value;
                b = true;
            }

            return b;
        }
    }
}
