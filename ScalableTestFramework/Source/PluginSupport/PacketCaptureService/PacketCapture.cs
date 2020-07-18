using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.PacketCapture;

namespace HP.ScalableTest.PluginSupport.PacketCaptureService
{
    /// <summary>
    /// Packet Capture Service implementation
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class PacketCapture : IPacketCapture
    {
        /// <summary>
        /// network packets instance
        /// </summary>
        Pcap _networkPackets;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCapture"/> class.
        /// </summary>
        public PacketCapture()
        {
            TraceFactory.Logger.Info("New instance created");
            _networkPackets = new Pcap();
        }

        /// <summary>
        /// Starts capturing network packets
        /// </summary>
        /// <param name="filter">Filter to be applied while capturing the packets</param>
        /// <param name="packetLocation">Packets file path</param>
        /// <param name="count">Number of packets to be captured, zero to capture indefinitely</param>
        /// <returns>Unique Identifier</returns>
        public string Start(string filter, string packetLocation, int count)
        {
            // check to see if there is a packets base path configuration in the app.config file
            string basePath = ConfigurationManager.AppSettings["PacketsBaseLocation"];

            TraceFactory.Logger.Info(basePath);

            if (!string.IsNullOrEmpty(packetLocation) && !Path.IsPathRooted(packetLocation))
            {
                if (!string.IsNullOrEmpty(basePath))
                {
                    packetLocation = Path.Combine(basePath, packetLocation);
                }
            }

            TraceFactory.Logger.Info("Started packet capture thru service with parameters filter: '{0}' packet location: '{1}' and count: '{2}'".FormatWith(filter, packetLocation, count));

            return _networkPackets.StartCapture(filter, packetLocation, count).ToString();
        }

        /// <summary>
        /// Starts capturing network packets
        /// Note: 1. <param name="testId"> will be post fixed with .pcap
        /// 2. Packet storage location will be sessionId\testId
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <param name="testId">Packet file name</param>
        /// <returns>Unique identifier</returns>
        public string TestCapture(string sessionId, string testId)
        {
            // check to see if there is a packets base path configuration in the app.config file
            string basePath = ConfigurationManager.AppSettings["PacketsBaseLocation"];
            string packetLocation = Path.Combine(sessionId, "{0}.pcap".FormatWith(testId));

            TraceFactory.Logger.Info(basePath);

            if (!string.IsNullOrEmpty(packetLocation) && !Path.IsPathRooted(packetLocation))
            {
                if (!string.IsNullOrEmpty(basePath))
                {
                    packetLocation = Path.Combine(basePath, packetLocation);
                }
            }

            TraceFactory.Logger.Info("Started packet capture through service at packet location: '{0}'".FormatWith(packetLocation));

            try
            {
                return _networkPackets.StartCapture(packetLocation: packetLocation).ToString();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Start Capture exception.");
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(ex.JoinAllErrorMessages()));
            }

            return string.Empty;
        }

        /// <summary>
        /// Stops capturing network packets
        /// </summary>
        /// <param name="guid">Unique Identifier</param>
        /// <returns>Packets details</returns>
        public PacketDetails Stop(string guid)
        {
            TraceFactory.Logger.Info("Stopped packet capture thru service");
            PacketDetails details = _networkPackets.StopCapture(new Guid(guid));

            return details;
        }

        /// <summary>
        /// Gets the network interface devices
        /// </summary>
        /// <returns></returns>
        public Collection<NetworkDevice> GetDevices()
        {
            return _networkPackets.GetDeviceList();
        }

        /// <summary>
        /// Sets the network device to capture packets
        /// </summary>
        /// <param name="device"></param>
        public void SetDevice(NetworkDevice device)
        {
            _networkPackets.SetDevice(device);
        }

        /// <summary>
        /// Validates the packets for the existence of specified search strings.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="errorMessage">Error information.</param>
        /// <param name="searchValues">The values to be searched in the packets.</param>
        /// <returns>True if all the search values are present in the string, else true.</returns>
        public bool Validate(string packetsPath, string filter, ref string errorMessage, params string[] searchValues)
        {
            return _networkPackets.Validate(packetsPath, filter, ref errorMessage, searchValues);
        }

        /// <summary>
        /// Gets the line containing the specified searchValue from the packets. 
        /// If multiple values are matching the search string, '|' separated values will be returned.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="searchValue">The values to be searched in the captured packets.</param>
        /// <returns>The string contains the specified information.</returns>
        public string GetPacketData(string packetsPath, string filter, string searchValue)
        {
            return _networkPackets.GetPacketData(packetsPath, filter, searchValue);
        }
    }
}
