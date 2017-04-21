using System.Collections.Generic;
using System.Linq;

namespace RoutingLib
{
    public class RouteResult
    {
        public RouteResult(IEnumerable<Route> routes)
        {
            //Routes = routes.OrderBy(r => r.Cost).ToList();
            Routes = routes.ToList();
            NumberOfRoutes = Routes.Count;
        }

        public int NumberOfRoutes { get;}

        public IReadOnlyList<Route> Routes { get; }

        public int ShortestCost()
        {
            return Routes.Min(route => route.Cost);
        }
        public int ShortestDepth()
        {
            return Routes.Min(route => route.Stops);
        }
    }
}