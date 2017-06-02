using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;

namespace RoutingLib.Tests.Req
{
    [TestClass]
    public class RouteNumberOfDifferentRoutesTests : BaseTests
    {
        //private readonly RouteEngine _routeEngine = new RouteEngine();
        private readonly IRouteEngine _routeEngine = new RouteEngineV2();


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