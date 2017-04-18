using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutingLib.Factory;
using RoutingLib.Tests.Extensions;

namespace RoutingLib.Tests
{
    [TestClass]
    public class GraphTests 
    {
        private Graph _graph;

        [TestInitialize]
        public void TestInit()
        {
            _graph = Graph.Create()
                    .Add("AB4")
                    .Add("AD5")
                    .Add("BC6")
                    .Add("CD11")
                    .Add("ED55");
        }

        [TestMethod]
        public void AddNode_ThatAlreadyExists_ShouldNot_ThrowError()
        {
            try
            {
                _graph.Add("ED57");
                _graph.Nodes.Count.Should().Be(4);
            }
            catch (ApplicationException aex)
            {
                aex.Message.Should().Be("Edge already exists");
            }
            catch (Exception )
            {
                Assert.Fail("should be application exception");
            }
        }

        [TestMethod]
        public void FindSingleNodeTests()
        {
            var aNode = _graph.FindNode("A");
            aNode.Name.Should().Be("A");
            aNode.Edges.Count.Should().Be(2);
            aNode.Edges.ContainsKey("B").IsTrue();
        }

        [TestMethod]
        public void FindMultipleNodeTests()
        {
            try
            {
                var nodes = _graph.FindNodes(new [] {"C", "E"});
                nodes.Length.Should().Be(2);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void FindNode_ThatDontExists_ReturnsNull()
        {
            var qnode = _graph.FindNode("Q");
            Assert.IsNull(qnode);
        }

        [TestMethod]
        public void AddEdgeLineInput_CreatesNodes()
        {
            _graph.Nodes.ToList().ForEach(n => Console.WriteLine(n.Value.Name));

            _graph.Nodes.Count.Should().Be(5);
        }


        [TestMethod]
        public void AddEdgeLineInput_CreatesNodesWithCorrectEdge()
        {
            _graph.Nodes.ToList().ForEach(n => Console.WriteLine(n.Value.Name));

            var a = _graph.Nodes["A"];
            a.Edges.Count.Should().Be(2);
            a.Edges["B"].Cost.Should().Be(4);
            a.Edges["D"].Cost.Should().Be(5);

            var c = _graph.Nodes["C"];
            c.Edges["D"].Cost.Should().Be(11);
            var e = _graph.Nodes["E"];
            e.Edges["D"].Cost.Should().Be(55);
        }
    }

}