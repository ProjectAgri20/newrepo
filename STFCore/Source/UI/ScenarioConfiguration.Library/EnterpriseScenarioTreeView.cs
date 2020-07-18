using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.Utility;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Tree view for displaying objects related to scenario configuration.
    /// </summary>
    public partial class EnterpriseScenarioTreeView : RadTreeView
    {
        /// <summary>
        /// Notification when a scenario has finished being imported.
        /// </summary>
        public event EventHandler ImportCompleted;
        /// <summary>
        /// Notification when configuration status has changed.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> OnStatusChanged;
        /// <summary>
        /// Notification when a request for startup of a group of scenarios has been initiated.
        /// </summary>
        public event EventHandler<RadTreeViewEventArgs> OnStartBatch;

        private EnterpriseTestUIController _controller;
        private Dictionary<Guid, RadTreeNode> _nodeMap = new Dictionary<Guid, RadTreeNode>();

        private Font _enabledFont;
        private Font _disabledFont;
        private Guid _lastSelectedId;
        //------------------------------------------------------------------------------------------------------
        // ADD: The following four member variables have been added to aid in the implementation of general
        // Cut/Copy/Paste functionality.
        //  _selectedIds is a List of Guids that holds all the subtree "root nodes" of items that have been
        //               selected. List is used To simplify extraction. Limited searching.
        //  _selectedSubNodeIds is a Dictionary of all nodes (in all subtrees) that are selected. It is a
        //               a dictionary because it is used to search for nodes that need to be highlighted in
        //               the UI.  The search performance is significantly better than for an un-ordered List.
        // _inPaste is a bool that allows us to determine when we are copying or cut/pasting so that we can
        //               avoid some things (like clearing the selected list when a node is added).
        private List<Guid> _selectedIds;
        private Dictionary<Guid, Guid> _selectedSubNodeIds;
        private bool _inPaste;

        /// <summary>
        /// Returns the selected configuration object.
        /// </summary>
        public ConfigurationObjectTag SelectedConfigurationObject
        {
            get
            {
                RadTreeNode selectedNode = this.SelectedNode;

                if (selectedNode != null)
                {
                    return _controller.GetObjectTag((Guid)selectedNode.Tag);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the first scenario configuration object at or above the selected tree node.
        /// </summary>
        /// <value>The selected scenario configuration object.</value>
        public ConfigurationObjectTag SelectedScenarioConfigurationObject
        {
            get
            {
                return GetConfigurationObjectTagAncestry(this.SelectedNode).FirstOrDefault(x => x.ObjectType == ConfigurationObjectType.EnterpriseScenario);
            }
        }

        /// <summary>
        /// Returns the number of selected trees.
        /// </summary>
        /// <returns></returns>
        public int NumberOfSelectedTrees()
        {
            return _selectedIds.Count;
        }


        /// <summary>
        /// Clear current McSelections so you can McOrder something McNew.
        /// </summary>
        public void ClearMCSelections()
        {
            if (!_inPaste)
            {
                _selectedIds.Clear();
                _selectedSubNodeIds.Clear();
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to allow creation of scenarios at the root level.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if scenarios can be created at the root level; otherwise, <c>false</c>.
        /// </value>
        public bool AllowRootScenarioCreation { get; set; }

        /// <summary>
        /// Occurs when the selected node is changing.
        /// </summary>
        public event EventHandler<RadTreeViewCancelEventArgs> ConfigurationObjectSelectionChanging;

        /// <summary>
        /// Occurs when a node is selected.
        /// </summary>
        public event EventHandler<ConfigurationTagEventArgs> ConfigurationObjectSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseScenarioTreeView"/> class.
        /// </summary>
        public EnterpriseScenarioTreeView()
        {
            InitializeComponent();

            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ThemeClassName = typeof(RadTreeView).FullName;
            this.TreeViewElement.Comparer = new EnterpriseScenarioTreeNodeComparer(this.TreeViewElement);

            _enabledFont = this.Font;
            _disabledFont = new Font(_enabledFont, FontStyle.Italic);
            _selectedIds = new List<Guid>();
            _selectedSubNodeIds = new Dictionary<Guid, Guid>();
            _inPaste = false;
        }

        /// <summary>
        /// Initializes this instance by specifying the <see cref="EnterpriseTestUIController"/>.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public void Initialize(EnterpriseTestUIController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            _controller = controller;
            _controller.NodeAdded += new EventHandler<EnterpriseTestUIEventArgs>(controller_NodeAdded);
            _controller.NodeModified += new EventHandler<EnterpriseTestUIEventArgs>(controller_NodeModified);
            _controller.NodeRemoved += new EventHandler<EnterpriseTestUIEventArgs>(controller_NodeRemoved);
        }

        /// <summary>
        /// Clears all nodes in this instance.
        /// </summary>
        public void Clear()
        {
            // Some of the icons are loaded from the database, so this can't be initialized in the constructor.
            this.ImageList = IconManager.Instance.ConfigurationIcons;

            this.Nodes.Clear();
            _nodeMap.Clear();
        }

        private void controller_NodeAdded(object sender, EnterpriseTestUIEventArgs e)
        {
            RadTreeNode node = new RadTreeNode();
            node.Tag = e.Id;
            node.Text = e.Name;
            node.ImageKey = e.ImageKey;
            Attach(node, e.ParentId);
        }

        private void controller_NodeModified(object sender, EnterpriseTestUIEventArgs e)
        {
            RadTreeNode node = FindNode(e.Id);
            if (node != null)
            {
                node.Text = e.Name;
                node.ImageKey = e.ImageKey;

                // Only move the node if we need to
                if (GetParentId(node) != e.ParentId)
                {
                    Detach(node);
                    Attach(node, e.ParentId);
                }
            }
        }

        private void controller_NodeRemoved(object sender, EnterpriseTestUIEventArgs e)
        {
            RadTreeNode node = FindNode(e.Id);
            if (node != null)
            {
                Detach(node);
                RefreshTree();
            }
        }

        private void EnterpriseScenarioTreeView_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            // If we are in the middle of a drag/drop, don't fire the event
            var dragDropService = this.TreeViewElement.DragDropService as EnterpriseScenarioTreeDragDropService;
            if (dragDropService.DoingDrag)
            {
                return;
            }
            if ((Control.MouseButtons & MouseButtons.Right) == MouseButtons.Right)
            {
                return;
            }

            if (ConfigurationObjectSelectionChanging != null)
            {
                ConfigurationObjectSelectionChanging(this, e);
            }
        }

        private void EnterpriseScenarioTreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            // If we are in the middle of a drag/drop, don't fire the event
            var dragDropService = this.TreeViewElement.DragDropService as EnterpriseScenarioTreeDragDropService;
            if ((dragDropService.DoingDrag) || (_inPaste))
            {
                return;
            }

            if (Control.MouseButtons == MouseButtons.Right)
            {
                return;
            }

            if (e.Node != null)
            {
                Guid selectedId = (Guid)e.Node.Tag;
                if ((Control.MouseButtons & MouseButtons.Right) != MouseButtons.Right)
                {
                    if (_selectedIds.Count > 0)
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control && _controller.NodeCCPGroupsMatch(selectedId, _selectedIds[0]))
                        {
                            SelectWithControlKey(selectedId, e.Node);
                            return;
                        }
                        else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            SelectWithShiftKey(selectedId, e.Node);
                            return;
                        }
                    }
                    _selectedIds.Clear();
                    _selectedSubNodeIds.Clear();
                    _selectedIds.Add(selectedId);
                    _selectedSubNodeIds.Add(selectedId, selectedId);
                    GrabAllChildren(e.Node.FirstNode);
                }

                if (selectedId != _lastSelectedId)
                {
                    // Get the tag information for the selected node
                    ConfigurationObjectTag tag = _controller.GetObjectTag((Guid)e.Node.Tag);
                    if (tag == null)
                    {
                        // We tried to get information for an object that the controller couldn't find
                        // Log the Id for debugging purposes.
                        e.Node.Enabled = false;
                        TraceFactory.Logger.Debug("The selected object was not found.  Id={0}".FormatWith(selectedId));
                        return;
                    }

                    if (ConfigurationObjectSelected != null)
                    {
                        ConfigurationObjectSelected(this, new ConfigurationTagEventArgs(tag));
                    }

                    _lastSelectedId = selectedId;
                }
            }
        }

        /// <summary>
        /// Adds the currently selected node to the list of selected nodes.
        /// </summary>
        /// <param name="selectedId">The Id of the selected node.</param>
        /// <param name="selectedNode">The selected node.</param>
        private void SelectWithControlKey(Guid selectedId, RadTreeNode selectedNode)
        {
            if (_selectedSubNodeIds.ContainsValue(selectedId))
            {
                if (_selectedIds.Count > 1)
                {
                    //-----------------------------------------------------------------------------
                    //ADD: This could be really complicated - so instead, we will keep it simple.
                    // If the tester selects a SubTree of a tree that is already selected, we
                    // will NOT remove that section from the selected list - because in the case of
                    // a drag/drop or a cut, it would create an orphan. If the selection was the
                    // 'root' of what was already selected, we will remove it from the list.
                    if (_selectedIds.Contains(selectedId))
                    {
                        _selectedIds.Remove(selectedId);
                        _selectedSubNodeIds.Remove(selectedId);
                        RemoveAllChildren(selectedNode.FirstNode);
                    }
                }
                return;
            }

            if (!_selectedIds.Contains(selectedId))
            {
                _selectedIds.Add(selectedId);
                _selectedSubNodeIds.Add(selectedId, selectedId);
                GrabAllChildren(selectedNode.FirstNode);
            }
        }

        /// <summary>
        /// Selects all nodes between the currently selected node and the last selected node.
        /// </summary>
        /// <param name="selectedId">The Id of the selected node.</param>
        /// <param name="selectedNode">The selected node.</param>
        private void SelectWithShiftKey(Guid selectedId, RadTreeNode selectedNode)
        {
            if (!_selectedIds.Contains(selectedId))
            {
                if (_selectedIds.Count == 1)
                {
                    RadTreeNode lastSelectedNode = FindNode(_selectedIds.FirstOrDefault());
                    if (lastSelectedNode != null && selectedNode.Parent == lastSelectedNode.Parent)
                    {
                        int start = Math.Min(selectedNode.Index, lastSelectedNode.Index);
                        int end = Math.Max(selectedNode.Index, lastSelectedNode.Index);

                        // Make sure we don't re-add the previously selected Id
                        if (lastSelectedNode.Index < selectedNode.Index)
                        {
                            start++;
                        }
                        else
                        {
                            end--;
                        }

                        Guid nodeId = Guid.Empty;
                        for (int i = start; i <= end; i++)
                        {
                            RadTreeNode siblingNode = GetSibling(selectedNode, i);
                            if (siblingNode != null)
                            {
                                nodeId = (Guid)siblingNode.Tag;
                                _selectedIds.Add(nodeId);
                                _selectedSubNodeIds.Add(nodeId, nodeId);
                                GrabAllChildren(siblingNode.FirstNode);
                            }
                        }
                    }
                }
            }
        }

        private void EnterpriseScenarioTreeView_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            if (e.Node.ImageKey.EndsWith("Disabled", StringComparison.Ordinal))
            {
                //----------------------------------------------------------------------------------------------
                // ADD: I need a way to highlight that a node/nodes/subtree is part of the selected "group".
                // Searching the _selectedIds List is really slow and causes significant UI delay. So -
                // _selectedSubNodeIds was created and holds all the ids of all nodes that are "selected" either
                // directly, or indirectly (as part of a subtree). Dictionaries are very fast for searching
                // because it uses a hash (something along the lines of O(1)).  Could also use a HashTable if
                // we decided to do so.
                e.Node.ForeColor = _selectedSubNodeIds.ContainsValue((Guid)e.Node.Tag) ? SystemColors.HotTrack : SystemColors.ControlText;
                e.NodeElement.ContentElement.Font = _disabledFont;
            }
            else
            {
                //----------------------------------------------------------------------------------------------
                // ADD: See above comment.
                e.Node.ForeColor = _selectedSubNodeIds.ContainsValue((Guid)e.Node.Tag) ? SystemColors.HotTrack : SystemColors.ControlText;
                e.NodeElement.ContentElement.Font = _enabledFont;
            }
        }

        /// <summary>
        /// Selects the node with the specified ID.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        public void SelectNode(Guid? nodeId)
        {
            RadTreeNode node = FindNode(nodeId);
            if (node != null)
            {
                node.Selected = true;
            }
        }

        /// <summary>
        /// Expands the nodes with the specified IDs.
        /// </summary>
        /// <param name="nodeIds">The node ids.</param>
        public void ExpandNodes(IEnumerable<Guid?> nodeIds)
        {
            foreach (Guid? nodeId in nodeIds)
            {
                RadTreeNode node = FindNode(nodeId);
                if (node != null)
                {
                    node.Expand();
                }
            }
        }

        #region Context Menu

        private RadTreeNode _contextMenuNode;

        private void EnterpriseScenarioTreeView_ContextMenuOpening(object sender, TreeViewContextMenuOpeningEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Enabled)
                {
                    // Set the context menu node - we will need it when a menu option is selected
                    _contextMenuNode = e.Node;
                    ConfigureContextMenu((Guid)e.Node.Tag);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                // We have right-clicked somewhere else on the treeview - only allow addition of folders/scenarios
                new_MenuItem.Enabled = true;
                SetNewScenarioVisibility(AllowRootScenarioCreation);
                SetExpandCollapseVisibility(true);

                // Set visibility for other options
                rename_MenuItem.Visibility = ElementVisibility.Collapsed;
                delete_MenuItem.Visibility = ElementVisibility.Collapsed;
                importScenario_MenuItem.Visibility = ElementVisibility.Collapsed;
                exportScenario_MenuItem.Visibility = ElementVisibility.Collapsed;
                startBatch_MenuItem.Visibility = ElementVisibility.Collapsed;
                //----------------------------------------------------------------------------------------------
                // ADD: Setting context menu enabled state for the Move Here/Copy Here menu items. This
                // is pretty straightforward.  Only Folders or scenarios can be at the root. _selectedIds holds.
                // similar items, so if the first one is a folder or a scenario, enable Move Here/Copy Here -
                // otherwise, do not enable them
                cutScenario_MenuItem.Enabled = (_selectedIds.Count > 0) && (_controller.IsScenarioFolder(_selectedIds[0]) || _controller.IsScenario(_selectedIds[0]));
                copyscenario_MenuItem.Enabled = (_selectedIds.Count > 0) && (_controller.IsScenarioFolder(_selectedIds[0]) || _controller.IsScenario(_selectedIds[0]));

                SetEnableDisableVisibility(false);
            }
        }

        private void ConfigureContextMenu(Guid nodeId)
        {
            // Add the appropriate "new" options based on the selected node
            new_MenuItem.Enabled = _controller.CanContainFolder(nodeId);
            SetNewScenarioVisibility(_controller.CanContainScenario(nodeId));

            //----------------------------------------------------------------------------------------------
            // ADD: Setting context menu enabled state for Move Here/Copy Here menu items. Since
            // the context menu was not popped from the root - check and see what is selected and if it
            // could be drag/dropped to this location.  Nove/Copy are enabled ONLY when:
            //   1) One or more 'nodes' are selected.
            //   2) The first thing in the selected list passes the 'CanDrop' test- on every item in
            //         _selecetdIds because we would allow the selection of multiple 'types' of
            //         metadata (for a VR), but not every VR supports all varieties of metadata.
            //   3) The target is NOT in any of the selected 'subtrees'
            cutScenario_MenuItem.Enabled = copyscenario_MenuItem.Enabled = false;
            if (_selectedIds.Count > 0)
            {
                RadTreeNode activeNode = FindNode(nodeId);
                EnterpriseScenarioTreeView treeView = activeNode.TreeView as EnterpriseScenarioTreeView;
                bool isOK = !_selectedSubNodeIds.ContainsValue(nodeId);
                for (int ndx = 0; (ndx < _selectedIds.Count) && (isOK); ndx++)
                {
                    isOK = treeView._controller.CanDrop(_selectedIds[ndx], nodeId);
                }
                cutScenario_MenuItem.Enabled = copyscenario_MenuItem.Enabled = isOK;
            }

            // Rename and delete are always available
            rename_MenuItem.Visibility = ElementVisibility.Visible;
            delete_MenuItem.Visibility = ElementVisibility.Visible;
            importScenario_MenuItem.Visibility = ElementVisibility.Visible;
            exportScenario_MenuItem.Visibility = ElementVisibility.Visible;
            startBatch_MenuItem.Visibility = ElementVisibility.Visible;
            delete_MenuItem.Enabled = _selectedIds.Count <= 1;

            exportScenario_MenuItem.Enabled = _controller.CanExport(nodeId);
            importScenario_MenuItem.Enabled = _controller.IsScenarioFolder(nodeId);
            startBatch_MenuItem.Enabled = _controller.IsFolder((Guid)_contextMenuNode.Tag);

            // Can only expand/collapse if there are children
            SetExpandCollapseVisibility(_contextMenuNode.Nodes.Count > 0);

            // Ask the controller if this resource can be enabled or disabled
            var enableDisableStatus = _controller.CanEnableDisable(nodeId);
            SetEnableDisableVisibility(enableDisableStatus.Item1);
            enableDisable_MenuItem.Text = enableDisableStatus.Item2;
        }

        private void SetNewScenarioVisibility(bool value)
        {
            ElementVisibility visibility = value ? ElementVisibility.Visible : ElementVisibility.Collapsed;
            newScenario_MenuItem.Visibility = visibility;
            newMenu_SeparatorItem.Visibility = visibility;
        }

        private void SetExpandCollapseVisibility(bool value)
        {
            ElementVisibility visibility = value ? ElementVisibility.Visible : ElementVisibility.Collapsed;
            expand_MenuItem.Visibility = visibility;
            collapse_MenuItem.Visibility = visibility;
            expandCollapse_SeparatorItem.Visibility = visibility;
        }

        private void SetEnableDisableVisibility(bool value)
        {
            ElementVisibility visibility = value ? ElementVisibility.Visible : ElementVisibility.Collapsed;
            enableDisable_MenuItem.Visibility = visibility;
            enableDisable_SeparatorItem.Visibility = visibility;
        }

        private void newFolder_MenuItem_Click(object sender, EventArgs e)
        {
            Guid folderId;
            if (_contextMenuNode != null)
            {
                folderId = _controller.CreateFolder((Guid)_contextMenuNode.Tag);
                _contextMenuNode = null;
            }
            else
            {
                folderId = _controller.CreateFolder(null);
            }

            SelectNode(folderId);
            ClearMCSelections();
        }

        private void newScenario_MenuItem_Click(object sender, EventArgs e)
        {
            Guid scenarioId;
            if (_contextMenuNode != null)
            {
                scenarioId = _controller.CreateScenario((Guid)_contextMenuNode.Tag);
                _contextMenuNode = null;
            }
            else
            {
                scenarioId = _controller.CreateScenario(null);
            }

            SelectNode(scenarioId);
            ClearMCSelections();
        }

        private void rename_MenuItem_Click(object sender, EventArgs e)
        {
            this.AllowEdit = true;
            this.Edited += new TreeNodeEditedEventHandler(rename_MenuItem_Complete);

            string text = _contextMenuNode.Text;
            _contextMenuNode.BeginEdit();
            _contextMenuNode.Text = text;
        }

        private void rename_MenuItem_Complete(object sender, TreeNodeEditedEventArgs e)
        {
            this.AllowEdit = false;
            this.Edited -= new TreeNodeEditedEventHandler(rename_MenuItem_Complete);

            _controller.Rename((Guid)_contextMenuNode.Tag, _contextMenuNode.Text);
            _contextMenuNode = null;
        }

        private void delete_MenuItem_Click(object sender, EventArgs e)
        {
            // Pop a dialog to confirm this with the user
            DialogResult result = MessageBox.Show
                ("Are you sure you want to delete '{0}'?".FormatWith(_contextMenuNode.Text),
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                this.SuspendLayout();
                var expanded = FindNodes(n => n.Expanded).Select(n => n.Tag as Guid?).ToList();
                _controller.Delete((Guid)_contextMenuNode.Tag);
                this.SelectedNode = null;
                expanded.Remove((Guid?)_contextMenuNode.Tag);
                ExpandNodes(expanded);
                this.ResumeLayout();
                ClearMCSelections();
            }

            _contextMenuNode = null;
        }

        private void startBatch_MenuItem_Click(object sender, EventArgs e)
        {
            OnStartBatch?.Invoke(this, new RadTreeViewEventArgs(this.SelectedNode));
        }

        private void expand_MenuItem_Click(object sender, EventArgs e)
        {
            if (_contextMenuNode != null)
            {
                _contextMenuNode.ExpandAll();
                _contextMenuNode = null;
            }
            else
            {
                this.ExpandAll();
            }
        }

        private void collapse_MenuItem_Click(object sender, EventArgs e)
        {
            if (_contextMenuNode != null)
            {
                _contextMenuNode.Collapse();
                _contextMenuNode = null;
            }
            else
            {
                this.CollapseAll();
            }
        }

        private void enableDisable_MenuItem_Click(object sender, EventArgs e)
        {
            Guid nodeId = (Guid)_contextMenuNode.Tag;

            switch (enableDisable_MenuItem.Text)
            {
                case "Enable":
                    _controller.EnableDisable(nodeId, true);
                    break;

                case "Disable":
                    _controller.EnableDisable(nodeId, false);
                    break;
            }
            _contextMenuNode = null;
        }

        private void exportScenario_MenuItem_Click(object sender, EventArgs e)
        {
            Guid nodeId = (Guid)_contextMenuNode.Tag;
            if (_controller.IsScenario(nodeId))
            {
                ExportScenario(nodeId);
            }
            else if (_controller.IsResourceMetaData(nodeId))
            {
                ExportMetadata(nodeId);
            }
        }

        private void importScenario_MenuItem_Click(object sender, EventArgs e)
        {
            Guid nodeId = (Guid)_contextMenuNode.Tag;
            if (_controller.IsScenarioFolder(nodeId))
            {
                using (ScenarioImportWizardForm form = new ScenarioImportWizardForm(nodeId))
                {
                    form.ShowDialog();

                    if (form.ScenarioImported)
                    {
                        OnImportCompleted();
                    }
                }
            }
        }

        private void ExportScenario(Guid scenarioId)
        {
            bool includePrinters = false;
            bool includeDocuments = false;

            EnterpriseScenario scenario = null;
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                scenario = EnterpriseScenario.SelectWithAllChildren(context, scenarioId);

                using (ExportOptionsForm optionsForm = new ExportOptionsForm(scenario.Name))
                {
                    if (optionsForm.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        includeDocuments = optionsForm.IncludeDocuments;
                        includePrinters = optionsForm.IncludePrinters;
                    }
                }
            }

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            using (var dialog = new ExportSaveFileDialog(directory, "Export Test Scenario Data", ImportExportType.Scenario))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    directory = Path.GetDirectoryName(dialog.Base.FileName);

                    BackgroundWorker exportWorker = new BackgroundWorker();
                    exportWorker.DoWork += ExportScenario_DoWork;
                    exportWorker.RunWorkerCompleted += Export_RunWorkerCompleted;
                    exportWorker.RunWorkerAsync(new object[] { scenario, dialog.Base.FileName, includePrinters, includeDocuments });
                }
            }
        }


        private void Export_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                MessageBox.Show("Data successfully exported.", "STB Data Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportScenario_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] items = e.Argument as object[];

            EnterpriseScenario scenario = (EnterpriseScenario)items[0];
            string filePath = (string)items[1];
            bool includePrinters = (bool)items[2];
            bool includeDocuments = (bool)items[3];

            try
            {
                ContractFactory.OnStatusChanged += ContractFactory_OnStatusChanged;
                var contract = ContractFactory.Create(scenario, includePrinters, includeDocuments);
                ContractFactory.OnStatusChanged -= ContractFactory_OnStatusChanged;

                SendStatus(new StatusChangedEventArgs("Saving {0}".FormatWith(filePath)));
                contract.Save(filePath);

                SendStatus(new StatusChangedEventArgs(string.Empty));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                ShowExportMessageBox(ex.Message, "Export Scenario");
                SendStatus(new StatusChangedEventArgs("Export failed"));
                e.Cancel = true;
            }
        }

        private void ExportMetadata(Guid virtualResourceMetadataId)
        {
            VirtualResourceMetadata metadata = null;
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                metadata = VirtualResourceMetadata.SelectWithUsage(context, virtualResourceMetadataId);
            }

            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            using (var dialog = new ExportSaveFileDialog(directory, "Export Plugin Configuration Data", ImportExportType.Metadata))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    directory = Path.GetDirectoryName(dialog.Base.FileName);

                    BackgroundWorker exportWorker = new BackgroundWorker();
                    exportWorker.DoWork += ExportMetadata_DoWork;
                    exportWorker.RunWorkerCompleted += Export_RunWorkerCompleted;
                    exportWorker.RunWorkerAsync(new object[] { metadata, dialog.Base.FileName });
                }
            }
        }

        private void ExportMetadata_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] items = e.Argument as object[];

            VirtualResourceMetadata metadata = (VirtualResourceMetadata)items[0];
            string filePath = (string)items[1];

            try
            {
                PluginConfigurationData configurationData = metadata.BuildConfigurationData();
                PluginConfigurationData.WriteToFile(configurationData, filePath);

                SendStatus(new StatusChangedEventArgs(string.Empty));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                ShowExportMessageBox(ex.Message, "Export Metadata");
                SendStatus(new StatusChangedEventArgs("Export failed"));
                e.Cancel = true;
            }
        }

        private void ShowExportMessageBox(string message, string caption)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error)));
            }
            else
            {
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendStatus(StatusChangedEventArgs args)
        {
            OnStatusChanged?.Invoke(this, args);
        }

        private void ContractFactory_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            SendStatus(e);
        }

        /// <summary>
        /// Handles the 'Copy' Context menu click.  Used to be only copying scenarios - hence the name, but
        /// now it handles copies of folders, scenarios, resources and activities. Same day, if we ever
        /// clean this code, we could rename this appropriately.... Also, this handler and
        /// cutScenario_menuItem_Click are essentially the same except for the treatment of _useCut.  So - when
        /// we do cleanup, these two methods could be combined
        /// </summary>
        /// <param name="sender">Win param - not used</param>
        /// <param name="e">win param - not used.</param>
        private void copyScenario_MenuItem_Click(object sender, EventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            // ADD: Handling Copy Here. Traverse the entire list of "head nodes" that were "selected".
            // We rely on CanDrop and the context menu to prevent this method from being invoked when
            // the target is inappropriate. So - due to the complications around ResourceFolders and
            // MetadataFolders (namely the way they are linked and the way VR's and Metadata must be
            // added to the appropriate collections in the 'parent' scenario or VR) we will build
            // an organized 'list' of all the guids that represent the 'sub tree' for each selection.
            // That list drives the copy process.  We also build a 'mapping' from the old nodes to the new
            // nodes (so we can identify owners/parents) and make sure all the nodes are created and linked
            // properly.  One thing we may want to consider is whether or not the current design/fucntion
            // of ResourceFolders and MetadataFolders should be betetr integrated so they can be processed
            // much like the other nodes.
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            List<Guid?> TreeSource = new List<Guid?>();
            Dictionary<Guid?, Guid?> TargMappings = new Dictionary<Guid?, Guid?>();
            _inPaste = true;
            var selected = FindNodes(n => n.Selected).Select(n => n.Tag as Guid?).ToList();
            var expanded = FindNodes(n => n.Expanded).Select(n => n.Tag as Guid?).ToList();
            foreach (Guid id in _selectedIds)
            {
                // Treat each selection on its own - so clear the lists and mapps each time.
                TreeSource.Clear();
                TargMappings.Clear();
                RadTreeNode node = FindNode(id);
                if (node == null)  // This could happen if we select something and then delete it before we paste it.
                {
                    _inPaste = false;
                    return;
                }
                //----------------------------------------------------------------------------------------------
                //ADD: In building the TreeSource List, the current functional ORDER that the tree is traversed
                // is CRITICAL. We traverse through FirstNode to find children nodes and then NextNode to find
                // siblings. Folders always come first and then come scenarios/virtual resoruces/metadata.
                // This fact is used because as we traverse TreeSource for the copy - folders are created
                // before any node that would be placed in the folders. Each node we create is 'mapped' in
                // TargMappings (dictionary) with the original Guid as the key and the new Guid as the value.
                // So - it is easy to find the new 'parent' because it should alread be in TargMappings.
                TreeSource.Add((Guid?)node.Parent.Tag);
                TreeSource.Add(id);
                BuildSubTreeList(node.FirstNode, TreeSource);
                TargMappings.Add((Guid?)node.Parent.Tag, (Guid?)(_contextMenuNode != null ? _contextMenuNode.Tag : null));
                //----------------------------------------------------------------------------------------------
                //ADD: Here it is a little messy.  Resources and Metadata can either be linked directly to their
                // scenario / VirtualResource or they can also be in SubFolders (ReourceFolders and
                // MetadataFolders). It is the subfolders that cause issues - because the VR/metadata must be
                // added to the appropriate scenario/vr - which is not the nodes parent (from a UI perspective).
                // In order to handle this, we have to find the original 'owner' (scenario/vr) and map it to the
                // new 'owner.  Could simplify this code, but I want to be explicit as to what I am doing here
                // in case we find more cases that need special handling!
                if (_controller.IsResource(id))
                {
                    RadTreeNode srcScen = FindScenarioParent(node);
                    if (srcScen == null)
                    {
                        throw new Exception("Cannot copy VR: Scenario Parent not found for " + node.Name);
                    }
                    RadTreeNode tgtScen = FindScenarioParent(_contextMenuNode);
                    if (tgtScen == null)
                    {
                        throw new Exception("Cannot copy VR: Scenario Parent not found for " + _contextMenuNode.Name);
                    }
                    TargMappings[(Guid)srcScen.Tag] = (Guid)tgtScen.Tag;
                }
                if (_controller.IsResourceMetaData(id))
                {
                    RadTreeNode srcVR = FindResourceParent(node);
                    if (srcVR == null)
                    {
                        throw new Exception("Cannot copy MD: VR Parent not found for " + node.Name);
                    }
                    RadTreeNode tgtVR = FindResourceParent(_contextMenuNode);
                    if (tgtVR == null)
                    {
                        throw new Exception("Cannot copy MD: VR Parent not found for " + _contextMenuNode.Name);
                    }
                    TargMappings[(Guid)srcVR.Tag] = (Guid)tgtVR.Tag;
                }
                int ndx;
                for (ndx = 0; ndx < TreeSource.Count; ndx += 2)
                {
                    Guid tn = _controller.CopyNode(TreeSource, ndx, TargMappings, _controller.GetType(id));
                    TargMappings.Add(TreeSource[ndx + 1], tn);
                }
            }
            _inPaste = false;
            this.Cursor = saveCursor;
            RefreshTree();
            ExpandNodes(expanded);

            if (selected.Count > 0)
            {
                RadTreeNode[] reSelect = FindNodes(n => n.Tag as Guid? == (Guid?)selected[0]);
                if (reSelect.Length > 0)
                {
                    this.SelectedNode = reSelect[0];
                }
            }

            _contextMenuNode = null;
        }

        /// <summary>
        /// Handles the 'Cut' context menu click.  Used to be only handling scenarios, but now it handles
        /// Folders, Scenarios, Resources (VRs) and Activities.  Probably should be combined with
        /// CopyScenario_MenuItem_Click (only difference is in _useCut).
        /// </summary>
        /// <param name="sender">Win Param - Not Used</param>
        /// <param name="e">Win param - Not Used</param>
        private void cutScenario_MenuItem_Click(object sender, EventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            // ADD: Handling Cut.  Traverse the entire list and MOVE the selected objects to their 'new'
            // parents.  There is one "Kludge" or caveat with Cut.  If you are pasting on the root, then
            // we MUST create either the root folder or the scenario rather than just moving it because
            // currently, we cannot MOVE something to the root. I hope to fix that in the future.
            Cursor saveCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            _inPaste = true;
            var selected = FindNodes(n => n.Selected).Select(n => n.Tag as Guid?).ToList();
            var expanded = FindNodes(n => n.Expanded).Select(n => n.Tag as Guid?).ToList();
            foreach (Guid id in _selectedIds)
            {
                Guid nodeId = Guid.Empty;
                RadTreeNode node = FindNode(id);
                if (_contextMenuNode != null)
                {
                    //--------------------------------------------------------------------------------------
                    // ADD: If we are NOT moving to the root - we can just hand the source subtree off to
                    // the Move method -and it does everything for us - yay!
                    if (GetParentId(node) != (Guid?)_contextMenuNode.Tag)  // Only do the paste if the Paste Target is different than the source.
                    {
                        _controller.Move(id, (Guid?)_contextMenuNode.Tag);
                    }
                }
                else if (GetParentId(node) != null)  // Only do the Cut-paste if the source was also not the root because the target is the root
                {
                    //--------------------------------------------------------------------------------------
                    // ADD:  - - KLUDGE ALERT - -
                    //  At this time, We cannot cut/paste directly to the root.  This does not affect a copy
                    // command because we have to create new nodes.  But here, we are trying to MOVE the
                    // selected nodes -- and we cannot. The way I am working around this is to go ahead and
                    // CREATE the first node at the root and then move everything underneath.  This 'sort'
                    // of works, but has the problem that GUID of the main node has changed and so anything
                    // that was linked to that GUILD would fail.

                    if (_controller.IsFolder((Guid)node.Tag))
                    {
                        nodeId = _controller.CopyFolder(id, null);
                    }
                    else if (_controller.IsScenario((Guid)node.Tag))
                    {
                        nodeId = _controller.CopyScenario(id, null);
                    }
                    else
                    {
                        throw new Exception("Attempt to Cut/Paste a nonfolder/scenario object directly to the root?");
                    }
                    //--------------------------------------------------------------------------------------
                    // ADD: Since ONLY ScenarioFolders and EnterpriseScenarios are allowed at root - AND
                    // we are having to do a bit of jumping to create the 'head' node (since we can't move
                    // it to the root, yet) the following logic is the same for folders and scenarios: that
                    // is, that after the new node has been copied to root - we can move all of the subtrees
                    // from the old 'head' node to the new one -- then delete the old head.
                    if (node.FirstNode != null)
                    {
                        RadTreeNode walker = node.FirstNode.NextNode;
                        _controller.Move((Guid)node.FirstNode.Tag, nodeId);
                        while (walker != null)
                        {
                            RadTreeNode tempNode = walker.NextNode;
                            _controller.Move((Guid)walker.Tag, nodeId);
                            walker = tempNode;
                        }
                    }
                    _controller.Delete(id);
                }
            }

            SelectNode((Guid?)_selectedIds[0]);
            _selectedIds.Clear();
            _selectedSubNodeIds.Clear();
            _inPaste = false;
            this.Cursor = saveCursor;
            RefreshTree();
            ExpandNodes(expanded);

            if (selected.Count > 0)
            {
                RadTreeNode[] reSelect = FindNodes(n => n.Tag as Guid? == (Guid?)selected[0]);
                if (reSelect.Length > 0)
                {
                    this.SelectedNode = reSelect[0];
                }
            }

            _contextMenuNode = null;
        }

        #endregion

        #region Drag and Drop

        private class EnterpriseScenarioTreeDragDropService : TreeViewDragDropService
        {
            private RadTreeNode _draggedNode;

            public bool DoingDrag
            {
                get { return _draggedNode != null; }
            }

            public EnterpriseScenarioTreeDragDropService(RadTreeViewElement owner)
                : base(owner)
            {
            }

            // When we start dragging, save the dragged node
            protected override void PerformStart()
            {
                TraceFactory.Logger.Debug("Drag/drop started.");
                base.PerformStart();
                _draggedNode = (this.Context as TreeNodeElement).Data;
            }

            // When we stop dragging, clear the saved node
            protected override void PerformStop()
            {
                TraceFactory.Logger.Debug("Drag/drop ended.");
                base.PerformStop();
                _draggedNode = null;
            }

            // Define when drop is allowed
            protected override void OnPreviewDragOver(RadDragOverEventArgs e)
            {
                base.OnPreviewDragOver(e);
                if (e == null)
                {
                    throw new ArgumentNullException("e");
                }

                try
                {
                    RadTreeNode targetNode = (e.HitTarget as TreeNodeElement).Data;

                    if (targetNode != null)
                    {
                        // Can't drop an object onto its parent - strange things happen
                        if (targetNode.Nodes.Contains(_draggedNode))
                        {
                            e.CanDrop = false;
                        }
                        // Can't drop an object onto one of its children
                        else if (_draggedNode.Find(n => n == targetNode) != null)
                        {
                            e.CanDrop = false;
                        }
                        else
                        {
                            // Let the UI controller tell us if this is allowed
                            EnterpriseScenarioTreeView treeView = targetNode.TreeView as EnterpriseScenarioTreeView;
                            Guid sourceId = (Guid)_draggedNode.Tag;
                            Guid targetId = (Guid)targetNode.Tag;
                            e.CanDrop = treeView._controller.CanDrop(sourceId, targetId);
                        }
                    }
                    else
                    {
                        e.CanDrop = true;
                    }
                }
                //----------------------------------------------------------------------------------------------
                //ADD: This try/catch block has been added to stop the Drag/Drop process from throwing a null
                // reference whenever the drop is positioned over the root.  Instead of crashing - we will
                // just not allow the drop.
                catch (NullReferenceException)
                {
                    e.CanDrop = false;
                }
            }

            // When drop is complete, tell the controller to update the node associations
            protected override void OnPreviewDragDrop(RadDropEventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("Performing base drag/drop operation.");
                RadTreeNode targetNode = (e.HitTarget as TreeNodeElement).Data;
                EnterpriseScenarioTreeView treeView = targetNode.TreeView as EnterpriseScenarioTreeView;
                Guid targetId = (Guid)targetNode.Tag;
                base.OnPreviewDragDrop(e);

                if (e == null)
                {
                    throw new ArgumentNullException("e");
                }
                treeView._inPaste = true;
                foreach (Guid sourceId in treeView._selectedIds)
                {
                    System.Diagnostics.Debug.WriteLine("Moving {0} to {1}".FormatWith(sourceId, targetNode.Text));
                    treeView._controller.Move(sourceId, targetId);
                }
                treeView._inPaste = false;
                treeView._selectedIds.Clear();
                treeView._selectedSubNodeIds.Clear();
            }
        }

        #endregion

        #region Behavior Changes

        private void EnterpriseScenarioTreeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            // Prevent nodes from auto-expanding while performing drag/drop
            if (this.TreeViewElement.DragDropService.State == RadServiceState.Working)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Custom tree view element for the <see cref="EnterpriseScenarioTreeView"/>.
        /// </summary>
        private class EnterpriseScenarioTreeViewElement : RadTreeViewElement
        {
            // Enable theming for the element
            protected override Type ThemeEffectiveType
            {
                get { return typeof(RadTreeViewElement); }
            }

            // Replace the default drag drop service with the custom one
            protected override TreeViewDragDropService CreateDragDropService()
            {
                return new EnterpriseScenarioTreeDragDropService(this);
            }

            // Prevent multi-selection by dragging with mouse
            protected override bool ProcessMouseMove(MouseEventArgs e)
            {
                return false;
            }
        }

        /// <summary>
        /// Custom node sorter for the <see cref="EnterpriseScenarioTreeView"/>.
        /// </summary>
        private class EnterpriseScenarioTreeNodeComparer : TreeNodeComparer
        {
            public EnterpriseScenarioTreeNodeComparer(RadTreeViewElement treeView)
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

                bool xIsFolder = x.ImageKey.Contains("Folder", StringComparison.OrdinalIgnoreCase);
                bool yIsFolder = y.ImageKey.Contains("Folder", StringComparison.OrdinalIgnoreCase);

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
                    return string.Compare(x.Text, y.Text, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        /// <summary>
        /// Creates the tree view element.
        /// </summary>
        /// <returns></returns>
        protected override RadTreeViewElement CreateTreeViewElement()
        {
            return new EnterpriseScenarioTreeViewElement();
        }

        #endregion

        #region Node Operations

        private void RefreshTree()
        {
            // Disable this event while refreshing the node otherwise the main form will pick it
            // up and try to handle it as though the user clicked to a different location.
            // In this case, this is not what is happening, the node is just being refreshed
            // because something has been removed.  Once the node is refreshed the the event
            // can be enabled again.
            SelectedNodeChanging -= EnterpriseScenarioTreeView_SelectedNodeChanging;

            //Rebuild the nodes
            Clear();
            _controller.Refresh();
            SelectNode(_lastSelectedId);

            // Enable the event again so that processing will happen in the main form whenever
            // the user changes selection.
            SelectedNodeChanging += EnterpriseScenarioTreeView_SelectedNodeChanging;
        }

        private RadTreeNode FindNode(Guid? nodeId)
        {
            if (_nodeMap.ContainsKey((Guid)nodeId))
            {
                return _nodeMap[(Guid)nodeId];
            }

            return null;
            //return Find(n => n.Tag as Guid? == nodeId);
        }

        private static Guid? GetParentId(RadTreeNode node)
        {
            return node.Parent == null ? null : (Guid?)node.Parent.Tag;
        }

        private RadTreeNode GetSibling(RadTreeNode node, int siblingIndex)
        {
            RadTreeNode siblingNode = node.Parent?.Nodes[siblingIndex];
            if (siblingNode == null)
            {
                //No parent means we are at the root.
                siblingNode = Nodes[siblingIndex];
            }
            return siblingNode;
        }

        private void Attach(RadTreeNode node, Guid? parentId)
        {
            if (parentId != null)
            {
                try
                {
                    var parent = FindNode(parentId);
                    if (parent != null && parent.Nodes != null)
                    {
                        parent.Nodes.Add(node);
                        if (!_nodeMap.ContainsKey((Guid)node.Tag))
                        {
                            _nodeMap.Add((Guid)node.Tag, node);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Debug("Did not find node with ID=" + parentId);
                    TraceFactory.Logger.Error(ex);
                }
            }
            else
            {
                this.Nodes.Add(node);
                if (!_nodeMap.ContainsKey((Guid)node.Tag))
                {
                    _nodeMap.Add((Guid)node.Tag, node);
                }
            }
        }

        private void Detach(RadTreeNode node)
        {
            if (node.Parent != null)
            {
                _lastSelectedId = (Guid)node.Parent.Tag; //Preserve the Parent location since the node is going to be removed.
                node.Parent.Nodes.Remove(node);
            }

            // Make sure this node gets removed from the tree,
            // whether or not it is a root node
            if (this.Nodes.Contains(node))
            {
                this.Nodes.Remove(node);
            }

            if (_nodeMap.ContainsKey((Guid)node.Tag))
            {
                _nodeMap.Remove((Guid)node.Tag);
            }
        }

        //----------------------------------------------------------------------------------------------------
        // ADD: There may be a utility function somewhere to do this but I did not find it.  Anyhow, just grab
        // all the children nodes and place them in the _selectedSubNodeIds dictionary so we can make the UI
        // look nicer without taking a huge amount of time by searching a list or worse, checking parentage
        // when the UI is trying to display!  The nodes in this tree are hooked in the following manner:
        // RootNode.FirstNode -> This is the first direct child of this node (and continues until null)
        // RootNode.FirstNode.NextNode -> This is the chain of siblings to First node (until null).
        /// <summary>
        /// Recursive: Adds all the child nodes (and their children) to the _selectedSubNodeIds dictionary.  The dictionary
        /// is used for speed and efficiency because a dictionary is MUCH faster to search than a List. The nodes are linked
        /// such that the direct descendent follows 'FirstNode' and siblings follow NextNode.
        /// </summary>
        /// <param name="node">The node to add</param>
        private void GrabAllChildren(RadTreeNode node)
        {
            if (node != null)
            {
                if (_selectedIds.Contains((Guid)node.Tag))
                {
                    _selectedIds.Remove((Guid)node.Tag);
                }
                _selectedSubNodeIds[(Guid)node.Tag] = (Guid)node.Tag;
                GrabAllChildren(node.FirstNode);
                GrabAllChildren(node.NextNode);
            }
        }

        /// <summary>
        /// Returns a list of all nodes that are ancestors of startingNode
        /// First item in the list will be the starting node and last item will be topmost root node
        /// </summary>
        /// <param name="startingNode">The node.</param>
        /// <returns>List&lt;RadTreeNode&gt;.</returns>
        private List<RadTreeNode> GetNodeAncestry(RadTreeNode startingNode)
        {
            List<RadTreeNode> result = new List<RadTreeNode>();
            if (startingNode != null)
            {
                var tmpNode = startingNode;
                result.Add(tmpNode);
                while (tmpNode.Parent != null)
                {
                    tmpNode = tmpNode.Parent;
                    result.Add(tmpNode);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all items that are ancestors of startingNode
        /// First item in the list will be the tag of the starting node and last item will be the tag of the topmost root node
        /// </summary>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>List&lt;ConfigurationObjectTag&gt;.</returns>
        private List<ConfigurationObjectTag> GetConfigurationObjectTagAncestry(RadTreeNode startingNode)
        {
            List<ConfigurationObjectTag> result = new List<ConfigurationObjectTag>();
            var ancestors = GetNodeAncestry(startingNode);
            result.AddRange(ancestors.Select(x => _controller.GetObjectTag((Guid)x.Tag)));
            return result;
        }

        /// <summary>
        /// Removes all the guids of a 'subtree' during a multi-select when the user ctrl-clicks a second
        /// time on a tree.
        /// </summary>
        /// <param name="node">root od the subtree to remove</param>
        private void RemoveAllChildren(RadTreeNode node)
        {
            if (node != null)
            {
                if (_selectedSubNodeIds.ContainsValue((Guid)node.Tag))
                {
                    _selectedSubNodeIds.Remove((Guid)node.Tag);
                }
                RemoveAllChildren(node.FirstNode);
                RemoveAllChildren(node.NextNode);
            }
        }


        /// <summary>
        /// Traverses back up a sub tree to find the scenario that 'owns' a virtual resource.
        /// </summary>
        /// <param name="node">the Virtual resource node to start with</param>
        /// <returns>NScenario node that owns the parameter</returns>
        private RadTreeNode FindScenarioParent(RadTreeNode node)
        {
            RadTreeNode scenarioParent = node;
            while ((scenarioParent != null) && (!_controller.IsScenario((Guid)scenarioParent.Tag)))
            {
                scenarioParent = scenarioParent.Parent;
            }
            return scenarioParent;
        }


        /// <summary>
        /// Traverses back up a sub tree to find the virtual resource that owns a metadata node
        /// </summary>
        /// <param name="node">the metadata node to start with</param>
        /// <returns>the Virtual Resource that owns the parameter</returns>
        private RadTreeNode FindResourceParent(RadTreeNode node)
        {
            RadTreeNode vrParent = node;
            while ((vrParent != null) && (!_controller.IsResource((Guid)vrParent.Tag)))
            {
                vrParent = vrParent.Parent;
            }
            return vrParent;
        }

        #endregion


        /// <summary>
        /// Recursive method that builds a List representation of a tree based on 'couplets'.
        /// A Couplet is defined as [0] - Parent Node Id, [1] Node id.  Using a list of
        /// couplets (by use, not definition) rather than a dictionary because any node can
        /// be the parent node for multiple other nodes.  Always traverse FirstNode (child)
        /// then NextNode (sibling).
        /// NOTE: ORder the list is 'sorted' in is CRITICAL: Always expect Folders first, then
        /// Scenarios/VirtualResources/Metadata.
        /// </summary>
        /// <param name="Node">Current node in the tree</param>
        /// <param name="TreeList">The List being built.</param>
        private void BuildSubTreeList(RadTreeNode Node, List<Guid?> TreeList)
        {
            if (Node == null)
            {
                return;
            }
            TreeList.Add((Guid?)Node.Parent.Tag);
            TreeList.Add((Guid?)Node.Tag);
            BuildSubTreeList(Node.FirstNode, TreeList);
            BuildSubTreeList(Node.NextNode, TreeList);
        }

        private void OnImportCompleted()
        {
            ImportCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets focus on the node with the specified text searching down the hierarchy from the specified current node.
        /// </summary>
        /// <param name="currentNode">The Current Node.</param>
        /// <param name="searchText">The node display text to search for.</param>
        /// <returns>True if the specified node was found, false otherwise.</returns>
        public bool SearchForNextNode(RadTreeNode currentNode, string searchText)
        {
            bool stopSearch = false;

            List<Tuple<Guid, string>> nodeList = FlattenTreeNodes();

            // Find the index of the currently selected node.
            int currentNodeIndex = nodeList.FindIndex(n => n.Item1 == (Guid)currentNode.Tag);

            // Find the index of the node that contains the search text.
            int requestedNodeIndex = nodeList.FindIndex(currentNodeIndex + 1, n => n.Item2.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            // Goto the requested node.
            if (requestedNodeIndex != -1)
            {
                this.SelectNode(nodeList[requestedNodeIndex].Item1);
                this.FindNode(nodeList[requestedNodeIndex].Item1).EnsureVisible();
                stopSearch = true;
            }

            return stopSearch;
        }

        private List<Tuple<Guid, string>> FlattenTreeNodes()
        {
            // Get the XML representation of the RadTreeView data and convert it to a flat list of tuples.
            XDocument treeXml = XDocument.Parse(this.TreeViewXml);
            List<Tuple<Guid, string>> nodeList = new List<Tuple<Guid, string>>();

            var nodeQuery = from n in treeXml.Descendants("Nodes")
                            select new
                            {
                                guidValue = n.Element("Tag").Value,
                                textValue = n.Attribute("Text").Value
                            };

            foreach (var node in nodeQuery)
            {
                Guid nodeGuid = Guid.Parse(node.guidValue);
                nodeList.Add(new Tuple<Guid, string>(nodeGuid, node.textValue));
            }

            return nodeList;
        }

        /// <summary>
        /// Sets focus on the node with the specified text searching up the hierarchy from the specified current node.
        /// </summary>
        /// <param name="currentNode">The Current Node.</param>
        /// <param name="searchText">The node display text to search for.</param>
        /// <returns>True if the specified node was found, false otherwise.</returns>
        public bool SearchForPrevNode(RadTreeNode currentNode, string searchText)
        {
            bool stopSearch = false;

            List<Tuple<Guid, string>> nodeList = FlattenTreeNodes();

            // Find the index of the currently selected node.
            int currentNodeIndex = nodeList.FindIndex(n => n.Item1 == (Guid)currentNode.Tag);

            // Find the index of the node that contains the search text.
            int requestedNodeIndex = nodeList.FindLastIndex(currentNodeIndex - 1, n => n.Item2.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            // Goto the requested node.
            if (requestedNodeIndex != -1)
            {
                this.SelectNode(nodeList[requestedNodeIndex].Item1);
                this.FindNode(nodeList[requestedNodeIndex].Item1).EnsureVisible();
                stopSearch = true;
            }

            return stopSearch;
        }
    }
}
