using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to add asset info to a <see cref="SystemManifest"/>.
    /// </summary>
    public class ManifestAssetAgent : IManifestComponentAgent
    {
        private readonly Dictionary<Guid, AssetIdCollection> _activityAssets = new Dictionary<Guid, AssetIdCollection>();

        /// <summary>
        /// Gets Requested Assets for the specified scenario.
        /// </summary>
        public IEnumerable<string> RequestedAssets
        {
            get { return _activityAssets.Values.SelectMany(n => n).Distinct(); }
        }

        /// <summary>
        /// Creates a new instance of ManifestAssetAgent.
        /// </summary>
        /// <param name="scenarioId"></param>
        public ManifestAssetAgent(Guid scenarioId)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Retrieve asset usage data for all enabled activities in the specified session
                var activities = (from assetUsage in context.VirtualResourceMetadataAssetUsages
                                  let data = assetUsage.AssetSelectionData
                                  let metadata = assetUsage.VirtualResourceMetadata
                                  let resource = metadata.VirtualResource
                                  where resource.EnterpriseScenarioId == scenarioId
                                     && resource.Enabled == true
                                     && metadata.Enabled == true
                                     && data != null
                                  select new { Id = metadata.VirtualResourceMetadataId, Assets = data }).ToList();

                foreach (var activity in activities)
                {
                    AssetSelectionData assetSelectionData = GetSelectionData(activity.Assets);
                    _activityAssets.Add(activity.Id, GetAssetIds(assetSelectionData));
                }
            }
        }

        private static AssetSelectionData GetSelectionData(string data)
        {
            return Serializer.Deserialize<AssetSelectionData>(XElement.Parse(data));
        }

        private static AssetIdCollection GetAssetIds(AssetSelectionData selectionData)
        {
            AssetIdCollection result = new AssetIdCollection();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (string assetId in selectionData.SelectedAssets)
                {
                    result.Add(assetId);

                    //Check to see if the asset has badge boxes
                    BadgeBox badgeBox = context.BadgeBoxes.FirstOrDefault(n => n.PrinterId == assetId);
                    if (badgeBox != null)
                    {
                        TraceFactory.Logger.Debug($"Asset: {assetId} has associated BadgeBox: {badgeBox.BadgeBoxId}");
                        result.Add(badgeBox.BadgeBoxId);
                    }

                    //Check to see if the asset has cameras
                    Core.AssetInventory.Camera camera = context.Assets.OfType<Core.AssetInventory.Camera>().FirstOrDefault(c => c.PrinterId == assetId);
                    if (camera != null)
                    {
                        TraceFactory.Logger.Debug($"Asset: {assetId} has associated Camera: {camera.AssetId}");
                        result.Add(camera.AssetId);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Adds Asset info to the specified <see cref="SystemManifest"/>.
        /// </summary>
        /// <param name="manifest">The <see cref="SystemManifest"/>.</param>
        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.AllAssets.Clear();
            manifest.ActivityAssets.Clear();

            List<AssetInfo> assetInfoList = new List<AssetInfo>();
            var assetIds = _activityAssets.Values.SelectMany(n => n).Distinct().ToList();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                assetInfoList.AddRange(context.Assets.Where(n => assetIds.Contains(n.AssetId)).ToAssetInfoCollection());
                assetInfoList.AddRange(context.BadgeBoxes.Where(n => assetIds.Contains(n.PrinterId)).ToBadgeBoxInfoCollection());
            }

            foreach (AssetInfo asset in assetInfoList)
            {
                manifest.AllAssets.Add(asset);
                TraceFactory.Logger.Debug("asset: " + asset.AssetId + " Desc: " + asset.Description);
            }

            foreach (var activityAsset in _activityAssets)
            {
                manifest.ActivityAssets.Add(activityAsset.Key, activityAsset.Value);
            }
        }

        /// <summary>
        /// No logging occurs here. Each Asset map will log it's own device.
        /// </summary>
        /// <param name="sessionId"></param>
        public void LogComponents(string sessionId)
        {
        }
    }
}
