using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NoIP.DDNS
{
    internal static class ParseExtensions
    {
        public static Stream ToStream(this string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static T ParseXml<T>(this string value) where T : class
        {
            //TODO: Switch to DataContracts and make responses internal
            var reader = XmlReader.Create(value.Trim().ToStream(), 
                                          new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Auto });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
    }
}