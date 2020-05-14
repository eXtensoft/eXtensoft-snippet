using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;

namespace Bitsmith.Models
{
    public static class ContentExtensions
    {

        public static List<MimeMap> Default(this List<MimeMap> list)
        {


            return list;
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
            out ContentItem item)
        {
            bool b = vm.Validate();
            item = new ContentItem();
            if (b)
            {
                item.Id = Guid.NewGuid().ToString().ToLower();
                item.Display = vm.Display;
                item.Body = vm.Body;
                item.Mime = vm.Mime;
                item.Scope = vm.Scope;                
                item.Properties.DefaultTags(domain);
                item.Properties.AddRange(resolver.Resolve(vm.Tags));
                if (!String.IsNullOrWhiteSpace(vm.Path))
                {
                    item.Paths.Add(vm.Path);
                }

                vm.Display = string.Empty;
                vm.Body = string.Empty;
            }
            return b;
        }

        private static void DefaultTags(this List<Property> properties, Domain domain)
        {
            DateTime now = DateTime.Now;
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Createdby}", Value = Environment.UserName });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}", Value = domain.Id });
        }

    }
}
