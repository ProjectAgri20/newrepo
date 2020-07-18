namespace HP.ScalableTest.UI.SessionExecution
{
    partial class SessionExecutionTreeView
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
            this.contextMenu = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.restartVM_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.pauseWorker_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.resumeWorker_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.haltWorker_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.asset_ContextMenu = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.suspendAsset_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.resumeAsset_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.enableCRC_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.disableCRC_MenuItem = new Telerik.WinControls.UI.RadMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.restartVM_MenuItem,
            this.pauseWorker_MenuItem,
            this.resumeWorker_MenuItem,
            this.haltWorker_MenuItem});
            // 
            // restartVM_MenuItem
            // 
            this.restartVM_MenuItem.AccessibleDescription = "Restart Machine";
            this.restartVM_MenuItem.AccessibleName = "Restart Machine";
            this.restartVM_MenuItem.Image = null;
            this.restartVM_MenuItem.Name = "restartVM_MenuItem";
            this.restartVM_MenuItem.Text = "Restart Machine";
            this.restartVM_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.restartVM_MenuItem.Click += new System.EventHandler(this.restartVM_MenuItem_Click);
            // 
            // pauseWorker_MenuItem
            // 
            this.pauseWorker_MenuItem.AccessibleDescription = "Pause Worker";
            this.pauseWorker_MenuItem.AccessibleName = "Pause Worker";
            this.pauseWorker_MenuItem.Image = null;
            this.pauseWorker_MenuItem.Name = "pauseWorker_MenuItem";
            this.pauseWorker_MenuItem.Text = "Pause Worker";
            this.pauseWorker_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.pauseWorker_MenuItem.Click += new System.EventHandler(this.pauseWorker_MenuItem_Click);
            // 
            // resumeWorker_MenuItem
            // 
            this.resumeWorker_MenuItem.AccessibleDescription = "Resume Worker";
            this.resumeWorker_MenuItem.AccessibleName = "Resume Worker";
            this.resumeWorker_MenuItem.Image = null;
            this.resumeWorker_MenuItem.Name = "resumeWorker_MenuItem";
            this.resumeWorker_MenuItem.Text = "Resume Worker";
            this.resumeWorker_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.resumeWorker_MenuItem.Click += new System.EventHandler(this.resumeWorker_MenuItem_Click);
            // 
            // haltWorker_MenuItem
            // 
            this.haltWorker_MenuItem.AccessibleDescription = "Halt Worker";
            this.haltWorker_MenuItem.AccessibleName = "Halt Worker";
            this.haltWorker_MenuItem.Image = null;
            this.haltWorker_MenuItem.Name = "haltWorker_MenuItem";
            this.haltWorker_MenuItem.Text = "Halt Worker";
            this.haltWorker_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.haltWorker_MenuItem.Click += new System.EventHandler(this.haltWorker_MenuItem_Click);
            // 
            // asset_ContextMenu
            // 
            this.asset_ContextMenu.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.suspendAsset_MenuItem,
            this.resumeAsset_MenuItem,
            this.enableCRC_MenuItem,
            this.disableCRC_MenuItem});
            // 
            // suspendAsset_MenuItem
            // 
            this.suspendAsset_MenuItem.AccessibleDescription = "Suspend";
            this.suspendAsset_MenuItem.AccessibleName = "Suspend";
            this.suspendAsset_MenuItem.Name = "suspendAsset_MenuItem";
            this.suspendAsset_MenuItem.Text = "Suspend";
            this.suspendAsset_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.suspendAsset_MenuItem.Click += new System.EventHandler(this.suspendAsset_MenuItem_Click);
            // 
            // resumeAsset_MenuItem
            // 
            this.resumeAsset_MenuItem.AccessibleDescription = "Resume";
            this.resumeAsset_MenuItem.AccessibleName = "Resume";
            this.resumeAsset_MenuItem.Name = "resumeAsset_MenuItem";
            this.resumeAsset_MenuItem.Text = "Resume";
            this.resumeAsset_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.resumeAsset_MenuItem.Click += new System.EventHandler(this.resumeAsset_MenuItem_Click);
            // 
            // enableCRC_MenuItem
            // 
            this.enableCRC_MenuItem.AccessibleDescription = "Enable Paperless";
            this.enableCRC_MenuItem.AccessibleName = "CRC Enabled";
            this.enableCRC_MenuItem.Name = "enableCRC_MenuItem";
            this.enableCRC_MenuItem.Text = "Enable Paperless";
            this.enableCRC_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.enableCRC_MenuItem.Click += new System.EventHandler(this.enablePaperless_MenuItem_Click);
            // 
            // disableCRC_MenuItem
            // 
            this.disableCRC_MenuItem.AccessibleDescription = "Disable Paperless";
            this.disableCRC_MenuItem.AccessibleName = "Disable Paperless";
            this.disableCRC_MenuItem.Name = "disableCRC_MenuItem";
            this.disableCRC_MenuItem.Text = "Disable Paperless";
            this.disableCRC_MenuItem.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.disableCRC_MenuItem.Click += new System.EventHandler(this.disablePaperless_MenuItem_Click);
            // 
            // SessionExecutionTreeView
            // 
            this.RadContextMenu = this.contextMenu;
            this.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.SpacingBetweenNodes = -1;
            this.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.SessionExecutionTreeView_SelectedNodeChanged);
            this.ContextMenuOpening += new Telerik.WinControls.UI.TreeViewContextMenuOpeningEventHandler(this.SessionExecutionTreeView_ContextMenuOpening);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadContextMenu contextMenu;
        private Telerik.WinControls.UI.RadMenuItem restartVM_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem pauseWorker_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem resumeWorker_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem haltWorker_MenuItem;
        private Telerik.WinControls.UI.RadContextMenu asset_ContextMenu;
        private Telerik.WinControls.UI.RadMenuItem suspendAsset_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem resumeAsset_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem enableCRC_MenuItem;
        private Telerik.WinControls.UI.RadMenuItem disableCRC_MenuItem;
    }
}
