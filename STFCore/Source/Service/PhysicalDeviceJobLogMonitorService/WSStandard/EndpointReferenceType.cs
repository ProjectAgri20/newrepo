using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
//using HP.Epr.WebServicesResponder.FromJedi;

namespace HP.Epr.WebServicesResponder.WSStandard
{
    /// <summary>
    /// Helper methods and constructors for EndpointReferenceType
    /// </summary>
    public partial class EndpointReferenceType
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EndpointReferenceType()
        { }

        /// <summary>
        /// Constructor that takes a string as the endpoint address
        /// </summary>
        /// <param name="address">the address</param>
        public EndpointReferenceType(string address)
        {
            this.addressField = new AttributedURI();
            this.addressField.Value = address;
        }

        /// <summary>
        /// Converts and EndpointReferenceType to an EndpointAddressAugust2004
        /// </summary>
        /// <returns>Returns an EndpointAddressAugust2004 object</returns>
        public EndpointAddressAugust2004 ToEndpointReference()
        {
            var builder = new StringBuilder();

            using (var writer = XmlDictionaryWriter.CreateDictionaryWriter(XmlWriter.Create(builder, new XmlWriterSettings() { Indent = false, OmitXmlDeclaration = true })))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EndpointReferenceType));

                serializer.Serialize(writer, this);
            }

            using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(new StringReader(builder.ToString()), new XmlReaderSettings() { IgnoreWhitespace = true })))
            {
                builder.Clear();

                var address = EndpointAddress.ReadFrom(AddressingVersion.WSAddressingAugust2004, reader);

                return EndpointAddressAugust2004.FromEndpointAddress(address);
            }
        }

        /// <summary>
        /// Converts and EndpointAddressAugust2004 to an EndpointReferenceType.
        /// </summary>
        /// <returns>Returns an EndpointReferenceType object</returns>
        public static EndpointReferenceType FromEndpointReference(EndpointAddressAugust2004 epr)
        {
            var builder = new StringBuilder();

            using (var writer = XmlDictionaryWriter.CreateDictionaryWriter(XmlWriter.Create(builder, new XmlWriterSettings() { Indent = false, OmitXmlDeclaration = true })))
            {
                epr.ToEndpointAddress().WriteTo(AddressingVersion.WSAddressingAugust2004, writer);
            }

            using (var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(new StringReader(builder.ToString()), new XmlReaderSettings() { IgnoreWhitespace = true })))
            {
                builder.Clear();

                var serializer = new XmlSerializer(typeof(EndpointReferenceType));

                return (EndpointReferenceType)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Retrieves an end point reference for the ns qualified serviceName
        /// </summary>
        /// <param name="references">A list of endpoint references</param>
        /// <param name="ns">The namespace qaulifier for the serviceName</param>
        /// <param name="serviceName">The name of the service to communicate with</param>
        /// <returns>the endpoint address</returns>
        public static EndpointAddress GetEndpointAddress(List<EndpointReferenceType> references, string ns, String serviceName)
        {
            foreach (EndpointReferenceType reference in references)
            {
                if (serviceName.Equals(reference.ServiceName.Value.Name) && ns.Equals(reference.ServiceName.Value.Namespace))
                {
                    return reference.ToEndpointReference().ToEndpointAddress();
                }
            }
//            throw new InvalidMessageInformationHeaderException(ns + ":" + serviceName + "not found");
            throw new Exception("Invalid Message Header " + ns + ":" + serviceName + "not found");
        }
    }
}
