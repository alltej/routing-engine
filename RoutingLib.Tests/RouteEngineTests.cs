using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Engine;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests
{
    [TestClass]
    public class RouteEngineTests : BaseTests
    {

        [TestMethod]
        public void RouteEngine_FromNode_IsNull_ThrowsException()
        {
            try
            {
                var engine = new RouteEngine();
                engine.GetRoutesBetween(null, Node.Create("A"));
            }
            catch (ArgumentException aex)
            {
                aex.Message.Should().Be("from node cannot be null");
            }
            catch (Exception ex)
            {
                Assert.Fail("should be an applicaiton exception. " + ex.Message);
            }
        }

        [TestMethod]
        public void RouteEngine_ToNode_IsNull_ThrowsException()
        {
            try
            {
                var engine = new RouteEngine();
                engine.GetRoutesBetween(Node.Create("A") , null);
            }
            catch (ArgumentException aex)
            {
                aex.Message.Should().Be("to node cannot be null");
            }
            catch (Exception ex)
            {
                Assert.Fail("should be an applicaiton exception. " + ex.Message);
            }

        }
        
    }

}