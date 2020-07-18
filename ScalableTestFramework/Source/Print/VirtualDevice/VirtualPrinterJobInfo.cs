using System;

namespace HP.ScalableTest.Print.VirtualDevice
{
    /// <summary>
    /// Information about a print job processed by a virtual printer.
    /// </summary>
    public sealed class VirtualPrinterJobInfo
    {
        /// <summary>
        /// Gets the PJL header for the job.
        /// </summary>
        public PjlHeader PjlHeader { get; }

        /// <summary>
        /// Gets or sets the time when the first byte was received.
        /// </summary>
        public DateTimeOffset FirstByteReceived { get; set; }

        /// <summary>
        /// Gets or sets the time when the last byte was received.
        /// </summary>
        public DateTimeOffset LastByteReceived { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes received.
        /// </summary>
        public long BytesReceived { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPrinterJobInfo" /> class.
        /// </summary>
        /// <param name="pjlHeader">The PJL header.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pjlHeader" /> is null.</exception>
        public VirtualPrinterJobInfo(PjlHeader pjlHeader)
        {
            PjlHeader = pjlHeader ?? throw new ArgumentNullException(nameof(pjlHeader));
        }
    }
}
