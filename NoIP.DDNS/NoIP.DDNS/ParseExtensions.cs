using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NoIP.DDNS
{
    /// <summary>
    /// Extension methods to parse responses from No-IP services.
    /// </summary>
    internal static class ParseExtensions
    {
        /// <summary>
        /// Converts a string to a stream.
        /// </summary>
        /// <param name="value"><see cref="string"/> to be converted.</param>
        /// <returns><exception cref="MemoryStream"> of the string.</exception></returns>
        public static Stream ToStream(this string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Parses an XML response from No-IP services.
        /// </summary>
        /// <typeparam name="T">Type of message returned from the service.</typeparam>
        /// <param name="value">Raw <see cref="string"/> response from the No-IP service.</param>
        /// <returns>Parsed response.</returns>
        public static T ParseXml<T>(this string value) where T : class
        {
            //TODO: Switch to DataContracts and make responses internal
            var reader = XmlReader.Create(value.Trim().ToStream(), 
                                          new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Auto });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
    }
}