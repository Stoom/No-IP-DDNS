using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public void UpdateSingleHostIpAddressSuccessfully()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => "host1:1";

                var host = new Host("Host1") {Address = IPAddress.Parse("127.0.0.1")};

                AssertExtensions.NoExpectedException<UpdateException>(() => _client.UpdateHost(host));
            }
        }

        [TestMethod]
        public void UpdateMultipleHostsWithASingleIpAddressSuccessfully()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => "host1:1\nhost2:0";

                var address = IPAddress.Parse("127.0.0.1");

                var hosts = new List<Host>
                {
                    new Host("Host1") { Address = address },
                    new Host("Host2") { Address = address }
                };

                AssertExtensions.NoExpectedException<UpdateException>(() => _client.UpdateHost(hosts));
            }
        }

        [TestMethod]
        public void UpdateMultipleHostsWithDifferentIpAddressSuccessfully()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                var i = -1;
                ShimWebClient.AllInstances.DownloadStringString = (client, s) =>
                {
                    i = (i + 1)%3;
                    var results = new [] { "host1:1\nhost2:0","host3:1\nhost4:0","host5:1" };
                    return results[i];
                };

                var hosts = new List<Host>
                {
                    new Host("Host1") { Address = IPAddress.Parse("127.0.0.1") },
                    new Host("Host2") { Address = IPAddress.Parse("127.0.0.1") },
                    new Host("Host3") { Address = IPAddress.Parse("127.0.0.2") },
                    new Host("Host4") { Address = IPAddress.Parse("127.0.0.2") },
                    new Host("Host5") { Address = IPAddress.Parse("127.0.0.3") },
                };

                AssertExtensions.NoExpectedException<UpdateException>(() => _client.UpdateHost(hosts));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidLoginException))]
        public void UpdateMultipleHostsWithBadLoginAndThrowInvalidLoginException()
        {
            _client.Id = "BadUser";
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => ":4";

                var hosts = new List<Host>
                {
                    new Host("host1") { Address = IPAddress.Parse("127.0.0.1") },
                    new Host("host2") { Address = IPAddress.Parse("127.0.0.1") },
                };
                _client.UpdateHost(hosts);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void UpdateMultipleHostsWithTempDisabledClientIdAndThrowAuthenticationException()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => ":98";

                var hosts = new List<Host>
                {
                    new Host("host1") { Address = IPAddress.Parse("127.0.0.1") },
                    new Host("host2") { Address = IPAddress.Parse("127.0.0.1") },
                };
                _client.UpdateHost(hosts);
            }
        }

        [TestMethod]
        public void UpdateMultipleNonExistingHostsAndThrowUpdateException()
        {
            _client.Id = _noipClientId;
            _client.Key = _noipClientKey;

            Assert.IsTrue(_client.IsRegistered);

            using (ShimsContext.Create())
            {
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => "host1:2\nhost2:2\n";

                var hosts = new List<Host>
                {
                    new Host("host1") { Address = IPAddress.Parse("127.0.0.1") },
                    new Host("host2") { Address = IPAddress.Parse("127.0.0.1") },
                };

                var ex = AssertExtensions.ExpectedException<UpdateException>(() => _client.UpdateHost(hosts));

                var expectedResults = new Dictionary<string, UpdateStatus>
                {
                    {"host1", UpdateStatus.HostNameDoesNotExist},
                    {"host2", UpdateStatus.HostNameDoesNotExist}
                };

                Assert.IsTrue(expectedResults.SequenceEqual(ex.HostStatus));
            }
        }
    }
}
