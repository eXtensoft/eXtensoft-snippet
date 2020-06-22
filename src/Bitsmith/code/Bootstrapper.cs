using System;
using System.Collections.Generic;
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
        public static void Start()
        {
            InitializeLocations();
        }

        private static void InitializeLocations()
        {
            DirectoryInfo settings = new DirectoryInfo(AppConstants.SettingsDirectory);
            if (!settings.Exists)
            {
                settings.Create();
            }
            DirectoryInfo project = new DirectoryInfo(AppConstants.ProjectDirectory);
            if (!project.Exists)
            {
                project.Create();
            }
            DirectoryInfo content = new DirectoryInfo(Path.Combine(AppConstants.ContentDirectory, AppConstants.ContentFiles));
            if (!content.Exists)
            {
                content.Create();
            }
            DirectoryInfo chronos = new DirectoryInfo(AppConstants.ChronosDirectory);
            if (!chronos.Exists)
            {
                chronos.Create();
            }
        }

        public static Workspace Workspace()
        {
            return new Workspace();
        }

        internal static Data Data()
        {
            return new Data();
        }

        public static StateManager StateMachine()
        {
            XDocument doc = XDocument.Parse(Resources.statemachine);
            return new StateManager()
            {
                Machine = new StateMachine(doc)
            };
        }
    }
}
