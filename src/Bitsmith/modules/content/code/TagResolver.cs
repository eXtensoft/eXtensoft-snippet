using Bitsmith.Models;
using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Bitsmith
{
    public class TagResolver
    {
        private DomainViewModel _Domain;
        public DomainViewModel Domain
        {
            get
            {
                return _Domain;
            }
            set
            {

                _Domain = value;
                _SelectedDomain = _Domain.Id;
            foreach (var item in TagItems)
            {
                item.Model.Counters.Total(_SelectedDomain);
            }
            RefreshFiltering();                             
            }
        }

        private void ChangeDomain(string id)
        {
           
        }

        private string _SelectedDomain = AppConstants.Default;


        private ICollectionView _Items;
        public ICollectionView Items
        {
            get
            {
                if (_Items == null)
                {
                    _Items = CollectionViewSource.GetDefaultView(TagItems);
                    _Items.SortDescriptions.Add(new SortDescription("Count", ListSortDirection.Descending));
                    _Items.Filter = FilterForDomain;
                }
                return _Items;
            }
        }

        private ICollectionView _Recent;
        public ICollectionView Recent
        {
            get
            {
                if (_Recent == null)
                {
                    _Recent = CollectionViewSource.GetDefaultView(RecentTags);
                    _Recent.Filter = FilterForDomain;
                }
                return _Recent;
            }
        }

        private ICollectionView _Popular;
        public ICollectionView Popular
        {
            get
            {
                if (_Popular == null)
                {
                    _Popular = CollectionViewSource.GetDefaultView(PopularTags);
                    _Popular.Filter = FilterForDomain;
                }
                return _Popular;
            }
        }

        private bool FilterForDomain(object o)
        {
            bool b = false;

            var vm = o as TagMapViewModel;
            if (vm != null && !DomainExcludes(vm.Key))
            {
                b = vm.SetFilter(_SelectedDomain) && vm.Count > Domain.MinimumTagCount;               
            }
            return b;
        }
        private bool DomainExcludes(string key)
        {
            return Exclusions.ContainsKey(_SelectedDomain) && Exclusions[_SelectedDomain].Contains(key);
        }


        private ObservableCollection<TagMapViewModel> TagItems { get; set; } = new ObservableCollection<TagMapViewModel>();

        public ObservableCollection<TagMapViewModel> RecentTags { get; set; } = new ObservableCollection<TagMapViewModel>();
        public ObservableCollection<TagMapViewModel> PopularTags { get; set; } = new ObservableCollection<TagMapViewModel>();


        public void RefreshFiltering()
        {
            Items.Refresh();
            Popular.Refresh();
            Recent.Refresh();
        }
        public Dictionary<string,List<string>> Exclusions { get; set; }

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
                    var parts = tag.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!Exclusions.ContainsKey(_SelectedDomain) || !Exclusions[_SelectedDomain].Contains(parts[0]))
                    {
                        Property property = new Property();
                        if (parts.Length > 1)
                        {
                            property.Value = TypeResolver.Resolve(parts[1]);
                        }

                        string key = parts[0].Trim();

                        var found = TagItems.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                        if (found == null)
                        {
                            
                            found = new TagMapViewModel(new TagMap() { Key = key });
                            TagItems.Add(found);
                        }
                        property.Name = found.Key;
                        var counter = found.Model.Counters.FirstOrDefault(y => y.Key.Equals(y.Key, StringComparison.OrdinalIgnoreCase) &&
                        y.Domain.Equals(_SelectedDomain, StringComparison.OrdinalIgnoreCase));
                        if (counter == null)
                        {
                            counter = new Counter() { Key = key, Domain = _SelectedDomain };
                            found.Model.Counters.Add(counter);
                        }
                        counter.Count++;

                        if (recenttags.Add(found.Key))
                        {
                            RecentTags.Add(found);
                        }
                        list.Add(property);
                    }
                }
            }
            RefreshFiltering();
            return list;
        }

        internal void SetRecentTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                var found = TagItems.FirstOrDefault(x => x.Key.Equals(tag, StringComparison.OrdinalIgnoreCase));
                if (found != null && recenttags.Add(tag))
                {
                    RecentTags.Add(found);
                }
            }
        }

        public TagResolver(Dictionary<string, List<string>> exclusions)
        {
            Exclusions = exclusions;
        }



        internal void Load(List<ContentItem> contentItems)
        {
            List<TagMap> maps = new List<TagMap>();
            foreach (var item in contentItems)
            {
                var domaintag = item.Properties.FirstOrDefault(x => x.Name.Equals("x-domain", StringComparison.OrdinalIgnoreCase));
                string domain = domaintag != null ? domaintag.Value.ToString() : AppConstants.Default;

                foreach (var tag in item.Properties.Where(t => !Exclusions[_SelectedDomain].Contains(t.Name)))
                {
                    string key = tag.Name;
                    var tagmap = maps.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase) );
                    if (tagmap == null)
                    {
                        tagmap = new TagMap() { Key = key };
                        maps.Add(tagmap);
                    }

                    var counter = tagmap.Counters.FirstOrDefault(y => y.Key.Equals(y.Key,StringComparison.OrdinalIgnoreCase) && 
                        y.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase));
                    if (counter == null)
                    {
                        counter = new Counter() { Key = key, Domain = domain };
                        tagmap.Counters.Add(counter);
                    }
                    counter.Count++;
                }                
            }


            List<TagMapViewModel> vms = (from x in maps
                                         let z = new TagMapViewModel(x)
                                         
                                         select z).ToList();
            int i = 0;
            int max = 5;          
            foreach (var item in vms.OrderByDescending(o => o.Count))
            {
                if (i++ < max)
                {
                    PopularTags.Add(item);
                }
                TagItems.Add(item);
            }
        }
    }
}
