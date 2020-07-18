using System;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Event arguments for In-box driver lookup
    /// </summary>
    public class DriverStoreScanningEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the total number of INF files to be scanned
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of scans completed
        /// </summary>
        public int Complete { get; set; }

        /// <summary>
        /// Gets or sets the driver name.
        /// </summary>
        public string Driver { get; set; }
    }
}
