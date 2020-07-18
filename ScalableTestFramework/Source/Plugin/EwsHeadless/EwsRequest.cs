using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using HP.DeviceAutomation;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Represents data required for an EWS operation, including a set of key/value pairs for
    /// the web request payload and an XML schema for validation of the values.
    /// </summary>
    public class EwsRequest
    {
        private XmlSchemaSet _contract;
        internal Dictionary<string, string> PayloadValues = new Dictionary<string, string>();
        internal List<string> SchemaParemeters = new List<string>();

        /// <summary>
        /// Gets the request type.
        /// </summary>
        /// <value>The request type.</value>
        public string RequestType { get; }

        /// <summary>
        /// Gets the request subtype.
        /// </summary>
        /// <value>The request subtype.</value>
        public string RequestSubtype { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsRequest"/> class.
        /// </summary>
        /// <param name="requestType">The type of the EWS request.</param>
        /// <param name="requestSubtype">The subtype of the EWS request.</param>
        /// <param name="contract">The contract.</param>
        internal EwsRequest(string requestType, string requestSubtype, XmlSchemaSet contract)
        {
            RequestType = requestType;
            RequestSubtype = requestSubtype;
            _contract = contract;
        }

        internal void AddWithoutValidate(string key, object value)
        {
            if (PayloadValues.ContainsKey(key))
            {
                PayloadValues.Remove(key);
            }
            PayloadValues.Add(key, value.ToString());
        }

        /// <summary>
        /// Adds the specified key/value pair to the web request payload.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            string expectedType = value?.GetType().Name;

            if (IsValidSchemaElement(key, expectedType))
            {
                if (PayloadValues.ContainsKey(key))
                {
                    PayloadValues.Remove(key);
                }
                PayloadValues.Add(key, value?.ToString());
            }
            else
            {
                throw new XmlSchemaException("element is not valid, schema mismatch");
            }
        }



        /// <summary>
        /// Validates this instance to ensure the web request fulfills the requirements of the contract.
        /// </summary>
        /// <returns><c>true</c> if validation is successful, <c>throws Device communication exception</c> otherwise.</returns>
        public bool Validate()
        {
            try
            {
                XDocument payloadDocument = new XDocument(
                new XElement("Contract",
                    new XElement("Name", this.RequestType),
                    new XElement("Parameters",
                        PayloadValues.Select(n => new XElement(n.Key, n.Value.ToString()))
                    )
                )
            );
                payloadDocument.Validate(_contract, null);
            }
            catch (Exception ex)
            {
                throw new DeviceCommunicationException("Contract validation failed", ex.InnerException);
            }



            return true;
        }
        private void AddParamsInSchema(string paramName)
        {
            if (!SchemaParemeters.Contains(paramName))
            {
                SchemaParemeters.Add(paramName);
            }

        }
        /// <summary>
        /// Validates whether the given Key and datatype of the key is as specified in the schema 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataType"></param>
        /// <returns>true if Valiation Pass else false</returns>
        private bool IsValidSchemaElement(string key, string dataType)
        {
            if (_contract == null)
            {
                return true;
            }

            XmlSchema deviceSchema = null;
            foreach (XmlSchema schema in _contract.Schemas())
            {
                deviceSchema = schema;
            }
            // Iterate over each XmlSchemaElement in the Values collection 
            // of the Elements property. 
            foreach (XmlSchemaElement element in deviceSchema.Elements.Values)
            {
                // Get the complex type of the Customer element.
                XmlSchemaComplexType complexType = element.ElementSchemaType as XmlSchemaComplexType;

                // Get the sequence particle of the complex type.
                XmlSchemaSequence sequence = complexType.ContentTypeParticle as XmlSchemaSequence;

                // Iterate over each XmlSchemaElement in the Items collection. 

                if (sequence != null)
                {
                    foreach (XmlSchemaElement childElement in sequence.Items)
                    {
                        XmlSchemaComplexType childComplexType = childElement.ElementSchemaType as XmlSchemaComplexType;
                        XmlSchemaSequence childSequence = childComplexType?.ContentTypeParticle as XmlSchemaSequence;
                        if (childSequence == null) continue;
                        //Filling all the Parameter names to AllParamsInSchema, this is used in the payload to remove the unfilled values.
                        foreach (var childOfChild in childSequence.Items.Cast<XmlSchemaElement>())
                        {
                            AddParamsInSchema(childOfChild.Name);
                        }

                        if (childSequence.Items.Cast<XmlSchemaElement>().Where(childOfChild => string.CompareOrdinal(key.ToUpper(CultureInfo.InvariantCulture), childOfChild.Name.ToUpper(CultureInfo.InvariantCulture)) == 0).Any(childOfChild => childOfChild.ElementSchemaType.Datatype.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(dataType.ToUpper(CultureInfo.InvariantCulture))))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
