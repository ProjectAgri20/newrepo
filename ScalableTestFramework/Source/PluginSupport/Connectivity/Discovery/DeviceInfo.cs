using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.PluginSupport.Connectivity.Discovery
{
    /// <summary>
    /// Contains device information
    /// </summary>
    public class DeviceInfo
    {
        #region Constants

        const string LINK_LOCAL_PREFIX = "fe80";

        #endregion

        #region Local Variables

        /// <summary>
        /// Contains the port number
        /// </summary>
        int _port;

        /// <summary>
        /// Contains the host name
        /// </summary>
        string _hostName;

        /// <summary>
        /// Contains the model name
        /// </summary>
        string _model;

        /// <summary>
        /// Contains the make
        /// </summary>
        string _make;

        /// <summary>
        /// Contains the IPv4 address
        /// </summary>
        string _ipv4Address;

        /// <summary>
        /// Contains IPv6 address
        /// </summary>
        string _ipv6Address;

        /// <summary>
        /// Contains UUID of the device
        /// </summary>
        string _uuid;

        /// <summary>
        /// Contains the description
        /// </summary>
        string _description;

        /// <summary>
        /// Contains the probe match
        /// </summary>
        string _probeMatchString;

        /// <summary>
        /// Contains printer MAC Address
        /// </summary>
        string _macAddress;

        /// <summary>
        /// Contains Link Local Address
        /// </summary>
        IPAddress _linkLocal;

        /// <summary>
        /// Contains list of stateless IP v6 Address
        /// </summary>
        Collection<IPAddress> _stateLess;

        /// <summary>
        /// Contains list of state full IP v6 Address
        /// </summary>
        IPAddress _stateFull;

        /// <summary>
        /// Contains the Device types (supported features)
        /// </summary>
        ArrayList _deviceTypes;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo"/> class.
        /// </summary>
        /// <param name="probeMatch">Probe Match String</param>
        public DeviceInfo(string probeMatch)
        {
            _deviceTypes = new ArrayList();
            _stateLess = new Collection<IPAddress>();

            // set the device information from the probe match string
            SetDeviceProperties(probeMatch);

            // set different IPv6 addresses
            SetIPv6Address(probeMatch);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Host Name
        /// </summary>
        public string HostName
        {
            get { return _hostName; }
        }

        /// <summary>
        /// Gets Port Number
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        /// <summary>
        /// Gets Model name
        /// </summary>
        public string Model
        {
            get { return _model; }
        }

        /// <summary>
        /// Gets Make of the device
        /// </summary>
        public string Make
        {
            get { return _make; }
        }

        /// <summary>
        /// Gets IPv4 address of the device
        /// </summary>
        public string IPv4Address
        {
            get { return _ipv4Address; }
        }

        /// <summary>
        /// Gets IPv6 address of the device
        /// </summary>
        public string IPv6Address
        {
            get { return _ipv6Address; }
        }

        /// <summary>
        /// Gets UUID of the device
        /// </summary>
        public string Uuid
        {
            get { return _uuid; }
        }

        /// <summary>
        /// Gets the description of the device
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the Probe Match as string
        /// </summary>
        public string ProbeMatchString
        {
            get { return _probeMatchString; }
        }

        /// <summary>
        /// Gets the device types (supported features)
        /// </summary>
        public ArrayList DeviceTypes
        {
            get { return _deviceTypes; }
        }

        /// <summary>
        /// Gets printer link local IPv6 address
        /// </summary>
        public IPAddress LinkLocalAddress
        {
            get { return _linkLocal; }
        }

        /// <summary>
        /// Gets printer state less IPv6 address
        /// </summary>
        public Collection<IPAddress> StateLessAddress
        {
            get { return _stateLess; }
        }

        /// <summary>
        /// Gets printer state full IPv6 address
        /// </summary>
        public IPAddress StateFullAddress
        {
            get { return _stateFull; }
        }

        /// <summary>
        /// Gets MAC Address
        /// </summary>
        public string MacAddress
        {
            get { return _macAddress; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the device properties from the probe match result
        /// <param name="probeMatch">Device probe match string</param>
        /// </summary>
        private void SetDeviceProperties(string probeMatch)
        {
            try
            {
                if (!string.IsNullOrEmpty(probeMatch))
                {
                    // Probe match string
                    _probeMatchString = probeMatch;

                    // IP Addresses
                    _ipv4Address = GetXmlKeyValue("dd:IPv4Address", probeMatch);
                    _ipv6Address = GetXmlKeyValue("dd:IPv6Address", probeMatch);

                    Logger.LogDebug("Printer Discovered : {0}".FormatWith(_ipv4Address));

                    // Hostname
                    _hostName = GetXmlKeyValue("dd:Hostname", probeMatch);

                    // MAC Address
                    _macAddress = GetXmlKeyValue("dd:HardwareAddress", probeMatch);

                    // Port number
                    // eg: http://16.190.66.213:3910  http://fe80::21b:78ff:fee8:ff78:3910 [Format 1]
                    // eg: http://16.190.65.5:3911/ http://[FE80::29C:2FF:FE14:D05A]:3911/ [Format 2]
                    string xAddress = GetXmlKeyValue("wsd:XAddrs", probeMatch);
                    string port = xAddress.Split(' ')[0].Split(':')[2];

                    // check if port number has '/' or not
                    port = port.Replace('/', ' ');
                    _port = Convert.ToInt32(port, CultureInfo.CurrentCulture);


                    // UUID
                    _uuid = GetXmlKeyValue("dd:UUID", probeMatch).Split(':').Last();

                    // Device Details
                    // eg: MFG:Hewlett-Packard;CMD:PJL,BIDI-ECP,PJL,POSTSCRIPT,PDF,PCLXL,PCL;MDL:HP LaserJet P3010 Series;CLS:PRINTER;DES:Hewlett-Packard LaserJet P3010 Series;
                    // eg: MFG:HP;MDL:HP Officejet Pro X576dw MFP;DES:CN598A;
                    string[] deviceDetails = GetXmlKeyValue("pwg:DeviceId", probeMatch).Split(';');

                    foreach (string deviceDetail in deviceDetails)
                    {
                        if (deviceDetail.StartsWith("MFG", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _make = deviceDetail.Split(':')[1];
                        }
                        else if (deviceDetail.StartsWith("MDL", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _model = deviceDetail.Split(':')[1];
                        }
                        else if (deviceDetail.StartsWith("DES", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _description = deviceDetail.Split(':')[1];
                        }
                    }

                    string types = GetXmlKeyValue("wsd:Types", probeMatch);

                    if (!string.IsNullOrEmpty(types))
                    {
                        string[] deviceTypes = GetXmlKeyValue("wsd:Types", probeMatch).Split(' ');
                        _deviceTypes.AddRange(deviceTypes);
                    }
                }
            }
            catch
            {
                Logger.LogDebug("Exception occurred in setting the device details after discovery");
            }
        }

        private void SetIPv6Address(string probeMatchString)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(probeMatchString);
            _stateLess.Clear();

            foreach (XmlNode node in document.GetElementsByTagName("dd:IPv6Address"))
            {
                // Link local address will be the first IPv6Address in the xml which starts with fe80
                if (node.InnerText.StartsWith(LINK_LOCAL_PREFIX, StringComparison.CurrentCultureIgnoreCase))
                {
                    _linkLocal = IPAddress.Parse(node.InnerText);
                    break;
                }
            }

            if (null != _linkLocal)
            {
                foreach (XmlNode node in document.GetElementsByTagName("dd:IPv6Address"))
                {
                    // IPv6Address xml consists of link local, stateless and stateful addresses, hence removing link local since its already set
                    if (!node.InnerText.StartsWith(LINK_LOCAL_PREFIX, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // Stateless addresses (Acquired from Router) will end with link local address
                        if (node.InnerText.EndsWith(_linkLocal.ToString().Replace(string.Concat(LINK_LOCAL_PREFIX, "::"), string.Empty), StringComparison.CurrentCultureIgnoreCase))
                        {
                            _stateLess.Add(IPAddress.Parse(node.InnerText));
                            continue;
                        }
                        else
                        {
                            _stateFull = IPAddress.Parse(node.InnerText);
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value of the given key node
        /// </summary>
        /// <param name="key">Key node</param>
        /// <param name="xmlData">Xml String</param>
        /// <returns>Returns the key value</returns>
        private static string GetXmlKeyValue(string key, string xmlData)
        {
            string xmlValue = string.Empty;

            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                {
                    reader.ReadToFollowing(key);
                    xmlValue = reader.ReadElementContentAsString();
                }
            }
            catch
            {
                return string.Empty;
            }

            return xmlValue;
        }

        #endregion
    }
}
