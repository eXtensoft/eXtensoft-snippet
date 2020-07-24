using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Bitsmith.Styx
{
    public class Graph: Item
    {
        public string Id { get; set; } 
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }

        public Graph()
        {
        
        }
        public Graph(List<Vertex> vertices, List<Edge> edges)
        {
            
            Vertices = vertices;
            Edges = edges;
            Initialize();
        }
        public void Initialize()
        {
            foreach (var edge in Edges)
            {
                edge.From = Vertices.FirstOrDefault(v => v.Identifier.Token.Equals(edge.Vector.From));
                edge.To = Vertices.FirstOrDefault(v => v.Identifier.Token.Equals(edge.Vector.To));            
            }
            foreach (var vertex in Vertices)
            {
                vertex.Graph = this;
            }
        }
    }
}
