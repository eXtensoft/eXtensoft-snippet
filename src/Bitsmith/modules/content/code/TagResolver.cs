using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitsmith
{
    public class TagResolver
    {       
        public Dictionary<string,TagMap> TagMaps { get; set; } = new Dictionary<string,TagMap>(StringComparer.OrdinalIgnoreCase);

        public List<string> Exclusions { get; set; } = new List<string>() 
        { 
            "created-at",
            "created-by",
            "modified-at",
        };

        public List<Property> Resolve(string tags)
        {
            var candidates = tags.Split(new char[] { ';', ',' });
            List<string> list = (from candidate in candidates select candidate.Trim()).ToList();
            return Resolve(list);
        }

        public List<Property> Resolve(List<string> tags)
        {
            List<Property> list = new List<Property>();
            foreach (var tag in tags)
            {
                var parts = tag.Split(new char[] { ':' },StringSplitOptions.RemoveEmptyEntries);
                if (!Exclusions.Contains(parts[0]))
                {
                    Property property = new Property();                   
                    if (parts.Length > 1)
                    {
                        property.Value = TypeResolver.Resolve(parts[1]);
                    }
                    property.Name = TagMaps.ContainsKey(parts[0]) ? TagMaps[parts[0]].Key : parts[0];

                    if (!TagMaps.ContainsKey(parts[0]))
                    {
                        property.Name = parts[0];
                        TagMaps.Add(parts[0], new TagMap() { Key = parts[0], Count = 0 });
                    }
                    else
                    {
                        property.Name = TagMaps[parts[0]].Key;                        
                    }
                    TagMaps[property.Name].Count++;
                    list.Add(property);
                }
            }            
            return list;
        }

        internal void Load(List<ContentItem> items)
        {
            foreach (var item in items)
            {
                foreach (var tag in item.Properties.Where(t => !Exclusions.Contains(t.Name)))
                {
                    if (!TagMaps.ContainsKey(tag.Name))
                    {
                        TagMaps.Add(tag.Name, new TagMap() { Key = tag.Name, Count = 0 });
                    }
                    TagMaps[tag.Name].Count++;
                }
                
            }
        }


    }
}
