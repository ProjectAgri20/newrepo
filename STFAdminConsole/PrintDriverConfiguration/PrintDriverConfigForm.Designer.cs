namespace HP.ScalableTest.LabConsole
{
    partial class PrintDriverConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintDriverConfigForm));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.close_Button = new System.Windows.Forms.Button();
            this.config_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.add_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.config_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.printDriver_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.printDriverPackageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.config_ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printDriver_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriver_RadGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriverPackageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(739, 335);
            this.close_Button.Margin = new System.Windows.Forms.Padding(4);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(100, 32);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // config_ToolStrip
            // 
            this.config_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.config_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.config_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_ToolStripButton,
            this.remove_ToolStripButton});
            this.config_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.config_ToolStrip.Name = "config_ToolStrip";
            this.config_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.config_ToolStrip.Size = new System.Drawing.Size(852, 27);
            this.config_ToolStrip.TabIndex = 5;
            this.config_ToolStrip.Text = "toolStrip1";
            // 
            // add_ToolStripButton
            // 
            this.add_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("add_ToolStripButton.Image")));
            this.add_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_ToolStripButton.Name = "add_ToolStripButton";
            this.add_ToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.add_ToolStripButton.Text = "Add";
            this.add_ToolStripButton.ToolTipText = "Add a Driver";
            this.add_ToolStripButton.Click += new System.EventHandler(this.add_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.remove_ToolStripButton.Text = "Remove";
            this.remove_ToolStripButton.ToolTipText = "Remove selected Driver";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.remove_ToolStripButton_Click);
            // 
            // config_ImageList
            // 
            this.config_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("config_ImageList.ImageStream")));
            this.config_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.config_ImageList.Images.SetKeyName(0, "cd_add");
            this.config_ImageList.Images.SetKeyName(1, "cd_delete");
            this.config_ImageList.Images.SetKeyName(2, "driver_general");
            this.config_ImageList.Images.SetKeyName(3, "driver_discrete");
            // 
            // printDriver_RadGridView
            // 
            this.printDriver_RadGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printDriver_RadGridView.Location = new System.Drawing.Point(0, 30);
            // 
            // 
            // 
            gridViewTextBoxColumn1.DataType = typeof(System.Guid);
            gridViewTextBoxColumn1.FieldName = "PrintDriverPackageId";
            gridViewTextBoxColumn1.HeaderText = "PrintDriverPackageId";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.Name = "PrintDriverPackageId";
            gridViewTextBoxColumn2.FieldName = "Name";
            gridViewTextBoxColumn2.HeaderText = "Name";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.Name = "Name";
            gridViewTextBoxColumn3.FieldName = "InfX86";
            gridViewTextBoxColumn3.HeaderText = "InfX86";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.Name = "InfX86";
            gridViewTextBoxColumn4.FieldName = "InfX64";
            gridViewTextBoxColumn4.HeaderText = "InfX64";
            gridViewTextBoxColumn4.IsAutoGenerated = true;
            gridViewTextBoxColumn4.Name = "InfX64";
            gridViewTextBoxColumn5.DataType = typeof(System.Collections.Generic.ICollection<HP.ScalableTest.Core.AssetInventory.PrintDriver>);
            gridViewTextBoxColumn5.FieldName = "PrintDrivers";
            gridViewTextBoxColumn5.HeaderText = "PrintDrivers";
            gridViewTextBoxColumn5.IsAutoGenerated = true;
            gridViewTextBoxColumn5.IsVisible = false;
            gridViewTextBoxColumn5.Name = "PrintDrivers";
            this.printDriver_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.printDriver_RadGridView.MasterTemplate.DataSource = this.printDriverPackageBindingSource;
            this.printDriver_RadGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.printDriver_RadGridView.Name = "printDriver_RadGridView";
            this.printDriver_RadGridView.Size = new System.Drawing.Size(852, 298);
            this.printDriver_RadGridView.TabIndex = 6;
            this.printDriver_RadGridView.Text = "radGridView1";
            // 
            // printDriverPackageBindingSource
            // 
            this.printDriverPackageBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.PrintDriverPackage);
            // 
            // PrintDriverConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(852, 380);
            this.Controls.Add(this.printDriver_RadGridView);
            this.Controls.Add(this.config_ToolStrip);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintDriverConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Driver Repository";
            this.Load += new System.EventHandler(this.PrintDriverConfigForm_Load);
            this.config_ToolStrip.ResumeLayout(false);
            this.config_ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printDriver_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriver_RadGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriverPackageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.ToolStrip config_ToolStrip;
        private System.Windows.Forms.ImageList config_ImageList;
        private System.Windows.Forms.ToolStripButton add_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView printDriver_RadGridView;
        private System.Windows.Forms.BindingSource printDriverPackageBindingSource;
    }
}