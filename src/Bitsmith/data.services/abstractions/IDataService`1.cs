using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.DataServices.Abstractions
{
    public interface IDataService
    {
        bool TryRead<T>(out List<T> list, out string message, string filepath = "") where T : class, new();
        bool TryRead<T>(string filepath, out T model, out string message) where T : class, new();
        bool TryRead<T>(string filepath, out List<T> list, out string message) where T : class, new();
        bool TryWrite<T>(List<T> list, out string message, string filepath = "") where T : class, new();
        bool TryWrite<T>(T model, out string message, string filepath = "") where T : class, new();
        string Filepath<T>(string suffix = "") where T : class, new();
        bool TryLocateFile(out FileInfo info, bool isAppendName = false);
        bool TryCopyTo(FileInfo from, FileInfo to, out string message, bool overwriteExisting = true);
        bool TryMove(FileInfo from, FileInfo to);
        bool TryFindDirectory(out DirectoryInfo info, string selectedPath = "");



    }
}
