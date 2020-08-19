using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class ContentManager
    {
        public bool IsInitialized { get; private set; } = false;
        public DirectoryInfo ContentDirectory { get; set; }

        public int MaxLength { get; set; } = 300;
        public bool TryInload(FileInfo info, out string filename)
        {
            filename = string.Empty;
            bool b = false;
            if (IsInitialized)
            {
                filename = info.Name;
              
                if (File.Exists(Path.Combine(ContentDirectory.FullName, filename)))
                {
                    filename = $"{info.Name.Substring(0,info.Name.LastIndexOf('.'))}.{Guid.NewGuid().ToString().Substring(0,4)}{info.Extension}".Scrub();
                }
                else
                {
                    filename = filename.Scrub();
                }
                string fullFilepath = Path.Combine(ContentDirectory.FullName, filename);
                File.Copy(info.FullName, fullFilepath);
                b = true;
            }
            return b;
        }


        internal bool TryInloadAsFile(string body, string id, out string filename, out FileInfo info)
        {
            bool b = false;
            info = null;
            filename = $"{id}.txt";
            if (IsInitialized)
            {
                string fullFilepath = Path.Combine(ContentDirectory.FullName, filename);
                info = new FileInfo(filename);               
                File.WriteAllText(fullFilepath, body);
                b = true;
            }
            return b;
        }

        public ContentManager(string folderpath)
        {
            if (!Directory.Exists(folderpath))
            {
                ContentDirectory = new DirectoryInfo(folderpath);
                ContentDirectory.Create();
                IsInitialized = true;
            }
            else
            {
                ContentDirectory = new DirectoryInfo(folderpath);
                IsInitialized = true;
            }
        }


    }
}
