namespace RoutingLib
{
    public class Edge
    {
        public static Edge Create(Node target, int cost)
        {
            return new Edge(target, cost);
        }
        public Edge(Node target, int cost)
        {
            Cost = cost;
            Target = target;
        }

        public int Cost { get;  }
        public Node Target { get;  }
    }
}