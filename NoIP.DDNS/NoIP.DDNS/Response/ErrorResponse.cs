using System;
using System.Xml.Serialization;

namespace NoIP.DDNS.Response
{
    [Serializable, XmlRoot("error")]
    public class ErrorResponse
    {
        [XmlText()]
        public string Error { get; set; }
    }
}
