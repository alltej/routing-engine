using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests.Req
{
    [TestClass]
    public class RouteLenghtOfShortestRouteTests
        : BaseTests
    {
        //private readonly RouteEngine _routeEngine = new RouteEngine();
        private readonly IRouteEngine _routeEngine = new RouteEngineV2();

        [TestMethod]
        public void T08_LengthOf_ShortestRoute_A_to_C()
        {
            var result = _routeEngine.GetRoutes(A, C);
            PrintOutput(result.Routes);
            
            var shortestCost = result.ShortestCost();
            shortestCost.Should().Be(9);
        }

        [TestMethod]
        public void T09_LengthOf_ShortestRoute_B_to_B()
        {
            var result = _routeEngine.GetRoutes(B, B);
            PrintOutput(result.Routes);

            var shortestCost = result.ShortestCost();
            shortestCost.Should().Be(9);
        }

        [TestMethod]
        public void T10_NumberOf_DifferentRoutes_With_LessThan_AGiven_Distance()
        {
            //1. get path with shortest cost
            var maxDistance = 30;
            var result = _routeEngine.GetRoutes(C, C, maxDistance, null);

            PrintOutput(result.Routes);
            Assert.AreEqual(9, result.Routes.Count);

            var routes = result.Routes.Where(r => r.Cost < 30).ToList();
            Assert.AreEqual(7, routes.Count);
        }
    }

}