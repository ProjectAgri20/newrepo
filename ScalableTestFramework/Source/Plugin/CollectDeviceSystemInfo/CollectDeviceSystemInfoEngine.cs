using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    /// <summary>
    /// The engine for collecting and logging info about device memory
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    public class CollectDeviceSystemInfoEngine : DeviceWorkflowLogSource
    {

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// bool: Property that returns true if the error message has data
        /// </summary>
        public bool IsError => (string.IsNullOrEmpty(ErrorMessage) == false) ? true : false;

        private PluginExecutionData _executionData;
        /// <summary>
        /// Gets or sets the problem devices.
        /// </summary>
        /// <value>
        /// The problem devices.
        /// </value>
        public ConcurrentBag<string> ProblemDevices { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDeviceSystemInfoEngine"/> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <exception cref="ArgumentNullException">executionData</exception>
        public CollectDeviceSystemInfoEngine(PluginExecutionData executionData):base()
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }
            ErrorMessage = string.Empty;
            ProblemDevices = new ConcurrentBag<string>();
            _executionData = executionData;
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
        }
        private bool CollectDeviceMemoryProfile(IDeviceInfo device)
        {
            bool result = false;
            try
            {
                RecordEvent(DeviceWorkflowMarker.ActivityBegin, device.AssetId);
                ExecutionServices.SessionRuntime.CollectDeviceMemoryProfile(device, "CollectDeviceSystemInfo");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd, device.AssetId);
                result = true;
            }
            catch (Exception ex)
            {
                ErrorMessage += $"Unable to collect memory for device {device.Address}.  ({ex.Message})";
                ExecutionServices.SystemTrace.LogError(ErrorMessage, ex);
            }
            return result;
        }

        /// <summary>
        /// Processes the memory collection by the given device.
        /// </summary>
        /// <param name="device">The device.</param>
        public void ProcessMemCollectionByDevice(IDeviceInfo device)
        {
            var token = new AssetLockToken(device, new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0));
            RecordEvent(DeviceWorkflowMarker.DeviceLockBegin, device.AssetId);
            ExecutionServices.CriticalSection.Run(token, () =>
            {
                ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, device));
                if (!CollectDeviceMemoryProfile(device))
                {                    
                    ProblemDevices.Add(device.Address);
                }

            });

            RecordEvent(DeviceWorkflowMarker.DeviceLockEnd, device.AssetId);
        }
    }
}
