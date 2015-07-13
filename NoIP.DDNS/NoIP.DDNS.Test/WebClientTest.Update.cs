﻿using System.Collections.Generic;
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
                ShimWebClient.AllInstances.DownloadStringString = (client, s) => "host1:1";

                var address = IPAddress.Parse("127.0.0.1");

                var hosts = new HashSet<Host>
                {
                    new Host("Host1") { Address = address },
                    new Host("Host2") { Address = address }
                };

                AssertExtensions.NoExpectedException<UpdateException>(() => _client.UpdateHost(hosts));
            }
        }
    }
}
