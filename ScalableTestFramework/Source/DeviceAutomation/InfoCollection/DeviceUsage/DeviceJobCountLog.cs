using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceUsage
{
    /// <summary>
    /// Data log for device Job counters.
    /// </summary>
    public sealed class DeviceJobCountLog : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "DeviceJobCount";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DeviceJobCountId);

        /// <summary>
        /// Gets the device Job count ID.
        /// </summary>
        [DataLogProperty]
        public Guid DeviceJobCountId { get; }

        /// <summary>
        /// Gets the device Job snapshot ID.
        /// </summary>
        [DataLogProperty]
        public Guid DeviceJobSnapshotId { get; }

        /// <summary>
        /// Gets the colour print counts.
        /// </summary>
        [DataLogProperty]
        public int ColourCount { get; private set; }

        /// <summary>
        /// Gets the B/w print counts.
        /// </summary>
        [DataLogProperty]
        public int MonoCount { get; private set; }

        /// <summary>
        /// Gets the ADF scan counts.
        /// </summary>
        [DataLogProperty]
        public int AdfCount { get; private set; }

        /// <summary>
        /// Gets the Flatbed scan counts.
        /// </summary>
        [DataLogProperty]
        public int FlatbedCount { get; private set; }

        /// <summary>
        /// Gets the total print counts.
        /// </summary>
        [DataLogProperty]
        public int PrintTotal { get; private set; }

        /// <summary>
        /// Gets the total scan counts.
        /// </summary>
        [DataLogProperty]
        public int ScanTotal { get; private set; }

        /// <summary>
        /// Gets the total Fax counts.
        /// </summary>
        [DataLogProperty]
        public int FaxCount { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceJobCountLog" /> class.
        /// </summary>
        /// <param name="deviceJobSnapshotId">The device Job snapshot ID.</param>
        /// <param name="colourCount"></param>
        /// <param name="monoCount"></param>
        /// <param name="adfCount"></param>
        /// <param name="flatbedCount"></param>
        /// <param name="faxCount"></param>
        public DeviceJobCountLog(Guid deviceJobSnapshotId, int colourCount, int monoCount, int adfCount, int flatbedCount, int faxCount)
        {
            DeviceJobCountId = SequentialGuid.NewGuid();
            DeviceJobSnapshotId = deviceJobSnapshotId;
            ColourCount = colourCount;
            MonoCount = monoCount;
            AdfCount = adfCount;
            FlatbedCount = flatbedCount;
            PrintTotal = colourCount + monoCount;
            ScanTotal = adfCount + flatbedCount;
            FaxCount = faxCount;
        }
    }
}
