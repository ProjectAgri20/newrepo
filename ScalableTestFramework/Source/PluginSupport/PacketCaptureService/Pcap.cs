using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.PacketCapture;
using HP.ScalableTest.Utility;
using PcapDotNet.Core;
using PcapDotNet.Packets;

namespace HP.ScalableTest.PluginSupport.PacketCaptureService
{
    internal class Pcap
    {
        /// <summary>
        /// Current live device to capture network packets
        /// </summary>
        LivePacketDevice _device;

        /// <summary>
        /// List of current packet capture objects
        /// </summary>
        Collection<CurrentPacketCapture> _currentPacketCaptureList;

        /// <summary>
        /// Packets count
        /// </summary>
        int _packetsCounts = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pcap"/> class.
        /// </summary>
        public Pcap()
        {
            _currentPacketCaptureList = new Collection<CurrentPacketCapture>();
        }

        /// <summary>
        /// Gets list of available live network devices
        /// </summary>
        /// <returns></returns>
        public Collection<NetworkDevice> GetDeviceList()
        {
            Collection<NetworkDevice> networkDevices = new Collection<NetworkDevice>();
            // Retrieve the device list from the local machine
            IList<LivePacketDevice> devices = LivePacketDevice.AllLocalMachine;

            if (devices.Count == 0)
            {
                Logger.LogInfo("No interfaces found! Make sure WinPcap is installed.");
                return null;
            }

            foreach (LivePacketDevice livePacketDevice in devices)
            {
                NetworkDevice networkDevice = new NetworkDevice();
                networkDevice.Name = livePacketDevice.Description;
                networkDevice.IPv4Address = GetIPv4Address(livePacketDevice.Addresses);
                networkDevices.Add(networkDevice);
            }

            return networkDevices;
        }

        /// <summary>
        /// Sets the device to capture network packets
        /// </summary>
        /// <param name="device">Device to be set</param>
        public void SetDevice(NetworkDevice device)
        {
            foreach (LivePacketDevice liveDevice in LivePacketDevice.AllLocalMachine)
            {
                if (liveDevice.Description == device.Name && device.IPv4Address.Equals(GetIPv4Address(liveDevice.Addresses)))
                {
                    _device = liveDevice;
                    break;
                }
            }
        }

        /// <summary>
        /// Starts capturing network packets
        /// </summary>
        /// <param name="filter">Filter to be applied while capturing</param>
        /// <param name="packetLocation">Packets file name</param>
        /// <param name="count">Number of packets to be captured, zero means capture indefinitely</param>
        /// <returns>Returns unique identifier</returns>
        public Guid StartCapture(string filter = "", string packetLocation = "", int count = 0)
        {
            try
            {
                // if the device is null then use the first device from the list
                if (_device == null)
                {
                    try
                    {
                        _device = LivePacketDevice.AllLocalMachine[0];
                    }
                    catch (Exception ex)
                    {
                        Logger.LogInfo("Device get exception");
                        Logger.LogDebug("Exception details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                    }
                }

                Logger.LogInfo("Device name: {0}".FormatWith(_device));

                Guid guid = Guid.NewGuid();

                if (string.IsNullOrEmpty(packetLocation))
                {
                    packetLocation = Path.Combine(Path.GetTempPath(), guid.ToString() + ".pcap");
                }

                Logger.LogDebug("Packet Capture Location: {0}".FormatWith(packetLocation));

                // If the directory is not exists create one
                if (!Directory.Exists(Path.GetDirectoryName(packetLocation)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(packetLocation));
                }

                // Open the device, portion of the packet to capture
                // 65536 guarantees that the whole packet will be captured on all the link layers
                PacketCommunicator communicator = _device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000);

                // set the filter
                communicator.SetFilter(filter); // this might throw exception if filter is not properly

                Thread packetCaptureThread = new Thread(new ParameterizedThreadStart(CaptureAsync));

                CurrentPacketCapture currentPacketCapture = new CurrentPacketCapture();
                currentPacketCapture.Communicator = communicator;
                currentPacketCapture.PacketsPath = packetLocation;
                currentPacketCapture.StartTime = DateTime.Now;
                currentPacketCapture.Thread = packetCaptureThread;
                currentPacketCapture.UniqueIdentifier = guid;

                _currentPacketCaptureList.Add(currentPacketCapture);

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("Communicator", communicator);
                parameters.Add("PacketLocation", packetLocation);
                parameters.Add("Count", count);

                packetCaptureThread.IsBackground = true;
                packetCaptureThread.Start(parameters);

                Logger.LogInfo("Started Thread ");

                return guid;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error occurred while capturing the packets: " + ex.Message);
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Stops capturing
        /// </summary>
        /// <param name="guid">Unique identifier which is coming from the start method</param>
        /// <returns></returns>
        public PacketDetails StopCapture(Guid guid)
        {
            try
            {
                CurrentPacketCapture currentPacketCapture = _currentPacketCaptureList.Where(x => x.UniqueIdentifier == guid).FirstOrDefault();

                if (null != currentPacketCapture)
                {
                    currentPacketCapture.Thread.Abort();

                    currentPacketCapture.Communicator.Break();

                    currentPacketCapture.EndTime = DateTime.Now;

                    PacketDetails packetDetails = new PacketDetails();
                    packetDetails.PacketsLocation = currentPacketCapture.PacketsPath;
                    packetDetails.StartTime = currentPacketCapture.StartTime;
                    packetDetails.EndTime = currentPacketCapture.EndTime;
                    packetDetails.PacketsCount = GetPacketsCount(currentPacketCapture.PacketsPath);

                    _currentPacketCaptureList.Remove(currentPacketCapture);

                    return packetDetails;
                }
            }
            catch (Exception e)
            {
                Logger.LogInfo("Error occurred while stopping the capture" + e.Message);
                return null;
            }

            return null;
        }

        /// <summary>
        /// Cleanups the older items from the list
        /// </summary>
        /// <param name="days">Number of older days of packets to cleanup</param>
        public void CleanUp(int days)
        {
            foreach (CurrentPacketCapture item in _currentPacketCaptureList)
            {
                if (item.StartTime.AddDays(days) > DateTime.Now)
                {
                    _currentPacketCaptureList.Remove(item);
                }
            }
        }

        /// <summary>
        /// Captures packets anynchorsly
        /// </summary>
        /// <param name="parameters">Parameters to capture</param>
        private void CaptureAsync(object parameters)
        {
            PacketCommunicator communicator;
            string packetLocation;
            int count;

            Logger.LogInfo("Started Capturing packets asynchronsly");

            // parse the parameters and start the packet capture
            Dictionary<string, object> args = parameters as Dictionary<string, object>;

            object obj;
            args.TryGetValue("Communicator", out obj);
            communicator = (PacketCommunicator)obj;

            args.TryGetValue("PacketLocation", out obj);
            packetLocation = (string)obj;

            args.TryGetValue("Count", out obj);
            count = (int)obj;

            PacketDumpFile dumpFile = communicator.OpenDump(packetLocation);

            communicator.ReceivePackets(count, dumpFile.Dump);
        }

        /// <summary>
        /// Gets IPv4 address from device address list
        /// </summary>
        /// <param name="addresses">Device address list</param>
        /// <returns>IPv4 address</returns>
        private static IPAddress GetIPv4Address(ReadOnlyCollection<DeviceAddress> addresses)
        {
            IPAddress add = null;

            foreach (DeviceAddress address in addresses)
            {
                if (address.Address.Family == SocketAddressFamily.Internet)
                {
                    string s = address.Address.ToString().Replace(SocketAddressFamily.Internet.ToString(), "").Trim();
                    IPAddress.TryParse(s, out add);

                    break;
                }
            }

            return add;
        }

        /// <summary>
        /// Gets number of packets count from the pcap file
        /// </summary>
        /// <param name="packetsPath"></param>
        /// <returns></returns>
        private int GetPacketsCount(string packetsPath)
        {
            // Create the offline device
            OfflinePacketDevice selectedDevice = new OfflinePacketDevice(packetsPath);

            // Open the capture file
            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                //communicator.Mode = PacketCommunicatorMode.Statistics;
                // Read and dispatch packets until EOF is reached
                _packetsCounts = 0;
                communicator.ReceivePackets(0, DispatcherHandler);
            }

            return _packetsCounts;
        }

        /// <summary>
        /// Handler for counting number of packets
        /// </summary>
        /// <param name="packet">Packet</param>
        private void DispatcherHandler(Packet packet)
        {
            _packetsCounts++;
        }

        /// <summary>
        /// Validates the packets for the existence of specified search strings.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="errorMessage">Error Info.</param>
        /// <param name="searchValues">The values to be searched in the packets.</param>
        /// <returns>True if all the search values are present in the string, else true.</returns>
        public bool Validate(string packetsPath, string filter, ref string errorMessage, params string[] searchValues)
        {
            string outputText = ReadPackets(packetsPath, filter);

            if (string.IsNullOrEmpty(outputText))
            {
                errorMessage += "Either the filter is invalid or no packets are captured.{0}".FormatWith(Environment.NewLine);
                return false;
            }

            bool isValuePresent = true;

            foreach (string value in searchValues)
            {
                if (outputText.Contains(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    errorMessage += "The search value '{0}' is present in the packets.{1}".FormatWith(value, Environment.NewLine);
                }
                else
                {
                    errorMessage += "The search value '{0}' is not present in the packets.{1}".FormatWith(value, Environment.NewLine);
                    isValuePresent &= false;
                }
            }

            return isValuePresent;
        }

        /// <summary>
        /// Read the pcap file contents to a string.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <returns>the pcap file contents as string.</returns>
        private static string ReadPackets(string packetsPath, string filter)
        {
            try
            {
                // Apply the specified filter and read the contents of the pcap file.
                // Note: If the filter is invalid no content will be read.
                string arguments = string.Empty;

                if (string.IsNullOrEmpty(filter))
                {
                    arguments = "-V -r \"{0}\"".FormatWith(packetsPath);
                }
                else
                {
                    arguments = "-R \"{0}\" -V -r \"{1}\"".FormatWith(filter, packetsPath);
                }

                return ProcessUtil.Execute("tshark.exe", arguments).StandardOutput;
            }
            catch (Exception ex)
            {
                Logger.LogDebug("Exception: {0}".FormatWith(ex.JoinAllErrorMessages()));
                return string.Empty;
            }
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
            string outputText = ReadPackets(packetsPath, filter);

            if (string.IsNullOrEmpty(outputText))
            {
                Logger.LogInfo("Either the filter is invalid or no packets are captured.");
                return string.Empty;
            }

            // Split the text into lines and find the line containing the specified info
            return string.Join("|", Array.FindAll(outputText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries), x => x.Contains(searchValue)).Select(x => x.Trim(new char[] { '\n', '\r', ' ' })).Distinct());
        }
    }

    /// <summary>
    /// Live packet capture details
    /// </summary>
    class CurrentPacketCapture
    {
        /// <summary>
        /// Gets or Sets packet communicator object
        /// </summary>
        public PacketCommunicator Communicator { get; set; }

        /// <summary>
        /// Gets or Sets Thread
        /// </summary>
        public Thread Thread { get; set; }

        /// <summary>
        /// Gets or Sets Unique Identifier
        /// </summary>
        public Guid UniqueIdentifier { get; set; }

        /// <summary>
        /// Gets or sets Packets Path
        /// </summary>
        public string PacketsPath { get; set; }

        /// <summary>
        /// Gets or Sets Start Time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or Sets End Time
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
