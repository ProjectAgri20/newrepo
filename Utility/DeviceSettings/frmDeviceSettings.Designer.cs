namespace HP.RDL.STF.DeviceSettings
{
    partial class FrmDeviceSettings
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDeviceSettings));
			this.btnRun = new System.Windows.Forms.Button();
			this.txtIPAddress = new System.Windows.Forms.TextBox();
			this.dgvDataFim = new System.Windows.Forms.DataGridView();
			this.endPointDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.parentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.elementDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueOrigDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueNewDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataFimsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblProductName = new System.Windows.Forms.Label();
			this.lblFimOld = new System.Windows.Forms.Label();
			this.lblFimNew = new System.Windows.Forms.Label();
			this.btnExit = new System.Windows.Forms.Button();
			this.sdgExcel = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.msFiles = new System.Windows.Forms.MenuStrip();
			this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
			this.MnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.MnuFileClear = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.ofdFiles = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.dgvDataFim)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataFimsBindingSource)).BeginInit();
			this.msFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(666, 31);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(75, 23);
			this.btnRun.TabIndex = 0;
			this.btnRun.Text = "Run";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.BtnRunClick);
			// 
			// txtIPAddress
			// 
			this.txtIPAddress.Location = new System.Drawing.Point(12, 50);
			this.txtIPAddress.Name = "txtIPAddress";
			this.txtIPAddress.Size = new System.Drawing.Size(100, 20);
			this.txtIPAddress.TabIndex = 1;
			this.txtIPAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtIPAddress_KeyDown);
			// 
			// dgvDataFim
			// 
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.Wheat;
			this.dgvDataFim.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvDataFim.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvDataFim.AutoGenerateColumns = false;
			this.dgvDataFim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDataFim.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.endPointDataGridViewTextBoxColumn,
            this.parentDataGridViewTextBoxColumn,
            this.elementDataGridViewTextBoxColumn,
            this.valueOrigDataGridViewTextBoxColumn,
            this.valueNewDataGridViewTextBoxColumn});
			this.dgvDataFim.DataSource = this.dataFimsBindingSource;
			this.dgvDataFim.Location = new System.Drawing.Point(12, 83);
			this.dgvDataFim.Name = "dgvDataFim";
			this.dgvDataFim.ReadOnly = true;
			this.dgvDataFim.RowHeadersVisible = false;
			this.dgvDataFim.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvDataFim.ShowCellErrors = false;
			this.dgvDataFim.ShowCellToolTips = false;
			this.dgvDataFim.ShowEditingIcon = false;
			this.dgvDataFim.ShowRowErrors = false;
			this.dgvDataFim.Size = new System.Drawing.Size(729, 383);
			this.dgvDataFim.TabIndex = 2;
			// 
			// endPointDataGridViewTextBoxColumn
			// 
			this.endPointDataGridViewTextBoxColumn.DataPropertyName = "EndPoint";
			this.endPointDataGridViewTextBoxColumn.HeaderText = "Source";
			this.endPointDataGridViewTextBoxColumn.Name = "endPointDataGridViewTextBoxColumn";
			this.endPointDataGridViewTextBoxColumn.ReadOnly = true;
			this.endPointDataGridViewTextBoxColumn.Width = 150;
			// 
			// parentDataGridViewTextBoxColumn
			// 
			this.parentDataGridViewTextBoxColumn.DataPropertyName = "Parent";
			this.parentDataGridViewTextBoxColumn.HeaderText = "Region";
			this.parentDataGridViewTextBoxColumn.Name = "parentDataGridViewTextBoxColumn";
			this.parentDataGridViewTextBoxColumn.ReadOnly = true;
			this.parentDataGridViewTextBoxColumn.Width = 150;
			// 
			// elementDataGridViewTextBoxColumn
			// 
			this.elementDataGridViewTextBoxColumn.DataPropertyName = "Element";
			this.elementDataGridViewTextBoxColumn.HeaderText = "Element";
			this.elementDataGridViewTextBoxColumn.Name = "elementDataGridViewTextBoxColumn";
			this.elementDataGridViewTextBoxColumn.ReadOnly = true;
			this.elementDataGridViewTextBoxColumn.Width = 150;
			// 
			// valueOrigDataGridViewTextBoxColumn
			// 
			this.valueOrigDataGridViewTextBoxColumn.DataPropertyName = "ValueOrig";
			this.valueOrigDataGridViewTextBoxColumn.HeaderText = "Before FIM";
			this.valueOrigDataGridViewTextBoxColumn.Name = "valueOrigDataGridViewTextBoxColumn";
			this.valueOrigDataGridViewTextBoxColumn.ReadOnly = true;
			this.valueOrigDataGridViewTextBoxColumn.Width = 125;
			// 
			// valueNewDataGridViewTextBoxColumn
			// 
			this.valueNewDataGridViewTextBoxColumn.DataPropertyName = "ValueNew";
			this.valueNewDataGridViewTextBoxColumn.HeaderText = "After FIM";
			this.valueNewDataGridViewTextBoxColumn.Name = "valueNewDataGridViewTextBoxColumn";
			this.valueNewDataGridViewTextBoxColumn.ReadOnly = true;
			this.valueNewDataGridViewTextBoxColumn.Width = 125;
			// 
			// dataFimsBindingSource
			// 
			//this.dataFimsBindingSource.DataSource = typeof(HP.RDL.STF.DeviceSettings.DataFims);
			// 
			// lblProductName
			// 
			this.lblProductName.AutoSize = true;
			this.lblProductName.ForeColor = System.Drawing.Color.Maroon;
			this.lblProductName.Location = new System.Drawing.Point(134, 31);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.Size = new System.Drawing.Size(125, 13);
			this.lblProductName.TabIndex = 3;
			this.lblProductName.Text = "Device Name and Model";
			this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblFimOld
			// 
			this.lblFimOld.AutoSize = true;
			this.lblFimOld.ForeColor = System.Drawing.Color.Maroon;
			this.lblFimOld.Location = new System.Drawing.Point(134, 57);
			this.lblFimOld.Name = "lblFimOld";
			this.lblFimOld.Size = new System.Drawing.Size(63, 13);
			this.lblFimOld.TabIndex = 4;
			this.lblFimOld.Text = "Original FIM";
			this.lblFimOld.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblFimNew
			// 
			this.lblFimNew.AutoSize = true;
			this.lblFimNew.ForeColor = System.Drawing.Color.Maroon;
			this.lblFimNew.Location = new System.Drawing.Point(409, 57);
			this.lblFimNew.Name = "lblFimNew";
			this.lblFimNew.Size = new System.Drawing.Size(69, 13);
			this.lblFimNew.TabIndex = 5;
			this.lblFimNew.Text = "Updated FIM";
			this.lblFimNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.Location = new System.Drawing.Point(666, 476);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 6;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.Maroon;
			this.label1.Location = new System.Drawing.Point(9, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "IP Address";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// msFiles
			// 
			this.msFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem});
			this.msFiles.Location = new System.Drawing.Point(0, 0);
			this.msFiles.Name = "msFiles";
			this.msFiles.Size = new System.Drawing.Size(753, 24);
			this.msFiles.TabIndex = 9;
			this.msFiles.Text = "menuStrip1";
			// 
			// fILEToolStripMenuItem
			// 
			this.fILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuFileOpen,
            this.MnuFileSave,
            this.MnuFileSaveAs,
            this.toolStripMenuItem2,
            this.MnuFileClear,
            this.toolStripMenuItem1,
            this.MnuFileExit});
			this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
			this.fILEToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			this.fILEToolStripMenuItem.Text = "FILE";
			// 
			// MnuFileOpen
			// 
			this.MnuFileOpen.Name = "MnuFileOpen";
			this.MnuFileOpen.Size = new System.Drawing.Size(123, 22);
			this.MnuFileOpen.Text = "&Open...";
			this.MnuFileOpen.Click += new System.EventHandler(this.MnuFileOpen_Click);
			// 
			// MnuFileSave
			// 
			this.MnuFileSave.Name = "MnuFileSave";
			this.MnuFileSave.Size = new System.Drawing.Size(123, 22);
			this.MnuFileSave.Text = "&Save";
			this.MnuFileSave.Click += new System.EventHandler(this.MnuFileSave_Click);
			// 
			// MnuFileSaveAs
			// 
			this.MnuFileSaveAs.Name = "MnuFileSaveAs";
			this.MnuFileSaveAs.Size = new System.Drawing.Size(123, 22);
			this.MnuFileSaveAs.Text = "Save &As...";
			this.MnuFileSaveAs.Click += new System.EventHandler(this.MnuFileSaveAs_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(120, 6);
			// 
			// MnuFileClear
			// 
			this.MnuFileClear.Name = "MnuFileClear";
			this.MnuFileClear.Size = new System.Drawing.Size(123, 22);
			this.MnuFileClear.Text = "Clear";
			this.MnuFileClear.Click += new System.EventHandler(this.MnuFileClear_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(120, 6);
			// 
			// MnuFileExit
			// 
			this.MnuFileExit.Name = "MnuFileExit";
			this.MnuFileExit.Size = new System.Drawing.Size(123, 22);
			this.MnuFileExit.Text = "E&xit";
			this.MnuFileExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// ofdFiles
			// 
			this.ofdFiles.FileName = "ofdFiles";
			// 
			// FrmDeviceSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(753, 507);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lblFimNew);
			this.Controls.Add(this.lblFimOld);
			this.Controls.Add(this.lblProductName);
			this.Controls.Add(this.dgvDataFim);
			this.Controls.Add(this.txtIPAddress);
			this.Controls.Add(this.btnRun);
			this.Controls.Add(this.msFiles);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.msFiles;
			this.Name = "FrmDeviceSettings";
			this.Text = "Jedi Firmware Comparison";
			((System.ComponentModel.ISupportInitialize)(this.dgvDataFim)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataFimsBindingSource)).EndInit();
			this.msFiles.ResumeLayout(false);
			this.msFiles.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.DataGridView dgvDataFim;
        private System.Windows.Forms.BindingSource dataFimsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn endPointDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn elementDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueOrigDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueNewDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblFimOld;
        private System.Windows.Forms.Label lblFimNew;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.SaveFileDialog sdgExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip msFiles;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MnuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem MnuFileSave;
        private System.Windows.Forms.ToolStripMenuItem MnuFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem MnuFileExit;
        private System.Windows.Forms.OpenFileDialog ofdFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MnuFileClear;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}

