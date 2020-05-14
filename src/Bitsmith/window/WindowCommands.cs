using System.Windows.Input;

namespace Bitsmith
{
    public class WindowCommands
    {
        public static readonly ICommand MinimizeWindow = new RoutedCommand("MinimizeWindowCommand", typeof(WindowCommands));
        public static readonly ICommand MaximizeWindow = new RoutedCommand("MaximizeWindowCommand", typeof(WindowCommands));

        public static readonly ICommand DisplayOfficeButtonOptions = new RoutedCommand("DisplayOfficeButtonOptions", typeof(WindowCommands));
    }
}
