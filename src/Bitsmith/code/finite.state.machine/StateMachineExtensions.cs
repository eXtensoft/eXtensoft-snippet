using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml;
using System.Xml.Linq;

namespace Bitsmith
{
    public static class StateMachineExtensions
    {
        private static Dictionary<string, string> maps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "content", "\uE929" },
            { "tasks", "\uE8FD" },
            { "timeentry", "\uE916" },
            { "settings", "\uE713" },
            { "loggedoff", "\uE894" },
            { "credentials", "\uE74E" },
            { "rolodex","\uEC25"}, //E70A E8CF
            { "datatool","\uE90F"}, // E90F E80A E943 {}
        };
        public static StateManager Initialize(this StateManager manager, XDocument xdoc)
        {
            manager.Machine = new StateMachine(xdoc);
            if (manager.Machine.States.Any(x => x.IsNavigate))
            {
                manager.IsShowNavMenu = true;
                var cannavigateto = from t in manager.Machine.Transitions.Where(x => x.OriginState.Equals(manager.Machine.BeginState)) select t.Name;

                manager.Menu = new System.Collections.ObjectModel.ObservableCollection<NavMenuItem>(
                        from x in manager.Machine.States
                        orderby x.SortOrder
                        select new NavMenuItem()
                        {
                            Display = x.Display,
                            Name = x.Name,
                            IsShow = x.IsNavigate,
                            IsCurrent = x.Name.Equals(manager.Machine.BeginState, StringComparison.OrdinalIgnoreCase),
                            SortOrder = x.SortOrder,
                            CanNavigateTo = cannavigateto.Contains(x.Name),
                            Content = maps.ContainsKey(x.Name)?  maps[x.Name]:x.Name
                        }
                    );

                var view = (CollectionView)CollectionViewSource.GetDefaultView(manager.Menu);                
                view.Filter = new Predicate<object>((o) =>
                {
                    var item = o as NavMenuItem;
                    return (item != null && item.IsShow);
                });
            }
            return manager;
        }

    }
}
