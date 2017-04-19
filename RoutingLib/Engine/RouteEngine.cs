using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingLib.Engine
{
    public class RouteEngine : IRouteEngine
    {
        //if no depth provided, we will just return the direct(no reverse) paths
        public RouteResult GetRoutesBetween(Node from, Node to, int? maxDepth = null, int? maxDistance = null)
        {
            if (from == null) throw new ArgumentException($"{nameof(from)} node cannot be null");
            if (to == null) throw new ArgumentException($"{nameof(to)} node cannot be null");

            var routes = maxDistance.HasValue 
                ? GetRoutesWithMaxDistance(from, to, maxDistance.Value, maxDepth) 
                : GetRoutes(from, to, maxDepth);

            var result = new RouteResult(routes);

            return result;
        }
        
        //if no depth provided, we will just return the direct(no reverse) paths
        public RouteResult GetRoutesBetweenParallel(Node from, Node to, int? maxDepth = null, int? maxDistance = null)
        {
            Console.WriteLine("Parallel");
            if (from == null) throw new ArgumentException($"{nameof(from)} node cannot be null");
            if (to == null) throw new ArgumentException($"{nameof(to)} node cannot be null");

            var routes = maxDistance.HasValue 
                ? GetRoutesWithMaxDistanceParallel(from, to, maxDistance.Value, maxDepth) 
                : GetRoutesParallel(from, to, maxDepth);

            var result = new RouteResult(routes);

            return result;
        }

        private List<Route> GetRoutesWithMaxDistance(Node from, Node to, int maxDistance, int? maxDepth)
        {
            var pathOutput = GetRoutes(from, to);
            var lowestCost = pathOutput.Min(cost => cost.Cost);

            var shortestPaths = pathOutput.Where(p => p.Cost == lowestCost); //.ToList();
            var maxDepthInShortestRoutes = shortestPaths.Max(r => r.PathString.Length);

            //calculate depth of the path with the shortest cost
            var targetCost = maxDistance;
            var depthMultiplier = targetCost/lowestCost;

            var targetDepth = depthMultiplier*(maxDepthInShortestRoutes);

            if (maxDepth.HasValue && maxDepth < targetDepth)
            {
                targetDepth = maxDepth.Value;
            }
            var allRoutes = GetRoutes(from, to, targetDepth);

            var routes = maxDepth.HasValue 
                ? allRoutes.Where(p => p.Cost <= targetCost && p.Stops <= maxDepth.Value) 
                : allRoutes.Where(p => p.Cost <= targetCost);

            return routes.ToList();
        }
        private List<Route> GetRoutesWithMaxDistanceParallel(Node from, Node to, int maxDistance, int? maxDepth)
        {
            var pathOutput = GetRoutesParallel(from, to);

            //if (!pathOutput.Any()) return new List<Route>();

            var lowestCost = pathOutput.Min(cost => cost.Cost);

            var shortestPaths = pathOutput.Where(p => p.Cost == lowestCost); //.ToList();
            var maxDepthInShortestRoutes = shortestPaths.Max(r => r.PathString.Length);

            //calculate depth of the path with the shortest cost
            var targetCost = maxDistance;
            var depthMultiplier = targetCost/lowestCost;

            var targetDepth = depthMultiplier*(maxDepthInShortestRoutes);

            var allRoutes = GetRoutesParallel(from, to, targetDepth);

            var routes = maxDepth.HasValue 
                ? allRoutes.Where(p => p.Cost <= targetCost && p.Stops <= maxDepth.Value) 
                : allRoutes.Where(p => p.Cost <= targetCost);

            return routes.ToList();
        }
        
        //if no depth provided, we will just return the direct(no reverse) paths
        private List<Route> GetRoutes(Node from, Node to, int? maxDepth = null)
        {
            var paths = FindAllPaths(from, to, maxDepth);

            var routes = new List<Route>();

            foreach (var path in paths)
            {
                var route = CreateRouteFromPath(path);
                routes.Add(route);
            }
            //return routes.OrderBy(r => r.Cost).ToList();
            return routes;
        }        

        //if no depth provided, we will just return the direct(no reverse) paths
        private List<Route> GetRoutesParallel(Node from, Node to, int? maxDepth = null)
        {
            //var queue = new Queue<Node>();
            //AddFromInitQueue(queue, from.Edges);

            //while (queue.Any())
            //{
            //    var pathsByInit = FindAllPathsAsync(from, to, maxDepth);
            //}
            var results = new List<Path>();
            //Task.Run(() =>
            Parallel.ForEach(from.Edges, edge =>
                {
                    var p = FindAllPaths(edge.Value.Target, to, maxDepth);
                    if (p.Any())
                    {
                        lock (results)
                        {
                            results.AddRange(p.ToList());
                        }
                    }

                }
            );

           // var paths = FindAllPaths(from, to, maxDepth);

            var routes = new List<Route>();

            foreach (var path in results)
            {
                path.Nodes.Insert(0, from);
                var route = CreateRouteFromPath(path);
                routes.Add(route);

            }
            //return routes.OrderBy(r => r.Cost).ToList();
            return routes;
        }

        //private void AddFromInitQueue(Queue<Node> queue, Dictionary<string, Edge> edges)
        //{
        //    foreach (var edge in edges)
        //    {
        //        queue.Enqueue(Node.Create(edge.Value.Target.Name));
        //    }
        //}

        public Route BuildRouteFor(params Node[] nodes)
        {
            var path = new Path(nodes.ToList());
            var route = CreateRouteFromPath(path);

            return route;
        }

        private static Route CreateRouteFromPath(Path path)
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

        private static IEnumerable<Path> FindAllPaths(Node from, Node target, int? depth = null)
        {
            var queue = new Queue<Tuple<Node, List<Node>>>();
            queue.Enqueue(new Tuple<Node, List<Node>>(from, new List<Node>()));

            var paths = new List<Path>();

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
                        if (IsCurrentNodeATargetNode(currentNode, target))
                        {
                            AddCurrentNodesEdgesToQueue(queue, currentNode, currentNodePaths);

                            paths.Add(new Path(currentNodePaths));
                        }
                        continue;
                    }
                }

                AddCurrentNodesEdgesToQueue(queue, currentNode, currentNodePaths);

                if (IsCurrentNodeATargetNode(currentNode, target))
                {
                    paths.Add(new Path(currentNodePaths));
                }

            }

            if (from.Name == target.Name && paths.Any())
                paths.Remove(paths[0]);

            return paths;
        }
        //private static async Task<List<Path>> FindAllPathsAsync(Node from, Node target, int? depth = null)
        //{
        //    var queue = new Queue<Tuple<Node, List<Node>>>();
        //    queue.Enqueue(new Tuple<Node, List<Node>>(from, new List<Node>()));

        //    var paths = new List<Path>();

        //    while (queue.Any())
        //    {
        //        var dequeuedTuple = queue.Dequeue();
        //        var currentNode = dequeuedTuple.Item1;
        //        var currentNodePaths = dequeuedTuple.Item2;

        //        if (depth.HasValue)
        //        {
        //            if (IsNumberOfDepthReached(depth.Value, currentNodePaths.Count)) continue;
        //        }
        //        else
        //        {
        //            //this if condition to check if the PATH (traverses) contains the currentNode
        //            if (currentNodePaths.Contains(currentNode))
        //            {
        //                if (IsCurrentNodeATargetNode(currentNode, target))
        //                {
        //                    AddCurrentNodesEdgesToQueue(queue, currentNode, currentNodePaths);

        //                    paths.Add(new Path(currentNodePaths));
        //                }
        //                continue;
        //            }
        //        }

        //        AddCurrentNodesEdgesToQueue(queue, currentNode, currentNodePaths);

        //        if (IsCurrentNodeATargetNode(currentNode, target))
        //        {
        //            paths.Add(new Path(currentNodePaths));
        //        }

        //    }

        //    if (from.Name == target.Name && paths.Any())
        //        paths.Remove(paths[0]);

        //    return await Task.FromResult(paths);
        //    //return paths;
        //}

        private static bool IsCurrentNodeATargetNode(Node currentNode, Node targetNode)
        {
            return currentNode == targetNode;
        }

        private static void AddCurrentNodesEdgesToQueue(Queue<Tuple<Node, List<Node>>> queue, Node currentNode, List<Node> currentNodePaths)
        {
            currentNodePaths.Add(currentNode);
            AddEdgesTargetNodeToQueue(queue, currentNode.Edges, currentNodePaths);
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

    }

}