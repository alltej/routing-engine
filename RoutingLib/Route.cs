using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingLib
{
    public class Route
    {
        public Route(string pathString, int stops, int cost)
        {
            PathString = pathString;
            Stops = stops;                                             
            Cost = cost;
        }

        public int Cost { get;  }

        public string PathString { get; }

        public int Stops { get; }

        public static Route Create(string pathString, int stops, int cost)
        {                                                                            
            return new Route(pathString, stops, cost);
        }

        public static Route CreateFromPath(PathV1 path)
        {
            var pathString = "";
            var cost = 0;
            var stops = 0;
            Node previousNode = null;
            foreach (var node in path.Nodes)
            {
                pathString += node.Name;
                stops++;
                var currentNode = node;

                if (previousNode != null)
                {
                    var edge = previousNode.Edges.SingleOrDefault(e => e.Key == currentNode.Name).Value;
                    if (edge == null) throw new ApplicationException("NO SUCH ROUTE");
                    cost += edge.Cost;
                }

                previousNode = currentNode;
            }
            var route = Route.Create(pathString, stops - 1, cost);
            return route;
        }


        public static Route CreateFromTuples(List<Tuple<string, int>> tuple)
        {
            var pathString = "";
            var cost = 0;
            var stops = 0;
            foreach (var node in tuple)
            {
                pathString += node.Item1;
                stops++;
                cost += node.Item2;
            }
            var route = Route.Create(pathString, stops - 1, cost);
            return route;
        }

        public static Route CreateFromNodes(List<Node> nodes)
        {
            var list = GetTupleList(nodes);
            return CreateFromTuples(list);
        }

        public static List<Tuple<string, int>> GetTupleList(List<Node> nodes)
        {
            var path = new List<Tuple<string, int>>();
            var previousNode = nodes.First();
            path.Add(Tuple.Create(previousNode.Name, 0));

            var nodesSkip1 = nodes.Skip(1);

            foreach (var node in nodesSkip1)
            {
                //previousNode.Edges.FirstOrDefault(n=>n.Value.Target == node.Name)
                var edge = previousNode.Edges[node.Name];
                if (edge == null) throw new ApplicationException("NO SUCH ROUTE");
                path.Add(Tuple.Create(edge.Target.Name, edge.Cost));
                previousNode = node;
            }
            return path;
        }
    }
}