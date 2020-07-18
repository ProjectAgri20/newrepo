namespace HP.RDL.RdlHPMibTranslator
{
	partial class FrmTranslator
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
			Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
			Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
			Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
			Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
			Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTranslator));
			this.mnuFile = new Telerik.WinControls.UI.RadMenuItem();
			this.mnuFileOpen = new Telerik.WinControls.UI.RadMenuItem();
			this.rMnuAddMib = new Telerik.WinControls.UI.RadMenuItem();
			this.mnuFileOpenCsv = new Telerik.WinControls.UI.RadMenuItem();
			this.radMenuSeparatorItem1 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
			this.mnuFileSave = new Telerik.WinControls.UI.RadMenuItem();
			this.mnuFileClear = new Telerik.WinControls.UI.RadMenuItem();
			this.openDlg = new System.Windows.Forms.OpenFileDialog();
			this.txtIpAddr = new System.Windows.Forms.TextBox();
			this.lblIpAddr = new System.Windows.Forms.Label();
			this.txtOidValue = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblReleaseName = new System.Windows.Forms.Label();
			this.rgvOidInfo = new Telerik.WinControls.UI.RadGridView();
			this.listOidEntBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnExit = new System.Windows.Forms.Button();
			this.btnGetOids = new System.Windows.Forms.Button();
			this.saveDlg = new System.Windows.Forms.SaveFileDialog();
			this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
			this.MnuOptions = new Telerik.WinControls.UI.RadMenuItem();
			this.MnuOptSnmpVer1 = new Telerik.WinControls.UI.RadMenuItem();
			this.MnuOptSnmpVer2 = new Telerik.WinControls.UI.RadMenuItem();
			this.lblInfo = new System.Windows.Forms.Label();
			this.lblOrder = new System.Windows.Forms.Label();
			this.MnuOptPWD = new Telerik.WinControls.UI.RadMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.rgvOidInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rgvOidInfo.MasterTemplate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.listOidEntBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
			this.SuspendLayout();
			// 
			// mnuFile
			// 
			this.mnuFile.AccessibleDescription = "File";
			this.mnuFile.AccessibleName = "File";
			this.mnuFile.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
			this.mnuFile.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuFileOpen,
            this.mnuFileOpenCsv,
            this.radMenuSeparatorItem1,
            this.mnuFileSave,
            this.mnuFileClear});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Text = "File";
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.AccessibleDescription = "Open";
			this.mnuFileOpen.AccessibleName = "Open";
			this.mnuFileOpen.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rMnuAddMib});
			this.mnuFileOpen.Name = "mnuFileOpen";
			this.mnuFileOpen.Text = "Open MIB";
			this.mnuFileOpen.Click += new System.EventHandler(this.MnuFileOpenClick);
			// 
			// rMnuAddMib
			// 
			this.rMnuAddMib.AccessibleDescription = "Add MIB";
			this.rMnuAddMib.AccessibleName = "Add MIB";
			this.rMnuAddMib.Name = "rMnuAddMib";
			this.rMnuAddMib.Text = "Add MIB";
			this.rMnuAddMib.Click += new System.EventHandler(this.RMnuAddMibClick);
			// 
			// mnuFileOpenCsv
			// 
			this.mnuFileOpenCsv.AccessibleDescription = "Open CSV";
			this.mnuFileOpenCsv.AccessibleName = "Open CSV";
			this.mnuFileOpenCsv.Name = "mnuFileOpenCsv";
			this.mnuFileOpenCsv.Text = "Open CSV";
			this.mnuFileOpenCsv.Click += new System.EventHandler(this.MnuFileOpenCsv);
			// 
			// radMenuSeparatorItem1
			// 
			this.radMenuSeparatorItem1.AccessibleDescription = "radMenuSeparatorItem1";
			this.radMenuSeparatorItem1.AccessibleName = "radMenuSeparatorItem1";
			this.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1";
			this.radMenuSeparatorItem1.Text = "radMenuSeparatorItem1";
			// 
			// mnuFileSave
			// 
			this.mnuFileSave.AccessibleDescription = "Save";
			this.mnuFileSave.AccessibleName = "Save";
			this.mnuFileSave.Name = "mnuFileSave";
			this.mnuFileSave.Text = "Save";
			this.mnuFileSave.Click += new System.EventHandler(this.MnuFileSave_Click);
			// 
			// mnuFileClear
			// 
			this.mnuFileClear.AccessibleDescription = "Clear";
			this.mnuFileClear.AccessibleName = "Clear";
			this.mnuFileClear.Name = "mnuFileClear";
			this.mnuFileClear.Text = "Clear";
			this.mnuFileClear.Click += new System.EventHandler(this.MnuFileClearClick);
			// 
			// openDlg
			// 
			this.openDlg.FileName = "openFileDialog1";
			// 
			// txtIpAddr
			// 
			this.txtIpAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtIpAddr.Location = new System.Drawing.Point(804, 26);
			this.txtIpAddr.Name = "txtIpAddr";
			this.txtIpAddr.Size = new System.Drawing.Size(169, 20);
			this.txtIpAddr.TabIndex = 1;
			// 
			// lblIpAddr
			// 
			this.lblIpAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIpAddr.AutoSize = true;
			this.lblIpAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblIpAddr.ForeColor = System.Drawing.Color.Maroon;
			this.lblIpAddr.Location = new System.Drawing.Point(682, 29);
			this.lblIpAddr.Name = "lblIpAddr";
			this.lblIpAddr.Size = new System.Drawing.Size(116, 13);
			this.lblIpAddr.TabIndex = 2;
			this.lblIpAddr.Text = "Device IP Address:";
			// 
			// txtOidValue
			// 
			this.txtOidValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOidValue.Location = new System.Drawing.Point(804, 52);
			this.txtOidValue.Name = "txtOidValue";
			this.txtOidValue.Size = new System.Drawing.Size(169, 20);
			this.txtOidValue.TabIndex = 3;
			this.txtOidValue.Text = "1.3.6.1.4.1.11";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Maroon;
			this.label1.Location = new System.Drawing.Point(677, 55);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "HP Start OID Value:";
			// 
			// lblReleaseName
			// 
			this.lblReleaseName.AutoSize = true;
			this.lblReleaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReleaseName.ForeColor = System.Drawing.Color.Maroon;
			this.lblReleaseName.Location = new System.Drawing.Point(12, 59);
			this.lblReleaseName.Name = "lblReleaseName";
			this.lblReleaseName.Size = new System.Drawing.Size(133, 13);
			this.lblReleaseName.TabIndex = 5;
			this.lblReleaseName.Text = "Device Release Name";
			// 
			// rgvOidInfo
			// 
			this.rgvOidInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rgvOidInfo.Location = new System.Drawing.Point(15, 78);
			// 
			// 
			// 
			this.rgvOidInfo.MasterTemplate.AllowAddNewRow = false;
			this.rgvOidInfo.MasterTemplate.AllowDeleteRow = false;
			this.rgvOidInfo.MasterTemplate.AllowEditRow = false;
			gridViewTextBoxColumn1.FieldName = "OidName";
			gridViewTextBoxColumn1.HeaderText = "OID Name";
			gridViewTextBoxColumn1.IsAutoGenerated = true;
			gridViewTextBoxColumn1.MinWidth = 20;
			gridViewTextBoxColumn1.Name = "OidName";
			gridViewTextBoxColumn1.Width = 150;
			gridViewTextBoxColumn2.FieldName = "OidString";
			gridViewTextBoxColumn2.HeaderText = "MIB OID";
			gridViewTextBoxColumn2.IsAutoGenerated = true;
			gridViewTextBoxColumn2.MinWidth = 20;
			gridViewTextBoxColumn2.Name = "OidString";
			gridViewTextBoxColumn2.Width = 150;
			gridViewTextBoxColumn3.FieldName = "OidIndex";
			gridViewTextBoxColumn3.HeaderText = "HP Device OID";
			gridViewTextBoxColumn3.IsAutoGenerated = true;
			gridViewTextBoxColumn3.MinWidth = 20;
			gridViewTextBoxColumn3.Name = "OidIndex";
			gridViewTextBoxColumn3.Width = 175;
			gridViewTextBoxColumn4.FieldName = "OidIndexValue";
			gridViewTextBoxColumn4.HeaderText = "HP Device Value";
			gridViewTextBoxColumn4.IsAutoGenerated = true;
			gridViewTextBoxColumn4.Name = "OidIndexValue";
			gridViewTextBoxColumn4.Width = 100;
			gridViewTextBoxColumn5.FieldName = "SyntaxValue";
			gridViewTextBoxColumn5.HeaderText = "Syntax";
			gridViewTextBoxColumn5.IsAutoGenerated = true;
			gridViewTextBoxColumn5.Name = "SyntaxValue";
			gridViewTextBoxColumn5.ReadOnly = true;
			gridViewTextBoxColumn5.Width = 75;
			gridViewTextBoxColumn6.FieldName = "Access";
			gridViewTextBoxColumn6.HeaderText = "Access";
			gridViewTextBoxColumn6.IsAutoGenerated = true;
			gridViewTextBoxColumn6.Name = "Access";
			gridViewTextBoxColumn6.Width = 75;
			gridViewTextBoxColumn7.FieldName = "Status";
			gridViewTextBoxColumn7.HeaderText = "Status";
			gridViewTextBoxColumn7.IsAutoGenerated = true;
			gridViewTextBoxColumn7.Name = "Status";
			gridViewTextBoxColumn7.Width = 75;
			gridViewTextBoxColumn8.FieldName = "Description";
			gridViewTextBoxColumn8.HeaderText = "Description";
			gridViewTextBoxColumn8.IsAutoGenerated = true;
			gridViewTextBoxColumn8.Name = "Description";
			gridViewTextBoxColumn8.Width = 200;
			gridViewTextBoxColumn9.DataType = typeof(System.Collections.Generic.List<string>);
			gridViewTextBoxColumn9.FieldName = "Syntax";
			gridViewTextBoxColumn9.HeaderText = "Syntax List";
			gridViewTextBoxColumn9.IsAutoGenerated = true;
			gridViewTextBoxColumn9.IsVisible = false;
			gridViewTextBoxColumn9.Name = "Syntax";
			gridViewTextBoxColumn10.FieldName = "Parent";
			gridViewTextBoxColumn10.HeaderText = "Parent";
			gridViewTextBoxColumn10.IsAutoGenerated = true;
			gridViewTextBoxColumn10.IsVisible = false;
			gridViewTextBoxColumn10.Name = "Parent";
			gridViewTextBoxColumn11.FieldName = "Value";
			gridViewTextBoxColumn11.HeaderText = "Value";
			gridViewTextBoxColumn11.IsAutoGenerated = true;
			gridViewTextBoxColumn11.IsVisible = false;
			gridViewTextBoxColumn11.Name = "Value";
			gridViewTextBoxColumn12.FieldName = "Enumerations";
			gridViewTextBoxColumn12.HeaderText = "Enumerations";
			gridViewTextBoxColumn12.IsAutoGenerated = true;
			gridViewTextBoxColumn12.IsVisible = false;
			gridViewTextBoxColumn12.Name = "Enumerations";
			gridViewTextBoxColumn12.ReadOnly = true;
			this.rgvOidInfo.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12});
			this.rgvOidInfo.MasterTemplate.DataSource = this.listOidEntBindingSource;
			this.rgvOidInfo.MasterTemplate.EnableAlternatingRowColor = true;
			this.rgvOidInfo.MasterTemplate.ShowRowHeaderColumn = false;
			this.rgvOidInfo.Name = "rgvOidInfo";
			this.rgvOidInfo.ReadOnly = true;
			this.rgvOidInfo.Size = new System.Drawing.Size(946, 473);
			this.rgvOidInfo.TabIndex = 6;
			this.rgvOidInfo.Text = "radGridView1";
			// 
			// listOidEntBindingSource
			// 
			this.listOidEntBindingSource.DataSource = typeof(HP.RDL.RdlHPMibTranslator.ListOidEnt);
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.Location = new System.Drawing.Point(891, 562);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 32);
			this.btnExit.TabIndex = 7;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// btnGetOids
			// 
			this.btnGetOids.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGetOids.Location = new System.Drawing.Point(587, 33);
			this.btnGetOids.Name = "btnGetOids";
			this.btnGetOids.Size = new System.Drawing.Size(75, 32);
			this.btnGetOids.TabIndex = 8;
			this.btnGetOids.Text = "Get OIDs";
			this.btnGetOids.UseVisualStyleBackColor = true;
			this.btnGetOids.Click += new System.EventHandler(this.BtnGetOidsClick);
			// 
			// radMenu1
			// 
			this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuFile,
            this.MnuOptions});
			this.radMenu1.Location = new System.Drawing.Point(0, 0);
			this.radMenu1.Name = "radMenu1";
			this.radMenu1.Size = new System.Drawing.Size(973, 20);
			this.radMenu1.TabIndex = 0;
			this.radMenu1.Text = "radMenu1";
			// 
			// MnuOptions
			// 
			this.MnuOptions.AccessibleDescription = "Options";
			this.MnuOptions.AccessibleName = "Options";
			this.MnuOptions.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.MnuOptSnmpVer1,
            this.MnuOptSnmpVer2,
            this.MnuOptPWD});
			this.MnuOptions.Name = "MnuOptions";
			this.MnuOptions.Text = "Options";
			// 
			// MnuOptSnmpVer1
			// 
			this.MnuOptSnmpVer1.AccessibleDescription = "SNMP Version 1";
			this.MnuOptSnmpVer1.AccessibleName = "SNMP Version 1";
			this.MnuOptSnmpVer1.CheckOnClick = true;
			this.MnuOptSnmpVer1.Name = "MnuOptSnmpVer1";
			this.MnuOptSnmpVer1.Tag = "SNMPVer1";
			this.MnuOptSnmpVer1.Text = "SNMP Version 1";
			this.MnuOptSnmpVer1.Click += new System.EventHandler(this.MnuOptSnmpVer_Click);
			// 
			// MnuOptSnmpVer2
			// 
			this.MnuOptSnmpVer2.AccessibleDescription = "SNMP Version 2";
			this.MnuOptSnmpVer2.AccessibleName = "SNMP Version 2";
			this.MnuOptSnmpVer2.CheckOnClick = true;
			this.MnuOptSnmpVer2.Name = "MnuOptSnmpVer2";
			this.MnuOptSnmpVer2.Tag = "SNMPVer2";
			this.MnuOptSnmpVer2.Text = "SNMP Version 2";
			this.MnuOptSnmpVer2.Click += new System.EventHandler(this.MnuOptSnmpVer_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.AutoSize = true;
			this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.Maroon;
			this.lblInfo.Location = new System.Drawing.Point(352, 29);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(229, 39);
			this.lblInfo.TabIndex = 9;
			this.lblInfo.Text = "1: Load the HP MIB\r\n2: Run desired OID tree against device\r\n3: Load Managed Contr" +
    "acts\' csv file";
			// 
			// lblOrder
			// 
			this.lblOrder.AutoSize = true;
			this.lblOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOrder.ForeColor = System.Drawing.Color.Maroon;
			this.lblOrder.Location = new System.Drawing.Point(152, 29);
			this.lblOrder.Name = "lblOrder";
			this.lblOrder.Size = new System.Drawing.Size(194, 13);
			this.lblOrder.TabIndex = 10;
			this.lblOrder.Text = "Recommended Order of retrieval:";
			// 
			// MnuOptPWD
			// 
			this.MnuOptPWD.AccessibleDescription = "Admin PWD";
			this.MnuOptPWD.AccessibleName = "Admin PWD";
			this.MnuOptPWD.Name = "MnuOptPWD";
			this.MnuOptPWD.Text = "Admin PWD";
			// 
			// FrmTranslator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(973, 597);
			this.Controls.Add(this.lblOrder);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.btnGetOids);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.rgvOidInfo);
			this.Controls.Add(this.lblReleaseName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtOidValue);
			this.Controls.Add(this.lblIpAddr);
			this.Controls.Add(this.txtIpAddr);
			this.Controls.Add(this.radMenu1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmTranslator";
			this.Text = "RDL HP OID and MIB Translator";
			this.Load += new System.EventHandler(this.FrmTranslator_Load);
			this.Shown += new System.EventHandler(this.FrmTranslator_Shown);
			((System.ComponentModel.ISupportInitialize)(this.rgvOidInfo.MasterTemplate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rgvOidInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.listOidEntBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Telerik.WinControls.UI.RadMenuItem mnuFile;
		private Telerik.WinControls.UI.RadMenuItem mnuFileOpen;
		private System.Windows.Forms.OpenFileDialog openDlg;
		private System.Windows.Forms.TextBox txtIpAddr;
		private System.Windows.Forms.Label lblIpAddr;
		private System.Windows.Forms.TextBox txtOidValue;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblReleaseName;
		private Telerik.WinControls.UI.RadGridView rgvOidInfo;
		private System.Windows.Forms.BindingSource listOidEntBindingSource;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnGetOids;
		private Telerik.WinControls.UI.RadMenuItem mnuFileOpenCsv;
		private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem1;
		private Telerik.WinControls.UI.RadMenuItem mnuFileSave;
		private Telerik.WinControls.UI.RadMenu radMenu1;
		private System.Windows.Forms.SaveFileDialog saveDlg;
        private Telerik.WinControls.UI.RadMenuItem rMnuAddMib;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblOrder;
        private Telerik.WinControls.UI.RadMenuItem mnuFileClear;
        private Telerik.WinControls.UI.RadMenuItem MnuOptions;
        private Telerik.WinControls.UI.RadMenuItem MnuOptSnmpVer1;
        private Telerik.WinControls.UI.RadMenuItem MnuOptSnmpVer2;
		private Telerik.WinControls.UI.RadMenuItem MnuOptPWD;

	}
}

