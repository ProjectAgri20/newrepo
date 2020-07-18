using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Xml;
using System.Xml.Linq;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Utility class for serializing/deserializing objects.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes the specified object to XML.
        /// </summary>
        /// <param name="item">The object to serialize. Must have <see cref="DataContractAttribute" /> applied.</param>
        /// <returns>An <see cref="XElement" /> representing the specified object.</returns>
        /// <exception cref="InvalidDataContractException"><paramref name="item" /> is not a valid data contract.</exception>
        public static XElement Serialize(object item)
        {
            NetDataContractSerializer serializer = CreateSerializer(Enumerable.Empty<SerializerProxyMap>());
            XDocument doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
            {
                serializer.WriteObject(writer, item);
            }
            return doc.Root;
        }

        /// <summary>
        /// Deserializes the specified XML into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the XML into.</typeparam>
        /// <param name="xml">The serialized object data.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="xml"/> is null.</exception>
        /// <exception cref="InvalidCastException">The XML did not deserialize into an object of type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        public static T Deserialize<T>(XElement xml)
        {
            return Deserialize<T>(xml, Enumerable.Empty<SerializerProxyMap>());
        }

        /// <summary>
        /// Deserializes the specified XML into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the XML into.</typeparam>
        /// <param name="xml">The serialized object data.</param>
        /// <param name="proxyMaps">A collection of <see cref="SerializerProxyMap" /> objects that control deserialization.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="xml"/> is null.
        /// <para>or</para>
        /// <paramref name="proxyMaps" /> is null.
        /// </exception>
        /// <exception cref="InvalidCastException">The XML did not deserialize into an object of type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static T Deserialize<T>(XElement xml, IEnumerable<SerializerProxyMap> proxyMaps)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            if (proxyMaps == null)
            {
                throw new ArgumentNullException(nameof(proxyMaps));
            }

            // Namespaces of serialized XML are based on the namespace of the type being serialized.
            // If the proxy maps specify changed namespaces, the XML must be modified to reflect this.
            if (proxyMaps.Any())
            {
                xml = UpdateNamespaces(xml, proxyMaps);
            }

            NetDataContractSerializer serializer = CreateSerializer(proxyMaps);
            return (T)serializer.ReadObject(xml.CreateReader());
        }

        private static XElement UpdateNamespaces(XElement xml, IEnumerable<SerializerProxyMap> proxyMaps)
        {
            string rawXml = xml.ToString();
            foreach (SerializerProxyMap proxyMap in proxyMaps)
            {
                rawXml = proxyMap.UpdateNamespace(rawXml);
            }
            return XElement.Parse(rawXml);
        }

        private static NetDataContractSerializer CreateSerializer(IEnumerable<SerializerProxyMap> proxyMaps)
        {
            return new NetDataContractSerializer
            {
                AssemblyFormat = FormatterAssemblyStyle.Simple,
                Binder = new CustomBinder(proxyMaps)
            };
        }

        /// <summary>
        /// Custom serialization binder that can search loaded assemblies and apply <see cref="SerializerProxyMap" /> overrides.
        /// </summary>
        private sealed class CustomBinder : SerializationBinder
        {
            private readonly List<SerializerProxyMap> _proxyMaps = new List<SerializerProxyMap>();

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomBinder" /> class.
            /// </summary>
            /// <param name="proxyMaps">The proxy maps.</param>
            public CustomBinder(IEnumerable<SerializerProxyMap> proxyMaps)
            {
                _proxyMaps.AddRange(proxyMaps);
            }

            /// <summary>
            /// When overridden in a derived class, controls the binding of a serialized object to a type.
            /// </summary>
            /// <param name="assemblyName">Specifies the <see cref="Assembly" /> name of the serialized object.</param>
            /// <param name="typeName">Specifies the <see cref="Type" /> name of the serialized object.</param>
            /// <returns>The type of the object the formatter creates a new instance of.</returns>
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (string.IsNullOrEmpty(assemblyName))
                {
                    return null;
                }

                string simpleAssemblyName = assemblyName.Split(',')[0];

                // Check proxy maps to see if anything matches.
                SerializerProxyMap matchingMap = _proxyMaps.FirstOrDefault(n => n.IsMatch(simpleAssemblyName, typeName));
                if (matchingMap != null)
                {
                    return matchingMap.ProxyType;
                }

                // Check in loaded assemblies.  The default binder will load from the application directory,
                // but does not look in already-loaded assemblies.  This check solves problems with
                // "Assembly x is not found" errors in plugin assemblies, etc.
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in loadedAssemblies)
                {
                    if (assembly.FullName.Split(',')[0] == simpleAssemblyName)
                    {
                        return assembly.GetType(typeName);
                    }
                }

                // Fall back to the default serialization behavior.
                return null;
            }
        }
    }
}
