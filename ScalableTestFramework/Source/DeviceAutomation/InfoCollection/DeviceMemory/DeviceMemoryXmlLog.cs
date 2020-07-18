using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory
{
    /// <summary>
    /// Data entry for the Jedi Memory XML collected from a Jedi device.
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Data.FrameworkDataLog" />
    public sealed class DeviceMemoryXmlLog : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "DeviceMemoryXml";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DeviceMemoryXmlId);

        /// <summary>
        /// Gets the device memory XML identifier.
        /// </summary>
        /// <value>
        /// The device memory XML identifier.
        /// </value>
        [DataLogProperty]
        public Guid DeviceMemoryXmlId { get; }

        /// <summary>
        /// Gets the device memory snapshot ID.
        /// </summary>
        [DataLogProperty]
        public Guid DeviceMemorySnapshotId { get; }

        /// <summary>
        /// Gets the memory count XML.
        /// </summary>
        /// <value>
        /// The memory count XML.
        /// </value>
        [DataLogProperty(MaxLength = -1)]
        public string MemoryXml { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMemoryXmlLog"/> class.
        /// </summary>
        /// <param name="deviceMemorySnapshotId">The device memory snapshot identifier.</param>
        /// <param name="memoryXML">The memory XML.</param>
        public DeviceMemoryXmlLog(Guid deviceMemorySnapshotId, string memoryXML)
        {
            DeviceMemoryXmlId = SequentialGuid.NewGuid();
            DeviceMemorySnapshotId = deviceMemorySnapshotId;
            MemoryXml = memoryXML;
        }
    }
}
