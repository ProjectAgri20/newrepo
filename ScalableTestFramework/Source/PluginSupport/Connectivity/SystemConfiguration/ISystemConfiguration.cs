using System;
using System.Net;
using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration
{
    /// <summary>
    /// Contains various methods related to the system configuration.
    /// </summary>
    [ServiceContract]
    public interface ISystemConfiguration
    {
        /// <summary>
        /// Sets the system time to the specified date time value.
        /// </summary>
        /// <param name="dateTime">Time to be set.</param>
        /// <param name="resetTime">The amount of time to reset the changed time.</param>
        /// <returns>true if the date time is set, else false.</returns>
        [OperationContract]
        bool SetSystemTime(DateTime dateTime, TimeSpan? resetTime = null);

        /// <summary>
        /// Gets the current date time value.
        /// </summary>
        /// <returns>The current system time.</returns>
        [OperationContract]
        DateTime GetSystemTime();

        /// <summary>
        /// Set static IP address for the client machine for the specified network.
        /// </summary>
        /// <param name="networkName">The name of the network to be modified.</param>
        /// <param name="ipAddress">The new IP address to be set.</param>
        /// <param name="subnetMask">The subnet mask to be set.</param>
        /// <param name="gateWay">The gateway to be set.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        bool SetStaticIPAddress(string networkName, IPAddress ipAddress, IPAddress subnetMask, IPAddress gateWay = null);

        /// <summary>
        /// Set DHCP address for the client machine for the specified network.
        /// </summary>
        /// <param name="networkName">The name of the network to be modified.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        bool SetDhcpIPAddress(string networkName);

        /// <summary>
        /// Gets the fully qualified name of the machine
        /// </summary>
        /// <returns>The fully qualified name of the machine.</returns>
        [OperationContract]
        string GetFullyQualifiedname();

        /// <summary>
        /// Gets the host name of the machine
        /// </summary>
        /// <returns>The host name of the machine.</returns>
        [OperationContract]
        string GetHostName();

    }
}
