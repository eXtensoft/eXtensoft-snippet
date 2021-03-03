using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitsmith.Schemas;
using System.Windows;
using System.Collections.ObjectModel;

namespace Bitsmith.ViewModels
{
    public class SchemaModule : Module<Schema>
    {
        private bool _IsEnableSchemas = true;
        public bool IsEnableSchemas
        {
            get
            {
                return _IsEnableSchemas;
            }
            set
            {
                _IsEnableSchemas = value;
                OnPropertyChanged("IsEnableSchemas");
            }
        }

        public SettingsModule Settings { get; set; }

        public ObservableCollection<SchemaBuilderViewModel> JsonSchemas { get; set; }

        public SchemaModule(IDataService dataService, SettingsModule settings)
        {
            DataService = dataService;

            Settings = settings;
        }

        public override void Initialize()
        {
            JsonSchemas = new ObservableCollection<SchemaBuilderViewModel>(from x in Models select new SchemaBuilderViewModel(x));
            UserPreferences = Settings.UserPreferences;
        }
        public override string Filepath()
        {
            return Path.Combine(AppConstants.SchemasDirectory, DataService.Filepath<Schema>());
        }

       

        internal override void SetPreferences()
        {
            var selected = JsonSchemas.FirstOrDefault(p => p.IsSelectedContentSchema);
            if (selected != null)
            {
                UserPreferences.EnsurePreference(ModuleKey, "selectedContentSchema", selected.Id);
            }
        }

        protected override void ApplyPreferences(UserSettings userPreferences)
        {
            if (UserPreferences.TryGet<string>(ModuleKey, "selectedContentSchema", out string id) &&
                JsonSchemas.Any(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                var selected = JsonSchemas.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
                selected.IsSelectedContentSchema = true;
            }
            else
            {
                var first = JsonSchemas.FirstOrDefault();
                if (first != null)
                {
                    first.IsSelectedContentSchema = true;
                }
            }
        }


        protected override bool LoadData()
        {
            string filepath = Filepath();
            if (!File.Exists(filepath))
            {
                Models = new List<Schema>().Default();
                if (!DataService.TryWrite<Schema>(Models,out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = DataService.TryRead<Schema>(filepath, out List<Schema> list, out string message);
            if (b)
            {
                Models = list;
            }
            else
            {
                OnFailure(message);
            }
        

            return b;
        }

        protected virtual void OnFailure(string message)
        {
            MessageBox.Show(message);
        }
    }
}
