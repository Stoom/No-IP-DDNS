using System.Collections.Generic;
using System.Linq;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoIP.DDNS.DTO;
using NoIP.DDNS.Exceptions;

namespace NoIP.DDNS.Test
{
    public partial class WebClientTest
    {

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
		<host name=""Host1"" group="""" wildcard=""false"" ></host>
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

        [TestMethod]
        [ExpectedException(typeof(InvalidLoginException))]
        public void ReturnAllZonesWithBadPasswordAndThrowException()
        {
            _client.Id = _noipClientId;
            _client.Key = "BadPass";

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => @"bad password";

                var results = _client.GetZones() as HashSet<Zone>;

                var expectedResults = new HashSet<Zone>
                {
                    new Zone("NoIPDDNS", ZoneType.Plus)
                };

                Assert.IsNotNull(results);
                Assert.IsTrue(expectedResults.SequenceEqual(results));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidLoginException))]
        public void ReturnAllZonesWithBadUserAndThrowException()
        {
            _client.Id = "BadUser";
            _client.Key = "BadPass";

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => @"bad password";

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
