using System;

namespace NoIP.DDNS.DTO
{
    /// <summary>
    /// No-IP zone record.
    /// </summary>
    public class Zone : IEquatable<Zone>
    {
        /// <summary>
        /// Name of DNS zone (aka domain).
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Service level of the zone.
        /// </summary>
        public ZoneType Type { get; private set; }

        /// <summary>
        /// Creates an instance of a No-IP zone.
        /// </summary>
        /// <param name="zoneName">Name of DNS zone (aka domain).</param>
        /// <param name="zoneType">Service level of the zone.</param>
        public Zone(string zoneName, ZoneType zoneType)
        {
            if (String.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException("zoneName");

            Name = zoneName;
            Type = zoneType;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hash = 0;
            hash ^= Name.GetHashCode();
            hash ^= Type.GetHashCode();
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as Zone);
        }

        /// <inheritdoc />
        public bool Equals(Zone other)
        {
            if (other == null)
                return false;

            var equals = true;
            equals &= Name.Equals(other.Name);
            equals &= Type.Equals(other.Type);
            return equals;
        }
    }
}
