using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoIP.DDNS.Test
{
    [TestClass]
    public class WebClientTest
    {
        private UserAgent _ua = new UserAgent("DDnsTest");

        [TestMethod]
        public void CheckIfRegisteredAndReturnFalse()
        {
            var client = new Client(_ua);
            Assert.IsFalse(client.IsRegistered);
        }

        [TestMethod]
        public void CreateClientWithCredCheckIfRegisteredAndReturnTrue()
        {
            var client = new Client(_ua)
                {
                    Id = "TestId",
                    Key = "TestKey"
                };
            Assert.IsTrue(client.IsRegistered);
        }
    }
}
