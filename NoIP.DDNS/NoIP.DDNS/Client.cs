using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
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
                    hosts.Add(new Host(host.Name) {Wildcard = host.Wildcard});
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
            if (host == null)
                throw new ArgumentNullException("host");
            if (host.Address == null)
                throw new ArgumentException("IP Address is not valid.");

            IDictionary<string, UpdateStatus> response;

            using (var client = new WebClient())
            {
                InitializeWebClient(client);
                var hostQueryString = String.Format("h[]={0}", host.Name);
                var uri = String.Format(UPDATE_URL_SECURE, Id, hostQueryString, host.Address);
                uri += GenerateQueryStringPassword(uri);
                var rawResponse = client.DownloadString(uri);
                
                //TODO: negatives

                response = ParseUpdateResponse(rawResponse);
            }
            
            //TODO: Check for failurs and throw accordingly

            throw new NotImplementedException();
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
                    {HttpRequestHeader.UserAgent, UserAgent.ToString()},
                };
        }

        private IDictionary<string, UpdateStatus> ParseUpdateResponse(string response)
        {
            throw new NotImplementedException();
        }
    }
}
