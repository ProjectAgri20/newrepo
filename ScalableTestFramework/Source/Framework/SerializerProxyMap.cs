using System;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Defines a mapping from serialized class information to a proxy <see cref="Type" />
    /// that can be used for deserialization.
    /// </summary>
    public sealed class SerializerProxyMap
    {
        /// <summary>
        /// Gets the proxy <see cref="Type" /> into which data will be serialized.
        /// </summary>
        public Type ProxyType { get; }

        /// <summary>
        /// Gets the assembly name of the serialized <see cref="Type" />.
        /// </summary>
        public string SerializedAssemblyName { get; }

        /// <summary>
        /// Gets the namespace of the serialized <see cref="Type" />.
        /// </summary>
        public string SerializedTypeNamespace { get; }

        /// <summary>
        /// Gets the name of the serialized <see cref="Type" />.
        /// </summary>
        public string SerializedTypeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerProxyMap" /> class.
        /// </summary>
        /// <param name="proxyType">The proxy <see cref="Type" /> into which data will be deserialized.</param>
        /// <param name="assemblyName">The assembly name of the serialized <see cref="Type" />.  (Does not include filename suffix or version information.)</param>
        /// <param name="typeNamespace">The namespace of the serialized <see cref="Type" />.</param>
        /// <param name="typeName">The name of the serialized <see cref="Type" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="proxyType" /> is null.
        /// <para>or</para>
        /// <paramref name="assemblyName" /> is null.
        /// <para>or</para>
        /// <paramref name="typeNamespace" /> is null.
        /// <para>or</para>
        /// <paramref name="typeName" /> is null.
        /// </exception>
        public SerializerProxyMap(Type proxyType, string assemblyName, string typeNamespace, string typeName)
        {
            ProxyType = proxyType ?? throw new ArgumentNullException(nameof(proxyType));
            SerializedAssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            SerializedTypeNamespace = typeNamespace ?? throw new ArgumentNullException(nameof(typeNamespace));
            SerializedTypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        }

        /// <summary>
        /// Determines whether the specified assembly and type matches this proxy definition.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="typeName">The type name.</param>
        /// <returns><c>true</c> if the specified assembly and type matches this proxy definition; otherwise, <c>false</c>.</returns>
        internal bool IsMatch(string assemblyName, string typeName)
        {
            string fullTypeName = string.Join(".", SerializedTypeNamespace, SerializedTypeName);

            return SerializedAssemblyName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase)
                && fullTypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Updates all namespaces in the specified XML to convert from the old namespace defined by
        /// this proxy map to the new namespace defined by the proxy type.
        /// </summary>
        /// <param name="xml">The original serialized XML.</param>
        /// <returns>The updated XML with new namespaces.</returns>
        internal string UpdateNamespace(string xml)
        {
            string namespacePrefix = "http://schemas.datacontract.org/2004/07/";
            string oldXmlNamespace = namespacePrefix + SerializedTypeNamespace;
            string newXmlNamespace = namespacePrefix + ProxyType.Namespace;
            return xml.Replace(oldXmlNamespace, newXmlNamespace);
        }
    }
}
