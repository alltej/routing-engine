using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingLib.Engine
{
    public class RouteEngineV2 : IRouteEngine
    {
        public RouteResult GetRoutes(Node from, Node to, int? maxCost = default(int?), int? maxDepth = default(int?))
        {
            if (from == null) throw new ArgumentException($"{nameof(from)} node cannot be null");
            if (to == null) throw new ArgumentException($"{nameof(to)} node cannot be null");

            var routes = maxCost.HasValue 
                ? FindAllRoutesWithCost(from, to, maxCost.Value, maxDepth) 
                : FindAllRoutes(from, to, maxDepth);

            var result = new RouteResult(routes);

            return result;
        }

        public RouteResult GetRoutesWithMaxDepth(Node from, Node to, int maxDepth)
        {
            return GetRoutes(from, to, maxDepth: maxDepth);
        }

        public RouteResult GetRoutesWithMaxCost(Node from, Node to, int maxCost)
        {
            return GetRoutes(from, to, maxCost, null);
        }
        private List<Route> FindAllRoutesWithCost(Node from, Node to, int maxDistance, int? maxDepth)
        {
            var allRoutes = FindAllRoutes(from, to);
            var lowestCost = allRoutes.Min(cost => cost.Cost);
            var maxCost = allRoutes.Max(cost => cost.Cost);

            var routesWithMaxCost = allRoutes.Where(p => p.Cost == maxCost); 
            var maxDepthWithMaxCost = routesWithMaxCost.Max(r => r.Stops);

            //calculate depth of the path with the shortest cost
            var targetCost = maxDistance;

            var targetDepth = CalculateTargetDepth(maxDepth, targetCost, lowestCost, maxDepthWithMaxCost);

            var routesByDepth = FindAllRoutes(from, to, targetDepth);

            var routes = maxDepth.HasValue 
                ? routesByDepth.Where(p => p.Cost <= targetCost && p.Stops <= maxDepth.Value) 
                : routesByDepth.Where(p => p.Cost <= targetCost);

            return routes.ToList();
        }

        public static int CalculateTargetDepth(int? maxDepth, int targetCost, int lowestCost, int maxDepthInShortestRoutes)
        {
            var depthMultiplier = targetCost/lowestCost;

            var targetDepth = depthMultiplier*(maxDepthInShortestRoutes);

            if (maxDepth.HasValue && maxDepth <= targetDepth)
            {
                targetDepth = maxDepth.Value;
            }
            return targetDepth;
        }

        private static List<Route> FindAllRoutes(Node from, Node to, int? maxDepth = null)
        {
            var paths = FindAllPaths(from, to, maxDepth);

            var routes = new List<Route>();

            foreach (var path in paths)
            {
                var route = Route.CreateFromTuples(path.Paths);
                routes.Add(route);
            }
            return routes;
        }

        public Route BuildRouteFor(params Node[] nodes)
        {
            var path = new PathV1(nodes.ToList());
            var route = Route.CreateFromPath(path);

            return route;
        }

        private static IEnumerable<PathV2> FindAllPaths(Node from, Node target, int? depth = null)
        {
            var queue = new Queue<Tuple<Node, List<Tuple<string, int>>>>();
            queue.Enqueue(new Tuple<Node, List<Tuple<string, int>>>(from, new List<Tuple<string, int>>()));

            var paths = new List<PathV2>();
            while (queue.Any())
            {
                var dequeuedTuple = queue.Dequeue();
                var currentNode = dequeuedTuple.Item1;
                var currentNodePaths = dequeuedTuple.Item2;

                if (depth.HasValue)
                {
                    if (IsNumberOfDepthReached(depth.Value, currentNodePaths.Count)) continue;
                }
                else
                {
                    //this if condition to check if the PATH (traverses) contains the currentNode
                    if (currentNodePaths.FirstOrDefault(c=> c.Item1 == currentNode.Name ) != null)
                    {
                        if (IsCurrentNodeTargetNode(currentNode, target))
                        {
                            AddEdgesTargetNodeToQueueTuple(queue, currentNode, currentNodePaths);

                            paths.Add(new PathV2(currentNodePaths));
                        }
                        continue;
                    }
                }

                AddEdgesTargetNodeToQueueTuple(queue, currentNode, currentNodePaths);

                if (IsCurrentNodeTargetNode(currentNode, target))
                {
                    var tuple = Tuple.Create(currentNode.Name, 0);
                    var tuples = new List<Tuple<string, int>>(currentNodePaths) { tuple };
                    paths.Add(new PathV2(tuples));
                }
            }
            queue.TrimExcess();
            if (from.Name == target.Name && paths.Any())
                paths.Remove(paths[0]);

            return paths;
        }

        private static bool IsCurrentNodeTargetNode(Node currentNode, Node targetNode)
        {
            return currentNode == targetNode;
        }

        private static bool IsNumberOfDepthReached(int depth, int currentNodePathsCount)
        {
            return currentNodePathsCount > depth;
        }

        private static void AddEdgesTargetNodeToQueueTuple(Queue<Tuple<Node, List<Tuple<string, int>>>> queue, Node currentNode, List<Tuple<string, int>> currentNodePaths)
        {
            foreach (var edge in currentNode.Edges)
            {
                var tuple = Tuple.Create(currentNode.Name, edge.Value.Cost);
                var tuples = new List<Tuple<string, int>>(currentNodePaths) {tuple};
                queue.Enqueue(new Tuple<Node, List<Tuple<string, int>>>(edge.Value.Target, new List<Tuple<string, int>>(tuples)));
            }
        }

    }

}