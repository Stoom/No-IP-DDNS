using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Fakes;
using ARSoft.Tools.Net.Dns;
using ARSoft.Tools.Net.Dns.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoIP.DDNS.DTO;

namespace NoIP.DDNS.Test
{
    public partial class WebClientTest
    {

        [TestMethod]
        public void ReturnAllHostsForGivenZone()
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
		<host name=""Host2"" group="""" wildcard=""true"" ></host>
	</domain>
</noip_host_list>
";

                var zones = _client.GetZones();
                var results = _client.GetHosts(zones.First());

                var expectedResults = new HashSet<Host>
                {
                    new Host("Host1"),
                    new Host("Host2") { Wildcard = true }
                };

                Assert.IsNotNull(results);
                Assert.IsTrue(expectedResults.SequenceEqual(results));
            }
        }

        [TestMethod]
        public void ReturnHostForGivenZoneAndLocallyResolveIpAddress()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;
            Assert.IsTrue(_client.IsRegistered);

            const string shimAddress = "127.0.0.1";

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) =>
@"
<?xml version=""1.0"" ?>
<noip_host_list email=""fakeEmail"" enhanced=""false"" webserver="""">
	<domain name=""NoIPDDNS"" type=""plus"">
		<host name=""Host1"" group="""" wildcard=""false"" ></host>
		<host name=""Host2"" group="""" wildcard=""true"" ></host>
	</domain>
</noip_host_list>
";
                ShimDnsClient.AllInstances.ResolveStringRecordTypeRecordClass = (dnsClient, s, arg3, arg4) =>
                {
                    switch (s)
                    {
                        case "Host1":
                            return new DnsMessage
                            {
                                IsEDnsEnabled = true,
                                IsRecursionAllowed = true,
                                IsRecursionDesired = true,
                                ReturnCode = ReturnCode.NoError,
                                AnswerRecords = new List<DnsRecordBase>
                                {
                                    new ARecord("Host1", 60, IPAddress.Parse(shimAddress))
                                }
                            };
                        case "Host2":
                            return new DnsMessage
                            {
                                IsEDnsEnabled = true,
                                IsRecursionAllowed = true,
                                IsRecursionDesired = true,
                                ReturnCode = ReturnCode.NoError,
                                AnswerRecords = new List<DnsRecordBase>
                                {
                                    new ARecord("Host1", 60, IPAddress.Parse(shimAddress))
                                }
                            };
                        default:
                            return null;
                    }
                };

                _client.ResolveDns = DnsResolveMode.Local;
                var zones = _client.GetZones();
                var results = _client.GetHosts(zones.First());

                var expectedResults = new HashSet<Host>
                {
                    new Host("Host1") {Address = IPAddress.Parse(shimAddress)},
                    new Host("Host2") {Address = IPAddress.Parse(shimAddress), Wildcard = true}
                };

                Assert.IsNotNull(results);
                Assert.IsTrue(expectedResults.SequenceEqual(results));
            }
        }
    }
}
