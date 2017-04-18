using System.Collections.Generic;
using System.Linq;

namespace RoutingLib
{
    public class RouteResult
    {
        public RouteResult(IEnumerable<Route> routes)
        {
            Routes = routes.OrderBy(r => r.Cost).ToList();
        }

        public List<Route> Routes { get; }

        public int ShortestCost()
        {
            return Routes.Min(cost => cost.Cost);
        }
    }
}