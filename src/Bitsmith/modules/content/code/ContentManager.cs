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
        public bool TryInload(FileInfo info, out string filename)
        {
            filename = string.Empty;
            bool b = false;
            if (IsInitialized)
            {
                filename = info.Name;
              
                if (File.Exists(Path.Combine(ContentDirectory.Name, filename)))
                {
                    filename = $"{info.Name.Substring(0,info.Name.LastIndexOf('.'))}.{Guid.NewGuid().ToString().Substring(0,4)}{info.Extension}".Scrub();
                }
                else
                {
                    filename = filename.Scrub();
                }
                string fullFilepath = Path.Combine(ContentDirectory.Name, filename);
                File.Copy(info.FullName, fullFilepath);
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
