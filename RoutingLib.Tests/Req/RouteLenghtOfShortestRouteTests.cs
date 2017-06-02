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
            shortestCost.Should().Be(27);
        }

        [TestMethod]
        public void T09_LengthOf_ShortestRoute_B_to_B()
        {
            var result = _routeEngine.GetRoutes(B, B);
            PrintOutput(result.Routes);

            var shortestCost = result.ShortestCost();
            shortestCost.Should().Be(27);
        }


    }

}