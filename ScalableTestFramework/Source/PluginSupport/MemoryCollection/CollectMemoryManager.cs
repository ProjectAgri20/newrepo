using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.MemoryCollection
{
    /// <summary>
    /// CollectMemoryManager
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class CollectMemoryManager
    {
        /// <summary>
        /// Gets the device information.
        /// </summary>
        /// <value>
        /// The device information.
        /// </value>
        protected Framework.Assets.IDeviceInfo DeviceInfo { get; private set; }
        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public IDevice Device { get; private set; }

        private static List<MemoryCaptureDeviceInfo> _memoryDevices = new List<MemoryCaptureDeviceInfo>();

        /// <summary>
        /// Occurs when [status update] is utilized.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectMemoryManager"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">The device information.</param>
        public CollectMemoryManager(IDevice device, Framework.Assets.IDeviceInfo deviceInfo)
        {
            Device = device;
            DeviceInfo = deviceInfo;
        }
        /// <summary>
        /// Collects the memory profile on the device.
        /// </summary>
        /// <param name="memoryProfilerConfig"></param>
        /// <param name="snapshotLabel"></param>
        public void CollectDeviceMemoryProfile(DeviceMemoryProfilerConfig memoryProfilerConfig, string snapshotLabel)
        {
            bool collectMemory = false;

            try
            {
                if (DeviceInfo != null && !string.IsNullOrEmpty(DeviceInfo.Address) && memoryProfilerConfig != null && (memoryProfilerConfig.SampleAtCountIntervals || memoryProfilerConfig.SampleAtTimeIntervals)) //TODO: Move this to before calling CollectDeviceMemoryProfile
                {
                    MemoryCaptureDeviceInfo dmc = GetMemoryCaptureDeviceInfo(DeviceInfo);

                    if (dmc != null && dmc.EligibleForMemoryCapture)
                    {
                        dmc.CountSinceLastCapture++;

                        // for debugging when memory collection should occur
                        //UpdateStatus("Ticks = {0}, est. next sample time={1}".FormatWith(
                        //    (!dmc.DateOfLastCapture.HasValue ? 0 : (DateTime.Now - dmc.DateOfLastCapture.Value).Ticks),
                        //    (!dmc.DateOfLastCapture.HasValue ? DateTime.Now : dmc.DateOfLastCapture.Value.AddTicks(config.TargetSampleTime.Ticks))
                        //    ));

                        // flag to collect if sampling is enabled and it's either the first time or the target has been met/exceeded
                        if (
                            (memoryProfilerConfig.SampleAtCountIntervals && (!dmc.DateOfLastCapture.HasValue || dmc.CountSinceLastCapture >= memoryProfilerConfig.TargetSampleCount))
                            || (memoryProfilerConfig.SampleAtTimeIntervals && (!dmc.DateOfLastCapture.HasValue || (DateTime.Now - dmc.DateOfLastCapture.Value).Ticks >= memoryProfilerConfig.TargetSampleTime.Ticks))
                           )
                        {
                            collectMemory = true;
                        }
                        else
                        {
                            OnStatusUpdate($"-- DWA -- Not Collecting device memory");
                        }
                    }

                    if (collectMemory && dmc != null)
                    {
                        OnStatusUpdate($"Collecting device memory (last snapshot at {dmc.DateOfLastCapture}, count = {dmc.CountSinceLastCapture})...");

                        //Call into SessionProxyBackendConnection
                        ExecutionServices.SessionRuntime.CollectDeviceMemoryProfile(DeviceInfo, snapshotLabel);

                        dmc.DateOfLastCapture = DateTime.Now;
                        dmc.CountSinceLastCapture = 0;

                        OnStatusUpdate($"Device memory collected (last snapshot at {dmc.DateOfLastCapture}, count reset to {dmc.CountSinceLastCapture}.)");
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = $"Unable to collect memory for device {DeviceInfo.Address}.  ({ex.Message})";
                OnStatusUpdate(msg);
                ExecutionServices.SystemTrace.LogError(msg, ex);
            }

        }
        private MemoryCaptureDeviceInfo GetMemoryCaptureDeviceInfo(Framework.Assets.IDeviceInfo lookupDevice)
        {
            MemoryCaptureDeviceInfo result = null;

            if (lookupDevice != null)
            {
                result = _memoryDevices.FirstOrDefault(x => x.Address == lookupDevice.Address);
                if (result == null)
                {
                    result = new MemoryCaptureDeviceInfo() { Address = lookupDevice.Address };
                    result.EligibleForMemoryCapture = (Device is JediDevice);
                    _memoryDevices.Add(result);
                }
            }
            return result;
        }

        private class MemoryCaptureDeviceInfo
        {
            public string Address = string.Empty;
            public DateTime? DateOfLastCapture = null;
            public int CountSinceLastCapture = 0;
            public bool EligibleForMemoryCapture = false;
        }
        /// <summary>
        /// Invoke the StatusUpdate Event.
        /// </summary>
        /// <param name="message"></param>
        protected void OnStatusUpdate(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }
}
