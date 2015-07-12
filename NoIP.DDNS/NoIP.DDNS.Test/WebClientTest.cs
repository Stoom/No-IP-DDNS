using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoIP.DDNS.Exceptions;

namespace NoIP.DDNS.Test
{
    [TestClass]
    public partial class WebClientTest
    {
        private UserAgent _ua = new UserAgent("DDnsTest");
        private Client _client;

        [TestInitialize]
        public void InitTest()
        {
            _client = new Client(_ua);
        }

        [TestMethod]
        public void CheckIfRegisteredAndReturnFalse()
        {
            Assert.IsFalse(_client.IsRegistered);
        }

        [TestMethod]
        public void CreateClientWithCredCheckIfRegisteredAndReturnTrue()
        {
            _client.Id = "TestId";
            _client.Key = "TestKey";

            Assert.IsTrue(_client.IsRegistered);
        }

        [TestMethod]
        public void RegisterClient()
        {
            _client.Register(_noipUsername, _noipPassword);
            Assert.IsTrue(_client.IsRegistered);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidLoginException))]
        public void RegisterClientWithBadUserAndPasswordAndReturnException()
        {
            _client.Register("BadUser", "BadPassword");
        }
    }
}
