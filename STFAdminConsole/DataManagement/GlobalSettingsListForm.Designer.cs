namespace HP.ScalableTest.LabConsole
{
    partial class GlobalSettingsListForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalSettingsListForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.settings_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.settingContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addPlugin_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.delete_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.reloadToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.apply_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.viewAll_CheckBox = new System.Windows.Forms.CheckBox();
            this.systemSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.userBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView.MasterTemplate)).BeginInit();
            this.settingContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.systemSettingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.viewAll_CheckBox);
            this.panel1.Controls.Add(this.settings_RadGridView);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1070, 561);
            this.panel1.TabIndex = 0;
            // 
            // settings_RadGridView
            // 
            this.settings_RadGridView.ContextMenuStrip = this.settingContextMenuStrip;
            this.settings_RadGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settings_RadGridView.Location = new System.Drawing.Point(0, 27);
            // 
            // 
            // 
            this.settings_RadGridView.MasterTemplate.AllowAddNewRow = false;
            this.settings_RadGridView.MasterTemplate.AllowColumnReorder = false;
            this.settings_RadGridView.MasterTemplate.AllowDeleteRow = false;
            this.settings_RadGridView.MasterTemplate.AllowEditRow = false;
            this.settings_RadGridView.MasterTemplate.AllowRowResize = false;
            this.settings_RadGridView.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn6.FieldName = "Type";
            gridViewTextBoxColumn6.HeaderText = "Type";
            gridViewTextBoxColumn6.IsAutoGenerated = true;
            gridViewTextBoxColumn6.IsVisible = false;
            gridViewTextBoxColumn6.Name = "type_Column";
            gridViewTextBoxColumn7.FieldName = "SubType";
            gridViewTextBoxColumn7.HeaderText = "SubType";
            gridViewTextBoxColumn7.Name = "subType_Column";
            gridViewTextBoxColumn8.FieldName = "Name";
            gridViewTextBoxColumn8.HeaderText = "Name";
            gridViewTextBoxColumn8.IsAutoGenerated = true;
            gridViewTextBoxColumn8.Name = "name_Column";
            gridViewTextBoxColumn9.FieldName = "Value";
            gridViewTextBoxColumn9.HeaderText = "Value";
            gridViewTextBoxColumn9.IsAutoGenerated = true;
            gridViewTextBoxColumn9.Name = "value_Column";
            gridViewTextBoxColumn10.FieldName = "Description";
            gridViewTextBoxColumn10.HeaderText = "Description";
            gridViewTextBoxColumn10.IsAutoGenerated = true;
            gridViewTextBoxColumn10.Name = "description_Column";
            this.settings_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10});
            this.settings_RadGridView.MasterTemplate.DataSource = this.systemSettingBindingSource;
            this.settings_RadGridView.MasterTemplate.EnableFiltering = true;
            this.settings_RadGridView.Name = "settings_RadGridView";
            this.settings_RadGridView.Size = new System.Drawing.Size(1070, 534);
            this.settings_RadGridView.TabIndex = 2;
            this.settings_RadGridView.Text = "radGridView1";
            this.settings_RadGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.settings_RadGridView_CellDoubleClick);
            // 
            // settingContextMenuStrip
            // 
            this.settingContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.settingContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.settingContextMenuStrip.Name = "settingContextMenuStrip";
            this.settingContextMenuStrip.Size = new System.Drawing.Size(122, 56);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripMenuItem.Image")));
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripMenuItem.Image")));
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPlugin_ToolStripButton,
            this.edit_ToolStripButton,
            this.delete_ToolStripButton,
            this.reloadToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(6, 0, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addPlugin_ToolStripButton
            // 
            this.addPlugin_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addPlugin_ToolStripButton.Image")));
            this.addPlugin_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPlugin_ToolStripButton.Name = "addPlugin_ToolStripButton";
            this.addPlugin_ToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.addPlugin_ToolStripButton.Text = "Add";
            this.addPlugin_ToolStripButton.Click += new System.EventHandler(this.add_ToolStripButton_Click);
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
            // delete_ToolStripButton
            // 
            this.delete_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delete_ToolStripButton.Image")));
            this.delete_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_ToolStripButton.Name = "delete_ToolStripButton";
            this.delete_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.delete_ToolStripButton.Text = "Remove";
            this.delete_ToolStripButton.Click += new System.EventHandler(this.delete_ToolStripButton_Click);
            // 
            // reloadToolStripButton
            // 
            this.reloadToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("reloadToolStripButton.Image")));
            this.reloadToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reloadToolStripButton.Name = "reloadToolStripButton";
            this.reloadToolStripButton.Size = new System.Drawing.Size(67, 24);
            this.reloadToolStripButton.Text = "Reload";
            this.reloadToolStripButton.Click += new System.EventHandler(this.reloadToolStripButton_Click);
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(944, 570);
            this.apply_Button.Margin = new System.Windows.Forms.Padding(4);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(112, 36);
            this.apply_Button.TabIndex = 2;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(822, 570);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(112, 36);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(701, 570);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(112, 36);
            this.ok_Button.TabIndex = 4;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // viewAll_CheckBox
            // 
            this.viewAll_CheckBox.AutoSize = true;
            this.viewAll_CheckBox.Location = new System.Drawing.Point(340, 3);
            this.viewAll_CheckBox.Name = "viewAll_CheckBox";
            this.viewAll_CheckBox.Size = new System.Drawing.Size(126, 19);
            this.viewAll_CheckBox.TabIndex = 3;
            this.viewAll_CheckBox.Text = "View Type Column";
            this.viewAll_CheckBox.UseVisualStyleBackColor = true;
            this.viewAll_CheckBox.CheckedChanged += new System.EventHandler(this.viewAll_CheckBox_CheckedChanged);
            // 
            // systemSettingBindingSource
            // 
            this.systemSettingBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.SystemSetting);
            // 
            // userBindingSource
            // 
            this.userBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.User);
            // 
            // GlobalSettingsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 620);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GlobalSettingsListForm";
            this.Text = "Settings Management";
            this.Load += new System.EventHandler(this.GlobalSettingsListForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settings_RadGridView)).EndInit();
            this.settingContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.systemSettingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addPlugin_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton delete_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView settings_RadGridView;
        private System.Windows.Forms.BindingSource userBindingSource;
        private System.Windows.Forms.BindingSource systemSettingBindingSource;
        private System.Windows.Forms.ContextMenuStrip settingContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton reloadToolStripButton;
        private System.Windows.Forms.CheckBox viewAll_CheckBox;
    }
}