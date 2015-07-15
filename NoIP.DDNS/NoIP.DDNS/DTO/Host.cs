using System;
using System.Net;

namespace NoIP.DDNS.DTO
{
    /// <summary>
    /// No-IP host record.
    /// </summary>
    public class Host : IEquatable<Host>
    {
        /// <summary>
        /// Host's DNS name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Boolean if the DNS record is a whildcard (ie *.host.domain.com).
        /// </summary>
        public bool Wildcard { get; set; }
        /// <summary>
        /// IP address of the host.
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// Creates an instance of a No-IP host record.
        /// </summary>
        /// <param name="hostName">Host's DNS name.</param>
        public Host(string hostName)
        {
            if (String.IsNullOrWhiteSpace(hostName))
                throw new ArgumentNullException("hostName");

            Name = hostName;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = 0;
            hash ^= Name.GetHashCode();
            hash ^= Wildcard.GetHashCode();
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as Host);
        }

        /// <inheritdoc />
        public bool Equals(Host other)
        {
            if (other == null)
                return false;

            var equals = true;
            equals &= Name.Equals(other.Name);
            equals &= Wildcard.Equals(other.Wildcard);
            return equals;
        }
    }
}
