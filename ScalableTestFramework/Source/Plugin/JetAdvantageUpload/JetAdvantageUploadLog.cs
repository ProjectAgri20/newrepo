using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    public class JetAdvantageUploadLog : ActivityDataLog
    {
        public JetAdvantageUploadLog(PluginExecutionData executionData)
            : base(executionData)
        {
            LoginId = string.Empty;

            DestinationUrl = string.Empty;
            FileName = string.Empty;
            FileType = string.Empty;
            FileSentDateTime = DateTime.MinValue.AddYears(1775);
            FileSizeSentBytes = null;
            FileSizeReceivedBytes = null;
            CompletionDateTime = DateTime.MinValue.AddYears(1775); // needs to be changed to null after table updates take place.
            CompletionStatus = string.Empty;
        }
        /// <summary>
        /// Gets or sets the login id for the JetAdvantage user account
        /// </summary>
        /// <value>The login identifier.</value>
        [DataLogProperty]
        public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the destination URL.
        /// </summary>
        /// <value>The destination URL.</value>
        [DataLogProperty(MaxLength = 255)]
        public string DestinationUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        [DataLogProperty]
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the time when file was sent.
        /// </summary>
        /// <value>The file sent time.</value>
        [DataLogProperty]
        public DateTimeOffset FileSentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the size of the file as sent.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataLogProperty]
        public long? FileSizeSentBytes { get; set; }

        /// <summary>
        /// Gets or sets the time when the file was received.
        /// </summary>
        /// <value>The file received time.</value>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? FileReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the size of the file as sent.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public long? FileSizeReceivedBytes { get; set; }

        /// <summary>
        /// Gets or sets the completion time.
        /// </summary>
        /// <value>
        /// The completion time.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset CompletionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the completion status.
        /// </summary>
        /// <value>
        /// The completion status.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public string CompletionStatus { get; set; }

        public override string TableName => "JetAdvantageUpload";
    }

}
