namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class AdminWorkerControl
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminWorkerControl));
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn2 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor2 = new Telerik.WinControls.Data.SortDescriptor();
            this.activity_GridViewTemplate = new Telerik.WinControls.UI.MasterGridViewTemplate();
            this.testcaseid_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.activity_GroupBox = new System.Windows.Forms.GroupBox();
            this.activity_TabControl = new System.Windows.Forms.TabControl();
            this.main_TabPage = new System.Windows.Forms.TabPage();
            this.activity_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.newActivity_ToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.deleteActivity_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyActivity_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.reorder_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.enableAll_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.activity_GridView = new Telerik.WinControls.UI.RadGridView();
            this.setup_TabPage = new System.Windows.Forms.TabPage();
            this.teardown_TabPage = new System.Windows.Forms.TabPage();
            this.platform_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.name_Label = new System.Windows.Forms.Label();
            this.platform_ComboBox = new System.Windows.Forms.ComboBox();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.activity_GridViewTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testcaseid_numericUpDown)).BeginInit();
            this.activity_GroupBox.SuspendLayout();
            this.activity_TabControl.SuspendLayout();
            this.main_TabPage.SuspendLayout();
            this.activity_ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activity_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activity_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // activity_GridViewTemplate
            // 
            gridViewTextBoxColumn1.FieldName = "Name";
            gridViewTextBoxColumn1.HeaderText = "Name";
            gridViewTextBoxColumn1.Name = "name_GridViewColumn";
            gridViewTextBoxColumn2.FieldName = "MetadataType";
            gridViewTextBoxColumn2.HeaderText = "Activity Type";
            gridViewTextBoxColumn2.Name = "metadataType_GridViewColumn";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn3.Expression = "IIF(rawOrder_GridViewColumn >= 2000, \'PostProcess\', IIF(rawOrder_GridViewColumn >" +
    "= 1000 , \'PreProcess\', \'Main\') )";
            gridViewTextBoxColumn3.HeaderText = "Phase";
            gridViewTextBoxColumn3.Name = "phase_GridViewColumn";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn4.FieldName = "ExecutionOrder";
            gridViewTextBoxColumn4.HeaderText = "Raw Order";
            gridViewTextBoxColumn4.Name = "rawOrder_GridViewColumn";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.VisibleInColumnChooser = false;
            gridViewDecimalColumn1.DecimalPlaces = 0;
            gridViewDecimalColumn1.Expression = "rawOrder_GridViewColumn % 1000";
            gridViewDecimalColumn1.HeaderText = "Order";
            gridViewDecimalColumn1.Name = "order_GridViewColumn";
            gridViewDecimalColumn1.ReadOnly = true;
            this.activity_GridViewTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewDecimalColumn1});
            // 
            // testcaseid_numericUpDown
            // 
            this.testcaseid_numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.testcaseid_numericUpDown.Location = new System.Drawing.Point(557, 43);
            this.testcaseid_numericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.testcaseid_numericUpDown.Name = "testcaseid_numericUpDown";
            this.testcaseid_numericUpDown.Size = new System.Drawing.Size(130, 23);
            this.testcaseid_numericUpDown.TabIndex = 109;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(484, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 108;
            this.label1.Text = "TestCase Id";
            // 
            // activity_GroupBox
            // 
            this.activity_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activity_GroupBox.Controls.Add(this.activity_TabControl);
            this.activity_GroupBox.Location = new System.Drawing.Point(9, 121);
            this.activity_GroupBox.Name = "activity_GroupBox";
            this.activity_GroupBox.Size = new System.Drawing.Size(678, 411);
            this.activity_GroupBox.TabIndex = 107;
            this.activity_GroupBox.TabStop = false;
            this.activity_GroupBox.Text = "Worker Activity Plugin Configuration";
            // 
            // activity_TabControl
            // 
            this.activity_TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activity_TabControl.Controls.Add(this.main_TabPage);
            this.activity_TabControl.Controls.Add(this.setup_TabPage);
            this.activity_TabControl.Controls.Add(this.teardown_TabPage);
            this.activity_TabControl.Location = new System.Drawing.Point(6, 22);
            this.activity_TabControl.Name = "activity_TabControl";
            this.activity_TabControl.SelectedIndex = 0;
            this.activity_TabControl.Size = new System.Drawing.Size(666, 383);
            this.activity_TabControl.TabIndex = 106;
            this.activity_TabControl.SelectedIndexChanged += new System.EventHandler(this.activity_TabControl_SelectedIndexChanged);
            // 
            // main_TabPage
            // 
            this.main_TabPage.Controls.Add(this.activity_ToolStrip);
            this.main_TabPage.Controls.Add(this.activity_GridView);
            this.main_TabPage.Location = new System.Drawing.Point(4, 24);
            this.main_TabPage.Name = "main_TabPage";
            this.main_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.main_TabPage.Size = new System.Drawing.Size(658, 355);
            this.main_TabPage.TabIndex = 0;
            this.main_TabPage.Tag = "Main";
            this.main_TabPage.Text = "Main Workflow";
            this.main_TabPage.UseVisualStyleBackColor = true;
            // 
            // activity_ToolStrip
            // 
            this.activity_ToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activity_ToolStrip.AutoSize = false;
            this.activity_ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.activity_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.activity_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newActivity_ToolStripDropDownButton,
            this.deleteActivity_ToolStripButton,
            this.toolStripSeparator1,
            this.copyActivity_ToolStripButton,
            this.reorder_ToolStripButton,
            this.enableAll_ToolStripButton});
            this.activity_ToolStrip.Location = new System.Drawing.Point(0, 3);
            this.activity_ToolStrip.Name = "activity_ToolStrip";
            this.activity_ToolStrip.Size = new System.Drawing.Size(654, 25);
            this.activity_ToolStrip.TabIndex = 72;
            this.activity_ToolStrip.Text = "toolStrip1";
            // 
            // newActivity_ToolStripDropDownButton
            // 
            this.newActivity_ToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("newActivity_ToolStripDropDownButton.Image")));
            this.newActivity_ToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newActivity_ToolStripDropDownButton.Name = "newActivity_ToolStripDropDownButton";
            this.newActivity_ToolStripDropDownButton.Size = new System.Drawing.Size(58, 22);
            this.newActivity_ToolStripDropDownButton.Text = "Add";
            // 
            // deleteActivity_ToolStripButton
            // 
            this.deleteActivity_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteActivity_ToolStripButton.Image")));
            this.deleteActivity_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteActivity_ToolStripButton.Name = "deleteActivity_ToolStripButton";
            this.deleteActivity_ToolStripButton.Size = new System.Drawing.Size(70, 22);
            this.deleteActivity_ToolStripButton.Text = "Remove";
            this.deleteActivity_ToolStripButton.Click += new System.EventHandler(this.deleteActivity_ToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // copyActivity_ToolStripButton
            // 
            this.copyActivity_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyActivity_ToolStripButton.Image")));
            this.copyActivity_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyActivity_ToolStripButton.Name = "copyActivity_ToolStripButton";
            this.copyActivity_ToolStripButton.Size = new System.Drawing.Size(55, 22);
            this.copyActivity_ToolStripButton.Text = "Copy";
            this.copyActivity_ToolStripButton.Click += new System.EventHandler(this.copyActivity_ToolStripButton_Click);
            // 
            // reorder_ToolStripButton
            // 
            this.reorder_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("reorder_ToolStripButton.Image")));
            this.reorder_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reorder_ToolStripButton.Name = "reorder_ToolStripButton";
            this.reorder_ToolStripButton.Size = new System.Drawing.Size(68, 22);
            this.reorder_ToolStripButton.Text = "Reorder";
            this.reorder_ToolStripButton.Click += new System.EventHandler(this.reorder_ToolStripButton_Click);
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
            // activity_GridView
            // 
            this.activity_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activity_GridView.BackColor = System.Drawing.SystemColors.Control;
            this.activity_GridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.activity_GridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.activity_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.activity_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.activity_GridView.Location = new System.Drawing.Point(2, 31);
            // 
            // activity_GridView
            // 
            this.activity_GridView.MasterTemplate.AllowAddNewRow = false;
            this.activity_GridView.MasterTemplate.AllowCellContextMenu = false;
            this.activity_GridView.MasterTemplate.AllowColumnChooser = false;
            this.activity_GridView.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.activity_GridView.MasterTemplate.AllowColumnReorder = false;
            this.activity_GridView.MasterTemplate.AllowDeleteRow = false;
            this.activity_GridView.MasterTemplate.AllowDragToGroup = false;
            this.activity_GridView.MasterTemplate.AllowRowResize = false;
            this.activity_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn5.AllowGroup = false;
            gridViewTextBoxColumn5.AllowHide = false;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "Name";
            gridViewTextBoxColumn5.HeaderText = "Name";
            gridViewTextBoxColumn5.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn5.MinWidth = 85;
            gridViewTextBoxColumn5.Name = "name_GridViewColumn";
            gridViewTextBoxColumn5.ReadOnly = true;
            gridViewTextBoxColumn5.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn5.Width = 313;
            gridViewTextBoxColumn6.AllowHide = false;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.FieldName = "MetadataType";
            gridViewTextBoxColumn6.HeaderText = "Activity Type";
            gridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn6.MaxWidth = 150;
            gridViewTextBoxColumn6.MinWidth = 85;
            gridViewTextBoxColumn6.Name = "metadataType_GridViewColumn";
            gridViewTextBoxColumn6.ReadOnly = true;
            gridViewTextBoxColumn6.Width = 104;
            gridViewCheckBoxColumn1.AllowHide = false;
            gridViewCheckBoxColumn1.EnableExpressionEditor = false;
            gridViewCheckBoxColumn1.FieldName = "Enabled";
            gridViewCheckBoxColumn1.HeaderText = "Enabled";
            gridViewCheckBoxColumn1.MaxWidth = 70;
            gridViewCheckBoxColumn1.MinWidth = 70;
            gridViewCheckBoxColumn1.Name = "enabled_GridViewColumn";
            gridViewCheckBoxColumn1.Width = 70;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.FieldName = "Order";
            gridViewTextBoxColumn7.HeaderText = "Order";
            gridViewTextBoxColumn7.MaxWidth = 70;
            gridViewTextBoxColumn7.MinWidth = 70;
            gridViewTextBoxColumn7.Name = "order_GridViewColumn";
            gridViewTextBoxColumn7.ReadOnly = true;
            gridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn7.Width = 70;
            gridViewDecimalColumn2.DecimalPlaces = 0;
            gridViewDecimalColumn2.EnableExpressionEditor = false;
            gridViewDecimalColumn2.FieldName = "Value";
            gridViewDecimalColumn2.HeaderText = "Count";
            gridViewDecimalColumn2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            gridViewDecimalColumn2.MaxWidth = 80;
            gridViewDecimalColumn2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            gridViewDecimalColumn2.MinWidth = 80;
            gridViewDecimalColumn2.Name = "value_GridViewColumn";
            gridViewDecimalColumn2.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewDecimalColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewDecimalColumn2.ThousandsSeparator = true;
            gridViewDecimalColumn2.Width = 80;
            this.activity_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewCheckBoxColumn1,
            gridViewTextBoxColumn7,
            gridViewDecimalColumn2});
            this.activity_GridView.MasterTemplate.EnableAlternatingRowColor = true;
            sortDescriptor1.PropertyName = "name_GridViewColumn";
            sortDescriptor2.PropertyName = "value_GridViewColumn";
            this.activity_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1,
            sortDescriptor2});
            this.activity_GridView.Name = "activity_GridView";
            this.activity_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.activity_GridView.RootElement.AccessibleDescription = null;
            this.activity_GridView.RootElement.AccessibleName = null;
            this.activity_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(2, 31, 240, 150);
            this.activity_GridView.Size = new System.Drawing.Size(653, 321);
            this.activity_GridView.TabIndex = 81;
            this.activity_GridView.Text = "radGridView1";
            this.activity_GridView.Click += new System.EventHandler(this.activity_GridView_Click);
            // 
            // setup_TabPage
            // 
            this.setup_TabPage.Location = new System.Drawing.Point(4, 24);
            this.setup_TabPage.Name = "setup_TabPage";
            this.setup_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.setup_TabPage.Size = new System.Drawing.Size(658, 355);
            this.setup_TabPage.TabIndex = 1;
            this.setup_TabPage.Tag = "Setup";
            this.setup_TabPage.Text = "Global Setup";
            this.setup_TabPage.ToolTipText = "These setup activities will be run before main workflow activities.";
            this.setup_TabPage.UseVisualStyleBackColor = true;
            // 
            // teardown_TabPage
            // 
            this.teardown_TabPage.Location = new System.Drawing.Point(4, 24);
            this.teardown_TabPage.Name = "teardown_TabPage";
            this.teardown_TabPage.Size = new System.Drawing.Size(658, 355);
            this.teardown_TabPage.TabIndex = 2;
            this.teardown_TabPage.Tag = "Teardown";
            this.teardown_TabPage.Text = "Global Teardown";
            this.teardown_TabPage.ToolTipText = "These actvities will run after the main workflow activities.";
            this.teardown_TabPage.UseVisualStyleBackColor = true;
            // 
            // platform_Label
            // 
            this.platform_Label.AutoSize = true;
            this.platform_Label.Location = new System.Drawing.Point(20, 74);
            this.platform_Label.Name = "platform_Label";
            this.platform_Label.Size = new System.Drawing.Size(53, 15);
            this.platform_Label.TabIndex = 42;
            this.platform_Label.Text = "Platform";
            // 
            // description_Label
            // 
            this.description_Label.AutoSize = true;
            this.description_Label.Location = new System.Drawing.Point(6, 45);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(67, 15);
            this.description_Label.TabIndex = 43;
            this.description_Label.Text = "Description";
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Location = new System.Drawing.Point(34, 16);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(39, 15);
            this.name_Label.TabIndex = 44;
            this.name_Label.Text = "Name";
            // 
            // platform_ComboBox
            // 
            this.platform_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platform_ComboBox.FormattingEnabled = true;
            this.platform_ComboBox.Location = new System.Drawing.Point(79, 71);
            this.platform_ComboBox.Name = "platform_ComboBox";
            this.platform_ComboBox.Size = new System.Drawing.Size(399, 23);
            this.platform_ComboBox.TabIndex = 41;
            this.platform_ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.platform_ComboBox_Validating);
            // 
            // description_TextBox
            // 
            this.description_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description_TextBox.Location = new System.Drawing.Point(79, 42);
            this.description_TextBox.MaxLength = 500;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(399, 23);
            this.description_TextBox.TabIndex = 40;
            // 
            // name_TextBox
            // 
            this.name_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name_TextBox.Location = new System.Drawing.Point(79, 13);
            this.name_TextBox.MaxLength = 255;
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(608, 23);
            this.name_TextBox.TabIndex = 39;
            this.name_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.name_TextBox_Validating);
            // 
            // AdminWorkerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.testcaseid_numericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.activity_GroupBox);
            this.Controls.Add(this.platform_Label);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.platform_ComboBox);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.name_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AdminWorkerControl";
            this.Size = new System.Drawing.Size(697, 541);
            ((System.ComponentModel.ISupportInitialize)(this.activity_GridViewTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testcaseid_numericUpDown)).EndInit();
            this.activity_GroupBox.ResumeLayout(false);
            this.activity_TabControl.ResumeLayout(false);
            this.main_TabPage.ResumeLayout(false);
            this.activity_ToolStrip.ResumeLayout(false);
            this.activity_ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activity_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activity_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label platform_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.ComboBox platform_ComboBox;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.TextBox name_TextBox;
        private Telerik.WinControls.UI.MasterGridViewTemplate activity_GridViewTemplate;
        private System.Windows.Forms.TabControl activity_TabControl;
        private System.Windows.Forms.TabPage main_TabPage;
        private System.Windows.Forms.ToolStrip activity_ToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton newActivity_ToolStripDropDownButton;
        private System.Windows.Forms.ToolStripButton deleteActivity_ToolStripButton;
        private System.Windows.Forms.ToolStripButton copyActivity_ToolStripButton;
        private System.Windows.Forms.ToolStripButton reorder_ToolStripButton;
        private System.Windows.Forms.ToolStripButton enableAll_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView activity_GridView;
        private System.Windows.Forms.TabPage setup_TabPage;
        private System.Windows.Forms.TabPage teardown_TabPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.GroupBox activity_GroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown testcaseid_numericUpDown;

    }
}
