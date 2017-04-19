using System.Collections.Generic;

namespace RoutingLib
{
    public class Path
    {
        public Path(IReadOnlyList<Node> nodeList)
        {
            Nodes = nodeList;
        }

        public IReadOnlyList<Node> Nodes { get; private set; }
    }
}