using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HP.ScalableTest.Core
{
    /// <summary>
    /// Provides serialization via older methods where backwards compatibility is required,
    /// such as serialized data that is stored in a database or external file.
    /// </summary>
    /// <remarks>
    /// Avoid using this class in new code.  Use <see cref="Framework.Serializer" /> instead.
    /// </remarks>
    public static class LegacySerializer
    {
        /// <summary>
        /// Serializes the specified object using <see cref="XmlSerializer" />.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized.</typeparam>
        /// <param name="item">The object to serialize.</param>
        /// <returns>An <see cref="XElement" /> representing the specified object.</returns>
        /// <exception cref="InvalidOperationException">The specified object type cannot be serialized.</exception>
        public static XElement SerializeXml<T>(T item) where T : new()
        {
            // XmlSerializer will cache the result if this constructor is used.
            // Other constructors don't cache and can result in memory leaks.
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XDocument doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);
                serializer.Serialize(writer, item, xmlns);
            }
            return doc.Root;
        }

        /// <summary>
        /// Deserializes the specified XML string into an object using <see cref="XmlSerializer" />.
        /// </summary>
        /// <typeparam name="T">The type of object to be deserialized.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>An object of the specified type deserialized from the XML input.</returns>
        /// <exception cref="InvalidOperationException">The specified object type cannot be deserialized.</exception>
        /// <exception cref="XmlException"><paramref name="xml" /> is not a valid XML string.</exception>
        public static T DeserializeXml<T>(string xml) where T : new()
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new T();
            }

            return DeserializeXml<T>(XElement.Parse(xml));
        }

        /// <summary>
        /// Deserializes the specified <see cref="XElement" /> into an object using <see cref="XmlSerializer" />.
        /// </summary>
        /// <typeparam name="T">The type of object to be deserialized.</typeparam>
        /// <param name="xml">The <see cref="XElement" /> containing the XML to deserialize.</param>
        /// <returns>An object of the specified type deserialized from the XML input.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="xml" /> is null.</exception>
        /// <exception cref="InvalidOperationException">The specified object type cannot be deserialized.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static T DeserializeXml<T>(XElement xml) where T : new()
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            // XmlSerializer will cache the result if this constructor is used.
            // Other constructors don't cache and can result in memory leaks.
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (XmlReader xmlReader = xml.CreateReader())
            {
                return (T)serializer.Deserialize(xmlReader);
            }
        }

        /// <summary>
        /// Serializes the specified object using <see cref="DataContractSerializer" />.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized.</typeparam>
        /// <param name="item">The object to serialize.</param>
        /// <returns>An <see cref="XElement" /> representing the specified object.</returns>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        public static XElement SerializeDataContract<T>(T item)
        {
            XDocument doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, item);
            }

            return doc.Root;
        }

        /// <summary>
        /// Deserializes the specified XML string into an object using <see cref="DataContractSerializer" />.
        /// </summary>
        /// <typeparam name="T">The type of object to be deserialized.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>An object of the specified type deserialized from the XML input.</returns>
        /// <exception cref="ArgumentException"><paramref name="xml" /> is null or empty.</exception>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        /// <exception cref="SerializationException">An error was encountered during deserialization.</exception>
        /// <exception cref="XmlException"><paramref name="xml" /> is not a valid XML string.</exception>
        public static T DeserializeDataContract<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("XML cannot be null or empty.", nameof(xml));
            }

            return DeserializeDataContract<T>(XElement.Parse(xml));
        }

        /// <summary>
        /// Deserializes the specified <see cref="XElement" /> into an object using <see cref="DataContractSerializer" />.
        /// </summary>
        /// <typeparam name="T">The type of object to be deserialized.</typeparam>
        /// <param name="xml">The <see cref="XElement" /> containing the XML to deserialize.</param>
        /// <returns>An object of the specified type deserialized from the XML input.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="xml" /> is null.</exception>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        /// <exception cref="SerializationException">An error was encountered during deserialization.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static T DeserializeDataContract<T>(XElement xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            using (XmlReader xmlReader = xml.CreateReader())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(xmlReader);
            }
        }
    }
}
