using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Class for logging data from the Connectivity Base plug-in to the ConnectivityTestDetails table.
    /// </summary>
    public class ConnectivityTestDetailLog : ActivityDataLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectivityTestDetailLog"/> class.
        /// </summary>
        public ConnectivityTestDetailLog(PluginExecutionData executionData)
            : base(executionData)
        {
        }

        /// <summary>
        /// Gets name of the table
        /// </summary>
        public override string TableName
        {
            get { return "ConnectivityTestDetail"; }
        }

        /// <summary>
        /// Gets or sets the test case id.
        /// </summary>
        /// <value>
        /// The test case id.
        /// </value>
        [DataLogProperty]
        public int TestId { get; set; }

        /// <summary>
        /// Gets or sets the product family.
        /// </summary>
        /// <value>
        /// The product family.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string ProductFamily { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        /// <value>
        /// The product name.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the test name.
        /// </summary>
        /// <value>
        /// The test name.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string TestName { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataLogProperty(MaxLength = 1024)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset EndTime { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [DataLogProperty]
        public bool Result { get; set; }

        /// <summary>
        /// Gets or sets the error details.
        /// </summary>
        /// <value>
        /// The error details.
        /// </value>
        [DataLogProperty(MaxLength = -1)]
        public string ErrorDetails { get; set; }

        /// <summary>
        /// Gets or sets the sliver name
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string Sliver { get; set; }

        /// <summary>
        /// Gets or Sets IPv4 or IPv6 connectivity
        /// </summary>
        [DataLogProperty(MaxLength = 5)]
        public string Connectivity { get; set; }

        /// <summary>
        /// Gets or Sets Wired or Wireless mode
        /// </summary>
        [DataLogProperty(MaxLength = 10)]
        public string Mode { get; set; }

        /// <summary>
        /// Gets or Sets print protocol like IPP/ RAW/ ...
        /// </summary>
        [DataLogProperty(MaxLength = 10)]
        public string PrintProtocol { get; set; }

        /// <summary>
        /// Gets or Sets firmware version
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string FirmwareVersion { get; set; }
    }
}
