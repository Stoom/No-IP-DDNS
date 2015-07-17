using System.Collections.Generic;
using System.Net;
using ARSoft.Tools.Net.Dns;
using ARSoft.Tools.Net.Dns.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoIP.DDNS.Test
{
    [TestClass]
    public class DnsResolverTest
    {
        [TestMethod]
        public void ResolveKnownGoodHostFromDefaultLocalDnsServerAndReturnIpAddress()
        {
            const string host = "host1";
            const string shimAddress = "127.0.0.1";

            using (ShimsContext.Create())
            {
                ShimDnsClient.AllInstances.ResolveStringRecordTypeRecordClass = (dnsClient, s, arg3, arg4) => new DnsMessage
                {
                    IsEDnsEnabled = true,
                    IsRecursionAllowed = true,
                    IsRecursionDesired = true,
                    ReturnCode = ReturnCode.NoError,
                    AnswerRecords = new List<DnsRecordBase> 
                    {
                        new ARecord(host, 60, IPAddress.Parse(shimAddress))
                    }
                };

                var client = new DnsResolver();
                var address = client.Resolve(host);

                var expectedResults = IPAddress.Parse(shimAddress);

                Assert.AreEqual(expectedResults, address);
            }
        }

        [TestMethod]
        public void ResolveKnownGoodHostFromRemoteDnsServerAndReturnIpAddress()
        {
            const string host = "host1";
            const string shimAddress = "127.0.0.1";
            const string remoteDnsServer = "8.8.8.8";

            using (ShimsContext.Create())
            {
                ShimDnsClient.AllInstances.ResolveStringRecordTypeRecordClass = (dnsClient, s, arg3, arg4) => new DnsMessage
                {
                    IsEDnsEnabled = true,
                    IsRecursionAllowed = true,
                    IsRecursionDesired = true,
                    ReturnCode = ReturnCode.NoError,
                    AnswerRecords = new List<DnsRecordBase> 
                    {
                        new ARecord(host, 60, IPAddress.Parse(shimAddress))
                    }
                };

                var client = new DnsResolver(IPAddress.Parse(remoteDnsServer));
                var address = client.Resolve(host);

                var expectedResults = IPAddress.Parse(shimAddress);

                Assert.AreEqual(expectedResults, address);
            }
        }
    }
}
