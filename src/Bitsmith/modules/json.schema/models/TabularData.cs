using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public class TabularData
    {
        public bool IsOkay { get; set; } = false;
        public string Message { get; set; }
        public string Mime { get; set; }
        public FileInfo Info { get; set; }
        public DataTable Data { get; set; }

        public string Body { get; set; }

        public List<Property> Properties { get; set; }
        public List<DataField> Fields { get; set; }


    }

    public class TabularColumn
    {
        public bool IsParse { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public Type Datatype { get; set; }
    }
}
