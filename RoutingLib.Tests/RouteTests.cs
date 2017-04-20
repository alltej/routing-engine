using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests
{
    [TestClass]
    public class RouteTests :BaseTests
    {

        [TestMethod]
        public void CreateFromPathTests()
        {
            var nodes = new List<Node> {A, B, C};
            var path = new Path(nodes);

            var route = Route.CreateFromPath(path);
            route.Stops.Should().Be(2);
            route.Cost.Should().Be(9);
            route.PathString.Should().Be("ABC");

        }
    }

}