namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class PerfMonCollectorControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerfMonCollectorControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.domain_textBox = new System.Windows.Forms.TextBox();
            this.domain_label = new System.Windows.Forms.Label();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.password_label = new System.Windows.Forms.Label();
            this.userName_textBox = new System.Windows.Forms.TextBox();
            this.username_label = new System.Windows.Forms.Label();
            this.interval_groupBox = new System.Windows.Forms.GroupBox();
            this.interval_TimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.platform_Label = new System.Windows.Forms.Label();
            this.platform_ComboBox = new System.Windows.Forms.ComboBox();
            this.name_Label = new System.Windows.Forms.Label();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.server_ListBox = new System.Windows.Forms.ListBox();
            this.category_ListBox = new System.Windows.Forms.ListBox();
            this.instance_ListBox = new System.Windows.Forms.ListBox();
            this.counter_ListBox = new System.Windows.Forms.ListBox();
            this.instance_Label = new System.Windows.Forms.Label();
            this.counters_Label = new System.Windows.Forms.Label();
            this.category_Label = new System.Windows.Forms.Label();
            this.server_Label = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.toolTipServer = new System.Windows.Forms.ToolTip(this.components);
            this.selectedCounters_DataGridView = new System.Windows.Forms.DataGridView();
            this.targetHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.counter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.counters_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.addCounter_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteCounter_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            this.interval_groupBox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedCounters_DataGridView)).BeginInit();
            this.counters_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.domain_textBox);
            this.groupBox1.Controls.Add(this.domain_label);
            this.groupBox1.Controls.Add(this.password_textBox);
            this.groupBox1.Controls.Add(this.password_label);
            this.groupBox1.Controls.Add(this.userName_textBox);
            this.groupBox1.Controls.Add(this.username_label);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(18, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(660, 62);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Credentials (Optional)";
            this.toolTipServer.SetToolTip(this.groupBox1, "Enter user credentials if accessing remote server");
            // 
            // domain_textBox
            // 
            this.domain_textBox.Location = new System.Drawing.Point(72, 24);
            this.domain_textBox.Name = "domain_textBox";
            this.domain_textBox.Size = new System.Drawing.Size(148, 23);
            this.domain_textBox.TabIndex = 6;
            // 
            // domain_label
            // 
            this.domain_label.AutoSize = true;
            this.domain_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domain_label.Location = new System.Drawing.Point(17, 27);
            this.domain_label.Name = "domain_label";
            this.domain_label.Size = new System.Drawing.Size(49, 15);
            this.domain_label.TabIndex = 5;
            this.domain_label.Text = "Domain";
            // 
            // password_textBox
            // 
            this.password_textBox.Location = new System.Drawing.Point(526, 24);
            this.password_textBox.MaxLength = 20;
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.PasswordChar = '*';
            this.password_textBox.Size = new System.Drawing.Size(126, 23);
            this.password_textBox.TabIndex = 10;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_label.Location = new System.Drawing.Point(463, 27);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(57, 15);
            this.password_label.TabIndex = 9;
            this.password_label.Text = "Password";
            // 
            // userName_textBox
            // 
            this.userName_textBox.Location = new System.Drawing.Point(308, 24);
            this.userName_textBox.Name = "userName_textBox";
            this.userName_textBox.Size = new System.Drawing.Size(126, 23);
            this.userName_textBox.TabIndex = 8;
            // 
            // username_label
            // 
            this.username_label.AutoSize = true;
            this.username_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username_label.Location = new System.Drawing.Point(237, 27);
            this.username_label.Name = "username_label";
            this.username_label.Size = new System.Drawing.Size(65, 15);
            this.username_label.TabIndex = 7;
            this.username_label.Text = "User Name";
            // 
            // interval_groupBox
            // 
            this.interval_groupBox.Controls.Add(this.interval_TimeSpanControl);
            this.interval_groupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.interval_groupBox.Location = new System.Drawing.Point(18, 285);
            this.interval_groupBox.Name = "interval_groupBox";
            this.interval_groupBox.Size = new System.Drawing.Size(220, 54);
            this.interval_groupBox.TabIndex = 20;
            this.interval_groupBox.TabStop = false;
            this.interval_groupBox.Text = "Interval";
            this.toolTipServer.SetToolTip(this.interval_groupBox, "Set the interval for collecting the counter samples");
            // 
            // interval_TimeSpanControl
            // 
            this.interval_TimeSpanControl.AutoSize = true;
            this.interval_TimeSpanControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.interval_TimeSpanControl.Font = new System.Drawing.Font("Arial", 9F);
            this.interval_TimeSpanControl.Location = new System.Drawing.Point(17, 17);
            this.interval_TimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.interval_TimeSpanControl.Name = "interval_TimeSpanControl";
            this.interval_TimeSpanControl.Size = new System.Drawing.Size(160, 24);
            this.interval_TimeSpanControl.TabIndex = 21;
            // 
            // platform_Label
            // 
            this.platform_Label.AutoSize = true;
            this.platform_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.platform_Label.Location = new System.Drawing.Point(20, 45);
            this.platform_Label.Name = "platform_Label";
            this.platform_Label.Size = new System.Drawing.Size(53, 15);
            this.platform_Label.TabIndex = 2;
            this.platform_Label.Text = "Platform";
            // 
            // platform_ComboBox
            // 
            this.platform_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platform_ComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.platform_ComboBox.FormattingEnabled = true;
            this.platform_ComboBox.Location = new System.Drawing.Point(79, 42);
            this.platform_ComboBox.Name = "platform_ComboBox";
            this.platform_ComboBox.Size = new System.Drawing.Size(383, 23);
            this.platform_ComboBox.TabIndex = 3;
            this.toolTipServer.SetToolTip(this.platform_ComboBox, "The platform on which the perfmon collector will execute");
            this.platform_ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.platform_ComboBox_Validating);
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_Label.Location = new System.Drawing.Point(34, 16);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(39, 15);
            this.name_Label.TabIndex = 0;
            this.name_Label.Text = "Name";
            // 
            // name_TextBox
            // 
            this.name_TextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_TextBox.Location = new System.Drawing.Point(79, 13);
            this.name_TextBox.MaxLength = 255;
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(599, 23);
            this.name_TextBox.TabIndex = 1;
            // 
            // server_ListBox
            // 
            this.server_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.server_ListBox.DisplayMember = "HostName";
            this.server_ListBox.FormattingEnabled = true;
            this.server_ListBox.HorizontalScrollbar = true;
            this.server_ListBox.ItemHeight = 15;
            this.server_ListBox.Location = new System.Drawing.Point(10, 20);
            this.server_ListBox.Name = "server_ListBox";
            this.server_ListBox.Size = new System.Drawing.Size(150, 109);
            this.server_ListBox.TabIndex = 0;
            this.server_ListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.server_ListBox_MouseMove);
            // 
            // category_ListBox
            // 
            this.category_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.category_ListBox.FormattingEnabled = true;
            this.category_ListBox.HorizontalScrollbar = true;
            this.category_ListBox.ItemHeight = 15;
            this.category_ListBox.Location = new System.Drawing.Point(174, 20);
            this.category_ListBox.Name = "category_ListBox";
            this.category_ListBox.Size = new System.Drawing.Size(150, 109);
            this.category_ListBox.TabIndex = 1;
            this.category_ListBox.SelectedIndexChanged += new System.EventHandler(this.category_ListBox_SelectedIndexChanged);
            // 
            // instance_ListBox
            // 
            this.instance_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instance_ListBox.FormattingEnabled = true;
            this.instance_ListBox.HorizontalScrollbar = true;
            this.instance_ListBox.ItemHeight = 15;
            this.instance_ListBox.Location = new System.Drawing.Point(339, 20);
            this.instance_ListBox.Name = "instance_ListBox";
            this.instance_ListBox.Size = new System.Drawing.Size(150, 109);
            this.instance_ListBox.TabIndex = 2;
            // 
            // counter_ListBox
            // 
            this.counter_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.counter_ListBox.FormattingEnabled = true;
            this.counter_ListBox.HorizontalScrollbar = true;
            this.counter_ListBox.ItemHeight = 15;
            this.counter_ListBox.Location = new System.Drawing.Point(504, 20);
            this.counter_ListBox.Name = "counter_ListBox";
            this.counter_ListBox.Size = new System.Drawing.Size(147, 109);
            this.counter_ListBox.TabIndex = 3;
            // 
            // instance_Label
            // 
            this.instance_Label.AutoSize = true;
            this.instance_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instance_Label.Location = new System.Drawing.Point(341, 0);
            this.instance_Label.Name = "instance_Label";
            this.instance_Label.Size = new System.Drawing.Size(51, 15);
            this.instance_Label.TabIndex = 14;
            this.instance_Label.Text = "Instance";
            // 
            // counters_Label
            // 
            this.counters_Label.AutoSize = true;
            this.counters_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.counters_Label.Location = new System.Drawing.Point(506, 0);
            this.counters_Label.Name = "counters_Label";
            this.counters_Label.Size = new System.Drawing.Size(55, 15);
            this.counters_Label.TabIndex = 15;
            this.counters_Label.Text = "Counters";
            // 
            // category_Label
            // 
            this.category_Label.AutoSize = true;
            this.category_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.category_Label.Location = new System.Drawing.Point(176, 0);
            this.category_Label.Name = "category_Label";
            this.category_Label.Size = new System.Drawing.Size(55, 15);
            this.category_Label.TabIndex = 13;
            this.category_Label.Text = "Category";
            // 
            // server_Label
            // 
            this.server_Label.AutoSize = true;
            this.server_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.server_Label.Location = new System.Drawing.Point(10, 0);
            this.server_Label.Name = "server_Label";
            this.server_Label.Size = new System.Drawing.Size(39, 15);
            this.server_Label.TabIndex = 12;
            this.server_Label.Text = "Server";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.server_Label);
            this.groupBox4.Controls.Add(this.category_Label);
            this.groupBox4.Controls.Add(this.counters_Label);
            this.groupBox4.Controls.Add(this.instance_Label);
            this.groupBox4.Controls.Add(this.counter_ListBox);
            this.groupBox4.Controls.Add(this.instance_ListBox);
            this.groupBox4.Controls.Add(this.category_ListBox);
            this.groupBox4.Controls.Add(this.server_ListBox);
            this.groupBox4.Location = new System.Drawing.Point(18, 139);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(660, 140);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // selectedCounters_DataGridView
            // 
            this.selectedCounters_DataGridView.AllowUserToAddRows = false;
            this.selectedCounters_DataGridView.AllowUserToDeleteRows = false;
            this.selectedCounters_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedCounters_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.selectedCounters_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.targetHost,
            this.category,
            this.instance,
            this.counter,
            this.interval});
            this.selectedCounters_DataGridView.Location = new System.Drawing.Point(3, 380);
            this.selectedCounters_DataGridView.MultiSelect = false;
            this.selectedCounters_DataGridView.Name = "selectedCounters_DataGridView";
            this.selectedCounters_DataGridView.RowHeadersVisible = false;
            this.selectedCounters_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.selectedCounters_DataGridView.Size = new System.Drawing.Size(678, 153);
            this.selectedCounters_DataGridView.TabIndex = 71;
            // 
            // targetHost
            // 
            this.targetHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.targetHost.DataPropertyName = "TargetHost";
            this.targetHost.HeaderText = "Host Name";
            this.targetHost.Name = "targetHost";
            this.targetHost.Width = 92;
            // 
            // category
            // 
            this.category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.category.DataPropertyName = "Category";
            this.category.HeaderText = "Category";
            this.category.Name = "category";
            this.category.Width = 80;
            // 
            // instance
            // 
            this.instance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.instance.DataPropertyName = "InstanceName";
            this.instance.HeaderText = "Instance";
            this.instance.Name = "instance";
            this.instance.Width = 76;
            // 
            // counter
            // 
            this.counter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.counter.DataPropertyName = "Counter";
            this.counter.HeaderText = "Counter";
            this.counter.Name = "counter";
            this.counter.Width = 75;
            // 
            // interval
            // 
            this.interval.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.interval.DataPropertyName = "IntervalString";
            this.interval.HeaderText = "Interval";
            this.interval.Name = "interval";
            // 
            // counters_ToolStrip
            // 
            this.counters_ToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.counters_ToolStrip.AutoSize = false;
            this.counters_ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.counters_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.counters_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCounter_ToolStripButton,
            this.deleteCounter_ToolStripButton});
            this.counters_ToolStrip.Location = new System.Drawing.Point(3, 352);
            this.counters_ToolStrip.Name = "counters_ToolStrip";
            this.counters_ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.counters_ToolStrip.Size = new System.Drawing.Size(655, 25);
            this.counters_ToolStrip.TabIndex = 72;
            this.counters_ToolStrip.Text = "toolStrip1";
            // 
            // addCounter_ToolStripButton
            // 
            this.addCounter_ToolStripButton.Enabled = false;
            this.addCounter_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addCounter_ToolStripButton.Image")));
            this.addCounter_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addCounter_ToolStripButton.Name = "addCounter_ToolStripButton";
            this.addCounter_ToolStripButton.Size = new System.Drawing.Size(95, 22);
            this.addCounter_ToolStripButton.Text = "Add Counter";
            this.addCounter_ToolStripButton.Click += new System.EventHandler(this.addCounter_ToolStripButton_Click);
            // 
            // deleteCounter_ToolStripButton
            // 
            this.deleteCounter_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteCounter_ToolStripButton.Image")));
            this.deleteCounter_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteCounter_ToolStripButton.Name = "deleteCounter_ToolStripButton";
            this.deleteCounter_ToolStripButton.Size = new System.Drawing.Size(116, 22);
            this.deleteCounter_ToolStripButton.Text = "Remove Counter";
            this.deleteCounter_ToolStripButton.Click += new System.EventHandler(this.deleteCounter_TooStripButton_Click);
            // 
            // PerfMonCollectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.counters_ToolStrip);
            this.Controls.Add(this.selectedCounters_DataGridView);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.platform_Label);
            this.Controls.Add(this.platform_ComboBox);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.interval_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PerfMonCollectorControl";
            this.Size = new System.Drawing.Size(684, 536);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.interval_groupBox.ResumeLayout(false);
            this.interval_groupBox.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedCounters_DataGridView)).EndInit();
            this.counters_ToolStrip.ResumeLayout(false);
            this.counters_ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.TextBox userName_textBox;
        private System.Windows.Forms.Label username_label;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.GroupBox interval_groupBox;
        private ScalableTest.Framework.UI.TimeSpanControl interval_TimeSpanControl;
        private System.Windows.Forms.Label platform_Label;
        private System.Windows.Forms.ComboBox platform_ComboBox;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.TextBox domain_textBox;
        private System.Windows.Forms.Label domain_label;
        private System.Windows.Forms.ListBox server_ListBox;
        private System.Windows.Forms.ListBox category_ListBox;
        private System.Windows.Forms.ListBox instance_ListBox;
        private System.Windows.Forms.ListBox counter_ListBox;
        private System.Windows.Forms.Label instance_Label;
        private System.Windows.Forms.Label counters_Label;
        private System.Windows.Forms.Label category_Label;
        private System.Windows.Forms.Label server_Label;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolTip toolTipServer;
        private System.Windows.Forms.DataGridView selectedCounters_DataGridView;
        private System.Windows.Forms.ToolStrip counters_ToolStrip;
        private System.Windows.Forms.ToolStripButton addCounter_ToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteCounter_ToolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn instance;
        private System.Windows.Forms.DataGridViewTextBoxColumn counter;
        private System.Windows.Forms.DataGridViewTextBoxColumn interval;
    }
}
