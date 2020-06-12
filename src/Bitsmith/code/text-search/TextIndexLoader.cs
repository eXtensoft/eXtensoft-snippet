﻿using Bitsmith.FullText;
using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public class TextIndexLoader
    {
        public static void Load(TextIndexer indexer, IEnumerable<ContentItem> contentItems, string directory)
        {

            foreach (var item in contentItems)
            {
                Load(indexer, item, directory);
            }
            var profile = indexer.Profiler.Present();
        }

        public static void Load(TextIndexer indexer, ContentItem item, string directory)
        {
            List<string> lines = new List<string>();
            var mime = item.Mime;

            switch (mime)
            {
                case "text":
                    if (TrySplit(item.Body, out lines))
                    {
                        indexer.Index(item.Id.ToString(), lines);
                    }
                    break;
                case "txt":

                    if (TryReadSplit(Path.Combine(directory, item.Body), out lines))
                    {
                        indexer.Index(item.Id.ToString(), lines);
                    }
                    break;
                case "text/credential":
                case "resource/url":
                    break;
                default:
                    break;
            }
        }
        public static bool TryReadSplit(FileInfo info, out List<string> lines)
        {
            return TryReadSplit(info.FullName, out lines);
        }
        
        public static bool TryReadSplit(string filepath, out List<string> lines)
        {
            bool b = false;
            lines = new List<string>();
            if (File.Exists(filepath))
            {
                try
                {
                    lines = new List<string>(File.ReadAllLines(filepath));
                    if (lines.Count > 0)
                    {
                        b = true;
                    }
                }
                catch { }
            }
            return b;
        }
        public static bool TrySplit(string content, out List<string> lines)
        {
            bool b = false;
            lines = new List<string>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                lines = new List<string>(content.Split(new char[] { '\r', '\n' }));
                if (lines.Count > 0)
                {
                    b = true;
                }
            }

            return b;
        }
    }
}