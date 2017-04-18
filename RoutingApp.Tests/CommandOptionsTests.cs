using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoutingApp.Tests
{
    [TestClass]
    public class CommandOptionsTests
    {
        [TestMethod]
        public void CommandOptionsArg_ValidCommands_Tests()
        {
            foreach (var cmd in Commands.ValidCommands)
            {
                var opt = new CommandOptions(cmd);
                Assert.IsTrue(opt.IsValid);
            }
        }

        [TestMethod]
        public void CommandOptions_InvalidCommands_Tests()
        {
            var c1 = new CommandOptions("Q");
            Assert.IsFalse(c1.IsValid);

            var c2 = new CommandOptions("12");
            Assert.IsFalse(c2.IsValid);
        }

        [TestMethod]
        public void SetMaxCostTest()
        {
            var opt = new CommandOptions("4")
            {
                StartArg = "A",
                EndArg = "E"
            };

            opt.SetMaxCost("<=30");

            Assert.AreEqual(30, opt.MaxCost);
        }

        [TestMethod]
        public void SetMaxDepthTest()
        {
            var opt = new CommandOptions("4")
            {
                StartArg = "A",
                EndArg = "E"
            };

            opt.SetMaxDepth("<=4");

            Assert.AreEqual(4, opt.MaxDepth);
        }

        [TestMethod]
        public void IsExitCommandTest()
        {
            var opt = new CommandOptions("exit");

            Assert.AreEqual(true, opt.IsExitCommand);
        }
    }
}
