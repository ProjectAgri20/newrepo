using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory
{
    /// <summary>
    /// Data log for device memory counters.
    /// </summary>
    public sealed class DeviceMemoryCountLog : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "DeviceMemoryCount";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DeviceMemoryCountId);

        /// <summary>
        /// Gets the device memory count ID.
        /// </summary>
        [DataLogProperty]
        public Guid DeviceMemoryCountId { get; }

        /// <summary>
        /// Gets the device memory snapshot ID.
        /// </summary>
        [DataLogProperty]
        public Guid DeviceMemorySnapshotId { get; }

        /// <summary>
        /// Gets the device memory category name.
        /// </summary>
        [DataLogProperty(MaxLength = 255, TruncationAllowed = true)]
        public string CategoryName { get; }

        /// <summary>
        /// Gets the data label.
        /// </summary>
        [DataLogProperty(MaxLength = 255, TruncationAllowed = true)]
        public string DataLabel { get; }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        [DataLogProperty]
        public long DataValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMemoryCountLog" /> class.
        /// </summary>
        /// <param name="deviceMemorySnapshotId">The device memory snapshot ID.</param>
        /// <param name="categoryName">The device memory category name.</param>
        /// <param name="dataLabel">The data label.</param>
        /// <param name="dataValue">The data value.</param>
        public DeviceMemoryCountLog(Guid deviceMemorySnapshotId, string categoryName, string dataLabel, long dataValue)
        {
            DeviceMemoryCountId = SequentialGuid.NewGuid();
            DeviceMemorySnapshotId = deviceMemorySnapshotId;
            CategoryName = categoryName;
            DataLabel = dataLabel;
            DataValue = dataValue;
        }
    }
}
