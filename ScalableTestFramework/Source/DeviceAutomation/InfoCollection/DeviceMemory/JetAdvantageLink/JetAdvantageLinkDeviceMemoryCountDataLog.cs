using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using System;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink
{
    /// <summary>
    /// Data log for JetAdvantageLink device memory snap shot
    /// </summary>
    public class JetAdvantageLinkDeviceMemoryCountDataLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "JetAdvantageLinkDeviceMemoryCount";

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageLinkDeviceMemoryCountDataLog" /> class.
        /// </summary>
        /// <param name="pluginExecutionData"></param>
        public JetAdvantageLinkDeviceMemoryCountDataLog(PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
        }

        /// <summary>
        /// Gets the JetAdvantageLinkMemorySnapshotId
        /// </summary>
        [DataLogProperty]
        public Guid JetAdvantageLinkMemorySnapshotId { get; set; }

        /// <summary>
        /// Gets the CategoryName
        /// </summary>
        [DataLogProperty]
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets the DataLabel
        /// </summary>
        [DataLogProperty(MaxLength = 255, TruncationAllowed = true)]
        public string DataLabel { get; set; }

        /// <summary>
        /// Gets the DataValue
        /// </summary>
        [DataLogProperty]
        public long DataValue { get; set; }
    }
}
