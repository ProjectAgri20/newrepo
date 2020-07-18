using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Data log for triage information.
    /// </summary>
    public class TriageDataLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "TriageData";

        /// <summary>
        /// Initializes a new instance of the <see cref="TriageDataLog"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        public TriageDataLog(PluginExecutionData pluginExecutionData)
            : base(pluginExecutionData)
        {
        }

        /// <summary>
        /// Gets or sets the control ids.
        /// </summary>
        /// <value>
        /// The control ids.
        /// </value>
        [DataLogProperty(MaxLength = -1)]
        public string ControlIds { get; set; }

        /// <summary>
        /// Gets or sets the control panel image.
        /// </summary>
        /// <value>
        /// The control panel image.
        /// </value>
        [DataLogProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] ControlPanelImage { get; set; }


        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        [DataLogProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the reason for data collection.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [DataLogProperty(MaxLength = -1)]
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the device warnings.
        /// </summary>
        /// <value>
        /// The device warnings.
        /// </value>
        [DataLogProperty(MaxLength = -1)]
        public string DeviceWarnings { get; set; }

        /// <summary>
        /// Gets or sets the triage date time.
        /// </summary>
        /// <value>
        /// The triage date time.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset TriageDateTime { get; set; }

        /// <summary>
        /// Gets or sets the UIDumpData for JetAdvantageLink
        /// </summary>
        /// <value>
        /// The UiDumpData
        /// </value>
        [DataLogProperty(MaxLength = -1)]
        public string UIDumpData { get; set; }
    }
}
