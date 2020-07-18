namespace HP.ScalableTest.LabConsole
{
    partial class VirtualMachineUserViewForm
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
            if (disposing && _entities != null)
            {
                _entities.Dispose();
                _entities = null;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VirtualMachineUserViewForm));
            this.ok_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.groups_DataGridView = new System.Windows.Forms.DataGridView();
            this.groupSelectedColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.groupNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.users_DataGridView = new System.Windows.Forms.DataGridView();
            this.userNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.platformFilter_ToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.machines_DataGridView = new System.Windows.Forms.DataGridView();
            this.machineSelectedColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.hostNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holdIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.powerStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usageStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualMachine_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machine_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.helpPrompt_TextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groups_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.users_DataGridView)).BeginInit();
            this.group_ToolStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.machines_DataGridView)).BeginInit();
            this.virtualMachine_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(645, 557);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
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
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 440F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groups_DataGridView, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.users_DataGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.group_ToolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.machines_DataGridView, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 62);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(711, 489);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3});
            this.toolStrip2.Location = new System.Drawing.Point(129, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(142, 25);
            this.toolStrip2.TabIndex = 22;
            this.toolStrip2.Text = "toolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(96, 22);
            this.toolStripLabel3.Text = "Assigned Groups";
            // 
            // groups_DataGridView
            // 
            this.groups_DataGridView.AllowUserToAddRows = false;
            this.groups_DataGridView.AllowUserToDeleteRows = false;
            this.groups_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groups_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.groups_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.groups_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.groups_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groups_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupSelectedColumn,
            this.groupNameColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.groups_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.groups_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groups_DataGridView.Location = new System.Drawing.Point(132, 28);
            this.groups_DataGridView.MultiSelect = false;
            this.groups_DataGridView.Name = "groups_DataGridView";
            this.groups_DataGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.groups_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.groups_DataGridView.RowHeadersVisible = false;
            this.groups_DataGridView.RowHeadersWidth = 35;
            this.groups_DataGridView.RowTemplate.Height = 22;
            this.groups_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.groups_DataGridView.Size = new System.Drawing.Size(136, 458);
            this.groups_DataGridView.TabIndex = 21;
            this.groups_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.groups_DataGridView_CellClick);
            // 
            // groupSelectedColumn
            // 
            this.groupSelectedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupSelectedColumn.HeaderText = "";
            this.groupSelectedColumn.Name = "groupSelectedColumn";
            this.groupSelectedColumn.ReadOnly = true;
            this.groupSelectedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.groupSelectedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.groupSelectedColumn.Width = 19;
            // 
            // groupNameColumn
            // 
            this.groupNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupNameColumn.DataPropertyName = "GroupName";
            this.groupNameColumn.HeaderText = "Name";
            this.groupNameColumn.Name = "groupNameColumn";
            this.groupNameColumn.ReadOnly = true;
            // 
            // users_DataGridView
            // 
            this.users_DataGridView.AllowUserToAddRows = false;
            this.users_DataGridView.AllowUserToDeleteRows = false;
            this.users_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.users_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.users_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.users_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.users_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.users_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userNameColumn});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.users_DataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.users_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.users_DataGridView.Location = new System.Drawing.Point(3, 28);
            this.users_DataGridView.MultiSelect = false;
            this.users_DataGridView.Name = "users_DataGridView";
            this.users_DataGridView.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.users_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.users_DataGridView.RowHeadersVisible = false;
            this.users_DataGridView.RowHeadersWidth = 35;
            this.users_DataGridView.RowTemplate.Height = 22;
            this.users_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.users_DataGridView.Size = new System.Drawing.Size(123, 458);
            this.users_DataGridView.TabIndex = 12;
            this.users_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.users_DataGridView_CellClick);
            // 
            // userNameColumn
            // 
            this.userNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.userNameColumn.DataPropertyName = "UserName";
            this.userNameColumn.HeaderText = "Name";
            this.userNameColumn.Name = "userNameColumn";
            this.userNameColumn.ReadOnly = true;
            // 
            // group_ToolStrip
            // 
            this.group_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.group_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2});
            this.group_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.group_ToolStrip.Name = "group_ToolStrip";
            this.group_ToolStrip.Size = new System.Drawing.Size(129, 25);
            this.group_ToolStrip.TabIndex = 16;
            this.group_ToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabel2.Text = "Users";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.platformFilter_ToolStripComboBox});
            this.toolStrip1.Location = new System.Drawing.Point(271, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(440, 25);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(109, 22);
            this.toolStripLabel1.Text = "Assigned Machines";
            // 
            // platformFilter_ToolStripComboBox
            // 
            this.platformFilter_ToolStripComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.platformFilter_ToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformFilter_ToolStripComboBox.DropDownWidth = 100;
            this.platformFilter_ToolStripComboBox.Name = "platformFilter_ToolStripComboBox";
            this.platformFilter_ToolStripComboBox.Size = new System.Drawing.Size(300, 25);
            // 
            // machines_DataGridView
            // 
            this.machines_DataGridView.AllowUserToAddRows = false;
            this.machines_DataGridView.AllowUserToDeleteRows = false;
            this.machines_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.machines_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.machines_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.machines_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.machines_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.machines_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.machineSelectedColumn,
            this.hostNameColumn,
            this.holdIdColumn,
            this.powerStateColumn,
            this.usageStateColumn});
            this.machines_DataGridView.ContextMenuStrip = this.virtualMachine_contextMenuStrip;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.machines_DataGridView.DefaultCellStyle = dataGridViewCellStyle11;
            this.machines_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machines_DataGridView.Location = new System.Drawing.Point(274, 28);
            this.machines_DataGridView.MultiSelect = false;
            this.machines_DataGridView.Name = "machines_DataGridView";
            this.machines_DataGridView.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.machines_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.machines_DataGridView.RowHeadersVisible = false;
            this.machines_DataGridView.RowTemplate.Height = 22;
            this.machines_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.machines_DataGridView.Size = new System.Drawing.Size(434, 458);
            this.machines_DataGridView.TabIndex = 14;
            // 
            // machineSelectedColumn
            // 
            this.machineSelectedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.machineSelectedColumn.HeaderText = "";
            this.machineSelectedColumn.Name = "machineSelectedColumn";
            this.machineSelectedColumn.ReadOnly = true;
            this.machineSelectedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.machineSelectedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.machineSelectedColumn.Width = 19;
            // 
            // hostNameColumn
            // 
            this.hostNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hostNameColumn.DataPropertyName = "Name";
            this.hostNameColumn.HeaderText = "Hostname";
            this.hostNameColumn.Name = "hostNameColumn";
            this.hostNameColumn.ReadOnly = true;
            this.hostNameColumn.Width = 80;
            // 
            // holdIdColumn
            // 
            this.holdIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.holdIdColumn.DataPropertyName = "HoldId";
            this.holdIdColumn.HeaderText = "Hold Id";
            this.holdIdColumn.Name = "holdIdColumn";
            this.holdIdColumn.ReadOnly = true;
            this.holdIdColumn.Width = 66;
            // 
            // powerStateColumn
            // 
            this.powerStateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.powerStateColumn.DataPropertyName = "PowerState";
            this.powerStateColumn.HeaderText = "Power State";
            this.powerStateColumn.Name = "powerStateColumn";
            this.powerStateColumn.ReadOnly = true;
            this.powerStateColumn.Width = 90;
            // 
            // usageStateColumn
            // 
            this.usageStateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.usageStateColumn.DataPropertyName = "UsageState";
            this.usageStateColumn.HeaderText = "Usage";
            this.usageStateColumn.Name = "usageStateColumn";
            this.usageStateColumn.ReadOnly = true;
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
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("uncheckAllToolStripMenuItem.Image")));
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.uncheckAllToolStripMenuItem.Text = "Uncheck All";
            // 
            // machine_ImageList
            // 
            this.machine_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("machine_ImageList.ImageStream")));
            this.machine_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.machine_ImageList.Images.SetKeyName(0, "Tick");
            this.machine_ImageList.Images.SetKeyName(1, "Blank");
            // 
            // helpPrompt_TextBox
            // 
            this.helpPrompt_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.helpPrompt_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpPrompt_TextBox.Location = new System.Drawing.Point(12, 12);
            this.helpPrompt_TextBox.Multiline = true;
            this.helpPrompt_TextBox.Name = "helpPrompt_TextBox";
            this.helpPrompt_TextBox.Size = new System.Drawing.Size(708, 44);
            this.helpPrompt_TextBox.TabIndex = 16;
            this.helpPrompt_TextBox.Text = resources.GetString("helpPrompt_TextBox.Text");
            // 
            // VirtualMachineUserViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 592);
            this.Controls.Add(this.helpPrompt_TextBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ok_Button);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "VirtualMachineUserViewForm";
            this.Text = "User Group and Virtual Machine Assignments";
            this.Load += new System.EventHandler(this.VirtualMachineGroupConfigForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groups_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.users_DataGridView)).EndInit();
            this.group_ToolStrip.ResumeLayout(false);
            this.group_ToolStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.machines_DataGridView)).EndInit();
            this.virtualMachine_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView users_DataGridView;
        private System.Windows.Forms.ContextMenuStrip virtualMachine_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.DataGridView machines_DataGridView;
        private System.Windows.Forms.ToolStrip group_ToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox platformFilter_ToolStripComboBox;
        private System.Windows.Forms.ImageList machine_ImageList;
        private System.Windows.Forms.DataGridView groups_DataGridView;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.DataGridViewImageColumn machineSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn holdIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn powerStateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usageStateColumn;
        private System.Windows.Forms.DataGridViewImageColumn groupSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userNameColumn;
        private System.Windows.Forms.TextBox helpPrompt_TextBox;
    }
}