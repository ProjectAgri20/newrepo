namespace HP.ScalableTest.UI.Framework
{
    partial class PrintDriverCfmManagementForm
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

                if (_context != null)
                {
                    _context.Dispose();
                }
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintDriverCfmManagementForm));
            this.version_CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.refreshVersion_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.versionUnselectAll_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.versionSelectAll_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.refreshProduct_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.productUnselectAll_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.productSelectAll_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.product_CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.upload_Button = new System.Windows.Forms.Button();
            this.configFiles_DataGridView = new System.Windows.Forms.DataGridView();
            this.driverConfigIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.configFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printDriverProductsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printDriverVersionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printDriverConfigBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.apply_Button = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configFiles_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriverConfigBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // version_CheckedListBox
            // 
            this.version_CheckedListBox.CheckOnClick = true;
            this.version_CheckedListBox.FormattingEnabled = true;
            this.version_CheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.version_CheckedListBox.Name = "version_CheckedListBox";
            this.version_CheckedListBox.Size = new System.Drawing.Size(178, 349);
            this.version_CheckedListBox.TabIndex = 1;
            this.version_CheckedListBox.Click += new System.EventHandler(this.version_CheckedListBox_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.refreshVersion_LinkLabel);
            this.groupBox2.Controls.Add(this.versionUnselectAll_LinkLabel);
            this.groupBox2.Controls.Add(this.versionSelectAll_LinkLabel);
            this.groupBox2.Controls.Add(this.version_CheckedListBox);
            this.groupBox2.Location = new System.Drawing.Point(543, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 388);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Associated Versions";
            // 
            // refreshVersion_LinkLabel
            // 
            this.refreshVersion_LinkLabel.AutoSize = true;
            this.refreshVersion_LinkLabel.Location = new System.Drawing.Point(6, 369);
            this.refreshVersion_LinkLabel.Name = "refreshVersion_LinkLabel";
            this.refreshVersion_LinkLabel.Size = new System.Drawing.Size(44, 13);
            this.refreshVersion_LinkLabel.TabIndex = 5;
            this.refreshVersion_LinkLabel.TabStop = true;
            this.refreshVersion_LinkLabel.Text = "Refresh";
            this.refreshVersion_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refreshVersion_LinkLabel_LinkClicked);
            // 
            // versionUnselectAll_LinkLabel
            // 
            this.versionUnselectAll_LinkLabel.AutoSize = true;
            this.versionUnselectAll_LinkLabel.Location = new System.Drawing.Point(121, 369);
            this.versionUnselectAll_LinkLabel.Name = "versionUnselectAll_LinkLabel";
            this.versionUnselectAll_LinkLabel.Size = new System.Drawing.Size(63, 13);
            this.versionUnselectAll_LinkLabel.TabIndex = 4;
            this.versionUnselectAll_LinkLabel.TabStop = true;
            this.versionUnselectAll_LinkLabel.Text = "Unselect All";
            this.versionUnselectAll_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.versionUnselectAll_LinkLabel_LinkClicked);
            // 
            // versionSelectAll_LinkLabel
            // 
            this.versionSelectAll_LinkLabel.AutoSize = true;
            this.versionSelectAll_LinkLabel.Location = new System.Drawing.Point(64, 369);
            this.versionSelectAll_LinkLabel.Name = "versionSelectAll_LinkLabel";
            this.versionSelectAll_LinkLabel.Size = new System.Drawing.Size(51, 13);
            this.versionSelectAll_LinkLabel.TabIndex = 4;
            this.versionSelectAll_LinkLabel.TabStop = true;
            this.versionSelectAll_LinkLabel.Text = "Select All";
            this.versionSelectAll_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.versionSelectAll_LinkLabel_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.refreshProduct_LinkLabel);
            this.groupBox3.Controls.Add(this.productUnselectAll_LinkLabel);
            this.groupBox3.Controls.Add(this.productSelectAll_LinkLabel);
            this.groupBox3.Controls.Add(this.product_CheckedListBox);
            this.groupBox3.Location = new System.Drawing.Point(347, 49);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(190, 388);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Associated Products";
            // 
            // refreshProduct_LinkLabel
            // 
            this.refreshProduct_LinkLabel.AutoSize = true;
            this.refreshProduct_LinkLabel.Location = new System.Drawing.Point(6, 369);
            this.refreshProduct_LinkLabel.Name = "refreshProduct_LinkLabel";
            this.refreshProduct_LinkLabel.Size = new System.Drawing.Size(44, 13);
            this.refreshProduct_LinkLabel.TabIndex = 4;
            this.refreshProduct_LinkLabel.TabStop = true;
            this.refreshProduct_LinkLabel.Text = "Refresh";
            this.refreshProduct_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refreshProduct_LinkLabel_LinkClicked);
            // 
            // productUnselectAll_LinkLabel
            // 
            this.productUnselectAll_LinkLabel.AutoSize = true;
            this.productUnselectAll_LinkLabel.Location = new System.Drawing.Point(121, 369);
            this.productUnselectAll_LinkLabel.Name = "productUnselectAll_LinkLabel";
            this.productUnselectAll_LinkLabel.Size = new System.Drawing.Size(63, 13);
            this.productUnselectAll_LinkLabel.TabIndex = 3;
            this.productUnselectAll_LinkLabel.TabStop = true;
            this.productUnselectAll_LinkLabel.Text = "Unselect All";
            this.productUnselectAll_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.productUnselectAll_LinkLabel_LinkClicked);
            // 
            // productSelectAll_LinkLabel
            // 
            this.productSelectAll_LinkLabel.AutoSize = true;
            this.productSelectAll_LinkLabel.Location = new System.Drawing.Point(64, 369);
            this.productSelectAll_LinkLabel.Name = "productSelectAll_LinkLabel";
            this.productSelectAll_LinkLabel.Size = new System.Drawing.Size(51, 13);
            this.productSelectAll_LinkLabel.TabIndex = 2;
            this.productSelectAll_LinkLabel.TabStop = true;
            this.productSelectAll_LinkLabel.Text = "Select All";
            this.productSelectAll_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.productSelectAll_LinkLabel_LinkClicked);
            // 
            // product_CheckedListBox
            // 
            this.product_CheckedListBox.CheckOnClick = true;
            this.product_CheckedListBox.FormattingEnabled = true;
            this.product_CheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.product_CheckedListBox.Name = "product_CheckedListBox";
            this.product_CheckedListBox.Size = new System.Drawing.Size(178, 349);
            this.product_CheckedListBox.TabIndex = 1;
            this.product_CheckedListBox.Click += new System.EventHandler(this.product_CheckedListBox_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(575, 453);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 6;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(493, 453);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 7;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // upload_Button
            // 
            this.upload_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.upload_Button.Location = new System.Drawing.Point(12, 453);
            this.upload_Button.Name = "upload_Button";
            this.upload_Button.Size = new System.Drawing.Size(75, 23);
            this.upload_Button.TabIndex = 8;
            this.upload_Button.Text = "Add";
            this.toolTip1.SetToolTip(this.upload_Button, "Adds  config file to database and uploads to fileserver if necessary");
            this.upload_Button.UseVisualStyleBackColor = true;
            this.upload_Button.Click += new System.EventHandler(this.upload_Button_Click);
            // 
            // configFiles_DataGridView
            // 
            this.configFiles_DataGridView.AllowUserToAddRows = false;
            this.configFiles_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.configFiles_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.configFiles_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configFiles_DataGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.configFiles_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.configFiles_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.configFiles_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.driverConfigIdDataGridViewTextBoxColumn,
            this.configFileDataGridViewTextBoxColumn,
            this.printDriverProductsDataGridViewTextBoxColumn,
            this.printDriverVersionsDataGridViewTextBoxColumn});
            this.configFiles_DataGridView.DataSource = this.printDriverConfigBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.configFiles_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.configFiles_DataGridView.Location = new System.Drawing.Point(6, 19);
            this.configFiles_DataGridView.MultiSelect = false;
            this.configFiles_DataGridView.Name = "configFiles_DataGridView";
            this.configFiles_DataGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.configFiles_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.configFiles_DataGridView.RowHeadersVisible = false;
            this.configFiles_DataGridView.RowTemplate.Height = 22;
            this.configFiles_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.configFiles_DataGridView.Size = new System.Drawing.Size(317, 363);
            this.configFiles_DataGridView.TabIndex = 9;
            this.configFiles_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.configFiles_DataGridView_CellClick);
            // 
            // driverConfigIdDataGridViewTextBoxColumn
            // 
            this.driverConfigIdDataGridViewTextBoxColumn.DataPropertyName = "DriverConfigId";
            this.driverConfigIdDataGridViewTextBoxColumn.HeaderText = "Driver Config Id";
            this.driverConfigIdDataGridViewTextBoxColumn.Name = "driverConfigIdDataGridViewTextBoxColumn";
            this.driverConfigIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverConfigIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // configFileDataGridViewTextBoxColumn
            // 
            this.configFileDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.configFileDataGridViewTextBoxColumn.DataPropertyName = "ConfigFile";
            this.configFileDataGridViewTextBoxColumn.HeaderText = "Driver Configuration (CFM) File";
            this.configFileDataGridViewTextBoxColumn.Name = "configFileDataGridViewTextBoxColumn";
            this.configFileDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // printDriverProductsDataGridViewTextBoxColumn
            // 
            this.printDriverProductsDataGridViewTextBoxColumn.DataPropertyName = "PrintDriverProducts";
            this.printDriverProductsDataGridViewTextBoxColumn.HeaderText = "Print Driver Products";
            this.printDriverProductsDataGridViewTextBoxColumn.Name = "printDriverProductsDataGridViewTextBoxColumn";
            this.printDriverProductsDataGridViewTextBoxColumn.ReadOnly = true;
            this.printDriverProductsDataGridViewTextBoxColumn.Visible = false;
            // 
            // printDriverVersionsDataGridViewTextBoxColumn
            // 
            this.printDriverVersionsDataGridViewTextBoxColumn.DataPropertyName = "PrintDriverVersions";
            this.printDriverVersionsDataGridViewTextBoxColumn.HeaderText = "Print Driver Versions";
            this.printDriverVersionsDataGridViewTextBoxColumn.Name = "printDriverVersionsDataGridViewTextBoxColumn";
            this.printDriverVersionsDataGridViewTextBoxColumn.ReadOnly = true;
            this.printDriverVersionsDataGridViewTextBoxColumn.Visible = false;
            // 
            // printDriverConfigBindingSource
            // 
            this.printDriverConfigBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.PrintDriverConfig);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.configFiles_DataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 388);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available CFM Files";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.MinimumSize = new System.Drawing.Size(200, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(654, 33);
            this.label1.TabIndex = 11;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(656, 453);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 12;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // PrintDriverCfmManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 488);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.upload_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintDriverCfmManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CFM File Management";
            this.Load += new System.EventHandler(this.PrintDriverCFMManagementForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configFiles_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriverConfigBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox version_CheckedListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox product_CheckedListBox;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button upload_Button;
        private System.Windows.Forms.DataGridView configFiles_DataGridView;
        private System.Windows.Forms.BindingSource printDriverConfigBindingSource;
        private System.Windows.Forms.LinkLabel versionUnselectAll_LinkLabel;
        private System.Windows.Forms.LinkLabel versionSelectAll_LinkLabel;
        private System.Windows.Forms.LinkLabel productUnselectAll_LinkLabel;
        private System.Windows.Forms.LinkLabel productSelectAll_LinkLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.LinkLabel refreshVersion_LinkLabel;
        private System.Windows.Forms.LinkLabel refreshProduct_LinkLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverConfigIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn configFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printDriverProductsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printDriverVersionsDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}