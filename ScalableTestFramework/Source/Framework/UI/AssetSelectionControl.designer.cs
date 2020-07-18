namespace HP.ScalableTest.Framework.UI
{
    partial class AssetSelectionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetSelectionControl));
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.add_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.enableAll_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.quickEntry_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.asset_GridView = new Telerik.WinControls.UI.RadGridView();
            this.enableImages = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.asset_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.asset_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_ToolStripButton,
            this.remove_ToolStripButton,
            this.enableAll_ToolStripButton,
            this.toolStripSeparator,
            this.quickEntry_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(692, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // add_ToolStripButton
            // 
            this.add_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("add_ToolStripButton.Image")));
            this.add_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_ToolStripButton.Name = "add_ToolStripButton";
            this.add_ToolStripButton.Size = new System.Drawing.Size(49, 22);
            this.add_ToolStripButton.Text = "Add";
            this.add_ToolStripButton.Click += new System.EventHandler(this.add_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(70, 22);
            this.remove_ToolStripButton.Text = "Remove";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.remove_ToolStripButton_Click);
            // 
            // enableAll_ToolStripButton
            // 
            this.enableAll_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("enableAll_ToolStripButton.Image")));
            this.enableAll_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.enableAll_ToolStripButton.Name = "enableAll_ToolStripButton";
            this.enableAll_ToolStripButton.Size = new System.Drawing.Size(79, 22);
            this.enableAll_ToolStripButton.Text = "Enable All";
            this.enableAll_ToolStripButton.Click += new System.EventHandler(this.enableAll_ToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // quickEntry_ToolStripButton
            // 
            this.quickEntry_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("quickEntry_ToolStripButton.Image")));
            this.quickEntry_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.quickEntry_ToolStripButton.Name = "quickEntry_ToolStripButton";
            this.quickEntry_ToolStripButton.Size = new System.Drawing.Size(88, 22);
            this.quickEntry_ToolStripButton.Text = "Quick Entry";
            this.quickEntry_ToolStripButton.Click += new System.EventHandler(this.quickEntry_ToolStripButton_Click);
            // 
            // asset_GridView
            // 
            this.asset_GridView.BackColor = System.Drawing.SystemColors.Control;
            this.asset_GridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.asset_GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.asset_GridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.asset_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.asset_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.asset_GridView.Location = new System.Drawing.Point(0, 25);
            // 
            // 
            // 
            this.asset_GridView.MasterTemplate.AllowAddNewRow = false;
            this.asset_GridView.MasterTemplate.AllowColumnResize = false;
            this.asset_GridView.MasterTemplate.AllowDeleteRow = false;
            this.asset_GridView.MasterTemplate.AllowRowReorder = true;
            this.asset_GridView.MasterTemplate.AllowRowResize = false;
            this.asset_GridView.MasterTemplate.AutoGenerateColumns = false;
            this.asset_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewCheckBoxColumn1.EnableExpressionEditor = false;
            gridViewCheckBoxColumn1.FieldName = "Enabled";
            gridViewCheckBoxColumn1.HeaderText = "Enabled";
            gridViewCheckBoxColumn1.MaxWidth = 75;
            gridViewCheckBoxColumn1.MinWidth = 75;
            gridViewCheckBoxColumn1.Name = "enabled_GridViewColumn";
            gridViewCheckBoxColumn1.Width = 75;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "AssetId";
            gridViewTextBoxColumn1.HeaderText = "Name";
            gridViewTextBoxColumn1.MaxWidth = 100;
            gridViewTextBoxColumn1.MinWidth = 100;
            gridViewTextBoxColumn1.Name = "assetId_GridViewColumn";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 100;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "AssetType";
            gridViewTextBoxColumn2.HeaderText = "Type";
            gridViewTextBoxColumn2.MaxWidth = 100;
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "assetType_GridViewColumn";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 100;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Description";
            gridViewTextBoxColumn3.HeaderText = "Description";
            gridViewTextBoxColumn3.Name = "description_GridViewColumn";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.Width = 319;
            gridViewTextBoxColumn4.FieldName = "ReservationKey";
            gridViewTextBoxColumn4.HeaderText = "Reservation Key";
            gridViewTextBoxColumn4.MinWidth = 70;
            gridViewTextBoxColumn4.Name = "reservationKey_GridViewColumn";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.Width = 101;
            this.asset_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1,
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.asset_GridView.MasterTemplate.MultiSelect = true;
            this.asset_GridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.asset_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.asset_GridView.Name = "asset_GridView";
            this.asset_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.asset_GridView.Size = new System.Drawing.Size(692, 322);
            this.asset_GridView.TabIndex = 1;
            this.asset_GridView.Text = "radGridView1";
            // 
            // enableImages
            // 
            this.enableImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("enableImages.ImageStream")));
            this.enableImages.TransparentColor = System.Drawing.Color.Transparent;
            this.enableImages.Images.SetKeyName(0, "Enable");
            this.enableImages.Images.SetKeyName(1, "Disable");
            // 
            // AssetSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.asset_GridView);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AssetSelectionControl";
            this.Size = new System.Drawing.Size(692, 347);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.asset_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.asset_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton add_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
        private System.Windows.Forms.ToolStripButton enableAll_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView asset_GridView;
        private System.Windows.Forms.ImageList enableImages;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton quickEntry_ToolStripButton;
    }
}
