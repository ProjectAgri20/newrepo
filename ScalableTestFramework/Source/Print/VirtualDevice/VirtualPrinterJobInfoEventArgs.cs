using System;

namespace HP.ScalableTest.Print.VirtualDevice
{
    /// <summary>
    /// Event args containing a <see cref="VirtualPrinterJobInfo" /> object.
    /// </summary>
    public sealed class VirtualPrinterJobInfoEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the virtual printer job data.
        /// </summary>
        public VirtualPrinterJobInfo Job { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPrinterJobInfoEventArgs" /> class.
        /// </summary>
        /// <param name="job">The job.</param>
        public VirtualPrinterJobInfoEventArgs(VirtualPrinterJobInfo job)
        {
            Job = job;
        }
    }
}
