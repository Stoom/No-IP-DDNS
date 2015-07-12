using System;
using System.Xml.Serialization;
using NoIP.DDNS.DTO;

namespace NoIP.DDNS.Response
{
    [Serializable, XmlRoot("noip_host_list")]
    public class SettingsResponse
    {
        [XmlElementAttribute("domain")]
        public NoipHostListDomain[] Domain { get; set; }
        [XmlAttribute("email")]
        public string Email { get; set; }
        [XmlAttribute("enhanced")]
        public bool Enhanced { get; set; }
        [XmlAttribute("webserver")]
        public string Webserver { get; set; }
    }

    [Serializable, XmlRoot("domain")]
    public class NoipHostListDomain
    {
        [XmlElement("host")]
        public NoipHostListDomainHost[] Host { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("type")]
        public ZoneType Type { get; set; }
    }

    [Serializable, XmlRoot("host")]
    public class NoipHostListDomainHost
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("group")]
        public string Group { get; set; }
        [XmlAttribute("wildcard")]
        public bool Wildcard { get; set; }
    }
}
