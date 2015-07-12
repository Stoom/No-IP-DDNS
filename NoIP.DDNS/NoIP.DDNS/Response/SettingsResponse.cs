using System;
using System.Xml.Serialization;
using NoIP.DDNS.DTO;

namespace NoIP.DDNS.Response
{
    [Serializable, XmlRoot("client")]
    public class SettingsResponse
    {
        public NoipHostListDomain Domain { get; set; }
        [XmlAttribute]
        public string Email { get; set; }
        [XmlAttribute]
        public bool Enhanced { get; set; }
        [XmlAttribute]
        public string Webserver { get; set; }

        [Serializable]
        public class NoipHostListDomain
        {
            [XmlElement("host")]
            public NoipHostListDomainHost[] Host { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public ZoneType Type { get; set; }
        }

        [Serializable]
        public class NoipHostListDomainHost
        {
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public string Group { get; set; }
            [XmlAttribute]
            public bool Wildcard { get; set; }
        }
    }
}
