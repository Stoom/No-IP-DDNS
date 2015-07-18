using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NoIP.DDNS.DTO;
using NoIP.DDNS.Exceptions;
using NoIP.DDNS.Response;

namespace NoIP.DDNS
{
    /// <summary>
    /// No-IP Dynamic DNS client.  The client manages all connections to the No-IP Servers.
    /// </summary>
    public partial class Client
    {
        /// <summary>
        /// Useragent that is submitted to No-IP.  This will display on the client managment page.
        /// </summary>
        public UserAgent UserAgent { get; set; }
        /// <summary>
        /// Boolean value to determine if the client is registered or not.  Client must be registered to preforme any task.
        /// </summary>
        public Boolean IsRegistered
        {
            get { return !String.IsNullOrWhiteSpace(Id) && !String.IsNullOrWhiteSpace(Key); }
        }
        /// <summary>
        /// No-IP client id (aka "secure" username).
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// No-IP client key (aka "secure" password).
        /// </summary>
        public String Key { get; set; }

        /// <summary>
        /// Dns resolver to resolve No-IP host addresses.
        /// </summary>
        public DnsResolver Dns { get; set; }

        /// <summary>
        /// Constructs an instance of the No-IP Client.
        /// </summary>
        /// <param name="userAgent">Useragent of integrating program.</param>
        public Client(UserAgent userAgent)
        {
            if (userAgent == null)
                throw new ArgumentNullException("userAgent");

            UserAgent = userAgent;
        }

        /// <summary>
        /// Cached zones and hosts.
        /// </summary>
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

        /// <summary>
        /// Registers the integrating assembly with the No-IP services.
        /// </summary>
        /// <param name="username">No-IP username/e-mail address.</param>
        /// <param name="password">No-IP password.</param>
        /// <exception cref="InvalidLoginException">Client ID or Key is incorrect.</exception>
        /// <exception cref="UserBannedException">User has been banned on No-IP.</exception>
        /// <exception cref="NoIpException">Unexpected error has occured.</exception>
        public void Register(string username, string password)
        {
            using (var client = new WebClient())
            {
                InitializeWebClient(client);
                var registerUri = String.Format(REGISTER_URL_SECURE, Uri.EscapeDataString(username), Uri.EscapeDataString(password));
                var rawResponse = client.DownloadString(registerUri);
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

        /// <summary>
        /// Gets a list of all zones (domains) registered to the user.
        /// </summary>
        /// <returns><see cref="ISet{Zone}"/> of all zones registed to user.</returns>
        /// <exception cref="InvalidLoginException">Client ID or Key is incorrect.</exception>
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

        /// <summary>
        /// Gets a list of all hosts for a given zone.
        /// </summary>
        /// <param name="zone">Zone to get hosts for.</param>
        /// <returns><see cref="ISet{T}"/> of all hosts in a given zone.</returns>
        /// <exception cref="InvalidLoginException">Client ID or Key is incorrect.</exception>
        public ISet<Host> GetHosts(Zone zone)
        {
            GetZones();
            var hosts = CachedZonesAndHosts[zone];

            if (Dns != null)
            {
                Parallel.ForEach(hosts, host =>
                {
                    host.Address = Dns.Resolve(host.Name);
                });
            }

            return CachedZonesAndHosts[zone];
        }

        /// <summary>
        /// Updates a given host to the new IP address.
        /// </summary>
        /// <param name="host">Given host to update.</param>
        /// <exception cref="InvalidLoginException">Client ID or Key is incorrect.</exception>
        /// <exception cref="AuthenticationException">Usually a client id has been disabled.</exception>
        /// <exception cref="UpdateException">An error occured when updated the host not related to authentication.</exception>
        public void UpdateHost(Host host)
        {
            UpdateHost(new List<Host> { host });
        }

        /// <summary>
        /// Updates a given list of hosts to the new IP addresses.
        /// </summary>
        /// <param name="hosts"><see cref="IList{T}"/> of hosts to update.</param>
        /// <exception cref="InvalidLoginException">Client ID or Key is incorrect.</exception>
        /// <exception cref="AuthenticationException">Usually a client id has been disabled.</exception>
        /// <exception cref="UpdateException">An error occured when updated the host not related to authentication.</exception>
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

        /// <summary>
        /// Generates the encrypted password to append to the end of a request URI.
        /// </summary>
        /// <param name="url">request URI without "&amp;pass=" parameter.</param>
        /// <returns><see cref="string"/> of the encrypted password.</returns>
        protected string GenerateQueryStringPassword(string url)
        {
            var uri = new Uri(url);
            var hmacshA1 = new HMACSHA1(Encoding.ASCII.GetBytes(Key.ToLowerInvariant()));
            hmacshA1.Initialize();
            var str = Convert.ToBase64String(hmacshA1.ComputeHash(Encoding.ASCII.GetBytes(uri.PathAndQuery)));
            return Uri.EscapeDataString(string.Format("HMAC{{{0}}}", str.ToLowerInvariant()));
        }

        /// <summary>
        /// Sets common web client headers and options.
        /// </summary>
        /// <param name="client"><see cref="WebClient"/> used to make requests.</param>
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
