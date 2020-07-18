using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.SecurityConfiguration
{
    [DataContract]
    public class SecurityConfigurationActivityData
    {

        /// <summary>
        /// Sets Security Type
        /// </summary>
        [DataMember]
        public SecurityConfigurationType SecurityType { get; set; }

        /// <summary>
        /// Sets Encryption Strength value
        /// </summary>
        [DataMember]
        public string EncryptionStrength { get; set; }

        /// <summary>
        /// Enable/Disable SNMPV1/V3
        /// </summary>
        [DataMember]
        public string SnmpV1V2 { get; set; }

        /// <summary>
        /// Enable/Disable SNMPV3 Enhanced
        /// </summary>
        [DataMember]
        public string SnmpV3Enhanced { get; set; }

        /// <summary>
        /// Sets Access Control IPv4
        /// </summary>
        [DataMember]
        public string AccessControlIpv4 { get; set; }

        /// <summary>
        /// Sets Mask Address
        /// </summary>
        [DataMember]
        public string Mask { get; set; }

        /// <summary>
        /// sets 802.1x Authentication Password
        /// </summary>
        [DataMember]
        public string AuthenticationPassword { get; set; }

        /// <summary>
        /// Sets SNMPv3 Username
        /// </summary>
        [DataMember]
        public string Snmpv3UserName { get; set; }

        /// <summary>
        /// sets Authentication Passphrase Protocol
        /// </summary>
        [DataMember]
        public string AuthenticationPassphraseProtocol { get; set; }

        /// <summary>
        /// sets Privacy Passphrase Protocol
        /// </summary>
        [DataMember]
        public string PrivacyPassphraseProtocol { get; set; }

        /// <summary>
        /// Sets SNMPV1/V2 read only access
        /// </summary>
        [DataMember]
        public string Snmpv1v2ReadOnlyAccess { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfigurationActivityData"/> class.
        /// </summary>
        public SecurityConfigurationActivityData()
        {

            EncryptionStrength = string.Empty;
            SnmpV1V2 = string.Empty;
            SnmpV3Enhanced = string.Empty;
            AccessControlIpv4 = string.Empty;
            Mask = string.Empty;
            AuthenticationPassword = string.Empty;
            Snmpv3UserName = string.Empty;
            AuthenticationPassphraseProtocol = string.Empty;
            PrivacyPassphraseProtocol = string.Empty;
            Snmpv1v2ReadOnlyAccess = string.Empty;

        }
    }

    /// <summary>
    /// Defines the Security Configuration Type
    /// </summary>
    public enum SecurityConfigurationType
    {
        Basic = 0,
        Enhanced,
        Custom
    }

    /// <summary>
    /// SecurityConfigurationSetting options
    /// </summary>
    public class SecurityConfigurationSetting
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}