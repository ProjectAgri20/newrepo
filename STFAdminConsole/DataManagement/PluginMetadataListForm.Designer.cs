namespace HP.ScalableTest.LabConsole
{
    partial class PluginMetadataListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginMetadataListForm));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.metadataTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addPlugin_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.delete_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.apply_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.plugin_RadGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.metadataTypeBindingSource)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugin_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plugin_RadGridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // metadataTypeBindingSource
            // 
            this.metadataTypeBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.MetadataType);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPlugin_ToolStripButton,
            this.edit_ToolStripButton,
            this.delete_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(798, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addPlugin_ToolStripButton
            // 
            this.addPlugin_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addPlugin_ToolStripButton.Image")));
            this.addPlugin_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPlugin_ToolStripButton.Name = "addPlugin_ToolStripButton";
            this.addPlugin_ToolStripButton.Size = new System.Drawing.Size(63, 24);
            this.addPlugin_ToolStripButton.Text = "New";
            this.addPlugin_ToolStripButton.Click += new System.EventHandler(this.addPlugin_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(59, 24);
            this.edit_ToolStripButton.Text = "Edit";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.edit_ToolStripButton_Click);
            // 
            // delete_ToolStripButton
            // 
            this.delete_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delete_ToolStripButton.Image")));
            this.delete_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_ToolStripButton.Name = "delete_ToolStripButton";
            this.delete_ToolStripButton.Size = new System.Drawing.Size(87, 24);
            this.delete_ToolStripButton.Text = "Remove";
            this.delete_ToolStripButton.Click += new System.EventHandler(this.delete_ToolStripButton_Click);
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(685, 496);
            this.apply_Button.Margin = new System.Windows.Forms.Padding(4);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(100, 32);
            this.apply_Button.TabIndex = 2;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(577, 496);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(469, 496);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 4;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // plugin_RadGridView
            // 
            this.plugin_RadGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plugin_RadGridView.Location = new System.Drawing.Point(0, 30);
            // 
            // 
            // 
            gridViewTextBoxColumn1.FieldName = "Name";
            gridViewTextBoxColumn1.HeaderText = "Name";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.Name = "Name";
            gridViewTextBoxColumn2.FieldName = "Title";
            gridViewTextBoxColumn2.HeaderText = "Title";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.Name = "Title";
            gridViewTextBoxColumn3.FieldName = "Group";
            gridViewTextBoxColumn3.HeaderText = "Group";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.Name = "Group";
            gridViewTextBoxColumn4.FieldName = "ResourceTypesString";
            gridViewTextBoxColumn4.HeaderText = "Applicable Workers";
            gridViewTextBoxColumn4.IsAutoGenerated = true;
            gridViewTextBoxColumn4.Name = "ResourceTypesString";
            gridViewTextBoxColumn4.ReadOnly = true;
            this.plugin_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.plugin_RadGridView.MasterTemplate.DataSource = this.metadataTypeBindingSource;
            this.plugin_RadGridView.Name = "plugin_RadGridView";
            this.plugin_RadGridView.Size = new System.Drawing.Size(798, 459);
            this.plugin_RadGridView.TabIndex = 5;
            this.plugin_RadGridView.Text = "radGridView1";
            this.plugin_RadGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.plugin_RadGridView_CellDoubleClick);
            // 
            // PluginMetadataListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 541);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.plugin_RadGridView);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.apply_Button);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PluginMetadataListForm";
            this.Text = "Plugin Reference Management";
            this.Load += new System.EventHandler(this.PluginMetadataManagementForm_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PluginMetadataListForm_HelpRequested);
            ((System.ComponentModel.ISupportInitialize)(this.metadataTypeBindingSource)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugin_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plugin_RadGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addPlugin_ToolStripButton;
        private System.Windows.Forms.BindingSource metadataTypeBindingSource;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton delete_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView plugin_RadGridView;
    }
}