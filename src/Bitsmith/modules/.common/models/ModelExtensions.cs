using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
            list.Add(new PathNode()
            {
                Path = $"/{AppConstants.Paths.Content}",
                Slug = $"/{AppConstants.Paths.Content}",
                Display = $"{AppConstants.Paths.Content.ToTitleCase()}"
            });
            list.Add(new PathNode()
            {
                Path = $"/{AppConstants.Paths.Files}",
                Slug = $"/{AppConstants.Paths.Files}",
                Display = $"{AppConstants.Paths.Files.ToTitleCase()}"
            });
            list.Add(new PathNode()
            {
                Path = $"/{AppConstants.Paths.Default}",
                Slug = $"/{AppConstants.Paths.Default}",
                Display = $"{AppConstants.Paths.Default.ToTitleCase()}"
            });
            return list;
        }


        public static PathNode Default(this PathNode model)
        {
            model.Path = $"/{AppConstants.Paths.Content}";
            model.Slug = $"/{AppConstants.Paths.Content}";
            model.Display = Environment.UserName;
            return model;
        }

        public static string ToKebab(this string text)
        {
            string output = !String.IsNullOrWhiteSpace(text) ? text.Trim().Replace("  ", " ").Replace(" ", "-").ToLower() : string.Empty;
            return output;
        }
        public static string CamelToKebab(this string text)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(text))
            {
                string s = !String.IsNullOrWhiteSpace(text) ? text.Trim().Replace("  ", " ").Replace(" ", "-") : string.Empty;
                var array = s.ToCharArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (i == 0)
                    {

                    }
                    else if(Char.IsUpper(array[i]))
                    {
                        sb.Append("-");                        
                    }
                    sb.Append(array[i].ToString().ToLower());

                }
            }
            else
            {
                return text;
            }
            return sb.ToString();

        }

        public static string ToTitleCase(this string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                return ti.ToTitleCase(text.Trim());
            }
            return text;
            
        }

        public static bool StripSlug(this string path, out string slug, out string next)
        {
            bool b = false;
            slug = string.Empty;
            next = string.Empty;
            if (!string.IsNullOrWhiteSpace(path))
            {
                string s = path.Trim();
                var pos = s.IndexOf('/', 1);
                if (pos > -1)
                {
                    slug = path.Substring(1, pos - 1);
                    next = path.Substring(slug.Length+1);
                    b = true;
                }
            }             
            return b;
        }
    }
}
