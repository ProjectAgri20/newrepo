namespace HP.SolutionTest.WorkBench
{
    partial class WizardDeviceSetupPage
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.deviceSetup_GridView = new Telerik.WinControls.UI.RadGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CrcPaperLess_ToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.deviceSetup_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceSetup_GridView.MasterTemplate)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceSetup_GridView
            // 
            this.deviceSetup_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceSetup_GridView.BackColor = System.Drawing.SystemColors.Window;
            this.deviceSetup_GridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.deviceSetup_GridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.deviceSetup_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.deviceSetup_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.deviceSetup_GridView.Location = new System.Drawing.Point(12, 34);
            // 
            // 
            // 
            this.deviceSetup_GridView.MasterTemplate.AllowAddNewRow = false;
            this.deviceSetup_GridView.MasterTemplate.AllowDeleteRow = false;
            this.deviceSetup_GridView.MasterTemplate.AllowDragToGroup = false;
            this.deviceSetup_GridView.MasterTemplate.AutoGenerateColumns = false;
            this.deviceSetup_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "AssetId";
            gridViewTextBoxColumn1.HeaderText = "Device ID";
            gridViewTextBoxColumn1.MaxWidth = 118;
            gridViewTextBoxColumn1.MinWidth = 50;
            gridViewTextBoxColumn1.Name = "deviceId_GridViewColumn";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 103;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "Address";
            gridViewTextBoxColumn2.HeaderText = "Address";
            gridViewTextBoxColumn2.MaxWidth = 200;
            gridViewTextBoxColumn2.MinWidth = 75;
            gridViewTextBoxColumn2.Name = "address_GridViewColumn";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 127;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Description";
            gridViewTextBoxColumn3.HeaderText = "Description";
            gridViewTextBoxColumn3.MinWidth = 150;
            gridViewTextBoxColumn3.Name = "description_GridViewColumn";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.Width = 346;
            gridViewCheckBoxColumn1.EnableExpressionEditor = false;
            gridViewCheckBoxColumn1.FieldName = "UseCrc";
            gridViewCheckBoxColumn1.HeaderText = "Paperless Mode";
            gridViewCheckBoxColumn1.MaxWidth = 175;
            gridViewCheckBoxColumn1.MinWidth = 100;
            gridViewCheckBoxColumn1.Name = "crcMode_GridViewColumn";
            gridViewCheckBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewCheckBoxColumn1.Width = 121;
            this.deviceSetup_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewCheckBoxColumn1});
            this.deviceSetup_GridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.deviceSetup_GridView.MasterTemplate.EnableGrouping = false;
            this.deviceSetup_GridView.MasterTemplate.MultiSelect = true;
            this.deviceSetup_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.deviceSetup_GridView.Name = "deviceSetup_GridView";
            this.deviceSetup_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.deviceSetup_GridView.Size = new System.Drawing.Size(715, 342);
            this.deviceSetup_GridView.TabIndex = 3;
            this.deviceSetup_GridView.Text = "radGridView1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.CrcPaperLess_ToolStripBtn,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(730, 25);
            this.toolStrip1.TabIndex = 4;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(254, 22);
            this.toolStripLabel1.Text = "Select options for devices used in this scenario.";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // CrcPaperLess_ToolStripBtn
            // 
            this.CrcPaperLess_ToolStripBtn.Checked = true;
            this.CrcPaperLess_ToolStripBtn.CheckOnClick = true;
            this.CrcPaperLess_ToolStripBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CrcPaperLess_ToolStripBtn.Image = global::HP.SolutionTest.WorkBench.Properties.Resources.CheckboxOn;
            this.CrcPaperLess_ToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CrcPaperLess_ToolStripBtn.Name = "CrcPaperLess_ToolStripBtn";
            this.CrcPaperLess_ToolStripBtn.Size = new System.Drawing.Size(207, 22);
            this.CrcPaperLess_ToolStripBtn.Text = "Use paperless mode for all devices";
            this.CrcPaperLess_ToolStripBtn.Click += new System.EventHandler(this.CrcPaperLess_ToolStripBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // WizardDeviceSetupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.deviceSetup_GridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WizardDeviceSetupPage";
            this.Size = new System.Drawing.Size(730, 385);
            ((System.ComponentModel.ISupportInitialize)(this.deviceSetup_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceSetup_GridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private Telerik.WinControls.UI.RadGridView deviceSetup_GridView;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton CrcPaperLess_ToolStripBtn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
