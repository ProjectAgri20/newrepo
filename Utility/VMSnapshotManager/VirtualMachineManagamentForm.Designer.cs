namespace HP.ScalableTest.Utility
{
    partial class VirtualMachineManagamentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VirtualMachineManagamentForm));
            this.remove_Button = new System.Windows.Forms.Button();
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
            this.create_Button = new System.Windows.Forms.Button();
            this.helpPrompt_TextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clear_button = new System.Windows.Forms.Button();
            this.log_textBox = new System.Windows.Forms.TextBox();
            this.execute_button = new System.Windows.Forms.Button();
            this.executables_dataGridView = new System.Windows.Forms.DataGridView();
            this.executableColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.argumentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arguments_textBox = new System.Windows.Forms.TextBox();
            this.Queue_button = new System.Windows.Forms.Button();
            this.browse_button = new System.Windows.Forms.Button();
            this.file_textBox = new System.Windows.Forms.TextBox();
            this.poweron_button = new System.Windows.Forms.Button();
            this.shutdown_button = new System.Windows.Forms.Button();
            this.validate_button = new System.Windows.Forms.Button();
            this.operations_groupBox = new System.Windows.Forms.GroupBox();
            this.button_reboot = new System.Windows.Forms.Button();
            this.buildDeploy_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.machines_DataGridView)).BeginInit();
            this.virtualMachine_contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.executables_dataGridView)).BeginInit();
            this.operations_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // remove_Button
            // 
            this.remove_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remove_Button.Location = new System.Drawing.Point(108, 56);
            this.remove_Button.Name = "remove_Button";
            this.remove_Button.Size = new System.Drawing.Size(116, 23);
            this.remove_Button.TabIndex = 14;
            this.remove_Button.Text = "Remove Snapshot";
            this.remove_Button.UseVisualStyleBackColor = true;
            this.remove_Button.Click += new System.EventHandler(this.remove_Button_Click);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(424, 522);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.platformFilter_ToolStripComboBox});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
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
            this.machines_DataGridView.Location = new System.Drawing.Point(7, 28);
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
            this.machines_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.machines_DataGridView.Size = new System.Drawing.Size(414, 491);
            this.machines_DataGridView.TabIndex = 14;
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
            // create_Button
            // 
            this.create_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.create_Button.Location = new System.Drawing.Point(7, 56);
            this.create_Button.Name = "create_Button";
            this.create_Button.Size = new System.Drawing.Size(95, 23);
            this.create_Button.TabIndex = 13;
            this.create_Button.Text = "Create Snapshot";
            this.create_Button.UseVisualStyleBackColor = true;
            this.create_Button.Click += new System.EventHandler(this.create_Button_Click);
            // 
            // helpPrompt_TextBox
            // 
            this.helpPrompt_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.helpPrompt_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.helpPrompt_TextBox.Location = new System.Drawing.Point(12, 6);
            this.helpPrompt_TextBox.Multiline = true;
            this.helpPrompt_TextBox.Name = "helpPrompt_TextBox";
            this.helpPrompt_TextBox.Size = new System.Drawing.Size(608, 23);
            this.helpPrompt_TextBox.TabIndex = 15;
            this.helpPrompt_TextBox.Text = "This is a tool designed to help system administrator manage VMs for STF";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clear_button);
            this.groupBox1.Controls.Add(this.log_textBox);
            this.groupBox1.Controls.Add(this.execute_button);
            this.groupBox1.Controls.Add(this.executables_dataGridView);
            this.groupBox1.Controls.Add(this.arguments_textBox);
            this.groupBox1.Controls.Add(this.Queue_button);
            this.groupBox1.Controls.Add(this.browse_button);
            this.groupBox1.Controls.Add(this.file_textBox);
            this.groupBox1.Location = new System.Drawing.Point(439, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 628);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application";
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(116, 295);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(104, 23);
            this.clear_button.TabIndex = 24;
            this.clear_button.Text = "Clear";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // log_textBox
            // 
            this.log_textBox.Location = new System.Drawing.Point(10, 329);
            this.log_textBox.MaxLength = 50000;
            this.log_textBox.Multiline = true;
            this.log_textBox.Name = "log_textBox";
            this.log_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log_textBox.Size = new System.Drawing.Size(387, 293);
            this.log_textBox.TabIndex = 21;
            // 
            // execute_button
            // 
            this.execute_button.Location = new System.Drawing.Point(6, 295);
            this.execute_button.Name = "execute_button";
            this.execute_button.Size = new System.Drawing.Size(104, 23);
            this.execute_button.TabIndex = 4;
            this.execute_button.Text = "Execute";
            this.execute_button.UseVisualStyleBackColor = true;
            this.execute_button.Click += new System.EventHandler(this.execute_button_Click);
            // 
            // executables_dataGridView
            // 
            this.executables_dataGridView.AllowUserToAddRows = false;
            this.executables_dataGridView.AllowUserToDeleteRows = false;
            this.executables_dataGridView.AllowUserToResizeRows = false;
            this.executables_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.executables_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.executableColumn,
            this.argumentColumn});
            this.executables_dataGridView.Location = new System.Drawing.Point(6, 134);
            this.executables_dataGridView.MultiSelect = false;
            this.executables_dataGridView.Name = "executables_dataGridView";
            this.executables_dataGridView.RowHeadersVisible = false;
            this.executables_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.executables_dataGridView.Size = new System.Drawing.Size(373, 150);
            this.executables_dataGridView.TabIndex = 4;
            // 
            // executableColumn
            // 
            this.executableColumn.DataPropertyName = "ExecutableFileName";
            this.executableColumn.HeaderText = "Executable";
            this.executableColumn.Name = "executableColumn";
            this.executableColumn.ReadOnly = true;
            this.executableColumn.Width = 175;
            // 
            // argumentColumn
            // 
            this.argumentColumn.DataPropertyName = "Argument";
            this.argumentColumn.HeaderText = "Argument";
            this.argumentColumn.Name = "argumentColumn";
            this.argumentColumn.ReadOnly = true;
            this.argumentColumn.Width = 200;
            // 
            // arguments_textBox
            // 
            this.arguments_textBox.Location = new System.Drawing.Point(6, 60);
            this.arguments_textBox.Name = "arguments_textBox";
            this.arguments_textBox.Size = new System.Drawing.Size(317, 20);
            this.arguments_textBox.TabIndex = 2;
            // 
            // Queue_button
            // 
            this.Queue_button.Location = new System.Drawing.Point(231, 86);
            this.Queue_button.Name = "Queue_button";
            this.Queue_button.Size = new System.Drawing.Size(92, 23);
            this.Queue_button.TabIndex = 3;
            this.Queue_button.Text = "Queue";
            this.Queue_button.UseVisualStyleBackColor = true;
            this.Queue_button.Click += new System.EventHandler(this.Queue_button_Click);
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(329, 34);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(50, 20);
            this.browse_button.TabIndex = 1;
            this.browse_button.Text = "...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // file_textBox
            // 
            this.file_textBox.Location = new System.Drawing.Point(6, 34);
            this.file_textBox.Name = "file_textBox";
            this.file_textBox.Size = new System.Drawing.Size(317, 20);
            this.file_textBox.TabIndex = 0;
            // 
            // poweron_button
            // 
            this.poweron_button.Location = new System.Drawing.Point(6, 19);
            this.poweron_button.Name = "poweron_button";
            this.poweron_button.Size = new System.Drawing.Size(95, 23);
            this.poweron_button.TabIndex = 10;
            this.poweron_button.Text = "Power On";
            this.poweron_button.UseVisualStyleBackColor = true;
            this.poweron_button.Click += new System.EventHandler(this.poweron_button_Click);
            // 
            // shutdown_button
            // 
            this.shutdown_button.Location = new System.Drawing.Point(107, 19);
            this.shutdown_button.Name = "shutdown_button";
            this.shutdown_button.Size = new System.Drawing.Size(116, 23);
            this.shutdown_button.TabIndex = 11;
            this.shutdown_button.Text = "Shut down";
            this.shutdown_button.UseVisualStyleBackColor = true;
            this.shutdown_button.Click += new System.EventHandler(this.shutdown_button_Click);
            // 
            // validate_button
            // 
            this.validate_button.Location = new System.Drawing.Point(229, 19);
            this.validate_button.Name = "validate_button";
            this.validate_button.Size = new System.Drawing.Size(95, 23);
            this.validate_button.TabIndex = 12;
            this.validate_button.Text = "Validate";
            this.validate_button.UseVisualStyleBackColor = true;
            this.validate_button.Click += new System.EventHandler(this.validate_button_Click);
            // 
            // operations_groupBox
            // 
            this.operations_groupBox.Controls.Add(this.button_reboot);
            this.operations_groupBox.Controls.Add(this.buildDeploy_button);
            this.operations_groupBox.Controls.Add(this.validate_button);
            this.operations_groupBox.Controls.Add(this.shutdown_button);
            this.operations_groupBox.Controls.Add(this.poweron_button);
            this.operations_groupBox.Controls.Add(this.remove_Button);
            this.operations_groupBox.Controls.Add(this.create_Button);
            this.operations_groupBox.Location = new System.Drawing.Point(12, 563);
            this.operations_groupBox.Name = "operations_groupBox";
            this.operations_groupBox.Size = new System.Drawing.Size(424, 100);
            this.operations_groupBox.TabIndex = 20;
            this.operations_groupBox.TabStop = false;
            this.operations_groupBox.Text = "VM Operations";
            // 
            // button_reboot
            // 
            this.button_reboot.Location = new System.Drawing.Point(229, 56);
            this.button_reboot.Name = "button_reboot";
            this.button_reboot.Size = new System.Drawing.Size(95, 23);
            this.button_reboot.TabIndex = 15;
            this.button_reboot.Text = "Reboot";
            this.button_reboot.UseVisualStyleBackColor = true;
            this.button_reboot.Click += new System.EventHandler(this.button_reboot_Click);
            // 
            // buildDeploy_button
            // 
            this.buildDeploy_button.Location = new System.Drawing.Point(329, 19);
            this.buildDeploy_button.Name = "buildDeploy_button";
            this.buildDeploy_button.Size = new System.Drawing.Size(92, 60);
            this.buildDeploy_button.TabIndex = 25;
            this.buildDeploy_button.Text = "Deploy STF Build";
            this.buildDeploy_button.UseVisualStyleBackColor = true;
            this.buildDeploy_button.Click += new System.EventHandler(this.buildDeploy_button_Click);
            // 
            // VirtualMachineManagamentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 675);
            this.Controls.Add(this.operations_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.helpPrompt_TextBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "VirtualMachineManagamentForm";
            this.Text = "Virtual Machine Management";
            this.Load += new System.EventHandler(this.VirtualMachineGroupConfigForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.machines_DataGridView)).EndInit();
            this.virtualMachine_contextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.executables_dataGridView)).EndInit();
            this.operations_groupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button remove_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button create_Button;
        private System.Windows.Forms.TextBox helpPrompt_TextBox;
        private System.Windows.Forms.ContextMenuStrip virtualMachine_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.DataGridView machines_DataGridView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox platformFilter_ToolStripComboBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn machineSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn holdIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn powerStateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn usageStateColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Queue_button;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox file_textBox;
        private System.Windows.Forms.TextBox arguments_textBox;
        private System.Windows.Forms.DataGridView executables_dataGridView;
        private System.Windows.Forms.Button poweron_button;
        private System.Windows.Forms.Button shutdown_button;
        private System.Windows.Forms.Button validate_button;
        private System.Windows.Forms.TextBox log_textBox;
        private System.Windows.Forms.Button execute_button;
        private System.Windows.Forms.GroupBox operations_groupBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn executableColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn argumentColumn;
        private System.Windows.Forms.Button clear_button;
        private System.Windows.Forms.Button button_reboot;
        private System.Windows.Forms.Button buildDeploy_button;
    }
}