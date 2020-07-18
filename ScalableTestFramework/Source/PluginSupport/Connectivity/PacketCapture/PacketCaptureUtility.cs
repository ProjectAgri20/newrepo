using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.PacketCapture
{
    /// <summary>
    /// Provides interface to capture and validate wireshark traces.
    /// </summary>
    public class PacketCaptureUtility
    {
        #region Local Variables

        /// <summary>
        /// Collection of Current Captures
        /// </summary>
        private Collection<CaptureDetails> _currentCaptures = null;

        /// <summary>
        /// Collection to maintain process id for each capture
        /// </summary>
        private Dictionary<Guid, int> _processDetails = null;

        #endregion

        #region Constructor

        /// <summary>
        /// <see cref="PacketCaptureUtility"/>
        /// </summary>
        public PacketCaptureUtility()
        {
            _currentCaptures = new Collection<CaptureDetails>();
            _processDetails = new Dictionary<Guid, int>();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Start capture on the network interface corresponding to the subnet specified.
        /// </summary>
        /// <param name="subnetAddress">IP Address of the client where the traces needs to be captured.</param>
        /// <param name="packetsPath">Location to save the packets.</param>
        /// <returns>The UniqueIdentifier to locate the capture details.</returns>
        public Guid StartCapture(IPAddress subnetAddress, string packetsPath = "")
        {
            return StartCapture(GetClientNetworkId(subnetAddress), packetsPath);
        }

        /// <summary>
        /// Start capture on the network interface corresponding to the subnet specified.
        /// </summary>
		/// <param name="packetsPath">Location to save the packets.</param>
		/// <param name="interfaceIdentifier">The network interface Id on which the traces needs to be captured.</param>
        /// <returns>The UniqueIdentifier to locate the capture details.</returns>
		public Guid StartCapture(string interfaceIdentifier = "", string packetsPath = "")
        {
            Thread currentThread = null;
            Guid guid = Guid.NewGuid();

            try
            {
                if (string.IsNullOrEmpty(interfaceIdentifier))
                {
                    interfaceIdentifier = NetworkInterface.GetAllNetworkInterfaces().Where(x => (x.OperationalStatus == OperationalStatus.Up && x.NetworkInterfaceType != NetworkInterfaceType.Loopback)).FirstOrDefault().Id;
                }
                if (string.IsNullOrEmpty(packetsPath))
                {
                    packetsPath = Path.Combine(Path.GetTempPath(), "{0}.pcap".FormatWith(guid.ToString()));
                }

                TraceFactory.Logger.Debug("Packet Capture Location: {0}".FormatWith(packetsPath));

                // If the directory is not exists create one
                if (!Directory.Exists(Path.GetDirectoryName(packetsPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(packetsPath));
                }

                currentThread = new Thread(() => Capture(interfaceIdentifier, packetsPath, guid));

                CaptureDetails details = new CaptureDetails();
                details.UniqueIdentifier = guid;
                details.CurrentThread = currentThread;
                details.packetsLocation = packetsPath;
                details.StartTime = DateTime.Now;

                currentThread.Start();
                _currentCaptures.Add(details);

                return guid;
            }
            catch (Exception ex)
            {
                if (null != currentThread)
                {
                    StopCapture(guid);
                }

                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(ex.JoinAllErrorMessages()));

                return Guid.Empty;
            }
        }

        /// <summary>
        /// Stops the capture process.
        /// </summary>
        /// <param name="guid">The Guid to identify the capture process</param>
        /// <returns>The path where the traces are stored.</returns>
        public string StopCapture(Guid guid)
        {
            try
            {
                CaptureDetails details = _currentCaptures.Where(x => x.UniqueIdentifier == guid).FirstOrDefault();

                if (null == details)
                {
                    TraceFactory.Logger.Info("No packets are found. Either there was an error or the invalid interface id is specified.");
                    return string.Empty;
                }

                details.CurrentThread.Abort();
                details.EndTime = DateTime.Now;

                _currentCaptures.Remove(details);

                return details.packetsLocation;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info(ex.Message);
                return string.Empty;
            }
            finally
            {
                ProcessUtil.Execute("cmd.exe", @"/C Taskkill /PID {0} /F".FormatWith(_processDetails.Where(x => x.Key == guid).FirstOrDefault().Value));
            }
        }

        /// <summary>
        /// Validates the packets for the existence of specified search strings.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="searchValues">The values to be searched in the packets.</param>
        /// <returns>True if all the search values are present in the string, else true.</returns>
        public bool Validate(string packetsPath, string filter, params string[] searchValues)
        {
            string outputText = ReadPackets(packetsPath, filter);

            if (string.IsNullOrEmpty(outputText))
            {
                TraceFactory.Logger.Info("Either the filter is invalid or no packets are captured.{0}".FormatWith(Environment.NewLine));
                return false;
            }

            bool isValuePresent = true;

            foreach (string value in searchValues)
            {
                if (outputText.Contains(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("The search value '{0}' is present in the packets.{1}".FormatWith(value, Environment.NewLine));
                }
                else
                {
                    TraceFactory.Logger.Info("The search value '{0}' is not present in the packets.{1}".FormatWith(value, Environment.NewLine));
                    isValuePresent &= false;
                }
            }

            return isValuePresent;
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
                TraceFactory.Logger.Info("Either the filter is invalid or no packets are captured.");
                return string.Empty;
            }

            // Split the text into lines and find the line containing the specified info
            return string.Join("|", Array.FindAll(outputText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries), x => x.Contains(searchValue)).Select(x => x.Trim(new char[] { '\n', '\r', ' ' })).Distinct());
        }

        #endregion

        #region Private Functions

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
                string tSharkExecutable = string.Empty;

                // First check for the 64 bit version and if it is not found look for x86
                if (File.Exists(@"C:\Program Files\Wireshark\tshark.exe"))
                {
                    tSharkExecutable = @"C:\Program Files\Wireshark\tshark.exe";
                }
                else if (File.Exists(@"C:\Program Files (x86)\Wireshark\tshark.exe"))
                {
                    tSharkExecutable = @"C:\Program Files (x86)\Wireshark\tshark.exe";
                }

                if (string.IsNullOrEmpty(filter))
                {
                    arguments = "-V -r \"{0}\"".FormatWith(packetsPath);
                }
                else
                {
                    arguments = "-R \"{0}\" -V -r \"{1}\"".FormatWith(filter, packetsPath);
                }

                return ProcessUtil.Execute(tSharkExecutable, arguments).StandardOutput;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the client network Id corresponding to a particular server.
        /// </summary>
        /// <param name="subnetAddress">IP Address of the server</param>
        /// <returns>The client network name.</returns>
        private static string GetClientNetworkId(IPAddress subnetAddress)
        {
            // Gets all the non-loop back interfaces which are up.
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces().Where(m => (m.OperationalStatus == OperationalStatus.Up && m.NetworkInterfaceType != NetworkInterfaceType.Loopback)))
            {
                //UnicastIPAddressInformation ip = item.GetIPProperties().UnicastAddresses.Where(x => (x.Address.AddressFamily == AddressFamily.InterNetwork && x.Address.IsInSameSubnet(subnetAddress, subnetAddress.GetSubnetMask()))).FirstOrDefault();
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.IsInSameSubnet(subnetAddress, subnetAddress.GetSubnetMask()))
                    {
                        return item.Id;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Starts packet capture on the specified interface.
        /// </summary>
        /// <param name="networkIdentifier">The network interface on which the packets needs to be captured.</param>
        /// <param name="packetsPath">The path where to store the packets.</param>
        private void Capture(string networkIdentifier, string packetsPath, Guid guid)
        {
            string command = "dumpcap";
            Process captureProcess = new Process
            {
                StartInfo = { FileName = command, Arguments = "-i \"\\Device\\NPF_{0}\" -w \"{1}\"".FormatWith(networkIdentifier, packetsPath), CreateNoWindow = true, UseShellExecute = false },
                EnableRaisingEvents = true
            };

            captureProcess.Start();
            // Maintain details of process id based on Guid. This is required to kill the process when StopCapture is called.
            _processDetails.Add(guid, captureProcess.Id);
        }

        #endregion
    }

    /// <summary>
    /// Represents the packet capture details
    /// </summary>
    public class CaptureDetails
    {
        /// <summary>
        /// The Unique Identifier.
        /// </summary>
        public Guid UniqueIdentifier { get; set; }

        /// <summary>
        /// The packets location.
        /// </summary>
        public string packetsLocation { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// The current thread.
        /// </summary>
        public Thread CurrentThread { get; set; }
    }
}
