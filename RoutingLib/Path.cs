using System.Collections.Generic;

namespace RoutingLib
{
    public class Path
    {
        public Path(List<Node> nodeList)
        {
            Nodes = nodeList;
        }

        public List<Node> Nodes { get; private set; }
    }
}