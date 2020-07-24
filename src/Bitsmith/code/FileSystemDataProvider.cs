using Bitsmith.ProjectManagement;
using Bitsmith.Styx;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public class FileSystemDataProvider
    {

        public static bool TryRead<T>(out List<T> list,
            out string message,
            string filepath = "") where T : class, new()
        {
            bool b = false;
            message = string.Empty;
            list = new List<T>();
            filepath = !String.IsNullOrWhiteSpace(filepath) ? filepath : Filepath<T>();
            try
            {
                if (File.Exists(filepath))
                {
                    list = GenericObjectManager.ReadGenericList<T>(filepath);
                    //string filecontents = File.ReadAllText(filepath);
                    //if (!string.IsNullOrWhiteSpace(filecontents))
                    //{

                    //    list = JsonConvert.DeserializeObject<List<T>>(filecontents);
                    //}
                    b = list.Count > 0;
                }
            }
            catch (Exception ex)
            {

                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return b;
        }


        public static bool TryRead<T>(string filepath,
            out T model,
            out string message) where T : class, new()
        {
            bool b = false;
            model = null;
            message = string.Empty;
            try
            {
                if (File.Exists(filepath))
                {
                    //string filecontents = File.ReadAllText(filepath);
                    //if (!string.IsNullOrWhiteSpace(filecontents))
                    //{
                    //    model = JsonConvert.DeserializeObject<T>(filecontents);
                    //    b = true;
                    //}
                    model = GenericObjectManager.ReadGenericItem<T>(filepath);
                    b = true;
                }
            }
            catch (Exception ex)
            {

                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return b;
        }

        public static bool TryRead<T>(string filepath,
            out List<T> list,
            out string message)
        {
            bool b = false;
            list = null;
            message = string.Empty;
            try
            {
                if (File.Exists(filepath))
                {
                    //string filecontents = File.ReadAllText(filepath);
                    //if (!string.IsNullOrWhiteSpace(filecontents))
                    //{
                    //    list = JsonConvert.DeserializeObject<List<T>>(filecontents);

                    //    b = true;
                    //}
                    list = GenericObjectManager.ReadGenericList<T>(filepath);
                    b = true;
                }
            }
            catch (Exception ex)
            {

                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return b;
        }

        public static bool TryWrite<T>(List<T> list, out string message, string filepath = "") where T : class, new()
        {
            bool b = false;
            message = string.Empty;
            filepath = !String.IsNullOrWhiteSpace(filepath) ? filepath : Filepath<T>();
            try
            {
                //JsonSerializerSettings options = new JsonSerializerOptions() 
                //{ 
                //    WriteIndented = true, 
                //    IgnoreReadOnlyProperties = true, 
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                //};
                //JsonSerializerSettings settings = new JsonSerializerSettings();
                //settings.Formatting = Formatting.Indented;

                //var json = JsonConvert.SerializeObject(list, settings);
                //File.WriteAllText(filepath, json);
                GenericObjectManager.WriteGenericList<T>(list, filepath);
                b = true;
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return b;
        }

        public static bool TryWrite<T>(T model, out string message, string filepath = "") where T : class, new()
        {
            bool b = false;
            message = string.Empty;
            filepath = !String.IsNullOrWhiteSpace(filepath) ? filepath : Filepath<T>();
            try
            {
                //JsonSerializerOptions options = new JsonSerializerOptions() 
                //{ 
                //    WriteIndented = true, 
                //    IgnoreReadOnlyProperties = true, 
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                //};

                //var json = JsonConvert.SerializeObject(model, Formatting.Indented);
                //File.WriteAllText(filepath, json);
                GenericObjectManager.WriteGenericItem<T>(model, filepath);
                b = true;
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return b;
        }


        public static bool TryLocateFile(out FileInfo info, bool isAppendName = false)
        {
            bool b = false;
            info = null;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            Nullable<bool> result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var candidate = dialog.FileName;
                if (File.Exists(candidate))
                {
                    info = new FileInfo(candidate);
                    b = true;
                }
            }

            return b;
        }





        public static bool TryCopyTo(FileInfo from, FileInfo to, out string message, bool overwriteExisting = true)
        {
            bool b = false;
            message = string.Empty;
            if (File.Exists(from.FullName) && to.Directory.Exists)
            {
                try
                {
                    File.Copy(from.FullName, to.FullName, overwriteExisting);
                    b = true;
                }
                catch (Exception ex)
                {
                    message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
            return b;
        }


        public static bool TryCopyTo(FileInfo info, DirectoryInfo destinationDirectory, out string message, bool overwriteExisting = true)
        {
            bool b = false;
            message = string.Empty;
            if (!destinationDirectory.Exists)
            {
                Directory.CreateDirectory(destinationDirectory.FullName);
            }
            FileInfo destinationInfo = new FileInfo(Path.Combine(destinationDirectory.FullName, info.Name));
            b = TryCopyTo(info, destinationInfo, out string copyMessage, overwriteExisting);
            message = copyMessage;
            return b;
        }


        public static bool TryMove(FileInfo from, FileInfo to)
        {
            bool b = false;
            if (from.Exists)
            {
                if (!to.Directory.Exists)
                {
                    to.Directory.Create();
                }
                try
                {
                    File.Move(from.FullName, to.FullName);
                    b = true;
                }
                catch { }
            }

            return b;
        }



        public static bool TryFindDirectory(out DirectoryInfo info, string selectedPath = "")
        {
            bool b = false;
            info = null;



            return b;
        }

        private static Dictionary<Type, string> filepathmaps = new Dictionary<Type, string>() 
        {
            { typeof(TaskManager), "tasks" },
            { typeof(GraphDesigner), "graphs" },
        };

        public static string Filepath<T>() where T : class, new()
        {
            T t = new T();
            var type = t.GetType();
            var name = !filepathmaps.ContainsKey(type) ? type.Name.Expand() : filepathmaps[type];
            return $"{name}.xml";

        }

        public static string Filepath<T>(string suffix) where T : class, new()
        {
            T t = new T();
            var type = t.GetType();
            var name = !filepathmaps.ContainsKey(type) ? type.Name.Expand() : filepathmaps[type];
            return $"{name}_{suffix}.xml";
        }


    }
}
