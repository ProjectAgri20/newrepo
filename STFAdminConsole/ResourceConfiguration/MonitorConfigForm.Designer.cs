namespace HP.ScalableTest.LabConsole
{
    partial class MonitorConfigForm
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
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorConfigForm));
            this.ok_Button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.monitor_DataGridView = new System.Windows.Forms.DataGridView();
            this.id_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.server_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.config_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menu_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.new_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.monitor_DataGridView)).BeginInit();
            this.menu_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(1279, 596);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 7;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.monitor_DataGridView);
            this.panel1.Controls.Add(this.menu_ToolStrip);
            this.panel1.Location = new System.Drawing.Point(4, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1375, 586);
            this.panel1.TabIndex = 12;
            // 
            // monitor_DataGridView
            // 
            this.monitor_DataGridView.AllowUserToAddRows = false;
            this.monitor_DataGridView.AllowUserToDeleteRows = false;
            this.monitor_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.monitor_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.monitor_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.monitor_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_Column,
            this.server_Column,
            this.type_Column,
            this.config_Column});
            this.monitor_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitor_DataGridView.Location = new System.Drawing.Point(0, 27);
            this.monitor_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.monitor_DataGridView.MultiSelect = false;
            this.monitor_DataGridView.Name = "monitor_DataGridView";
            this.monitor_DataGridView.ReadOnly = true;
            this.monitor_DataGridView.RowHeadersVisible = false;
            this.monitor_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.monitor_DataGridView.Size = new System.Drawing.Size(1375, 559);
            this.monitor_DataGridView.TabIndex = 13;
            this.monitor_DataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Monitor_DataGridView_CellDoubleClick);
            // 
            // id_Column
            // 
            this.id_Column.DataPropertyName = "DigitalSendOutputLocationId";
            this.id_Column.HeaderText = "ID";
            this.id_Column.Name = "id_Column";
            this.id_Column.ReadOnly = true;
            this.id_Column.Visible = false;
            // 
            // server_Column
            // 
            this.server_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.server_Column.DataPropertyName = "ServerHostName";
            this.server_Column.HeaderText = "Server Name";
            this.server_Column.MinimumWidth = 130;
            this.server_Column.Name = "server_Column";
            this.server_Column.ReadOnly = true;
            this.server_Column.Width = 130;
            // 
            // type_Column
            // 
            this.type_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.type_Column.DataPropertyName = "MonitorType";
            this.type_Column.HeaderText = "Monitor Type";
            this.type_Column.MinimumWidth = 110;
            this.type_Column.Name = "type_Column";
            this.type_Column.ReadOnly = true;
            this.type_Column.Width = 110;
            // 
            // config_Column
            // 
            this.config_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.config_Column.DataPropertyName = "Configuration";
            this.config_Column.HeaderText = "Configuration";
            this.config_Column.Name = "config_Column";
            this.config_Column.ReadOnly = true;
            // 
            // menu_ToolStrip
            // 
            this.menu_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.menu_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_ToolStripButton,
            this.edit_ToolStripButton,
            this.remove_ToolStripButton});
            this.menu_ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menu_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.menu_ToolStrip.Name = "menu_ToolStrip";
            this.menu_ToolStrip.Size = new System.Drawing.Size(1375, 27);
            this.menu_ToolStrip.TabIndex = 12;
            this.menu_ToolStrip.Text = "toolStrip1";
            // 
            // new_ToolStripButton
            // 
            this.new_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("new_ToolStripButton.Image")));
            this.new_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.new_ToolStripButton.Name = "new_ToolStripButton";
            this.new_ToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.new_ToolStripButton.Text = "Add";
            this.new_ToolStripButton.ToolTipText = "Add a new server ";
            this.new_ToolStripButton.Click += new System.EventHandler(this.new_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(51, 24);
            this.edit_ToolStripButton.Text = "Edit";
            this.edit_ToolStripButton.ToolTipText = "Edit the selected server";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.edit_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.remove_ToolStripButton.Text = "Remove";
            this.remove_ToolStripButton.ToolTipText = "Delete the selected server";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.remove_ToolStripButton_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Location";
            this.dataGridViewTextBoxColumn1.HeaderText = "Location";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ValidationOptions";
            this.dataGridViewTextBoxColumn2.HeaderText = "Validation Options";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // MonitorConfigForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ok_Button;
            this.ClientSize = new System.Drawing.Size(1383, 632);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ok_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MonitorConfigForm";
            this.Text = "Monitor Configuration Settings";
            this.Load += new System.EventHandler(this.MonitorConfigForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.monitor_DataGridView)).EndInit();
            this.menu_ToolStrip.ResumeLayout(false);
            this.menu_ToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView monitor_DataGridView;
        private System.Windows.Forms.ToolStrip menu_ToolStrip;
        private System.Windows.Forms.ToolStripButton new_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn server_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn type_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn config_Column;
    }
}