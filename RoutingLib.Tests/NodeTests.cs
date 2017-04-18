using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests
{
    [TestClass]
    public class NodeTests : BaseTests
    {
        private readonly RouteEngine _routeEngine = new RouteEngine();

        [TestMethod]
        public void Node_EdgeTarget_Cannot_Appear_MoreThan_Once()
        {
            try
            {
                var q = Node.Create("Q");
                var r = Node.Create("R");
                q.AddEdge(Edge.Create(r, 1));
                q.AddEdge(Edge.Create(r, 1));
                Assert.Fail("should throw exception");
            }
            catch (ApplicationException aex)
            {
                aex.Message.Should().Be("Edge already exists");
            }
        }

        [TestMethod]
        public void Node_EdgeTarget_Cannot_BeSame_Node()
        {
            try
            {
                var q = Node.Create("Q");
                q.AddEdge(Edge.Create(q, 1));
                Assert.Fail("should throw exception");
            }
            catch (ApplicationException aex)
            {
                aex.Message.Should().Be("Edge target cannot be the same as the origin node");
            }
        }
    }

}