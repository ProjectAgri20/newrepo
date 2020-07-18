namespace HP.ScalableTest.Plugin.DSSInstall
{
    partial class DSSInstallConfigurationControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.textBoxSetupFile = new System.Windows.Forms.TextBox();
            this.browse_button = new System.Windows.Forms.Button();
            this.CustomLocation_checkBox = new System.Windows.Forms.CheckBox();
            this.textBoxCustomLocation = new System.Windows.Forms.TextBox();
            this.browselocation_button = new System.Windows.Forms.Button();
            this.installoption_groupBox = new System.Windows.Forms.GroupBox();
            this.uninstall_radioButton = new System.Windows.Forms.RadioButton();
            this.fullinstall_radioButton = new System.Windows.Forms.RadioButton();
            this.configutil_radioButton = new System.Windows.Forms.RadioButton();
            this.installfile_groupBox = new System.Windows.Forms.GroupBox();
            this.setuppath_label = new System.Windows.Forms.Label();
            this.optional_groupBox = new System.Windows.Forms.GroupBox();
            this.savesettings_checkBox = new System.Windows.Forms.CheckBox();
            this.launch_checkBox = new System.Windows.Forms.CheckBox();
            this.readme_checkBox = new System.Windows.Forms.CheckBox();
            this.transitiondelay_label = new System.Windows.Forms.Label();
            this.note_textBox = new System.Windows.Forms.TextBox();
            this.delay_label = new System.Windows.Forms.Label();
            this.delay_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.cancel_checkBox = new System.Windows.Forms.CheckBox();
            this.validate_checkBox = new System.Windows.Forms.CheckBox();
            this.dss_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.installoption_groupBox.SuspendLayout();
            this.installfile_groupBox.SuspendLayout();
            this.optional_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delay_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxSetupFile
            // 
            this.textBoxSetupFile.BackColor = System.Drawing.Color.White;
            this.textBoxSetupFile.Location = new System.Drawing.Point(15, 41);
            this.textBoxSetupFile.Name = "textBoxSetupFile";
            this.textBoxSetupFile.ReadOnly = true;
            this.textBoxSetupFile.Size = new System.Drawing.Size(527, 23);
            this.textBoxSetupFile.TabIndex = 3;
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(548, 40);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(67, 23);
            this.browse_button.TabIndex = 4;
            this.browse_button.Text = "...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // CustomLocation_checkBox
            // 
            this.CustomLocation_checkBox.AutoSize = true;
            this.CustomLocation_checkBox.Location = new System.Drawing.Point(15, 87);
            this.CustomLocation_checkBox.Name = "CustomLocation_checkBox";
            this.CustomLocation_checkBox.Size = new System.Drawing.Size(117, 19);
            this.CustomLocation_checkBox.TabIndex = 6;
            this.CustomLocation_checkBox.Text = "Custom Location";
            this.dss_toolTip.SetToolTip(this.CustomLocation_checkBox, "Choose custom location");
            this.CustomLocation_checkBox.UseVisualStyleBackColor = true;
            // 
            // textBoxCustomLocation
            // 
            this.textBoxCustomLocation.BackColor = System.Drawing.Color.White;
            this.textBoxCustomLocation.Location = new System.Drawing.Point(15, 112);
            this.textBoxCustomLocation.Name = "textBoxCustomLocation";
            this.textBoxCustomLocation.ReadOnly = true;
            this.textBoxCustomLocation.Size = new System.Drawing.Size(527, 23);
            this.textBoxCustomLocation.TabIndex = 7;
            // 
            // browselocation_button
            // 
            this.browselocation_button.Location = new System.Drawing.Point(548, 111);
            this.browselocation_button.Name = "browselocation_button";
            this.browselocation_button.Size = new System.Drawing.Size(67, 23);
            this.browselocation_button.TabIndex = 8;
            this.browselocation_button.Text = "...";
            this.browselocation_button.UseVisualStyleBackColor = true;
            this.browselocation_button.Click += new System.EventHandler(this.buttonBrowseLocation_Click);
            // 
            // installoption_groupBox
            // 
            this.installoption_groupBox.Controls.Add(this.uninstall_radioButton);
            this.installoption_groupBox.Controls.Add(this.fullinstall_radioButton);
            this.installoption_groupBox.Controls.Add(this.configutil_radioButton);
            this.installoption_groupBox.Location = new System.Drawing.Point(24, 181);
            this.installoption_groupBox.Name = "installoption_groupBox";
            this.installoption_groupBox.Size = new System.Drawing.Size(200, 125);
            this.installoption_groupBox.TabIndex = 9;
            this.installoption_groupBox.TabStop = false;
            this.installoption_groupBox.Text = "Installation Options";
            // 
            // uninstall_radioButton
            // 
            this.uninstall_radioButton.AutoSize = true;
            this.uninstall_radioButton.Location = new System.Drawing.Point(6, 81);
            this.uninstall_radioButton.Name = "uninstall_radioButton";
            this.uninstall_radioButton.Size = new System.Drawing.Size(71, 19);
            this.uninstall_radioButton.TabIndex = 11;
            this.uninstall_radioButton.TabStop = true;
            this.uninstall_radioButton.Tag = "3";
            this.uninstall_radioButton.Text = "Uninstall";
            this.dss_toolTip.SetToolTip(this.uninstall_radioButton, "Uninstall the software");
            this.uninstall_radioButton.UseVisualStyleBackColor = true;
            // 
            // fullinstall_radioButton
            // 
            this.fullinstall_radioButton.AutoSize = true;
            this.fullinstall_radioButton.Checked = true;
            this.fullinstall_radioButton.Location = new System.Drawing.Point(6, 31);
            this.fullinstall_radioButton.Name = "fullinstall_radioButton";
            this.fullinstall_radioButton.Size = new System.Drawing.Size(78, 19);
            this.fullinstall_radioButton.TabIndex = 10;
            this.fullinstall_radioButton.TabStop = true;
            this.fullinstall_radioButton.Tag = "1";
            this.fullinstall_radioButton.Text = "Full Install";
            this.dss_toolTip.SetToolTip(this.fullinstall_radioButton, "Perform a full installation");
            this.fullinstall_radioButton.UseVisualStyleBackColor = true;
            // 
            // configutil_radioButton
            // 
            this.configutil_radioButton.AutoSize = true;
            this.configutil_radioButton.Location = new System.Drawing.Point(6, 56);
            this.configutil_radioButton.Name = "configutil_radioButton";
            this.configutil_radioButton.Size = new System.Drawing.Size(161, 19);
            this.configutil_radioButton.TabIndex = 0;
            this.configutil_radioButton.TabStop = true;
            this.configutil_radioButton.Tag = "2";
            this.configutil_radioButton.Text = "Configuration Utility Only";
            this.dss_toolTip.SetToolTip(this.configutil_radioButton, "Install only configuration utility");
            this.configutil_radioButton.UseVisualStyleBackColor = true;
            // 
            // installfile_groupBox
            // 
            this.installfile_groupBox.Controls.Add(this.setuppath_label);
            this.installfile_groupBox.Controls.Add(this.textBoxSetupFile);
            this.installfile_groupBox.Controls.Add(this.browse_button);
            this.installfile_groupBox.Controls.Add(this.CustomLocation_checkBox);
            this.installfile_groupBox.Controls.Add(this.browselocation_button);
            this.installfile_groupBox.Controls.Add(this.textBoxCustomLocation);
            this.installfile_groupBox.Location = new System.Drawing.Point(24, 20);
            this.installfile_groupBox.Name = "installfile_groupBox";
            this.installfile_groupBox.Size = new System.Drawing.Size(638, 141);
            this.installfile_groupBox.TabIndex = 10;
            this.installfile_groupBox.TabStop = false;
            this.installfile_groupBox.Text = "Setup Options";
            // 
            // setuppath_label
            // 
            this.setuppath_label.AutoSize = true;
            this.setuppath_label.Location = new System.Drawing.Point(12, 23);
            this.setuppath_label.Name = "setuppath_label";
            this.setuppath_label.Size = new System.Drawing.Size(88, 15);
            this.setuppath_label.TabIndex = 9;
            this.setuppath_label.Text = "Setup File Path:";
            // 
            // optional_groupBox
            // 
            this.optional_groupBox.Controls.Add(this.savesettings_checkBox);
            this.optional_groupBox.Controls.Add(this.launch_checkBox);
            this.optional_groupBox.Controls.Add(this.readme_checkBox);
            this.optional_groupBox.Controls.Add(this.transitiondelay_label);
            this.optional_groupBox.Controls.Add(this.note_textBox);
            this.optional_groupBox.Controls.Add(this.delay_label);
            this.optional_groupBox.Controls.Add(this.delay_numericUpDown);
            this.optional_groupBox.Controls.Add(this.cancel_checkBox);
            this.optional_groupBox.Controls.Add(this.validate_checkBox);
            this.optional_groupBox.Location = new System.Drawing.Point(241, 181);
            this.optional_groupBox.Name = "optional_groupBox";
            this.optional_groupBox.Size = new System.Drawing.Size(325, 194);
            this.optional_groupBox.TabIndex = 11;
            this.optional_groupBox.TabStop = false;
            this.optional_groupBox.Text = "Optional";
            // 
            // savesettings_checkBox
            // 
            this.savesettings_checkBox.AutoSize = true;
            this.savesettings_checkBox.Checked = true;
            this.savesettings_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.savesettings_checkBox.Location = new System.Drawing.Point(159, 57);
            this.savesettings_checkBox.Name = "savesettings_checkBox";
            this.savesettings_checkBox.Size = new System.Drawing.Size(159, 19);
            this.savesettings_checkBox.TabIndex = 9;
            this.savesettings_checkBox.Text = "Save Settings on upgrade";
            this.dss_toolTip.SetToolTip(this.savesettings_checkBox, "Saves current settings when upgraded");
            this.savesettings_checkBox.UseVisualStyleBackColor = true;
            // 
            // launch_checkBox
            // 
            this.launch_checkBox.AutoSize = true;
            this.launch_checkBox.Location = new System.Drawing.Point(6, 82);
            this.launch_checkBox.Name = "launch_checkBox";
            this.launch_checkBox.Size = new System.Drawing.Size(88, 19);
            this.launch_checkBox.TabIndex = 8;
            this.launch_checkBox.Text = "Launch DSS";
            this.dss_toolTip.SetToolTip(this.launch_checkBox, "Selects the option to launch DSS on the final screen");
            this.launch_checkBox.UseVisualStyleBackColor = true;
            // 
            // readme_checkBox
            // 
            this.readme_checkBox.AutoSize = true;
            this.readme_checkBox.Location = new System.Drawing.Point(159, 31);
            this.readme_checkBox.Name = "readme_checkBox";
            this.readme_checkBox.Size = new System.Drawing.Size(97, 19);
            this.readme_checkBox.TabIndex = 7;
            this.readme_checkBox.Text = "View Readme";
            this.dss_toolTip.SetToolTip(this.readme_checkBox, "Selects the option to view readme on the final screen");
            this.readme_checkBox.UseVisualStyleBackColor = true;
            // 
            // transitiondelay_label
            // 
            this.transitiondelay_label.AutoSize = true;
            this.transitiondelay_label.Location = new System.Drawing.Point(6, 110);
            this.transitiondelay_label.Name = "transitiondelay_label";
            this.transitiondelay_label.Size = new System.Drawing.Size(91, 15);
            this.transitiondelay_label.TabIndex = 6;
            this.transitiondelay_label.Text = "Transition Delay";
            // 
            // note_textBox
            // 
            this.note_textBox.Location = new System.Drawing.Point(6, 134);
            this.note_textBox.Multiline = true;
            this.note_textBox.Name = "note_textBox";
            this.note_textBox.ReadOnly = true;
            this.note_textBox.Size = new System.Drawing.Size(252, 54);
            this.note_textBox.TabIndex = 5;
            this.note_textBox.Text = "NOTE: If delay is set to maximum (20) then an idle time of two hours is observed " +
    "before commencing the installation.";
            // 
            // delay_label
            // 
            this.delay_label.AutoSize = true;
            this.delay_label.Location = new System.Drawing.Point(181, 110);
            this.delay_label.Name = "delay_label";
            this.delay_label.Size = new System.Drawing.Size(25, 15);
            this.delay_label.TabIndex = 4;
            this.delay_label.Text = "Sec";
            // 
            // delay_numericUpDown
            // 
            this.delay_numericUpDown.Location = new System.Drawing.Point(103, 105);
            this.delay_numericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.delay_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.delay_numericUpDown.Name = "delay_numericUpDown";
            this.delay_numericUpDown.Size = new System.Drawing.Size(72, 23);
            this.delay_numericUpDown.TabIndex = 3;
            this.dss_toolTip.SetToolTip(this.delay_numericUpDown, "time taken before proceeding to next screen, if it is set to 20 seconds then time" +
        " taken is 2 hours before starting the installation");
            this.delay_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cancel_checkBox
            // 
            this.cancel_checkBox.AutoSize = true;
            this.cancel_checkBox.Location = new System.Drawing.Point(6, 57);
            this.cancel_checkBox.Name = "cancel_checkBox";
            this.cancel_checkBox.Size = new System.Drawing.Size(151, 19);
            this.cancel_checkBox.TabIndex = 1;
            this.cancel_checkBox.Text = "Cancel Setup (Random)";
            this.dss_toolTip.SetToolTip(this.cancel_checkBox, "Cancels the setup at random screen");
            this.cancel_checkBox.UseVisualStyleBackColor = true;
            // 
            // validate_checkBox
            // 
            this.validate_checkBox.AutoSize = true;
            this.validate_checkBox.Location = new System.Drawing.Point(6, 31);
            this.validate_checkBox.Name = "validate_checkBox";
            this.validate_checkBox.Size = new System.Drawing.Size(100, 19);
            this.validate_checkBox.TabIndex = 0;
            this.validate_checkBox.Text = "Validate Setup";
            this.dss_toolTip.SetToolTip(this.validate_checkBox, "Validates the install or uninstall process");
            this.validate_checkBox.UseVisualStyleBackColor = true;
            // 
            // DSSInstallConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optional_groupBox);
            this.Controls.Add(this.installfile_groupBox);
            this.Controls.Add(this.installoption_groupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DSSInstallConfigurationControl";
            this.Size = new System.Drawing.Size(728, 435);
            this.installoption_groupBox.ResumeLayout(false);
            this.installoption_groupBox.PerformLayout();
            this.installfile_groupBox.ResumeLayout(false);
            this.installfile_groupBox.PerformLayout();
            this.optional_groupBox.ResumeLayout(false);
            this.optional_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delay_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TextBox textBoxSetupFile;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.CheckBox CustomLocation_checkBox;
        private System.Windows.Forms.TextBox textBoxCustomLocation;
        private System.Windows.Forms.Button browselocation_button;
        private System.Windows.Forms.GroupBox installoption_groupBox;
        private System.Windows.Forms.RadioButton uninstall_radioButton;
        private System.Windows.Forms.RadioButton fullinstall_radioButton;
        private System.Windows.Forms.RadioButton configutil_radioButton;
        private System.Windows.Forms.GroupBox installfile_groupBox;
        private System.Windows.Forms.Label setuppath_label;
        private System.Windows.Forms.GroupBox optional_groupBox;
        private System.Windows.Forms.TextBox note_textBox;
        private System.Windows.Forms.Label delay_label;
        private System.Windows.Forms.NumericUpDown delay_numericUpDown;
        private System.Windows.Forms.CheckBox cancel_checkBox;
        private System.Windows.Forms.CheckBox validate_checkBox;
        private System.Windows.Forms.Label transitiondelay_label;
        private System.Windows.Forms.CheckBox launch_checkBox;
        private System.Windows.Forms.CheckBox readme_checkBox;
        private System.Windows.Forms.CheckBox savesettings_checkBox;
        private System.Windows.Forms.ToolTip dss_toolTip;
    }
}
