using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;

namespace HP.ScalableTest.PluginSupport.Connectivity
{

    /// <summary>
    /// Represents the Encryption strength for 802.1X Authentication
    /// </summary>
    public enum EncryptionStrengths
    {
        //**Note:
        /// <summary>
        /// Low
        /// </summary>
        [EnumValue("EAP_ENCRSTR_LOW||EAP_ENCRSTR_LOW||EAP_ENCRSTR_LOW||low")]
        Low,

        /// <summary>
        /// Medium
        /// </summary>
        [EnumValue("EAP_ENCRSTR_MED||EAP_ENCRSTR_MED||EAP_ENCRSTR_MED||medium")]
        Medium,

        /// <summary>
        /// High
        /// </summary>
        [EnumValue("EAP_ENCRSTR_HIGH||EAP_ENCRSTR_HIGH||EAP_ENCRSTR_HIGH||high")]
        High
    }

    /// <summary>
    /// Fall back option when the authentication is failed.
    /// </summary>
    public enum FallbackOption
    {
        /// <summary>
        /// Connect the network
        /// </summary>
        [EnumValue("ON_FAIL_CONNECT")]
        Connect,
        /// <summary>
        /// Block the network
        /// </summary>
        [EnumValue("ON_FAIL_BLOCK")]
        Block
    }

    /// <summary>
    /// The configuration details required for configuring 802.1x authentication on the printer.
    /// </summary>
    public struct Dot1XConfigurationDetails
    {
        /// <summary>
        /// <see cref="AuthenticationMode"/>
        /// </summary>
        public AuthenticationMode AuthenticationProtocol { get; set; }

        /// <summary>
        /// The user name required for 802.1x authentication.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password required for 802.1x authentication.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// <see cref="EncryptionStrengths"/>
        /// </summary>
        public EncryptionStrengths EncryptionStrength { get; set; }

        /// <summary>
        /// True to re-authenticate on apply
        /// </summary>
        public bool ReAuthenticate { get; set; }

        /// <summary>
        /// <see cref="FallbackOption"/>
        /// </summary>
        public FallbackOption FailSafe { get; set; }

        /// <summary>
        /// The fully qualified name of the server
        /// </summary>
        public string ServerId { get; set; }

        /// <summary>
        /// Server ID exact match
        /// </summary>
        public bool RequireServerIdMatch { get; set; }

        public override string ToString()
        {
            return "Authentication Mode: {0}, EncryptionStrength: {1}, User Name: {2}, Password: {3}, Server Id: {4}, Fail Safe: {5}, ReAuthenticate On Apply: {6}"
                .FormatWith(AuthenticationProtocol, EncryptionStrength, UserName, Password, ServerId, FailSafe, ReAuthenticate ? "Enabled" : "Disabled");
        }
    }
}
