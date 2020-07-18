using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.SolutionTest.WorkBench
{
    internal partial class ScenarioSelectionForm : Form
    {
        public Guid SelectedScenarioId { get; private set; }

        public ScenarioSelectionForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            scenarioTreeView.ImageList = IconManager.Instance.ConfigurationIcons;
            scenarioTreeView.TreeViewElement.Comparer = new ScenarioSelectionTreeNodeComparer(scenarioTreeView.TreeViewElement);
        }

        private void ScenarioSelectionForm_Load(object sender, EventArgs e)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var folders = ConfigurationTreeFolder.Select(context, "ScenarioFolder");
                foreach (ConfigurationTreeFolder folder in ConfigurationTreeFolder.SortHierarchical(folders))
                {
                    RadTreeNode node = new RadTreeNode(folder.Name);
                    node.Tag = folder.ConfigurationTreeFolderId;
                    node.ImageKey = "Folder";

                    if (folder.ParentId == null)
                    {
                        scenarioTreeView.Nodes.Add(node);
                    }
                    else
                    {
                        FindNode(folder.ParentId).Nodes.Add(node);
                    }
                }

                foreach (EnterpriseScenario scenario in context.EnterpriseScenarios)
                {
                    RadTreeNode node = new RadTreeNode(scenario.Name);
                    node.Tag = scenario.EnterpriseScenarioId;
                    node.ImageKey = "Scenario";

                    if (scenario.FolderId == null)
                    {
                        scenarioTreeView.Nodes.Add(node);
                    }
                    else
                    {
                        FindNode(scenario.FolderId).Nodes.Add(node);
                    }
                }
            }
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
