using Bitsmith.BusinessProcess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bitsmith.ViewModels
{
    public class SettingsModule : Module<Settings>
    {

        public WorkflowCollection Workflows { get; set; }

        private Settings _Settings;
        public Settings Settings
        { 
            get { return _Settings; }
            set
            {
                _Settings = value;
            }
        }

        public SettingsModule()
        {

        }


        protected override bool LoadData()
        {
            string filepath = Filepath;
            if (!File.Exists(filepath))
            {
                Settings settings = new Settings().Default();
                if (!FileSystemDataProvider.TryWrite<Settings>(settings, out string error, filepath))
                {
                    OnFailure(error);
                }
            }

            bool b = FileSystemDataProvider.TryRead<Settings>(Filepath, out _Settings, out string message);
            if (!b)
            {
                OnFailure(message);
            }

            return b;
        }

        public override void Initialize()
        {
            Workflows = new WorkflowCollection();
            foreach (var workflow in _Settings.Workflows)
            {
                Workflows.Add(workflow);
            }
        }

        protected override void SaveData()
        {
            base.SaveData();
        }


        protected virtual void OnFailure(string message)
        {
            MessageBox.Show(message);
        }

    }
}
