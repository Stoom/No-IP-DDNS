using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using NoIP.DDNS.DTO;
using NoIP.DDNS.Exceptions;
using NoIP.DDNS.Response;

namespace NoIP.DDNS
{
    public partial class Client
    {
        public UserAgent UserAgent { get; set; }
        public Boolean IsRegistered
        {
            get { return !String.IsNullOrWhiteSpace(Id) && !String.IsNullOrWhiteSpace(Key); }
        }
        public String Id { get; set; }
        public String Key { get; set; }

        public Client(UserAgent userAgent)
        {
            if (userAgent == null)
                throw new ArgumentNullException("userAgent");

            UserAgent = userAgent;
        }

        protected Dictionary<Zone, HashSet<Host>> CachedZonesAndHosts = new Dictionary<Zone, HashSet<Host>>();

        private readonly HashSet<UpdateStatus> _validStatuses = new HashSet<UpdateStatus>
        {
            UpdateStatus.IpCurrent,
            UpdateStatus.Success,
            UpdateStatus.HostRedirectUpdated,
            UpdateStatus.GroupUpdateSuccess,
            UpdateStatus.GroupIsCurrent
        };

        private readonly HashSet<UpdateStatus> _invalidLookupStatuses = new HashSet<UpdateStatus>
        {
            UpdateStatus.InvalidUserName, 
            UpdateStatus.InvalidPassword
        };

        private readonly HashSet<UpdateStatus> _authentationStatuses = new HashSet<UpdateStatus>
        {
            UpdateStatus.AccountDisabled,
            UpdateStatus.ClientDisabled,
            UpdateStatus.ClientIdTemporarilyDisabled
        };

        public void Register(string username, string password)
        {
            //TODO: Refactor
            using (var webClient = new WebClient())
            {
                webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                webClient.Headers = new WebHeaderCollection 
                {
                    {HttpRequestHeader.UserAgent, UserAgent.ToString()},
                };
                var registerUri = String.Format(REGISTER_URL_SECURE, Uri.EscapeDataString(username), Uri.EscapeDataString(password));
                var rawResponse = webClient.DownloadString(registerUri);
                try
                {
                    var response = rawResponse.ParseXml<RegisterResponse>();
                    Id = response.Id;
                    Key = response.Key;
                }
                catch (InvalidOperationException)
                {
                    var response = rawResponse.ParseXml<ErrorResponse>();
                    if (response.Error.ToUpperInvariant().Contains("BANNED"))
                        throw new UserBannedException();
                    if (response.Error.ToUpperInvariant().Contains("PASSWORD") ||
                        response.Error.ToUpperInvariant().Contains("UNKNOWN"))
                        throw new InvalidLoginException();
                    throw new NoIpException(response.Error);
                }
            }
        }

        public ISet<Zone> GetZones()
        {
            SettingsResponse response;
            using (var client = new WebClient())
            {
                InitializeWebClient(client);
                var settingsUri = String.Format(SETTINGS_URL_SECURE, Id);
                settingsUri += String.Format("&pass={0}", GenerateQueryStringPassword(settingsUri));
                var rawResponse = client.DownloadString(settingsUri);

                if (rawResponse.ToUpperInvariant() == "BAD PASSWORD")
                    throw new InvalidLoginException("Incorrect login or client is not registered correctly.");

                response = rawResponse.ParseXml<SettingsResponse>();
            }

            CachedZonesAndHosts.Clear();
            foreach (var zone in response.Domains)
            {
                var hosts = new HashSet<Host>();
                foreach (var host in zone.Hosts)
                {
                    hosts.Add(new Host(host.Name) { Wildcard = host.Wildcard });
                }
                CachedZonesAndHosts.Add(new Zone(zone.Name, zone.Type), hosts);
            }
            return new HashSet<Zone>(CachedZonesAndHosts.Keys);
        }

        public ISet<Host> GetHosts(Zone zone)
        {
            GetZones();
            return CachedZonesAndHosts[zone];
        }

        public void UpdateHost(Host host)
        {
            UpdateHost(new List<Host> { host });
        }

        public void UpdateHost(IList<Host> hosts)
        {
            if (hosts == null || hosts.Count == 0)
                throw new ArgumentNullException("hosts");
            if (hosts.Any(x => x.Address == null))
                throw new ArgumentException("IP Address is not valid.");
            if (hosts.Any(x => x.Address.AddressFamily != AddressFamily.InterNetwork))
                throw new ArgumentException("Unsupport address version.");

            IDictionary<string, UpdateStatus> response = new Dictionary<string, UpdateStatus>();

            using (var client = new WebClient())
            {
                InitializeWebClient(client);
                var hostGroupings = hosts.OrderBy(x => x.Address.ToString())
                                         .GroupBy(x => x.Address);
                foreach (var hostGrouping in hostGroupings)
                {
                    var hostQueryString = hostGrouping.Aggregate(String.Empty, (current, host) => current + String.Format("&h[]={0}", host.Name)).TrimStart('&');
                    var updateUri = String.Format(UPDATE_URL_SECURE, Id, hostQueryString, hostGrouping.Key);
                    updateUri += String.Format("&pass={0}", GenerateQueryStringPassword(updateUri));
                    var rawResponse = client.DownloadString(updateUri);
                    response.Merge(ParseUpdateResponse(rawResponse));
                }
            }

            var responseStatuses = response.Values.ToHashSet();
            if (responseStatuses.Intersect(_invalidLookupStatuses).Any())
                throw new InvalidLoginException();
            if (responseStatuses.Intersect(_authentationStatuses).Any())
                throw new AuthenticationException();
            if (responseStatuses.Except(_validStatuses).Any())
                throw new UpdateException("Host(s) update failed.", response);
        }

        protected string GenerateQueryStringPassword(string url)
        {
            var uri = new Uri(url);
            var hmacshA1 = new HMACSHA1(Encoding.ASCII.GetBytes(Key.ToLowerInvariant()));
            hmacshA1.Initialize();
            var str = Convert.ToBase64String(hmacshA1.ComputeHash(Encoding.ASCII.GetBytes(uri.PathAndQuery)));
            return Uri.EscapeDataString(string.Format("HMAC{{{0}}}", str.ToLowerInvariant()));
        }

        protected void InitializeWebClient(WebClient client)
        {
            client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            client.Headers = new WebHeaderCollection 
                {
                    {HttpRequestHeader.UserAgent, UserAgent.ToString()}
                };
        }

        private static IDictionary<string, UpdateStatus> ParseUpdateResponse(string response)
        {
            var results = new Dictionary<string, UpdateStatus>();
            foreach (var hostStatus in response.Trim()
                                               .Split('\n')
                                               .Select(status => status.Split(':')))
            {
                if (results.ContainsKey(hostStatus[0]))
                    results[hostStatus[0]] = (UpdateStatus)Convert.ToInt32(hostStatus[1]);
                else
                    results.Add(hostStatus[0], (UpdateStatus)Convert.ToInt32(hostStatus[1]));
            }
            return results;
        }
    }
}
