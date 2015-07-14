using System;
using System.Net;

namespace NoIP.DDNS.DTO
{
    public class Host : IEquatable<Host>
    {
        public string Name { get; set; }
        public bool Wildcard { get; set; }
        public IPAddress Address { get; set; }

        public Host(string hostName)
        {
            if (String.IsNullOrWhiteSpace(hostName))
                throw new ArgumentNullException("hostName");

            Name = hostName;
        }

        public override int GetHashCode()
        {
            var hash = 0;
            hash ^= Name.GetHashCode();
            hash ^= Wildcard.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Host);
        }

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
