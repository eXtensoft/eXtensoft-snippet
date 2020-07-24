using System;
using System.Collections.Generic;
using System.Text;

namespace Bitsmith.Styx
{
    public class NavGraph<T,U> where T : class, new() where U : class, new()
    {
        public List<NavNode<T,U>> Nodes { get; set; }
        public List<NavLink<T,U>> Links { get; set; }

        public NavGraph(List<NavNode<T,U>> nodes, List<NavLink<T,U>> links)
        {
            Nodes = nodes;
            Links = links;
            foreach (var node in Nodes)
            {
                node.Graph = this;
            }
        }
    }
}
