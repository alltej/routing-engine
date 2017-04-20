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
                engine.GetRoutes(null, Node.Create("A"));
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
                engine.GetRoutes(Node.Create("A") , null);
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

        [TestMethod]
        public void TargetDepthCalculatorTest_WithNullMaxDepth_ShouldBe_CalcDepth()
        {
            var depth = RouteEngine.CalculateTargetDepth(null, 60, 38, 13);
            depth.Should().Be(13);
        }

        [TestMethod]
        public void TargetDepthCalculatorTest_WithMaxDepth_LT_CalcDepth()
        {
            var depth = RouteEngine.CalculateTargetDepth(10, 60, 38, 13);
            depth.Should().Be(10);
        }

        [TestMethod]
        public void TargetDepthCalculatorTest_WithMaxDepth_GT_CalcDepth()
        {
            var depth = RouteEngine.CalculateTargetDepth(15, 60, 38, 13);
            depth.Should().Be(13);
        }
        
    }

}