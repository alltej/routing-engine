using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests
{
    [TestClass]
    public class CodingAssignmentTests : BaseTests
    {
        //private readonly RouteEngine _routeEngine = new RouteEngine();
        private readonly IRouteEngine _routeEngine = new RouteEngineV2();

        [TestMethod]
        public void T01_DistanceOfRoute_ABC()
        {
            var route = _routeEngine.BuildRouteFor(A, B, C);
            route.Cost.Should().Be(9);
            route.Stops.Should().Be(2);
        }
        [TestMethod]
        public void T02_DistanceOfRoute_Given_TwoNodes()
        {
            var route = _routeEngine.BuildRouteFor(A, D);
            route.Cost.Should().Be(5);
            route.Stops.Should().Be(1);
        }

        [TestMethod]
        public void T03_DistanceOfRoute_ADC()
        {
            var route = _routeEngine.BuildRouteFor(A, D, C);
            route.Cost.Should().Be(13);
            route.Stops.Should().Be(2);
        }

        [TestMethod]
        public void T04_DistanceOfRoute_Given_MultipleNodes()
        {
            var route = _routeEngine.BuildRouteFor(A, E, B, C, D);
            route.Cost.Should().Be(22);
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

        private static void PrintOutput(IReadOnlyCollection<Route> pathOutput)
        {
            Console.WriteLine($"# Paths: {pathOutput.Count}");
            foreach (var tuple in pathOutput)
            {
                Console.WriteLine($"{tuple.PathString}; Cost: {tuple.Cost}; Stops: {tuple.Stops}");
            }
        }
    }

}