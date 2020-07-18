namespace HP.ScalableTest.LabConsole
{
    partial class ServerSettingsListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerSettingsListForm));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.add_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.settings_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.ok_Button = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_ToolStripButton,
            this.edit_ToolStripButton,
            this.remove_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(6, 0, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(768, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip_Main";
            // 
            // add_ToolStripButton
            // 
            this.add_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("add_ToolStripButton.Image")));
            this.add_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_ToolStripButton.Name = "add_ToolStripButton";
            this.add_ToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.add_ToolStripButton.Text = "Add";
            this.add_ToolStripButton.Click += new System.EventHandler(this.add_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(51, 24);
            this.edit_ToolStripButton.Text = "Edit";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.edit_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.remove_ToolStripButton.Text = "Remove";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.remove_ToolStripButton_Click);
            // 
            // settings_RadGridView
            // 
            this.settings_RadGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settings_RadGridView.Location = new System.Drawing.Point(0, 30);
            // 
            // 
            // 
            this.settings_RadGridView.MasterTemplate.AllowAddNewRow = false;
            this.settings_RadGridView.MasterTemplate.AllowColumnReorder = false;
            this.settings_RadGridView.MasterTemplate.AllowDeleteRow = false;
            this.settings_RadGridView.MasterTemplate.AllowEditRow = false;
            this.settings_RadGridView.MasterTemplate.AllowRowResize = false;
            this.settings_RadGridView.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn5.FieldName = "ServerHostName";
            gridViewTextBoxColumn5.HeaderText = "ServerName";
            gridViewTextBoxColumn5.IsAutoGenerated = true;
            gridViewTextBoxColumn5.Name = "server_Column";
            gridViewTextBoxColumn5.Width = 100;
            gridViewTextBoxColumn6.FieldName = "Name";
            gridViewTextBoxColumn6.HeaderText = "Name";
            gridViewTextBoxColumn6.IsAutoGenerated = true;
            gridViewTextBoxColumn6.Name = "name_Column";
            gridViewTextBoxColumn7.FieldName = "Value";
            gridViewTextBoxColumn7.HeaderText = "Value";
            gridViewTextBoxColumn7.IsAutoGenerated = true;
            gridViewTextBoxColumn7.Name = "value_Column";
            gridViewTextBoxColumn8.FieldName = "Description";
            gridViewTextBoxColumn8.HeaderText = "Description";
            gridViewTextBoxColumn8.IsAutoGenerated = true;
            gridViewTextBoxColumn8.Name = "description_Column";
            gridViewTextBoxColumn8.Width = 100;
            this.settings_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8});
            this.settings_RadGridView.MasterTemplate.EnableFiltering = true;
            this.settings_RadGridView.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.settings_RadGridView.Name = "settings_RadGridView";
            this.settings_RadGridView.Size = new System.Drawing.Size(768, 262);
            this.settings_RadGridView.TabIndex = 3;
            this.settings_RadGridView.Text = "radGridView1";
            this.settings_RadGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.settings_RadGridView_CellDoubleClick);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ok_Button.Location = new System.Drawing.Point(653, 299);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(112, 36);
            this.ok_Button.TabIndex = 5;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // ServerSettingsListForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ok_Button;
            this.ClientSize = new System.Drawing.Size(768, 338);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.settings_RadGridView);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ServerSettingsListForm";
            this.Text = "Server Settings";
            this.Load += new System.EventHandler(this.ServerSettingsListForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton add_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView settings_RadGridView;
        private System.Windows.Forms.Button ok_Button;
    }
}