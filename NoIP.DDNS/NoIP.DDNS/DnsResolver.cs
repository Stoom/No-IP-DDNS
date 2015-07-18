﻿using System;
using System.Linq;
using System.Net;
using ARSoft.Tools.Net.Dns;

namespace NoIP.DDNS
{
    /// <summary>
    /// DNS resolver to resolve No-IP host addresses.
    /// </summary>
    public class DnsResolver
    {
        private const int QUERY_TIMEOUT = 10000;

        private readonly DnsClient _client;

        /// <summary>
        /// Creates a new instance of DnsResolver that resolves against the local DNS servers.
        /// </summary>
        public DnsResolver()
        {
            _client = DnsClient.Default;
        }

        /// <summary>
        /// Creates a new instance of DnsResolver that resolves against specified DNS servers.
        /// </summary>
        /// <param name="remoteDnsServerAddress">Array of server addresses to use for resolving.</param>
        public DnsResolver(params IPAddress[] remoteDnsServerAddress)
        {
            if  (remoteDnsServerAddress == null)
                throw new ArgumentNullException("remoteDnsServerAddress");

            _client = new DnsClient(remoteDnsServerAddress.ToList(), QUERY_TIMEOUT);
        }

        /// <summary>
        /// Resolves a specified host to IP address.
        /// </summary>
        /// <param name="dnsHostName">DNS host name to resolve.</param>
        /// <returns>Resolved host name if host is found, or null if lookup fails.</returns>
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
