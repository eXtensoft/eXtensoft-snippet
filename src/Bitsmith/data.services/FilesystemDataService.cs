using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.DataServices
{
    public class FilesystemDataService
    {

        public bool TryCopyTo(FileInfo from, FileInfo to, out string message, bool overwriteExisting = true)
        {
            return FileSystemDataProvider.TryCopyTo(from, to, out message, overwriteExisting);
        }

        public bool TryFindDirectory(out DirectoryInfo info, string selectedPath = "")
        {
            return FileSystemDataProvider.TryFindDirectory(out info, selectedPath);
        }

        public bool TryLocateFile(out FileInfo info, bool isAppendName = false)
        {
            return FileSystemDataProvider.TryLocateFile(out info, isAppendName);
        }

        public bool TryMove(FileInfo from, FileInfo to)
        {
            return FileSystemDataProvider.TryMove(from, to);
        }

    }
}
