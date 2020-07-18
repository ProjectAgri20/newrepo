namespace HP.ScalableTest.Plugin.DeviceUtility
{
    partial class DeviceUtilityConfigurationControl
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
            this.FieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.PluginBehaviorGroupBox = new System.Windows.Forms.GroupBox();
            this.LockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.groupBoxDeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.AssetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.ActionGroupBox = new System.Windows.Forms.GroupBox();
            this.PluginActionComboBox = new System.Windows.Forms.ComboBox();
            this.WaitForReadyCheckBox = new System.Windows.Forms.CheckBox();
            this.PaperlessModeAfterRebootComboBox = new System.Windows.Forms.ComboBox();
            this.PaperlessModeAfterRebootLabel = new System.Windows.Forms.Label();
            this.PluginBehaviorGroupBox.SuspendLayout();
            this.groupBoxDeviceConfiguration.SuspendLayout();
            this.ActionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PluginBehaviorGroupBox
            // 
            this.PluginBehaviorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PluginBehaviorGroupBox.Controls.Add(this.LockTimeoutControl);
            this.PluginBehaviorGroupBox.Location = new System.Drawing.Point(5, 434);
            this.PluginBehaviorGroupBox.Name = "PluginBehaviorGroupBox";
            this.PluginBehaviorGroupBox.Size = new System.Drawing.Size(662, 83);
            this.PluginBehaviorGroupBox.TabIndex = 4;
            this.PluginBehaviorGroupBox.TabStop = false;
            this.PluginBehaviorGroupBox.Text = "Plugin Behavior";
            // 
            // LockTimeoutControl
            // 
            this.LockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LockTimeoutControl.Location = new System.Drawing.Point(6, 20);
            this.LockTimeoutControl.Name = "LockTimeoutControl";
            this.LockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.LockTimeoutControl.TabIndex = 10;
            // 
            // groupBoxDeviceConfiguration
            // 
            this.groupBoxDeviceConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDeviceConfiguration.Controls.Add(this.AssetSelectionControl);
            this.groupBoxDeviceConfiguration.Location = new System.Drawing.Point(5, 90);
            this.groupBoxDeviceConfiguration.Name = "groupBoxDeviceConfiguration";
            this.groupBoxDeviceConfiguration.Size = new System.Drawing.Size(662, 341);
            this.groupBoxDeviceConfiguration.TabIndex = 3;
            this.groupBoxDeviceConfiguration.TabStop = false;
            this.groupBoxDeviceConfiguration.Text = "Devices";
            // 
            // AssetSelectionControl
            // 
            this.AssetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetSelectionControl.Location = new System.Drawing.Point(5, 15);
            this.AssetSelectionControl.Name = "AssetSelectionControl";
            this.AssetSelectionControl.Size = new System.Drawing.Size(650, 320);
            this.AssetSelectionControl.TabIndex = 0;
            // 
            // ActionGroupBox
            // 
            this.ActionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionGroupBox.Controls.Add(this.PaperlessModeAfterRebootLabel);
            this.ActionGroupBox.Controls.Add(this.PaperlessModeAfterRebootComboBox);
            this.ActionGroupBox.Controls.Add(this.PluginActionComboBox);
            this.ActionGroupBox.Controls.Add(this.WaitForReadyCheckBox);
            this.ActionGroupBox.Location = new System.Drawing.Point(5, 3);
            this.ActionGroupBox.Name = "ActionGroupBox";
            this.ActionGroupBox.Size = new System.Drawing.Size(662, 81);
            this.ActionGroupBox.TabIndex = 5;
            this.ActionGroupBox.TabStop = false;
            this.ActionGroupBox.Text = "Plugin Action";
            // 
            // PluginActionComboBox
            // 
            this.PluginActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PluginActionComboBox.FormattingEnabled = true;
            this.PluginActionComboBox.Location = new System.Drawing.Point(10, 32);
            this.PluginActionComboBox.Name = "PluginActionComboBox";
            this.PluginActionComboBox.Size = new System.Drawing.Size(326, 21);
            this.PluginActionComboBox.TabIndex = 13;
            // 
            // WaitForReadyCheckBox
            // 
            this.WaitForReadyCheckBox.AutoSize = true;
            this.WaitForReadyCheckBox.Checked = true;
            this.WaitForReadyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WaitForReadyCheckBox.Location = new System.Drawing.Point(361, 20);
            this.WaitForReadyCheckBox.Name = "WaitForReadyCheckBox";
            this.WaitForReadyCheckBox.Size = new System.Drawing.Size(286, 17);
            this.WaitForReadyCheckBox.TabIndex = 12;
            this.WaitForReadyCheckBox.Text = "Block Until Device Comes to Ready State After Reboot";
            this.WaitForReadyCheckBox.UseVisualStyleBackColor = true;
            // 
            // PaperlessModeAfterRebootComboBox
            // 
            this.PaperlessModeAfterRebootComboBox.FormattingEnabled = true;
            this.PaperlessModeAfterRebootComboBox.Location = new System.Drawing.Point(513, 43);
            this.PaperlessModeAfterRebootComboBox.Name = "PaperlessModeAfterRebootComboBox";
            this.PaperlessModeAfterRebootComboBox.Size = new System.Drawing.Size(134, 21);
            this.PaperlessModeAfterRebootComboBox.TabIndex = 14;
            // 
            // PaperlessModeAfterRebootLabel
            // 
            this.PaperlessModeAfterRebootLabel.AutoSize = true;
            this.PaperlessModeAfterRebootLabel.Location = new System.Drawing.Point(358, 46);
            this.PaperlessModeAfterRebootLabel.Name = "PaperlessModeAfterRebootLabel";
            this.PaperlessModeAfterRebootLabel.Size = new System.Drawing.Size(149, 13);
            this.PaperlessModeAfterRebootLabel.TabIndex = 15;
            this.PaperlessModeAfterRebootLabel.Text = "Paperless Mode After Reboot:";
            // 
            // DeviceUtilityConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ActionGroupBox);
            this.Controls.Add(this.PluginBehaviorGroupBox);
            this.Controls.Add(this.groupBoxDeviceConfiguration);
            this.Name = "DeviceUtilityConfigurationControl";
            this.Size = new System.Drawing.Size(671, 523);
            this.PluginBehaviorGroupBox.ResumeLayout(false);
            this.groupBoxDeviceConfiguration.ResumeLayout(false);
            this.ActionGroupBox.ResumeLayout(false);
            this.ActionGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Framework.UI.FieldValidator FieldValidator;
        private System.Windows.Forms.GroupBox PluginBehaviorGroupBox;
        private Framework.UI.LockTimeoutControl LockTimeoutControl;
        private System.Windows.Forms.GroupBox groupBoxDeviceConfiguration;
        private Framework.UI.AssetSelectionControl AssetSelectionControl;
        private System.Windows.Forms.GroupBox ActionGroupBox;
        private System.Windows.Forms.ComboBox PluginActionComboBox;
        private System.Windows.Forms.CheckBox WaitForReadyCheckBox;
        private System.Windows.Forms.Label PaperlessModeAfterRebootLabel;
        private System.Windows.Forms.ComboBox PaperlessModeAfterRebootComboBox;
    }
}
