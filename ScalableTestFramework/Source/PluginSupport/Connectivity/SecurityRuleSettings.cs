using System;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    #region Public Enumerators

    /// <summary>
    /// Firewall Rule Action 
    /// </summary>
    public enum DefaultAction
    {
        /// <summary>
        /// Allow Action
        /// </summary>
        [EnumValue("ALLOW")]
        Allow,
        /// <summary>
        /// Drop Action
        /// </summary>
        [EnumValue("DROP")]
        Drop
    }

    /// <summary>
    /// Address template
    /// </summary>
    public enum DefaultAddressTemplates
    {
        /// <summary>
        /// Custom Address Template
        /// </summary>
        Custom,
        /// <summary>
        /// All IP Addresses Address Template
        /// </summary>
        [EnumValue("All IP Addresses||All IP Addresses||All IP Addresses||All IP Addresses")]
        AllIPAddresses,
        /// <summary>
        /// All IPv4  Address Template
        /// </summary>
        [EnumValue("All IPv4 Addresses||All IPv4 Addresses||All IPv4 Addresses||All IPv4 Addresses")]
        AllIPv4Addresses,
        /// <summary>
        /// All IPv6  Address Template
        /// </summary>
        [EnumValue("All IPv6 Addresses||All IPv6 Addresses||All IPv6 Addresses||All IPv6 Addresses")]
        AllIPv6Addresses,
        /// <summary>
        /// All IPv6 Link Local  Address Template
        /// </summary>
        [EnumValue("All link local IPv6||All Link-Local IPv6 Addresses||All link local IPv6||All IPv6 Link Local Addresses")]
        AllIPv6LinkLocal,
        /// <summary>
        /// All IPv6 Non Link Local Address Template
        /// </summary>
        [EnumValue("All non link local IPv6||All Non Link-Local IPv6 Addresses||All non link local IPv6||All IPv6 Non Link Local Addresses 1")]
        AllIPv6NonLinkLocal,
        /// <summary>
        /// All IPv6 Non Link Local Address Template 1
        /// </summary>
        [EnumValue("All IPv6 non link local addresses 1||All IPv6 Non-Link Local Addresses 1||All IPv6 Non link local Addresses 1||All IPv6 Non Link Local Addresses 2")]
        AllIPv6NonLinkLocalAddresses2,
        /// <summary>
        /// All IPv6 Non Link Local Address Template 2
        /// </summary>
        [EnumValue("All IPv6 non link local addresses 2||All IPv6 Non-Link Local Addresses 2||All IPv6 Non link local Addresses 2||All IPv6 Non Link Local Addresses 3")]
        AllIPv6NonLinkLocalAddresses3,
        /// <summary>
        /// All IPv6 Non Link Local Address Template 3
        /// </summary>
        [EnumValue("All IPv6 non link local addresses 3||All IPv6 Non-Link Local Addresses 3||All IPv6 Non link local Addresses 3||All IPv6 Non Link Local Addresses 4")]
        AllIPv6NonLinkLocalAddresses4,

        //Added additional default templates that are applicable for Ink
    }

    /// <summary>
    /// Custom Address Template Type
    /// </summary>
    public enum CustomAddressTemplateType
    {
        /// <summary>
        /// Custom IP Address
        /// </summary>
        IPAddress,
        /// <summary>
        /// Custom Predefined Addresses
        /// </summary>
        PredefinedAddresses,
        /// <summary>
        /// Custom IP Address Range
        /// </summary>
        IPAddressRange,
        /// <summary>
        /// Custom IP Address Prefix
        /// </summary>
        IPAddressPrefix
    }

    /// <summary>
    /// Default Service Template
    /// </summary>
    public enum DefaultServiceTemplates
    {
        /// <summary>
        /// Custom Service Template
        /// </summary>
        Custom,
        /// <summary>
        /// All Service Template
        /// </summary>
        [EnumValue("All Services||All Services||All Services||All Services")]
        AllServices,
        /// <summary>
        /// All Print Service Template
        /// </summary>
        [EnumValue("All Print Services||All Print Services||All Print Services||All Print Services")]
        AllPrintServices,
        /// <summary>
        /// All Management Service Template
        /// </summary>
        [EnumValue("All Management Services||All Management Services||All Management Services||All Management Services")]
        AllManagementServices,
        /// <summary>
        /// All Digital Send Service Template
        /// </summary>
        [EnumValue("All Digital Send Services||||||All Digital Send Services")]
        AllDigitalSendServices,
        /// <summary>
        /// All Discovery Service Template
        /// </summary>
        [EnumValue("All Discovery Services||All Discovery Services||All Discovery Services||All Discovery Services")]
        AllDiscoveryServices,
        /// <summary>
        /// All Web Service Template
        /// </summary>
        [EnumValue("||All Web Services Print|Microsoft Web Services||")]
        AllWebServicesPrint,
        /// <summary>
        /// All Cloud Service Template
        /// </summary>
        [EnumValue("||Cloud Services||")]
        CloudServices
    }

    /// <summary>
    /// IP sec/ Firewall Action
    /// </summary>
    public enum IPsecFirewallAction
    {
        /// <summary>
        /// Allow Rule for Firewall
        /// </summary>
        AllowTraffic,
        /// <summary>
        /// Drop Rule for Firewall
        /// </summary>
        DropTraffic,
        /// <summary>
        /// IP Security Rule option
        /// </summary>
        ProtectedWithIPsec
    }

    /// <summary>
    /// Authentication/ Security type
    /// </summary>
    public enum SecurityKeyType
    {
        /// <summary>
        /// Dynamic Security Type
        /// </summary>
        Dynamic, // It is also referred as Internet Key Exchange
                 /// <summary>
                 /// Manual Security Type
                 /// </summary>
        Manual
    }

    /// <summary>
    /// Version for Dynamic keys
    /// </summary>
    public enum IKEVersion
    {
        /// <summary>
        /// IKE Version 1
        /// </summary>
        [EnumValue("v1||||||IKEv1")]
        IKEv1,
        /// <summary>
        /// IKE Version 2
        /// </summary>
        [EnumValue("v2||||||IKEv2")]
        IKEv2
    }

    /// <summary>
    /// IKE Strengths
    /// </summary>
    public enum IKESecurityStrengths
    {
        /// <summary>
        /// IKE High Security Strengths
        /// </summary>
        [EnumValue("1||||||High interoperability/Low security")]
        HighInteroperabilityLowsecurity,
        /// <summary>
        /// IKE Medium Security Strengths
        /// </summary>
        [EnumValue("2||||||Medium interoperability/Medium security")]
        MediumInteroperabilityMediumsecurity,
        /// <summary>
        /// IKE Low Security Strengths
        /// </summary>
        [EnumValue("3||||||Low interoperability/High security")]
        LowInteroperabilityHighsecurity,
        /// <summary>
        /// IKE Advanced High Security Strengths
        /// </summary>
        [EnumValue("4||||||High Security (with Advanced Features)")]
        HighSecurityWithAdvancedFeatures,
        /// <summary>
        /// IKE Custom Security Strengths
        /// </summary>
        [EnumValue("5||||||custom")]
        Custom
    }

    /// <summary>
    /// IKE Authentication Types
    /// </summary>
    public enum IKEAAuthenticationTypes
    {
        /// <summary>
        /// Pres Shared Key Authentication Type
        /// </summary>
        PreSharedKey, // PSK
                      /// <summary>
                      /// Certificate Authentication Type
                      /// </summary>
        Certificates,
        /// <summary>
        /// Kerberos Authentication Type
        /// </summary>
        Kerberos
    }

    /// <summary>
    /// Diffie Hellman Group Cryptography parameter
    /// </summary>
    [Flags]
    public enum DiffieHellmanGroups
    {
        /// <summary>
        /// DH2
        /// </summary>
        DH1 = 1,
        /// <summary>
        /// DH2
        /// </summary>
        DH2 = 2,
        /// <summary>
        /// DH5
        /// </summary>
        DH5 = 4,
        /// <summary>
        /// DH14
        /// </summary>
        DH14 = 8,
        /// <summary>
        /// DH15
        /// </summary>
        DH15 = 16,
        /// <summary>
        /// DH16
        /// </summary>
        DH16 = 32,
        /// <summary>
        /// DH17
        /// </summary>
        DH17 = 64,
        /// <summary>
        /// DH18
        /// </summary>
        DH18 = 128,
        /// <summary>
        /// DH19
        /// </summary>
        DH19 = 256,
        /// <summary>
        /// DH20
        /// </summary>
        DH20 = 512,
        /// <summary>
        /// DH21
        /// </summary>
        DH21 = 1024,
        /// <summary>
        /// DH22
        /// </summary>
        DH22 = 2048,
        /// <summary>
        /// DH23
        /// </summary>
        DH23 = 4096,
        /// <summary>
        /// DH24
        /// </summary>
        DH24 = 8192
    }

    /// <summary>
    /// Encryption Algorithm
    /// </summary>
    [Flags]
    public enum Encryptions
    {
        /// <summary>
        /// DES Encryption
        /// </summary>
        [EnumValue("des")]
        DES = 1,
        /// <summary>
        /// DES3 Encryption
        /// </summary>
        [EnumValue("3des")]
        DES3 = 2,
        /// <summary>
        /// AES128 Encryption
        /// </summary>
        [EnumValue("aes128")]
        AES128 = 4,
        /// <summary>
        /// AES192 Encryption
        /// </summary>
        [EnumValue("aes192")]
        AES192 = 8,
        /// <summary>
        /// AES256 Encryption
        /// </summary>
        [EnumValue("aes256")]
        AES256 = 16,
        /// <summary>
        /// No Encryption
        /// </summary>
        None = 32
    }

    /// <summary>
    /// Authentication Algorithm
    /// </summary>
    [Flags]
    public enum Authentications
    {
        /// <summary>
        /// MD5 Authentication
        /// </summary>
        [EnumValue("md5")]
        MD5 = 1,
        /// <summary>
        /// SHA1 Authentication
        /// </summary>
        [EnumValue("sha1")]
        SHA1 = 2,
        /// <summary>
        /// SHA256 Authentication
        /// </summary>
        [EnumValue("sha256")]
        SHA256 = 4,
        /// <summary>
        /// SHA384 Authentication
        /// </summary>
        [EnumValue("sha384")]
        SHA384 = 8,
        /// <summary>
        /// SHA512 Authentication
        /// </summary>
        [EnumValue("sha512")]
        SHA512 = 16,
        /// <summary>
        /// AESXCBC Authentication
        /// </summary>
        [EnumValue("aesxbc")]
        AESXCBC = 32,
        /// <summary>
        /// No Authentication
        /// </summary>
        None = 32
    }

    /// <summary>
    /// Encapsulation Type
    /// </summary>
    public enum EncapsulationType
    {
        /// <summary>
        /// Transport Encapsulation Type
        /// </summary>
        Transport,
        /// <summary>
        /// Tunnel Encapsulation Type
        /// </summary>
        Tunnel
    }

    /// <summary>
    /// Identity Authentication
    /// </summary>
    public enum IdentityType
    {
        /// <summary>
        /// Distinguished Name
        /// </summary>
        [EnumValue("DN")]
        DistinguishedName,
        /// <summary>
        /// FQDN
        /// </summary>
        [EnumValue("FQDN")]
        FQDN,
        /// <summary>
        /// E-Mail
        /// </summary>
        [EnumValue("MAIL")]
        EMail,
        /// <summary>
        /// IP Address
        /// </summary>
        [EnumValue("IPSEC_IPADDR")]
        IPAddress,
        /// <summary>
        /// Key ID
        /// </summary>
        [EnumValue("KEYID")]
        KeyID
    }

    /// <summary>
    /// Kerberos Encryption Type
    /// </summary>
    public enum KerberosEncryptionType
    {
        /// <summary>
        /// DESCBCMD5 Kerberos Encryption Type
        /// </summary>
        [EnumValue("DESCBCMD5")]
        DESCBCMD5,
        /// <summary>
        /// AES128SHA1 Kerberos Encryption Type
        /// </summary>
        [EnumValue("AES128")]
        AES128SHA1,
        /// <summary>
        /// AES256SHA1 Kerberos Encryption Type
        /// </summary>
        [EnumValue("AES256")]
        AES256SHA1
    }

    /// <summary>
    /// Custom Service Protocol Type
    /// </summary>
    public enum ServiceProtocolType
    {
        /// <summary>
        /// TCP Service Protocol Type
        /// </summary>
        [EnumValue("TCP")]
        TCP,
        /// <summary>
        /// UDP Service Protocol Type
        /// </summary>
        [EnumValue("UDP")]
        UDP,
        /// <summary>
        /// ICMPv4 Service Protocol Type
        /// </summary>
        [EnumValue("ICMPv4")]
        ICMPv4,
        /// <summary>
        /// ICMPv6 Service Protocol Type
        /// </summary>
        [EnumValue("ICMPv6")]
        ICMPv6,
        /// <summary>
        /// IGMP Service Protocol Type
        /// </summary>
        [EnumValue("")]
        IGMP
    }

    /// <summary>
    /// Custom Service Type
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// Printer Service Type
        /// </summary>
        [EnumValue("LOCALSRV||LOCALSRV||LOCALSRV||local")]
        Printer,
        /// <summary>
        /// Remote/ Local Machine Service Type
        /// </summary>
        [EnumValue("REMOTESRV||LOCALSRV||LOCALSRV||remote")]
        Remote
    }

    #endregion

    #region Structure

    /// <summary>
    /// Custom Service structure
    /// </summary>
    [Serializable]
    public struct Service
    {
        /// <summary>
        /// Custom Service Name
        /// </summary>
        public string Name;
        /// <summary>
        /// Custom Service Protocol Type
        /// </summary>
        public ServiceProtocolType Protocol;
        /// <summary>
        /// Custom Service Type
        /// </summary>
        public ServiceType ServiceType;
        /// <summary>
        /// Custom Printer Port No
        /// </summary>
        public string PrinterPort;
        /// <summary>
        /// Custom Remote Port No
        /// </summary>
        public string RemotePort;
        /// <summary>
        /// Is Default Custom service or Manage Service
        /// </summary>
        public bool IsDefault;
        /// <summary>
        /// ICMP type value
        /// </summary>
        public string IcmpType;
    }

    #endregion

    /// <summary>
    /// Security Rule Settings contains all configuration required to create a IPsec/ Firewall rule
    /// </summary>
    [Serializable]
    public class SecurityRuleSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SecurityRuleSettings()
        {

        }

        /// <summary>
        /// Creates Security rule settings with name, address, service, IPsec templates with action.
        /// </summary>
        /// <param name="name">Name of the rule</param>
        /// <param name="addressTemplate">Address Template settings</param>
        /// <param name="serviceTemplate">Service Template settings</param>
        /// <param name="action">IPsec/Firewall Action</param>
        /// <param name="ipsecTemplate">IPsec template settings</param>
        public SecurityRuleSettings(string name, AddressTemplateSettings addressTemplate, ServiceTemplateSettings serviceTemplate, IPsecFirewallAction action, IPsecTemplateSettings ipsecTemplate, DefaultAction defaultAction = DefaultAction.Drop)
        {
            Name = name;
            AddressTemplate = addressTemplate;
            ServiceTemplate = serviceTemplate;
            Action = action;
            IPsecTemplate = ipsecTemplate;
            DefaultAction = defaultAction;
        }

        /// <summary>
        /// Address Template Settings
        /// </summary>
        public AddressTemplateSettings AddressTemplate { get; set; }
        /// <summary>
        /// Service Template Settings
        /// </summary>
        public ServiceTemplateSettings ServiceTemplate { get; set; }
        /// <summary>
        /// IP Sec/ Firewall Rule Action
        /// </summary>
        public IPsecFirewallAction Action { get; set; }
        /// <summary>
        /// IP Security Template Settings
        /// </summary>
        public IPsecTemplateSettings IPsecTemplate { get; set; }
        /// <summary>
        /// Rule Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// IP Sec/ Firewall Rule default Action
        /// </summary>
        public DefaultAction DefaultAction { get; set; }
        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n {1} \n\n {2} \n\n {3} \n\n" + "\n ## Default Address: {4} \n ## Default Service: {5} \n ## Action: \t  {6} \n",
                                             Name, AddressTemplate != null ? AddressTemplate.ToString() : "Not Applicable", ServiceTemplate != null ? ServiceTemplate.ToString() : "Not Applicable", IPsecTemplate != null ? IPsecTemplate.ToString() : "Not Applicable", "All IP Addresses", "All Services", DefaultAction);



            //return AddressTemplate.ToString() + "\n" + ServiceTemplate.ToString() + "\n"+ IPsecTemplate != null ? IPsecTemplate.ToString() : "Not Applicable";
        }
    }

    /// <summary>
    /// Address template settings contains configurations required for creating Address template
    /// </summary>
    [Serializable]
    public class AddressTemplateSettings
    {
        public const string REPORT_INFO_MARK = "[I]:: ::";
        public const string HTML_TAB = "----";

        /// <summary>
        /// Default constructor
        /// </summary>
        string Tab = "-----";
        public AddressTemplateSettings()
        {

        }

        /// <summary>
        /// Creates Address template settings with template type
        /// </summary>
        /// <param name="defaultTemplate">Template Type</param>
        public AddressTemplateSettings(DefaultAddressTemplates defaultTemplate)
        {
            DefaultTemplate = defaultTemplate;
        }

        /// <summary>
        /// Creates Address template settings with template type, name, custom and addresses
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="customTemplateType">Customer template type</param>
        /// <param name="localAddress">Local Address</param>
        /// <param name="remoteAddress">Remote Address</param>
        public AddressTemplateSettings(string name, CustomAddressTemplateType customTemplateType, string localAddress, string remoteAddress)
        {
            DefaultTemplate = DefaultAddressTemplates.Custom;
            Name = name;
            CustomTemplateType = customTemplateType;
            LocalAddress = localAddress;
            RemoteAddress = remoteAddress;
        }

        /// <summary>
        /// Default Address Templates available
        /// </summary>
        public DefaultAddressTemplates DefaultTemplate { get; set; }
        /// <summary>
        /// Custom Address Template Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Custom Address Template Type
        /// </summary>
        public CustomAddressTemplateType CustomTemplateType { get; set; }
        /// <summary>
        /// Custom Local Address
        /// </summary>
        public string LocalAddress { get; set; }
        /// <summary>
        /// Custom Remote Address
        /// </summary>
        public string RemoteAddress { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            if (DefaultTemplate == DefaultAddressTemplates.Custom)
            {
                return String.Format(REPORT_INFO_MARK + HTML_TAB + "************************ Address Template ************************ \n Template Type  :  {0} (Custom) \n ## Custom Template Type :  {1} \n" + Tab + Tab + "Local Address/Range  :  {2}; \n" + Tab + Tab + "Remote Address/Range :  {3}",
                    Name, CustomTemplateType, LocalAddress, RemoteAddress);
            }
            else
            {
                return string.Format(REPORT_INFO_MARK + HTML_TAB + "************************ Address Template ************************ \n Template Type  :  {0} (Default) ", DefaultTemplate.ToString());
            }
        }
    }

    /// <summary>
    /// Service template settings contains configurations required for creating Service template
    /// </summary>
    [Serializable]
    public class ServiceTemplateSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        string Tab = "-----";
        public ServiceTemplateSettings()
        {

        }

        /// <summary>
        /// Creates Service Template settings with template type
        /// </summary>
        /// <param name="defaultTemplate"></param>
        public ServiceTemplateSettings(DefaultServiceTemplates defaultTemplate)
        {
            DefaultTemplate = defaultTemplate;
        }

        /// <summary>
        /// Creates Service template settings with template type, name and services
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="services">List of services</param>
        public ServiceTemplateSettings(string name, Collection<Service> services)
        {
            DefaultTemplate = DefaultServiceTemplates.Custom;
            Name = name;
            Services = services;
        }

        /// <summary>
        /// Default Service Templates
        /// </summary>
        public DefaultServiceTemplates DefaultTemplate { get; set; }
        /// <summary>
        /// Custom Service Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Custom Service Details
        /// </summary>
        public Collection<Service> Services { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            string value = null;

            if (DefaultTemplate != DefaultServiceTemplates.Custom)
            {
                return String.Format("************************ Service Template ************************ \n Template Type :  {0} (Default)  ", DefaultTemplate);
            }
            else
            {
                for (int data = 0; data < Services.Count; data++)
                {
                    value += Services[data].Name +
                    "\n" + Tab + Tab + " Local Port  :  " + Services[data].PrinterPort + "\n" + Tab + Tab + " Remote Port :  " + Services[data].RemotePort;
                }
                return String.Format("************************ Service Template ************************ \n Template Type : {0} (Custom)  \n " + "## Services : {1} \n", Name, value);
            }
        }
    }

    /// <summary>
    /// IPsec template settings contains configurations required for create IPsec template
    /// </summary>
    [Serializable]
    public class IPsecTemplateSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public IPsecTemplateSettings()
        {

        }

        /// <summary>
        /// Creates IP security template settings with name, key type and key settings.
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="keyType">Security Key Type</param>
        /// <param name="dynamicKeysSettings">Dynamic Key Settings</param>
        /// <param name="manualKeysSettings">Manual Key Settings</param>
        public IPsecTemplateSettings(string name, SecurityKeyType keyType, DynamicKeysSettings dynamicKeysSettings, ManualKeysSettings manualKeysSettings)
        {
            Name = name;
            KeyType = keyType;
            DynamicKeysSettings = dynamicKeysSettings;
            ManualKeysSettings = manualKeysSettings;
        }

        /// <summary>
        /// Creates IPsecurity template settings with name and dynamic key settings
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="dynamicKeysSettings">Dynamic key settings: <see cref="DynamicKeysSettings"/></param>
        public IPsecTemplateSettings(string name, DynamicKeysSettings dynamicKeysSettings)
        {
            Name = name;
            KeyType = SecurityKeyType.Dynamic;
            DynamicKeysSettings = dynamicKeysSettings;
        }

        /// <summary>
        /// Creates IPSecurity template settings with name and manual key settings
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="manualKeySettings">Manual key settings: <see cref="ManualKeysSettings"/></param>
        public IPsecTemplateSettings(string name, ManualKeysSettings manualKeySettings)
        {
            Name = name;
            KeyType = SecurityKeyType.Manual;
            ManualKeysSettings = manualKeySettings;
        }

        /// <summary>
        /// IP Security Template Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Security Type: Dynamic/ Manual
        /// </summary>
        public SecurityKeyType KeyType { get; set; }
        /// <summary>
        /// Dynamic Security Details
        /// </summary>
        public DynamicKeysSettings DynamicKeysSettings { get; set; }
        /// <summary>
        /// Manual Security Details
        /// </summary>
        public ManualKeysSettings ManualKeysSettings { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("************************ IpSec Template ************************\n\n\t ## Template Key Type : {1} \n\n\t ## Dynamic Keys Settings : {2} \n\n\t ## Manual Keys Settings : {3}", Name, KeyType,
                                  DynamicKeysSettings != null ? DynamicKeysSettings.ToString() : "Not Applicable",
                                  ManualKeysSettings != null ? ManualKeysSettings.ToString() : "Not Applicable");
        }
    }

    /// <summary>
    /// Phase 1 settings for IPsec rule
    /// </summary>
    [Serializable]
    public class IKEPhase1Settings
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        string Tab = "-----";
        public IKEPhase1Settings()
        {

        }

        /// <summary>
        /// IKE Phase 1 Settings with default values
        /// </summary>
        /// <param name="group"><see cref="DiffieHellmanGroups"/></param>
        /// <param name="encryption"><see cref="Encryptions"/></param>
        /// <param name="authentication"><see cref="Authentications"/></param>
        /// <param name="saLifeTime">SA Lifetime in seconds</param>
        public IKEPhase1Settings(DiffieHellmanGroups group, Encryptions encryption, Authentications authentication, long saLifeTime)
        {
            DiffieHellmanGroup = group;
            Encryption = encryption;
            Authentication = authentication;
            SALifeTime = saLifeTime;
        }

        /// <summary>
        /// Diffie Hellman Group
        /// </summary>
        public DiffieHellmanGroups DiffieHellmanGroup { get; set; }
        /// <summary>
        /// Encryption Algorithms
        /// </summary>
        public Encryptions Encryption { get; set; }
        /// <summary>
        /// Authentication Algorithms
        /// </summary>
        public Authentications Authentication { get; set; }
        /// <summary>
        /// Lifetime
        /// </summary>
        public long SALifeTime { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n" + "" + Tab + Tab + "" + " Negotiation\t\t : {0} \n" + Tab + Tab + " Diffie-Hellman Groups   : {1} \n" + Tab + Tab + " Encryption\t\t : {2} \n" + Tab + Tab + " Authentication \t\t: {3} \n" + Tab + Tab + " SALifeTime \t\t: {4}", "Main Mode", DiffieHellmanGroup.ToString(), Encryption.ToString(), Authentication.ToString(), SALifeTime);
        }
    }

    /// <summary>
    /// Phase 2 settings for IPsec rule
    /// </summary>
    [Serializable]
    public class IKEPhase2Settings
    {
        string Tab = "-----";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public IKEPhase2Settings()
        {

        }

        /// <summary>
        /// IKE Phase 2 settings with ESP or ESP and AH Cryptographic parameter
        /// For only ESP configuration: provide ESP Authentication and ESP Encryption , set <param name="both"/> to false
        /// For ESP and AH configuration: provide ESP ENcryption and AH authentication, set <param name="both"/> to true
        /// Note: AdvancedIKESettings configuration needs to be handled by the caller
        /// </summary>
        /// <param name="encapsulation"><see cref="EncapsulationType"/></param>
        /// <param name="authentication"><see cref="Authentications"/></param>
        /// <param name="encryption"><see cref="Encryptions"/></param>
        /// <param name="saLifetime">SA Lifetime in seconds</param>
        /// <param name="saSize">SA Lifetime in data(KB)</param>
        /// <param name="both">true to use combination of ESP Encryption and AH Authentication, false to use ESP ENcryption and ESP Authentication</param>
        public IKEPhase2Settings(EncapsulationType encapsulation, Authentications authentication, Encryptions encryption, long saLifetime, long saSize, bool both, string tunnelLocalAddress = null, string tunnelRemoteAddress = null)
        {
            Encapsulation = encapsulation;
            TunnelLocalAddress = tunnelLocalAddress;
            TunnelRemoteAddress = tunnelRemoteAddress;
            ESPEnable = true;
            ESPEncryption = encryption;
            SALifeTime = saLifetime;
            SASize = saSize;

            if (both)
            {
                AHEnable = true;
                AHAuthentication = authentication;
                ESPAuthentication = Authentications.None;
            }
            else
            {
                AHEnable = false;
                ESPAuthentication = authentication;
                AHAuthentication = Authentications.None;
            }
        }

        /// <summary>
        /// IKE Phase 2 settings with AH Cryptographic parameter
        /// Note: AdvancedIKESettings configuration needs to be handled by the caller
        /// </summary>
        /// <param name="encapsulation"><see cref="EncapsulationType"/></param>
        /// <param name="authentication"><see cref="Authentications"/></param>        
        /// <param name="saLifetime">SA Lifetime in seconds</param>
        /// <param name="saSize">SA Lifetime in data(KB)</param>
        public IKEPhase2Settings(EncapsulationType encapsulation, Authentications authentication, long saLifetime, long saSize)
        {
            Encapsulation = encapsulation;
            AHEnable = true;
            AHAuthentication = authentication;
            SALifeTime = saLifetime;
            SASize = saSize;
            ESPEnable = false;
        }

        /// <summary>
        /// Encapsulation Type
        /// </summary>
        public EncapsulationType Encapsulation { get; set; }
        /// <summary>
        /// Tunnel Local Address
        /// </summary>
        public string TunnelLocalAddress { get; set; }
        /// <summary>
        /// Tunnel Remote Address
        /// </summary>
        public string TunnelRemoteAddress { get; set; }
        /// <summary>
        /// Setting to enable/disable ESP Authentication/Encryption
        /// </summary>
        public bool ESPEnable { get; set; }
        /// <summary>
        /// ESP Encryption
        /// </summary>
        public Encryptions ESPEncryption { get; set; }
        /// <summary>
        /// ESP Authentication
        /// </summary>
        public Authentications ESPAuthentication { get; set; }
        /// <summary>
        /// Settings to enable/disable AH Authentication
        /// </summary>
        public bool AHEnable { get; set; }
        /// <summary>
        /// Ah Authentication
        /// </summary>
        public Authentications AHAuthentication { get; set; }
        /// <summary>
        /// SA Life time in seconds
        /// </summary>
        public long SALifeTime { get; set; }
        /// <summary>
        /// SA Size
        /// </summary>
        public long SASize { get; set; }
        /// <summary>
        /// Advanced IKE Settings
        /// </summary>
        public AdvancedIKESettings AdvancedIKESettings { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n" + Tab + Tab + " Negogiation \t\t : {0} \n" + Tab + Tab + " Encapsulation Type \t: {1} \n" + Tab + Tab + " Tunnel Local Address \t: {2} \n" + Tab + Tab + " Tunnel Remote Address \t: {3} \n" + Tab + Tab + " ESP Option \t\t: {4} \n" + Tab + Tab + " ESP Encryption \t\t: {5} \n" + Tab + Tab + " ESP Authentication \t: {6} \n" + Tab + Tab + " AH Option \t\t: {7} \n" + Tab + Tab + " AH Authentication \t: {8} \n" + Tab + Tab + " SALifeTime in KB \t: {9} \n" + Tab + Tab + " SASize \t\t\t: {10} \n" + Tab + Tab +
                                "Advanced IKE Settings \t: {11}", "Quick Mode", Encapsulation, TunnelLocalAddress != null ? TunnelLocalAddress : "Not Applicable", TunnelRemoteAddress != null ? TunnelRemoteAddress : "Not Applicable", ESPEnable, ESPEncryption.ToString(), ESPAuthentication.ToString(), AHEnable, AHAuthentication.ToString(), SALifeTime, SASize,
                                AdvancedIKESettings != null ? AdvancedIKESettings.ToString() : "Not Applicable");
        }
    }

    /// <summary>
    /// Advanced IKE settings for IPsec rule
    /// </summary>
    [Serializable]
    public class AdvancedIKESettings
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        string Tab = "-----";
        public AdvancedIKESettings()
        {

        }

        /// <summary>
        /// Constructor with all properties
        /// </summary>
        /// <param name="replayDetection">true to enable the option, false to disable</param>
        /// <param name="keyPFS">true to enable the option, false to disable</param>
        /// <param name="group"><see cref="DiffieHellmanGroups"/></param>
        public AdvancedIKESettings(bool replayDetection, bool keyPFS, DiffieHellmanGroups group)
        {
            ReplayDetection = replayDetection;
            KeyPFS = keyPFS;
            DiffieHellmanGroup = group;
        }

        /// <summary>
        /// Replay Detection
        /// </summary>
        public bool ReplayDetection { get; set; }
        /// <summary>
        /// Perfect Forward Secrecy
        /// </summary>
        public bool KeyPFS { get; set; }
        /// <summary>
        /// Diffie Hellman Group
        /// </summary>
        public DiffieHellmanGroups DiffieHellmanGroup { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n" + Tab + Tab + "Replay Detection: {0} \n" + Tab + Tab + "Key Perfect Forward Secrecy (Session PFS):  {1}\n" + Tab + " Diffie-Hellman Groups : {2} \n", ReplayDetection, KeyPFS, DiffieHellmanGroup.ToString());
        }
    }

    /// <summary>
    /// Dynamic Key settings for IPsec rule
    /// </summary>
    [Serializable]
    public class DynamicKeysSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamicKeysSettings()
        {

        }

        /// <summary>
        /// Creates Dynamic Key settings with version, strengths and V1, V2 settings.
        /// </summary>
        /// <param name="ikeVersion">IKE Version</param>
        /// <param name="strengths">Security Strengths</param>
        /// <param name="v1Settings">IKEv1 Settings</param>
        /// <param name="v2Settings">IKEv2 settings</param>
        public DynamicKeysSettings(IKEVersion ikeVersion, IKESecurityStrengths strengths, IKEv1Settings v1Settings, IKEv2Settings v2Settings)
        {
            Version = ikeVersion;
            Strengths = strengths;
            V1Settings = v1Settings;
            V2Settings = v2Settings;
        }

        /// <summary>
        /// Dynamic key settings with IKEVersion1 as default with user provided Strength and IKEv1Settings
        /// </summary>
        /// <param name="strength">IKE Security Strength: <see cref="IKESecurityStrengths"/></param>
        /// <param name="v1Settings">V1 Settings: <see cref="IKEv1Settings"/></param>
        public DynamicKeysSettings(IKESecurityStrengths strength, IKEv1Settings v1Settings)
        {
            Version = IKEVersion.IKEv1;
            Strengths = strength;
            V1Settings = v1Settings;
        }

        /// <summary>
        /// IKE Version
        /// </summary>
        public IKEVersion Version { get; set; }
        /// <summary>
        /// IKE Security Strengths
        /// </summary>
        public IKESecurityStrengths Strengths { get; set; }
        /// <summary>
        /// IKEV1 Security Settings
        /// </summary>
        public IKEv1Settings V1Settings { get; set; }
        /// <summary>
        /// IKEV2 Security Settings
        /// </summary>
        public IKEv2Settings V2Settings { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n\n" + "## Internet Key Exchange Version : {0} \n" + "## IKE Security: {1} \n" + "\n## IKEV1 Settings\t: {2} \n" + "\n\t\t## IKEV2 Settings\t: {3}", Version, Strengths, V1Settings != null ? V1Settings.ToString() : "Not Applicable",
                                  V2Settings != null ? V2Settings.ToString() : "Not Applicable");
        }
    }

    /// <summary>
    /// IkEv1 settings for IPsec rule creation
    /// </summary>
    [Serializable]
    public class IKEv1Settings
    {
        public const string Tab = "-----";
        /// <summary>
        /// Default constructor
        /// </summary>
        public IKEv1Settings()
        {

        }

        /// <summary>
        /// Constructor with PSK Authentication type
        /// Note: This can used when Default Strength is selected
        /// <param name="pskValue">PSK Value to set</param>
        /// </summary>
        public IKEv1Settings(string pskValue)
        {
            AuthenticationTypes = IKEAAuthenticationTypes.PreSharedKey;
            PSKValue = pskValue;
        }

        /// <summary>
        /// Constructor with PSK Authentication type
        /// Note: This can be used when Custom Strength is selected
        /// </summary>
        /// <param name="pskValue">PSK Value to set</param>
        /// <param name="phase1Settings">Phase 1 Settings</param>
        /// <param name="phase2Settings">Phase 2 Settings</param>
        public IKEv1Settings(string pskValue, IKEPhase1Settings phase1Settings, IKEPhase2Settings phase2Settings)
        {
            AuthenticationTypes = IKEAAuthenticationTypes.PreSharedKey;
            PSKValue = pskValue;
            IKEv1Phase1Settings = phase1Settings;
            IKEv1Phase2Settings = phase2Settings;
        }

        /// <summary>
        /// Constructor with Certificate Authentication type
        /// </summary>
        /// <param name="caCertificatePath">CA Certificate File Path</param>
        /// <param name="idCertificatePath">ID Certificate File Path</param>
        /// <param name="idPassword">ID Certificate Password</param>
        public IKEv1Settings(string caCertificatePath, string idCertificatePath, string idPassword)
        {
            AuthenticationTypes = IKEAAuthenticationTypes.Certificates;
            CACertificatePath = caCertificatePath;
            IDCertificatePath = idCertificatePath;
            IDCertificatePassword = idPassword;
        }


        /// <summary>
        /// Constructor with Certificate Authentication type
        /// </summary>
        /// <param name="caCertificatePath">CA Certificate File Path</param>
        /// <param name="idCertificatePath">ID Certificate File Path</param>
        /// <param name="idPassword">ID Certificate Password</param>
        /// <param name="phase1Settings">Phase 1 Settings</param>
        /// <param name="phase2Settings">Phase 2 Settings</param>
        public IKEv1Settings(string caCertificatePath, string idCertificatePath, string idPassword, IKEPhase1Settings phase1Settings, IKEPhase2Settings phase2Settings)
        {
            AuthenticationTypes = IKEAAuthenticationTypes.Certificates;
            CACertificatePath = caCertificatePath;
            IDCertificatePath = idCertificatePath;
            IDCertificatePassword = idPassword;
            IKEv1Phase1Settings = phase1Settings;
            IKEv1Phase2Settings = phase2Settings;
        }

        /// <summary>
        /// Constructor with Kerberos Authentication type
        /// </summary>
        /// <param name="kerberosSettings">Kerberos settings</param>
        public IKEv1Settings(KerberosSettings kerberosSettings)
        {
            AuthenticationTypes = IKEAAuthenticationTypes.Kerberos;
            KerberosSettings = kerberosSettings;
        }

        /// <summary>
        /// IKE Authentication Type 
        /// </summary>
        public IKEAAuthenticationTypes AuthenticationTypes { get; set; }
        /// <summary>
        /// Pre Shared Key Value
        /// </summary>
        public string PSKValue { get; set; }
        /// <summary>
        /// ID Certificate Path
        /// </summary>
        public string IDCertificatePath { get; set; }
        /// <summary>
        /// ID Certificate Password
        /// </summary>
        public string IDCertificatePassword { get; set; }
        /// <summary>
        /// CA Certificate Path
        /// </summary>
        public string CACertificatePath { get; set; }
        /// <summary>
        /// Kerberos Settings
        /// </summary>
        public KerberosSettings KerberosSettings { get; set; }
        /// <summary>
        /// IKE Phase1 Settings
        /// </summary>
        public IKEPhase1Settings IKEv1Phase1Settings { get; set; }
        /// <summary>
        /// IKE Phase2 Settings
        /// </summary>
        public IKEPhase2Settings IKEv1Phase2Settings { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            if (AuthenticationTypes == IKEAAuthenticationTypes.PreSharedKey)
            {
                return String.Format("\n" + Tab + Tab + " Authentication Type  : {0}\n" + Tab + Tab + " PSKValue  \t\t: {1} \n" + "\n" + "## IKEv1 Phase1 Settings \t: {2} \n" + "## IKEv1 Phase2 Settings : {3}",
                                  AuthenticationTypes, PSKValue, IKEv1Phase1Settings != null ? IKEv1Phase1Settings.ToString() : "Not Applicable", IKEv1Phase2Settings != null ? IKEv1Phase2Settings.ToString() : "Not Applicable");
            }
            else if (AuthenticationTypes == IKEAAuthenticationTypes.Certificates)
            {
                return String.Format("\n" + Tab + Tab + " Authentication Type    : {0}\n" + Tab + Tab + " ID Certificate Path  \t: {1} \n" + Tab + Tab + " ID Certificate Password : {2}\n" + Tab + Tab + " CA Certificate Path  \t: {3} \n\n" + "## IKEv1 Phase1 Settings : {4} \n\n" + "## IKEv1 Phase2 Settings : {5}",
                                  AuthenticationTypes, IDCertificatePath, IDCertificatePassword, CACertificatePath, IKEv1Phase1Settings != null ? IKEv1Phase1Settings.ToString() : "Not Applicable", IKEv1Phase2Settings != null ? IKEv1Phase2Settings.ToString() : "Not Applicable");
            }
            else
            {
                return String.Format("\n" + Tab + Tab + " Authentication Type  : {0}\n\t\t\t ## Kerberos Settings \t: {1} \n\n" + "## IKEv1 Phase1 Settings \t: {2} \n\n" + "## IKEv1 Phase2 Settings \t: {3}",
                                  AuthenticationTypes, KerberosSettings != null ? KerberosSettings.ToString() : "Not Applicable", IKEv1Phase1Settings != null ? IKEv1Phase1Settings.ToString() : "Not Applicable",
                                  IKEv1Phase2Settings != null ? IKEv1Phase2Settings.ToString() : "Not Applicable");
            }
        }
    }

    /// <summary>
    /// Kerberos configuration
    /// </summary>
    [Serializable]
    public class KerberosSettings
    {
        /// <summary>
        /// Default Kerberos constructor
        /// </summary>
        string Tab = "-----";
        public KerberosSettings()
        {

        }

        /// <summary>
        /// Import Settings Constructor
        /// </summary>
        /// <param name="importSettings">Import settings</param>
        public KerberosSettings(KerberosImportSettings importSettings)
        {
            IsManual = false;
            Validate = true;
            ImportSettings = importSettings;
        }

        /// <summary>
        /// Manual Settings Constructor
        /// </summary>
        /// <param name="manualSettings">Manual settings</param>
        public KerberosSettings(KerberosManualSettings manualSettings)
        {
            IsManual = true;
            Validate = true;
            ManualSettings = manualSettings;
        }

        /// <summary>
        /// Option to configure manually
        /// </summary>
        public bool IsManual { get; set; }
        /// <summary>
        /// Kerberos Manual Settings
        /// </summary>
        public KerberosManualSettings ManualSettings { get; set; }
        /// <summary>
        /// Kerberos Import Settings
        /// </summary>
        public KerberosImportSettings ImportSettings { get; set; }
        /// <summary>
        /// Option to validate configured settings
        /// </summary>
        public bool Validate { get; set; } // this setting is nothing do with validation, if it is on validate the Kerberos once after confirmation.

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n\t\t\t## Manual Settings \t: {0} \n" + Tab + Tab + " Validate \t: {1} \n" + "## Manual Settings \t: {2} \n" + Tab + Tab + " Import Settings \t: {3}", IsManual, Validate,
                                 ManualSettings != null ? ManualSettings.ToString() : "Not Applicable", ImportSettings != null ? ImportSettings.ToString() : "Not Applicable");
        }
    }

    /// <summary>
    /// Kerberos Import settings
    /// </summary>
    [Serializable]
    public class KerberosImportSettings
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        string Tab = "-----";
        public KerberosImportSettings()
        {

        }

        /// <summary>
        /// Constructor with Config and Keytab file paths
        /// </summary>
        /// <param name="configFilePath">Kerberos Configuration file path</param>
        /// <param name="keytabFilePath">Keytab file path</param>
        public KerberosImportSettings(string configFilePath, string keytabFilePath)
        {
            ConfigurationFilePath = configFilePath;
            KeyTabFilePath = keytabFilePath;
            TimeSyncPeriod = 0;
            SNTPServer = string.Empty;
        }

        /// <summary    
        /// Kerberos configuration File path
        /// </summary>
        public string ConfigurationFilePath { get; set; }
        /// <summary>
        /// Kerberos Key tab File path
        /// </summary>
        public string KeyTabFilePath { get; set; }
        /// <summary>
        /// Time Sync period for settings
        /// </summary>
        public int TimeSyncPeriod { get; set; }
        /// <summary>
        /// SNTP server address
        /// </summary>
        public string SNTPServer { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n" + Tab + Tab + " Configuration FilePath \t: {0}\n" + Tab + Tab + " KeyTab FilePath \t: {1}\n" + Tab + Tab + " TimeSync Period \t: {2}\n" + Tab + Tab + " SNTP Server \t\t: {3} \n", ConfigurationFilePath, KeyTabFilePath, TimeSyncPeriod, SNTPServer);
        }
    }

    /// <summary>
    /// Kerberos Manual settings
    /// </summary>
    [Serializable]
    public class KerberosManualSettings
    {

        /// <summary>
        /// Constructor with KDCServer, PrincipalRealm, Password and EncryptionType
        /// </summary>
        /// <param name="KDCServer">Kerberos Server IP Address</param>
        /// <param name="PrincipalRealm">Username@domainName of the Kerberos Server</param>
        /// <param name="Password">Password of the User</param>
        /// <param name="EncryptionType">KerberosEncryptionType</param>
        string Tab = "-----";
        public KerberosManualSettings(string kdcServer, string principalRealm, string password, KerberosEncryptionType encryptionType)
        {
            KDCServer = kdcServer;
            PrincipalRealm = principalRealm;
            Password = password;
            EncryptionType = encryptionType;
        }

        /// <summary>
        /// KDC Server
        /// </summary>
        public string KDCServer { get; set; }
        /// <summary>
        /// Principal Realm
        /// </summary>
        public string PrincipalRealm { get; set; }
        /// <summary>
        /// Kerberos Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Kerberos Encryption Type
        /// </summary>
        public KerberosEncryptionType EncryptionType { get; set; }
        /// <summary>
        /// Iteration Count
        /// </summary>
        public long IterationCount { get; set; }
        /// <summary>
        /// Key Version number
        /// </summary>
        public short KeyVersionNumber { get; set; }
        /// <summary>
        /// Clock Skew
        /// </summary>
        public int ClockSkew { get; set; }
        /// <summary>
        /// Time sync Period
        /// </summary>
        public int TimeSyncPeriod { get; set; }
        /// <summary>
        /// SNTP Server Address
        /// </summary>
        public string SNTPServer { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n" + Tab + Tab + " KDC Server \t\t: {0}\n" + Tab + Tab + " Principal Realm \t: {1}\n" + Tab + Tab + " Password \t\t: {2}\n" + Tab + Tab + " Encryption Type \t: {3}\n" + Tab + Tab + " Iteration Count \t: {4}\n" + Tab + Tab + " Key Version Number \t: {5}\n" + Tab + Tab + " ClockSkew\t\t: {6}\n" + Tab + Tab + " TimeSync Period \t: {7}\n" + Tab + Tab + " SNTP Server \t\t: {8} \n",
                                  KDCServer, PrincipalRealm, Password, EncryptionType, IterationCount, KeyVersionNumber, ClockSkew, TimeSyncPeriod, SNTPServer);
        }
    }

    /// <summary>
    /// IKEv2 settings for IPsec rule creation
    /// </summary>
    [Serializable]
    public class IKEv2Settings
    {
        // Local Settings
        /// <summary>
        /// Local IKE Authentication Type
        /// </summary>
        string Tab = "-----";
        public IKEAAuthenticationTypes LocalAuthenticationType { get; set; }
        /// <summary>
        /// Local Identity Type
        /// </summary>
        public IdentityType LocalPSKIdentityType { get; set; }
        /// <summary>
        /// Local PSK Identity
        /// </summary>
        public string LocalPSKIdentity { get; set; }
        // Make the PSKKey Type always as ASCII
        /// <summary>
        /// Local PSK Key Value
        /// </summary>
        public string LocalPSKKey { get; set; }
        /// <summary>
        /// Local Certificate Identity Type
        /// </summary>
        public IdentityType LocalCertIdentityType { get; set; }
        /// <summary>
        /// Local ID Certificate Path
        /// </summary>
        public string LocalIDCertificatePath { get; set; }
        /// <summary>
        /// Local IP Certificate Password
        /// </summary>
        public string LocalIDCertificatePassword { get; set; }
        /// <summary>
        /// Local CA Certificate Path
        /// </summary>
        public string LocalCACertificatePath { get; set; }

        // Remote Settings
        /// <summary>
        /// Remote IKE Authentication Type
        /// </summary>
        public IKEAAuthenticationTypes RemoteAuthenticationType { get; set; }
        /// <summary>
        /// Remote Identity Ty[e
        /// </summary>
        public IdentityType RemotePSKIdentityType { get; set; }
        /// <summary>
        /// Remote PSK Identity
        /// </summary>
        public string RemotePSKIdentity { get; set; }
        // Make the PSKKey Type always as ASCII
        /// <summary>
        /// Remote PSK Key
        /// </summary>
        public string RemotePSKKey { get; set; }
        /// <summary>
        /// Remote Certificate Identity Type
        /// </summary>
        public IdentityType RemoteCertIdentityType { get; set; }
        // RemoteCertIdentity property value needs to be same as local Cert Identity which is retrieved from EWS directly
        //public string RemoteCertIdentity { get; set; }
        /// <summary>
        /// IKEv2 Phase 1 Settings
        /// </summary>
        public IKEPhase1Settings IKEv2Phase1Settings { get; set; }
        /// <summary>
        /// IKEv2 Phase 2 Settings
        /// </summary>
        public IKEPhase2Settings IKEv2Phase2Settings { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n" + Tab + Tab + " Local Authentication Type: {0},\n" + Tab + Tab + " Local PSKIdentity Type: {1} \n" + Tab + Tab + " Local PSKIdentity: {2} \n" + Tab + Tab + " Local PSKKey: {3}\n" + Tab + Tab + " Local CertIdentity Type: {4}\n" + Tab + Tab + " Local IDCertificatePath: {5}\n " + Tab + Tab + " Local IDCertificatePassword: {6} " +
                                "\n" + Tab + Tab + " Local CACertificatePath: {7}\n" + Tab + Tab + " Remote Authentication Type: {8} \n" + Tab + Tab + " Remote PSKIdentity Type: {9}\n" + Tab + Tab + " Remote PSKIdentity: {10}\n" + Tab + Tab + " Remote PSKKey: {11}\n" + Tab + Tab + " Remote CertIdentity Type: {12} \n## IKEv2 Phase1 Settings: {13} \n " +
                                "IKEv2 Phase2 Settings: {14} \n", LocalAuthenticationType, LocalPSKIdentityType, LocalPSKIdentity, LocalPSKKey, LocalCertIdentityType, LocalIDCertificatePath, LocalIDCertificatePassword,
                                LocalCACertificatePath, RemoteAuthenticationType, RemotePSKIdentityType, RemotePSKIdentity, RemotePSKKey, RemoteCertIdentityType,
                                IKEv2Phase1Settings != null ? IKEv2Phase1Settings.ToString() : "Not Applicable", IKEv2Phase2Settings != null ? IKEv2Phase2Settings.ToString() : "Not Applicable");
        }

    }

    /// <summary>
    /// Manual Key settings for IPsec rule
    /// </summary>
    [Serializable]
    public class ManualKeysSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ManualKeysSettings()
        {

        }

        /// <summary>
        /// Constructor with basic and advanced manual settings
        /// </summary>
        /// <param name="basicManualKeySettings">Basic Settings</param>
        /// <param name="advancedManualKeysSettings">Advanced Settings</param>
        public ManualKeysSettings(BasicManualKeysSettings basicManualKeySettings, AdvancedManualKeysSettings advancedManualKeysSettings)
        {
            BasicSettings = basicManualKeySettings;
            AdvancedSettings = advancedManualKeysSettings;
        }

        /// <summary>
        /// Basic Manual Key Settings
        /// </summary>
        public BasicManualKeysSettings BasicSettings { get; set; }
        /// <summary>
        /// Advanced Manual Key Settings
        /// </summary>
        public AdvancedManualKeysSettings AdvancedSettings { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n\n\t\t## Basic Settings : {0} \n\n\t\t##Advanced Settings: {1} \n",
                                  BasicSettings != null ? BasicSettings.ToString() : "Not Applicable",
                                  AdvancedSettings != null ? AdvancedSettings.ToString() : "Not Applicable");
        }
    }

    /// <summary>
    /// Basic Manual settings
    /// </summary>
    [Serializable]
    public class BasicManualKeysSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        string Tab = "-----";
        public BasicManualKeysSettings()
        {

        }

        /// <summary>
        /// Constructor with ESP Basic Manual Key Settings
        /// </summary>
        /// <param name="espEncryption"><see cref="Encryptions"/></param>
        /// <param name="espAuthentication"><see cref="Authentications"/></param>
        /// <param name="encapsulationType"><see cref="EncapsulationType"/></param>
        /// <param name="localAddress">Local address</param>
        /// <param name="remoteAddress">Remote address</param>
        public BasicManualKeysSettings(Encryptions espEncryption, Authentications espAuthentication, EncapsulationType encapsulationType, string localAddress = null, string remoteAddress = null)
        {
            AHEnable = false;
            ESPEnable = true;
            ESPEncryption = espEncryption;
            ESPAuthentication = espAuthentication;
            Encapsulation = encapsulationType;
            LocalAddress = localAddress;
            RemoteAddress = remoteAddress;
        }

        /// <summary>
        /// Encapsulation Type
        /// </summary>
        public EncapsulationType Encapsulation { get; set; }
        /// <summary>
        /// Local Address
        /// </summary>
        public string LocalAddress { get; set; }
        /// <summary>
        /// Remote Address
        /// </summary>
        public string RemoteAddress { get; set; }
        /// <summary>
        /// Option to configure ESP Authentication/Encryption
        /// </summary>
        public bool ESPEnable { get; set; }
        /// <summary>
        /// ESP Encryption
        /// </summary>
        public Encryptions ESPEncryption { get; set; }
        /// <summary>
        /// ESP Authentication
        /// </summary>
        public Authentications ESPAuthentication { get; set; }
        /// <summary>
        /// Option to configure AH Authentication
        /// </summary>
        public bool AHEnable { get; set; }
        /// <summary>
        /// AH Authentication
        /// </summary>
        public Authentications AHAuthentication { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n\t\t\t" + Tab + Tab + " Encapsulation Type : {0}\n\t\t\t" + Tab + Tab + " Tunnel Peer Address : \n\t\t\t\t" + Tab + Tab + " Local Address : {1}\n\t\t\t\t" + Tab + Tab + " Remote Address : {2}\n\t\t\t" + Tab + Tab + " Cryptographic Parameters :" + "\n\t\t\t\t" + Tab + Tab + " ESP Option  \t\t: {3}\n\t\t\t\t" + Tab + Tab + " Encryption  \t\t: {4}\n\t\t\t\t" + Tab + Tab + " ESP Authentication \t: {5}\n\t\t\t\t" + Tab + Tab + " AH Option  \t\t: {6}\n\t\t\t\t" + Tab + Tab + " AH Authentication  \t: {7} \n",
                                  Encapsulation, LocalAddress, RemoteAddress, ESPEnable, ESPEncryption, ESPAuthentication, AHEnable, AHAuthentication);
        }
    }

    /// <summary>
    /// Advanced Manual settings
    /// </summary>
    [Serializable]
    public class AdvancedManualKeysSettings
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        string Tab = "-----";
        public AdvancedManualKeysSettings()
        {

        }

        /// <summary>
        /// Constructor with ESP Advanced Manual Key Settings
        /// </summary>
        /// <param name="espIn">ESP In Value</param>
        /// <param name="espOut">ESP Out Value</param>
        /// <param name="encryptionIn">Encryption In Value</param>
        /// <param name="encryptionOut">Encryption Out Value</param>
        /// <param name="authenticationIn">Authentication In Value</param>
        /// <param name="authenticationOut">Authentication Out Value</param>
        public AdvancedManualKeysSettings(int espIn, int espOut, string encryptionIn, string encryptionOut, string authenticationIn, string authenticationOut)
        {
            ESPSPIIn = espIn;
            ESPSPIOut = espOut;
            AHSPIIn = espIn;
            AHSPIOut = espOut;
            EncryptionIn = encryptionIn;
            EncryptionOut = encryptionOut;
            AuthenticationIn = authenticationIn;
            AuthenticationOut = authenticationOut;
        }

        // SPI format is Decimal by default
        /// <summary>
        /// ESP SPI In Value
        /// </summary>
        public int ESPSPIIn { get; set; }
        /// <summary>
        /// ESP SPI Out Value
        /// </summary>
        public int ESPSPIOut { get; set; }
        /// <summary>
        /// AH SPI In Value
        /// </summary>
        public int AHSPIIn { get; set; }
        /// <summary>
        /// AH SPI Out Value
        /// </summary>
        public int AHSPIOut { get; set; }
        // Key format is Hex by default
        /// <summary>
        /// Encryption In Value
        /// </summary>
        public string EncryptionIn { get; set; }
        /// <summary>
        /// Encryption Out Value
        /// </summary>
        public string EncryptionOut { get; set; }
        /// <summary>
        /// Authentication In Value
        /// </summary>
        public string AuthenticationIn { get; set; }
        /// <summary>
        /// Authentication Out Value
        /// </summary>
        public string AuthenticationOut { get; set; }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("\n\t\t\t" + Tab + Tab + " ESP SPI In \t: {0} \n\t\t\t" + Tab + Tab + " ESP SPI Out \t: {1} \n\t\t\t" + Tab + Tab + " AH SPI In \t: {2}, \n\t\t\t" + Tab + Tab + " AH SPI Out \t: {3} \n" + Tab + Tab + " Encryption In \t\t: {4}\n" + Tab + Tab + " Encryption Out \t\t: {5}\n" + Tab + Tab + " Authentication In \t: {6}\n" + Tab + Tab + " Authentication Out \t: {7} \n",
                                  ESPSPIIn, ESPSPIOut, AHSPIIn, AHSPIOut, EncryptionIn, EncryptionOut, AuthenticationIn, AuthenticationOut);
        }
    }
}