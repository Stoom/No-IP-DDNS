﻿using System.Linq;
using System.Net;
using ARSoft.Tools.Net.Dns;

namespace NoIP.DDNS
{
    public class DnsResolver
    {

        private DnsClient _client;

        public DnsResolver()
        {
            _client = DnsClient.Default;
        }

        public IPAddress Resolve(string dnsHostName)
        {
            var dnsResponse = _client.Resolve(dnsHostName);
            if (dnsResponse == null || (dnsResponse.ReturnCode != ReturnCode.NoError && dnsResponse.ReturnCode != ReturnCode.NxDomain))
                return null;
            var record = dnsResponse.AnswerRecords.First() as ARecord;
            return (record != null) ? record.Address : null;
        }
    }
}
