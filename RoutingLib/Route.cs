using System;
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

        public static Route CreateFromPath(Path path)
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
    }
}