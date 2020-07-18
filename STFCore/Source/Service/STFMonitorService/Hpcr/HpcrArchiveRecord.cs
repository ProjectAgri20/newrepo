using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Service.Monitor.Hpcr
{
    /// <summary>
    /// column data retrieved from the HPCR Archive table
    /// </summary>
    public class HpcrArchiveRecord
    {
        /// <summary>
        /// Gets or sets the STF log identifier.
        /// </summary>
        /// <value>
        /// The STF log identifier.
        /// </value>
        public Guid StfLogId { get; set; }

        /// <summary>
        /// Gets or sets the STF digital send server job identifier.
        /// </summary>
        /// <value>
        /// The STF digital send server job identifier.
        /// </value>
        public Guid StfDigitalSendServerJobId { get; set; }

        /// <summary>
        /// Gets or sets the STF log status.
        /// </summary>
        /// <value>
        /// The STF log status.
        /// </value>
        public string StfLogStatus { get; set; }

        /// <summary>
        /// Gets or sets the row identifier.
        /// </summary>
        /// <value>
        /// The row identifier.
        /// </value>
        public int RowIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the date submitted.
        /// </summary>
        /// <value>
        /// The date submitted.
        /// </value>
        public DateTime? DateSubmitted { get; set; }

        /// <summary>
        /// Gets or sets the date delivered.
        /// </summary>
        /// <value>
        /// The date delivered.
        /// </value>
        public DateTime? DateDelivered { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>
        /// The name of the document.
        /// </value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the type of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the final status.
        /// </summary>
        /// <value>
        /// The final status.
        /// </value>
        public string FinalStatus { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the scanned pages.
        /// </summary>
        /// <value>
        /// The scanned pages.
        /// </value>
        public int ScannedPages { get; set; }

        /// <summary>
        /// Gets or sets the file size bytes.
        /// </summary>
        /// <value>
        /// The file size bytes.
        /// </value>
        public int FileSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the device model.
        /// </summary>
        /// <value>
        /// The device model.
        /// </value>
        public string DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the DSS version.
        /// </summary>
        /// <value>
        /// The DSS version.
        /// </value>
        public string DssVersion { get; set; }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId
        {
            get
            {
                string result = null;
                string[] data = DocumentName.Split('-');
                if (data.Length > 0 && data[0].Length == 8)
                {
                    result = data[0];
                }
                return result;
            }
        }
    }
    internal struct HpcrWorkflows
    {
        public const string myfiles = "myfiles";
        public const string scantofolder = "scantofolder";
        public const string publicdistributions = "publicdistributions";
        public const string personaldistributions = "personaldistributions";
        public const string scantome = "scantome";
    }
    internal struct HpcrDatabaseColumns
    {
        public const string StfLogId = "StfLogId";
        public const string RowIdentifier = "RowIdentifier";
        public const string prDateDelivered = "prDateDelivered";
        public const string prDateSubmitted = "prDateSubmitted";
        public const string prDestination = "prDestination";
        public const string DocumentName = "DocumentName";
        public const string prFinalFormCode = "prFinalFormCode";
        public const string DocumentBytes = "DocumentBytes";
        public const string DocumentCount = "DocumentCount";
        public const string prHPCRServerVersion = "prHPCRServerVersion";
        public const string prFinalStatusText = "prFinalStatusText";
        public const string StfLogStatus = "StfLogStatus";
        public const string DeviceModelName = "DeviceModelName";
        public const string StfDigitalSendServerJobId = "StfDigitalSendServerJobId";
    }
    internal struct StfLogStatus
    {
        public const string Completed = "Completed";
        public const string Partial = "Partial";
        public const string Ignore = "Ignore";
    }
}
