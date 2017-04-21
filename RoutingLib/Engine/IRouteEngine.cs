namespace RoutingLib.Engine
{
    public interface IRouteEngine
    {
        Route BuildRouteFor(params Node[] nodes);
        RouteResult GetRoutesV1(Node from, Node to, int? maxCost = default(int?), int? maxDepth = default(int?));
        RouteResult GetRoutes(Node from, Node to, int? maxCost = default(int?), int? maxDepth = default(int?));
        RouteResult GetRoutesWithMaxDepthV1(Node from, Node to, int maxDepth);
        RouteResult GetRoutesWithMaxDepth(Node from, Node to, int maxDepth);
        RouteResult GetRoutesWithMaxCost(Node from, Node to, int maxCost);
    }
}