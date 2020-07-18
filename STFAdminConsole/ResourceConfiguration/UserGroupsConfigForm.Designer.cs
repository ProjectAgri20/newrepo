namespace HP.ScalableTest.LabConsole
{
    partial class UserGroupsConfigForm
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
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }

            if (disposing && _resetEvent != null)
            {
                _resetEvent.Dispose();
                _resetEvent = null;
            }

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
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn1 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn2 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserGroupsConfigForm));
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.enumBinder1 = new Telerik.WinControls.UI.Data.EnumBinder();
            this.enumBinder2 = new Telerik.WinControls.UI.Data.EnumBinder();
            this.ok_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.addGroupToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.user_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.user_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.groups_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.userGroup_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userGroupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cancel_Button = new System.Windows.Forms.Button();
            this.apply_Button = new System.Windows.Forms.Button();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.user_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.user_RadGridView.MasterTemplate)).BeginInit();
            this.user_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).BeginInit();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groups_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groups_RadGridView.MasterTemplate)).BeginInit();
            this.userGroup_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userGroupBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // enumBinder1
            // 
            this.enumBinder1.Source = typeof(HP.ScalableTest.Core.Security.UserRole);
            gridViewComboBoxColumn1.DataSource = this.enumBinder1;
            gridViewComboBoxColumn1.DisplayMember = "Description";
            gridViewComboBoxColumn1.ValueMember = "Value";
            this.enumBinder1.Target = gridViewComboBoxColumn1;
            // 
            // enumBinder2
            // 
            this.enumBinder2.Source = typeof(HP.ScalableTest.Core.Security.UserRole);
            gridViewComboBoxColumn2.DataSource = this.enumBinder2;
            gridViewComboBoxColumn2.DisplayMember = "Description";
            gridViewComboBoxColumn2.ValueMember = "Value";
            this.enumBinder2.Target = gridViewComboBoxColumn2;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(431, 513);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 4;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 382F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.user_RadGridView, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groups_RadGridView, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(759, 505);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.addGroupToolStripButton,
            this.editToolStripButton,
            this.removeToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(377, 27);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(96, 24);
            this.toolStripLabel1.Text = "User Groups";
            // 
            // addGroupToolStripButton
            // 
            this.addGroupToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addGroupToolStripButton.Image")));
            this.addGroupToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addGroupToolStripButton.Name = "addGroupToolStripButton";
            this.addGroupToolStripButton.Size = new System.Drawing.Size(61, 24);
            this.addGroupToolStripButton.Text = "Add";
            this.addGroupToolStripButton.Click += new System.EventHandler(this.addGroupToolStripButton_Click);
            // 
            // editToolStripButton
            // 
            this.editToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripButton.Image")));
            this.editToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editToolStripButton.Name = "editToolStripButton";
            this.editToolStripButton.Size = new System.Drawing.Size(59, 24);
            this.editToolStripButton.Text = "Edit";
            this.editToolStripButton.Click += new System.EventHandler(this.editToolStripButton_Click);
            // 
            // removeToolStripButton
            // 
            this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
            this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeToolStripButton.Name = "removeToolStripButton";
            this.removeToolStripButton.Size = new System.Drawing.Size(87, 24);
            this.removeToolStripButton.Text = "Remove";
            this.removeToolStripButton.Click += new System.EventHandler(this.removeToolStripButton_Click);
            // 
            // user_RadGridView
            // 
            this.user_RadGridView.ContextMenuStrip = this.user_contextMenuStrip;
            this.user_RadGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.user_RadGridView.Location = new System.Drawing.Point(380, 30);
            // 
            // 
            // 
            this.user_RadGridView.MasterTemplate.AllowAddNewRow = false;
            this.user_RadGridView.MasterTemplate.AllowCellContextMenu = false;
            this.user_RadGridView.MasterTemplate.AllowColumnChooser = false;
            this.user_RadGridView.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.user_RadGridView.MasterTemplate.AllowColumnReorder = false;
            this.user_RadGridView.MasterTemplate.AllowDeleteRow = false;
            this.user_RadGridView.MasterTemplate.AllowDragToGroup = false;
            this.user_RadGridView.MasterTemplate.AllowRowResize = false;
            this.user_RadGridView.MasterTemplate.AutoGenerateColumns = false;
            gridViewCheckBoxColumn1.FieldName = "Selected";
            gridViewCheckBoxColumn1.HeaderText = "";
            gridViewCheckBoxColumn1.Name = "userSelectedColumn";
            gridViewTextBoxColumn1.FieldName = "UserName";
            gridViewTextBoxColumn1.HeaderText = "Name";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.Name = "userNameColumn";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn2.FieldName = "Domain";
            gridViewTextBoxColumn2.HeaderText = "Domain";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.Name = "Domain";
            gridViewTextBoxColumn2.ReadOnly = true;
            this.user_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1,
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2});
            this.user_RadGridView.MasterTemplate.DataSource = this.userBindingSource;
            this.user_RadGridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.user_RadGridView.MasterTemplate.EnableGrouping = false;
            this.user_RadGridView.MasterTemplate.MultiSelect = true;
            this.user_RadGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.user_RadGridView.Name = "user_RadGridView";
            this.user_RadGridView.ShowGroupPanel = false;
            this.user_RadGridView.Size = new System.Drawing.Size(376, 472);
            this.user_RadGridView.TabIndex = 18;
            this.user_RadGridView.Text = "radGridView1";
            this.user_RadGridView.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.user_RadGridView_CellClick);
            // 
            // user_contextMenuStrip
            // 
            this.user_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.user_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.user_contextMenuStrip.Name = "virtualMachine_contextMenuStrip";
            this.user_contextMenuStrip.Size = new System.Drawing.Size(262, 56);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("selectAllToolStripMenuItem.Image")));
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.selectAllToolStripMenuItem.Text = "Check All Selected Rows";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uncheckAllToolStripMenuItem.Image")));
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(261, 26);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck All Selected Rows";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
            // 
            // userBindingSource
            // 
            this.userBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.User);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2});
            this.toolStrip2.Location = new System.Drawing.Point(377, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(382, 25);
            this.toolStrip2.TabIndex = 16;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(145, 22);
            this.toolStripLabel2.Text = "Group Membership";
            // 
            // groups_RadGridView
            // 
            this.groups_RadGridView.ContextMenuStrip = this.userGroup_ContextMenuStrip;
            this.groups_RadGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groups_RadGridView.Location = new System.Drawing.Point(3, 30);
            // 
            // 
            // 
            this.groups_RadGridView.MasterTemplate.AllowAddNewRow = false;
            this.groups_RadGridView.MasterTemplate.AllowCellContextMenu = false;
            this.groups_RadGridView.MasterTemplate.AllowColumnChooser = false;
            this.groups_RadGridView.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.groups_RadGridView.MasterTemplate.AllowColumnReorder = false;
            this.groups_RadGridView.MasterTemplate.AllowDeleteRow = false;
            this.groups_RadGridView.MasterTemplate.AllowDragToGroup = false;
            this.groups_RadGridView.MasterTemplate.AllowEditRow = false;
            this.groups_RadGridView.MasterTemplate.AllowRowResize = false;
            this.groups_RadGridView.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn3.FieldName = "GroupName";
            gridViewTextBoxColumn3.HeaderText = "Name";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.Name = "GroupName";
            gridViewTextBoxColumn4.FieldName = "Description";
            gridViewTextBoxColumn4.HeaderText = "Description";
            gridViewTextBoxColumn4.IsAutoGenerated = true;
            gridViewTextBoxColumn4.Name = "Description";
            this.groups_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.groups_RadGridView.MasterTemplate.DataSource = this.userGroupBindingSource;
            this.groups_RadGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.groups_RadGridView.Name = "groups_RadGridView";
            this.groups_RadGridView.ShowGroupPanel = false;
            this.groups_RadGridView.ShowRowErrors = false;
            this.groups_RadGridView.Size = new System.Drawing.Size(371, 472);
            this.groups_RadGridView.TabIndex = 17;
            this.groups_RadGridView.Text = "radGridView1";
            this.groups_RadGridView.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.groups_RadGridView_UserAddedRow);
            this.groups_RadGridView.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.groups_RadGridView_CellClick);
            // 
            // userGroup_ContextMenuStrip
            // 
            this.userGroup_ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.userGroup_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.userGroup_ContextMenuStrip.Name = "platform_ContextMenuStrip";
            this.userGroup_ContextMenuStrip.Size = new System.Drawing.Size(182, 84);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // userGroupBindingSource
            // 
            this.userGroupBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.UserGroup);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(539, 513);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 14;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(647, 513);
            this.apply_Button.Margin = new System.Windows.Forms.Padding(4);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(100, 32);
            this.apply_Button.TabIndex = 16;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripMenuItem.Image")));
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // UserGroupsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 558);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(661, 605);
            this.Name = "UserGroupsConfigForm";
            this.Text = "UserGroup Management ";
            this.Load += new System.EventHandler(this.UserGroupsConfigForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.user_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.user_RadGridView)).EndInit();
            this.user_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groups_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groups_RadGridView)).EndInit();
            this.userGroup_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userGroupBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.ContextMenuStrip user_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip userGroup_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private Telerik.WinControls.UI.RadGridView user_RadGridView;
        private Telerik.WinControls.UI.RadGridView groups_RadGridView;
        private System.Windows.Forms.BindingSource userBindingSource;
        private Telerik.WinControls.UI.Data.EnumBinder enumBinder1;
        private Telerik.WinControls.UI.Data.EnumBinder enumBinder2;
        private System.Windows.Forms.BindingSource userGroupBindingSource;
        private System.Windows.Forms.ToolStripButton addGroupToolStripButton;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.ToolStripButton removeToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    }
}