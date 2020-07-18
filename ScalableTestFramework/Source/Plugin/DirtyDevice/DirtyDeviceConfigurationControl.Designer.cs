namespace HP.ScalableTest.Plugin.DirtyDevice
{
    partial class DirtyDeviceConfigurationControl
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
            this.PluginBehaviorGroupBox = new System.Windows.Forms.GroupBox();
            this.dirtyDeviceSettings = new DirtyDevice.Controls.DirtyDeviceSettings();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.groupBoxDeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.PluginBehaviorGroupBox.SuspendLayout();
            this.groupBoxDeviceConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // PluginBehaviorGroupBox
            // 
            this.PluginBehaviorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PluginBehaviorGroupBox.Controls.Add(this.lockTimeoutControl);
            this.PluginBehaviorGroupBox.Location = new System.Drawing.Point(3, 203);
            this.PluginBehaviorGroupBox.Name = "PluginBehaviorGroupBox";
            this.PluginBehaviorGroupBox.Size = new System.Drawing.Size(713, 78);
            this.PluginBehaviorGroupBox.TabIndex = 4;
            this.PluginBehaviorGroupBox.TabStop = false;
            this.PluginBehaviorGroupBox.Text = "Plugin Behavior";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 20);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 10;
            // 
            // groupBoxDeviceConfiguration
            // 
            this.groupBoxDeviceConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDeviceConfiguration.Controls.Add(this.assetSelectionControl);
            this.groupBoxDeviceConfiguration.Location = new System.Drawing.Point(5, 287);
            this.groupBoxDeviceConfiguration.Name = "groupBoxDeviceConfiguration";
            this.groupBoxDeviceConfiguration.Size = new System.Drawing.Size(711, 233);
            this.groupBoxDeviceConfiguration.TabIndex = 3;
            this.groupBoxDeviceConfiguration.TabStop = false;
            this.groupBoxDeviceConfiguration.Text = "Devices";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(5, 15);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(699, 212);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // dirtyDeviceSettings
            // 
            this.dirtyDeviceSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dirtyDeviceSettings.Location = new System.Drawing.Point(0, 3);
            this.dirtyDeviceSettings.Name = "pluginActionSettings1";
            this.dirtyDeviceSettings.Size = new System.Drawing.Size(716, 198);
            this.dirtyDeviceSettings.TabIndex = 11;
            // 
            // DirtyDeviceConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dirtyDeviceSettings);
            this.Controls.Add(this.PluginBehaviorGroupBox);
            this.Controls.Add(this.groupBoxDeviceConfiguration);
            this.Name = "DirtyDeviceConfigurationControl";
            this.Size = new System.Drawing.Size(720, 523);
            this.PluginBehaviorGroupBox.ResumeLayout(false);
            this.groupBoxDeviceConfiguration.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox PluginBehaviorGroupBox;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.GroupBox groupBoxDeviceConfiguration;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Controls.DirtyDeviceSettings dirtyDeviceSettings;
    }
}
