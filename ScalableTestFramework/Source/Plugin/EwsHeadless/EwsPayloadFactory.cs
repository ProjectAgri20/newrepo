using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using HP.DeviceAutomation;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Gets the EWS Contract based on Device Family, Device Filter Type and Firmware Version
    /// If the Contract not found for the given Firmware Version then tries to fetch the Default based on the Device Family and Device Filter Type
    /// </summary>
    public static class EwsPayloadFactory
    {
        private static readonly List<string> Locations = new List<string>();

        /// <summary>
        /// Adds the path of the contract location
        /// </summary>
        /// <param name="path"></param>
        /// <Output>Stores the Location of the contract to the Location dictionary object</Output>
        public static void AddContractLocation(string path)
        {
            if (!Locations.Contains(path))
            {
                Locations.Add(path);
            }
        }

        /// <summary>
        /// This method will select contract based on the Device Family, Device Filter Type, Firmware Version and Locations
        /// </summary>
        /// <param name="deviceFamily">Device Family</param>
        /// <param name="requestType">Device Filter Type</param>
        /// <param name="firmwareVersion">Firmware Version</param>
        /// <returns>Will return the EWS contract</returns>
        internal static XmlSchemaSet SelectContract(string deviceFamily, string requestType, string firmwareVersion)
        {
            bool isSchemaFound = false;
            string schemafilename;
            XmlSchemaSet schemaset = new XmlSchemaSet();
            //ResourceSet resourceSet = Contracts.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            foreach (string schemalocation in Locations)
            {
                //getting the Schema from  the location specified by the end user
                try
                {
                    //Getting schema files from the location
                    var fileEntries = Directory.GetFiles(schemalocation, "*.xsd").Select(Path.GetFileName).ToList();
                    //Searching the schemafilename with the Firmware version of the device
                    schemafilename = $"{requestType}_{deviceFamily}_{firmwareVersion}.xsd";
                    string targetSchemaFile = fileEntries.FirstOrDefault(x => x.Equals(schemafilename, StringComparison.OrdinalIgnoreCase));

                    //Checking if the targetschemafile is null or empty which means the schema file is not available in the location specified.
                    if (string.IsNullOrEmpty(targetSchemaFile))
                    {
                        schemafilename = $"{requestType}_{deviceFamily}_Default.xsd";
                        targetSchemaFile = fileEntries.FirstOrDefault(x => x.Equals(schemafilename, StringComparison.OrdinalIgnoreCase));

                        if (string.IsNullOrEmpty(targetSchemaFile))
                            continue;
                    }

                    targetSchemaFile = schemalocation + "/" + targetSchemaFile;
                    schemaset.Add(string.Empty, targetSchemaFile);
                    schemaset.Compile();
                    isSchemaFound = true;
                    break;
                }
                catch (Exception)
                {
                    //ignored
                }
            }

            //Checking the Location mentioned by the user
            if (isSchemaFound)
                return schemaset;
            throw new Exception("Could not find the contract for the Firmware Version or the default");
        }

        internal static List<PayloadDefinition> CreatePayloadDefinitions(EwsRequest request, string firmware, string deviceFamily)
        {
            List<PayloadDefinition> payloadDefinitions = new List<PayloadDefinition>();
            XDocument xDoc = GetPayloadXDoc(request.RequestType);
            //Geting the Number of Requests
            int numberOfRequests = GetPayLoadRequestCount(firmware, deviceFamily, request.RequestType, request.RequestSubtype);

            for (int i = 1; i <= numberOfRequests; i++)
            {
                PayloadDefinition payloadDefinition = GetPayLoadFromFile(xDoc, firmware, deviceFamily, request.RequestSubtype, i);
                payloadDefinitions.Add(payloadDefinition);
            }
            return payloadDefinitions;
        }

        internal static int GetPayLoadRequestCount(string firmwareVersion, string deviceFamilyType, string filterType, string subFilterType)
        {
            XDocument xDocument = GetPayloadXDoc(filterType);
            var printerElements = xDocument.Element("Printers")?.Elements("Printer").Where(x => x.Attribute("Type").Value.Equals(deviceFamilyType.ToUpper(CultureInfo.CurrentCulture)) &&
                                                                                          x.Attribute("SubFilterType").Value.Equals(subFilterType)).ToList();

            foreach (var printerElement in printerElements?.Where(printerElement => printerElement.Elements("FirmwareVersion").Any(x => x.Value == firmwareVersion)))
            {
                return printerElement.Descendants("Request").Count();
            }
            return printerElements.First(x => x.Attribute("Default").Value == "Yes").Descendants("Request").Count();
        }

        internal static PayloadDefinition GetPayLoad(string firmwareVersion, string deviceFamilyType, string filterType, string subFilterType, int requestNumber)
        {
            XDocument xDoc = GetPayloadXDoc(filterType);
            return GetPayLoadFromFile(xDoc, firmwareVersion, deviceFamilyType, subFilterType, requestNumber);
            //XmlDocument doc = GetPayloadXmlDoc(filterType);
            // return GetPayLoadFromFile(doc, firmwareVersion, deviceFamilyType, subFilterType, requestNumber);
        }

        private static XDocument GetPayloadXDoc(string filterType)
        {
            //ResourceSet resourceSet = Payloads.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            foreach (string location in Locations)
            {
                //getting the xml(Payload) from  the location specified by the end user
                try
                {
                    string[] fileEntries = Directory.GetFiles(location, "*.xml").Select(Path.GetFileName).ToArray();
                    string payloadfilename = filterType + ".xml";
                    string targetXmlFile = fileEntries.FirstOrDefault(x => x.Equals(payloadfilename));

                    if (!string.IsNullOrEmpty(targetXmlFile))
                    {
                        targetXmlFile = location + "/" + targetXmlFile;
                        return XDocument.Load(targetXmlFile);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            //Checking in the EWSAutmation.Payloads - Resources for the Payload info
            //string resourceName = resourceSet.GetString(filterType);
            //if (!string.IsNullOrEmpty(resourceName))
            //{
            //    return XDocument.Parse(resourceName);
            //}

            throw new FileNotFoundException("Payload file was not found for the Request Type and Sub-Request Type");
        }

        private static PayloadDefinition GetPayLoadFromFile(XDocument xDoc, string firmwareVersion, string deviceFamilyType, string subFilterType, int requestNumber)
        {
            var printerElements = xDoc.Element("Printers")?.Elements("Printer").Where(x => x.Attribute("Type")?.Value == deviceFamilyType.ToUpper(CultureInfo.InvariantCulture) && x.Attribute("SubFilterType")?.Value == subFilterType).ToList();

            if (printerElements != null)
                if (!printerElements.Any(printerElement => printerElement.Elements("FirmwareVersion").Any(x => x.Value == firmwareVersion)))
                {
                    var request = printerElements.First(x => x.Attribute("Default").Value.Equals("Yes", StringComparison.OrdinalIgnoreCase)).Element("Requests").Elements("Request").FirstOrDefault(x => x.Attribute("Order")?.Value == requestNumber.ToString(CultureInfo.InvariantCulture));
                    if (request != null)
                        return GeneratePayLoadDefinition(request);
                }

            return printerElements.Where(printerElement => printerElement.Elements("FirmwareVersion").Any(x => x.Value == firmwareVersion)).Select(printerElement => printerElement.Element("Requests")?.Elements("Request").FirstOrDefault(x => x.Attribute("Order")?.Value == requestNumber.ToString(CultureInfo.InvariantCulture))).Select(GeneratePayLoadDefinition).FirstOrDefault();
        }

        private static PayloadDefinition GeneratePayLoadDefinition(XElement request)
        {
            PayloadDefinition definition = new PayloadDefinition();
            if (request != null)
            {
                definition.TargetUrl = request.Element("URL")?.Value;
                var httpMethod = request.Element("Method")?.Value;
                if (httpMethod.Equals("UPLOAD"))
                {
                    definition.IsUpload = true;
                    definition.HttpMethod = HttpVerb.Post;
                    definition.IsRemoveSolution = false;
                }
                else
                {
                    definition.IsUpload = false;
                    definition.HttpMethod = (HttpVerb)Enum.Parse(typeof(HttpVerb), httpMethod, ignoreCase: true);
                }

                definition.IsSessionIdRequired = !string.IsNullOrEmpty(request.Attribute("sessionID")?.Value);

                if (definition.HttpMethod != HttpVerb.Get)
                {
                    definition.Payload = SanitizePayLoad(request);
                    definition.IsViewStateRequired = !string.IsNullOrEmpty(request.Attribute("Viewstate")?.Value);
                    definition.IsWizardIdRequired = !string.IsNullOrEmpty(request.Attribute("WizardID")?.Value);
                    definition.IsHideRequired = !string.IsNullOrEmpty(request.Attribute("Hide")?.Value);

                    definition.ViewStateUrl = request.Element("ViewstateURL")?.Value;
                    definition.WizardIdUrl = request.Element("WizardIDURL")?.Value;
                    definition.HideUrl = request.Element("HideURL")?.Value;

                    definition.IsRemoveSolution = request.Element("IsRemoveSolution") != null && Convert.ToBoolean(request.Element("IsRemoveSolution")?.Value, CultureInfo.InvariantCulture);
                }

                SetPayloadHeaders(request, definition);

                var namevaluepairs =
                    request.Element("NameValuePairs")?
                        .Elements("NameValuePair")
                        .OrderBy(x => x.Attribute("Order")?.Value)
                        .ToList();
                if (namevaluepairs == null) return definition;
                foreach (var namevaluepair in namevaluepairs)
                {
                    definition.NameValuePairs.Add(namevaluepair.Element("Name")?.Value, namevaluepair.Element("Value")?.Value);
                }
            }
            return definition;
        }

        private static void SetPayloadHeaders(XElement request, PayloadDefinition definition)
        {
            var headers = request.Element("Headers")?.Elements("Header").OrderBy(x => x.Attribute("Order")?.Value).ToList();
            if (headers != null)
                foreach (var header in headers)
                {
                    definition.Headers.Add(header.Element("HeaderName")?.Value, header.Element("HeaderValue")?.Value);
                }
        }

        private static string SanitizePayLoad(XElement request)
        {
            var payLoad = request.Element("Payload")?.Value;
            if (!string.IsNullOrEmpty(payLoad))
            {
                payLoad = payLoad.Replace("amp;", "&");
                payLoad = payLoad.Replace("gt;", ">");
                payLoad = payLoad.Replace("lt;", "<");
            }
            return payLoad;
        }
    }
}