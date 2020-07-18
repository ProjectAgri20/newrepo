using System;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    public class FtpPrintingEventArgs : EventArgs
    {
        /// <summary>
        /// Total bytes of FTP job
        /// </summary>
        public long TotalBytes { get; set; }

        /// <summary>
        /// Data bytes sent for FTP job
        /// </summary>
        public long SentBytes { get; set; }

        /// <summary>
        /// Abort FTP job
        /// </summary>
        public bool Abort { get; set; }
    }
}
