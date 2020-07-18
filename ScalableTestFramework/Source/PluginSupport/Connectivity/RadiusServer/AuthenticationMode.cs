using System;

namespace HP.ScalableTest.PluginSupport.Connectivity.RadiusServer
{
    /// <summary>
    /// Represents the Authentication modes for 802.1X Authentication
    /// </summary>
    [Flags]
    public enum AuthenticationMode
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents EAP-TLS 
        /// </summary>
        [EnumValue("0D000000000000000000000000000000")]
        EAPTLS = 1,

        /// <summary>
        /// Represents EAP-PEAP
        /// </summary>
        [EnumValue("19000000000000000000000000000000")]
        PEAP = 2,

        /// <summary>
        /// Represents MSCHAPV2
        /// </summary>
        [EnumValue("1A000000000000000000000000000000")]
        MSCHAPV2 = 4,

        /// <summary>
        /// Represents LEAP
        /// </summary>        
        LEAP = 8,
    }
}
