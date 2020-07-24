using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Bitsmith.Styx
{
    public class NavNode<T,U> where T : class, new() where U : class, new()
    {
        [JsonIgnore]
        public NavGraph<T,U> Graph { get; set; }
        public string Display { get; set; }
        public string Token { get; set; }
        public T Model { get; set; }

        public NavNode(T model)
        {
            Model = model;
        }

        public NavNode()
        {

        }

        public List<NavLink<T,U>> Inbound
        {
            get
            {
                return Graph.Links.Where(x => x.To == this).ToList();
            }
        }
        public List<NavLink<T,U>> Outbound
        {
            get
            {
                return Graph.Links.Where(x => x.From == this).ToList();
            }
        }
    }
}
