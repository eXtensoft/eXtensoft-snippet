using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XTool.MongoDb.Schema;

namespace XTool.MongoDb
{
    public static class MongoDBExtensions
    {
        public static ConnectionString Default(this ConnectionString model)
        {
            model.Display = "local";
            model.Key = Guid.NewGuid().ToString();
            model.Text = "mongodb://localhost:27017";
            return model;
        }

        public static bool TryConnect(this string cn, out MongoContext context)
        {
            bool b = false;
            context = new MongoContext();
            if (ValidateMongoDbConnectionString(cn))
            {
                try
                {
                    var client = new MongoClient(cn);
                    var list = client.ListDatabases().ToList();
                    if (list.Any())
                    {
                        var start = cn.LastIndexOf('/') + 1;
                        var length = cn.LastIndexOf(':') - start;
                        context.Display = cn.Substring(start, length);
                        context.Client = client;
                        b = true;
                        foreach (var item in list)
                        {
                            var name = item.GetElement("name").Value.AsString;
                            var db = new Database()
                            {
                                Name = name,
                                SizeOnDisk = (long)item.GetElement("sizeOnDisk").Value.AsDouble,
                                IsEmpty = item.GetElement("empty").Value.AsBoolean
                            };
                            var mongodb = client.GetDatabase(name);
                            foreach (var collectionName in mongodb.ListCollectionNames().ToList())
                            {
                                var col = new Collection() 
                                { 
                                    Fullname = $"{name}.{collectionName}",
                                    Name = collectionName,
                                };
                                db.Collections.Add(col);
                            }
                            context.Databases.Add(db);

                        }
                    }
                }
                catch (Exception ex)
                {
                    b = false;
                    MessageBox.Show(ex.Message);
                }
            }


            return b;
        }

        private static bool ValidateMongoDbConnectionString(string candidate)
        {
            return !string.IsNullOrWhiteSpace(candidate);
        }
    }
}
