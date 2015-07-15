using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NoIP.DDNS.Response
{
    /// <summary>
    /// Error response from No-IP service.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("error")]
    public class ErrorResponse
    {
        /// <summary>
        /// Error message.
        /// </summary>
        [XmlText()]
        public string Error { get; set; }
    }
}
