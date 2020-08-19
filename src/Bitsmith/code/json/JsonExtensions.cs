using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public static class JsonExtensions
    {
        public static string ToPrettyJson(this string json)
        {
            var obj = JsonConvert.DeserializeObject(json);
            var settings = new JsonSerializerSettings() 
            { 
                ContractResolver = new DefaultContractResolver() 
                { 
                    NamingStrategy = new CamelCaseNamingStrategy() 
                },
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToJson<T>(this T model) where T : class, new()
        {
            var settings = new JsonSerializerSettings() 
            { 
                ContractResolver = new DefaultContractResolver() 
                { 
                    NamingStrategy = new CamelCaseNamingStrategy() 
                },
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(model,settings);
        }
    }
}
