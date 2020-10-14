using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.MongoDb.Schema
{
    public class MongoContext
    {
        public string Display { get; set; }
        public string Key { get; set; }
        public IMongoClient Client { get; set; }

        public List<Database> Databases { get; set; } = new List<Database>();
    }
}
