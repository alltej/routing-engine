using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoutingLib.Tests.PerfTests
{
    [TestClass]
    public abstract class PerfBaseTests
    {
        public Node A;
        public Node B;
        public Node C;
        public Node D;
        public Node E;
        public Node F;
        public Node G;
        public Node H;
        public Node I;
        public Node J;
        public Node K;
        public Node L;
        public Node M;
        public Node N;
        public Node O;
        public Node P;

        [TestInitialize]
        public void TestInit()
        {
            A = Node.Create("A");
            B = Node.Create("B");
            C = Node.Create("C");
            D = Node.Create("D");
            E = Node.Create("E");


            F = Node.Create("F");
            G = Node.Create("G");
            H = Node.Create("H");
            I = Node.Create("I");
            J = Node.Create("J");
            K = Node.Create("K");
            L = Node.Create("L");
            M = Node.Create("M");
            N = Node.Create("N");
            O = Node.Create("O");
            P = Node.Create("P");

            A.AddEdge(Edge.Create(B, 5));
            B.AddEdge(Edge.Create(C, 4));
            C.AddEdge(Edge.Create(D, 8));
            D.AddEdge(Edge.Create(C, 8));
            D.AddEdge(Edge.Create(E, 6));
            A.AddEdge(Edge.Create(D, 5));
            C.AddEdge(Edge.Create(E, 2));
            E.AddEdge(Edge.Create(B, 3));
            A.AddEdge(Edge.Create(E, 7));



            D.AddEdge(Edge.Create(H, 3));
            A.AddEdge(Edge.Create(F, 3));
            A.AddEdge(Edge.Create(G, 5));
            F.AddEdge(Edge.Create(G, 5));
            G.AddEdge(Edge.Create(H, 5));
            H.AddEdge(Edge.Create(I, 5));
            H.AddEdge(Edge.Create(G, 5));
            I.AddEdge(Edge.Create(A, 5));
            I.AddEdge(Edge.Create(J, 5));
            J.AddEdge(Edge.Create(K, 5));
            J.AddEdge(Edge.Create(L, 5));
            J.AddEdge(Edge.Create(M, 5));
            K.AddEdge(Edge.Create(L, 5));
            L.AddEdge(Edge.Create(M, 5));
            M.AddEdge(Edge.Create(J, 5));
            M.AddEdge(Edge.Create(N, 5));
            N.AddEdge(Edge.Create(O, 5));
            O.AddEdge(Edge.Create(P, 5));
            P.AddEdge(Edge.Create(K, 5));
            P.AddEdge(Edge.Create(N, 5));

        }
    }
}