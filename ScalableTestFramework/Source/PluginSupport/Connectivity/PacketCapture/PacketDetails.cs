using System;

namespace HP.ScalableTest.PluginSupport.Connectivity.PacketCapture
{
    /// <summary>
    /// Contains Packet Details
    /// </summary>
    public class PacketDetails
    {
        /// <summary>
        /// Gets or Sets Packets file name
        /// </summary>
        public string PacketsLocation { get; set; }

        /// <summary>
        /// Gets or Sets Start Time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or Sets End Time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or Sets Packets Count
        /// </summary>
        public int PacketsCount { get; set; }
    }
}
