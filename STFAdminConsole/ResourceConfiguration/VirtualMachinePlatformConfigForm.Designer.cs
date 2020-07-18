namespace HP.ScalableTest.LabConsole
{
    partial class VirtualMachinePlatformConfigForm
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
            if (disposing && _assetInventoryContext != null)
            {
                _assetInventoryContext?.Dispose();
                _assetInventoryContext = null;

                _enterpriseTestContext?.Dispose();
                _enterpriseTestContext = null;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VirtualMachinePlatformConfigForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ok_Button = new System.Windows.Forms.Button();
            this.currentCode_Label = new System.Windows.Forms.Label();
            this.virtualMachines_Label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.virtualMachine_DataGridView = new System.Windows.Forms.DataGridView();
            this.virtualMachineSelectedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.virtualMachineNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualMachineHoldIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualMachine_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.platform_DataGridView = new System.Windows.Forms.DataGridView();
            this.platformPlatformIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.platformActiveColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.platformDescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.platform_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resourceType_DataGridView = new System.Windows.Forms.DataGridView();
            this.resourceTypeSelectedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.resourceTypeNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.helpPrompt_TextBox = new System.Windows.Forms.TextBox();
            this.apply_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.virtualMachine_DataGridView)).BeginInit();
            this.virtualMachine_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.platform_DataGridView)).BeginInit();
            this.platform_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resourceType_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(658, 466);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 4;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // currentCode_Label
            // 
            this.currentCode_Label.AutoSize = true;
            this.currentCode_Label.Location = new System.Drawing.Point(3, 0);
            this.currentCode_Label.Name = "currentCode_Label";
            this.currentCode_Label.Size = new System.Drawing.Size(121, 13);
            this.currentCode_Label.TabIndex = 9;
            this.currentCode_Label.Text = "Virtual Machine Platform";
            // 
            // virtualMachines_Label
            // 
            this.virtualMachines_Label.AutoSize = true;
            this.virtualMachines_Label.Location = new System.Drawing.Point(492, 0);
            this.virtualMachines_Label.Name = "virtualMachines_Label";
            this.virtualMachines_Label.Size = new System.Drawing.Size(140, 13);
            this.virtualMachines_Label.TabIndex = 10;
            this.virtualMachines_Label.Text = "Associated Virtual Machines";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.virtualMachine_DataGridView, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.currentCode_Label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.virtualMachines_Label, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.platform_DataGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.resourceType_DataGridView, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 73);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(889, 387);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // virtualMachine_DataGridView
            // 
            this.virtualMachine_DataGridView.AllowUserToAddRows = false;
            this.virtualMachine_DataGridView.AllowUserToDeleteRows = false;
            this.virtualMachine_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.virtualMachine_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.virtualMachine_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.virtualMachine_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.virtualMachine_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.virtualMachine_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.virtualMachineSelectedColumn,
            this.virtualMachineNameColumn,
            this.virtualMachineHoldIdColumn});
            this.virtualMachine_DataGridView.ContextMenuStrip = this.virtualMachine_contextMenuStrip;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.virtualMachine_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.virtualMachine_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualMachine_DataGridView.Location = new System.Drawing.Point(492, 16);
            this.virtualMachine_DataGridView.Name = "virtualMachine_DataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.virtualMachine_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.virtualMachine_DataGridView.RowHeadersVisible = false;
            this.virtualMachine_DataGridView.RowTemplate.Height = 22;
            this.virtualMachine_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.virtualMachine_DataGridView.Size = new System.Drawing.Size(244, 368);
            this.virtualMachine_DataGridView.TabIndex = 14;
            this.virtualMachine_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.virtualMachine_DataGridView_CellClick);
            this.virtualMachine_DataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.virtualMachine_DataGridView_CurrentCellDirtyStateChanged);
            // 
            // virtualMachineSelectedColumn
            // 
            this.virtualMachineSelectedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.virtualMachineSelectedColumn.DataPropertyName = "Selected";
            this.virtualMachineSelectedColumn.HeaderText = "";
            this.virtualMachineSelectedColumn.Name = "virtualMachineSelectedColumn";
            this.virtualMachineSelectedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.virtualMachineSelectedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.virtualMachineSelectedColumn.Width = 19;
            // 
            // virtualMachineNameColumn
            // 
            this.virtualMachineNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.virtualMachineNameColumn.DataPropertyName = "Name";
            this.virtualMachineNameColumn.HeaderText = "Name";
            this.virtualMachineNameColumn.Name = "virtualMachineNameColumn";
            this.virtualMachineNameColumn.Width = 60;
            // 
            // virtualMachineHoldIdColumn
            // 
            this.virtualMachineHoldIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.virtualMachineHoldIdColumn.DataPropertyName = "HoldId";
            this.virtualMachineHoldIdColumn.HeaderText = "Hold Id";
            this.virtualMachineHoldIdColumn.Name = "virtualMachineHoldIdColumn";
            // 
            // virtualMachine_contextMenuStrip
            // 
            this.virtualMachine_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.virtualMachine_contextMenuStrip.Name = "virtualMachine_contextMenuStrip";
            this.virtualMachine_contextMenuStrip.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("selectAllToolStripMenuItem.Image")));
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectAllToolStripMenuItem.Text = "Check All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uncheckAllToolStripMenuItem.Image")));
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck All";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(742, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Associated Resource Types";
            // 
            // platform_DataGridView
            // 
            this.platform_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.platform_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.platform_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.platform_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.platform_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.platform_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.platformPlatformIdColumn,
            this.platformActiveColumn,
            this.platformDescriptionColumn});
            this.platform_DataGridView.ContextMenuStrip = this.platform_ContextMenuStrip;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.platform_DataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.platform_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.platform_DataGridView.Location = new System.Drawing.Point(3, 16);
            this.platform_DataGridView.MultiSelect = false;
            this.platform_DataGridView.Name = "platform_DataGridView";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.platform_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.platform_DataGridView.RowHeadersWidth = 35;
            this.platform_DataGridView.RowTemplate.Height = 22;
            this.platform_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.platform_DataGridView.Size = new System.Drawing.Size(483, 368);
            this.platform_DataGridView.TabIndex = 12;
            this.platform_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.platform_DataGridView_CellClick);
            this.platform_DataGridView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.platform_DataGridView_RowValidating);
            this.platform_DataGridView.SelectionChanged += new System.EventHandler(this.platform_DataGridView_SelectionChanged);
            this.platform_DataGridView.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.platform_DataGridView_UserAddedRow);
            this.platform_DataGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.platform_DataGridView_UserDeletingRow);
            // 
            // platformPlatformIdColumn
            // 
            this.platformPlatformIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.platformPlatformIdColumn.DataPropertyName = "FrameworkClientPlatformId";
            this.platformPlatformIdColumn.HeaderText = "Platform Id";
            this.platformPlatformIdColumn.Name = "platformPlatformIdColumn";
            this.platformPlatformIdColumn.Width = 82;
            // 
            // platformActiveColumn
            // 
            this.platformActiveColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.platformActiveColumn.DataPropertyName = "Active";
            this.platformActiveColumn.HeaderText = "Active";
            this.platformActiveColumn.Name = "platformActiveColumn";
            this.platformActiveColumn.Width = 43;
            // 
            // platformDescriptionColumn
            // 
            this.platformDescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.platformDescriptionColumn.DataPropertyName = "Name";
            this.platformDescriptionColumn.HeaderText = "Description";
            this.platformDescriptionColumn.Name = "platformDescriptionColumn";
            // 
            // platform_ContextMenuStrip
            // 
            this.platform_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.platform_ContextMenuStrip.Name = "platform_ContextMenuStrip";
            this.platform_ContextMenuStrip.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // resourceType_DataGridView
            // 
            this.resourceType_DataGridView.AllowUserToAddRows = false;
            this.resourceType_DataGridView.AllowUserToDeleteRows = false;
            this.resourceType_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.resourceType_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.resourceType_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resourceType_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.resourceType_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resourceType_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resourceTypeSelectedColumn,
            this.resourceTypeNameColumn});
            this.resourceType_DataGridView.ContextMenuStrip = this.virtualMachine_contextMenuStrip;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.resourceType_DataGridView.DefaultCellStyle = dataGridViewCellStyle11;
            this.resourceType_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceType_DataGridView.Location = new System.Drawing.Point(742, 16);
            this.resourceType_DataGridView.Name = "resourceType_DataGridView";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resourceType_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.resourceType_DataGridView.RowHeadersVisible = false;
            this.resourceType_DataGridView.RowTemplate.Height = 22;
            this.resourceType_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resourceType_DataGridView.Size = new System.Drawing.Size(144, 368);
            this.resourceType_DataGridView.TabIndex = 13;
            this.resourceType_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resourceType_DataGridView_CellClick);
            // 
            // resourceTypeSelectedColumn
            // 
            this.resourceTypeSelectedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resourceTypeSelectedColumn.DataPropertyName = "Selected";
            this.resourceTypeSelectedColumn.HeaderText = "";
            this.resourceTypeSelectedColumn.Name = "resourceTypeSelectedColumn";
            this.resourceTypeSelectedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.resourceTypeSelectedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.resourceTypeSelectedColumn.Width = 19;
            // 
            // resourceTypeNameColumn
            // 
            this.resourceTypeNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resourceTypeNameColumn.DataPropertyName = "Name";
            this.resourceTypeNameColumn.HeaderText = "Name";
            this.resourceTypeNameColumn.Name = "resourceTypeNameColumn";
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(739, 466);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 14;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // helpPrompt_TextBox
            // 
            this.helpPrompt_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.helpPrompt_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpPrompt_TextBox.Location = new System.Drawing.Point(12, 9);
            this.helpPrompt_TextBox.Multiline = true;
            this.helpPrompt_TextBox.Name = "helpPrompt_TextBox";
            this.helpPrompt_TextBox.Size = new System.Drawing.Size(883, 58);
            this.helpPrompt_TextBox.TabIndex = 15;
            this.helpPrompt_TextBox.Text = resources.GetString("helpPrompt_TextBox.Text");
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(820, 466);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 16;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // VirtualMachinePlatformConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 501);
            this.Controls.Add(this.helpPrompt_TextBox);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "VirtualMachinePlatformConfigForm";
            this.Text = "Virtual Machine Platform Association";
            this.Load += new System.EventHandler(this.VirtualMachinePlatformConfigForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.virtualMachine_DataGridView)).EndInit();
            this.virtualMachine_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.platform_DataGridView)).EndInit();
            this.platform_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resourceType_DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label currentCode_Label;
        private System.Windows.Forms.Label virtualMachines_Label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView platform_DataGridView;
        private System.Windows.Forms.DataGridView resourceType_DataGridView;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox helpPrompt_TextBox;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.ContextMenuStrip virtualMachine_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip platform_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.DataGridView virtualMachine_DataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn virtualMachineSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualMachineNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualMachineHoldIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn platformPlatformIdColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn platformActiveColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn platformDescriptionColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn resourceTypeSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resourceTypeNameColumn;
        private System.Windows.Forms.Label label1;
    }
}