﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoutingLib.Tests
{
    [TestClass]
    public abstract class BaseTests
    {
        public Node A;
        public Node B;
        public Node C;
        public Node D;
        public Node E;

        [TestInitialize]
        public void TestInit()
        {
            A = Node.Create("A");
            B = Node.Create("B");
            C = Node.Create("C");
            D = Node.Create("D");
            E = Node.Create("E");

            A.AddEdge(Edge.Create(B, 5));
            B.AddEdge(Edge.Create(C, 4));
            C.AddEdge(Edge.Create(D, 8));
            D.AddEdge(Edge.Create(C, 8));
            D.AddEdge(Edge.Create(E, 6));
            A.AddEdge(Edge.Create(D, 5));
            C.AddEdge(Edge.Create(E, 2));
            E.AddEdge(Edge.Create(B, 3));
            A.AddEdge(Edge.Create(E, 7));
        }
    }
}