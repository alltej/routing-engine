using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;

namespace RoutingLib.Tests.PerfTests
{
    [TestClass]
    public class GetRoutesBetweenTests : PerfBaseTests
    {
        private readonly RouteEngine _routeEngine = new RouteEngine();


        [TestMethod]
        public void GetRoutesBetween_With_MaxDistance()
        {
            Stopwatch watch = Stopwatch.StartNew();
            var result = _routeEngine.GetRoutesBetween(A, P, 10,100);
            watch.Stop();

            long elapsed = watch.ElapsedMilliseconds;
            // Microseconds
            //int microSeconds = (int)(watch.ElapsedTicks * 1.0e6 / Stopwatch.Frequency + 0.4999);
            // Nanoseconds (estimation)
            //int nanoSeconds = (int)(watch.ElapsedTicks * 1.0e9 / Stopwatch.Frequency + 0.4999);
            Console.WriteLine($"ElapsedTime: {elapsed}");

            PrintOutput(result.Routes);
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