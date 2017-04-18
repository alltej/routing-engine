namespace RoutingLib
{
    public class Route
    {
        public Route(string pathString, int stops, int cost)
        {
            PathString = pathString;
            Stops = stops;
            Cost = cost;
        }

        public int Cost { get;  }

        public string PathString { get; }

        public int Stops { get; }

        public static Route Create(string pathString, int stops, int cost)
        {
            return new Route(pathString, stops, cost);
        }
    }
}