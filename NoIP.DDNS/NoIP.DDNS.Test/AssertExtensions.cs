using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoIP.DDNS.Test
{
    internal static class AssertExtensions
    {
        public static T ExpectedException<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                return ex;
            }

            Assert.Fail("Expected exception of type {0}", typeof(T));
            return null;
        }

        public static T NoExpectedException<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                Assert.Fail("No exception expected but exception was thrown of type {0}", typeof(T));
            }

            return null;
        }
    }
}
