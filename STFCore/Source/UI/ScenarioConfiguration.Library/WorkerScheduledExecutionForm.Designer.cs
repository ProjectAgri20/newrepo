namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class WorkerScheduledExecutionForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerScheduledExecutionForm));
            this.cancel_Button = new System.Windows.Forms.Button();
            this.schedule_DataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.workerExecutionScheduleItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ok_Button = new System.Windows.Forms.Button();
            this.duration_TextBox = new System.Windows.Forms.TextBox();
            this.duration_RadioButton = new System.Windows.Forms.RadioButton();
            this.iterationCount_RadioButton = new System.Windows.Forms.RadioButton();
            this.times_Label = new System.Windows.Forms.Label();
            this.repeat_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.hourMin_Label = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cumulativeTime_Label = new System.Windows.Forms.Label();
            this.cumulative_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.schedule_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerExecutionScheduleItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repeat_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(346, 450);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 1;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // schedule_DataGridView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.schedule_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.schedule_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schedule_DataGridView.AutoGenerateColumns = false;
            this.schedule_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.schedule_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewCheckBoxColumn1});
            this.schedule_DataGridView.DataSource = this.workerExecutionScheduleItemBindingSource;
            this.schedule_DataGridView.Location = new System.Drawing.Point(14, 14);
            this.schedule_DataGridView.Name = "schedule_DataGridView";
            this.schedule_DataGridView.RowHeadersWidth = 30;
            this.schedule_DataGridView.Size = new System.Drawing.Size(407, 332);
            this.schedule_DataGridView.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Mode";
            this.dataGridViewTextBoxColumn1.HeaderText = "Mode";
            this.dataGridViewTextBoxColumn1.Items.AddRange(new object[] {
            "Active",
            "Idle"});
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Days";
            this.dataGridViewTextBoxColumn2.HeaderText = "Days";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Hours";
            this.dataGridViewTextBoxColumn3.HeaderText = "Hours";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Minutes";
            this.dataGridViewTextBoxColumn4.HeaderText = "Minutes";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Stagger";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Stagger";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // workerExecutionScheduleItemBindingSource
            // 
            this.workerExecutionScheduleItemBindingSource.DataSource = typeof(HP.ScalableTest.Framework.Automation.ActivityExecution.ExecutionScheduleSegment);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(265, 450);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 3;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // duration_TextBox
            // 
            this.duration_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duration_TextBox.Enabled = false;
            this.duration_TextBox.Location = new System.Drawing.Point(161, 409);
            this.duration_TextBox.Name = "duration_TextBox";
            this.duration_TextBox.Size = new System.Drawing.Size(53, 23);
            this.duration_TextBox.TabIndex = 55;
            this.duration_TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.duration_TextBox_KeyDown);
            this.duration_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.duration_TextBox_Validating);
            // 
            // duration_RadioButton
            // 
            this.duration_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duration_RadioButton.AutoSize = true;
            this.duration_RadioButton.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.duration_RadioButton.Location = new System.Drawing.Point(14, 410);
            this.duration_RadioButton.Name = "duration_RadioButton";
            this.duration_RadioButton.Size = new System.Drawing.Size(134, 19);
            this.duration_RadioButton.TabIndex = 57;
            this.duration_RadioButton.TabStop = true;
            this.duration_RadioButton.Text = "Run the schedule for";
            this.duration_RadioButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.duration_RadioButton.UseVisualStyleBackColor = true;
            this.duration_RadioButton.CheckedChanged += new System.EventHandler(this.repeatCount_RadioButton_CheckedChanged);
            // 
            // iterationCount_RadioButton
            // 
            this.iterationCount_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.iterationCount_RadioButton.AutoSize = true;
            this.iterationCount_RadioButton.Checked = true;
            this.iterationCount_RadioButton.Location = new System.Drawing.Point(14, 380);
            this.iterationCount_RadioButton.Name = "iterationCount_RadioButton";
            this.iterationCount_RadioButton.Size = new System.Drawing.Size(130, 19);
            this.iterationCount_RadioButton.TabIndex = 56;
            this.iterationCount_RadioButton.TabStop = true;
            this.iterationCount_RadioButton.Tag = "";
            this.iterationCount_RadioButton.Text = "Iterate this schedule";
            this.iterationCount_RadioButton.UseVisualStyleBackColor = true;
            this.iterationCount_RadioButton.CheckedChanged += new System.EventHandler(this.repeatCount_RadioButton_CheckedChanged);
            // 
            // times_Label
            // 
            this.times_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.times_Label.AutoSize = true;
            this.times_Label.Location = new System.Drawing.Point(220, 382);
            this.times_Label.Name = "times_Label";
            this.times_Label.Size = new System.Drawing.Size(36, 15);
            this.times_Label.TabIndex = 58;
            this.times_Label.Text = "times";
            // 
            // repeat_NumericUpDown
            // 
            this.repeat_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.repeat_NumericUpDown.Location = new System.Drawing.Point(160, 380);
            this.repeat_NumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.repeat_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repeat_NumericUpDown.Name = "repeat_NumericUpDown";
            this.repeat_NumericUpDown.Size = new System.Drawing.Size(54, 23);
            this.repeat_NumericUpDown.TabIndex = 60;
            this.repeat_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // hourMin_Label
            // 
            this.hourMin_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hourMin_Label.AutoSize = true;
            this.hourMin_Label.Location = new System.Drawing.Point(220, 412);
            this.hourMin_Label.Name = "hourMin_Label";
            this.hourMin_Label.Size = new System.Drawing.Size(54, 15);
            this.hourMin_Label.TabIndex = 59;
            this.hourMin_Label.Text = "(hh:mm)";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider.Icon")));
            // 
            // cumulativeTime_Label
            // 
            this.cumulativeTime_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cumulativeTime_Label.Location = new System.Drawing.Point(321, 353);
            this.cumulativeTime_Label.Name = "cumulativeTime_Label";
            this.cumulativeTime_Label.Size = new System.Drawing.Size(100, 15);
            this.cumulativeTime_Label.TabIndex = 62;
            this.cumulativeTime_Label.Text = "(hh:mm)";
            this.cumulativeTime_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cumulative_Label
            // 
            this.cumulative_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cumulative_Label.AutoSize = true;
            this.cumulative_Label.Location = new System.Drawing.Point(262, 353);
            this.cumulative_Label.Name = "cumulative_Label";
            this.cumulative_Label.Size = new System.Drawing.Size(66, 15);
            this.cumulative_Label.TabIndex = 63;
            this.cumulative_Label.Text = "Total Time:";
            // 
            // WorkerScheduledExecutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(435, 485);
            this.Controls.Add(this.cumulative_Label);
            this.Controls.Add(this.cumulativeTime_Label);
            this.Controls.Add(this.repeat_NumericUpDown);
            this.Controls.Add(this.hourMin_Label);
            this.Controls.Add(this.times_Label);
            this.Controls.Add(this.duration_TextBox);
            this.Controls.Add(this.duration_RadioButton);
            this.Controls.Add(this.iterationCount_RadioButton);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.schedule_DataGridView);
            this.Controls.Add(this.cancel_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Name = "WorkerScheduledExecutionForm";
            this.Text = "Scheduled Execution";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.ScheduledExecutionConfigurationForm_HelpButtonClicked);
            this.Load += new System.EventHandler(this.ScheduledExecutionConfigurationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.schedule_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerExecutionScheduleItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repeat_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.DataGridView schedule_DataGridView;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.TextBox duration_TextBox;
        private System.Windows.Forms.RadioButton duration_RadioButton;
        private System.Windows.Forms.RadioButton iterationCount_RadioButton;
        private System.Windows.Forms.Label times_Label;
        private System.Windows.Forms.BindingSource workerExecutionScheduleItemBindingSource;
        private System.Windows.Forms.NumericUpDown repeat_NumericUpDown;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Label hourMin_Label;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label cumulativeTime_Label;
        private System.Windows.Forms.Label cumulative_Label;
    }
}