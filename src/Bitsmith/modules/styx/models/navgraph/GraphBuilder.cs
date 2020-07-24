using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Bitsmith.Styx
{
    public class GraphBuilder<T, U> where T : class, new() where U : class, new()
    {
        private List<NavNode<T, U>> _Nodes = new List<NavNode<T, U>>();
        private List<NavLink<T, U>> _Links = new List<NavLink<T, U>>();
        private HashSet<string> _NodeHS = new HashSet<string>();
        private HashSet<string> _LinkHS = new HashSet<string>();
        private List<string> _Lines = new List<string>();
        private string _Text;
        private bool _CanParse;

        public static T DefaultCreate(string text) =>  new T();
        public Func<string,T> CreateNode { get; set; }
        public GraphBuilder(string graphRepresentation)
        {
            _Text = graphRepresentation;
            var lines = _Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            _Lines = new List<string>(lines);
        }

        public GraphBuilder(string[] lines)
        {
            _Lines = (from line in lines select line.TrimEnd()).ToList();
        }
        public static bool TryParse(string text,
            out NavGraph<T, U> navgraph,
            out string message, Func<string, T> createNode ) 
        {
            
            bool b = false;
            navgraph = null;
            message = string.Empty;

            var lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            GraphBuilder<T,U> builder = new GraphBuilder<T, U>(lines);
            builder.CreateNode = createNode != null ? createNode : DefaultCreate;
            if (builder.Parse())
            {
                navgraph = builder.Build();
                b = true;
            }
            return b;
        }
        public static NavGraph<T,U> Build(string filepath)
        {
            NavGraph<T,U> graph = null;
            FileInfo info = new FileInfo(filepath);
            if (info.Exists)
            {
                GraphBuilder<T,U> builder = new GraphBuilder<T,U>(File.ReadAllLines(info.FullName));
                if (builder.Parse())
                {
                    graph = builder.Build();
                }
            }
            return graph;
        }


        public NavGraph<T,U> Build()
        {
            NavGraph<T,U> graph = null;
            if (_CanParse)
            {
                graph = new NavGraph<T,U>(_Nodes,_Links);
            }
            return graph;
        }

        public static int Level(string text, int step = 4)
        {
            int j = 0;
            foreach (var c in text)
            {
                int i = (int)c;
                if (i == (int)'\t')
                {
                    j += step;
                }
                else if (i == 32)
                {
                    j++;
                }
                else
                {
                    break;
                }
            }
            int total = j == 0 ? 0 : j % step == 0 ? j / step : j + 1;
            return total;
            //if (total > 0 && total % step == 0)
            //{
            //    return total / step;
            //}
            //else
            //{
            //    return total;
            //}
               
        }

        public bool Parse()
        {
            if (_Lines.Count > 1)
            {
                
                Dictionary<string, NavNode<T,U>> nodes = new Dictionary<string, NavNode<T,U>>();
                foreach (var item in _Lines)
                {
                    var candidate = item.ToToken();
                    if (_NodeHS.Add(candidate))
                    {
                        NavNode<T,U> node = new NavNode<T, U>() { Display = item.Trim(), Token = candidate };
                        nodes.Add(candidate, node);
                        _Nodes.Add(node);
                    }
                }
                Stack<LevelNode> stack = new Stack<LevelNode>();
                int step = 4;
                int max = _Lines.Count - 1;
                int linenumber = 0;
                string line = _Lines[linenumber];
                string token = line.ToToken();
                NavNode<T,U> master = nodes[token];
                int level = Level(line, step);

                var levelnode = new LevelNode()
                {
                    Level = Level(line, step),
                    Token = token
                };
                stack.Push(levelnode);
                do
                {
                    line = _Lines[++linenumber];
                    token = line.ToToken();
                    var next = new LevelNode()
                    {
                        Level = Level(line, step),
                        Token = token
                    };
                    if (next.Level == stack.Peek().Level) // next peer
                    {
                        stack.Pop();
                        if (stack.Count > 0)
                        {
                            var from = nodes[stack.Peek().Token];
                            var to = nodes[next.Token];
                            _Links.Add(new NavLink<T,U>(from, to));
                        }
                        stack.Push(next);
                    }
                    else if (next.Level > stack.Peek().Level) // first minion
                    {
                        var from = nodes[stack.Peek().Token];
                        var to = nodes[next.Token];
                        _Links.Add(new NavLink<T,U>(from, to));
                        stack.Push(next);
                    }
                    else if (next.Level < stack.Peek().Level) // ancestor peer
                    {
                        while (stack.Count > 0 && stack.Peek().Level >= next.Level)
                        {
                            stack.Pop();
                        }
                        if (stack.Count == 0)
                        {
                            stack.Push(next);
                        }
                        else
                        {
                            var from = nodes[stack.Peek().Token];
                            var to = nodes[next.Token];
                            _Links.Add(new NavLink<T,U>(from, to));
                            stack.Push(next);
                        }
                    }

                } while (linenumber < max);

                _CanParse = true;
            }
            return _CanParse;
        }
    }
}
