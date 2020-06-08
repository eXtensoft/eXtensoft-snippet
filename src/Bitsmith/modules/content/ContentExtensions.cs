using Bitsmith.ProjectManagement;
using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace Bitsmith.Models
{
    public static class ContentExtensions
    {
        public static bool Includes(this ContentItem model, List<QueryExpression> list)
        {
            bool b = false;
            for (int i = 0; !b && i < list.Count; i++)
            {
                QueryExpression expression = list[i];
                b = expression.Evaluate(model.Properties);
            }
            return b;
        }

        public static string Datatype(this Property property)
        {
            return (property.Value != null) ? property.Value.GetType().Name : "x:Null";
        }

        public static List<MimeMap> Default(this List<MimeMap> list)
        {


            return list;
        }

        public static Project Default(this Project model)
        {
            DateTime now = DateTime.Now;
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.CreatedAt = now;
            model.Domains = new List<Domain>();
            model.Domains.Add(new Domain().Default(now));
            return model;
        }
        public static Content Default(this Content model)
        {
            DateTime now = DateTime.Now;
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.CreatedAt = now;
            model.Domains = new List<Domain>();
            model.Domains.Add(new Domain().Default(now));
            return model;
        }

        public static Domain Default(this Domain model, DateTime now, string id)
        {
            model.CreatedOn = now;
            model.Id = id;
            model.Scope = ScopeOption.Private;
            model.Name = "default";

            return model;
        }

        public static Domain Default(this Domain model, DateTime now)
        {
            return model.Default(now, AppConstants.Default);
        }

        public static bool TryBuild(this NewContentViewModel vm, 
            TagResolver resolver,
            Domain domain, 
            IEnumerable<MimeMapViewModel> mimes,
            ContentManager contentManager,
            out ContentItem item)
        {
            bool b = vm.Validate();
            item = new ContentItem();
            if (b)
            {
                b = !b;
                item.Id = Guid.NewGuid().ToString().ToLower();
                item.Display = vm.Display;
                item.Scope = vm.Scope;
                item.Properties.DefaultTags(domain);
                item.Properties.AddRange(resolver.Resolve(vm.Tags));
                if (!String.IsNullOrWhiteSpace(vm.Path))
                {
                    item.Paths.Add(vm.Path);
                }
                if (vm.HasFile)
                {
                    FileInfo info = new FileInfo(vm.Filepath);
                    if (contentManager.TryInload(info, out string filename))
                    {
                        item.Mime = mimes.Resolve(info);
                        item.Body = filename;
                        item.Properties.Add(new Property() 
                        { 
                            Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}", 
                            Value = item.Mime 
                        });
                        b = true;
                    }
                }
                else
                {
                    if (vm.IsLink)
                    {
                        item.Body = vm.Body.StartsWith("http://") ? vm.Body : $"http://{vm.Body}";
                    }
                    else
                    {
                        item.Body = vm.Body;
                    }                    
                    item.Mime = vm.Mime;
                    b = true;
                }
                vm.Filepath = string.Empty;
                vm.Display = string.Empty;
                vm.Body = string.Empty;
                vm.Tags = new List<string>();
            }
            return b;
        }

        private static string Resolve(this IEnumerable<MimeMapViewModel> mimes, FileInfo info)
        {
            var found = mimes.FirstOrDefault(x => x.Extension.Equals(info.Extension,StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                return found.Id;
            }
            else
            {
                return info.Extension.TrimStart('.').ToLower();
            }
        }

        public static Control Resolve(this IEnumerable<MimeMapViewModel> mimes,ContentItemViewModel vm)
        {

            Control ctl = null;

            MimeMapViewModel map = null;
            var prop = vm.Model.Properties.FirstOrDefault(x => x.Name.Equals($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}", StringComparison.OrdinalIgnoreCase));
            if (prop != null)
            {
                map = mimes.FirstOrDefault(x => x.Id.Equals(prop.Value.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                map = mimes.FirstOrDefault(x => x.Id.Equals(vm.Mime, StringComparison.OrdinalIgnoreCase));
            }

            string contentType = string.Empty;
            if (map != null)
            {
                contentType = map.Model.View;
                Type viewType = Type.GetType(contentType);
                if (viewType != null)
                {
                    ctl = (Control)Activator.CreateInstance(viewType);
                    ctl.DataContext = vm;
                }
            }
            if (ctl == null)
            {
                ctl = new ContentItemView();
                ctl.DataContext = vm;
            }
            return ctl;

        }
        public static List<Property> Tags(this List<Property> properties)
        {
            return properties.Where(x => !ExcludedTags.Contains(x.Name)).ToList();
        }

        public static void Coalesce(this List<Property> existing, List<Property> toCoalesce)
        {

        }

        public static void DefaultTags(this List<Property> properties, Domain domain)
        {
            DateTime now = DateTime.Now;
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}", Value = Environment.UserName });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}", Value = domain.Id });
        }

        private static List<string> ExcludedTags = new List<string>() {
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}"
        };

        private static Dictionary<string, string> _Replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "extension", $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}" },
            { "ext", $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}" },
            { "word", $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}:word"  },
            { "excel", $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}:excel"  }
        };

        private static List<char> operators = new List<char>()
        {
            ':','=','>','<'
        };
        private static string Recombine(this string prefix, string input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            bool b = false;
            for (int i = 0;!b && i < input.Length; i++)
            {
                char c = input[i];
                if (operators.Contains(c))
                {
                    sb.Append(input.Substring(i));
                    b = true;

                }

            }
            return sb.ToString();
        }

        public static bool TryFindStartsWith(this Dictionary<string,string> maps, string input, out string key)
        {
            bool b = false;
            key = input;
            if (!string.IsNullOrWhiteSpace(input))
            {
                key = input.Split(new char[] { ':','=','>','<' }, StringSplitOptions.RemoveEmptyEntries)[0];
                b = maps.ContainsKey(key);
            }
            return b;
        }

        public static List<string> NormalizeQueryKeys(this List<string> list)
        {
            List<string> output = new List<string>();
            foreach (var item in list)
            {
                if (!String.IsNullOrWhiteSpace(item) )
                {
                    if (_Replacements.ContainsKey(item))
                    {
                        output.Add(_Replacements[item].Recombine(item));
                    }
                    else if (_Replacements.TryFindStartsWith(item, out string key))
                    {
                        output.Add(_Replacements[key].Recombine(item));                        
                    }
                    else
                    {
                        output.Add(item);
                    }
                }
                else
                {
                    output.Add(item);
                }
            }
            return output;
        }

        public static string Scrub(this string text)
        {
            return !String.IsNullOrWhiteSpace(text) ? text.Trim().Replace(' ', '-') : text.Trim();
        }

    }
}
