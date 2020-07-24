using System;
using System.Collections.Generic;
using System.Text;

namespace Bitsmith.Styx
{
    public class NavLink<T,U> where U : class, new() where T : class, new()
    {
        public U Model { get; set; }
        public NavNode<T,U> From { get; set; }
        public NavNode<T,U> To { get; set; }

        public NavLink(NavNode<T,U> from, NavNode<T,U> to)
        {
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return $"{From.Display} -> {To.Display}";
        }
    }
}
