using System;

namespace NoIP.DDNS.DTO
{
    public class Zone
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
    }
}
