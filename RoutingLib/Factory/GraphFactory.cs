using System.IO;

namespace RoutingLib.Factory
{
    public static class GraphFactory
    {
        public static Graph BuildFromFile(string fileDataPath = "")
        {
            var file = string.IsNullOrEmpty(fileDataPath) ? "RoutingData.txt" : fileDataPath;
            var lines = File.ReadAllLines(file);

            var graph = new Graph();
            foreach (var line in lines)
            {
                var input = line.Trim();
                if (input.Length >= 3)
                {
                    graph.AddEdge(input);
                }
            }

            return graph;
        }

        public static Graph Add(this Graph graph, string lineInput)
        {
            graph.AddEdge(lineInput);
            return graph;
        }
    }
}