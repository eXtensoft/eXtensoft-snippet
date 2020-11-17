using Bitsmith.DataServices.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.DataServices
{
    public class JsonDataService : FilesystemDataService, IDataService
    {
        public DataServiceStrategyOption Key => DataServiceStrategyOption.Json;
        public string Filepath<T>(string suffix = "") where T : class, new()
        {
            return  FileSystemDataProvider.Filepath<T>( FileFormat.Json);
        }
        public bool TryRead<T>(out List<T> list, out string message, string filepath = "") where T : class, new()
        {
            bool b = false;
            message = string.Empty;
            list = new List<T>();
            filepath = !String.IsNullOrWhiteSpace(filepath) ? filepath : Filepath<T>();
            try
            {
                if (File.Exists(filepath))
                {
                    string filecontents = File.ReadAllText(filepath);
                    if (!string.IsNullOrWhiteSpace(filecontents))
                    {

                        list = JsonConvert.DeserializeObject<List<T>>(filecontents);
                        b = true;
                    }
                }
            }
            catch (Exception ex)
            {

                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return b;
        }

        public bool TryRead<T>(string filepath, out T model, out string message) where T : class, new()
        {
            bool b = false;
            model = null;
            message = string.Empty;
            try
            {
                if (File.Exists(filepath))
                {
                    string filecontents = File.ReadAllText(filepath);
                    if (!string.IsNullOrWhiteSpace(filecontents))
                    {
                        model = JsonConvert.DeserializeObject<T>(filecontents);
                        b = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return b;
        }

        public bool TryRead<T>(string filepath,
            out List<T> list,
            out string message) where T : class, new()
        {
            bool b = false;
            list = null;
            message = string.Empty;
            try
            {
                if (File.Exists(filepath))
                {
                    string filecontents = File.ReadAllText(filepath);
                    if (!string.IsNullOrWhiteSpace(filecontents))
                    {
                        list = JsonConvert.DeserializeObject<List<T>>(filecontents);
                        b = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return b;
        }

        public bool TryWrite<T>(List<T> list, out string message, string filepath = "") where T : class, new()
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
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;

                var json = JsonConvert.SerializeObject(list, settings);
                File.WriteAllText(filepath, json);
                b = true;
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return b;
        }

        public bool TryWrite<T>(T model, out string message, string filepath = "") where T : class, new()
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

                var json = JsonConvert.SerializeObject(model, Formatting.Indented);
                File.WriteAllText(filepath, json);
                b = true;
            }
            catch (Exception ex)
            {
                message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return b;
        }
    }
}
