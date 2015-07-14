using System;
using System.ComponentModel;
using System.Xml.Serialization;
using NoIP.DDNS.DTO;

namespace NoIP.DDNS.Response
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("noip_host_list")]
    public class SettingsResponse
    {
        [XmlElement("domain")]
        public NoipHostListDomain[] Domains { get; set; }
        [XmlAttribute("email")]
        public string Email { get; set; }
        [XmlAttribute("enhanced")]
        public bool Enhanced { get; set; }
        [XmlAttribute("webserver")]
        public string Webserver { get; set; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("domain")]
    public class NoipHostListDomain
    {
        [XmlElement("host")]
        public NoipHostListDomainHost[] Hosts { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("type")]
        public ZoneType Type { get; set; }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
