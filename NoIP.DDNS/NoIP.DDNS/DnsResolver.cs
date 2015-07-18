using System;
using System.Linq;
using System.Net;
using ARSoft.Tools.Net.Dns;

namespace NoIP.DDNS
{
    public class DnsResolver
    {
        private const int QUERY_TIMEOUT = 10000;

        private readonly DnsClient _client;

        public DnsResolver()
        {
            _client = DnsClient.Default;
        }

        public DnsResolver(params IPAddress[] remoteDnsServerAddress)
        {
            if  (remoteDnsServerAddress == null)
                throw new ArgumentNullException("remoteDnsServerAddress");

            _client = new DnsClient(remoteDnsServerAddress.ToList(), QUERY_TIMEOUT);
        }

        public IPAddress Resolve(string dnsHostName)
        {
            var dnsResponse = _client.Resolve(dnsHostName);
            if (dnsResponse == null || (dnsResponse.ReturnCode != ReturnCode.NoError))
                return null;
            var record = dnsResponse.AnswerRecords.First() as ARecord;
            return (record != null) ? record.Address : null;
        }
    }
}
