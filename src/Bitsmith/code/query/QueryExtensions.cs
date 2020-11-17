using Bitsmith.Indexing;
using Bitsmith.Models;
using Bitsmith.ViewModels;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Search
{
    public static class QueryExtensions
    {

        public static string Id(this Query query)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"d:{query.Domain};");
            query.TokenQueries.ForEach((q) => {
                sb.Append($";q:{q.SearchType}.{q.Token}");
            });
            return sb.ToString();
        }

        public static Query Parse(this Query query, IPathNode node, string domainId)
        {
            SearchTypeOptions searchType = SearchTypeOptions.Path;
            query.Operator = QueryOperatorOption.And;
            query.Domain = domainId;
            var token = node.Path;
            if (node.Path.StripSlug(out string slug, out string next))
            {
                if (slug.Equals(AppConstants.Paths.Files))
                {
                    searchType = SearchTypeOptions.File;
                    token = next.TrimStart('/');
                } 
                else if (slug.Equals(AppConstants.Paths.Default))
                {
                    token = next;
                }
            }
            else
            {
                var path = node.Path.TrimStart('/');
                if (path.Equals(AppConstants.Paths.Files))
                {
                    // if just files, then all files
                    searchType = SearchTypeOptions.File;
                }
                else if (path.Equals(AppConstants.Paths.Default))
                {

                }
                else
                {
                    // content
                }
            }
            //var pos = node.Path.IndexOf('/', 1);
            //token = pos > 0 ? node.Path.Substring(pos) : node.Path;
            query.TokenQueries.Add(new TokenQuery() { SearchType = searchType, Token = token  });
            return query;
        }

        public static Query Parse(this Query query, string input, 
            SearchTypeOptions searchType,            
            QueryOperatorOption operatorOption,
            string domainId)
        {
            query.Operator = operatorOption;
            query.Domain = domainId;
            if (!string.IsNullOrWhiteSpace(input))
            {
                var tokens = input.SplitTrimLower().NormalizeQueryKeys();
                foreach (var token in tokens)
                {
                    query.TokenQueries.Add(new TokenQuery() { SearchType = searchType, Token = token });
                }
            }
            return query;
        }


        public static List<ContentItemViewModel> Execute(this Query query, 
            List<ContentItem> contentItems, 
            IContentIndexer indexer)
        {           
            foreach (var queryToken in query.TokenQueries)
            {
                switch (queryToken.SearchType)
                {
                    case SearchTypeOptions.None:
                        break;
                    case SearchTypeOptions.Tag:
                        queryToken.TagQuery(contentItems, query.Domain);
                        break;
                    case SearchTypeOptions.FullText:
                        queryToken.FullTextQuery(indexer);
                        break;
                    case SearchTypeOptions.Path:
                        queryToken.PathQuery(contentItems, query.Domain);
                        break;
                    case SearchTypeOptions.File:
                        queryToken.FileQuery(contentItems, query.Domain);
                        break;
                    case SearchTypeOptions.Fuzzy:
                        break;
                    case SearchTypeOptions.Recent:
                        break;
                    default:
                        break;
                }
            }
            List<ContentItemViewModel> list = query.Aggregate(contentItems);
            
            return list;
        }

        public static void TagQuery(this TokenQuery tokenQuery, List<ContentItem> contentItems, string domain)
        {
            tokenQuery.Ids = contentItems.ForDomain(domain).Where(x => x.Includes(tokenQuery.Token)).Select(y => y.Id).ToList();
        }

        public static void PathQuery(this TokenQuery tokenQuery, List<ContentItem> contentItems, string domain)
        {
            //tokenQuery.Ids = contentItems.ForDomain(domain).Where(x => x.HasPathStartingWith(tokenQuery.Token)).Select(y => y.Id).ToList();
            tokenQuery.Ids = contentItems.ForDomain(domain).Where(x => x.Paths.Contains(tokenQuery.Token)).Select(y => y.Id).ToList();
        } 

        public static bool HasPathStartingWith(this ContentItem contentItem, string pathPart)
        {
            bool b = false;
            for (int i = 0;!b && i < contentItem.Paths.Count; i++)
            {
                var path = contentItem.Paths[i];
                b = pathPart.Length <= path.Length && path.StartsWith(pathPart);
            }
            return b;
        }

        public static void FileQuery(this TokenQuery tokenQuery, List<ContentItem> contentItems, string domain)
        {
            tokenQuery.Ids = contentItems.ForDomain(domain).Where(x => x.HasFile(tokenQuery.Token)).Select(y => y.Id).ToList();
        }

        public static void FullTextQuery(this TokenQuery tokenQuery, IContentIndexer indexer)
        {
            tokenQuery.Ids = indexer.Query(tokenQuery.Token).ToList();
        }

        public static List<ContentItemViewModel> Aggregate(this Query query,List<ContentItem> contentItems)
        {
            List<ContentItemViewModel> list = new List<ContentItemViewModel>();
            List<string> ids = new List<string>();
            HashSet<string> hs = new HashSet<string>();
            foreach (var queryToken in query.TokenQueries.Where(x=>x.SearchType == SearchTypeOptions.FullText))
            {
                List<string> items = new List<string>();
                foreach (var id in queryToken.Ids)
                {
                    if (hs.Add(id))
                    {
                        ids.Add(id);
                        var vm = new ContentItemViewModel(contentItems.FirstOrDefault(y => y.Id.Equals(id, StringComparison.OrdinalIgnoreCase)));
                        vm.SearchTerms.Add(queryToken.Token);
                        list.Add(vm);
                    }
                    else
                    {
                        var found = list.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                        if (found != null && 
                            !found.SearchTerms.Contains(queryToken.Token))
                        {
                            found.SearchTerms.Add(queryToken.Token);
                        }
                    }
                }
            }
            foreach (var queryToken in query.TokenQueries.Where(x => x.SearchType != SearchTypeOptions.FullText))
            {
                List<string> items = new List<string>();
                foreach (var id in queryToken.Ids)
                {
                    if (hs.Add(id))
                    {
                        ids.Add(id);
                        list.Add(new ContentItemViewModel(contentItems.FirstOrDefault(y => y.Id.Equals(id, StringComparison.OrdinalIgnoreCase))));
                    }
                }
            }         
            return list;
        }
    }
}
