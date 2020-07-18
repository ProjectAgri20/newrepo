using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using System;

namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    /// <summary>
    /// Added test result to ConnectorJobInput table
    /// </summary>
    public sealed class ConnectorJobInputLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName
        {
            get { return "ConnectorJobInput"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorJobInputLog" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>        
        /// <param name="jobType">The Job Type: Scan, Print, etc.</param>        
        /// <exception cref="ArgumentNullException"><paramref name="executionData" /> is null.</exception>
        public ConnectorJobInputLog(PluginExecutionData executionData, string jobType)
            : base(executionData)
        {
            JobType = jobType;
            DeviceId = null;
            AppName = null;            
            LoginID = null;
            FilePath = null;
            FilePrefix = null;
            OptionsData = null;
            JobEndStatus = null;
            PageCount = 0;            
        }
        
        /// <summary>
        /// Gets or sets the device ID.
        /// </summary>
        /// <value>The device identifier.</value>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the type of the Cloud: OneDrive, Google Drive, Sharepoint, Dropbox
        /// </summary>
        /// <value>The type of the cloud.</value>
        [DataLogProperty]
        public string AppName { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the Cloud Job: Scan, Print.
        /// </summary>
        /// <value>The type of the Cloud Job.</value>
        [DataLogProperty]
        public string JobType { get; set; }
        
        /// <summary>
        /// Gets or sets the CloudLoginID.
        /// </summary>
        /// <value>The CloudLoginID.</value>
        [DataLogProperty]
        public string LoginID { get; set; }

        /// <summary>
        /// Gets or sets the FilePath.
        /// </summary>
        /// <value>The file prefix.</value>
        [DataLogProperty(MaxLength = 255)]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the FilePrefix.
        /// </summary>
        /// <value>The file prefix.</value>
        [DataLogProperty]
        public string FilePrefix { get; set; }

        /// <summary>
        /// Gets or sets the selected options.
        /// </summary>
        /// <value>The selected options.</value>
        [DataLogProperty(MaxLength = -1)]
        public string OptionsData { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        [DataLogProperty]
        public int PageCount { get; set; }
        
        /// <summary>
        /// Gets or sets the job end status.
        /// </summary>
        /// <value>The job end status.</value>
        [DataLogProperty]
        public string JobEndStatus { get; set; }        
    }
}
