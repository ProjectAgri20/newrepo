using System.Collections.ObjectModel;
using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch
{
    /// <summary>
    /// Link speed of the port.
    /// </summary>
    public enum LinkSpeed
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Half duplex 10 Mbps.
        /// </summary>
        [EnumValue("10-half")]
        HalfDuplex10Mbps,
        /// <summary>
        /// Full duplex 10 Mbps.
        /// </summary>
        [EnumValue("10-full")]
        FullDuplex10Mbps,
        /// <summary>
        /// Half duplex 100 Mbps.
        /// </summary>
        [EnumValue("100-half")]
        HalfDuplex100Mbps,
        /// <summary>
        /// Full duplex 100 Mbps.
        /// </summary>
        [EnumValue("100-full")]
        FullDuplex100Mbps,
        /// <summary>
        /// Full duplex 1000 Mbps.
        /// </summary>
        [EnumValue("1000-full")]
        FullDuplex1000Mbps,
        /// <summary>
        /// Use Auto Negotiation for speed and duplex mode.
        /// </summary>
        [EnumValue("auto")]
        Auto,
        /// <summary>
        /// 10 Mbps, Use Auto Negotiation for duplex mode.
        /// </summary>
        [EnumValue("auto-10")]
        Auto10Mbps,
        /// <summary>
        /// 100 Mbps, Use Auto Negotiation for duplex mode.
        /// </summary>
        [EnumValue("auto-100")]
        Auto100Mbps,
        /// <summary>
        /// 1000 Mbps, Use Auto Negotiation for duplex mode.
        /// </summary>
        [EnumValue("auto-1000")]
        Auto1000Mbps
    }

    /// <summary>
    /// Interface for using different functionalities exposed for Network Switch
    /// </summary>
    public interface INetworkSwitch
    {
        /// <summary>
        /// Gets Model name
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Provides the Manufacturer name for Network switch
        /// </summary>
        string Make { get; }

        /// <summary>
        /// Provides the IP Address of the Switch
        /// </summary>
        IPAddress IPAddress { get; }

        /// <summary>
        /// List of VirtualLan's configured on the Network Switch
        /// If IP Address or Subnet Mask is not configured for a VLAN, 255.255.255.255 is returned as default
        /// </summary>
        /// <returns>List of VirtualLan's configured on the Network Switch</returns>
        Collection<VirtualLAN> GetAvailableVirtualLans();

        /// <summary>
        /// Gets the configured pots for the VLAN number.
        /// </summary>
        /// <param name="vlanIdentifier">The VLAN number.</param>
        /// <returns>The VLAN ports configured.</returns>
        Collection<int> GetVlanPorts(int vlanIdentifier);

        /// <summary>
        /// Enable a port on Network Switch
        /// </summary>
        /// <param name="portNumber">Port Number to be enabled</param>
        /// <returns>true if enabled successfully, false otherwise</returns>
        bool EnablePort(int portNumber);

        /// <summary>
        /// Disable a port on Network Switch
        /// </summary>
        /// <param name="portNumber">Port Number to be disabled</param>
        /// <returns>true if disabled successfully, false otherwise</returns>
        bool DisablePort(int portNumber);

        /// <summary>
        /// Check whether a port is disabled on Network Switch
        /// </summary>
        /// <param name="portNumber">Port Number which needs to be disabled</param>
        /// <returns>true if port is disabled, false otherwise</returns>
        bool IsPortDisabled(int portNumber);

        /// <summary>
        /// Enables 802.1X Authentication on the port
        /// </summary>
        /// <param name="portNumber">Port Number to be enabled</param>
        /// <param name="timeout">Amount of time to wait after disposing the telnet connection</param>
        void EnableAuthenticatorPort(int portNumber, int timeout = 1000);

        /// <summary>
        /// Disables 802.1X Authentication on the port
        /// </summary>
        /// <param name="portNumber">Port Number to be disabled</param>
        void DisableAuthenticatorPort(int portNumber);

        /// <summary>
        /// Get the log data of Network Switch
        /// </summary>
        /// <returns>Logs of Network Switch</returns>
        string GetLog();

        /// <summary>
        /// Changes the port to the destination VLAN
        /// </summary>
        /// <param name="portNumber">Port number to be changed to destination VLAN</param>
        /// <param name="destinationVirtualLanIdentifer">The destination VLAN identifier</param>
        /// <returns>bool if success, else false</returns>
        bool ChangeVirtualLan(int portNumber, int destinationVirtualLanIdentifer);

        /// <summary>
        /// Set the link speed of the port to the specified value.
        /// </summary>
        /// <param name="portNumber">The port number to be configured.</param>
        /// <param name="linkSpeed"><see cref="LinkSpeed"/></param>
        /// <returns>True if the link speed is set, else false.</returns>
        bool SetLinkSpeed(int portNumber, LinkSpeed linkSpeed);

        /// <summary>
        /// Configure radius server on the switch.
        /// </summary>
        /// <param name="radiusServer">IP address of the radius server.</param>
        /// <param name="sharedSecret">Shared secret for the radius server configuration.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        bool ConfigureRadiusServer(IPAddress radiusServer, string sharedSecret);

        /// <summary>
        /// DeConfigure/Remove radius server details from the switch.
        /// </summary>
        /// <param name="radiusServer">IP address of the radius server.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        bool DeconfigureRadisuServer(IPAddress radiusServer);

        /// <summary>
        /// Gets all the radius servers configured on the switch.
        /// </summary>
        /// <returns>All the available radius servers from the switch.</returns>
        Collection<IPAddress> GetRadiusServers();

        /// <summary>
        /// DeConfigure/Remove all radius server details from the switch.
        /// </summary>
        /// <returns>True if the configuration is successful, else false.</returns>
        bool DeConfigureAllRadiusServer();
    }
}
