using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
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
            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) =>
@"
<?xml version=""1.0""?>
<client>
	<id>C3A16882XXX</id>
	<key>2dc43c0d076447fccc7c4999eTESTKEY</key>
</client>
";
                _client.Register(_noipUsername, _noipPassword);
                Assert.IsTrue(_client.IsRegistered);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidLoginException))]
        public void RegisterClientWithBadUserAndPasswordAndReturnException()
        {
            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) =>
@"
<?xml version=""1.0""?>
<error>incorrect password</error>
";
                _client.Register("BadUser", "BadPassword");
            }
        }
    }
}
