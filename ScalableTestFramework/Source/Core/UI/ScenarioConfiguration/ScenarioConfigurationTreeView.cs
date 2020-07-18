using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Core.EnterpriseTest.Configuration;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.Core.UI.ScenarioConfiguration
{
    /// <summary>
    /// Tree view for displaying objects related to scenario configuration.
    /// </summary>
    public partial class ScenarioConfigurationTreeView : RadTreeView
    {
        private readonly Dictionary<Guid, ConfigurationTreeNode> _treeViewNodes = new Dictionary<Guid, ConfigurationTreeNode>();
        private ScenarioConfigurationUIController _uiController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioConfigurationTreeView" /> class.
        /// </summary>
        public ScenarioConfigurationTreeView()
        {
            InitializeComponent();
            ThemeClassName = typeof(RadTreeView).FullName;
        }

        /// <summary>
        /// Initializes this configuration tree view for use.
        /// </summary>
        /// <param name="uiController">The <see cref="ScenarioConfigurationUIController" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uiController" /> is null.</exception>
        public void Initialize(ScenarioConfigurationUIController uiController)
        {
            ImageList = IconManager.Instance.ConfigurationIcons;

            _uiController = uiController ?? throw new ArgumentNullException(nameof(uiController));
            _uiController.ConfigurationObjectsChanged += (s, e) => UpdateTreeView(e.ChangeSet);
        }

        /// <summary>
        /// Updates the tree view to reflect the changes in the specified <see cref="ConfigurationObjectChangeSet" />.
        /// </summary>
        /// <param name="changeSet">The <see cref="ConfigurationObjectChangeSet" />.</param>
        public void UpdateTreeView(ConfigurationObjectChangeSet changeSet)
        {
            BeginUpdate();

            foreach (ConfigurationObjectTag tag in changeSet.AddedObjects)
            {
                HandleAdded(tag);
            }

            foreach (ConfigurationObjectTag tag in changeSet.ModifiedObjects)
            {
                HandleModified(tag);
            }

            foreach (ConfigurationObjectTag tag in changeSet.RemovedObjects)
            {
                HandleRemoved(tag);
            }

            EndUpdate();
        }

        private void HandleAdded(ConfigurationObjectTag tag)
        {
            ConfigurationTreeNode node = new ConfigurationTreeNode(tag);
            _treeViewNodes.Add(tag.Id, node);

            ConfigurationTreeNode parent = GetNode(node.ParentNodeId);
            if (parent != null)
            {
                parent.Nodes.Add(node);
            }
            else
            {
                Nodes.Add(node);
            }
        }

        private void HandleModified(ConfigurationObjectTag tag)
        {
            GetNode(tag.Id)?.Refresh();
        }

        private void HandleRemoved(ConfigurationObjectTag tag)
        {
            ConfigurationTreeNode node = GetNode(tag.Id);
            if (node != null)
            {
                if (node.Parent != null)
                {
                    node.Parent.Nodes.Remove(node);
                }
                else
                {
                    Nodes.Remove(node);
                }

                _treeViewNodes.Remove(tag.Id);
            }
        }

        /// <summary>
        /// Clears all nodes from this tree view.
        /// </summary>
        public void ClearTreeView()
        {
            Nodes.Clear();
            _treeViewNodes.Clear();
        }

        private ConfigurationTreeNode GetNode(Guid? nodeId)
        {
            ConfigurationTreeNode node = null;
            if (nodeId.HasValue)
            {
                _treeViewNodes.TryGetValue(nodeId.Value, out node);
            }
            return node;
        }

        #region ConfigurationTreeNode Class

        private class ConfigurationTreeNode : RadTreeNode
        {
            public ConfigurationObjectTag ConfigurationObjectTag { get; }

            public Guid? ParentNodeId => ConfigurationObjectTag.FolderId ?? ConfigurationObjectTag.ParentId;

            public ConfigurationTreeNode(ConfigurationObjectTag tag)
            {
                ConfigurationObjectTag = tag;
                Refresh();
            }

            public void Refresh()
            {
                Text = ConfigurationObjectTag.Name;

                switch (ConfigurationObjectTag.ObjectType)
                {
                    case ConfigurationObjectType.ScenarioFolder:
                    case ConfigurationObjectType.ResourceFolder:
                    case ConfigurationObjectType.MetadataFolder:
                        ImageKey = "Folder";
                        break;

                    case ConfigurationObjectType.EnterpriseScenario:
                        ImageKey = "Scenario";
                        break;

                    case ConfigurationObjectType.VirtualResource:
                        ImageKey = ConfigurationObjectTag.ResourceType.ToString();
                        break;

                    case ConfigurationObjectType.VirtualResourceMetadata:
                        ImageKey = ConfigurationObjectTag.MetadataType;
                        break;
                }

                // If the node is disabled, modify the key
                if (ConfigurationObjectTag.EnabledState == EnabledState.Disabled)
                {
                    ImageKey += "Disabled";
                }
            }
        }

        #endregion

        #region Custom Tree View UI Behavior

        /// <summary>
        /// Creates a custom <see cref="ScenarioConfigurationTreeViewElement" /> object.
        /// </summary>
        /// <returns>A <see cref="ScenarioConfigurationTreeViewElement" />.</returns>
        protected override RadTreeViewElement CreateTreeViewElement()
        {
            return new ScenarioConfigurationTreeViewElement();
        }

        private sealed class ScenarioConfigurationTreeViewElement : RadTreeViewElement
        {
            public ScenarioConfigurationTreeViewElement()
            {
                Comparer = new ScenarioConfigurationTreeNodeComparer(this);
            }

            // Enable theming for the element
            protected override Type ThemeEffectiveType => typeof(RadTreeViewElement);

            // Prevent multi-selection by dragging with mouse
            protected override bool ProcessMouseMove(MouseEventArgs e) => false;

            // Implement custom comparer for sorting
            private sealed class ScenarioConfigurationTreeNodeComparer : TreeNodeComparer
            {
                public ScenarioConfigurationTreeNodeComparer(RadTreeViewElement treeViewElement)
                    : base(treeViewElement)
                {
                }

                public override int Compare(RadTreeNode x, RadTreeNode y)
                {
                    // Folders come before other items; then sort alphabetically
                    bool xIsFolder = x.ImageKey.Contains("Folder");
                    bool yIsFolder = y.ImageKey.Contains("Folder");

                    if (yIsFolder && !xIsFolder)
                    {
                        return 1;
                    }
                    else if (xIsFolder && !yIsFolder)
                    {
                        return -1;
                    }
                    else
                    {
                        return string.Compare(x.Text, y.Text);
                    }
                }
            }
        }

        #endregion
    }
}
