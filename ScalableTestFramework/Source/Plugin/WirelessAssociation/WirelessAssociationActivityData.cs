namespace HP.ScalableTest.Plugin.WirelessAssociation
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Text;

    using HP.ScalableTest.Utility;

    [DataContract]
    public class WirelessAssociationActivityData
    {
        /// <summary>
        /// Gets Authentication list
        /// </summary>
        public static Collection<string> AuthenticationList
        {
            get
            {
                Collection<string> result = new Collection<string>();
                foreach (AuthenticationMode key in EnumUtil.GetValues<AuthenticationMode>())
                {
                    if (((1 << (int)key)) != 0)
                    {
                        result.Add(key.GetDescription());
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the Authentication Type
        /// </summary>
        [DataMember]
        public AuthenticationMode AuthenticationType { get; set; }

        /// <summary>
        /// Gets the Printer Password in Hex
        /// </summary>
        [DataMember]
        public string HexPassphrase { get; set; }

        /// <summary>
        /// Gets the PassPhrase
        /// </summary>
        [DataMember]
        public string Passphrase { get; set; }

        /// <summary>
        ///Gets if the printer needs to be powercycled
        /// </summary>
        [DataMember]
        public bool PowerCycleRequired { get; set; }

        /// <summary>
        /// Gets the SSID
        /// </summary>
        [DataMember]
        public string Ssid { get; set; }

        /// <summary>
        ///Gets Wep Key Index (Redundant)
        /// </summary>
        [DataMember]
        public string WepKeyIndex { get; set; }

        /// <summary>
        /// Function to convert String To Hex
        /// </summary>
        /// <param name="asciiValue">ascii string</param>
        /// <returns></returns>
        public static string StringToHexConversion(string asciiValue)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(asciiValue))
            {
                char[] values = asciiValue.ToCharArray();
                foreach (char ch in values)
                {
                    // Get the integral value of the character.
                    int value = Convert.ToInt32(ch);
                    // Convert the decimal value to a hexadecimal value in string form.
                    result.Append($"{value:X}");
                }
            }

            return result.ToString();
        }
    }

    #region AuthenticationMode

    /// <summary>
    /// Settings for WPA2 Authentication Modes
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// WPA2 with AES
        /// </summary>
        [Description("WPA2 with AES")]
        WPAAES,

        /// <summary>
        /// WPA2 with 64HEX
        /// </summary>
        [Description("WPA2 with 64HEX")]
        WPAHex,

        /// <summary>
        /// WPA2 with AUTO
        /// </summary>
        [Description("WPA2 with AUTO")]
        WPAAuto,

        /// <summary>
        /// Auto with AES
        /// </summary>
        [Description("Auto with AES")]
        AutoAES,

        /// <summary>
        /// Auto with Auto
        /// </summary>
        [Description("Auto with Auto")]
        AutoAuto

        ///// <summary>
        ///// WEPKeyIndex
        ///// </summary>
        //[Description("WEP Key Index")]
        //WEPKeyIndex,

        ///// <summary>
        ///// WEP 64
        ///// </summary>
        //[Description("WEP 64")]
        //WEP64,

        ///// <summary>
        ///// WEP No Security
        ///// </summary>
        //[Description("WEP No Security")]
        //WEPNoSecurity,

        ///// <summary>
        ///// WEP No Security
        ///// </summary>
        //[Description("WEP Infra Mode")]
        //WEPInfraMode,

        ///// <summary>
        ///// WEP 128
        ///// </summary>
        //[Description("WEP 128")]
        //WEP128
    }

    #endregion
}