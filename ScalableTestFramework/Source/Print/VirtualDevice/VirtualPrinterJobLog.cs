using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.VirtualDevice
{
    /// <summary>
    /// Class for logging data about a print job consumed by a virtual printer.
    /// </summary>
    public sealed class VirtualPrinterJobLog : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "VirtualPrinterJob";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(VirtualPrinterJobId);

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPrinterJobLog" /> class.
        /// </summary>
        /// <param name="printJobClientId">The print job client identifier.</param>
        public VirtualPrinterJobLog(Guid printJobClientId)
        {
            VirtualPrinterJobId = SequentialGuid.NewGuid();
            PrintJobClientId = printJobClientId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPrinterJobLog" /> class.
        /// </summary>
        /// <param name="jobInfo">The <see cref="VirtualPrinterJobInfo" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="jobInfo" /> is null.</exception>
        public VirtualPrinterJobLog(VirtualPrinterJobInfo jobInfo)
        {
            if (jobInfo == null)
            {
                throw new ArgumentNullException(nameof(jobInfo));
            }

            VirtualPrinterJobId = SequentialGuid.NewGuid();
            PrintJobClientId = UniqueFile.ExtractId(jobInfo.PjlHeader.JobName);
            PjlJobName = jobInfo.PjlHeader.JobName;
            PjlLanguage = jobInfo.PjlHeader.Language;
            FirstByteReceivedDateTime = jobInfo.FirstByteReceived;
            LastByteReceivedDateTime = jobInfo.LastByteReceived;
            BytesReceived = jobInfo.BytesReceived;
        }

        /// <summary>
        /// Gets the virtual printer job identifier.
        /// </summary>
        [DataLogProperty]
        public Guid VirtualPrinterJobId { get; private set; }

        /// <summary>
        /// Gets or sets the unique identifier for the client print job.
        /// </summary>
        [DataLogProperty]
        public Guid PrintJobClientId { get; set; }

        /// <summary>
        /// Gets or sets the PJL job name.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string PjlJobName { get; set; }

        /// <summary>
        /// Gets or sets the PJL language.
        /// </summary>
        [DataLogProperty]
        public string PjlLanguage { get; set; }

        /// <summary>
        /// Gets or sets the time when the first byte was received.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? FirstByteReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the last byte was received.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? LastByteReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes received.
        /// </summary>
        [DataLogProperty]
        public long BytesReceived { get; set; }
    }
}
