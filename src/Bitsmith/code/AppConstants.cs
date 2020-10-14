
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public static class AppConstants
    {
        public const string OverlayContent = "overlay.content";
        public const string ContentDirectory = "content";
        public const string ContentFiles = "content-files";
        public const string TasksDirectory = "task";
        public const string StyxDirectory = "styx";
        public const string SettingsDirectory = "settings";
        public const string ChronosDirectory = "chronos";
        public const string Default = "cb883c5d-3141-4a67-97d8-6475527f3179";
        public const string LastOpenedFileDialogFolderpath = "openfile.dialog.folderpath";
        public const string StateMachine = "state-machine";

        public static class Defaults
        {
            public const string WorkflowId = "05cfd309-c090-43b4-958f-96d3652cd797";
        }

        public static class Tags
        {
            public const string Prefix = "x";
            public const string Domain = "domain";
            public const string CreatedBy = "created-by";
            public const string CreatedAt = "created-at";
            public const string ModifiedAt = "modified-at";
            public const string ViewedAt = "viewed-at";
            public const string Extension = "file-extension";
            public const string Task = "task";
            public const string Credentials = "credentials";
        }

        public static class Paths
        {
            public const string Content = "content";
            public const string Files = "files";
            public const string Default = "virtual";
        }
    }
}
