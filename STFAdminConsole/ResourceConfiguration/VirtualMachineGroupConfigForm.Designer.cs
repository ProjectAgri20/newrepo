namespace HP.ScalableTest.LabConsole
{
    partial class VirtualMachineGroupConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VirtualMachineGroupConfigForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ok_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.platformFilter_ToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.machines_DataGridView = new System.Windows.Forms.DataGridView();
            this.machineSelectedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hostNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.holdIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.powerStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usageStateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualMachine_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groups_DataGridView = new System.Windows.Forms.DataGridView();
            this.groupNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.helpPrompt_TextBox = new System.Windows.Forms.TextBox();
            this.apply_Button = new System.Windows.Forms.Button();
            this.userAssignments_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.machines_DataGridView)).BeginInit();
            this.virtualMachine_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groups_DataGridView)).BeginInit();
            this.group_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(380, 504);
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
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 420F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.machines_DataGridView, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groups_DataGridView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.group_ToolStrip, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 59);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(608, 439);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.platformFilter_ToolStripComboBox});
            this.toolStrip1.Location = new System.Drawing.Point(188, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(420, 25);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(95, 22);
            this.toolStripLabel1.Text = "Virtual Machines";
            // 
            // platformFilter_ToolStripComboBox
            // 
            this.platformFilter_ToolStripComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.platformFilter_ToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformFilter_ToolStripComboBox.DropDownWidth = 100;
            this.platformFilter_ToolStripComboBox.Name = "platformFilter_ToolStripComboBox";
            this.platformFilter_ToolStripComboBox.Size = new System.Drawing.Size(300, 25);
            this.platformFilter_ToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.platformFilter_ToolStripComboBox_SelectedIndexChanged);
            // 
            // machines_DataGridView
            // 
            this.machines_DataGridView.AllowUserToAddRows = false;
            this.machines_DataGridView.AllowUserToDeleteRows = false;
            this.machines_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.machines_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.machines_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.machines_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.machines_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.machines_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.machineSelectedColumn,
            this.hostNameColumn,
            this.holdIdColumn,
            this.powerStateColumn,
            this.usageStateColumn});
            this.machines_DataGridView.ContextMenuStrip = this.virtualMachine_contextMenuStrip;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.machines_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.machines_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machines_DataGridView.Location = new System.Drawing.Point(191, 28);
            this.machines_DataGridView.Name = "machines_DataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.machines_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.machines_DataGridView.RowHeadersVisible = false;
            this.machines_DataGridView.RowTemplate.Height = 22;
            this.machines_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.machines_DataGridView.Size = new System.Drawing.Size(414, 408);
            this.machines_DataGridView.TabIndex = 14;
            this.machines_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.machines_DataGridView_CellClick);
            this.machines_DataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.machines_DataGridView_CurrentCellDirtyStateChanged);
            // 
            // machineSelectedColumn
            // 
            this.machineSelectedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.machineSelectedColumn.DataPropertyName = "Selected";
            this.machineSelectedColumn.HeaderText = "";
            this.machineSelectedColumn.Name = "machineSelectedColumn";
            this.machineSelectedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.machineSelectedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.machineSelectedColumn.Width = 19;
            // 
            // hostNameColumn
            // 
            this.hostNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hostNameColumn.DataPropertyName = "Name";
            this.hostNameColumn.HeaderText = "Hostname";
            this.hostNameColumn.Name = "hostNameColumn";
            this.hostNameColumn.Width = 80;
            // 
            // holdIdColumn
            // 
            this.holdIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.holdIdColumn.DataPropertyName = "HoldId";
            this.holdIdColumn.HeaderText = "Hold Id";
            this.holdIdColumn.Name = "holdIdColumn";
            this.holdIdColumn.Width = 66;
            // 
            // powerStateColumn
            // 
            this.powerStateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.powerStateColumn.DataPropertyName = "PowerState";
            this.powerStateColumn.HeaderText = "Power State";
            this.powerStateColumn.Name = "powerStateColumn";
            this.powerStateColumn.Width = 90;
            // 
            // usageStateColumn
            // 
            this.usageStateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.usageStateColumn.DataPropertyName = "UsageState";
            this.usageStateColumn.HeaderText = "Usage";
            this.usageStateColumn.Name = "usageStateColumn";
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
            // groups_DataGridView
            // 
            this.groups_DataGridView.AllowUserToAddRows = false;
            this.groups_DataGridView.AllowUserToDeleteRows = false;
            this.groups_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.groups_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.groups_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.groups_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.groups_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.groups_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupNameColumn});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.groups_DataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.groups_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groups_DataGridView.Location = new System.Drawing.Point(3, 28);
            this.groups_DataGridView.MultiSelect = false;
            this.groups_DataGridView.Name = "groups_DataGridView";
            this.groups_DataGridView.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.groups_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.groups_DataGridView.RowHeadersVisible = false;
            this.groups_DataGridView.RowHeadersWidth = 35;
            this.groups_DataGridView.RowTemplate.Height = 22;
            this.groups_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.groups_DataGridView.Size = new System.Drawing.Size(182, 408);
            this.groups_DataGridView.TabIndex = 12;
            this.groups_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.groups_DataGridView_CellClick);
            // 
            // groupNameColumn
            // 
            this.groupNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupNameColumn.DataPropertyName = "GroupName";
            this.groupNameColumn.HeaderText = "Name";
            this.groupNameColumn.Name = "groupNameColumn";
            this.groupNameColumn.ReadOnly = true;
            // 
            // group_ToolStrip
            // 
            this.group_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.group_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2});
            this.group_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.group_ToolStrip.Name = "group_ToolStrip";
            this.group_ToolStrip.Size = new System.Drawing.Size(188, 25);
            this.group_ToolStrip.TabIndex = 16;
            this.group_ToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabel2.Text = "User Groups";
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(461, 504);
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
            this.helpPrompt_TextBox.Size = new System.Drawing.Size(608, 44);
            this.helpPrompt_TextBox.TabIndex = 15;
            this.helpPrompt_TextBox.Text = "Define Virtual Machine quotas by associating specific Virtual Machines with selec" +
    "ted groups.  This will align a user\'s Virtual Machine availability with group me" +
    "mbership.";
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(542, 504);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 16;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // userAssignments_LinkLabel
            // 
            this.userAssignments_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userAssignments_LinkLabel.AutoSize = true;
            this.userAssignments_LinkLabel.Location = new System.Drawing.Point(12, 509);
            this.userAssignments_LinkLabel.Name = "userAssignments_LinkLabel";
            this.userAssignments_LinkLabel.Size = new System.Drawing.Size(175, 13);
            this.userAssignments_LinkLabel.TabIndex = 17;
            this.userAssignments_LinkLabel.TabStop = true;
            this.userAssignments_LinkLabel.Text = "View Machine Assignments by User";
            this.userAssignments_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.userAssignments_LinkLabel_LinkClicked);
            // 
            // VirtualMachineGroupConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 539);
            this.Controls.Add(this.userAssignments_LinkLabel);
            this.Controls.Add(this.helpPrompt_TextBox);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "VirtualMachineGroupConfigForm";
            this.Text = "Virtual Machine Group Assignments";
            this.Load += new System.EventHandler(this.VirtualMachineGroupConfigForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.machines_DataGridView)).EndInit();
            this.virtualMachine_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groups_DataGridView)).EndInit();
            this.group_ToolStrip.ResumeLayout(false);
            this.group_ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView groups_DataGridView;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox helpPrompt_TextBox;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.ContextMenuStrip virtualMachine_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.DataGridView machines_DataGridView;
        private System.Windows.Forms.ToolStrip group_ToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameColumn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox platformFilter_ToolStripComboBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn machineSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn holdIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn powerStateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usageStateColumn;
        private System.Windows.Forms.LinkLabel userAssignments_LinkLabel;
    }
}