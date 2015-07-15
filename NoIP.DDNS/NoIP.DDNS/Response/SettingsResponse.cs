using System;
using System.ComponentModel;
using System.Xml.Serialization;
using NoIP.DDNS.DTO;

namespace NoIP.DDNS.Response
{
    /// <summary>
    /// Zone and host response from No-IP service.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("noip_host_list")]
    public class SettingsResponse
    {
        /// <summary>
        /// Array of zones.
        /// </summary>
        [XmlElement("domain")]
        public NoipHostListDomain[] Domains { get; set; }
        /// <summary>
        /// Client ID.
        /// </summary>
        [XmlAttribute("email")]
        public string Email { get; set; }
        /// <summary>
        /// Boolean if user is an enhanced user.
        /// </summary>
        [XmlAttribute("enhanced")]
        public bool Enhanced { get; set; }
        /// <summary>
        /// Web server.
        /// </summary>
        [XmlAttribute("webserver")]
        public string Webserver { get; set; }
    }

    /// <summary>
    /// Zone record in response from No-IP service.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("domain")]
    public class NoipHostListDomain
    {
        /// <summary>
        /// Array of hosts in zone.
        /// </summary>
        [XmlElement("host")]
        public NoipHostListDomainHost[] Hosts { get; set; }
        /// <summary>
        /// Zone name.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Service level of zone. 
        /// </summary>
        [XmlAttribute("type")]
        public ZoneType Type { get; set; }
    }

    /// <summary>
    /// Host record in response from No-IP service.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("host")]
    public class NoipHostListDomainHost
    {
        /// <summary>
        /// DNS name of host.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// The group the host belongs to.
        /// </summary>
        [XmlAttribute("group")]
        public string Group { get; set; }
        /// <summary>
        /// Boolean if the DNS record is a whildcard (ie *.host.domain.com).
        /// </summary>
        [XmlAttribute("wildcard")]
        public bool Wildcard { get; set; }
    }
}
