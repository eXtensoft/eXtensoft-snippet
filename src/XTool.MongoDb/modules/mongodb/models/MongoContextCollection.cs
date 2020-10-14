using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool.MongoDb.Schema
{
    public class MongoContextCollection : KeyedCollection<string, MongoContext>
    {
        protected override string GetKeyForItem(MongoContext item)
        {
            return item.Key;
        }
    }
}
