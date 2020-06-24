﻿using Bitsmith.Models;
using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bitsmith
{
    public class TagResolver
    {

        public ObservableCollection<TagMapViewModel> Items { get; set; } = new ObservableCollection<TagMapViewModel>();
        //public Dictionary<string,TagMap> TagMaps { get; set; } = new Dictionary<string,TagMap>(StringComparer.OrdinalIgnoreCase);

        public ObservableCollection<TagMapViewModel> Recent { get; set; } = new ObservableCollection<TagMapViewModel>();
        public ObservableCollection<TagMapViewModel> Popular { get; set; } = new ObservableCollection<TagMapViewModel>();

        public List<string> Exclusions { get; set; } = new List<string>() 
        { 
            "created-at",
            "created-by",
            "modified-at",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Credentials}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}",
        };

        public List<Property> Resolve(string tags)
        {
            var candidates = tags.Split(new char[] { ';', ',' });
            List<string> list = (from candidate in candidates select candidate.Trim()).ToList();
            return Resolve(list);
        }

        private HashSet<string> recenttags = new HashSet<string>();

        public List<Property> Resolve(List<string> tags )
        {
            List<Property> list = new List<Property>();
            if (tags != null)
            {
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

                        string key = parts[0].Trim();

                        var found = Items.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                        if (found != null)
                        {
                            property.Name = found.Key;
                            found.Count++;
                        }
                        else
                        {
                            found = new TagMapViewModel(new TagMap() { Key = key, Count = 1 });
                            property.Name = key;
                            Items.Add(found);
                        }
                        if (recenttags.Add(found.Key))
                        {
                            Recent.Add(found);
                        }
                        
                        list.Add(property);
                    }
                } 
            }
           
            return list;
        }

        internal void Load(List<ContentItem> items)
        {
            List<TagMapViewModel> list = new List<TagMapViewModel>();
            foreach (var item in items)
            {
                foreach (var tag in item.Properties.Where(t => !Exclusions.Contains(t.Name)))
                {

                    string key = tag.Name;
                    var found = list.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                    if (found != null)
                    {
                        found.Count++;
                    }
                    else
                    {

                        list.Add(new TagMapViewModel(new TagMap() { Key = key, Count = 1 }));
                    }

                }
                
            }
            int i = 0;
            int max = 15;
            foreach (var item in list.OrderByDescending(o => o.Count))
            {
                if (i++ < max)
                {
                    Popular.Add(item);
                }
                Items.Add(item);
            }

            

        }


    }
}
