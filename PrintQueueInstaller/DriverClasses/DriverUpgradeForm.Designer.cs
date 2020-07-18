namespace HP.ScalableTest.Print.Utility
{
    partial class DriverUpgradeForm
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

            if (_upgradeManager != null)
            {
                _upgradeManager.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverUpgradeForm));
            this.editControl_Panel = new System.Windows.Forms.Panel();
            this.driverToUpgrade_GridView = new System.Windows.Forms.DataGridView();
            this.includeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.queueNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTimeFormattedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endTimeFormattedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DurationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverUpgradeDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.upgrade_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.main_statusStrip = new System.Windows.Forms.StatusStrip();
            this.main_toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.upgradeStart_TextBox = new System.Windows.Forms.TextBox();
            this.upgradeEnd_TextBox = new System.Windows.Forms.TextBox();
            this.averagePerQueue_TextBox = new System.Windows.Forms.TextBox();
            this.totalUpgradeTime_TextBox = new System.Windows.Forms.TextBox();
            this.viewLog_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.upgrade_GroupBox = new System.Windows.Forms.GroupBox();
            this.upgradeTime_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.abort_Button = new System.Windows.Forms.Button();
            this.driver_GroupBox = new System.Windows.Forms.GroupBox();
            this.warning_Label = new System.Windows.Forms.Label();
            this.currentDriver_GroupBox = new System.Windows.Forms.GroupBox();
            this.installedDriverRefresh_Button = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.driverToUpgrade_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driverUpgradeDataBindingSource)).BeginInit();
            this.main_statusStrip.SuspendLayout();
            this.upgrade_GroupBox.SuspendLayout();
            this.upgradeTime_TableLayoutPanel.SuspendLayout();
            this.driver_GroupBox.SuspendLayout();
            this.currentDriver_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // editControl_Panel
            // 
            this.editControl_Panel.Location = new System.Drawing.Point(6, 19);
            this.editControl_Panel.Name = "editControl_Panel";
            this.editControl_Panel.Size = new System.Drawing.Size(711, 113);
            this.editControl_Panel.TabIndex = 4;
            // 
            // driverToUpgrade_GridView
            // 
            this.driverToUpgrade_GridView.AllowUserToAddRows = false;
            this.driverToUpgrade_GridView.AllowUserToDeleteRows = false;
            this.driverToUpgrade_GridView.AllowUserToOrderColumns = true;
            this.driverToUpgrade_GridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.driverToUpgrade_GridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.driverToUpgrade_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driverToUpgrade_GridView.AutoGenerateColumns = false;
            this.driverToUpgrade_GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.driverToUpgrade_GridView.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.driverToUpgrade_GridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.driverToUpgrade_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.driverToUpgrade_GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.includeDataGridViewCheckBoxColumn,
            this.dataGridViewTextBoxColumn1,
            this.queueNameDataGridViewTextBoxColumn,
            this.startTimeFormattedDataGridViewTextBoxColumn,
            this.endTimeFormattedDataGridViewTextBoxColumn,
            this.DurationColumn});
            this.driverToUpgrade_GridView.DataSource = this.driverUpgradeDataBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.driverToUpgrade_GridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.driverToUpgrade_GridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.driverToUpgrade_GridView.Location = new System.Drawing.Point(9, 105);
            this.driverToUpgrade_GridView.MultiSelect = false;
            this.driverToUpgrade_GridView.Name = "driverToUpgrade_GridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.driverToUpgrade_GridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.driverToUpgrade_GridView.RowHeadersVisible = false;
            this.driverToUpgrade_GridView.RowTemplate.Height = 24;
            this.driverToUpgrade_GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.driverToUpgrade_GridView.Size = new System.Drawing.Size(714, 238);
            this.driverToUpgrade_GridView.TabIndex = 5;
            // 
            // includeDataGridViewCheckBoxColumn
            // 
            this.includeDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.includeDataGridViewCheckBoxColumn.DataPropertyName = "Include";
            this.includeDataGridViewCheckBoxColumn.HeaderText = "Include";
            this.includeDataGridViewCheckBoxColumn.Name = "includeDataGridViewCheckBoxColumn";
            this.includeDataGridViewCheckBoxColumn.Width = 48;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn1.HeaderText = "Status";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 80;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // queueNameDataGridViewTextBoxColumn
            // 
            this.queueNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.queueNameDataGridViewTextBoxColumn.DataPropertyName = "QueueName";
            this.queueNameDataGridViewTextBoxColumn.HeaderText = "Queue";
            this.queueNameDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.queueNameDataGridViewTextBoxColumn.Name = "queueNameDataGridViewTextBoxColumn";
            this.queueNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.queueNameDataGridViewTextBoxColumn.Width = 200;
            // 
            // startTimeFormattedDataGridViewTextBoxColumn
            // 
            this.startTimeFormattedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.startTimeFormattedDataGridViewTextBoxColumn.DataPropertyName = "StartTimeFormatted";
            this.startTimeFormattedDataGridViewTextBoxColumn.HeaderText = "Start Time";
            this.startTimeFormattedDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.startTimeFormattedDataGridViewTextBoxColumn.Name = "startTimeFormattedDataGridViewTextBoxColumn";
            this.startTimeFormattedDataGridViewTextBoxColumn.ReadOnly = true;
            this.startTimeFormattedDataGridViewTextBoxColumn.Width = 130;
            // 
            // endTimeFormattedDataGridViewTextBoxColumn
            // 
            this.endTimeFormattedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.endTimeFormattedDataGridViewTextBoxColumn.DataPropertyName = "EndTimeFormatted";
            this.endTimeFormattedDataGridViewTextBoxColumn.HeaderText = "End Time";
            this.endTimeFormattedDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.endTimeFormattedDataGridViewTextBoxColumn.Name = "endTimeFormattedDataGridViewTextBoxColumn";
            this.endTimeFormattedDataGridViewTextBoxColumn.ReadOnly = true;
            this.endTimeFormattedDataGridViewTextBoxColumn.Width = 130;
            // 
            // DurationColumn
            // 
            this.DurationColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DurationColumn.DataPropertyName = "Duration";
            this.DurationColumn.HeaderText = "Total Duration";
            this.DurationColumn.Name = "DurationColumn";
            // 
            // driverUpgradeDataBindingSource
            // 
            this.driverUpgradeDataBindingSource.DataSource = typeof(HP.ScalableTest.Print.Utility.DriverUpgradeData);
            // 
            // upgrade_Button
            // 
            this.upgrade_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.upgrade_Button.Location = new System.Drawing.Point(458, 613);
            this.upgrade_Button.Name = "upgrade_Button";
            this.upgrade_Button.Size = new System.Drawing.Size(95, 23);
            this.upgrade_Button.TabIndex = 6;
            this.upgrade_Button.Text = "Start Upgrade";
            this.upgrade_Button.UseVisualStyleBackColor = true;
            this.upgrade_Button.Click += new System.EventHandler(this.upgrade_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(660, 613);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 7;
            this.cancel_Button.Text = "Close";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Queues Targeted for Upgrade";
            // 
            // main_statusStrip
            // 
            this.main_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.main_toolStripStatusLabel});
            this.main_statusStrip.Location = new System.Drawing.Point(0, 639);
            this.main_statusStrip.Name = "main_statusStrip";
            this.main_statusStrip.Size = new System.Drawing.Size(744, 22);
            this.main_statusStrip.TabIndex = 9;
            this.main_statusStrip.Text = "statusStrip1";
            // 
            // main_toolStripStatusLabel
            // 
            this.main_toolStripStatusLabel.Name = "main_toolStripStatusLabel";
            this.main_toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // upgradeStart_TextBox
            // 
            this.upgradeStart_TextBox.BackColor = System.Drawing.Color.White;
            this.upgradeStart_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.upgradeStart_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upgradeStart_TextBox.Location = new System.Drawing.Point(117, 5);
            this.upgradeStart_TextBox.Name = "upgradeStart_TextBox";
            this.upgradeStart_TextBox.ReadOnly = true;
            this.upgradeStart_TextBox.Size = new System.Drawing.Size(134, 13);
            this.upgradeStart_TextBox.TabIndex = 10;
            // 
            // upgradeEnd_TextBox
            // 
            this.upgradeEnd_TextBox.BackColor = System.Drawing.Color.White;
            this.upgradeEnd_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.upgradeEnd_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upgradeEnd_TextBox.Location = new System.Drawing.Point(117, 29);
            this.upgradeEnd_TextBox.Name = "upgradeEnd_TextBox";
            this.upgradeEnd_TextBox.ReadOnly = true;
            this.upgradeEnd_TextBox.Size = new System.Drawing.Size(134, 13);
            this.upgradeEnd_TextBox.TabIndex = 12;
            // 
            // averagePerQueue_TextBox
            // 
            this.averagePerQueue_TextBox.BackColor = System.Drawing.Color.White;
            this.averagePerQueue_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.averagePerQueue_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.averagePerQueue_TextBox.Location = new System.Drawing.Point(371, 29);
            this.averagePerQueue_TextBox.Name = "averagePerQueue_TextBox";
            this.averagePerQueue_TextBox.ReadOnly = true;
            this.averagePerQueue_TextBox.Size = new System.Drawing.Size(136, 13);
            this.averagePerQueue_TextBox.TabIndex = 14;
            // 
            // totalUpgradeTime_TextBox
            // 
            this.totalUpgradeTime_TextBox.BackColor = System.Drawing.Color.White;
            this.totalUpgradeTime_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.totalUpgradeTime_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalUpgradeTime_TextBox.Location = new System.Drawing.Point(371, 5);
            this.totalUpgradeTime_TextBox.Name = "totalUpgradeTime_TextBox";
            this.totalUpgradeTime_TextBox.ReadOnly = true;
            this.totalUpgradeTime_TextBox.Size = new System.Drawing.Size(136, 13);
            this.totalUpgradeTime_TextBox.TabIndex = 16;
            // 
            // viewLog_LinkLabel
            // 
            this.viewLog_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewLog_LinkLabel.AutoSize = true;
            this.viewLog_LinkLabel.Location = new System.Drawing.Point(666, 89);
            this.viewLog_LinkLabel.Name = "viewLog_LinkLabel";
            this.viewLog_LinkLabel.Size = new System.Drawing.Size(51, 13);
            this.viewLog_LinkLabel.TabIndex = 19;
            this.viewLog_LinkLabel.TabStop = true;
            this.viewLog_LinkLabel.Text = "View Log";
            this.viewLog_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.viewLog_LinkLabel_LinkClicked);
            // 
            // upgrade_GroupBox
            // 
            this.upgrade_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.upgrade_GroupBox.Controls.Add(this.upgradeTime_TableLayoutPanel);
            this.upgrade_GroupBox.Controls.Add(this.driverToUpgrade_GridView);
            this.upgrade_GroupBox.Controls.Add(this.viewLog_LinkLabel);
            this.upgrade_GroupBox.Controls.Add(this.label1);
            this.upgrade_GroupBox.Location = new System.Drawing.Point(9, 260);
            this.upgrade_GroupBox.Name = "upgrade_GroupBox";
            this.upgrade_GroupBox.Size = new System.Drawing.Size(726, 349);
            this.upgrade_GroupBox.TabIndex = 20;
            this.upgrade_GroupBox.TabStop = false;
            this.upgrade_GroupBox.Text = "Driver Upgrade Statistics";
            // 
            // upgradeTime_TableLayoutPanel
            // 
            this.upgradeTime_TableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.upgradeTime_TableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.upgradeTime_TableLayoutPanel.ColumnCount = 4;
            this.upgradeTime_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.upgradeTime_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.upgradeTime_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.upgradeTime_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.label9, 2, 0);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.label6, 0, 0);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.label8, 0, 1);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.upgradeStart_TextBox, 1, 0);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.upgradeEnd_TextBox, 1, 1);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.label10, 2, 1);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.totalUpgradeTime_TextBox, 3, 0);
            this.upgradeTime_TableLayoutPanel.Controls.Add(this.averagePerQueue_TextBox, 3, 1);
            this.upgradeTime_TableLayoutPanel.Location = new System.Drawing.Point(9, 25);
            this.upgradeTime_TableLayoutPanel.Name = "upgradeTime_TableLayoutPanel";
            this.upgradeTime_TableLayoutPanel.RowCount = 2;
            this.upgradeTime_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.upgradeTime_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.upgradeTime_TableLayoutPanel.Size = new System.Drawing.Size(512, 50);
            this.upgradeTime_TableLayoutPanel.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(256, 2);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 22);
            this.label9.TabIndex = 13;
            this.label9.Text = "Total Upgrade Time";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(2, 2);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 22);
            this.label6.TabIndex = 0;
            this.label6.Text = "Upgrade Start Time";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(2, 26);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 22);
            this.label8.TabIndex = 1;
            this.label8.Text = "Upgrade End Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(256, 26);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 22);
            this.label10.TabIndex = 14;
            this.label10.Text = "Average per Queue";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // abort_Button
            // 
            this.abort_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.abort_Button.Location = new System.Drawing.Point(559, 613);
            this.abort_Button.Name = "abort_Button";
            this.abort_Button.Size = new System.Drawing.Size(95, 23);
            this.abort_Button.TabIndex = 22;
            this.abort_Button.Text = "Abort Upgrade";
            this.abort_Button.UseVisualStyleBackColor = true;
            this.abort_Button.Click += new System.EventHandler(this.abort_Button_Click);
            // 
            // driver_GroupBox
            // 
            this.driver_GroupBox.Controls.Add(this.warning_Label);
            this.driver_GroupBox.Controls.Add(this.editControl_Panel);
            this.driver_GroupBox.Location = new System.Drawing.Point(9, 86);
            this.driver_GroupBox.Name = "driver_GroupBox";
            this.driver_GroupBox.Size = new System.Drawing.Size(726, 168);
            this.driver_GroupBox.TabIndex = 21;
            this.driver_GroupBox.TabStop = false;
            this.driver_GroupBox.Text = "New Driver to Use for Upgrade";
            // 
            // warning_Label
            // 
            this.warning_Label.ForeColor = System.Drawing.Color.MediumBlue;
            this.warning_Label.Location = new System.Drawing.Point(10, 135);
            this.warning_Label.Name = "warning_Label";
            this.warning_Label.Size = new System.Drawing.Size(707, 30);
            this.warning_Label.TabIndex = 24;
            this.warning_Label.Text = "Warning:";
            // 
            // currentDriver_GroupBox
            // 
            this.currentDriver_GroupBox.Controls.Add(this.installedDriverRefresh_Button);
            this.currentDriver_GroupBox.Controls.Add(this.label7);
            this.currentDriver_GroupBox.Location = new System.Drawing.Point(9, 12);
            this.currentDriver_GroupBox.Name = "currentDriver_GroupBox";
            this.currentDriver_GroupBox.Size = new System.Drawing.Size(726, 68);
            this.currentDriver_GroupBox.TabIndex = 22;
            this.currentDriver_GroupBox.TabStop = false;
            this.currentDriver_GroupBox.Text = "Select a Currently Installed Driver";
            // 
            // installedDriverRefresh_Button
            // 
            this.installedDriverRefresh_Button.FlatAppearance.BorderSize = 0;
            this.installedDriverRefresh_Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.installedDriverRefresh_Button.Image = ((System.Drawing.Image)(resources.GetObject("installedDriverRefresh_Button.Image")));
            this.installedDriverRefresh_Button.Location = new System.Drawing.Point(594, 25);
            this.installedDriverRefresh_Button.Name = "installedDriverRefresh_Button";
            this.installedDriverRefresh_Button.Size = new System.Drawing.Size(23, 23);
            this.installedDriverRefresh_Button.TabIndex = 25;
            this.installedDriverRefresh_Button.UseVisualStyleBackColor = true;
            this.installedDriverRefresh_Button.Click += new System.EventHandler(this.installedDriverRefresh_Button_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Installed Driver";
            // 
            // DriverUpgradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 661);
            this.Controls.Add(this.abort_Button);
            this.Controls.Add(this.currentDriver_GroupBox);
            this.Controls.Add(this.driver_GroupBox);
            this.Controls.Add(this.upgrade_GroupBox);
            this.Controls.Add(this.main_statusStrip);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.upgrade_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DriverUpgradeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Driver Upgrade";
            this.Load += new System.EventHandler(this.DriverUpgradeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.driverToUpgrade_GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driverUpgradeDataBindingSource)).EndInit();
            this.main_statusStrip.ResumeLayout(false);
            this.main_statusStrip.PerformLayout();
            this.upgrade_GroupBox.ResumeLayout(false);
            this.upgrade_GroupBox.PerformLayout();
            this.upgradeTime_TableLayoutPanel.ResumeLayout(false);
            this.upgradeTime_TableLayoutPanel.PerformLayout();
            this.driver_GroupBox.ResumeLayout(false);
            this.currentDriver_GroupBox.ResumeLayout(false);
            this.currentDriver_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel editControl_Panel;
        private System.Windows.Forms.DataGridView driverToUpgrade_GridView;
        private System.Windows.Forms.Button upgrade_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip main_statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel main_toolStripStatusLabel;
        private System.Windows.Forms.TextBox upgradeStart_TextBox;
        private System.Windows.Forms.TextBox upgradeEnd_TextBox;
        private System.Windows.Forms.TextBox averagePerQueue_TextBox;
        private System.Windows.Forms.TextBox totalUpgradeTime_TextBox;
        private System.Windows.Forms.LinkLabel viewLog_LinkLabel;
        private System.Windows.Forms.GroupBox upgrade_GroupBox;
        private System.Windows.Forms.GroupBox driver_GroupBox;
        private System.Windows.Forms.GroupBox currentDriver_GroupBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel upgradeTime_TableLayoutPanel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.BindingSource driverUpgradeDataBindingSource;
        private System.Windows.Forms.Button abort_Button;
        private System.Windows.Forms.DataGridViewCheckBoxColumn includeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn queueNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeFormattedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTimeFormattedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DurationColumn;
        private System.Windows.Forms.Button installedDriverRefresh_Button;
        private System.Windows.Forms.Label warning_Label;
    }
}