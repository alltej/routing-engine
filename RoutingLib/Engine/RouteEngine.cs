using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutingLib.Engine
{
    public class RouteEngine : IRouteEngine
    {
        public RouteResult GetRoutesV1(Node from, Node to, int? maxCost = default(int?), int? maxDepth = default(int?))
        {
            if (from == null) throw new ArgumentException($"{nameof(from)} node cannot be null");
            if (to == null) throw new ArgumentException($"{nameof(to)} node cannot be null");

            var routes = maxCost.HasValue 
                ? FindAllRoutesWithCostV1(from, to, maxCost.Value, maxDepth) 
                : FindAllRoutesV1(from, to, maxDepth);

            var result = new RouteResult(routes);

            return result;
        }

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

        public RouteResult GetRoutesWithMaxDepthV1(Node from, Node to, int maxDepth)
        {
            return GetRoutes(from, to, maxDepth: maxDepth);
        }
        public RouteResult GetRoutesWithMaxDepth(Node from, Node to, int maxDepth)
        {
            return GetRoutes(from, to, maxDepth: maxDepth);
        }

        public RouteResult GetRoutesWithMaxCost(Node from, Node to, int maxCost)
        {
            return GetRoutes(from, to, maxCost, null);
        }

        private List<Route> FindAllRoutesWithCostV1(Node from, Node to, int maxDistance, int? maxDepth)
        {
            var allRoutes = FindAllRoutesV1(from, to);
            var lowestCost = allRoutes.Min(cost => cost.Cost);
            var maxCost = allRoutes.Max(cost => cost.Cost);

            var routesWithMaxCost = allRoutes.Where(p => p.Cost == maxCost); 
            var maxDepthWithMaxCost = routesWithMaxCost.Max(r => r.Stops);

            //calculate depth of the path with the shortest cost
            var targetCost = maxDistance;

            var targetDepth = CalculateTargetDepth(maxDepth, targetCost, lowestCost, maxDepthWithMaxCost);

            var routesByDepth = FindAllRoutesV1(from, to, targetDepth);

            var routes = maxDepth.HasValue 
                ? routesByDepth.Where(p => p.Cost <= targetCost && p.Stops <= maxDepth.Value) 
                : routesByDepth.Where(p => p.Cost <= targetCost);

            return routes.ToList();
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

        private static List<Route> FindAllRoutesV1(Node from, Node to, int? maxDepth = null)
        {
            var paths = FindAllPathsV1(from, to, maxDepth);

            var routes = new List<Route>();

            foreach (var path in paths)
            {
                var route = Route.CreateFromPath(path);
                routes.Add(route);
            }
            return routes;
        }

        private static List<Route> FindAllRoutes(Node from, Node to, int? maxDepth = null)
        {
            var paths = FindAllPaths(from, to, maxDepth);

            var routes = new List<Route>();

            foreach (var path in paths)
            {
                var route = Route.CreateFromTuple(path.Paths);
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

        private static IEnumerable<Path2> FindAllPaths(Node from, Node target, int? depth = null)
        {
            //var queue = new Queue<Tuple<Node, List<Node>>>();
            var queue2 = new Queue<Tuple<Node, List<Tuple<string, int>>>>();
            //queue.Enqueue(new Tuple<Node, List<Node>>(from, new List<Node>()));
            queue2.Enqueue(new Tuple<Node, List<Tuple<string, int>>>(from, new List<Tuple<string, int>>()));

            var paths = new List<Path2>();
            //var pathsTuple = new List<Tuple<string, int>>();
            //
            while (queue2.Any())
            {
                var dequeuedTuple = queue2.Dequeue();
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
                            AddCurrentNodesEdgesToQueueTuple(queue2, currentNode, currentNodePaths);

                            //paths.Add(new Path(currentNodePaths));
                            var last = currentNodePaths.Last().Item1;
                            //currentNodePaths.Add(Tuple.Create(currentNode.Name, currentNode.Edges[last].Cost));
                            //pathsTuple.Add(Tuple.Create(currentNode.Name, currentNode.Edges[last].Cost));
                            // paths.Add(new Path(currentNodePaths));
                            
                            paths.Add(new Path2(currentNodePaths));
                        }
                        continue;
                    }
                }

                AddCurrentNodesEdgesToQueueTuple(queue2, currentNode, currentNodePaths);

                if (IsCurrentNodeTargetNode(currentNode, target))
                {
                    var tuple = Tuple.Create(currentNode.Name, 0);
                    var tuples = new List<Tuple<string, int>>(currentNodePaths) { tuple };
                    paths.Add(new Path2(tuples));
                }
                //queue.TrimExcess();//fix the memory leak but causing the app to slow
            }
            //queue.TrimExcess();
            if (from.Name == target.Name && paths.Any())
                paths.Remove(paths[0]);

            return paths;
        }

        private static IEnumerable<PathV1> FindAllPathsV1(Node from, Node target, int? depth = null)
        {
            var queue = new Queue<Tuple<Node, List<Node>>>();
            queue.Enqueue(new Tuple<Node, List<Node>>(from, new List<Node>()));

            var paths = new List<PathV1>();

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
                    if (currentNodePaths.Contains(currentNode))
                    {
                        if (IsCurrentNodeTargetNode(currentNode, target))
                        {
                            AddCurrentNodesEdgesToQueue(queue, currentNode, currentNodePaths);

                            paths.Add(new PathV1(currentNodePaths));
                        }
                        continue;
                    }
                }

                AddCurrentNodesEdgesToQueue(queue, currentNode, currentNodePaths);

                if (IsCurrentNodeTargetNode(currentNode, target))
                {
                    paths.Add(new PathV1(currentNodePaths));
                }
                queue.TrimExcess();//fix the memory leak but causing the app to slow
            }
            //queue.TrimExcess();
            if (from.Name == target.Name && paths.Any())
                paths.Remove(paths[0]);

            return paths;
        }

        private static bool IsCurrentNodeTargetNode(Node currentNode, Node targetNode)
        {
            return currentNode == targetNode;
        }

        private static void AddCurrentNodesEdgesToQueue(Queue<Tuple<Node, List<Node>>> queue, Node currentNode, List<Node> currentNodePaths)
        {
            currentNodePaths.Add(currentNode);
            AddEdgesTargetNodeToQueue(queue, currentNode.Edges, currentNodePaths);
        }
        private static void AddCurrentNodesEdgesToQueueTuple(Queue<Tuple<Node, List<Tuple<string, int>>>> queue, Node currentNode, List<Tuple<string, int>> currentNodePaths)
        {
            // queue2.Enqueue(new Tuple<Node, List<Tuple<string, int>>>(from, new List<Tuple<string, int>>()));


            AddEdgesTargetNodeToQueueTuple(queue, currentNode, currentNodePaths);
        }

        private static bool IsNumberOfDepthReached(int depth, int currentNodePathsCount)
        {
            return currentNodePathsCount > depth;
        }

        private static void AddEdgesTargetNodeToQueue(Queue<Tuple<Node, List<Node>>> queue, Dictionary<string, Edge> edges, List<Node> currentNodePaths)
        {
            foreach (var edge in edges)
            {
                queue.Enqueue(new Tuple<Node, List<Node>>(edge.Value.Target, new List<Node>(currentNodePaths)));
            }
        }
        private static void AddEdgesTargetNodeToQueueTuple(Queue<Tuple<Node, List<Tuple<string, int>>>> queue, Node currentNode, List<Tuple<string, int>> currentNodePaths)
        {
            //var last = currentNodePaths.Any() ? currentNodePaths.Last() : null;
            //var cost = 0;
            //if (last != null)
            //{
            //    Edge currentNodeEdgeLast;
            //    if (currentNode.Edges.TryGetValue(last.Item1, out currentNodeEdgeLast))
            //    {
            //        cost = currentNodeEdgeLast.Cost;
            //    }
            //}
            //currentNodePaths.Add(Tuple.Create(currentNode.Name, 0));

            foreach (var edge in currentNode.Edges)
            {
                //int cost = 0;
                var tuple = Tuple.Create(currentNode.Name, edge.Value.Cost);
                var tuples = new List<Tuple<string, int>>(currentNodePaths) {tuple};
                queue.Enqueue(new Tuple<Node, List<Tuple<string, int>>>(edge.Value.Target, new List<Tuple<string, int>>(tuples)));
            }
        }

    }

}