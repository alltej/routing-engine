namespace RoutingLib.Engine
{
    public interface IRouteEngine
    {
        Route BuildRouteFor(params Node[] nodes);
        RouteResult GetRoutes(Node from, Node to, int? maxCost = default(int?), int? maxDepth = default(int?));
        RouteResult GetRoutesWithMaxDepth(Node from, Node to, int maxDepth);
        RouteResult GetRoutesWithMaxCost(Node from, Node to, int maxCost);
    }
}