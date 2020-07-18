using System;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// Class for logging data about a scan job sent from a device.
    /// </summary>
    public sealed class DigitalSendJobInputLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "DigitalSendJobInput";

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendJobInputLog" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="filePrefix">The scan file prefix.</param>
        /// <param name="scanType">The scan type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="executionData" /> is null.</exception>
        public DigitalSendJobInputLog(PluginExecutionData executionData, ScanFilePrefix filePrefix, string scanType)
            : base(executionData)
        {
            FilePrefix = filePrefix.ToString();
            ScanType = scanType;
            DeviceId = null;
            Sender = null;
            JobEndStatus = null;
            PageCount = 0;
            DestinationCount = 1;
            Ocr = null;
        }

        /// <summary>
        /// Gets or sets the scan file prefix.
        /// </summary>
        /// <value>The file prefix.</value>
        [DataLogProperty(MaxLength = 255)]
        public string FilePrefix { get; set; }

        /// <summary>
        /// Gets or sets the type of the scan.
        /// </summary>
        /// <value>The type of the scan.</value>
        [DataLogProperty]
        public string ScanType { get; set; }

        /// <summary>
        /// Gets or sets the device ID.
        /// </summary>
        /// <value>The device identifier.</value>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        [DataLogProperty]
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the job end status.
        /// </summary>
        /// <value>The job end status.</value>
        [DataLogProperty]
        public string JobEndStatus { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        [DataLogProperty]
        public short PageCount { get; set; }

        /// <summary>
        /// Gets or sets the destination count.
        /// </summary>
        /// <value>The destination count.</value>
        [DataLogProperty]
        public short DestinationCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this job used OCR.
        /// </summary>
        /// <value><c>null</c> if OCR status is unknown, <c>true</c> if OCR was used; otherwise, <c>false</c>.</value>
        [DataLogProperty]
        public bool? Ocr { get; set; }
    }
}
