using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RoutingLib;
using RoutingLib.Engine;
using RoutingLib.Factory;

namespace RoutingApp
{
    class Program
    {
        private static Graph _graph;
        private static string _lastKnownFile;

        static void Main(string[] args)
        {
            _graph = args.Length > 0 ? GraphFactory.BuildFromFile(args[0]) : GraphFactory.BuildFromFile();

            var routeEngine = new RouteEngine();

            while (true)
            {
                ShowInputInstructions();

                var opt = GetCommandOptions();

                if (!opt.IsValid) continue;

                if (opt.IsExitCommand) break;

                try
                {
                    switch (opt.CommandType)
                    {
                        case 0://rebuild graph
                            _graph = RebuildGraph();
                            break;
                        case 1://total distance of routes from node to node(array of nodes)
                            ShowCurrentNodes(_graph);
                            break;
                        case 2://total distance of routes from node to node(array of nodes)
                            FindTotalDistance(opt, routeEngine);
                            break;
                        case 3://number of trips between two nodes
                            FindNumberOfTrips(opt, routeEngine);
                            break;
                        case 4://lenght of shortest route between two nodes
                            FindLenghtOfShortestRoute(opt, routeEngine);
                            break;
                        case 5://available routes between two nodes with given distance
                            FindAvailableRoutes(opt, routeEngine);
                            break;
                        case 9://available routes between two nodes with given distance
                            Console.Clear();

                            break;
                        default:
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private static void ShowCurrentNodes(Graph graph)
        {
            ConsoleOutputHeader();

            var sb = new StringBuilder();
            var sbEdges = new StringBuilder();
            var n = 0;
            var e = 0;
            foreach (var node in graph.Nodes)
            {
                n++;
                sb.Append(node.Value.Name + ", ");
                var nodeEdges = new StringBuilder();
                foreach (var edge in node.Value.Edges)
                {
                    e++;
                    nodeEdges.Append($"{node.Value.Name}{edge.Value.Target.Name}{edge.Value.Cost}, ");
                }
                sbEdges.Append(nodeEdges);

            }
            Console.WriteLine($"Nodes({n}):  {sb.ToString().Trim().TrimEnd(',')}");
            Console.WriteLine($"Edges({e}): {sbEdges.ToString().Trim().TrimEnd(',')}" );

            Console.ResetColor();
        }

        private static CommandOptions GetCommandOptions()
        {
            string cmdType;
            TryReadLine(false, out cmdType);

            var opt = new CommandOptions(cmdType);
            return opt;
        }

        private static Graph RebuildGraph()
        {
            string args;
            Console.WriteLine($"Enter full path to data file or hit enter to reload last data file: {_lastKnownFile}");
            TryReadLine(true, out args);

            if (args.Length <= 0) return GraphFactory.BuildFromFile(_lastKnownFile);

            var fi = new FileInfo(args);
            if (!fi.Exists) throw new ApplicationException("File does not exists");
            _lastKnownFile = fi.FullName;
            return GraphFactory.BuildFromFile(fi.FullName);
        }

        private static void FindTotalDistance(CommandOptions opt, IRouteEngine routeEngine)
        {
            string cmdArgs;
            Console.WriteLine("Enter comma delimited nodes ( eg. A,B,C,D,E ) ");
            TryReadLine(false, out cmdArgs);
            opt.SetCommandParams(cmdArgs.Split(','));

            var nodes = _graph.FindNodes(opt.GetNodeArgs());
            var route = routeEngine.BuildRouteFor(nodes);
            PrintOutput(route);
        }

        private static void FindLenghtOfShortestRoute(CommandOptions opt, RouteEngine routeEngine)
        {
            string cmdArgs;
            ShowInstructionForFromAndToNodes();

            TryReadLine(false, out cmdArgs);
            opt.SetCommandParams(cmdArgs.Split(','));

            var result = routeEngine.GetRoutes(_graph.FindNode(opt.StartArg), _graph.FindNode(opt.EndArg));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Shortest Route: {result.ShortestCost()}");
            Console.ResetColor();
        }

        private static void ShowInstructionForFromAndToNodes()
        {
            Console.WriteLine("Enter comma delimited from and to nodes ( eg. A,E ) ");
        }

        private static void FindNumberOfTrips(CommandOptions opt, IRouteEngine routeEngine)
        {
            string cmdArgs;
            ShowInstructionForFromAndToNodes();
            TryReadLine(false, out cmdArgs);
            opt.SetCommandParams(cmdArgs.Split(','));

            Console.WriteLine("Enter max depth/stops: (eg. 4 or <=4 or <4 )");
            TryReadLine(true, out cmdArgs);
            int maxDepth;
            if (int.TryParse(cmdArgs, out maxDepth))
            {
                opt.MaxDepth = maxDepth;
            }

            var predicate = opt.SetMaxDepth(cmdArgs);

            var result = routeEngine.GetRoutes(_graph.FindNode(opt.StartArg), _graph.FindNode(opt.EndArg), maxDepth: opt.MaxDepth);

            PrintOutput(result.Routes, predicate);
        }

        private static void FindAvailableRoutes(CommandOptions opt, IRouteEngine routeEngine)
        {
            string cmdArgs;
            ShowInstructionForFromAndToNodes();
            TryReadLine(false, out cmdArgs);
            opt.SetCommandParams(cmdArgs.Split(','));

            Console.WriteLine("Enter max cost/distance: (eg. 30 or <=30 or <30 )");
            TryReadLine(true, out cmdArgs);
            int maxCost;
            if (int.TryParse(cmdArgs, out maxCost))
            {
                opt.MaxCost = maxCost;
            }

            var predicate = opt.SetMaxCost(cmdArgs);

            var result = routeEngine.GetRoutes(_graph.FindNode(opt.StartArg), _graph.FindNode(opt.EndArg), maxCost: opt.MaxCost, maxDepth: null);
            PrintOutput(result.Routes, predicate);
        }

        private static void PrintOutput(IReadOnlyCollection<Route> routes, Func<Route, bool> predicate)
        {
            if (predicate != null)
            {
                var filteredRoutes = routes.Where(predicate);
                PrintOutput(filteredRoutes.ToList());
            }
            else
            {
                PrintOutput(routes);
            }
        }

        private static void TryReadLine(bool allowNull, out string cmdArgs)
        {
            while(true)
            {
                cmdArgs = Console.ReadLine();

                if (!allowNull && cmdArgs == null) return;
                return;
            }
        }

        private static void ShowInputInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("***************************************************************");
            Console.WriteLine("Enter 0 to reload graph data from file");
            Console.WriteLine("Enter 1 to display current nodes");
            Console.WriteLine("Enter 2 to calculate distance of nodes");
            Console.WriteLine("Enter 3 to get # of trips specify # stop between two nodes");
            Console.WriteLine("Enter 4 to get length of shortest route between two nodes");
            Console.WriteLine("Enter 5 to get routes with specified distance");
            Console.WriteLine("Enter 9 to clear screen");
            Console.WriteLine("***************************************************************");
            Console.WriteLine();
        }

        private static void PrintOutput(Route aRoute)
        {
            ConsoleOutputHeader();

            Console.WriteLine($"{aRoute.PathString}; Cost: {aRoute.Cost}; Stops: {aRoute.Stops}");

            Console.ResetColor();
        }

        private static void ConsoleOutputHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            //Console.WriteLine("Results:");
        }

        private static void PrintOutput(IReadOnlyCollection<Route> routes)
        {
            ConsoleOutputHeader();
            var orderedRoutes = routes.OrderBy(r => r.Cost);
            int i = 0;
            foreach (var tuple in orderedRoutes)
            {
                i++;
                Console.WriteLine($"{i}:  {tuple.PathString}; Cost: {tuple.Cost}; Stops: {tuple.Stops}");
            }
            Console.WriteLine();
            Console.WriteLine($"Paths: {routes.Count}");
            Console.ResetColor();
        }
    }
}
