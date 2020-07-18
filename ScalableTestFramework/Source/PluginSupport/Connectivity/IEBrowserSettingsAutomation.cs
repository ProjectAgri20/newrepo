using System;
using Microsoft.Win32;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Represents the supported Secure Protocols for the IE v11
    /// </summary>
    public enum SecureProtocol
    {
        [EnumValue("2728")]
        All,
        [EnumValue("8")]
        SSL20,
        [EnumValue("32")]
        SSL30,
        [EnumValue("128")]
        TLS10,
        [EnumValue("512")]
        TLS11,
        [EnumValue("2048")]
        TLS12,
        [EnumValue("2688")]
        AllTLS,
        [EnumValue("40")]
        AllSSL
    }

    /// <summary>
    /// Operates (Sets/Gets) the IE browser settings
    /// </summary>
    public class IEBrowserSettingsAutomation
    {
        /// <summary>
        /// Sets the Secure protocol settings on the IE browser
        /// </summary>
        /// <param name="protocols">Secure Protocol setting</param>
        public static void SetSecureProtocols(SecureProtocol protocols)
        {
            int decimalValue = Convert.ToInt16(Enum<SecureProtocol>.Value(protocols));

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "SecureProtocols", decimalValue, RegistryValueKind.DWord);
        }


    }
}
