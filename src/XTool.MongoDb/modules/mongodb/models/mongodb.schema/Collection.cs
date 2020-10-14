using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.MongoDb.Schema
{
    public class Collection
    {
        public string Display
        {
            get
            {
                return Name;
            }
            set { }
        }
        public string Fullname { get; set; }
        public string Name { get; set; }
    }
}
