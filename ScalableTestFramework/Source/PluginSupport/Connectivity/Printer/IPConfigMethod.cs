
using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Represents the IP Config Methods of the Printer
    /// EnumValue represent Telnet values
    /// Values represent SNMP values
    /// </summary>
    public enum IPConfigMethod
    {
        // Note : The EnumValue attribute represents the possible alternate value for IP Config method on EWS page / Telnet

        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// The DHCP Configuration
        /// </summary>
        [EnumValue("DHCP")]
        DHCP = 5,
        /// <summary>
        /// The BOOTP Configuration
        /// </summary>
        [EnumValue("BOOTP")]
        BOOTP = 4,
        /// <summary>
        /// The Manual IP Configuration
        /// </summary>
        [EnumValue("USER SPECIFIED")]
        Manual = 1,

        /// <summary>
        /// The Auto IP Configuration
        /// </summary>
        [EnumValue("Auto IP")]
        AUTOIP = 10
    }

    /// <summary>
    /// Represents the default type when BOOTP/DHCP/RARP servers are not available
    /// </summary>
    public enum DefaultIPType
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// The Legacy Default IP
        /// </summary>
        [EnumValue("legacyip")]
        LegacyIP,

        /// <summary>
        /// The Auto IP
        /// </summary>
        [EnumValue("Autoipxx")]
        AutoIP
    }

    public static class IPConfigMethodUtils
    {
        /// <summary>
        /// Get IP Configured on Printer
        /// Based on Product Family, IP Configuration value configured on EWS/ Telnet/ SNMP differs.
        /// These difference are handled in this function.
        /// </summary>
        /// <param name="configuredValue">Value configured on Telnet/ EWS/ SNMP</param>
        /// <returns></returns>
        public static IPConfigMethod GetIPConfigMethod(string configuredValue)
        {
            /*                      Telnet                      |   Snmp: Provides only Integer values              |               EWS
             * ********************************************************************************************************************************************************
             * VEP |  DHCP, BOOTP, Auto IP, USER SPECICIFIED    |   5 (DHCP), 4 (BOOTP), 10 (AUTO IP), 3 (MANUAL)   |   Dhcp, Bootp, Autoip, Manual
             * --------------------------------------------------------------------------------------------------------------------------------------------------------
             * TPS |  DHCP, BOOTP, AUTO_IP, MANUAL              |   5 (DHCP), 4 (BOOTP), 10 (AUTO IP), 3 (MANUAL)   |   PrefDHCP, PrefBootP, PrefAutoIP, PrefManual
             * --------------------------------------------------------------------------------------------------------------------------------------------------------
             * LFP |  DHCP, BOOTP, Auto IP, USER SPECICIFIED    |   5 (DHCP), 4 (BOOTP), 10 (AUTO IP), 3 (MANUAL)   |   Dhcp, Bootp, Autoip, Manual
             **********************************************************************************************************************************************************
             */

            IPConfigMethod ipConfigMethod = IPConfigMethod.None;

            if (configuredValue.Contains("AUTO", StringComparison.CurrentCultureIgnoreCase) || "10" == configuredValue)
            {
                ipConfigMethod = IPConfigMethod.AUTOIP;
            }
            else if (configuredValue.Contains("DHCP", StringComparison.CurrentCultureIgnoreCase) || "5" == configuredValue)
            {
                ipConfigMethod = IPConfigMethod.DHCP;
            }
            else if (configuredValue.Contains("BOOTP", StringComparison.CurrentCultureIgnoreCase) || "4" == configuredValue)
            {
                ipConfigMethod = IPConfigMethod.BOOTP;
            }
            else if (configuredValue.Contains("MANUAL", StringComparison.CurrentCultureIgnoreCase) ||
                    configuredValue.Contains("USER SPECIFIED", StringComparison.CurrentCultureIgnoreCase) || "3" == configuredValue)
            {
                ipConfigMethod = IPConfigMethod.Manual;
            }
            else
            {
                Framework.Logger.LogInfo("Unknown IP Configuration method with value: {0}".FormatWith(configuredValue));
            }

            return ipConfigMethod;
        }
    }
}
