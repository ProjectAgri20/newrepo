using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory
{
    /// <summary>
    /// Data log for device memory snapshots.
    /// </summary>
    public sealed class DeviceMemorySnapshotLog : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "DeviceMemorySnapshot";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DeviceMemorySnapshotId);

        /// <summary>
        /// Gets the device memory snapshot ID.
        /// </summary>
        [DataLogProperty]
        public Guid DeviceMemorySnapshotId { get; }

        /// <summary>
        /// Gets the session ID.
        /// </summary>
        [DataLogProperty]
        public string SessionId { get; }

        /// <summary>
        /// Gets the device ID.
        /// </summary>
        [DataLogProperty]
        public string DeviceId { get; }

        /// <summary>
        /// Gets the snapshot label.
        /// </summary>
        [DataLogProperty(TruncationAllowed = true)]
        public string SnapshotLabel { get; }

        /// <summary>
        /// Gets the usage count.
        /// </summary>
        [DataLogProperty]
        public int? UsageCount { get; }

        /// <summary>
        /// Gets the snapshot date time.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset SnapshotDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMemorySnapshotLog" /> class.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="deviceId">The device ID.</param>
        /// <param name="snapshotLabel">The snapshot label.</param>
        /// <param name="usageCount">The usage count.</param>
        /// <param name="snapshotDateTime">The snapshot date time.</param>
        public DeviceMemorySnapshotLog(string sessionId, string deviceId, string snapshotLabel, int? usageCount, DateTime snapshotDateTime)
        {
            DeviceMemorySnapshotId = SequentialGuid.NewGuid();
            SessionId = sessionId;
            DeviceId = deviceId;
            SnapshotLabel = snapshotLabel;
            UsageCount = usageCount;
            SnapshotDateTime = snapshotDateTime;
        }
    }
}
