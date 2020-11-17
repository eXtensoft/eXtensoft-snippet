using Bitsmith.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Bitsmith
{
    public static class Bootstrapper
    {
        private static List<string> directories = new List<string>() 
        { 
            AppConstants.SettingsDirectory,
            AppConstants.TasksDirectory,
            AppConstants.ContentDirectory,            
            AppConstants.ChronosDirectory,
            AppConstants.StyxDirectory,
        };

        public static void Start()
        {
            List<string> list = new List<string>(directories);
            list.Add(Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles));
            list.EnsureDirectories();

            InitializeDataService();
            InitializeImageMaps();
        }

        public static void ClearData()
        { 
            directories.EnsureDirectories(true);
        }

        private static void InitializeDataService()
        {
            if (!ConfigValueProvider.TryGetConfigValueAs<string>(AppConstants.Data.DataServiceKey, out string dataservice) || 
                AppConstants.Data.Services.Contains(dataservice))
            {
                dataservice = AppConstants.Data.Default;
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    if (config != null)
                    {
                        var found = config.AppSettings.Settings[AppConstants.Data.DataServiceKey];
                        if (found == null)
                        {
                            config.AppSettings.Settings.Add(AppConstants.Data.DataServiceKey, AppConstants.Data.Default);
                        }
                        
                        config.Save();
                        ConfigurationManager.RefreshSection(AppConstants.Data.AppSettings);
                    }
                }
                catch (ConfigurationErrorsException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static void InitializeImageMaps()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();

            list.Add(new Tuple<string, string>("zh", "images/flags/china-flag.xs.png"));
            list.Add(new Tuple<string, string>("fr", "images/flags/france-flag.xs.png"));
            list.Add(new Tuple<string, string>("de", "images/flags/germany-flag.xs.png"));
            list.Add(new Tuple<string, string>("it", "images/flags/italy-flag.xs.png"));
            list.Add(new Tuple<string, string>("es", "images/flags/spain-flag.xs.png"));
            list.Add(new Tuple<string, string>("en", "images/flags/united-kingdom-flag.xs.png"));

            Application.Current.Properties[AppConstants.ImageMaps] = list;
        }

        public static Workspace Workspace()
        {
            Workspace model = new Workspace();
            string strategy = ConfigValueProvider.GetConfigValueAs<string>(AppConstants.Data.DataServiceKey);
            if (Enum.TryParse<DataServiceStrategyOption>(strategy,true, out DataServiceStrategyOption option))
            {
                model.DataServiceStrategy = option;
            }
            return model;
        }



        public static StateManager StateMachine()
        {
            XDocument doc = XDocument.Parse(Resources.simple_state_machine);
            return new StateManager().Initialize(doc);
        }

        internal static void SetDataStrategy(DataServiceStrategyOption key)
        {
            ClearData();
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config != null)
                {
                    var found = config.AppSettings.Settings[AppConstants.Data.DataServiceKey];
                    if (found != null)
                    {
                        config.AppSettings.Settings.Remove(AppConstants.Data.DataServiceKey);
                    }
                    config.AppSettings.Settings.Add(AppConstants.Data.DataServiceKey, key.ToString().ToLower());

                    config.Save();
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
