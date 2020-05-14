using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public static class ModelExtensions
    {
        public static List<DomainPathMap> Default(this List<DomainPathMap> list)
        {

            var map = new DomainPathMap().Default();
            list.Add(map);
            return list;
        }


        public static DomainPathMap Default(this DomainPathMap pathMap)
        {
            pathMap.Items = pathMap.Items.Default();
            pathMap.Display = "Default";
            pathMap.Id = AppConstants.Default;
            

            return pathMap;
        }

        public static List<PathNode> Default(this List<PathNode> list)
        {
            list.Add(new PathNode().Default());
            return list;
        }


        public static PathNode Default(this PathNode model)
        {
            model.Path = $"/paths/{Environment.UserName}";
            model.Slug = $"/paths/{Environment.UserName.ToLower()}";
            model.Display = Environment.UserName;
            return model;
        }

        public static string ToKebab(this string text)
        {
            string output = !String.IsNullOrWhiteSpace(text) ? text.Trim().Replace("  ", " ").Replace(" ", "-").ToLower() : string.Empty;
            return output;
        }

    }
}
