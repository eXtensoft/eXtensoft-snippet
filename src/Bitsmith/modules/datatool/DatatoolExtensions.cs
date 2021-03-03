using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public static class DatatoolExtensions
    {
        public static Datatool Default(this Datatool model)
        {
            return model;
        }

        public static TabularData Default(this TabularData model, string body)
        {
            model.Body = body;
            return model;
        }

        public static TabularData Default(this TabularData model, FileInfo fileInfo)
        {
            model.Info = fileInfo;
            model.Fields = new List<DataField>();
            return model;
        }

        public static ContentItem Default(this ContentItem model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.Mime = "data";
            model.Display = "Data";
            return model;
        }


        private static List<string> _Exclusions = new List<string>() { "id","_id" };
        private static List<string> _Whitelist = new List<string>() { "System.String","System.Int32","System.Decimal","System.Boolean","System.Datetime"};
        public static bool IsPipelineByDefault(this DataField model)
        {
            bool b = true;
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                if (_Exclusions.Contains(model.Name.Trim().ToLower()))
                {
                    b = false;
                }
                else if(!_Whitelist.Any(y=>y.Equals(model.FieldType,StringComparison.OrdinalIgnoreCase)))
                {
                    b = false;
                }
            }

            return b;
        }

    }

}
