namespace RoutingLib.Engine
{
    public interface IRouteEngine
    {
        Route BuildRouteFor(params Node[] nodes);
        RouteResult GetRoutesBetween(Node @from, Node to, int? maxDepth = default(int?), int? maxDistance = default(int?));
    }
}