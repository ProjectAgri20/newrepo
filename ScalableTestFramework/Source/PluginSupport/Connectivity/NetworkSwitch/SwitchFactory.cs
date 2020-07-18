using System;
using System.Net;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch
{
    /// <summary>
    /// Factory class to create a Network Switch object
    /// </summary>    
    public static class SwitchFactory
    {
        /// <summary>
        /// Enumerator for different Network Switch
        /// </summary>
        private enum SwitchType
        {
            None,
            HPProCurve,
            Cisco
        }

        /// <summary>
        /// Initialize an <see cref=" INetworkSwitch"/> object
        /// </summary>
        /// <param name="address">IP Address of the Switch</param>
        /// <returns>Initializes a new instance of <see cref="INetworkSwitch"/> object</returns>        
        public static INetworkSwitch Create(IPAddress address)
        {
            SwitchType switchType = GetSwitchType(address);

            INetworkSwitch networkSwitch = null;

            switch (switchType)
            {
                case SwitchType.HPProCurve:
                    Logger.LogDebug("HP ProCurve Switch instance is created with IP {0}".FormatWith(address.ToString()));
                    networkSwitch = new HPProCurveSwitch(address);
                    break;

                case SwitchType.Cisco:
                    Logger.LogInfo("Cisco Network Switch is not implemented");
                    break;

                case SwitchType.None:
                    Logger.LogInfo("Network Switch manufacturer can't be identified or it is not implemented");
                    break;
            }

            return networkSwitch;
        }

        /// <summary>
        /// Get the SwitchType using IP Address of the Network Switch
        /// </summary>
        /// <param name="ipAddress">IP Address of Network Switch</param>
        /// <returns><see cref="SwitchType"/></returns>
        private static SwitchType GetSwitchType(IPAddress ipAddress)
        {
            TelnetIpc telnet = new TelnetIpc(ipAddress.ToString(), 23);
            SwitchType switchType = SwitchType.None;

            try
            {
                telnet.Connect();
                telnet.SendLine(" ");
                Thread.Sleep(1000);
                // This command is tested currently only for HP ProCurve switch, need to check for other switches how it behaves
                telnet.SendLine("show config");
                Thread.Sleep(3000);
                string data = telnet.ReceiveUntilMatch("$");

                // HP manufactured switch can contain any of the strings.
                // ProCurve, Hewlett-Packard, Hewlett Packard
                if (data.Contains("ProCurve", StringComparison.CurrentCultureIgnoreCase) || data.Contains("Hewlett-Packard", StringComparison.CurrentCultureIgnoreCase) || data.Contains("Hewlett Packard", StringComparison.CurrentCultureIgnoreCase))
                {
                    switchType = SwitchType.HPProCurve;
                }
            }
            finally
            {
                telnet.Dispose();
                Thread.Sleep(1000);
            }

            return switchType;
        }
    }
}
