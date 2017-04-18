using System.IO;
using RoutingLib;

namespace RoutingApp
{
    public class GraphFactory
    {
        public static Graph BuildGraph(string fileDataPath)
        {
            var lines = File.ReadAllLines(string.IsNullOrEmpty(fileDataPath) ? "RoutingData.txt" : fileDataPath);

            var graph = new Graph();
            foreach (var line in lines)
            {
                var input = line.Trim();
                if (input.Length >= 3)
                {
                    graph.Add(input);
                }
            }

            return graph;
        }
    }
}