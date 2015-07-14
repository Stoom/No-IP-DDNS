using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NoIP.DDNS.Response
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable, XmlRoot("error")]
    public class ErrorResponse
    {
        [XmlText()]
        public string Error { get; set; }
    }
}
