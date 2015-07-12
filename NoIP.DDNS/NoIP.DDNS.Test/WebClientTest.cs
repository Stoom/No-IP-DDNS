using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoIP.DDNS.DTO;
using NoIP.DDNS.Exceptions;
using NoIP.DDNS.Response;

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

        [TestMethod]
        public void ReturnAllZones()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) =>
@"
<?xml version=""1.0"" ?>
<noip_host_list email=""fakeEmail"" enhanced=""false"" webserver="""">
	<domain name=""NoIPDDNS"" type=""plus"">
		<host name=""stoom"" group="""" wildcard=""false"" ></host>
	</domain>
</noip_host_list>
";

                var results = _client.GetZones() as HashSet<Zone>;

                var expectedResults = new HashSet<Zone>
                {
                    new Zone("NoIPDDNS", ZoneType.Plus)
                };

                Assert.IsNotNull(results);
                Assert.IsTrue(expectedResults.SequenceEqual(results));
            }
        }
    }
}
