using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// The class takes the device IP and login credentials and utilizes WSTransferClient to retrieve
    /// the desired in information.
    /// </summary>
    public class ProccessStatusReq
    {
        public string LoginRole { get; set; }
        public string Password { get; set; }
        public string IPAddress { get; set; }
        /// <summary>
        /// Constructor: Requires the login role "admin", password, and the IP address of the targeted device
        /// </summary>
        /// <param name="role">string</param>
        /// <param name="pwd">string</param>
        /// <param name="ipAddr">string</param>
        public ProccessStatusReq(string role, string pwd, string ipAddr)
        {
            LoginRole = role;
            Password = pwd;
            IPAddress = ipAddr;
        }
        /// <summary>
        /// Entry point into the WS* process.
        /// Endpoint: address book, email, FIM, ...
        /// Resource URL: urn:hp:imaging:con:service:email:EmailService
        /// 
        /// The call to WSTranferClient.Get will return an XML string of data.36030
        /// </summary>
        /// <param name="endPoint">string</param>
        /// <param name="resourceUri">string</param>
        /// <returns>XElement</returns>
        public XElement GetEndPointLog(string endPoint, string resourceUri)
        {
            XElement xeDeviceData = null;

            JediDevice jedi = DeviceFactory.Create(IPAddress, Password) as JediDevice;

            if (jedi != null)
            {
                try
                {
                    xeDeviceData = jedi.WebServices.GetDeviceTicket(endPoint, resourceUri);

                }
                catch (DeviceCommunicationException dce)
                {
                    if (dce.InnerException != null)
                    {
                        if( !dce.InnerException.GetType().Equals(typeof(System.ServiceModel.EndpointNotFoundException)) && !dce.InnerException.GetType().Equals(typeof(System.ServiceModel.ProtocolException)))
                        { 
                            throw; 
                        }                     
                    }
                    else
                    {
                        throw new Exception("Unable to communicate with IP Address " + IPAddress + "\n\r" + dce.Message);
                    }
                }
                catch (EntryPointNotFoundException nf)
                {
                    throw new Exception("Unable to communicate with IP Address " + IPAddress + "\n\r" + nf.Message);
                }
            }
            return xeDeviceData;
        }
        /// <summary>
        /// Returns how many times the given child name appears in the given XElement list.
        /// </summary>
        /// <param name="parent">XElement</param>
        /// <param name="childLocalName">string</param>
        /// <returns>int</returns>
        public int CountChildName(XElement parent, string childLocalName)
        {
            int count = 0;
            if (parent.HasElements)
            {
                count = parent.Elements().Count(x => x.Name.LocalName.Equals(childLocalName, StringComparison.OrdinalIgnoreCase));
                
            }
            return count;
        }
        /// <summary>
        /// Returns a string listing of the element values with the given name.
        /// </summary>
        /// <param name="parent">XElement</param>
        /// <param name="childLocalName">string</param>
        /// <returns>List[string]</returns>
        public List<string> GetChildElementValues(XElement parent, string childLocalName)
        {
            var result = new List<string>();
            if (parent.HasElements)
            {
                result = parent.Elements().Where(x => x.Name.LocalName.Equals(childLocalName, StringComparison.OrdinalIgnoreCase)).Select(y => y.Value).ToList();

            }
            return result;
        }
        /// <summary>
        /// Returns the value of the given element name held int he XElement list
        /// </summary>
        /// <param name="parent">XElement</param>
        /// <param name="childLocalName">string</param>
        /// <returns>string</returns>
        public string GetChildElementValue(XElement parent, string childLocalName)
        {
            string result = string.Empty;
            if (parent.HasElements)
            {
                var childElem = parent.Elements().FirstOrDefault(x => x.Name.LocalName.Equals(childLocalName, StringComparison.OrdinalIgnoreCase));
                if (childElem != null)
                {
                    result = childElem.Value;
                }
            }
            return result;
        }
        /// <summary>
        /// Returns a list of XElements that do NOT contain the given parent name
        /// </summary>
        /// <param name="root">XElement</param>
        /// <param name="localName">string</param>
        /// <returns>List[XElement]</returns>
        public List<XElement> GetElements(XElement root, string localName)
        {
            var result = (
                        from el in root.Descendants().Where(x => x.Name.LocalName == localName)
                        select el
                        ).ToList();
            return result;
        }
        /// <summary>
        /// Returns a list of XElements that do contain the given parent name
        /// </summary>
        /// <param name="root">XElement</param>
        /// <param name="localName">string</param>
        /// <returns>List[XElement]</returns>
        public List<XElement> GetElementsSelf(XElement root, string localName)
        {
            var result = (
                        from el in root.DescendantsAndSelf().Where(x => x.Name.LocalName == localName)
                        select el
                        ).ToList();
            return result;
        }

    }
}
