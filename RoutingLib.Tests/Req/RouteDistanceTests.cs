using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests.Req
{
    [TestClass]
    public class RouteDistanceTests : BaseTests
    {
        //private readonly RouteEngine _routeEngine = new RouteEngine();
        private readonly IRouteEngine _routeEngine = new RouteEngineV2();

        [TestMethod]
        public void T01_DistanceOfRoute_ABC()
        {
            var route = _routeEngine.BuildRouteFor(A, B, C);
            route.Cost.Should().Be(27);
            route.Stops.Should().Be(2);
        }
        [TestMethod]
        public void T02_DistanceOfRoute_Given_TwoNodes()
        {
            var route = _routeEngine.BuildRouteFor(A, D);
            route.Cost.Should().Be(15);
            route.Stops.Should().Be(1);
        }

        [TestMethod]
        public void T03_DistanceOfRoute_ADC()
        {
            var route = _routeEngine.BuildRouteFor(A, D, C);
            route.Cost.Should().Be(39);
            route.Stops.Should().Be(2);
        }

        [TestMethod]
        public void T04_DistanceOfRoute_Given_MultipleNodes()
        {
            var route = _routeEngine.BuildRouteFor(A, E, B, C, D);
            route.Cost.Should().Be(66);
            route.Stops.Should().Be(4);
        }

        [TestMethod]
        public void T05_NoSuchRouteExists()
        {
            try
            {
                _routeEngine.BuildRouteFor(A, E, D);
                Assert.Fail("should throw exception");
            }
            catch (ApplicationException aex)
            {
                Assert.IsTrue(true);
                aex.Message.Should().Be("NO SUCH ROUTE");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


    }

}