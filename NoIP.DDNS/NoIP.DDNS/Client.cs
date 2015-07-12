using System;
using System.Net;
using System.Net.Cache;
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

        public void Register(string username, string password)
        {
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
    }
}
