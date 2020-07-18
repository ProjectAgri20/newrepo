using System;
using System.Xml.Linq;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceUsage;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory
{
    /// <summary>
    /// Retrieves memory counter information from a Jedi device.
    /// </summary>
    public sealed class JediMemoryRetrievalAgent
    {
        private readonly IDeviceInfo _device;
        private readonly string _sessionId;
        private readonly string _memoryPools;
        private readonly bool _enterpriseTesting;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediMemoryRetrievalAgent" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="memoryPools">The memory pools.</param>
        /// <param name="enterpriseTesting">if set to <c>true</c> [enterprise testing].</param>
        public JediMemoryRetrievalAgent(IDeviceInfo device, string sessionId, string memoryPools, bool enterpriseTesting)
        {
            _device = device;
            _sessionId = sessionId;
            _memoryPools = memoryPools;
            _enterpriseTesting = enterpriseTesting;
        }

        /// <summary>
        /// Collects the memory counter data and records it to the database.
        /// </summary>
        /// <param name="snapshotLabel">The snapshot label (i.e. why the memory was retrieved).</param>
        public void CollectDeviceMemoryCounters(string snapshotLabel)
        {
            int imageCount;
            DeviceUsageCollector collector = new DeviceUsageCollector(_device.Address);
            if (_device.AssetType != "SFP")
            {
                imageCount = collector.CollectUsageCounts().TotalImageCount;
            }
            else
            {
                imageCount = collector.CollectSFPUsageCounts().TotalImageCount;
            }

            CollectDeviceMemoryCounters(snapshotLabel, imageCount);
        }

        /// <summary>
        /// Collects the memory counter data and records it to the database.
        /// </summary>
        /// <param name="snapshotLabel">The snapshot label (i.e. why the memory was retrieved).</param>
        /// <param name="imageCount">The image count.</param>
        public void CollectDeviceMemoryCounters(string snapshotLabel, int imageCount)
        {
            XDocument memoryXml = null;
            try
            {
                using (JediDevice device = new JediDevice(_device.Address, _device.AdminPassword))
                {
                    memoryXml = device.Diagnostics.GetMemoryCounters();
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Unable to collect memory counters for device {_device.AssetId}.", ex);
            }

            Guid snapshotId = ProcessMemorySnapshot(snapshotLabel, imageCount);

            if (_enterpriseTesting)
            {
                ProcessMemoryXml(memoryXml, snapshotId, snapshotLabel);
            }
            ProcessMemoryData(snapshotId, memoryXml);
        }

        private void ProcessMemoryXml(XDocument xml, Guid snapshotId, string snapshotLabel)
        {
            DeviceMemoryXmlLog log = new DeviceMemoryXmlLog(snapshotId, xml.ToString());
            if (!ExecutionServices.DataLogger.AsInternal().Submit(log))
            {
                Logger.LogDebug("DWA - Submit failed inserting DeviceMemoryXmlLog, DMS for device " + _device.Address + "SSL: " + snapshotLabel + " - " + DateTime.Now.ToString());
            }
        }

        private Guid ProcessMemorySnapshot(string snapshotLabel, int? usageCount)
        {
            // debug statements currently appear to solve a timing issue when insert the parent record.
            DeviceMemorySnapshotLog snapshotLog = new DeviceMemorySnapshotLog(_sessionId, _device.AssetId, snapshotLabel, usageCount, DateTime.Now);
            //Logger.LogDebug("DWA - Prepping to log new DMS for device " + _device.Address + " SSL: " + snapshotLabel + " - " + DateTime.Now.ToString());
            if (!ExecutionServices.DataLogger.AsInternal().Submit(snapshotLog))
            {
                Logger.LogDebug("DWA - Submit failed, DMS for device " + _device.Address + "SSL: " + snapshotLabel + " - " + DateTime.Now.ToString());
            }
            //Logger.LogDebug("DWA - Returning SS ID - " + snapshotLog.DeviceMemorySnapshotId.ToString() + ", DMS for device " + _device.Address + "SSL: " + snapshotLabel + " - " + DateTime.Now.ToString());
            return snapshotLog.DeviceMemorySnapshotId;
        }

        private void ProcessMemoryData(Guid snapshotId, XDocument memoryData)
        {
            CategoryLabelParser clp = new CategoryLabelParser(memoryData, _memoryPools);

            foreach (DeviceMemoryCountLog dmcl in clp.ProcessMemoryData(snapshotId))
            {
                ExecutionServices.DataLogger.AsInternal().Submit(dmcl);
            }
        }
    }
}
