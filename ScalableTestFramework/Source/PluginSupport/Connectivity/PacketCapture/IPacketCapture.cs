using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.PacketCapture
{
    /// <summary>
    /// Packet Capture interface
    /// </summary>
    [ServiceContract]
    public interface IPacketCapture
    {
        /// <summary>
        /// Starts capturing network packets
        /// </summary>
        /// <param name="filter">Filter to be applied while capturing</param>
        /// <param name="packetLocation">Packets file name</param>
        /// <param name="count">Number of packets to be captured, zero means capture indefinitely</param>
        /// <returns>Returns unique identifier</returns>
        [OperationContract]
        string Start(string filter, string packetLocation, int count);

        /// <summary>
        /// Starts capturing network packets
        /// Note: 1. packetName will be post fixed with .pcap
        /// 2. Packet storage location will be sessionId\packetName
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <param name="testId">Packet file name</param>
        /// <returns>Unique identifier</returns>
        [OperationContract]
        string TestCapture(string sessionId, string testId);

        /// <summary>
        /// Stops capturing
        /// </summary>
        /// <param name="guid">Unique identifier which is coming from the start method</param>
        /// <returns>Returns packet details</returns>
        [OperationContract]
        PacketDetails Stop(string guid);

        /// <summary>
        /// Gets list of available live network devices
        /// </summary>
        /// <returns>Returns available Network Devices</returns>
        [OperationContract]
        Collection<NetworkDevice> GetDevices();

        /// <summary>
        /// Sets the device to capture network packets
        /// </summary>
        /// <param name="device">Device to be set</param>
        [OperationContract]
        void SetDevice(NetworkDevice device);

        /// <summary>
        /// Validates the packets for the existence of specified search strings.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="errorMessage">Error messages to be sent back from the service.</param>
        /// <param name="searchValues">The values to be searched in the packets.</param>
        /// <returns>True if all the search values are present in the string, else true.</returns>
        [OperationContract]
        bool Validate(string packetsPath, string filter, ref string errorMessage, params string[] searchValues);

        /// <summary>
        /// Gets the line containing the specified searchValue from the packets. 
        /// If multiple values are matching the search string, '|' separated values will be returned.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="searchValue">The values to be searched in the captured packets.</param>
        /// <returns>The string contains the specified information.</returns>
        [OperationContract]
        string GetPacketData(string packetsPath, string filter, string searchValue);
    }
}
