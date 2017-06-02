using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests.Req
{
    [TestClass]
    public class RouteNumberOfTripsTests : BaseTests
    {
        //private readonly RouteEngine _routeEngine = new RouteEngine();
        private readonly IRouteEngine _routeEngine = new RouteEngineV2();


        [TestMethod]
        public void T06_NumberOfTrips_With_MaximumStops()
        {
            var result = _routeEngine.GetRoutes(C, C, maxDepth: 3);

            PrintOutput(result.Routes);

            result.Routes.Count.Should().Be(2);
        }


        [TestMethod]
        public void T07_NumberOfTrips_With_ExactNumberOfStops()
        {
            var result = _routeEngine.GetRoutes(A, C, maxDepth: 4);

            var routesW4Stops = result.Routes.Where(p => p.Stops == 4).ToList();

            PrintOutput(routesW4Stops);

            Assert.AreEqual(3, routesW4Stops.Count);
        }

    }

}