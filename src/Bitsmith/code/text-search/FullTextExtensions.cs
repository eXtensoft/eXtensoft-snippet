﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bitsmith;
using Bitsmith.Models;
using Bitsmith.ViewModels;

namespace Bitsmith.FullText
{
    public static class FullTextExtensions
    {

        public static DataTable ToProfile(this TextIndexes indexes)
        {
            DataTable dt = new DataTable() { TableName = "Index Profile" };
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("Word", typeof(string));
            foreach (var item in indexes.OrderByDescending(i=>i.Count).ThenBy(x=>x.Value))
            {
                var r = dt.NewRow();
                r[0] = item.Count;
                r[1] = item.Value;
                dt.Rows.Add(r);
            }
            return dt;
        }
        //public static ContentModule IndexContent(this ContentModule module)
        //{
        //    string directory = System.IO.Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles);
        //    TextIndexLoader.Load(module.Indexer, module.Content.Items, directory);
           
        //    return module;
        //}
        //public static void IndexContent(this ContentModule module, ContentItem contentItem)
        //{
        //    string directory = System.IO.Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles);
        //    TextIndexLoader.Load(module.Indexer, contentItem, directory);
        //}
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
            //report.Reverse();
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

     
        public static bool Load(this TextIndexes index, List<TextIndex> list)
        {

            index.AddRange(list);

            return true;
        }

        public static void Delete(this TokenCollection tokens, string id)
        {
            foreach (var token in tokens)
            {
                if (token.Locations.Contains(id))
                {
                    token.Locations.Remove(id);
                }
            }
        }


    }
}
