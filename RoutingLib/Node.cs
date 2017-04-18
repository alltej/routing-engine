using System;
using System.Collections.Generic;

namespace RoutingLib
{
    public class Node
    {
        private readonly Dictionary<string, Edge> _edges;

        public static Node Create(string name)
        {
            return new Node(name);
        }
        private Node(string name)
        {
            Name = name.ToUpper();
            _edges = new Dictionary<string, Edge>();
        }

        public Dictionary<string, Edge> Edges => _edges;

        public void AddEdge(Edge edge)
        {
            try
            {
                if (this.Name.Equals(edge.Target.Name,StringComparison.OrdinalIgnoreCase))
                {
                    throw new ApplicationException("Edge target cannot be the same as the origin node");
                }
                _edges.Add(edge.Target.Name, edge);
            }
            catch (ArgumentException)
            {
                throw new ApplicationException("Edge already exists");
            }
        }

        public string Name { get; }
    }
}