using System;
using System.Collections.Generic;
using System.Linq;
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
            var path = new PathV1(nodes);

            var route = Route.CreateFromPath(path);
            route.Stops.Should().Be(2);
            route.Cost.Should().Be(9);
            route.PathString.Should().Be("ABC");
        }

        [TestMethod]
        public void CreateFromTupleTests()
        {
            var nodes = new List<Node> {A, B, C};

            var path = new List<Tuple<string, int>>();
            var previousNode = nodes.First();
            path.Add(Tuple.Create(previousNode.Name, 0));

            var nodesSkip1 = nodes.Skip(1);

            foreach (var node in nodesSkip1)
            {
                //previousNode.Edges.FirstOrDefault(n=>n.Value.Target == node.Name)
                var edge = previousNode.Edges[node.Name];
                if (edge == null) throw new ApplicationException("NO SUCH ROUTE");
                path.Add(Tuple.Create(edge.Target.Name, edge.Cost));
                previousNode = node;
            }

            var route = Route.CreateFromTuple(path);
            route.Stops.Should().Be(2);
            route.Cost.Should().Be(9);
            route.PathString.Should().Be("ABC");
        }
    }

}