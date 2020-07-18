using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Discovery
{
    /// <summary>
    /// Discovers web service enabled devices in the network over UDP
    /// </summary>
    public static class PrinterDiscovery
    {
        #region Constants

        /// <summary>
        /// Default Multicast IPv4 Address
        /// </summary>
        const string MULTICAST_PROBE_IPv4 = "239.255.255.250";

        /// <summary>
        /// Default Multicast IPv6 Address
        /// </summary>
        const string MULTICAST_PROBE_IPv6 = "ff05::c";

        /// <summary>
        /// Default Multicast Port number
        /// </summary>
        const int PROBE_PORT = 3702;

        #endregion

        #region Public Methods

        /// <summary>
        /// Discovers WS enabled devices in the network.
        /// Note: This method will not support in threading.
        /// <param name="ipv4NetworkPrefix">Network prefix to discover only in the given prefix eg: 192.168.201</param>
        /// <param name="discoverOnIPv6">Discover on IPv6 also</param>
        /// </summary>
        /// <returns>Returns the list of devices found in the network</returns>
        public static Collection<DeviceInfo> Discover(string ipv4NetworkPrefix = null, bool discoverOnIPv6 = true)
        {
            Collection<DeviceInfo> devices = new Collection<DeviceInfo>();

            // local machine may have many network interfaces
            // walk thru each network interface and probe the devices
            IPHostEntry receiverHost = Dns.GetHostEntry(Dns.GetHostName());

            string probeMessage = GetProbeXmlString();
            Collection<string> probeMatches;

            for (int interfaceIndex = 0; interfaceIndex < receiverHost.AddressList.Length; interfaceIndex++)
            {
                IPAddress clientAddress = receiverHost.AddressList[interfaceIndex];

                // if the subnet mask is null discover always
                if (ipv4NetworkPrefix == null || string.IsNullOrEmpty(ipv4NetworkPrefix))
                {
                    // if it is null discover
                    IPAddress ipv4Address = IPAddress.Parse(MULTICAST_PROBE_IPv4);
                    probeMatches = DiscoveryBase.ProbeDevice(clientAddress, ipv4Address, probeMessage, PROBE_PORT);
                    ConvertProbeMatchToDeviceInfo(probeMatches, devices);
                }
                else
                {
                    if (clientAddress.ToString().StartsWith(ipv4NetworkPrefix, StringComparison.InvariantCultureIgnoreCase))
                    {
                        IPAddress ipv4Address = IPAddress.Parse(MULTICAST_PROBE_IPv4);
                        probeMatches = DiscoveryBase.ProbeDevice(clientAddress, ipv4Address, probeMessage, PROBE_PORT);
                        ConvertProbeMatchToDeviceInfo(probeMatches, devices);
                    }
                }

                if (discoverOnIPv6)
                {
                    // Discovering on IPv6
                    IPAddress ipv6Address = IPAddress.Parse(MULTICAST_PROBE_IPv6);
                    probeMatches = DiscoveryBase.ProbeDevice(receiverHost.AddressList[interfaceIndex], ipv6Address, probeMessage, PROBE_PORT);

                    ConvertProbeMatchToDeviceInfo(probeMatches, devices);
                }
            }

            return devices;
        }

        /// <summary>
        /// Tells whether device is discovered or not
        /// Note: This method will not support in threading.
        /// </summary>
        /// <param name="deviceAddress">IP Address of the device</param>
        /// <param name="deviceInfo">Device information gets updated if the device is discovered</param>
        /// <param name="ipv4NetworkPrefix">Network prefix to discover only in the given prefix</param>
        /// <param name="discoverOnIPv6">Discover on IPv6 also</param>
        /// <returns>Returns true if the device is discovered, else returns false</returns>
        public static bool Discover(IPAddress deviceAddress, out DeviceInfo deviceInfo, string ipv4NetworkPrefix = null, bool discoverOnIPv6 = false)
        {
            Collection<DeviceInfo> devices = Discover(ipv4NetworkPrefix, discoverOnIPv6);

            deviceInfo = null;

            if (null != devices)
            {
                if (deviceAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    deviceInfo = devices.FirstOrDefault(d => d.IPv4Address.Equals(deviceAddress.ToString()));
                }
                else if (deviceAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    deviceInfo = devices.FirstOrDefault(d => d.IPv6Address.Equals(deviceAddress.ToString()));
                }

                if (null == deviceInfo)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Discover the device based on MAC address.
        /// </summary>
        /// <param name="macAddress">MAC address of the printer.</param>
        /// <param name="deviceInfo"><see cref="DeviceInfo"/></param>
        /// <param name="isPreviouslyInAutoIP">true if printer was configured with Auto IP previously and changed to different network.</param>
        /// <param name="ipv4NetworkPrefix">Network prefix to discover only in the given prefix</param>
        /// <param name="discoverOnIPv6">Discover on IPv6 also</param>
        /// <returns>true if MAC address is matching with discovered devices, false otherwise.</returns>
        public static bool Discover(string macAddress, out DeviceInfo deviceInfo, bool isPreviouslyInAutoIP = false, string ipv4NetworkPrefix = null, bool discoverOnIPv6 = false)
        {
            deviceInfo = null;
            Collection<DeviceInfo> discoveredDevices = Discover(ipv4NetworkPrefix, discoverOnIPv6);

            if (null != discoveredDevices)
            {
                // If Printer is changed from one network to other, sometimes Printer gets discovered with '0.0.0.0' IP Address first
                // then with new IP Address is discovered, so ignoring the first discovered device.
                // If Printer was previously configured with Auto IP and changed to different network, Printer with Auto IP is discovered first
                // then Printer with new IP Address is discovered, so ignoring the first discovered device
                deviceInfo = discoveredDevices.FirstOrDefault(item => item.MacAddress.EqualsIgnoreCase(macAddress)
                                && !IPAddress.Parse(item.IPv4Address).Equals(IPAddress.Any) && !(isPreviouslyInAutoIP && IPAddress.Parse(item.IPv4Address).IsAutoIP()));

                return deviceInfo == null ? false : true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Probes the given device (IP Address)
        /// </summary>
        /// <param name="deviceAddress">IP Address of the device to probe</param>
        /// <returns>Returns Device information if it is discovered else returns null</returns>
        public static DeviceInfo ProbeDevice(IPAddress deviceAddress)
        {
            Collection<DeviceInfo> devices = new Collection<DeviceInfo>();

            string probeMessage = GetProbeXmlString();

            // local machine may have many network interfaces
            // walk thru each network interface and probe the devices
            IPHostEntry receiverHost = Dns.GetHostEntry(Dns.GetHostName());

            for (int interfaceIndex = 0; interfaceIndex < receiverHost.AddressList.Length; interfaceIndex++)
            {
                Collection<string> probeMatches = DiscoveryBase.ProbeDevice(receiverHost.AddressList[interfaceIndex], deviceAddress, probeMessage, PROBE_PORT);

                ConvertProbeMatchToDeviceInfo(probeMatches, devices);
            }

            // since we are probing to a specific device it should get only one device
            if (devices.Count <= 0)
            {
                return null;
            }
            else
            {
                return devices[0];
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the Probe Xml as string
        /// </summary>
        /// <returns>Probe xml as string</returns>        
        private static string GetProbeXmlString()
        {
            // Construct static parameters for the PROBE packet.
            string pktGUIDString = "urn:" + "uuid:" + System.Guid.NewGuid().ToString();
            string probeToURI = "urn:schemas-xmlsoap-org:ws:2005:04:discovery";
            string probeTypes = "hpd:hpDevice";

            // Build the XML Document.
            XmlDocument probeDoc = new XmlDocument();
            XmlNode xmlNode = probeDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            probeDoc.AppendChild(xmlNode);

            // Build the SOAP Element
            XmlElement soapElement = probeDoc.CreateElement("s", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            soapElement.SetAttribute("xmlns:a", "http://schemas.xmlsoap.org/ws/2004/08/addressing");
            soapElement.SetAttribute("xmlns:d", "http://schemas.xmlsoap.org/ws/2005/04/discovery");
            soapElement.SetAttribute("xmlns:hpd", "http://www.hp.com/schemas/imaging/con/discovery/2006/09/19");
            soapElement.SetAttribute("xmlns:wsdp", "http://schemas.xmlsoap.org/ws/2006/02/devprof");
            soapElement.SetAttribute("xmlns:ledm", "http://www.hp.com/schemas/imaging/con/ledm/discoverytree/2007/07/01");

            // Build the PROBE Header Element
            XmlElement headerElement = probeDoc.CreateElement("s", "Header", "http://www.w3.org/2003/05/soap-envelope");
            XmlElement actionElement = probeDoc.CreateElement("a", "Action", "http://schemas.xmlsoap.org/ws/2004/08/addressing"); actionElement.AppendChild(probeDoc.CreateTextNode("http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe"));
            XmlElement messageIDElement = probeDoc.CreateElement("a", "MessageID", "http://schemas.xmlsoap.org/ws/2004/08/addressing"); messageIDElement.AppendChild(probeDoc.CreateTextNode(pktGUIDString));
            XmlElement toElement = probeDoc.CreateElement("a", "To", "http://schemas.xmlsoap.org/ws/2004/08/addressing"); toElement.AppendChild(probeDoc.CreateTextNode(probeToURI));

            headerElement.AppendChild(actionElement);
            headerElement.AppendChild(messageIDElement);
            headerElement.AppendChild(toElement);
            soapElement.AppendChild(headerElement);

            // Build the PROBE Body Element
            XmlElement bodyElement = probeDoc.CreateElement("s", "Body", "http://www.w3.org/2003/05/soap-envelope");
            XmlElement probeElement = probeDoc.CreateElement("d", "Probe", "http://schemas.xmlsoap.org/ws/2005/04/discovery");
            XmlElement typesElement = probeDoc.CreateElement("d", "Types", "http://schemas.xmlsoap.org/ws/2005/04/discovery");

            typesElement.AppendChild(probeDoc.CreateTextNode(probeTypes));
            probeElement.AppendChild(typesElement);
            bodyElement.AppendChild(probeElement);
            soapElement.AppendChild(bodyElement);
            probeDoc.AppendChild(soapElement);

            return probeDoc.InnerXml;
        }

        /// <summary>
        /// Adds the device to the collection if it is not available
        /// </summary>
        /// <param name="devices">Collection of devices</param>
        /// <param name="device">device details</param>        
        private static void AddUniqueDevice(Collection<DeviceInfo> devices, DeviceInfo device)
        {
            // Add the device to collection if it is not already exist
            if (!devices.Any(di => di.IPv4Address.Equals(device.IPv4Address)))
            {
                devices.Add(device);
            }
        }

        /// <summary>
        /// Convert probe match string to device info object
        /// </summary>
        /// <param name="probeMatches">probe matches</param>
        /// <param name="devices">Devices list</param>
        private static void ConvertProbeMatchToDeviceInfo(Collection<string> probeMatches, Collection<DeviceInfo> devices)
        {
            foreach (string probeMatch in probeMatches)
            {
                DeviceInfo device = new DeviceInfo(probeMatch);
                AddUniqueDevice(devices, device);
            }
        }

        #endregion
    }
}
