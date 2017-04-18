using System;
using System.Collections.Generic;

namespace RoutingLib
{
    public class Graph
    {
        public static Graph Create()
        {
            return new Graph();
        }

        public Graph()
        {
            Nodes = new Dictionary<string, Node>();
        }

        public Dictionary<string, Node> Nodes { get; }

        public void AddEdge(string line)
        {
            var origin = CreateNode(line[0].ToString());
            var target = CreateNode(line[1].ToString());
            var costString = line.Substring(2);
            int cost;
            if (int.TryParse(costString, out cost))
            {
                origin.AddEdge(Edge.Create(target, cost));
            }
        }

        public Node FindNode(string name)
        {
            try
            {
                return Nodes[name.Trim().ToUpper()];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Node[] FindNodes(string[] nodeNames)
        {
            var list = new List<Node>();
            foreach (var name in nodeNames)
            {
                var node = Nodes[name.Trim().ToUpper()];
                if (node != null)
                {
                    list.Add(node);
                }
            }
            return list.ToArray();
        }

        private Node CreateNode(string key)
        {
            Node node;
            if (Nodes.TryGetValue(key, out node)) return node;
            node = Node.Create(key);
            Nodes.Add(key, node);
            return node;
        }
    }
}