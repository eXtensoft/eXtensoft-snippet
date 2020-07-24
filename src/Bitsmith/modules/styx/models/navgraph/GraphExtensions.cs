using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Bitsmith.Styx
{
    public static class GraphExtensions
    {
        public static bool TryBuildRoot(this Graph graph, out Vertex root)
        {
            bool b = false;
            root = null;
            HashSet<string> hs = new HashSet<string>();
            foreach (var vertex in graph.Vertices)
            {
                hs.Add(vertex.Identifier.Token);
                vertex.Graph = graph;
            }
            foreach (var edge in graph.Edges)
            {
                edge.From = graph.Vertices.FirstOrDefault(v => v.Identifier.Token.Equals(edge.Vector.From));
                edge.To = graph.Vertices.FirstOrDefault(v => v.Identifier.Token.Equals(edge.Vector.To));
            }

            root = graph.Vertices.FirstOrDefault(v => v.Inbound.Count == 0);
            b = root != null;
                        
            return b;
        }

        public static string ToToken(this string candidate)
        {
            List<string> list = new List<string>();
            list.Add(candidate);
            list.Add(list[list.Count - 1].Trim());
            list.Add(list[list.Count - 1].Replace("&", "and"));
            list.Add(list[list.Count - 1].Replace("  ", " "));
            list.Add(list[list.Count - 1].Replace(" ", "-"));

            return list[list.Count - 1].ToLower();
        }

        public static Graph ToGraph<T,U>(this NavGraph<T,U> navGraph) where T : class, new() where U : class, new()
        {
            Graph g = new Graph() 
            { 
                Vertices = new List<Vertex>(), 
                Edges = new List<Edge>(), 
                Identifier = new Tag() 
                { 
                    Id = Guid.NewGuid().ToString(), 
                    Token = "graph", 
                    Display = "Graph"
                }
            };
            foreach (var node in navGraph.Nodes)
            {
                Vertex v = new Vertex().Default();
                v.Identifier.Display = node.Display;
                v.Identifier.Token = node.Token;
                v.Identifier.Id = Guid.NewGuid().ToString();
                g.Vertices.Add(v);
            }

            foreach (var link in navGraph.Links)
            {
                Edge e = new Edge().Default();
                e.Identifier.Display = link.ToString();
                e.Vector.From = link.From.Token;
                e.Vector.To = link.To.Token;
                g.Edges.Add(e);
            }
            

            return g;
        }

        private static Vertex Default(this Vertex model)
        {
            model.Identifier = new Tag();
            //model.Labels = new List<Tag>();
            //model.Properties = new List<Property>();
            return model;
        }
        private static Edge Default(this Edge model)
        {
            model.Identifier = new Tag();
            //model.Labels = new List<Tag>();
            //model.Properties = new List<Property>();
            model.Vector = new Direction();
            return model;
        }



    }
}
