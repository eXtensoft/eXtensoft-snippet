using Bitsmith.DataServices.Abstractions;
using Bitsmith.FullText;
using Bitsmith.Indexing;
using Bitsmith.ViewModels;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.NaturalLanguage
{
    public class IndexerModule : Module<LanguageSettings>
    {


        public IContentIndexer Indexer { get; set; }

        private SettingsModule _Settings;
        

        public IndexerModule(IDataService dataService, SettingsModule settings)
        {
            DataService = dataService;
            _Settings = settings;
            Filepath = Path.Combine(AppConstants.SettingsDirectory, base.Filepath);
        }


        public override void Initialize()
        {
            int i = 0;
            //this is where the IContentIndexer Indexer should be built
            var found = Models.FirstOrDefault(y => y.Language.Equals("en-US", StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                var directory = Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles);
                var simple = new SimpleTextIndexer(found) { Directory = new DirectoryInfo(directory) };
                if (DataService != null)
                {
                    string filepath = Path.Combine(AppConstants.ContentDirectory, DataService.Filepath<FullTextIndex>());
                    if (File.Exists(filepath) && 
                        DataService.TryRead<FullTextIndex>(filepath, out FullTextIndex model, out string message))
                    {
                        IsInitialized = true;
                        simple.IsInitialized = true;
                        simple.Indexes.Load(model.Indexes);
                    }
                    else
                    {
                        IsInitialized = false;
                    }
                }
                Indexer = simple;

            }
            
        }

       


        protected override bool LoadData()
        {
            bool b = false;
            var filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Models.Add(new LanguageSettings().Default());
                if (!FileSystemDataProvider.TryWrite<LanguageSettings>(Models, out string error, filepath))
                {
                    OnFailure("load Language Settings", error);
                }
            }
            b = FileSystemDataProvider.TryRead<LanguageSettings>(Filepath, out List<LanguageSettings> list, out string message);
            if (b)
            {
                Models = list;
            }
            else
            {
                OnFailure("", message);
            }


            return b;
        }

        protected override void SaveData()
        {
            //base.SaveData();

            //if (DataService != null)
            //{
            //    FullTextIndex fti = new FullTextIndex() 
            //    { 
            //        Indexes = Indexer.Indexes.ToList(), 
            //        CreatedAt = DateTime.Now 
            //    };
            //    if (fti.Indexes != null && 
            //        fti.Indexes.Count > 0)
            //    {
            //        string filepath = Path.Combine(AppConstants.ContentDirectory, DataService.Filepath<FullTextIndex>());
            //        DataService.TryWrite<FullTextIndex>(fti, out string message, filepath);
            //    }
            //}
        }

    }
}
