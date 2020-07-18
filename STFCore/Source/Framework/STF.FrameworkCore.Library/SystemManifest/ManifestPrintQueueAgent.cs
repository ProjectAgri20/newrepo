using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework
{
    public class ManifestPrintQueueAgent : IManifestComponentAgent
    {
        private readonly Dictionary<Guid, PrintQueueInfoCollection> _printQueues = new Dictionary<Guid, PrintQueueInfoCollection>();

        public IEnumerable<string> RequestedAssets
        {
            get { return _printQueues.Values.SelectMany(n => n).Select(n => n.AssociatedAssetId).Where(n => !string.IsNullOrEmpty(n)).Distinct(); }
        }

        public ManifestPrintQueueAgent(Guid scenarioId)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Retrieve print queue data for all enabled activities in the specified session
                var activities = (from printQueueUsage in context.VirtualResourceMetadataPrintQueueUsages
                                  let data = printQueueUsage.PrintQueueSelectionData
                                  let metadata = printQueueUsage.VirtualResourceMetadata
                                  let resource = metadata.VirtualResource
                                  where resource.EnterpriseScenarioId == scenarioId
                                     && resource.Enabled == true
                                     && metadata.Enabled == true
                                     && data != null
                                  select new { Id = metadata.VirtualResourceMetadataId, PrintQueues = data }).ToList();

                foreach (var activity in activities)
                {
                    PrintQueueSelectionData printQueueSelectionData = GetSelectionData(activity.PrintQueues);
                    _printQueues.Add(activity.Id, GetPrintQueues(printQueueSelectionData));
                }
            }
        }

        private static PrintQueueSelectionData GetSelectionData(string data)
        {
            return Serializer.Deserialize<PrintQueueSelectionData>(XElement.Parse(data));
        }

        private PrintQueueInfoCollection GetPrintQueues(PrintQueueSelectionData printQueueSelectionData)
        {
            List<PrintQueueInfo> printQueues = new List<PrintQueueInfo>();
            printQueues.AddRange(CreateLocalPrintQueues(printQueueSelectionData));
            printQueues.AddRange(CreateRemotePrintQueues(printQueueSelectionData));
            printQueues.AddRange(CreateDynamicPrintQueues(printQueueSelectionData));
            return new PrintQueueInfoCollection(printQueues);
        }

        private IEnumerable<PrintQueueInfo> CreateLocalPrintQueues(PrintQueueSelectionData printQueueSelectionData)
        {
            foreach (LocalPrintQueueDefinition definition in printQueueSelectionData.SelectedPrintQueues.OfType<LocalPrintQueueDefinition>())
            {
                yield return new LocalPrintQueueInfo(definition.QueueName, definition.AssociatedAssetId);
            }
        }

        private IEnumerable<PrintQueueInfo> CreateRemotePrintQueues(PrintQueueSelectionData printQueueSelectionData)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var printQueueIds = printQueueSelectionData.SelectedPrintQueues.OfType<RemotePrintQueueDefinition>().Select(n => n.PrintQueueId);
                return context.RemotePrintQueues.Where(n => printQueueIds.Contains(n.RemotePrintQueueId)).ToRemotePrintQueueInfoCollection();
            }
        }

        private IEnumerable<PrintQueueInfo> CreateDynamicPrintQueues(PrintQueueSelectionData printQueueSelectionData)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var printQueueDefinitions = printQueueSelectionData.SelectedPrintQueues.OfType<DynamicLocalPrintQueueDefinition>();
                var assetIds = printQueueDefinitions.Select(n => n.AssetId);
                var driverIds = printQueueDefinitions.Select(n => n.PrintDriverId);

                var assets = context.Assets.Where(n => assetIds.Contains(n.AssetId)).ToAssetInfoCollection();
                var drivers = context.PrintDrivers.Where(n => driverIds.Contains(n.PrintDriverId)).ToPrintDriverInfoCollection();

                foreach (DynamicLocalPrintQueueDefinition definition in printQueueDefinitions)
                {
                    IPrinterInfo printDevice = assets.FirstOrDefault(n => n.AssetId == definition.AssetId) as IPrinterInfo;
                    PrintDriverInfo printDriver = drivers.FirstOrDefault(n => n.PrintDriverId == definition.PrintDriverId);
                    if (printDevice != null && printDriver != null)
                    {
                        yield return new DynamicLocalPrintQueueInfo(printDevice, printDriver, definition.PrinterPort, definition.PrintDriverConfiguration);
                    }
                }
            }
        }

        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.ActivityPrintQueues.Clear();

            foreach (var item in _printQueues)
            {
                manifest.ActivityPrintQueues.Add(item.Key, item.Value);
            }
        }

        public void LogComponents(string sessionId)
        {
            // Nothing to log here - asset maps will log their own devices
        }
    }
}
