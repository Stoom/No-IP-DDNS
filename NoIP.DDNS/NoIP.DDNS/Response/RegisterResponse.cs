﻿using System;
using System.Xml.Serialization;

namespace NoIP.DDNS.Response
{
    [Serializable, XmlRoot("client")]
    public class RegisterResponse
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "key")]
        public string Key { get; set; }
    }
}
