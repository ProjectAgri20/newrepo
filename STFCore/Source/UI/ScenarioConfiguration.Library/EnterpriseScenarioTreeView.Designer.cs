using System;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class EnterpriseScenarioTreeView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _disabledFont != null)
            {
                _disabledFont.Dispose();
                _disabledFont = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterpriseScenarioTreeView));
            this.contextMenu = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.new_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.newFolder_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.newMenu_SeparatorItem = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.newScenario_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.rename_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.delete_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.exportScenario_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.importScenario_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.copyPaste_SeparatorItem = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.cutScenario_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.copyscenario_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.expandCollapse_SeparatorItem = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.startBatch_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.expand_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.collapse_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.enableDisable_SeparatorItem = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.enableDisable_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.new_MenuItem,
            this.rename_MenuItem,
            this.delete_MenuItem,
            this.exportScenario_MenuItem,
            this.importScenario_MenuItem,
            this.copyPaste_SeparatorItem,
            this.cutScenario_MenuItem,
            this.copyscenario_MenuItem,
            this.expandCollapse_SeparatorItem,
            this.startBatch_MenuItem,
            this.expand_MenuItem,
            this.collapse_MenuItem,
            this.enableDisable_SeparatorItem,
            this.enableDisable_MenuItem});
            // 
            // new_MenuItem
            // 
            this.new_MenuItem.AccessibleDescription = "Add";
            this.new_MenuItem.AccessibleName = "Add";
            this.new_MenuItem.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.newFolder_MenuItem,
            this.newMenu_SeparatorItem,
            this.newScenario_MenuItem});
            this.new_MenuItem.Name = "new_MenuItem";
            this.new_MenuItem.Text = "New";
            // 
            // newFolder_MenuItem
            // 
            this.newFolder_MenuItem.Name = "newFolder_MenuItem";
            this.newFolder_MenuItem.Text = "Folder";
            this.newFolder_MenuItem.Click += new System.EventHandler(this.newFolder_MenuItem_Click);
            // 
            // newMenu_SeparatorItem
            // 
            this.newMenu_SeparatorItem.Name = "newMenu_SeparatorItem";
            this.newMenu_SeparatorItem.Text = "New Menu Separator Item";
            this.newMenu_SeparatorItem.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newScenario_MenuItem
            // 
            this.newScenario_MenuItem.Name = "newScenario_MenuItem";
            this.newScenario_MenuItem.Text = "Test Scenario";
            this.newScenario_MenuItem.Click += new System.EventHandler(this.newScenario_MenuItem_Click);
            // 
            // rename_MenuItem
            // 
            this.rename_MenuItem.Name = "rename_MenuItem";
            this.rename_MenuItem.Text = "Rename";
            this.rename_MenuItem.Click += new System.EventHandler(this.rename_MenuItem_Click);
            // 
            // delete_MenuItem
            // 
            this.delete_MenuItem.Name = "delete_MenuItem";
            this.delete_MenuItem.Text = "Delete";
            this.delete_MenuItem.Click += new System.EventHandler(this.delete_MenuItem_Click);
            // 
            // exportScenario_MenuItem
            // 
            this.exportScenario_MenuItem.Name = "exportScenario_MenuItem";
            this.exportScenario_MenuItem.Text = "Export";
            this.exportScenario_MenuItem.Click += new System.EventHandler(this.exportScenario_MenuItem_Click);
            // 
            // importScenario_MenuItem
            // 
            this.importScenario_MenuItem.Name = "importScenario_MenuItem";
            this.importScenario_MenuItem.Text = "Import";
            this.importScenario_MenuItem.Click += new System.EventHandler(this.importScenario_MenuItem_Click);
            // 
            // copyPaste_SeparatorItem
            // 
            this.copyPaste_SeparatorItem.AccessibleDescription = " ";
            this.copyPaste_SeparatorItem.AccessibleName = " ";
            this.copyPaste_SeparatorItem.Name = "copyPaste_SeparatorItem";
            this.copyPaste_SeparatorItem.Text = "";
            this.copyPaste_SeparatorItem.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cutScenario_MenuItem
            // 
            this.cutScenario_MenuItem.AccessibleDescription = "Cut Scenario";
            this.cutScenario_MenuItem.AccessibleName = "Cut Scenario";
            this.cutScenario_MenuItem.Name = "cutScenario_MenuItem";
            this.cutScenario_MenuItem.Text = "Move Here";
            this.cutScenario_MenuItem.Click += new System.EventHandler(this.cutScenario_MenuItem_Click);
            // 
            // copyscenario_MenuItem
            // 
            this.copyscenario_MenuItem.AccessibleDescription = "Copy";
            this.copyscenario_MenuItem.AccessibleName = "Copy";
            this.copyscenario_MenuItem.Name = "copyscenario_MenuItem";
            this.copyscenario_MenuItem.Text = "Copy Here";
            this.copyscenario_MenuItem.Click += new System.EventHandler(this.copyScenario_MenuItem_Click);
            // 
            // expandCollapse_SeparatorItem
            // 
            this.expandCollapse_SeparatorItem.Name = "expandCollapse_SeparatorItem";
            this.expandCollapse_SeparatorItem.Text = "";
            this.expandCollapse_SeparatorItem.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startBatch_MenuItem
            // 
            this.startBatch_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("startBatch_MenuItem.Image")));
            this.startBatch_MenuItem.Name = "startBatch_MenuItem";
            this.startBatch_MenuItem.Text = "Start All";
            this.startBatch_MenuItem.Click += new System.EventHandler(this.startBatch_MenuItem_Click);
            // 
            // expand_MenuItem
            // 
            this.expand_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("expand_MenuItem.Image")));
            this.expand_MenuItem.Name = "expand_MenuItem";
            this.expand_MenuItem.Text = "Expand All";
            this.expand_MenuItem.Click += new System.EventHandler(this.expand_MenuItem_Click);
            // 
            // collapse_MenuItem
            // 
            this.collapse_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("collapse_MenuItem.Image")));
            this.collapse_MenuItem.Name = "collapse_MenuItem";
            this.collapse_MenuItem.Text = "Collapse All";
            this.collapse_MenuItem.Click += new System.EventHandler(this.collapse_MenuItem_Click);
            // 
            // enableDisable_SeparatorItem
            // 
            this.enableDisable_SeparatorItem.Name = "enableDisable_SeparatorItem";
            this.enableDisable_SeparatorItem.Text = "";
            this.enableDisable_SeparatorItem.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // enableDisable_MenuItem
            // 
            this.enableDisable_MenuItem.Name = "enableDisable_MenuItem";
            this.enableDisable_MenuItem.Text = "Enable";
            this.enableDisable_MenuItem.Click += new System.EventHandler(this.enableDisable_MenuItem_Click);
            // 
            // EnterpriseScenarioTreeView
            // 
            this.AllowDragDrop = true;
            this.AllowDrop = true;
            this.RadContextMenu = this.contextMenu;
            this.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.SpacingBetweenNodes = -1;
            this.SelectedNodeChanging += new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.EnterpriseScenarioTreeView_SelectedNodeChanging);
            this.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.EnterpriseScenarioTreeView_SelectedNodeChanged);
            this.NodeExpandedChanging += new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.EnterpriseScenarioTreeView_NodeExpandedChanging);
            this.NodeFormatting += new Telerik.WinControls.UI.TreeNodeFormattingEventHandler(this.EnterpriseScenarioTreeView_NodeFormatting);
            this.ContextMenuOpening += new Telerik.WinControls.UI.TreeViewContextMenuOpeningEventHandler(this.EnterpriseScenarioTreeView_ContextMenuOpening);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadContextMenu contextMenu;
        private Telerik.WinControls.UI.RadMenuItem rename_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem delete_MenuItem;
        private Telerik.WinControls.UI.RadMenuSeparatorItem expandCollapse_SeparatorItem;
        private Telerik.WinControls.UI.RadMenuItem expand_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem collapse_MenuItem;
        private Telerik.WinControls.UI.RadMenuSeparatorItem enableDisable_SeparatorItem;
        private Telerik.WinControls.UI.RadMenuItem enableDisable_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem new_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem newFolder_MenuItem;
        private Telerik.WinControls.UI.RadMenuSeparatorItem newMenu_SeparatorItem;
        private Telerik.WinControls.UI.RadMenuItem newScenario_MenuItem;
		private Telerik.WinControls.UI.RadMenuSeparatorItem copyPaste_SeparatorItem;
        private Telerik.WinControls.UI.RadMenuItem copyscenario_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem exportScenario_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem cutScenario_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem importScenario_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem startBatch_MenuItem;
    }
}
