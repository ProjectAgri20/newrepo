namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportDevicePoolControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.resolveDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.poolNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.resolveDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // resolveDataGridView
            // 
            this.resolveDataGridView.AllowUserToAddRows = false;
            this.resolveDataGridView.AllowUserToDeleteRows = false;
            this.resolveDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.resolveDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.resolveDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.resolveDataGridView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.resolveDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.resolveDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resolveDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.poolNameColumn});
            this.resolveDataGridView.Location = new System.Drawing.Point(18, 17);
            this.resolveDataGridView.Name = "resolveDataGridView";
            this.resolveDataGridView.RowHeadersVisible = false;
            this.resolveDataGridView.RowTemplate.Height = 28;
            this.resolveDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resolveDataGridView.Size = new System.Drawing.Size(298, 206);
            this.resolveDataGridView.TabIndex = 4;
            this.resolveDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.resolveDataGridView_DataError);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(343, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 104);
            this.label1.TabIndex = 5;
            this.label1.Text = "An Asset Pool is a grouping of assets assigned to a team.  If you are unsure whic" +
    "h pool you should select, check with your administrator.";
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameColumn.DataPropertyName = "AssetId";
            this.nameColumn.HeaderText = "Device Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.Width = 127;
            // 
            // poolNameColumn
            // 
            this.poolNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.poolNameColumn.DataPropertyName = "PoolName";
            this.poolNameColumn.HeaderText = "Pool Name";
            this.poolNameColumn.Name = "poolNameColumn";
            this.poolNameColumn.ToolTipText = "Specifies the asset pool this device belongs to";
            // 
            // ImportDevicePoolControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resolveDataGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportDevicePoolControl";
            this.Size = new System.Drawing.Size(649, 241);
            this.Load += new System.EventHandler(this.ImportDevicePoolControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.resolveDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView resolveDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn poolNameColumn;
    }
}
