using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceUsage;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using System;
using System.Collections.Generic;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink
{
    /// <summary>
    /// Submit the JetAdvantagememoryMonitoring Memory snap shot and data on the DB
    /// </summary>
    public class JetAdvantageLinkMemoryMonitoring
    {
        private PluginExecutionData _pluginExecutionData;
        private JetAdvantageLinkDeviceMemorySnapShotDataLog _snapshotLogger;
        private JetAdvantageLinkDeviceMemoryCountDataLog _countLogger;
        private JetAdvantageLinkUI _linkUI;
        private string _targetPackage;
        private IDeviceInfo _deviceInfo;

        /// <summary>
        /// JetAdvantageLinkMemoryMonitoring 
        /// </summary>
        /// <param name="linkUI">Link UI</param>
        /// <param name="targetPackage">Target Android Package name</param>
        /// <param name="pluginExecutionData">PluginExecution Data</param>
        /// <param name="deviceInfo">deviceInfo</param>
        public JetAdvantageLinkMemoryMonitoring(JetAdvantageLinkUI linkUI, string targetPackage, PluginExecutionData pluginExecutionData, IDeviceInfo deviceInfo)
        {
            _linkUI = linkUI;
            _targetPackage = targetPackage;
            _pluginExecutionData = pluginExecutionData;
            _deviceInfo = deviceInfo;
        }

        private string _targetMonitoringPackage { get; set; }

        private Dictionary<string, long> _memoryUsageData { get; set; }

        private string _deviceID { get; set; }

        private int _usageCount { get; set; }

        /// <summary>
        /// Collect MomoryMonitoringData of JetAdvantageLink
        /// </summary>
        public void CollectMemoryMonitoringData()
        {
            _deviceID = _deviceInfo.AssetId;

            DeviceUsageCollector collector = new DeviceUsageCollector(_deviceInfo.Address);
            try
            {
                if (_deviceInfo.AssetType != "SFP")
                {
                    _usageCount = collector.CollectUsageCounts().TotalImageCount;
                }
                else
                {
                    _usageCount = collector.CollectSFPUsageCounts().TotalImageCount;
                }
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError("Get Page Count is failed", ex);
                _usageCount = -1;
            }

            _targetMonitoringPackage = _targetPackage;
            _memoryUsageData = _linkUI.Controller.GetMemoryUsage();
        }

        /// <summary>
        ///  Submit the JetAdvantageLinkDeviceMemoryCountDataLog and JetAdvantageLinkMemorySnapShotDataLog
        /// </summary>
        public void Submit()
        {
            if (_memoryUsageData.ContainsKey(_targetMonitoringPackage) && _memoryUsageData.ContainsKey("Total"))
            {
                ExecutionServices.SystemTrace.LogDebug($"Logging Memory Monitoring data for session {_pluginExecutionData.SessionId} and activity {_pluginExecutionData.ActivityExecutionId}.");
                _snapshotLogger = new JetAdvantageLinkDeviceMemorySnapShotDataLog(_pluginExecutionData)
                {
                    SnapshotDateTime = DateTime.Now,
                    DeviceId = _deviceID,
                    SnapshotLabel = _targetMonitoringPackage,
                    UsageCount = _usageCount,
                };
                ExecutionServices.DataLogger.Submit(_snapshotLogger);

                _countLogger = new JetAdvantageLinkDeviceMemoryCountDataLog(_pluginExecutionData)
                {
                    JetAdvantageLinkMemorySnapshotId = _snapshotLogger.RecordId,
                    CategoryName = "Android PSS",
                    DataLabel = _targetMonitoringPackage,
                    DataValue = _memoryUsageData[_targetMonitoringPackage]
                };
                ExecutionServices.DataLogger.Submit(_countLogger);
                ExecutionServices.SystemTrace.LogDebug($"Target record id ::::: {_countLogger.RecordId}");

                _countLogger = new JetAdvantageLinkDeviceMemoryCountDataLog(_pluginExecutionData)
                {
                    JetAdvantageLinkMemorySnapshotId = _snapshotLogger.RecordId,
                    CategoryName = "Android PSS",
                    DataLabel = "Total",
                    DataValue = _memoryUsageData["Total"]
                };
                ExecutionServices.DataLogger.Submit(_countLogger);
                ExecutionServices.SystemTrace.LogDebug($"Total record id ::::: {_countLogger.RecordId}");
            }

        }


    }
}
