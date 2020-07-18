namespace HP.ScalableTest.LabConsole.ResourceConfiguration
{
	partial class SimulatorManagementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulatorManagementForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TsSimMgt = new System.Windows.Forms.ToolStrip();
            this.reservedFor_ToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.reservedFor_ToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.powerOn_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.powerOff_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.revert_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.launch_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deselectAll_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.selectAll_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.close_Button = new System.Windows.Forms.Button();
            this.info_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.event_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.simMgt_Grid = new System.Windows.Forms.DataGridView();
            this.status_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimulatorContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selected_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.assetId_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualMachine_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionId_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reservedFor_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostAddress_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.simReservedListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TsSimMgt.SuspendLayout();
            this.info_StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simMgt_Grid)).BeginInit();
            this.SimulatorContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simReservedListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TsSimMgt
            // 
            this.TsSimMgt.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.TsSimMgt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reservedFor_ToolStripLabel,
            this.reservedFor_ToolStripComboBox,
            this.toolStripSeparator1,
            this.powerOn_ToolStripButton,
            this.powerOff_ToolStripButton,
            this.revert_ToolStripButton,
            this.launch_ToolStripButton,
            this.toolStripSeparator2,
            this.deselectAll_ToolStripButton,
            this.selectAll_ToolStripButton});
            this.TsSimMgt.Location = new System.Drawing.Point(0, 0);
            this.TsSimMgt.Name = "TsSimMgt";
            this.TsSimMgt.Size = new System.Drawing.Size(863, 28);
            this.TsSimMgt.TabIndex = 1;
            this.TsSimMgt.Text = "toolStrip1";
            // 
            // reservedFor_ToolStripLabel
            // 
            this.reservedFor_ToolStripLabel.Name = "reservedFor_ToolStripLabel";
            this.reservedFor_ToolStripLabel.Size = new System.Drawing.Size(97, 25);
            this.reservedFor_ToolStripLabel.Text = "Reserved For:";
            // 
            // reservedFor_ToolStripComboBox
            // 
            this.reservedFor_ToolStripComboBox.Name = "reservedFor_ToolStripComboBox";
            this.reservedFor_ToolStripComboBox.Size = new System.Drawing.Size(160, 28);
            this.reservedFor_ToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.reservedFor_ToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // powerOn_ToolStripButton
            // 
            this.powerOn_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("powerOn_ToolStripButton.Image")));
            this.powerOn_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.powerOn_ToolStripButton.Name = "powerOn_ToolStripButton";
            this.powerOn_ToolStripButton.Size = new System.Drawing.Size(97, 25);
            this.powerOn_ToolStripButton.Text = "Power On";
            this.powerOn_ToolStripButton.Click += new System.EventHandler(this.powerOn_ToolStripButton_Click);
            // 
            // powerOff_ToolStripButton
            // 
            this.powerOff_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("powerOff_ToolStripButton.Image")));
            this.powerOff_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.powerOff_ToolStripButton.Name = "powerOff_ToolStripButton";
            this.powerOff_ToolStripButton.Size = new System.Drawing.Size(99, 25);
            this.powerOff_ToolStripButton.Text = "Power Off";
            this.powerOff_ToolStripButton.Click += new System.EventHandler(this.powerOff_ToolStripButton_Click);
            // 
            // revert_ToolStripButton
            // 
            this.revert_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("revert_ToolStripButton.Image")));
            this.revert_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.revert_ToolStripButton.Name = "revert_ToolStripButton";
            this.revert_ToolStripButton.Size = new System.Drawing.Size(75, 25);
            this.revert_ToolStripButton.Text = "Revert";
            this.revert_ToolStripButton.Click += new System.EventHandler(this.revert_ToolStripButton_Click);
            // 
            // launch_ToolStripButton
            // 
            this.launch_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("launch_ToolStripButton.Image")));
            this.launch_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.launch_ToolStripButton.Name = "launch_ToolStripButton";
            this.launch_ToolStripButton.Size = new System.Drawing.Size(79, 25);
            this.launch_ToolStripButton.Text = "Launch";
            this.launch_ToolStripButton.Click += new System.EventHandler(this.launch_ToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // deselectAll_ToolStripButton
            // 
            this.deselectAll_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.deselectAll_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deselectAll_ToolStripButton.Image")));
            this.deselectAll_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deselectAll_ToolStripButton.Name = "deselectAll_ToolStripButton";
            this.deselectAll_ToolStripButton.Size = new System.Drawing.Size(112, 25);
            this.deselectAll_ToolStripButton.Text = "Deselect All";
            this.deselectAll_ToolStripButton.Click += new System.EventHandler(this.deselectAll_ToolStripButton_Click);
            // 
            // selectAll_ToolStripButton
            // 
            this.selectAll_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.selectAll_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("selectAll_ToolStripButton.Image")));
            this.selectAll_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectAll_ToolStripButton.Name = "selectAll_ToolStripButton";
            this.selectAll_ToolStripButton.Size = new System.Drawing.Size(95, 25);
            this.selectAll_ToolStripButton.Text = "Select All";
            this.selectAll_ToolStripButton.Click += new System.EventHandler(this.selectAll_ToolStripButton_Click);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.Location = new System.Drawing.Point(749, 522);
            this.close_Button.Margin = new System.Windows.Forms.Padding(4);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(107, 34);
            this.close_Button.TabIndex = 2;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // info_StatusStrip
            // 
            this.info_StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.info_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.event_StatusLabel});
            this.info_StatusStrip.Location = new System.Drawing.Point(0, 562);
            this.info_StatusStrip.Name = "info_StatusStrip";
            this.info_StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.info_StatusStrip.Size = new System.Drawing.Size(863, 25);
            this.info_StatusStrip.TabIndex = 3;
            this.info_StatusStrip.Text = "statusStrip1";
            // 
            // event_StatusLabel
            // 
            this.event_StatusLabel.Name = "event_StatusLabel";
            this.event_StatusLabel.Size = new System.Drawing.Size(165, 20);
            this.event_StatusLabel.Text = "Simulator Management";
            // 
            // simMgt_Grid
            // 
            this.simMgt_Grid.AllowUserToAddRows = false;
            this.simMgt_Grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.simMgt_Grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.simMgt_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simMgt_Grid.AutoGenerateColumns = false;
            this.simMgt_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.simMgt_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selected_Column,
            this.assetId_Column,
            this.product_Column,
            this.virtualMachine_Column,
            this.sessionId_Column,
            this.reservedFor_Column,
            this.status_Column,
            this.hostAddress_Column});
            this.simMgt_Grid.ContextMenuStrip = this.SimulatorContextMenuStrip;
            this.simMgt_Grid.DataSource = this.simReservedListBindingSource;
            this.simMgt_Grid.Location = new System.Drawing.Point(0, 32);
            this.simMgt_Grid.Margin = new System.Windows.Forms.Padding(4);
            this.simMgt_Grid.MultiSelect = false;
            this.simMgt_Grid.Name = "simMgt_Grid";
            this.simMgt_Grid.RowHeadersVisible = false;
            this.simMgt_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.simMgt_Grid.Size = new System.Drawing.Size(863, 482);
            this.simMgt_Grid.TabIndex = 4;
            this.simMgt_Grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.simMgt_Grid_CellClick);
            // 
            // status_Column
            // 
            this.status_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.status_Column.DataPropertyName = "Status";
            this.status_Column.HeaderText = "Status";
            this.status_Column.Name = "status_Column";
            // 
            // SimulatorContextMenuStrip
            // 
            this.SimulatorContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SimulatorContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.SimulatorContextMenuStrip.Name = "contextMenuStrip1";
            this.SimulatorContextMenuStrip.Size = new System.Drawing.Size(160, 56);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("checkAllToolStripMenuItem.Image")));
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.checkAllToolStripMenuItem.Text = "Check All";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uncheckAllToolStripMenuItem.Image")));
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(159, 26);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck All";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
            // 
            // selected_Column
            // 
            this.selected_Column.DataPropertyName = "IsSelected";
            this.selected_Column.HeaderText = "Selected";
            this.selected_Column.Name = "selected_Column";
            this.selected_Column.Width = 56;
            // 
            // assetId_Column
            // 
            this.assetId_Column.DataPropertyName = "AssetId";
            this.assetId_Column.HeaderText = "Asset ID";
            this.assetId_Column.Name = "assetId_Column";
            this.assetId_Column.ReadOnly = true;
            // 
            // product_Column
            // 
            this.product_Column.DataPropertyName = "Product";
            this.product_Column.HeaderText = "Product";
            this.product_Column.Name = "product_Column";
            this.product_Column.ReadOnly = true;
            // 
            // virtualMachine_Column
            // 
            this.virtualMachine_Column.DataPropertyName = "VirtualMachine";
            this.virtualMachine_Column.HeaderText = "Virtual Machine";
            this.virtualMachine_Column.Name = "virtualMachine_Column";
            this.virtualMachine_Column.ReadOnly = true;
            this.virtualMachine_Column.Width = 125;
            // 
            // sessionId_Column
            // 
            this.sessionId_Column.DataPropertyName = "SessionId";
            this.sessionId_Column.HeaderText = "Session ID";
            this.sessionId_Column.Name = "sessionId_Column";
            this.sessionId_Column.ReadOnly = true;
            this.sessionId_Column.Width = 125;
            // 
            // reservedFor_Column
            // 
            this.reservedFor_Column.DataPropertyName = "ReservedFor";
            this.reservedFor_Column.HeaderText = "Reserved For";
            this.reservedFor_Column.Name = "reservedFor_Column";
            this.reservedFor_Column.ReadOnly = true;
            this.reservedFor_Column.Width = 116;
            // 
            // hostAddress_Column
            // 
            this.hostAddress_Column.DataPropertyName = "HostAddress";
            this.hostAddress_Column.HeaderText = "HostAddress";
            this.hostAddress_Column.Name = "hostAddress_Column";
            this.hostAddress_Column.ReadOnly = true;
            this.hostAddress_Column.Visible = false;
            // 
            // simReservedListBindingSource
            // 
            this.simReservedListBindingSource.DataSource = typeof(HP.ScalableTest.LabConsole.ResourceConfiguration.SimReservedList);
            // 
            // SimulatorManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 587);
            this.Controls.Add(this.simMgt_Grid);
            this.Controls.Add(this.info_StatusStrip);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.TsSimMgt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SimulatorManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simulator Management";
            this.TsSimMgt.ResumeLayout(false);
            this.TsSimMgt.PerformLayout();
            this.info_StatusStrip.ResumeLayout(false);
            this.info_StatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.simMgt_Grid)).EndInit();
            this.SimulatorContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.simReservedListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.ToolStrip TsSimMgt;
		private System.Windows.Forms.ToolStripComboBox reservedFor_ToolStripComboBox;
		private System.Windows.Forms.ToolStripLabel reservedFor_ToolStripLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton powerOff_ToolStripButton;
		private System.Windows.Forms.BindingSource simReservedListBindingSource;
		private System.Windows.Forms.ToolStripButton powerOn_ToolStripButton;
		private System.Windows.Forms.ToolStripButton revert_ToolStripButton;
		private System.Windows.Forms.ToolStripButton launch_ToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton deselectAll_ToolStripButton;
		private System.Windows.Forms.ToolStripButton selectAll_ToolStripButton;
		private System.Windows.Forms.Button close_Button;
		private System.Windows.Forms.StatusStrip info_StatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel event_StatusLabel;
        private System.Windows.Forms.DataGridView simMgt_Grid;
        private System.Windows.Forms.ContextMenuStrip SimulatorContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selected_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetId_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualMachine_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionId_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn reservedFor_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostAddress_Column;
	}
}