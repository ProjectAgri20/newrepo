namespace HP.ScalableTest
{
    partial class AssetReservationListForm<T>
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
            Telerik.WinControls.UI.GridViewTextBoxColumn startColumn = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn endColumn = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn3 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DummyForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.server_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDown_View = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem_Current = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_History = new System.Windows.Forms.ToolStripMenuItem();
            this.assetReservationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ok_Button = new System.Windows.Forms.Button();
            this.apply_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.reservation_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelButton = new System.Windows.Forms.Button();
            this.server_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetReservationBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reservation_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reservation_RadGridView.MasterTemplate)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // server_ContextMenuStrip
            // 
            this.server_ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.server_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.server_ContextMenuStrip.Name = "server_ContextMenuStrip";
            this.server_ContextMenuStrip.Size = new System.Drawing.Size(139, 56);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.edit_Button_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // assetReservationBindingSource
            // 
            this.assetReservationBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.AssetReservation);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(509, 291);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(723, 292);
            this.apply_Button.Margin = new System.Windows.Forms.Padding(4);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(100, 32);
            this.apply_Button.TabIndex = 6;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.reservation_RadGridView);
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(836, 252);
            this.panel1.TabIndex = 9;
            // 
            // reservation_RadGridView
            // 
            this.reservation_RadGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reservation_RadGridView.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            gridViewTextBoxColumn1.DataType = typeof(System.Guid);
            gridViewTextBoxColumn1.FieldName = "ReservationId";
            gridViewTextBoxColumn1.HeaderText = "ReservationId";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "ReservationId";
            gridViewTextBoxColumn2.FieldName = "AssetId";
            gridViewTextBoxColumn2.HeaderText = "AssetId";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.IsVisible = false;
            gridViewTextBoxColumn2.Name = "AssetId";
            startColumn.FieldName = "Start";
            startColumn.HeaderText = "Start";
            startColumn.IsAutoGenerated = true;
            startColumn.Name = "Start";
            endColumn.FieldName = "End";
            endColumn.HeaderText = "End";
            endColumn.IsAutoGenerated = true;
            endColumn.Name = "End";
            gridViewTextBoxColumn3.FieldName = "ReservedFor";
            gridViewTextBoxColumn3.HeaderText = "ReservedFor";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.Name = "ReservedFor";
            gridViewTextBoxColumn4.FieldName = "ReservedBy";
            gridViewTextBoxColumn4.HeaderText = "ReservedBy";
            gridViewTextBoxColumn4.IsAutoGenerated = true;
            gridViewTextBoxColumn4.Name = "ReservedBy";
            gridViewTextBoxColumn5.FieldName = "SessionId";
            gridViewTextBoxColumn5.HeaderText = "SessionId";
            gridViewTextBoxColumn5.IsAutoGenerated = true;
            gridViewTextBoxColumn5.Name = "SessionId";
            gridViewDateTimeColumn3.FieldName = "Received";
            gridViewDateTimeColumn3.HeaderText = "Submitted";
            gridViewDateTimeColumn3.IsAutoGenerated = true;
            gridViewDateTimeColumn3.Name = "Received";
            gridViewDateTimeColumn3.IsVisible = false;
            gridViewTextBoxColumn6.FieldName = "Notes";
            gridViewTextBoxColumn6.HeaderText = "Notes";
            gridViewTextBoxColumn6.IsAutoGenerated = true;
            gridViewTextBoxColumn6.IsVisible = false;
            gridViewTextBoxColumn6.Name = "Notes";
            gridViewDecimalColumn1.DataType = typeof(int);
            gridViewDecimalColumn1.FieldName = "Notify";
            gridViewDecimalColumn1.HeaderText = "Notify";
            gridViewDecimalColumn1.IsAutoGenerated = true;
            gridViewDecimalColumn1.IsVisible = false;
            gridViewDecimalColumn1.Name = "Notify";
            gridViewTextBoxColumn7.FieldName = "CreatedBy";
            gridViewTextBoxColumn7.HeaderText = "CreatedBy";
            gridViewTextBoxColumn7.IsAutoGenerated = true;
            gridViewTextBoxColumn7.IsVisible = false;
            gridViewTextBoxColumn7.Name = "CreatedBy";
            gridViewTextBoxColumn8.DataType = typeof(HP.ScalableTest.Core.AssetInventory.Asset);
            gridViewTextBoxColumn8.FieldName = "Asset";
            gridViewTextBoxColumn8.HeaderText = "Asset";
            gridViewTextBoxColumn8.IsAutoGenerated = true;
            gridViewTextBoxColumn8.IsVisible = false;
            gridViewTextBoxColumn8.Name = "Asset";
            gridViewTextBoxColumn9.FieldName = "NotificationRecipient";
            gridViewTextBoxColumn9.HeaderText = "NotificationRecipient";
            gridViewTextBoxColumn9.IsAutoGenerated = true;
            gridViewTextBoxColumn9.IsVisible = false;
            gridViewTextBoxColumn9.Name = "NotificationRecipient";
            gridViewTextBoxColumn9.ReadOnly = true;
            this.reservation_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            startColumn,
            endColumn,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewDateTimeColumn3,
            gridViewTextBoxColumn6,
            gridViewDecimalColumn1,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9});
            startColumn.MinWidth = 130;
            endColumn.MinWidth = 130;
            this.reservation_RadGridView.MasterTemplate.DataSource = this.assetReservationBindingSource;
            this.reservation_RadGridView.Name = "reservation_RadGridView";
            this.reservation_RadGridView.Size = new System.Drawing.Size(836, 252);
            this.reservation_RadGridView.TabIndex = 2;
            this.reservation_RadGridView.Text = "radGridView1";
            this.reservation_RadGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.asset_DataGridView_CellDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripButton,
            this.editToolStripButton,
            this.removeToolStripButton,
            this.toolStripSeparator1,
            this.toolStripDropDown_View});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(836, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.addToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.addToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addToolStripButton.Name = "addToolStripButton";
            this.addToolStripButton.Size = new System.Drawing.Size(63, 24);
            this.addToolStripButton.Text = "Add";
            this.addToolStripButton.ToolTipText = "Add a new reservation";
            this.addToolStripButton.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // editToolStripButton
            // 
            this.editToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripButton.Image")));
            this.editToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editToolStripButton.Name = "editToolStripButton";
            this.editToolStripButton.Size = new System.Drawing.Size(59, 24);
            this.editToolStripButton.Text = "Edit";
            this.editToolStripButton.ToolTipText = "Edit the selected reservation";
            this.editToolStripButton.Click += new System.EventHandler(this.edit_Button_Click);
            // 
            // removeToolStripButton
            // 
            this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
            this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeToolStripButton.Name = "removeToolStripButton";
            this.removeToolStripButton.Size = new System.Drawing.Size(87, 24);
            this.removeToolStripButton.Text = "Remove";
            this.removeToolStripButton.ToolTipText = "Delete the selected reservation";
            this.removeToolStripButton.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // toolStripDropDown_View
            // 
            this.toolStripDropDown_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Current,
            this.toolStripMenuItem_History});
            this.toolStripDropDown_View.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDown_View.Image")));
            this.toolStripDropDown_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDown_View.Name = "toolStripDropDown_View";
            this.toolStripDropDown_View.Size = new System.Drawing.Size(65, 24);
            this.toolStripDropDown_View.Text = "View";
            // 
            // toolStripMenuItem_Current
            // 
            this.toolStripMenuItem_Current.Checked = true;
            this.toolStripMenuItem_Current.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem_Current.Name = "toolStripMenuItem_Current";
            this.toolStripMenuItem_Current.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_Current.Text = "Current";
            this.toolStripMenuItem_Current.Click += new System.EventHandler(this.toolStripMenuItem_Current_Click);
            // 
            // toolStripMenuItem_History
            // 
            this.toolStripMenuItem_History.Name = "toolStripMenuItem_History";
            this.toolStripMenuItem_History.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_History.Text = "History";
            this.toolStripMenuItem_History.Click += new System.EventHandler(this.toolStripMenuItem_History_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(616, 292);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // AssetReservationListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(836, 336);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.ok_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AssetReservationListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Asset Reservations";
            this.Load += new System.EventHandler(this.AssetListForm_Load);
            this.server_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.assetReservationBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reservation_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reservation_RadGridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.ContextMenuStrip server_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addToolStripButton;
        private System.Windows.Forms.ToolStripButton removeToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDown_View;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Current;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_History;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.BindingSource assetReservationBindingSource;
        private Telerik.WinControls.UI.RadGridView reservation_RadGridView;
    }
}