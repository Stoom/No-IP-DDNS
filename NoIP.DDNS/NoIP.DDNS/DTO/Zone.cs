using System;

namespace NoIP.DDNS.DTO
{
    public class Zone : IEquatable<Zone>
    {
        public string Name { get; private set; }
        public ZoneType Type { get; private set; }

        public Zone(string zoneName, ZoneType zoneType)
        {
            if (String.IsNullOrWhiteSpace(zoneName))
                throw new ArgumentNullException("zoneName");

            Name = zoneName;
            Type = zoneType;
        }

        public override int GetHashCode()
        {
            var hash = 0;
            hash ^= Name.GetHashCode();
            hash ^= Type.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Zone);
        }

        public bool Equals(Zone other)
        {
            if (other == null)
                return false;

            var equals = false;
            equals |= Name.Equals(other.Name);
            equals |= Type.Equals(other.Type);
            return equals;
        }
    }
}
