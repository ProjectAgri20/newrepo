namespace HP.ScalableTest.UI
{
    partial class SimulatorListForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulatorListForm));
            this.simulator_DataGridView = new System.Windows.Forms.DataGridView();
            this.toolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_New = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Edit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Remove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Reservation = new System.Windows.Forms.ToolStripButton();
            this.button_Ok = new System.Windows.Forms.Button();
            this.panel_Main = new System.Windows.Forms.Panel();
            this.column_AssetId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Vm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Firmware = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.simulator_DataGridView)).BeginInit();
            this.toolStrip_Main.SuspendLayout();
            this.panel_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // simulator_DataGridView
            // 
            this.simulator_DataGridView.AllowUserToAddRows = false;
            this.simulator_DataGridView.AllowUserToDeleteRows = false;
            this.simulator_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.simulator_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.simulator_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.simulator_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.simulator_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_AssetId,
            this.column_Product,
            this.column_Address,
            this.column_Vm,
            this.column_Firmware,
            this.column_Type});
            this.simulator_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simulator_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.simulator_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.simulator_DataGridView.MultiSelect = false;
            this.simulator_DataGridView.Name = "simulator_DataGridView";
            this.simulator_DataGridView.ReadOnly = true;
            this.simulator_DataGridView.RowHeadersVisible = false;
            this.simulator_DataGridView.RowTemplate.Height = 25;
            this.simulator_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.simulator_DataGridView.Size = new System.Drawing.Size(864, 399);
            this.simulator_DataGridView.TabIndex = 11;
            this.simulator_DataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.simulator_DataGridView_CellDoubleClick);
            // 
            // toolStrip_Main
            // 
            this.toolStrip_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_New,
            this.toolStripButton_Edit,
            this.toolStripButton_Remove,
            this.toolStripButton_Reservation});
            this.toolStrip_Main.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_Main.Name = "toolStrip_Main";
            this.toolStrip_Main.Size = new System.Drawing.Size(865, 27);
            this.toolStrip_Main.TabIndex = 12;
            this.toolStrip_Main.Text = "toolStrip1";
            // 
            // toolStripButton_New
            // 
            this.toolStripButton_New.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_New.Image")));
            this.toolStripButton_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_New.Name = "toolStripButton_New";
            this.toolStripButton_New.Size = new System.Drawing.Size(55, 24);
            this.toolStripButton_New.Text = "New";
            this.toolStripButton_New.ToolTipText = "Add a new print device to your inventory";
            this.toolStripButton_New.Click += new System.EventHandler(this.toolStripButton_New_Click);
            // 
            // toolStripButton_Edit
            // 
            this.toolStripButton_Edit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Edit.Image")));
            this.toolStripButton_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Edit.Name = "toolStripButton_Edit";
            this.toolStripButton_Edit.Size = new System.Drawing.Size(51, 24);
            this.toolStripButton_Edit.Text = "Edit";
            this.toolStripButton_Edit.ToolTipText = "Edit the selected print device";
            this.toolStripButton_Edit.Click += new System.EventHandler(this.toolStripButton_Edit_Click);
            // 
            // toolStripButton_Remove
            // 
            this.toolStripButton_Remove.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Remove.Image")));
            this.toolStripButton_Remove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Remove.Name = "toolStripButton_Remove";
            this.toolStripButton_Remove.Size = new System.Drawing.Size(74, 24);
            this.toolStripButton_Remove.Text = "Remove";
            this.toolStripButton_Remove.ToolTipText = "Delete the selected print device from your inventory";
            this.toolStripButton_Remove.Click += new System.EventHandler(this.toolStripButton_Remove_Click);
            // 
            // toolStripButton_Reservation
            // 
            this.toolStripButton_Reservation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Reservation.Image")));
            this.toolStripButton_Reservation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Reservation.Name = "toolStripButton_Reservation";
            this.toolStripButton_Reservation.Size = new System.Drawing.Size(97, 24);
            this.toolStripButton_Reservation.Text = "Reservations";
            this.toolStripButton_Reservation.ToolTipText = "Manage any reservations for the selected print device";
            this.toolStripButton_Reservation.Click += new System.EventHandler(this.toolStripButton_Reservation_Click);
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(748, 436);
            this.button_Ok.Margin = new System.Windows.Forms.Padding(4);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(112, 32);
            this.button_Ok.TabIndex = 13;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // panel_Main
            // 
            this.panel_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Main.Controls.Add(this.simulator_DataGridView);
            this.panel_Main.Location = new System.Drawing.Point(0, 30);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(864, 399);
            this.panel_Main.TabIndex = 16;
            // 
            // column_AssetId
            // 
            this.column_AssetId.DataPropertyName = "AssetId";
            this.column_AssetId.HeaderText = "AssetId";
            this.column_AssetId.Name = "column_AssetId";
            this.column_AssetId.ReadOnly = true;
            // 
            // column_Product
            // 
            this.column_Product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_Product.DataPropertyName = "Product";
            this.column_Product.HeaderText = "Product";
            this.column_Product.Name = "column_Product";
            this.column_Product.ReadOnly = true;
            // 
            // column_Address
            // 
            this.column_Address.DataPropertyName = "Address";
            this.column_Address.HeaderText = "Address";
            this.column_Address.Name = "column_Address";
            this.column_Address.ReadOnly = true;
            // 
            // column_Vm
            // 
            this.column_Vm.DataPropertyName = "VirtualMachine";
            this.column_Vm.HeaderText = "Vm or Host";
            this.column_Vm.Name = "column_Vm";
            this.column_Vm.ReadOnly = true;
            // 
            // column_Firmware
            // 
            this.column_Firmware.DataPropertyName = "FirmwareVersion";
            this.column_Firmware.HeaderText = "Firmware Version";
            this.column_Firmware.Name = "column_Firmware";
            this.column_Firmware.ReadOnly = true;
            this.column_Firmware.Width = 120;
            // 
            // column_Type
            // 
            this.column_Type.DataPropertyName = "SimulatorType";
            this.column_Type.HeaderText = "Type";
            this.column_Type.Name = "column_Type";
            this.column_Type.ReadOnly = true;
            // 
            // SimulatorListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 473);
            this.Controls.Add(this.panel_Main);
            this.Controls.Add(this.toolStrip_Main);
            this.Controls.Add(this.button_Ok);
            this.Name = "SimulatorListForm";
            this.Text = "Simulator Inventory";
            this.Load += new System.EventHandler(this.SimulatorListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.simulator_DataGridView)).EndInit();
            this.toolStrip_Main.ResumeLayout(false);
            this.toolStrip_Main.PerformLayout();
            this.panel_Main.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView simulator_DataGridView;
        private System.Windows.Forms.ToolStrip toolStrip_Main;
        private System.Windows.Forms.ToolStripButton toolStripButton_New;
        private System.Windows.Forms.ToolStripButton toolStripButton_Edit;
        private System.Windows.Forms.ToolStripButton toolStripButton_Remove;
        private System.Windows.Forms.ToolStripButton toolStripButton_Reservation;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_AssetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Product;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Vm;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Firmware;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Type;
    }
}