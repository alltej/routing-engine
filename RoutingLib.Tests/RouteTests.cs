using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var path = new PathV1(nodes);
            var route = Route.CreateFromPath(path);
            route.Stops.Should().Be(2);
            route.Cost.Should().Be(27);
            route.PathString.Should().Be("ABC");
        }

        [TestMethod]
        public void CreateFromTupleTests()
        {
            var nodes = new List<Node> {A, B, C};
            var list = Route.GetTupleList(nodes);
            var route = Route.CreateFromTuples(list);
            route.Stops.Should().Be(2);
            route.Cost.Should().Be(27);
            route.PathString.Should().Be("ABC");
        }

        [TestMethod]
        public void CreateFromNodesTests()
        {
            var nodes = new List<Node> {A, B, C};

            var route = Route.CreateFromNodes(nodes);
            route.Stops.Should().Be(2);
            route.Cost.Should().Be(27);
            route.PathString.Should().Be("ABC");
        }


    }

}