using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests.PerfTests.V1
{
    [TestClass]
    public class GetRoutesBetweenTests : PerfBaseTests
    {
        private readonly IRouteEngine _routeEngine = new RouteEngine();


        [TestMethod]
        public void GetRoutesBetween_With_MaxDepth_And_MaxCost_A()
        {
            var watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutes(A, P, 60, 13);
            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);

            result.NumberOfRoutes.Should().Be(58);
            result.ShortestCost().Should().Be(38);
            result.ShortestDepth().Should().Be(8);
        }

        [TestMethod]
        public void GetRoutesBetween_With_MaxDepth_And_MaxCost_B()
        {
            var watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutes(A, P, 58, 13);
            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);

            result.NumberOfRoutes.Should().Be(42);
            result.ShortestCost().Should().Be(38);
            result.ShortestDepth().Should().Be(8);
        }

        [TestMethod]
        public void GetRoutesBetween_With_MaxCost_A()
        {
            var watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutes(A, P,60);
            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);
        }


        [TestMethod]
        public void GetRoutesBetween_With_MaxCost_B()
        {
            var watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutes(A, P,70);
            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);
        }


        [TestMethod]
        public void GetRoutesBetween_With_MaxDepth()
        {
            var watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutesWithMaxDepth(A, P,13);
            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);
        }


        [TestMethod]
        public void GetRoutesBetween_NoMaxCost_NoMaxDepth_Specified()
        {
            var watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutes(A, P);
            watch.Stop();

            var elapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);
        }

        private static void PrintOutput(IReadOnlyCollection<Route> pathOutput)
        {
            var list = pathOutput.OrderBy(route => route.Cost).ToList();
            Console.WriteLine($"# Paths: {list.Count}");
            var i = 1;
            foreach (var tuple in list)
            {
                Console.WriteLine($"{i++}: {tuple.PathString}; Cost/Distance: {tuple.Cost}; Depth/Stops: {tuple.Stops}");
            }
        }
    }

}