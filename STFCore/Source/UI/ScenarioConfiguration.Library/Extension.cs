using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Configuration;
using System.Collections.ObjectModel;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    internal static class Extension
    {
        /// <summary>
        /// Sets the VirtualMachinePlatform for the given VirtualResource and VirtualResourceType.
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="resource"></param>
        /// <param name="workerType"></param>
        public static void SetPlatform(this ComboBox comboBox, string resourcePlatform, VirtualResourceType workerType)
        {
            // Load the platform combo box
            var platforms = VirtualMachinePlatforms(workerType, UserManager.CurrentUser);
            var platform = platforms.FirstOrDefault(p => p.FrameworkClientPlatformId == resourcePlatform);
            if (platform == null)
            {
                // If the platform is not present in the user's authorized list of platforms, then
                // load it directly from the database and then add it to the list
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    platform = context.FrameworkClientPlatforms.FirstOrDefault(p => p.FrameworkClientPlatformId == resourcePlatform);
                }
                if (platform != null)
                {
                    platforms = platforms.Union(new List<FrameworkClientPlatform>() { platform }).OrderBy(x => x.Name).ToList();
                }
                else if (platforms.Count() > 0)
                {
                    platform = platforms.First();
                }
                else
                {
                    MessageBox.Show("You are not authorized to select any platforms.  Contact your Administrator.", "Unauthorized Access", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            comboBox.DataSource = platforms;
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "FrameworkClientPlatformId";
            comboBox.SelectedItem = platform;
        }

        /// <summary>
        /// Gets a collection of virtual machine platforms that this resource type supports.
        /// </summary>
        /// <param name="resourceType">The resource type to associate with platforms.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>IEnumerable{VirtualMachinePlatform}.</returns>
        public static IEnumerable<FrameworkClientPlatform> VirtualMachinePlatforms(VirtualResourceType resourceType, UserCredential user)
        {
            Collection<FrameworkClientPlatform> platformList = new Collection<FrameworkClientPlatform>();
            string stringType = resourceType.ToString();

            List<string> allowedPlatforms = new List<string>();
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                allowedPlatforms.AddRange(context.ResourceTypes.Find(stringType).FrameworkClientPlatforms.Select(n => n.FrameworkClientPlatformId));
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var allPlatforms = context.FrameworkClientPlatforms.Where(n => n.Active).AsEnumerable();
                return allPlatforms.Where(n => allowedPlatforms.Contains(n.FrameworkClientPlatformId, StringComparer.OrdinalIgnoreCase)).ToList();
            }
        }

        /// <summary>
        /// Applies the specified Y-coordinate to the Control.Location.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="y"></param>
        public static void SetY(this Control control, int y)
        {
            control.Location = new Point(control.Location.X, y);
        }

        /// <summary>
        /// Builds a PluginConfigurationData object from a VirtualResourceMetadata object.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static PluginConfigurationData BuildConfigurationData(this Data.EnterpriseTest.Model.VirtualResourceMetadata metadata)
        {
            XElement data = XElement.Parse(metadata.Metadata);
            PluginConfigurationData configurationData = new PluginConfigurationData(data, metadata.MetadataVersion);

            if (metadata.AssetUsage != null)
            {
                XElement assetData = XElement.Parse(metadata.AssetUsage.AssetSelectionData);
                configurationData.Assets = Serializer.Deserialize<AssetSelectionData>(assetData);
            }
            else
            {
                configurationData.Assets = new AssetSelectionData();
            }

            if (metadata.DocumentUsage != null)
            {
                XElement documentData = XElement.Parse(metadata.DocumentUsage.DocumentSelectionData);
                configurationData.Documents = Serializer.Deserialize<DocumentSelectionData>(documentData);
            }
            else
            {
                configurationData.Documents = new DocumentSelectionData();
            }

            if (metadata.ServerUsage != null)
            {
                XElement serverData = XElement.Parse(metadata.ServerUsage.ServerSelectionData);
                configurationData.Servers = Serializer.Deserialize<ServerSelectionData>(serverData);
            }
            else
            {
                configurationData.Servers = new ServerSelectionData();
            }

            if (metadata.PrintQueueUsage != null)
            {
                XElement printQueueData = XElement.Parse(metadata.PrintQueueUsage.PrintQueueSelectionData);
                configurationData.PrintQueues = Serializer.Deserialize<PrintQueueSelectionData>(printQueueData);
            }
            else
            {
                configurationData.PrintQueues = new PrintQueueSelectionData();
            }

            return configurationData;
        }
    }
}
