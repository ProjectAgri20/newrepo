using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HP.Epr.Framework
{
    public static class Extension
    {
        /// <summary>
        /// Converts a string of hex values to a byte array.
        /// </summary>
        /// <param name="hex">The hex.</param>
        /// <returns>byte array.</returns>
        public static byte[] HexToBytes(this string hex)
        {
            string cleanHex = hex.Replace(" ", string.Empty);
            cleanHex = cleanHex.Replace("-", string.Empty);
            cleanHex = cleanHex.Replace(":", string.Empty);
            return Enumerable.Range(0, cleanHex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(cleanHex.Substring(x, 2), 16))
                             .ToArray();

        }

        /// <summary>
        /// Short-hand string formatter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// De-serializes XML to an object..
        /// </summary>
        /// <typeparam name="T">The type to de-serialize</typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns>De-serialized object.</returns>
        public static T ToObject<T>(this XDocument xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml.ToString())))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        public static T ToObject<T>(this XElement xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml.ToString())))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Serializes the object to XML format.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>Serialized object.</returns>
        public static XDocument ToXml(this object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StreamWriter writer = new StreamWriter(new MemoryStream()))
            {
                serializer.Serialize(writer, obj);
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                return XDocument.Load(writer.BaseStream);
            }
        }

        /// <summary>
        /// Quick way of converting XDocument to XmlDocument.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static XmlDocument ToXmlDocument(this XDocument xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.ToString());
            return xmlDoc;
        }

        /// <summary>
        /// Quick way of converting XmlElement to XElement.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static XElement ToXElement(this XmlElement xml)
        {
            return XElement.Parse(xml.OuterXml);
        }

        /// <summary>
        /// Quick way of converting XmlDocument to XDocument.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static XDocument ToXDocument(this XmlDocument xml)
        {
            return XDocument.Parse(xml.OuterXml);
        }

        /// <summary>
        /// Quick way of converting XElement to XmlElement.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static XmlElement ToXmlElement(this XElement xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml.ToString());
            return xmlDoc.DocumentElement;
        }
                /// <summary>
                /// Get the <see cref="Exception.Message"/>s of the supplied <paramref name="error"/> and all <see cref="Exception.InnerException"/>s.
                /// </summary>
                /// <param name="error">The <see cref="Exception"/>, or <c>null</c>.</param>
                /// <returns>[<see cref="Exception.Message"/>[\r\nInnerexception: <see cref="Exception.Message"/>[...]]]</returns>
                public static string JoinAllErrorMessages(this Exception error)
                {
                    if (error != null)
                    {
                        if (error.InnerException != null)
                        {
                            return error.Message + Environment.NewLine + "InnerException: " + error.InnerException.JoinAllErrorMessages();
                        }
                        return error.Message;
                    }
                    return string.Empty;
                }
    }
}
