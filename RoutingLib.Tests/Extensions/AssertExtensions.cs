using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoutingLib.Tests.Extensions
{
    public static class IntExtension
    {
        public static int Should(this int anInt)
        {
            return anInt;
        }

        public static void Be(this int anInt, int actual)
        {
            Assert.AreEqual(anInt, actual);
        }
    }
    public static class StringExtension
    {
        public static string Should(this string anInt)
        {
            return anInt;
        }

        public static void Be(this string anInt, string actual)
        {
            Assert.AreEqual(anInt, actual);
        }
    }
    public static class BoolExtension
    {
        public static bool Should(this bool aBool)
        {
            return aBool;
        }

        public static void IsTrue(this bool expected)
        {
            Assert.AreEqual(expected, true);
        }
        public static void IsFalse(this bool expected)
        {
            Assert.AreEqual(expected, false);
        }
    }
}