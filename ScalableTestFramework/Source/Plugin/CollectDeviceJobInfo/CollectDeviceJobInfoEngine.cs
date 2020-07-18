using System;
using HP.ScalableTest.Framework;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceUsage;


namespace HP.ScalableTest.Plugin.CollectDeviceJobInfo
{
    /// <summary>
    /// The engine for collecting and logging info about job
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    public class CollectDeviceJobInfoEngine : DeviceWorkflowLogSource
    {
        private PluginExecutionData _executionData;

        /// <summary>
        /// Gets or sets the problem devices.
        /// </summary>
        /// <value>
        /// The problem devices.
        /// </value>
        public Dictionary<string, string> ProblemDevices { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDeviceJobInfoEngine"/> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <exception cref="ArgumentNullException">executionData</exception>
        public CollectDeviceJobInfoEngine(PluginExecutionData executionData) : base()
        {
            _executionData = executionData ?? throw new ArgumentNullException(nameof(executionData));
            ProblemDevices = new Dictionary<string, string>();
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
        }

        /// <summary>
        /// Collect device Job profile
        /// </summary>
        /// <param name="device"></param>
        private void CollectDeviceJobProfile(IDeviceInfo device)
        {
            DeviceUsageCollector collector = new DeviceUsageCollector(device.Address);
            RecordEvent(DeviceWorkflowMarker.ActivityBegin, device.AssetId);
            ProcessUsageCounts(device, collector);
            RecordEvent(DeviceWorkflowMarker.ActivityEnd, device.AssetId);
        }

        /// <summary>
        /// Process the Job information from the given device.
        /// </summary>
        /// <param name="device">The device.</param>
        public void ProcessJobCollectionByDevice(IDeviceInfo device)
        {
            var token = new AssetLockToken(device, new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0));
            RecordEvent(DeviceWorkflowMarker.DeviceLockBegin, device.AssetId);
            ExecutionServices.CriticalSection.Run(token, () =>
            {
                ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, device));
                CollectDeviceJobProfile(device);
            });
            RecordEvent(DeviceWorkflowMarker.DeviceLockEnd, device.AssetId);
        }

        /// <summary>
        /// Collect the device usage count.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        private void ProcessUsageCounts(IDeviceInfo device, DeviceUsageCollector collector)
        {
            DeviceUsageCounts usageCounts = collector.CollectUsageCounts();
            DeviceJobSnapshotLog snapshotLog = new DeviceJobSnapshotLog(_executionData.SessionId, device.AssetId, "CollectDeviceJobInfo", usageCounts.PrintTotalImages, DateTime.Now);
            DeviceJobCountLog usageCountLog = new DeviceJobCountLog(snapshotLog.DeviceJobSnapshotId, usageCounts.TotalColorPageCount, usageCounts.TotalMonoPageCount, usageCounts.AdfTotalImages, usageCounts.FlatbedTotalImages, usageCounts.TotalFaxCount);

            ExecutionServices.DataLogger.AsInternal().Submit(snapshotLog);
            ExecutionServices.DataLogger.AsInternal().Submit(usageCountLog);           

        }
    }
}
