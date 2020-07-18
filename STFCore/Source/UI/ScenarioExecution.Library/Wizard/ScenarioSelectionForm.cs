using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    /// <summary>
    /// Utility Form for selecting a scenario.
    /// </summary>
    internal partial class ScenarioSelectionForm : Form
    {
        /// <summary>
        /// The selected EnterpriseScenarioId
        /// </summary>
        public Guid SelectedScenarioId { get; private set; }

        /// <summary>
        /// Creates a new instance of ScenarioSelectionForm.
        /// </summary>
        public ScenarioSelectionForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            scenarioTreeView.ImageList = IconManager.Instance.ConfigurationIcons;
            scenarioTreeView.TreeViewElement.Comparer = new ScenarioSelectionTreeNodeComparer(scenarioTreeView.TreeViewElement);
        }

        private void ScenarioSelectionForm_Load(object sender, EventArgs e)
        {
            bool noOrphans = true;

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var folders = ConfigurationTreeFolder.Select(context, "ScenarioFolder");
                foreach (ConfigurationTreeFolder folder in ConfigurationTreeFolder.SortHierarchical(folders))
                {
                    RadTreeNode node = new RadTreeNode(folder.Name);
                    node.Tag = folder.ConfigurationTreeFolderId;
                    node.ImageKey = "Folder";

                    noOrphans &= AddNode(node, folder.ParentId);
                }

                foreach (EnterpriseScenario scenario in context.EnterpriseScenarios)
                {
                    RadTreeNode node = new RadTreeNode(scenario.Name);
                    node.Tag = scenario.EnterpriseScenarioId;
                    node.ImageKey = "Scenario";

                    noOrphans &= AddNode(node, scenario.FolderId);
                }
            }

            if (!noOrphans)
            {
                MessageBox.Show("One or more scenarios or folders did not load into the tree successfully.\n" +
                                "Please contact an administrator to determine which database items have incorrect configuration.",
                                "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool AddNode(RadTreeNode node, Guid? parentId)
        {
            if (parentId == null)
            {
                scenarioTreeView.Nodes.Add(node);
            }
            else
            {
                RadTreeNode parentNode = FindNode(parentId);
                if (parentNode == null)
                {
                    return false;
                }
                parentNode.Nodes.Add(node);
            }
            return true;
        }

        private void select_Button_Click(object sender, EventArgs e)
        {
            RadTreeNode selectedNode = scenarioTreeView.SelectedNode;
            if (selectedNode != null)
            {
                SelectedScenarioId = (Guid)selectedNode.Tag;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void scenarioTreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            select_Button.Enabled = IsScenario(e.Node);
        }

        private void scenarioTreeView_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e)
        {
            if (IsScenario(e.Node))
            {
                select_Button_Click(sender, EventArgs.Empty);
            }
        }

        private static bool IsScenario(RadTreeNode node)
        {
            return node.ImageKey == "Scenario";
        }

        private RadTreeNode FindNode(Guid? nodeId)
        {
            return scenarioTreeView.Find(n => n.Tag as Guid? == nodeId);
        }

        /// <summary>
        /// Custom sorter for tree view nodes
        /// </summary>
        private class ScenarioSelectionTreeNodeComparer : TreeNodeComparer
        {
            public ScenarioSelectionTreeNodeComparer(RadTreeViewElement treeView)
                : base(treeView)
            {
            }

            // Modify sorting so that folders come first
            public override int Compare(RadTreeNode x, RadTreeNode y)
            {
                if (x == null)
                {
                    throw new ArgumentNullException("x");
                }

                if (y == null)
                {
                    throw new ArgumentNullException("y");
                }

                // "Folder" comes before "Scenario", so just concatenate the image key and text and then compare
                return string.Compare(x.ImageKey + x.Text, y.ImageKey + y.Text, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
