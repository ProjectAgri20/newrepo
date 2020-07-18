using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.LabConsole
{
    partial class VMInventoryForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMInventoryForm));
            this.close_Button = new System.Windows.Forms.Button();
            this.vm_GridView = new Telerik.WinControls.UI.RadGridView();
            this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setMachineStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.availableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reservedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doNotScheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addHoldIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHoldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.releaseSession_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.synchronizeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.holdToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.addHoldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHoldToolStripMenuItemTop = new System.Windows.Forms.ToolStripMenuItem();
            this.usageToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.availableToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reservedToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.inUseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.doNotScheduleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseSession_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.vm_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vm_GridView.MasterTemplate)).BeginInit();
            this.mainContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(942, 542);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(100, 28);
            this.close_Button.TabIndex = 15;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // vm_GridView
            // 
            this.vm_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vm_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.vm_GridView.ContextMenuStrip = this.mainContextMenuStrip;
            this.vm_GridView.Location = new System.Drawing.Point(1, 28);
            // 
            // vm_GridView
            // 
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.FieldName = "Name";
            gridViewTextBoxColumn1.HeaderText = "Host Name";
            gridViewTextBoxColumn1.Name = "name_GridViewColumn";
            gridViewTextBoxColumn1.Width = 120;
            gridViewTextBoxColumn2.FieldName = "Environment";
            gridViewTextBoxColumn2.HeaderText = "Environment";
            gridViewTextBoxColumn2.Name = "environment_GridViewColumn";
            gridViewTextBoxColumn2.Width = 110;
            gridViewTextBoxColumn3.FieldName = "PowerState";
            gridViewTextBoxColumn3.HeaderText = "Power State";
            gridViewTextBoxColumn3.Name = "powerState_GridViewColumn";
            gridViewTextBoxColumn3.Width = 100;
            gridViewTextBoxColumn4.FieldName = "UsageState";
            gridViewTextBoxColumn4.HeaderText = "Usage State";
            gridViewTextBoxColumn4.Name = "usageState_GridViewColumn";
            gridViewTextBoxColumn4.Width = 100;
            gridViewTextBoxColumn5.FieldName = "HoldId";
            gridViewTextBoxColumn5.HeaderText = "Hold ID";
            gridViewTextBoxColumn5.Name = "holdId_GridViewColumn";
            gridViewTextBoxColumn5.Width = 120;
            gridViewTextBoxColumn6.FieldName = "SessionId";
            gridViewTextBoxColumn6.HeaderText = "Session ID";
            gridViewTextBoxColumn6.Name = "sessionId_GridViewColumn";
            gridViewTextBoxColumn6.Width = 100;
            gridViewTextBoxColumn7.FieldName = "Owner";
            gridViewTextBoxColumn7.HeaderText = "Owner";
            gridViewTextBoxColumn7.Name = "owner_GridViewColumn";
            gridViewTextBoxColumn7.Width = 100;
            gridViewDateTimeColumn1.AllowGroup = false;
            gridViewDateTimeColumn1.FieldName = "StartDate";
            gridViewDateTimeColumn1.HeaderText = "Start Date";
            gridViewDateTimeColumn1.Name = "startDate_GridViewColumn";
            gridViewDateTimeColumn1.Width = 175;
            gridViewTextBoxColumn8.FieldName = "Status";
            gridViewTextBoxColumn8.HeaderText = "Session Status";
            gridViewTextBoxColumn8.Name = "sessionStatus_GridViewColumn";
            gridViewTextBoxColumn8.Width = 100;
            gridViewTextBoxColumn9.AllowGroup = false;
            gridViewTextBoxColumn9.FieldName = "LastUpdated";
            gridViewTextBoxColumn9.HeaderText = "Last Updated";
            gridViewTextBoxColumn9.IsVisible = false;
            gridViewTextBoxColumn9.Name = "lastUpdated_GridViewColumn";
            gridViewTextBoxColumn10.AllowGroup = false;
            gridViewTextBoxColumn10.FieldName = "SortOrder";
            gridViewTextBoxColumn10.HeaderText = "Sort Order";
            gridViewTextBoxColumn10.IsVisible = false;
            gridViewTextBoxColumn10.Name = "sortOrder_GridViewColumn";
            this.vm_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewDateTimeColumn1,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10});
            this.vm_GridView.MasterTemplate.EnableFiltering = true;
            this.vm_GridView.MasterTemplate.MultiSelect = true;
            this.vm_GridView.Name = "vm_GridView";
            // 
            // 
            // 
            this.vm_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(1, 28, 240, 150);
            this.vm_GridView.Size = new System.Drawing.Size(1052, 497);
            this.vm_GridView.TabIndex = 16;
            this.vm_GridView.Text = "radGridView1";
            this.vm_GridView.SelectionChanged += new System.EventHandler(this.vm_GridView_SelectionChanged);
            // 
            // mainContextMenuStrip
            // 
            this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setMachineStateToolStripMenuItem,
            this.toolStripSeparator2,
            this.addHoldIdToolStripMenuItem,
            this.removeHoldToolStripMenuItem,
            this.toolStripSeparator1,
            this.releaseSession_ToolStripMenuItem});
            this.mainContextMenuStrip.Name = "mainContextMenuStrip";
            this.mainContextMenuStrip.Size = new System.Drawing.Size(230, 112);
            this.mainContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.mainContextMenuStrip_Opening);
            // 
            // setMachineStateToolStripMenuItem
            // 
            this.setMachineStateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.availableToolStripMenuItem,
            this.reservedToolStripMenuItem,
            this.inUseToolStripMenuItem,
            this.doNotScheduleToolStripMenuItem});
            this.setMachineStateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("setMachineStateToolStripMenuItem.Image")));
            this.setMachineStateToolStripMenuItem.Name = "setMachineStateToolStripMenuItem";
            this.setMachineStateToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            this.setMachineStateToolStripMenuItem.Text = "Set Machine State";
            // 
            // availableToolStripMenuItem
            // 
            this.availableToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("availableToolStripMenuItem.Image")));
            this.availableToolStripMenuItem.Name = "availableToolStripMenuItem";
            this.availableToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.availableToolStripMenuItem.Text = "Available";
            this.availableToolStripMenuItem.ToolTipText = "Set machine state to \"Available\"";
            this.availableToolStripMenuItem.Click += new System.EventHandler(this.availableToolStripMenuItem_Click);
            // 
            // reservedToolStripMenuItem
            // 
            this.reservedToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reservedToolStripMenuItem.Image")));
            this.reservedToolStripMenuItem.Name = "reservedToolStripMenuItem";
            this.reservedToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.reservedToolStripMenuItem.Text = "Reserved";
            this.reservedToolStripMenuItem.ToolTipText = "Set machine state to \"Reserved\"";
            this.reservedToolStripMenuItem.Click += new System.EventHandler(this.reservedToolStripMenuItem_Click);
            // 
            // inUseToolStripMenuItem
            // 
            this.inUseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("inUseToolStripMenuItem.Image")));
            this.inUseToolStripMenuItem.Name = "inUseToolStripMenuItem";
            this.inUseToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.inUseToolStripMenuItem.Text = "In Use";
            this.inUseToolStripMenuItem.ToolTipText = "Set machine state to \"In Use\"";
            this.inUseToolStripMenuItem.Click += new System.EventHandler(this.inUseToolStripMenuItem_Click);
            // 
            // doNotScheduleToolStripMenuItem
            // 
            this.doNotScheduleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("doNotScheduleToolStripMenuItem.Image")));
            this.doNotScheduleToolStripMenuItem.Name = "doNotScheduleToolStripMenuItem";
            this.doNotScheduleToolStripMenuItem.Size = new System.Drawing.Size(191, 24);
            this.doNotScheduleToolStripMenuItem.Text = "Do Not Schedule";
            this.doNotScheduleToolStripMenuItem.ToolTipText = "Set machine state to \"Do Not Schedule\"";
            this.doNotScheduleToolStripMenuItem.Click += new System.EventHandler(this.doNotScheduleToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(226, 6);
            // 
            // addHoldIdToolStripMenuItem
            // 
            this.addHoldIdToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addHoldIdToolStripMenuItem.Image")));
            this.addHoldIdToolStripMenuItem.Name = "addHoldIdToolStripMenuItem";
            this.addHoldIdToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            this.addHoldIdToolStripMenuItem.Text = "Add Machine Hold";
            this.addHoldIdToolStripMenuItem.Click += new System.EventHandler(this.addHoldIdToolStripMenuItem_Click);
            // 
            // removeHoldToolStripMenuItem
            // 
            this.removeHoldToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeHoldToolStripMenuItem.Image")));
            this.removeHoldToolStripMenuItem.Name = "removeHoldToolStripMenuItem";
            this.removeHoldToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            this.removeHoldToolStripMenuItem.Text = "Remove Machine Hold";
            this.removeHoldToolStripMenuItem.Click += new System.EventHandler(this.removeHoldToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(226, 6);
            // 
            // releaseSession_ToolStripMenuItem
            // 
            this.releaseSession_ToolStripMenuItem.Enabled = false;
            this.releaseSession_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("releaseSession_ToolStripMenuItem.Image")));
            this.releaseSession_ToolStripMenuItem.Name = "releaseSession_ToolStripMenuItem";
            this.releaseSession_ToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            this.releaseSession_ToolStripMenuItem.Text = "Release Session";
            this.releaseSession_ToolStripMenuItem.Click += new System.EventHandler(this.releaseSession_ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.synchronizeToolStripButton,
            this.holdToolStripDropDownButton,
            this.usageToolStripDropDownButton,
            this.releaseSession_ToolStripButton,
            this.toolStripSeparator3,
            this.refresh_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1054, 27);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // synchronizeToolStripButton
            // 
            this.synchronizeToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.synchronizeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("synchronizeToolStripButton.Image")));
            this.synchronizeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.synchronizeToolStripButton.Name = "synchronizeToolStripButton";
            this.synchronizeToolStripButton.Size = new System.Drawing.Size(108, 24);
            this.synchronizeToolStripButton.Text = "Synchronize";
            this.synchronizeToolStripButton.ToolTipText = "Synchronize VM Inventory with VM Management System";
            this.synchronizeToolStripButton.Click += new System.EventHandler(this.synchronizeToolStripButton_Click);
            // 
            // holdToolStripDropDownButton
            // 
            this.holdToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHoldToolStripMenuItem,
            this.removeHoldToolStripMenuItemTop});
            this.holdToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("holdToolStripDropDownButton.Image")));
            this.holdToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.holdToolStripDropDownButton.Name = "holdToolStripDropDownButton";
            this.holdToolStripDropDownButton.Size = new System.Drawing.Size(131, 24);
            this.holdToolStripDropDownButton.Text = "Machine Hold";
            this.holdToolStripDropDownButton.ToolTipText = "Add or remove Hold on machine";
            this.holdToolStripDropDownButton.DropDownOpening += new System.EventHandler(this.holdToolStripDropDownButton_DropDownOpening);
            // 
            // addHoldToolStripMenuItem
            // 
            this.addHoldToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addHoldToolStripMenuItem.Image")));
            this.addHoldToolStripMenuItem.Name = "addHoldToolStripMenuItem";
            this.addHoldToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
            this.addHoldToolStripMenuItem.Text = "Add Hold";
            this.addHoldToolStripMenuItem.Click += new System.EventHandler(this.addHoldIdToolStripMenuItem_Click);
            // 
            // removeHoldToolStripMenuItemTop
            // 
            this.removeHoldToolStripMenuItemTop.Image = ((System.Drawing.Image)(resources.GetObject("removeHoldToolStripMenuItemTop.Image")));
            this.removeHoldToolStripMenuItemTop.Name = "removeHoldToolStripMenuItemTop";
            this.removeHoldToolStripMenuItemTop.Size = new System.Drawing.Size(169, 24);
            this.removeHoldToolStripMenuItemTop.Text = "Remove Hold";
            this.removeHoldToolStripMenuItemTop.Click += new System.EventHandler(this.removeHoldToolStripMenuItem_Click);
            // 
            // usageToolStripDropDownButton
            // 
            this.usageToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.availableToolStripMenuItem1,
            this.reservedToolStripMenuItem1,
            this.inUseToolStripMenuItem1,
            this.doNotScheduleToolStripMenuItem1});
            this.usageToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("usageToolStripDropDownButton.Image")));
            this.usageToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.usageToolStripDropDownButton.Name = "usageToolStripDropDownButton";
            this.usageToolStripDropDownButton.Size = new System.Drawing.Size(132, 24);
            this.usageToolStripDropDownButton.Text = "Machine State";
            this.usageToolStripDropDownButton.ToolTipText = "Set machine usage state value";
            // 
            // availableToolStripMenuItem1
            // 
            this.availableToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("availableToolStripMenuItem1.Image")));
            this.availableToolStripMenuItem1.Name = "availableToolStripMenuItem1";
            this.availableToolStripMenuItem1.Size = new System.Drawing.Size(191, 24);
            this.availableToolStripMenuItem1.Text = "Available";
            this.availableToolStripMenuItem1.ToolTipText = "Set machine state to \"Available\"";
            this.availableToolStripMenuItem1.Click += new System.EventHandler(this.availableToolStripMenuItem_Click);
            // 
            // reservedToolStripMenuItem1
            // 
            this.reservedToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("reservedToolStripMenuItem1.Image")));
            this.reservedToolStripMenuItem1.Name = "reservedToolStripMenuItem1";
            this.reservedToolStripMenuItem1.Size = new System.Drawing.Size(191, 24);
            this.reservedToolStripMenuItem1.Text = "Reserved";
            this.reservedToolStripMenuItem1.ToolTipText = "Set machine state to \"Reserved\"";
            this.reservedToolStripMenuItem1.Click += new System.EventHandler(this.reservedToolStripMenuItem_Click);
            // 
            // inUseToolStripMenuItem1
            // 
            this.inUseToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("inUseToolStripMenuItem1.Image")));
            this.inUseToolStripMenuItem1.Name = "inUseToolStripMenuItem1";
            this.inUseToolStripMenuItem1.Size = new System.Drawing.Size(191, 24);
            this.inUseToolStripMenuItem1.Text = "In Use";
            this.inUseToolStripMenuItem1.ToolTipText = "Set machine state to \"In Use\"";
            this.inUseToolStripMenuItem1.Click += new System.EventHandler(this.inUseToolStripMenuItem_Click);
            // 
            // doNotScheduleToolStripMenuItem1
            // 
            this.doNotScheduleToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("doNotScheduleToolStripMenuItem1.Image")));
            this.doNotScheduleToolStripMenuItem1.Name = "doNotScheduleToolStripMenuItem1";
            this.doNotScheduleToolStripMenuItem1.Size = new System.Drawing.Size(191, 24);
            this.doNotScheduleToolStripMenuItem1.Text = "Do Not Schedule";
            this.doNotScheduleToolStripMenuItem1.ToolTipText = "Set machine state to \"Do Not Schedule\"";
            this.doNotScheduleToolStripMenuItem1.Click += new System.EventHandler(this.doNotScheduleToolStripMenuItem_Click);
            // 
            // releaseSession_ToolStripButton
            // 
            this.releaseSession_ToolStripButton.Enabled = false;
            this.releaseSession_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("releaseSession_ToolStripButton.Image")));
            this.releaseSession_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.releaseSession_ToolStripButton.Name = "releaseSession_ToolStripButton";
            this.releaseSession_ToolStripButton.Size = new System.Drawing.Size(133, 24);
            this.releaseSession_ToolStripButton.Text = "Release Session";
            this.releaseSession_ToolStripButton.ToolTipText = "Release the Session and all associated resources";
            this.releaseSession_ToolStripButton.Click += new System.EventHandler(this.releaseSession_ToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // refresh_ToolStripButton
            // 
            this.refresh_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refresh_ToolStripButton.Image")));
            this.refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_ToolStripButton.Name = "refresh_ToolStripButton";
            this.refresh_ToolStripButton.Size = new System.Drawing.Size(78, 24);
            this.refresh_ToolStripButton.Text = "Refresh";
            this.refresh_ToolStripButton.ToolTipText = "Refresh the grid display";
            this.refresh_ToolStripButton.Click += new System.EventHandler(this.refresh_ToolStripButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(12, 558);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 20);
            this.statusLabel.TabIndex = 18;
            // 
            // VMInventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(1054, 582);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.vm_GridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VMInventoryForm";
            this.Text = "Virtual Machine Inventory Management";
            this.Load += new System.EventHandler(this.VMInventoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vm_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vm_GridView)).EndInit();
            this.mainContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private Telerik.WinControls.UI.RadGridView vm_GridView;
        private System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setMachineStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem availableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reservedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inUseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doNotScheduleToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton synchronizeToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem addHoldIdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeHoldToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton holdToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem addHoldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeHoldToolStripMenuItemTop;
        private System.Windows.Forms.ToolStripDropDownButton usageToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem availableToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem reservedToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem inUseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem doNotScheduleToolStripMenuItem1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem releaseSession_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton releaseSession_ToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton refresh_ToolStripButton;
    }
}