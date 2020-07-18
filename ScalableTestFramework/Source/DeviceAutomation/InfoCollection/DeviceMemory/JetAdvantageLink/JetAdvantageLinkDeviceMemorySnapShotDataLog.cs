using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using System;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink
{
    /// <summary>
    /// Data log for JetAdvantageLink device memory snap shot.
    /// </summary>
    public class JetAdvantageLinkDeviceMemorySnapShotDataLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.        
        /// </summary>
        public override string TableName => "JetAdvantageLinkDeviceMemorySnapshot";

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageLinkDeviceMemoryCountDataLog" /> class.
        /// </summary>
        /// <param name="pluginExecutionData"></param>
        public JetAdvantageLinkDeviceMemorySnapShotDataLog(PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
        }

        /// <summary>
        /// Gets the DeviceId
        /// </summary>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets the SnapshotLabel
        /// </summary>
        [DataLogProperty(MaxLength = 255, TruncationAllowed = true)]
        public string SnapshotLabel { get; set; }

        /// <summary>
        /// Gets the device UsageCount
        /// </summary>
        [DataLogProperty]
        public int UsageCount { get; set; }

        /// <summary>
        /// Gets the SnapshotDateTime
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset SnapshotDateTime { get; set; }
    }
}
