namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    partial class PrintQueueManagementConfigurationControl
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
            this.queue_groupBox = new System.Windows.Forms.GroupBox();
            this.document_groupBox = new System.Windows.Forms.GroupBox();
            this.document_label = new System.Windows.Forms.Label();
            this.document_comboBox = new System.Windows.Forms.ComboBox();
            this.driver_groupBox = new System.Windows.Forms.GroupBox();
            this.upgrade_printDriverSelectionControl = new HP.ScalableTest.Framework.UI.PrintDriverSelectionControl();
            this.localcache_checkBox = new System.Windows.Forms.CheckBox();
            this.addactivity_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.remove_button = new System.Windows.Forms.Button();
            this.movedown_button = new System.Windows.Forms.Button();
            this.tasks_dataGridView = new System.Windows.Forms.DataGridView();
            this.Activitycolumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moveup_button = new System.Windows.Forms.Button();
            this.printqueueoperations_groupBox = new System.Windows.Forms.GroupBox();
            this.canceldelay_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.defaultprinter_checkBox = new System.Windows.Forms.CheckBox();
            this.configure_button = new System.Windows.Forms.Button();
            this.configure_radioButton = new System.Windows.Forms.RadioButton();
            this.canceljob_radioButton = new System.Windows.Forms.RadioButton();
            this.uninstall_radioButton = new System.Windows.Forms.RadioButton();
            this.print_radioButton = new System.Windows.Forms.RadioButton();
            this.upgrade_radioButton = new System.Windows.Forms.RadioButton();
            this.install_radioButton = new System.Windows.Forms.RadioButton();
            this.device_groupBox = new System.Windows.Forms.GroupBox();
            this.device_textBox = new System.Windows.Forms.TextBox();
            this.printerSelect_button = new System.Windows.Forms.Button();
            this.pqm_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.pacing_timeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.pacing_groupBox = new System.Windows.Forms.GroupBox();
            this.delay_label = new System.Windows.Forms.Label();
            this.queue_groupBox.SuspendLayout();
            this.document_groupBox.SuspendLayout();
            this.driver_groupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tasks_dataGridView)).BeginInit();
            this.printqueueoperations_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canceldelay_numericUpDown)).BeginInit();
            this.device_groupBox.SuspendLayout();
            this.pacing_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // queue_groupBox
            // 
            this.queue_groupBox.Controls.Add(this.pacing_groupBox);
            this.queue_groupBox.Controls.Add(this.document_groupBox);
            this.queue_groupBox.Controls.Add(this.driver_groupBox);
            this.queue_groupBox.Controls.Add(this.addactivity_button);
            this.queue_groupBox.Controls.Add(this.panel1);
            this.queue_groupBox.Controls.Add(this.printqueueoperations_groupBox);
            this.queue_groupBox.Controls.Add(this.device_groupBox);
            this.queue_groupBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.queue_groupBox.Location = new System.Drawing.Point(0, 0);
            this.queue_groupBox.Name = "queue_groupBox";
            this.queue_groupBox.Size = new System.Drawing.Size(634, 495);
            this.queue_groupBox.TabIndex = 0;
            this.queue_groupBox.TabStop = false;
            this.queue_groupBox.Text = "Print Queue Management";
            // 
            // document_groupBox
            // 
            this.document_groupBox.Controls.Add(this.document_label);
            this.document_groupBox.Controls.Add(this.document_comboBox);
            this.document_groupBox.Location = new System.Drawing.Point(199, 175);
            this.document_groupBox.Name = "document_groupBox";
            this.document_groupBox.Size = new System.Drawing.Size(429, 54);
            this.document_groupBox.TabIndex = 5;
            this.document_groupBox.TabStop = false;
            this.document_groupBox.Text = "Print Document";
            // 
            // document_label
            // 
            this.document_label.AutoSize = true;
            this.document_label.Location = new System.Drawing.Point(28, 22);
            this.document_label.Name = "document_label";
            this.document_label.Size = new System.Drawing.Size(56, 13);
            this.document_label.TabIndex = 1;
            this.document_label.Text = "Document";
            // 
            // document_comboBox
            // 
            this.document_comboBox.Enabled = false;
            this.document_comboBox.FormattingEnabled = true;
            this.document_comboBox.Location = new System.Drawing.Point(99, 19);
            this.document_comboBox.Name = "document_comboBox";
            this.document_comboBox.Size = new System.Drawing.Size(322, 21);
            this.document_comboBox.TabIndex = 0;
            // 
            // driver_groupBox
            // 
            this.driver_groupBox.Controls.Add(this.upgrade_printDriverSelectionControl);
            this.driver_groupBox.Controls.Add(this.localcache_checkBox);
            this.driver_groupBox.Location = new System.Drawing.Point(199, 75);
            this.driver_groupBox.Name = "driver_groupBox";
            this.driver_groupBox.Size = new System.Drawing.Size(429, 94);
            this.driver_groupBox.TabIndex = 8;
            this.driver_groupBox.TabStop = false;
            this.driver_groupBox.Text = "Driver Upgrade";
            // 
            // upgrade_printDriverSelectionControl
            // 
            this.upgrade_printDriverSelectionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.upgrade_printDriverSelectionControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.upgrade_printDriverSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.upgrade_printDriverSelectionControl.Location = new System.Drawing.Point(6, 14);
            this.upgrade_printDriverSelectionControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upgrade_printDriverSelectionControl.Name = "upgrade_printDriverSelectionControl";
            this.upgrade_printDriverSelectionControl.Size = new System.Drawing.Size(415, 59);
            this.upgrade_printDriverSelectionControl.TabIndex = 10;
            // 
            // localcache_checkBox
            // 
            this.localcache_checkBox.AutoSize = true;
            this.localcache_checkBox.Checked = true;
            this.localcache_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localcache_checkBox.Location = new System.Drawing.Point(7, 74);
            this.localcache_checkBox.Name = "localcache_checkBox";
            this.localcache_checkBox.Size = new System.Drawing.Size(134, 17);
            this.localcache_checkBox.TabIndex = 9;
            this.localcache_checkBox.Text = "Install from local cache";
            this.localcache_checkBox.UseVisualStyleBackColor = true;
            this.localcache_checkBox.CheckedChanged += new System.EventHandler(this.localcache_checkBox_CheckedChanged);
            // 
            // addactivity_button
            // 
            this.addactivity_button.Location = new System.Drawing.Point(448, 246);
            this.addactivity_button.Name = "addactivity_button";
            this.addactivity_button.Size = new System.Drawing.Size(177, 45);
            this.addactivity_button.TabIndex = 7;
            this.addactivity_button.Text = "Add Activity";
            this.addactivity_button.UseVisualStyleBackColor = true;
            this.addactivity_button.Click += new System.EventHandler(this.addtask_button_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.remove_button);
            this.panel1.Controls.Add(this.movedown_button);
            this.panel1.Controls.Add(this.tasks_dataGridView);
            this.panel1.Controls.Add(this.moveup_button);
            this.panel1.Location = new System.Drawing.Point(15, 330);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(613, 156);
            this.panel1.TabIndex = 6;
            // 
            // remove_button
            // 
            this.remove_button.Location = new System.Drawing.Point(563, 99);
            this.remove_button.Name = "remove_button";
            this.remove_button.Size = new System.Drawing.Size(47, 30);
            this.remove_button.TabIndex = 2;
            this.remove_button.Text = "Del";
            this.remove_button.UseVisualStyleBackColor = true;
            this.remove_button.Click += new System.EventHandler(this.remove_button_Click);
            // 
            // movedown_button
            // 
            this.movedown_button.Location = new System.Drawing.Point(563, 63);
            this.movedown_button.Name = "movedown_button";
            this.movedown_button.Size = new System.Drawing.Size(47, 30);
            this.movedown_button.TabIndex = 1;
            this.movedown_button.Text = "Down";
            this.movedown_button.UseVisualStyleBackColor = true;
            this.movedown_button.Click += new System.EventHandler(this.movedown_button_Click);
            // 
            // tasks_dataGridView
            // 
            this.tasks_dataGridView.AllowUserToAddRows = false;
            this.tasks_dataGridView.AllowUserToDeleteRows = false;
            this.tasks_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tasks_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Activitycolumn,
            this.descriptionColumn});
            this.tasks_dataGridView.Location = new System.Drawing.Point(6, 6);
            this.tasks_dataGridView.MultiSelect = false;
            this.tasks_dataGridView.Name = "tasks_dataGridView";
            this.tasks_dataGridView.RowHeadersVisible = false;
            this.tasks_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tasks_dataGridView.Size = new System.Drawing.Size(551, 147);
            this.tasks_dataGridView.TabIndex = 0;
            this.tasks_dataGridView.Validating += new System.ComponentModel.CancelEventHandler(this.tasks_dataGridView_Validating);
            // 
            // Activitycolumn
            // 
            this.Activitycolumn.DataPropertyName = "Operation";
            this.Activitycolumn.HeaderText = "Activity";
            this.Activitycolumn.Name = "Activitycolumn";
            this.Activitycolumn.ReadOnly = true;
            this.Activitycolumn.Width = 160;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.DataPropertyName = "Description";
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.ReadOnly = true;
            this.descriptionColumn.Width = 380;
            // 
            // moveup_button
            // 
            this.moveup_button.Location = new System.Drawing.Point(563, 27);
            this.moveup_button.Name = "moveup_button";
            this.moveup_button.Size = new System.Drawing.Size(47, 30);
            this.moveup_button.TabIndex = 0;
            this.moveup_button.Text = "Up";
            this.moveup_button.UseVisualStyleBackColor = true;
            this.moveup_button.Click += new System.EventHandler(this.moveup_button_Click);
            // 
            // printqueueoperations_groupBox
            // 
            this.printqueueoperations_groupBox.Controls.Add(this.delay_label);
            this.printqueueoperations_groupBox.Controls.Add(this.canceldelay_numericUpDown);
            this.printqueueoperations_groupBox.Controls.Add(this.defaultprinter_checkBox);
            this.printqueueoperations_groupBox.Controls.Add(this.configure_button);
            this.printqueueoperations_groupBox.Controls.Add(this.configure_radioButton);
            this.printqueueoperations_groupBox.Controls.Add(this.canceljob_radioButton);
            this.printqueueoperations_groupBox.Controls.Add(this.uninstall_radioButton);
            this.printqueueoperations_groupBox.Controls.Add(this.print_radioButton);
            this.printqueueoperations_groupBox.Controls.Add(this.upgrade_radioButton);
            this.printqueueoperations_groupBox.Controls.Add(this.install_radioButton);
            this.printqueueoperations_groupBox.Location = new System.Drawing.Point(15, 75);
            this.printqueueoperations_groupBox.Name = "printqueueoperations_groupBox";
            this.printqueueoperations_groupBox.Size = new System.Drawing.Size(175, 249);
            this.printqueueoperations_groupBox.TabIndex = 3;
            this.printqueueoperations_groupBox.TabStop = false;
            this.printqueueoperations_groupBox.Text = "Print Queue Operations";
            // 
            // canceldelay_numericUpDown
            // 
            this.canceldelay_numericUpDown.Location = new System.Drawing.Point(110, 140);
            this.canceldelay_numericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.canceldelay_numericUpDown.Name = "canceldelay_numericUpDown";
            this.canceldelay_numericUpDown.Size = new System.Drawing.Size(39, 20);
            this.canceldelay_numericUpDown.TabIndex = 10;
            // 
            // defaultprinter_checkBox
            // 
            this.defaultprinter_checkBox.AutoSize = true;
            this.defaultprinter_checkBox.Location = new System.Drawing.Point(9, 166);
            this.defaultprinter_checkBox.Name = "defaultprinter_checkBox";
            this.defaultprinter_checkBox.Size = new System.Drawing.Size(123, 17);
            this.defaultprinter_checkBox.TabIndex = 10;
            this.defaultprinter_checkBox.Text = "Make Printer Default";
            this.defaultprinter_checkBox.UseVisualStyleBackColor = true;
            // 
            // configure_button
            // 
            this.configure_button.Location = new System.Drawing.Point(110, 41);
            this.configure_button.Name = "configure_button";
            this.configure_button.Size = new System.Drawing.Size(52, 23);
            this.configure_button.TabIndex = 10;
            this.configure_button.Text = "...";
            this.configure_button.UseVisualStyleBackColor = true;
            this.configure_button.Click += new System.EventHandler(this.configure_button_Click);
            // 
            // configure_radioButton
            // 
            this.configure_radioButton.AutoSize = true;
            this.configure_radioButton.Location = new System.Drawing.Point(9, 44);
            this.configure_radioButton.Name = "configure_radioButton";
            this.configure_radioButton.Size = new System.Drawing.Size(105, 17);
            this.configure_radioButton.TabIndex = 9;
            this.configure_radioButton.Text = "Configure Queue";
            this.configure_radioButton.UseVisualStyleBackColor = true;
            // 
            // canceljob_radioButton
            // 
            this.canceljob_radioButton.AutoSize = true;
            this.canceljob_radioButton.Location = new System.Drawing.Point(9, 140);
            this.canceljob_radioButton.Name = "canceljob_radioButton";
            this.canceljob_radioButton.Size = new System.Drawing.Size(102, 17);
            this.canceljob_radioButton.TabIndex = 8;
            this.canceljob_radioButton.Text = "Cancel Print Job";
            this.canceljob_radioButton.UseVisualStyleBackColor = true;
            this.canceljob_radioButton.CheckedChanged += new System.EventHandler(this.canceljob_radioButton_CheckedChanged);
            // 
            // uninstall_radioButton
            // 
            this.uninstall_radioButton.AutoSize = true;
            this.uninstall_radioButton.Location = new System.Drawing.Point(9, 92);
            this.uninstall_radioButton.Name = "uninstall_radioButton";
            this.uninstall_radioButton.Size = new System.Drawing.Size(100, 17);
            this.uninstall_radioButton.TabIndex = 5;
            this.uninstall_radioButton.Text = "Uninstall Queue";
            this.uninstall_radioButton.UseVisualStyleBackColor = true;
            // 
            // print_radioButton
            // 
            this.print_radioButton.AutoSize = true;
            this.print_radioButton.Location = new System.Drawing.Point(9, 116);
            this.print_radioButton.Name = "print_radioButton";
            this.print_radioButton.Size = new System.Drawing.Size(93, 17);
            this.print_radioButton.TabIndex = 7;
            this.print_radioButton.Text = "Print to Queue";
            this.print_radioButton.UseVisualStyleBackColor = true;
            this.print_radioButton.CheckedChanged += new System.EventHandler(this.print_radioButton_CheckedChanged);
            // 
            // upgrade_radioButton
            // 
            this.upgrade_radioButton.AutoSize = true;
            this.upgrade_radioButton.Location = new System.Drawing.Point(9, 68);
            this.upgrade_radioButton.Name = "upgrade_radioButton";
            this.upgrade_radioButton.Size = new System.Drawing.Size(101, 17);
            this.upgrade_radioButton.TabIndex = 4;
            this.upgrade_radioButton.Text = "Upgrade Queue";
            this.upgrade_radioButton.UseVisualStyleBackColor = true;
            // 
            // install_radioButton
            // 
            this.install_radioButton.AutoSize = true;
            this.install_radioButton.Checked = true;
            this.install_radioButton.Location = new System.Drawing.Point(9, 20);
            this.install_radioButton.Name = "install_radioButton";
            this.install_radioButton.Size = new System.Drawing.Size(87, 17);
            this.install_radioButton.TabIndex = 4;
            this.install_radioButton.TabStop = true;
            this.install_radioButton.Text = "Install Queue";
            this.install_radioButton.UseVisualStyleBackColor = true;
            // 
            // device_groupBox
            // 
            this.device_groupBox.Controls.Add(this.device_textBox);
            this.device_groupBox.Controls.Add(this.printerSelect_button);
            this.device_groupBox.Location = new System.Drawing.Point(15, 19);
            this.device_groupBox.Name = "device_groupBox";
            this.device_groupBox.Size = new System.Drawing.Size(610, 50);
            this.device_groupBox.TabIndex = 2;
            this.device_groupBox.TabStop = false;
            this.device_groupBox.Text = "Printer";
            // 
            // device_textBox
            // 
            this.device_textBox.Location = new System.Drawing.Point(6, 19);
            this.device_textBox.Name = "device_textBox";
            this.device_textBox.ReadOnly = true;
            this.device_textBox.Size = new System.Drawing.Size(337, 20);
            this.device_textBox.TabIndex = 0;
            // 
            // printerSelect_button
            // 
            this.printerSelect_button.Location = new System.Drawing.Point(349, 19);
            this.printerSelect_button.Name = "printerSelect_button";
            this.printerSelect_button.Size = new System.Drawing.Size(75, 23);
            this.printerSelect_button.TabIndex = 1;
            this.printerSelect_button.Text = "...";
            this.printerSelect_button.UseVisualStyleBackColor = true;
            this.printerSelect_button.Click += new System.EventHandler(this.printerSelect_Click);
            // 
            // pacing_timeSpanControl
            // 
            this.pacing_timeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pacing_timeSpanControl.Location = new System.Drawing.Point(7, 33);
            this.pacing_timeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.pacing_timeSpanControl.Name = "pacing_timeSpanControl";
            this.pacing_timeSpanControl.Size = new System.Drawing.Size(110, 23);
            this.pacing_timeSpanControl.TabIndex = 9;
            // 
            // pacing_groupBox
            // 
            this.pacing_groupBox.Controls.Add(this.pacing_timeSpanControl);
            this.pacing_groupBox.Location = new System.Drawing.Point(199, 235);
            this.pacing_groupBox.Name = "pacing_groupBox";
            this.pacing_groupBox.Size = new System.Drawing.Size(243, 89);
            this.pacing_groupBox.TabIndex = 10;
            this.pacing_groupBox.TabStop = false;
            this.pacing_groupBox.Text = "Activity Pacing";
            // 
            // delay_label
            // 
            this.delay_label.AutoSize = true;
            this.delay_label.Location = new System.Drawing.Point(151, 144);
            this.delay_label.Name = "delay_label";
            this.delay_label.Size = new System.Drawing.Size(20, 13);
            this.delay_label.TabIndex = 11;
            this.delay_label.Text = "ms";
            // 
            // PrintQueueManagementConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queue_groupBox);
            this.Name = "PrintQueueManagementConfigurationControl";
            this.Size = new System.Drawing.Size(634, 495);
            this.queue_groupBox.ResumeLayout(false);
            this.document_groupBox.ResumeLayout(false);
            this.document_groupBox.PerformLayout();
            this.driver_groupBox.ResumeLayout(false);
            this.driver_groupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tasks_dataGridView)).EndInit();
            this.printqueueoperations_groupBox.ResumeLayout(false);
            this.printqueueoperations_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canceldelay_numericUpDown)).EndInit();
            this.device_groupBox.ResumeLayout(false);
            this.device_groupBox.PerformLayout();
            this.pacing_groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox queue_groupBox;
        private System.Windows.Forms.GroupBox device_groupBox;
        private System.Windows.Forms.TextBox device_textBox;
        private System.Windows.Forms.Button printerSelect_button;
        private System.Windows.Forms.GroupBox document_groupBox;
        private System.Windows.Forms.ComboBox document_comboBox;
        private System.Windows.Forms.GroupBox printqueueoperations_groupBox;
        private System.Windows.Forms.RadioButton print_radioButton;
        private System.Windows.Forms.RadioButton uninstall_radioButton;
        private System.Windows.Forms.RadioButton upgrade_radioButton;
        private System.Windows.Forms.RadioButton install_radioButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView tasks_dataGridView;
        private System.Windows.Forms.Button addactivity_button;
        private System.Windows.Forms.GroupBox driver_groupBox;
        private System.Windows.Forms.Label document_label;
        private System.Windows.Forms.Button remove_button;
        private System.Windows.Forms.Button movedown_button;
        private System.Windows.Forms.Button moveup_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn Activitycolumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
        private System.Windows.Forms.CheckBox localcache_checkBox;
        private System.Windows.Forms.RadioButton canceljob_radioButton;
        private System.Windows.Forms.RadioButton configure_radioButton;
        private System.Windows.Forms.Button configure_button;
        private HP.ScalableTest.Framework.UI.PrintDriverSelectionControl upgrade_printDriverSelectionControl;
        private System.Windows.Forms.CheckBox defaultprinter_checkBox;
        private System.Windows.Forms.NumericUpDown canceldelay_numericUpDown;
        private Framework.UI.FieldValidator pqm_fieldValidator;
        private System.Windows.Forms.GroupBox pacing_groupBox;
        private Framework.UI.TimeSpanControl pacing_timeSpanControl;
        private System.Windows.Forms.Label delay_label;
    }
}
