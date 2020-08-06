using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bitsmith
{
    public class NavMenuItemStyleSelector : StyleSelector
    {
        public Style Default { get; set; }
        public Style CurrentState { get; set; }

        public Style CanNavigateTo { get; set; }

        public Style CannotNavigateTo { get; set; }
        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style style = null;
            var navmenuitem = item as NavMenuItem;
            if (navmenuitem != null)
            {
                if (navmenuitem.IsCurrent)
                {
                    style = CurrentState;
                }
                else if (navmenuitem.CanNavigateTo)
                {
                    style = CanNavigateTo;
                }
                else
                {
                    style = CannotNavigateTo;
                }
            }
            if (style == null)
            {
                style = Default;
            }

            return style;
        }
    }
}
