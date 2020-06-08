using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class UserSettingsModule : Module
    {
        public bool IsAuthenticate { get; set; } = false;

        public bool IsSaveOnClose { get; set; } = true;
    }
}
