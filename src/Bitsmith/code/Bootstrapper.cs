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
        private static List<string> directories = new List<string>() 
        { 
            AppConstants.SettingsDirectory,
            AppConstants.TasksDirectory,
            Path.Combine(AppConstants.ContentDirectory,AppConstants.ContentFiles),
            AppConstants.ChronosDirectory,
            AppConstants.StyxDirectory,
        };

        public static void Start()
        {
            directories.EnsureDirectories();
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
            XDocument doc = XDocument.Parse(Resources.simple_state_machine);
            return new StateManager().Initialize(doc);

        }
    }
}
