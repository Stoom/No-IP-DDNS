using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoIP.DDNS.Test
{
    [TestClass]
    public class WebClientTest
    {
        [TestMethod]
        public void SetProgrameUserAgentAndReturnFullUserAgent()
        {
            var progameUserAgent = "DDnsTest";
            var client = new Client(progameUserAgent);

            var expectedResults = "DDNS/1.0 (Windows NT 6.3; WOW64) DDnaTest/1.0";
            Assert.AreEqual(expectedResults, client.UserAgent);
        }
    }
}
