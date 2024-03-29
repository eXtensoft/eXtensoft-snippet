﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Biblio;
using Biblio.Modules;
using Biblio.ViewModels;

namespace Biblio.FullText
{
    public static class FullTextExtensions
    {
        public static BibleModule IndexContent(this BibleModule module)
        {
           
            TextIndexLoader.Load(module.Indexer, module.Books, "KJV");

            return module;
        }

        public static List<Tuple<int,string>> Present(this Dictionary<string, int> profile)
        {
            SortedDictionary<int, int> groupprofile = new SortedDictionary<int, int>();
            List<Tuple<int, string>> tuples = new List<Tuple<int, string>>();
            SortedDictionary<int, List<string>> report = new SortedDictionary<int, List<string>>();
            foreach (string token in profile.Keys)
            {
                int count = profile[token];
                if (!report.ContainsKey(count))
                {
                    report.Add(count, new List<string>());
                }
                List<string> list = report[count];
                if (!list.Contains(token))
                {
                    list.Add(token);
                }
            }
            report.Reverse();
            foreach (int key in report.Keys)
            {
                if (!groupprofile.ContainsKey(key))
                {                    
                    groupprofile.Add(key, report[key].Count);
                }
                report[key].Sort();
                foreach (string token in report[key])
                {
                    tuples.Add(new Tuple<int, string>(key, token));
                }
            }
            return tuples;
        }
    }
}
