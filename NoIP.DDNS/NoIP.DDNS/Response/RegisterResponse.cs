using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NoIP.DDNS.Response
{
    /// <summary>
    /// Registration response from No-IP service.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("client")]
    public class RegisterResponse
    {
        /// <summary>
        /// New client ID.
        /// </summary>
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        /// <summary>
        /// New client key.
        /// </summary>
        [XmlElement(ElementName = "key")]
        public string Key { get; set; }
    }
}
