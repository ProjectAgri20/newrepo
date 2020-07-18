using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.UI.Framework;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportCompletionControl : UserControl
    {
        public event EventHandler OnNodeSelected;

        public ImportCompletionControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            saveToRadTreeView.ImageList = IconManager.Instance.ConfigurationIcons;
            saveToRadTreeView.TreeViewElement.Comparer = new FolderSelectionTreeNodeComparer(saveToRadTreeView.TreeViewElement);
        }

        public Guid SelectedFolderId { get; internal set; }

        private void SetFolderId(Guid id)
        {
            SelectedFolderId = id;
            if (OnNodeSelected != null)
            {
                OnNodeSelected(this, new EventArgs());
            }
        }

        public void LoadFolders()
        {
            saveToRadTreeView.Nodes.Clear();

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
                        saveToRadTreeView.Nodes.Add(node);
                    }
                    else
                    {
                        var temp = FindNode(folder.ParentId);
                        if (temp != null)
                        {
                            temp.Nodes.Add(node);
                        }
                        else
                        {
                            saveToRadTreeView.Nodes.Add(node);
                        }

                        //FindNode(folder.ParentId).Nodes.Add(node);
                    }
                }
            }
            FindSelectedNode();
        }

        private void FindSelectedNode()
        {
            if (SelectedFolderId != null && SelectedFolderId != Guid.Empty)
            {
                var node = FindNode(SelectedFolderId);
                if (node != null)
                {
                    node.Selected = true;
                    node.EnsureVisible();
                }
            }
        }

        private void saveToRadTreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                SetFolderId((Guid)e.Node.Tag);
            }
        }

        private void saveToRadTreeView_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                SetFolderId((Guid)e.Node.Tag);
            }
        }

        private RadTreeNode FindNode(Guid? nodeId)
        {
            return saveToRadTreeView.Find(n => n.Tag as Guid? == nodeId);
        }

        /// <summary>
        /// Custom sorter for tree view nodes
        /// </summary>
        private class FolderSelectionTreeNodeComparer : TreeNodeComparer
        {
            public FolderSelectionTreeNodeComparer(RadTreeViewElement treeView)
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

        private void saveToRadTreeView_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                FindSelectedNode();
            }
        }
    }
}
