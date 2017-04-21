using System;
using System.Collections.Generic;

namespace RoutingLib
{
    public class PathV1
    {
        public PathV1(IReadOnlyList<Node> nodeList)
        {
            Nodes = nodeList;
        }

        public IReadOnlyList<Node> Nodes { get; private set; }
    }

    public class PathV2
    {
        public PathV2(List<Tuple<string, int>> nodeList)
        {
            Paths = nodeList;
        }

        public List<Tuple<string, int>> Paths { get; set; }
    }
}