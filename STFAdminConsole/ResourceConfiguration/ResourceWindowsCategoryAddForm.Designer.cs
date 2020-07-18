namespace HP.ScalableTest.LabConsole.ResourceConfiguration
{
    partial class ResourceWindowsCategoryAddForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceWindowsCategoryAddForm));
            this.children_GridView = new System.Windows.Forms.DataGridView();
            this.servicesCheckBox_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.name_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.associatedResource_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.addAssociated_Button = new System.Windows.Forms.ToolStripButton();
            this.removeAssociated_Button = new System.Windows.Forms.ToolStripButton();
            this.groupBox_Assoc = new System.Windows.Forms.GroupBox();
            this.ok_Button = new System.Windows.Forms.Button();
            this.tabControl_Types = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.resource_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.addResource_Button = new System.Windows.Forms.ToolStripButton();
            this.removeResource_Button = new System.Windows.Forms.ToolStripButton();
            this.listBox_Resource = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.children_GridView)).BeginInit();
            this.associatedResource_ToolStrip.SuspendLayout();
            this.groupBox_Assoc.SuspendLayout();
            this.tabControl_Types.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.resource_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // children_GridView
            // 
            this.children_GridView.AllowUserToAddRows = false;
            this.children_GridView.AllowUserToDeleteRows = false;
            this.children_GridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.AliceBlue;
            this.children_GridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.children_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.children_GridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.children_GridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.children_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.children_GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.servicesCheckBox_Column,
            this.name_Column});
            this.children_GridView.EnableHeadersVisualStyles = false;
            this.children_GridView.Location = new System.Drawing.Point(12, 69);
            this.children_GridView.Name = "children_GridView";
            this.children_GridView.RowHeadersVisible = false;
            this.children_GridView.RowTemplate.Height = 24;
            this.children_GridView.Size = new System.Drawing.Size(814, 162);
            this.children_GridView.TabIndex = 4;
            // 
            // servicesCheckBox_Column
            // 
            this.servicesCheckBox_Column.FalseValue = "false";
            this.servicesCheckBox_Column.HeaderText = "Select For Removal";
            this.servicesCheckBox_Column.Name = "servicesCheckBox_Column";
            this.servicesCheckBox_Column.TrueValue = "true";
            // 
            // name_Column
            // 
            this.name_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name_Column.DataPropertyName = "Name";
            this.name_Column.HeaderText = "Name";
            this.name_Column.Name = "name_Column";
            this.name_Column.ReadOnly = true;
            // 
            // associatedResource_ToolStrip
            // 
            this.associatedResource_ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.associatedResource_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.associatedResource_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAssociated_Button,
            this.removeAssociated_Button});
            this.associatedResource_ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.associatedResource_ToolStrip.Location = new System.Drawing.Point(12, 36);
            this.associatedResource_ToolStrip.Name = "associatedResource_ToolStrip";
            this.associatedResource_ToolStrip.Size = new System.Drawing.Size(212, 27);
            this.associatedResource_ToolStrip.TabIndex = 5;
            this.associatedResource_ToolStrip.Text = "Associated Resources";
            // 
            // addAssociated_Button
            // 
            this.addAssociated_Button.Image = ((System.Drawing.Image)(resources.GetObject("addAssociated_Button.Image")));
            this.addAssociated_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addAssociated_Button.Name = "addAssociated_Button";
            this.addAssociated_Button.Size = new System.Drawing.Size(83, 24);
            this.addAssociated_Button.Text = "Add New ";
            this.addAssociated_Button.ToolTipText = "Add a New Service";
            this.addAssociated_Button.Click += new System.EventHandler(this.addAssociated_Button_Click);
            // 
            // removeAssociated_Button
            // 
            this.removeAssociated_Button.Image = ((System.Drawing.Image)(resources.GetObject("removeAssociated_Button.Image")));
            this.removeAssociated_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeAssociated_Button.Name = "removeAssociated_Button";
            this.removeAssociated_Button.Size = new System.Drawing.Size(117, 24);
            this.removeAssociated_Button.Text = "Remove Existing";
            this.removeAssociated_Button.ToolTipText = "Remove selected service";
            this.removeAssociated_Button.Click += new System.EventHandler(this.removeAssociated_Button_Click);
            // 
            // groupBox_Assoc
            // 
            this.groupBox_Assoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Assoc.Controls.Add(this.associatedResource_ToolStrip);
            this.groupBox_Assoc.Controls.Add(this.children_GridView);
            this.groupBox_Assoc.Location = new System.Drawing.Point(9, 153);
            this.groupBox_Assoc.Name = "groupBox_Assoc";
            this.groupBox_Assoc.Size = new System.Drawing.Size(832, 237);
            this.groupBox_Assoc.TabIndex = 7;
            this.groupBox_Assoc.TabStop = false;
            this.groupBox_Assoc.Text = "Associated Resources/Services";
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(761, 452);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(93, 28);
            this.ok_Button.TabIndex = 9;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // tabControl_Types
            // 
            this.tabControl_Types.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_Types.Controls.Add(this.tabPage1);
            this.tabControl_Types.Location = new System.Drawing.Point(3, 12);
            this.tabControl_Types.Name = "tabControl_Types";
            this.tabControl_Types.SelectedIndex = 0;
            this.tabControl_Types.Size = new System.Drawing.Size(855, 428);
            this.tabControl_Types.TabIndex = 10;
            this.tabControl_Types.SelectedIndexChanged += new System.EventHandler(this.tabControl_Types_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.resource_ToolStrip);
            this.tabPage1.Controls.Add(this.listBox_Resource);
            this.tabPage1.Controls.Add(this.groupBox_Assoc);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(847, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Resource Type";
            // 
            // resource_ToolStrip
            // 
            this.resource_ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.resource_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.resource_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addResource_Button,
            this.removeResource_Button});
            this.resource_ToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.resource_ToolStrip.Location = new System.Drawing.Point(9, 11);
            this.resource_ToolStrip.Name = "resource_ToolStrip";
            this.resource_ToolStrip.Size = new System.Drawing.Size(200, 27);
            this.resource_ToolStrip.TabIndex = 10;
            this.resource_ToolStrip.Text = "Associated Resources";
            // 
            // addResource_Button
            // 
            this.addResource_Button.Image = ((System.Drawing.Image)(resources.GetObject("addResource_Button.Image")));
            this.addResource_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addResource_Button.Name = "addResource_Button";
            this.addResource_Button.Size = new System.Drawing.Size(83, 24);
            this.addResource_Button.Text = "Add New ";
            this.addResource_Button.ToolTipText = "Add a New Service";
            this.addResource_Button.Click += new System.EventHandler(this.addResource_Button_Click);
            // 
            // removeResource_Button
            // 
            this.removeResource_Button.Image = ((System.Drawing.Image)(resources.GetObject("removeResource_Button.Image")));
            this.removeResource_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeResource_Button.Name = "removeResource_Button";
            this.removeResource_Button.Size = new System.Drawing.Size(74, 24);
            this.removeResource_Button.Text = "Remove";
            this.removeResource_Button.ToolTipText = "Remove selected service";
            this.removeResource_Button.Click += new System.EventHandler(this.removeResource_Button_Click);
            // 
            // listBox_Resource
            // 
            this.listBox_Resource.DisplayMember = "Name";
            this.listBox_Resource.FormattingEnabled = true;
            this.listBox_Resource.ItemHeight = 16;
            this.listBox_Resource.Location = new System.Drawing.Point(9, 41);
            this.listBox_Resource.Name = "listBox_Resource";
            this.listBox_Resource.Size = new System.Drawing.Size(249, 100);
            this.listBox_Resource.TabIndex = 0;
            this.listBox_Resource.ValueMember = "CategoryId";
            this.listBox_Resource.SelectedValueChanged += new System.EventHandler(this.listBox_Resource_SelectedValueChanged);
            // 
            // ResourceWindowsCategoryAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 492);
            this.Controls.Add(this.tabControl_Types);
            this.Controls.Add(this.ok_Button);
            this.Name = "ResourceWindowsCategoryAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Resource Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.children_GridView)).EndInit();
            this.associatedResource_ToolStrip.ResumeLayout(false);
            this.associatedResource_ToolStrip.PerformLayout();
            this.groupBox_Assoc.ResumeLayout(false);
            this.groupBox_Assoc.PerformLayout();
            this.tabControl_Types.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.resource_ToolStrip.ResumeLayout(false);
            this.resource_ToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView children_GridView;
        private System.Windows.Forms.ToolStrip associatedResource_ToolStrip;
        private System.Windows.Forms.ToolStripButton addAssociated_Button;
        private System.Windows.Forms.ToolStripButton removeAssociated_Button;
        private System.Windows.Forms.GroupBox groupBox_Assoc;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.DataGridViewCheckBoxColumn servicesCheckBox_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_Column;
        private System.Windows.Forms.TabControl tabControl_Types;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBox_Resource;
        private System.Windows.Forms.ToolStrip resource_ToolStrip;
        private System.Windows.Forms.ToolStripButton addResource_Button;
        private System.Windows.Forms.ToolStripButton removeResource_Button;
    }
}