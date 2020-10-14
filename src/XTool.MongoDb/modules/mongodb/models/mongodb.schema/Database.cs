using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.MongoDb.Schema
{
    public class Database
    {
        public string Display
        {
            get { return $"{Name} ({SizeOnDisk/1000})"; }
        }
        public string Fullname { get; set; }
        public string Name { get; set; }
        public long SizeOnDisk { get; set; }
        public bool IsEmpty { get; set; }

        public List<Collection> Collections { get; set; } = new List<Collection>();
    }
}
