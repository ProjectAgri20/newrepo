namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    public partial class GeneralSettingsControl
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
            this.ActivityTImeout_label = new System.Windows.Forms.Label();
            this.TimeZone_label = new System.Windows.Forms.Label();
            this.TimeSettings_groupBox = new System.Windows.Forms.GroupBox();
            this.sleep_Label = new System.Windows.Forms.Label();
            this.syncTime_label = new System.Windows.Forms.Label();
            this.port_Label = new System.Windows.Forms.Label();
            this.serverAddress_Label = new System.Windows.Forms.Label();
            this.AutoSyncTime_CheckBox = new System.Windows.Forms.CheckBox();
            this.CurrentTime_Label = new System.Windows.Forms.Label();
            this.CurrentDate_Label = new System.Windows.Forms.Label();
            this.defaultKeyLanguage_CheckBox = new System.Windows.Forms.CheckBox();
            this.ipAddress_Control = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceIPControl();
            this.timeZone_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.time_TimePicker = new HP.ScalableTest.Plugin.DeviceConfiguration.FieldControls.ChoiceTimePicker();
            this.date_TimePicker = new HP.ScalableTest.Plugin.DeviceConfiguration.FieldControls.ChoiceTimePicker();
            this.syncTime_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.sleepTimer_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.activityTimeout_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.port_TextBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.TimeSettings_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActivityTImeout_label
            // 
            this.ActivityTImeout_label.AutoSize = true;
            this.ActivityTImeout_label.Location = new System.Drawing.Point(12, 16);
            this.ActivityTImeout_label.Name = "ActivityTImeout_label";
            this.ActivityTImeout_label.Size = new System.Drawing.Size(130, 13);
            this.ActivityTImeout_label.TabIndex = 2;
            this.ActivityTImeout_label.Text = "Display Inactivity Timeout:";
            // 
            // TimeZone_label
            // 
            this.TimeZone_label.AutoSize = true;
            this.TimeZone_label.Location = new System.Drawing.Point(12, 71);
            this.TimeZone_label.Name = "TimeZone_label";
            this.TimeZone_label.Size = new System.Drawing.Size(61, 13);
            this.TimeZone_label.TabIndex = 3;
            this.TimeZone_label.Text = "Time Zone:";
            // 
            // TimeSettings_groupBox
            // 
            this.TimeSettings_groupBox.Controls.Add(this.ipAddress_Control);
            this.TimeSettings_groupBox.Controls.Add(this.timeZone_ComboBox);
            this.TimeSettings_groupBox.Controls.Add(this.time_TimePicker);
            this.TimeSettings_groupBox.Controls.Add(this.date_TimePicker);
            this.TimeSettings_groupBox.Controls.Add(this.syncTime_ComboBox);
            this.TimeSettings_groupBox.Controls.Add(this.sleepTimer_ComboBox);
            this.TimeSettings_groupBox.Controls.Add(this.activityTimeout_ComboBox);
            this.TimeSettings_groupBox.Controls.Add(this.port_TextBox);
            this.TimeSettings_groupBox.Controls.Add(this.sleep_Label);
            this.TimeSettings_groupBox.Controls.Add(this.syncTime_label);
            this.TimeSettings_groupBox.Controls.Add(this.port_Label);
            this.TimeSettings_groupBox.Controls.Add(this.serverAddress_Label);
            this.TimeSettings_groupBox.Controls.Add(this.AutoSyncTime_CheckBox);
            this.TimeSettings_groupBox.Controls.Add(this.CurrentTime_Label);
            this.TimeSettings_groupBox.Controls.Add(this.CurrentDate_Label);
            this.TimeSettings_groupBox.Controls.Add(this.TimeZone_label);
            this.TimeSettings_groupBox.Controls.Add(this.ActivityTImeout_label);
            this.TimeSettings_groupBox.Location = new System.Drawing.Point(23, 60);
            this.TimeSettings_groupBox.Name = "TimeSettings_groupBox";
            this.TimeSettings_groupBox.Size = new System.Drawing.Size(629, 341);
            this.TimeSettings_groupBox.TabIndex = 4;
            this.TimeSettings_groupBox.TabStop = false;
            this.TimeSettings_groupBox.Text = "Time Settings";
            this.TimeSettings_groupBox.Enter += new System.EventHandler(this.TimeSettings_groupBox_Enter);
            // 
            // sleep_Label
            // 
            this.sleep_Label.AutoSize = true;
            this.sleep_Label.Location = new System.Drawing.Point(12, 39);
            this.sleep_Label.Name = "sleep_Label";
            this.sleep_Label.Size = new System.Drawing.Size(66, 13);
            this.sleep_Label.TabIndex = 11;
            this.sleep_Label.Text = "Sleep Timer:";
            // 
            // syncTime_label
            // 
            this.syncTime_label.AutoSize = true;
            this.syncTime_label.Location = new System.Drawing.Point(15, 221);
            this.syncTime_label.Name = "syncTime_label";
            this.syncTime_label.Size = new System.Drawing.Size(124, 13);
            this.syncTime_label.TabIndex = 10;
            this.syncTime_label.Text = "Synchronize Time Every:";
            // 
            // port_Label
            // 
            this.port_Label.AutoSize = true;
            this.port_Label.Location = new System.Drawing.Point(15, 188);
            this.port_Label.Name = "port_Label";
            this.port_Label.Size = new System.Drawing.Size(98, 13);
            this.port_Label.TabIndex = 9;
            this.port_Label.Text = "Time Receive Port:";
            // 
            // serverAddress_Label
            // 
            this.serverAddress_Label.AutoSize = true;
            this.serverAddress_Label.Location = new System.Drawing.Point(15, 154);
            this.serverAddress_Label.Name = "serverAddress_Label";
            this.serverAddress_Label.Size = new System.Drawing.Size(82, 13);
            this.serverAddress_Label.TabIndex = 8;
            this.serverAddress_Label.Text = "Server Address:";
            // 
            // AutoSyncTime_CheckBox
            // 
            this.AutoSyncTime_CheckBox.AutoSize = true;
            this.AutoSyncTime_CheckBox.Location = new System.Drawing.Point(15, 122);
            this.AutoSyncTime_CheckBox.Name = "AutoSyncTime_CheckBox";
            this.AutoSyncTime_CheckBox.Size = new System.Drawing.Size(175, 17);
            this.AutoSyncTime_CheckBox.TabIndex = 7;
            this.AutoSyncTime_CheckBox.Text = "Sync Network Time with Server";
            this.AutoSyncTime_CheckBox.UseVisualStyleBackColor = true;
            // 
            // CurrentTime_Label
            // 
            this.CurrentTime_Label.AutoSize = true;
            this.CurrentTime_Label.Location = new System.Drawing.Point(12, 292);
            this.CurrentTime_Label.Name = "CurrentTime_Label";
            this.CurrentTime_Label.Size = new System.Drawing.Size(70, 13);
            this.CurrentTime_Label.TabIndex = 5;
            this.CurrentTime_Label.Text = "Current Time:";
            // 
            // CurrentDate_Label
            // 
            this.CurrentDate_Label.AutoSize = true;
            this.CurrentDate_Label.Location = new System.Drawing.Point(12, 263);
            this.CurrentDate_Label.Name = "CurrentDate_Label";
            this.CurrentDate_Label.Size = new System.Drawing.Size(70, 13);
            this.CurrentDate_Label.TabIndex = 4;
            this.CurrentDate_Label.Text = "Current Date:";
            // 
            // defaultKeyLanguage_CheckBox
            // 
            this.defaultKeyLanguage_CheckBox.AutoSize = true;
            this.defaultKeyLanguage_CheckBox.Location = new System.Drawing.Point(23, 25);
            this.defaultKeyLanguage_CheckBox.Name = "defaultKeyLanguage_CheckBox";
            this.defaultKeyLanguage_CheckBox.Size = new System.Drawing.Size(229, 17);
            this.defaultKeyLanguage_CheckBox.TabIndex = 18;
            this.defaultKeyLanguage_CheckBox.Text = "Set Default Language/Keyboard to English";
            this.defaultKeyLanguage_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ipAddress_Control
            // 
            this.ipAddress_Control.Location = new System.Drawing.Point(145, 150);
            this.ipAddress_Control.Name = "ipAddress_Control";
            this.ipAddress_Control.Size = new System.Drawing.Size(363, 27);
            this.ipAddress_Control.TabIndex = 28;
            // 
            // timeZone_ComboBox
            // 
            this.timeZone_ComboBox.Location = new System.Drawing.Point(139, 63);
            this.timeZone_ComboBox.Name = "timeZone_ComboBox";
            this.timeZone_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.timeZone_ComboBox.TabIndex = 27;
            // 
            // time_TimePicker
            // 
            this.time_TimePicker.Location = new System.Drawing.Point(145, 285);
            this.time_TimePicker.Name = "time_TimePicker";
            this.time_TimePicker.Size = new System.Drawing.Size(283, 28);
            this.time_TimePicker.TabIndex = 26;
            // 
            // date_TimePicker
            // 
            this.date_TimePicker.Location = new System.Drawing.Point(145, 257);
            this.date_TimePicker.Name = "date_TimePicker";
            this.date_TimePicker.Size = new System.Drawing.Size(283, 28);
            this.date_TimePicker.TabIndex = 25;
            // 
            // syncTime_ComboBox
            // 
            this.syncTime_ComboBox.Location = new System.Drawing.Point(145, 215);
            this.syncTime_ComboBox.Name = "syncTime_ComboBox";
            this.syncTime_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.syncTime_ComboBox.TabIndex = 23;
            // 
            // sleepTimer_ComboBox
            // 
            this.sleepTimer_ComboBox.Location = new System.Drawing.Point(139, 36);
            this.sleepTimer_ComboBox.Name = "sleepTimer_ComboBox";
            this.sleepTimer_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.sleepTimer_ComboBox.TabIndex = 21;
            // 
            // activityTimeout_ComboBox
            // 
            this.activityTimeout_ComboBox.Location = new System.Drawing.Point(139, 10);
            this.activityTimeout_ComboBox.Name = "activityTimeout_ComboBox";
            this.activityTimeout_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.activityTimeout_ComboBox.TabIndex = 20;
            // 
            // port_TextBox
            // 
            this.port_TextBox.Location = new System.Drawing.Point(145, 183);
            this.port_TextBox.Name = "port_TextBox";
            this.port_TextBox.Size = new System.Drawing.Size(361, 26);
            this.port_TextBox.TabIndex = 19;
            // 
            // GeneralSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.defaultKeyLanguage_CheckBox);
            this.Controls.Add(this.TimeSettings_groupBox);
            this.Name = "GeneralSettingsControl";
            this.Size = new System.Drawing.Size(655, 421);
            this.TimeSettings_groupBox.ResumeLayout(false);
            this.TimeSettings_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ActivityTImeout_label;
        private System.Windows.Forms.Label TimeZone_label;
        public System.Windows.Forms.GroupBox TimeSettings_groupBox;
        private System.Windows.Forms.Label CurrentTime_Label;
        private System.Windows.Forms.Label CurrentDate_Label;
        private System.Windows.Forms.Label sleep_Label;
        private System.Windows.Forms.Label syncTime_label;
        private System.Windows.Forms.Label port_Label;
        private System.Windows.Forms.Label serverAddress_Label;
        public System.Windows.Forms.CheckBox AutoSyncTime_CheckBox;
        public System.Windows.Forms.CheckBox defaultKeyLanguage_CheckBox;
        private ChoiceComboControl syncTime_ComboBox;
        private ChoiceComboControl sleepTimer_ComboBox;
        private ChoiceComboControl activityTimeout_ComboBox;
        private ChoiceTextControl port_TextBox;
        private FieldControls.ChoiceTimePicker time_TimePicker;
        private FieldControls.ChoiceTimePicker date_TimePicker;
        private ChoiceComboControl timeZone_ComboBox;
        private ChoiceIPControl ipAddress_Control;
    }
}
