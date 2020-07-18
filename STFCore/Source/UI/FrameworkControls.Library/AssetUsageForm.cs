using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.UI.Framework
{
    public partial class AssetUsageForm : Form
    {
        private Collection<AssetInfo> _assets = null;

        public AssetUsageForm(Collection<AssetInfo> assets)
        {
            InitializeComponent();
            _assets = assets;

            Text = "Asset Usages";
        }

        private void AssetUsageForm_Load(object sender, EventArgs e)
        {
            asset_TreeView.ImageList = IconManager.Instance.ConfigurationIcons;
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (var item in _assets.Reverse())
                {
                    ResourceUsageTreeItem treeItem = new ResourceUsageTreeItem();
                    treeItem.LoadByAsset(context, item.AssetId);

                    TreeNode rootNode = new TreeNode(item.AssetId);
                    rootNode.ImageKey = "Printing";
                    rootNode.SelectedImageKey = "Printing";

                    AddToTreeControl(rootNode, treeItem.Children);

                    asset_TreeView.Nodes.Add(rootNode);
                }
            }
        }

        private void AddToTreeControl(TreeNode node, Collection<ResourceUsageTreeItem> items)
        {
            foreach (var item in items)
            {
                TreeNode newNode = new TreeNode(item.EntityName);
                newNode.Tag = item;
                newNode.ImageKey = item.EntityType;
                newNode.SelectedImageKey = item.EntityType;
                node.Nodes.Add(newNode);

                AddToTreeControl(newNode, item.Children);
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void asset_TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var data = e.Node.Tag as ResourceUsageTreeItem;

            if (data != null)
            {
                creator_TextBox.Text = data.Creator;
                created_TextBox.Text = data.Created;
                lastRun_TextBox.Text = data.LastRun;
            }
        }

        private class ResourceUsageTreeItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ResourceUsageTreeItem"/> class.
            /// </summary>
            public ResourceUsageTreeItem()
            {
                Children = new Collection<ResourceUsageTreeItem>();
            }

            /// <summary>
            /// Gets the children.
            /// </summary>
            public Collection<ResourceUsageTreeItem> Children { get; private set; }

            /// <summary>
            /// Gets or sets the parent.
            /// </summary>
            public ResourceUsageTreeItem Parent { get; set; }

            /// <summary>
            /// Gets or sets the name of the entity.
            /// </summary>
            public string EntityName { get; set; }

            /// <summary>
            /// Gets or sets the entity unique identifier.
            /// </summary>
            public Guid EntityId { get; set; }

            /// <summary>
            /// Gets or sets the type of the entity.
            /// </summary>
            public string EntityType { get; set; }

            /// <summary>
            /// Gets or sets the creator.
            /// </summary>
            public string Creator { get; set; }

            /// <summary>
            /// Gets or sets the created.
            /// </summary>
            public string Created { get; set; }

            /// <summary>
            /// Gets or sets the last run.
            /// </summary>
            public string LastRun { get; set; }

            /// <summary>
            /// Adds the child.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns></returns>
            public ResourceUsageTreeItem AddChild(string name)
            {
                var child = new ResourceUsageTreeItem() { EntityName = name, Parent = this };
                Children.Add(child);
                return child;
            }

            /// <summary>
            /// Loads the tree by asset.
            /// </summary>
            /// <param name="entities">The entities.</param>
            /// <param name="assetId">The asset unique identifier.</param>
            public void LoadByAsset(EnterpriseTestEntities entities, string assetId)
            {
                EntityName = assetId;

                // For every item that is associated with the given Asset, create a tree hierarchy that
                // contains the Scenario, Virtual Resource and then the Metadata that is associated with
                // the Asset
                foreach (var item in GetAssetUsages(entities, assetId))
                {
                    // First look for the given scenario name
                    var scenario = Children.Where(e => e.EntityId.Equals(item.ScenarioId)).FirstOrDefault();
                    if (scenario == null)
                    {
                        // From the inside out, add a Scenario node, then a child resource node, then a
                        // child metadata node
                        AddMetadataNode(AddResourceNode(AddScenarioNode(item), item), item);
                    }
                    else
                    {
                        // Scenario exists, see if the resource exists.
                        var resource = scenario.Children.Where(e => e.EntityId.Equals(item.ResourceId)).FirstOrDefault();
                        if (resource == null)
                        {
                            // If the resource doesn't exist, add it, then a child metadata node
                            AddMetadataNode(AddResourceNode(scenario, item), item);
                        }
                        else
                        {
                            // Just add a child metadata node to the existing resource node
                            AddMetadataNode(resource, item);
                        }
                    }
                }
            }

            private static IEnumerable<AssetUsageData> GetAssetUsages(EnterpriseTestEntities entities, string assetId)
            {
                return from mau in entities.VirtualResourceMetadataAssetUsages.AsEnumerable()
                       where mau.AssetSelectionData.Contains(assetId)
                       join vrm in entities.VirtualResourceMetadataSet
                           on mau.VirtualResourceMetadataId equals vrm.VirtualResourceMetadataId into Metadata
                       from m in Metadata
                       join vr in entities.VirtualResources
                           on m.VirtualResourceId equals vr.VirtualResourceId into Resources
                       from r in Resources
                       join es in entities.EnterpriseScenarios
                           on r.EnterpriseScenarioId equals es.EnterpriseScenarioId into Scenarios
                       from s in Scenarios

                       select new AssetUsageData
                       {
                           AssetId = assetId,
                           ScenarioName = s.Name,
                           ScenarioId = s.EnterpriseScenarioId,
                           ResourceName = r.Name,
                           ResourceType = r.ResourceType,
                           ResourceId = m.VirtualResourceId,
                           MetadataDescription = m.Name,
                           MetadataType = m.MetadataType,
                           MetadataId = m.VirtualResourceMetadataId,
                       };
            }

            private static void AddMetadataNode(ResourceUsageTreeItem resource, AssetUsageData item)
            {
                // Add the metadata information
                if (item.MetadataId != Guid.Empty)
                {
                    // Look for the metadata node under the resource node, and if not found, add it.
                    var metadata = resource.Children.Where(e => e.EntityId.Equals(item.MetadataId)).FirstOrDefault();
                    if (metadata == null)
                    {
                        var metadataNode = resource.AddChild(item.MetadataDescription);
                        metadataNode.EntityId = item.MetadataId;
                        metadataNode.EntityType = item.MetadataType;
                        metadataNode.Creator = item.MetadataCreator;
                        metadataNode.Created = item.ResourceCreated == null ? string.Empty : ((DateTime)item.ResourceCreated).ToString();
                        metadataNode.LastRun = item.ResourceLastRun == null ? string.Empty : ((DateTime)item.ResourceLastRun).ToString();
                    }
                }
            }

            private ResourceUsageTreeItem AddScenarioNode(AssetUsageData item)
            {
                var scenarioNode = AddChild(item.ScenarioName);
                scenarioNode.EntityId = item.ScenarioId;
                scenarioNode.EntityType = "Scenario";
                scenarioNode.Creator = item.ScenarioCreator;
                scenarioNode.Created = item.ResourceCreated == null ? string.Empty : ((DateTime)item.ResourceCreated).ToString();
                scenarioNode.LastRun = item.ResourceLastRun == null ? string.Empty : ((DateTime)item.ResourceLastRun).ToString();

                return scenarioNode;
            }

            private static ResourceUsageTreeItem AddResourceNode(ResourceUsageTreeItem scenario, AssetUsageData item)
            {
                var resourceNode = scenario.AddChild(item.ResourceName);
                resourceNode.EntityId = item.ResourceId;
                resourceNode.EntityType = item.ResourceType;
                resourceNode.Creator = item.ResourceCreator;
                resourceNode.Created = item.ResourceCreated == null ? string.Empty : ((DateTime)item.ResourceCreated).ToString();
                resourceNode.LastRun = item.ResourceLastRun == null ? string.Empty : ((DateTime)item.ResourceLastRun).ToString();

                return resourceNode;
            }

            private static string GetValue(object item, string property)
            {
                return item.GetType().GetProperty(property).GetValue(item, null).ToString();
            }
        }

        private class AssetUsageData
        {
            public string AssetId { get; set; }
            public string ScenarioName { get; set; }
            public Guid ScenarioId { get; set; }
            public string ScenarioCreator { get; set; }
            public DateTime? ScenarioCreated { get; set; }
            public DateTime? ScenarioLastRun { get; set; }
            public string ResourceName { get; set; }
            public string ResourceType { get; set; }
            public Guid ResourceId { get; set; }
            public string ResourceCreator { get; set; }
            public DateTime? ResourceCreated { get; set; }
            public DateTime? ResourceLastRun { get; set; }
            public string MetadataDescription { get; set; }
            public string MetadataType { get; set; }
            public Guid MetadataId { get; set; }
            public string MetadataCreator { get; set; }
            public DateTime? MetadataCreated { get; set; }
            public DateTime? MetadataLastRun { get; set; }
        }
    }
}
