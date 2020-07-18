
// Changed to strings since the DAT SNMP doesn't use the ObjectIdentifier
// Don Anderson, 04/17/2015

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Class that contains SNMP object identifier constants.
    /// </summary>
    internal static class ObjectId
    {
        /// <summary>
        /// The public key of the device.
        /// </summary>
        public readonly static string PublicKey = "1.3.6.1.4.1.11.2.4.3.20.4.0";

        /// <summary>
        /// The Wireless Infrastructure mode of the device.
        /// </summary>
        public readonly static string InfrastructureMode = "1.2.840.10036.1.1.1.10.1";

        /// <summary>
        /// The Wireless SSID.
        /// </summary>
        public readonly static string SSIDName = "1.3.6.1.4.1.11.2.4.3.20.5.0";

        /// <summary>
        /// The Wireless WEP Protocol type OID of the device.
        /// </summary>
        public readonly static string WEPProtocolType = "1.2.840.10036.1.5.1.1.1";

        /// <summary>
        /// The WPA version OID of the device.
        /// </summary>
        public readonly static string WPAVersion = "1.3.6.1.4.1.11.2.4.3.20.42.0";

        /// <summary>
        /// The WPA Encryption OID of the device.
        /// </summary>
        public readonly static string WPAEncryption = "1.3.6.1.4.1.11.2.4.3.20.28.0";

        /// <summary>
        /// The Wireless Algorithm type OID 
        /// </summary>
        public readonly static string AlgorithmType = "1.2.840.10036.1.2.1.3.1.1";

        /// <summary>
        /// The Wireless WEp Key OID 
        /// </summary>
        public readonly static string WEPKey = "1.3.6.1.4.1.11.2.4.3.20.7.1.2.0";

        /// <summary>
        /// The Wireless WPA Pass phrase OID 
        /// </summary>
        public readonly static string WPAPassphrase = "1.3.6.1.4.1.11.2.4.3.20.36.0";

        /// <summary>
        /// The Wireless WPA enabled state OID 
        /// </summary>
        public readonly static string WPAEnabled = "1.2.840.10036.7.3.1.3.2";

        public readonly static string WepKeyIndex = "1.2.840.10036.1.5.1.2.1";

        public readonly static string AuthenticationServer = "1.3.6.1.4.1.11.2.4.3.20.1.0";

        public readonly static string WirelessStatus = "1.3.6.1.4.1.11.2.4.3.7.47.0";

        public readonly static string MacAddress = "1.3.6.1.4.1.11.2.4.3.1.23.0";
    }
}
