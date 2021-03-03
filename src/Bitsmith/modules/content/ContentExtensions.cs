using Bitsmith.ProjectManagement;
using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Bitsmith.Models
{
    public static class ContentExtensions
    {
        public static string ToCsv(this DataTable dt, char delimiter = ',')
        {
            int max = dt.Columns.Count;
            StringBuilder table = new StringBuilder();
            StringBuilder header = new StringBuilder();
            for (int i = 0; i < max; i++)
            {
                var col = dt.Columns[i];
                if (i > 0)
                {
                    header.Append(delimiter);
                }
                header.Append(col.ColumnName);
            }
            table.AppendLine(header.ToString());
            foreach (DataRow row in dt.Rows)
            {
                StringBuilder line = new StringBuilder();
                for (int i = 0; i < max; i++)
                {
                    if (i > 0)
                    {
                        line.Append(delimiter);
                    }
                    var contents = row[i].ToString();
                    if (!string.IsNullOrWhiteSpace(contents))
                    {
                        line.Append(contents.EscapeCsv());
                    }
                }
                table.AppendLine(line.ToString());
            }
            return table.ToString();
        }
        public static string EscapeCsv(this string text)
        {
            bool mustQuote = (text.Contains(",") || text.Contains("\"") || text.Contains("\r") || text.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in text)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return text;
        }

        public static List<Query> Cleanse(this List<Query> queries)
        {
            foreach (var query in queries)
            {
                foreach (var item in query.TokenQueries)
                {
                    item.Ids.Clear();
                }
            }
            return queries;
        }
        public static string GetHash(this Query query)
        {
            var d = query.TokenQueries.Depopulate();
            var name = query.Name;
            query.Name = null;
            var querytype = query.QueryType;
            query.QueryType = QueryTypeOption.None;
            var hash = query.ComputeHash();
            query.Name = name;
            query.QueryType = querytype;
            query.TokenQueries.Repopulate(d);
            return hash;
        }

        private static Dictionary<string,List<string>> Depopulate(this List<TokenQuery> tokens)
        {
            Dictionary<string, List<string>> d = new Dictionary<string, List<string>>();
            tokens.ForEach(tq => {
                var key = tq.ToKey();
                if (!d.ContainsKey(key))
                {
                    d.Add(key, tq.Ids);
                    tq.Ids.Clear();
                }
            });
            return d;
        }

        private static void Repopulate(this List<TokenQuery> tokens, Dictionary<string,List<string>> entries)
        {
            tokens.ForEach(tq => {
                var key = tq.ToKey();
                if (entries.ContainsKey(key))
                {
                    tq.Ids = entries[key];
                }
            });
        }

        private static string ToKey(this TokenQuery tokenQuery)
        {
            return $"{tokenQuery.Token}-{tokenQuery.SearchType.ToString()}";
        }

        public static string ComputeHash<T>(this T data) where T : class
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json.ComputeHash();
        }

        public static string ComputeHash(this string data)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data)).ToList();
                bytes.ForEach(b => { sb.Append(b.ToString("x2")); });
            }
            return sb.ToString();
        }
        public static List<string> TagExclusions(this List<string> list)
        {
            list.Add("created-at");
            list.Add("created-by");
            list.Add("modified-at");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Credentials}");
            list.Add($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}");
            return list;
        }
        public static void CleansePaths(this List<ContentItem> contentItems)
        {
            var filepath = $"/{AppConstants.Paths.Files}";
            var contentpath = $"/{AppConstants.Paths.Content}";
            contentItems.ForEach((c) => {
                if (c.Paths.Contains(contentpath))
                {
                    c.Paths.Remove(contentpath);
                }
                if (c.HasFile())
                {
                    bool b = false;
                    for (int i = 0;!b && i < c.Paths.Count; i++)
                    {
                        b = c.Paths[i].StartsWith(filepath);
                        if (b)
                        {
                            c.Paths.RemoveAt(i);
                        }
                    }
                }
            });
        }
        public static void EnsurePaths(this List<ContentItem> contentItems)
        {            
            var contentpath = $"/{AppConstants.Paths.Content}";
            contentItems.ForEach((c) => {
                if (c.Paths == null)
                {
                    c.Paths = new List<string>();
                }
                if (!c.Paths.Contains(contentpath))
                {
                    c.Paths.Add(contentpath);
                }
                if (c.HasFile())
                {
                    var path = $"/{AppConstants.Paths.Files}/{c.Mime}";
                    if (!c.Paths.Contains(path))
                    {
                        c.Paths.Add(path);
                    }
                }
            });
        }


        public static int Total(this List<Counter> list, string domainId = AppConstants.Default)
        {
            int i = 0;
            list.ForEach((item) => { i += item.Count; });
            return i;
        }
        public static bool InDomain(this ContentItem item, string domain)
        {
            return item.Properties.Any((p)=>
            { 
                return p.Name.Equals("x-domain") && 
                p.Value.ToString().Equals(domain, StringComparison.OrdinalIgnoreCase); 
            });
        }

        public static string Domain(this ContentItem item)
        {
            var found = item.Properties.FirstOrDefault(y => y.Name.Equals("x-domain"));
            return found?.Value.ToString() ?? AppConstants.Default;
        }

        public static List<ContentItem> ForDomain(this List<ContentItem> superset, string domain)
        {
            return (!string.IsNullOrWhiteSpace(domain) ? superset.Where(x => x.InDomain(domain)) : superset).ToList();
        }

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



        public static bool Includes(this ContentItem model, string token)
        {
            QueryExpression expression = new QueryExpression(token);
            return expression.Evaluate(model.Properties);
        }

        public static string Datatype(this Property property)
        {
            return (property.Value != null) ? property.Value.GetType().Name : "x:Null";
        }

        public static List<Query> Default(this List<Query> list, List<Domain> domains)
        {
            foreach (var domain in domains)
            {
                list.Add(new Query().Default(domain));               
            }
            return list;
        }

        public static string ToQueryText(this Query model )
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in model.TokenQueries)
            {
                sb.AppendLine(item.Token);
            }

            return sb.ToString().TrimEnd();
        }

        public static Query Default(this Query model, Domain domain)
        {
            model.QueryType = QueryTypeOption.Named;
            model.Name = "All";
            model.Domain = domain.Id;
            model.TokenQueries = new List<TokenQuery>().Default();
            return model;
        }
        public static List<TokenQuery> Default(this List<TokenQuery> list)
        {
            list.Add(new TokenQuery() { SearchType = SearchTypeOptions.Tag, Token = "all" });
            return list;
        }
        public static List<MimeMap> Default(this List<MimeMap> list)
        {


            return list;
        }

        public static TaskManager Default(this TaskManager model)
        {
            DateTime now = DateTime.Now;
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.CreatedAt = now;
            return model;
        }
        public static Content Default(this Content model)
        {
            DateTime now = DateTime.Now;
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.CreatedAt = now;
            return model;
        }

        public static Domain Default(this Domain model, DateTime now, string id)
        {
            model.CreatedOn = now;
            model.Id = id;
            model.Scope = ScopeOption.Private;
            model.Name = "default";
            model.Lists = new List<Property>();
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
            var contentType = vm.ContentType;
            if (b)
            {
                b = !b;
                item.Id = Guid.NewGuid().ToString().ToLower();
                item.Display = vm.Display;
                item.Scope = vm.Scope;
                item.Properties.DefaultTags(domain);
                item.Properties.AddRange(resolver.Resolve(vm.Tags));
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
                        var filetype = item.Mime.Trim(new char[] { '.' });
                        vm.Paths.Add($"/files/{filetype}");
                        b = true;
                    }
                }
                else
                {
                    item.Mime = vm.Mime;
                    if (vm.IsLink)
                    {
                        if(!vm.Body.StartsWithAny(new string[] { "http://", "https://" }))
                        {
                            item.Body = $"http://{vm.Body}";
                        }
                        else
                        {
                            item.Body = vm.Body;
                        }
                    }
                    else if (vm.Body.Length > contentManager.MaxLength && 
                        contentManager.TryInloadAsFile(vm.Body, item.Id, out string filename, out FileInfo info))
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
                    else
                    {
                        item.Body = vm.Body;
                    }                    
                    
                    b = true;
                }
                HashSet<string> hs = new HashSet<string>();
                foreach (var path in vm.Paths)
                {
                    if (hs.Add(path))
                    {
                        item.Paths.Add(path);
                    }
                }
                var tag = vm.SelectedTag;
                
                vm.Refresh(tag,contentType);
            }
            return b;
        }

        public static bool StartsWithAny(this string text, params string[] items)
        {
            IEnumerable<string> list = new List<string>(items);
            return text.StartsWithAny(list);
        }
        public static bool StartsWithAny(this string text, IEnumerable<string> tokens)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(text))
            {
                b = tokens.Any((t) => { return text.StartsWith(t, StringComparison.OrdinalIgnoreCase); });
            }
            return b;
        }

        public static string Resolve(this IEnumerable<MimeMapViewModel> mimes, FileInfo info)
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


        public static bool HasFile(this ContentItem item, string fileExtension = "")
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                return item.Properties.Any(x => x.Name.Equals(_Replacements["ext"]));               
            }
            else
            {
                var found = item.Properties.FirstOrDefault(x => x.Name.Equals($"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}"));
                return found != null && found.Value.Equals(fileExtension);
            }
        }

        public static List<Property> Tags(this List<Property> properties)
        {
            return properties.Where(x => !ExcludedTags.Contains(x.Name)).ToList();
        }

        public static void Coalesce(this List<Property> existing, List<Property> toCoalesce)
        {
            List<Property> list = new List<Property>();
            var modified = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}";
            foreach (var exclusion in existing.Where(x=> x.Name.StartsWith(AppConstants.Tags.Prefix)))
            {
                if (exclusion.Name.Equals(modified))
                {
                    exclusion.Value = DateTime.Now;
                }
                list.Add(exclusion);
            }
            list.AddRange(toCoalesce);
            existing.Clear();
            existing.AddRange(list);
        }

        public static void LastViewed(this ContentItem model)
        {
            var name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}";
            model.LastAt(name);           
        }

        public static void LastUpdated(this ContentItem model)
        {
            var name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}";
            model.LastAt(name);
        }

        private static void LastAt(this ContentItem model, string name)
        {
            var now = DateTime.Now;
            var found = model.Properties.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (found == null)
            {
                model.Properties.Add(new Property() { Name = name, Value = now });
            }
            else
            {
                found.Value = now;
            }
        }

        public static void DefaultTags(this List<Property> properties, Domain domain)
        {
            properties.DefaultTags();
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}", Value = domain.Id });
        }
        public static void DefaultTags(this List<Property> properties)
        {
            DateTime now = DateTime.Now;
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}", Value = now });
            properties.Add(new Property() { Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}", Value = Environment.UserName });
        }

        private static List<string> ExcludedTags = new List<string>() {
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ModifiedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.ViewedAt}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.CreatedBy}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Domain}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Extension}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Task}",
            $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Credentials}",
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

        public static bool TryBuildFlowDocument(this FileInfo info, out FlowDocument flowDocument, IEnumerable<string> termsToHighlight)
        {
            flowDocument = null;
            if (info.Exists)
            {
                if (info.Extension.Equals(".docx",StringComparison.OrdinalIgnoreCase))
                {
                    //return info.TryBuildFlowDocumentFromDocx(out flowDocument, termsToHighlight);
                }
                else if(info.Extension.Equals(".pdf",StringComparison.OrdinalIgnoreCase))
                {
                    return info.TryBuildFlowDocumentFromPdf(out flowDocument, termsToHighlight);
                }
            }
            return false;
        }

        public static bool TryBuildFlowDocumentFromPdf(this FileInfo info, out FlowDocument flowDocument, IEnumerable<string> termsToHighlight)
        {
            bool b = false;
            flowDocument = new FlowDocument();
            return b;
        }

        //public static bool TryBuildFlowDocumentFromDocx(this FileInfo info, out FlowDocument flowDocument, IEnumerable<string> termsToHighlight)
        //{
        //    bool b = false;
        //    flowDocument = new FlowDocument();
        //    try
        //    {
                
        //        using (var doc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(info.FullName,false))
        //        {
        //            if (String.IsNullOrWhiteSpace(termToHighlight))
        //            {
        //                foreach (var paragraph in doc.MainDocumentPart.Document.Body)
        //                {
        //                    Paragraph p = new Paragraph(new Run(paragraph.InnerText));

        //                    flowDocument.Blocks.Add(p);
        //                }
        //            }
        //            else
        //            {
        //                foreach (var paragraph in doc.MainDocumentPart.Document.Body)
        //                {

        //                    if (!paragraph.InnerText.Contains(termToHighlight))
        //                    {
        //                        Paragraph p = new Paragraph(new Run(paragraph.InnerText));
        //                        flowDocument.Blocks.Add(p);
        //                    }
        //                    else
        //                    {
        //                        Paragraph p = new Paragraph();
        //                        string text = paragraph.InnerText.Trim();
        //                        int pos = 0;
        //                        while (pos < text.Length)
        //                        {
        //                            int x = text.IndexOf(termToHighlight, pos);
        //                            if (x < 0)
        //                            {
        //                                string s = text.Substring(pos);
        //                                p.Inlines.Add(new Run(s));
        //                                pos = text.Length;
        //                            }
        //                            else
        //                            {
        //                                string s = text.Substring(pos, x - pos);
        //                                string t = text.Substring(x, termToHighlight.Length);
        //                                p.Inlines.Add(new Run(s));
        //                                p.Inlines.Add(new Span(new Run(t) { Background = Brushes.Yellow }));
        //                                pos = x + termToHighlight.Length;
        //                            }

        //                        }
        //                        flowDocument.Blocks.Add(p);
        //                    }
        //                }
        //            }
        //            b = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return b;
        //}
    }
}
