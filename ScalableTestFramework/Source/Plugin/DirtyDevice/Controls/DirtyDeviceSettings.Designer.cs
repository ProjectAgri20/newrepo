namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    partial class DirtyDeviceSettings
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
            HP.ScalableTest.Plugin.DirtyDevice.DigitalSendOutputFolderActivityData digitalSendOutputFolderActivityData1 = new HP.ScalableTest.Plugin.DirtyDevice.DigitalSendOutputFolderActivityData();
            HP.ScalableTest.Plugin.DirtyDevice.QuickSetActivityData quickSetActivityData1 = new HP.ScalableTest.Plugin.DirtyDevice.QuickSetActivityData();
            this.ActionSettingsTabControl = new System.Windows.Forms.TabControl();
            this.DigitalSendNetworkFolderTabPage = new System.Windows.Forms.TabPage();
            this.digitalSendOutputFolderSettings1 = new HP.ScalableTest.Plugin.DirtyDevice.Controls.DigitalSendOutputFolderSettings();
            this.ewsSettings1 = new HP.ScalableTest.Plugin.DirtyDevice.Controls.EwsSettings();
            this.pluginActions1 = new HP.ScalableTest.Plugin.DirtyDevice.Controls.PluginActionsPicker();
            this.ActionSettingsTabControl.SuspendLayout();
            this.DigitalSendNetworkFolderTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionSettingsTabControl
            // 
            this.ActionSettingsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionSettingsTabControl.Controls.Add(this.DigitalSendNetworkFolderTabPage);
            this.ActionSettingsTabControl.Location = new System.Drawing.Point(263, 3);
            this.ActionSettingsTabControl.Name = "ActionSettingsTabControl";
            this.ActionSettingsTabControl.SelectedIndex = 0;
            this.ActionSettingsTabControl.Size = new System.Drawing.Size(446, 214);
            this.ActionSettingsTabControl.TabIndex = 1;
            // 
            // DigitalSendNetworkFolderTabPage
            // 
            this.DigitalSendNetworkFolderTabPage.Controls.Add(this.digitalSendOutputFolderSettings1);
            this.DigitalSendNetworkFolderTabPage.Controls.Add(this.ewsSettings1);
            this.DigitalSendNetworkFolderTabPage.Location = new System.Drawing.Point(4, 22);
            this.DigitalSendNetworkFolderTabPage.Name = "DigitalSendNetworkFolderTabPage";
            this.DigitalSendNetworkFolderTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DigitalSendNetworkFolderTabPage.Size = new System.Drawing.Size(438, 188);
            this.DigitalSendNetworkFolderTabPage.TabIndex = 0;
            this.DigitalSendNetworkFolderTabPage.Text = "Digital Send / Network Folder";
            this.DigitalSendNetworkFolderTabPage.UseVisualStyleBackColor = true;
            // 
            // digitalSendOutputFolderSettings1
            // 
            this.digitalSendOutputFolderSettings1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.digitalSendOutputFolderSettings1.Location = new System.Drawing.Point(6, 6);
            this.digitalSendOutputFolderSettings1.Name = "digitalSendOutputFolderSettings1";
            this.digitalSendOutputFolderSettings1.Size = new System.Drawing.Size(426, 24);
            this.digitalSendOutputFolderSettings1.TabIndex = 4;
            digitalSendOutputFolderActivityData1.OutputFolder = null;
            this.digitalSendOutputFolderSettings1.Value = digitalSendOutputFolderActivityData1;
            // 
            // ewsSettings1
            // 
            this.ewsSettings1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ewsSettings1.Location = new System.Drawing.Point(6, 36);
            this.ewsSettings1.Name = "ewsSettings1";
            this.ewsSettings1.Size = new System.Drawing.Size(426, 25);
            this.ewsSettings1.TabIndex = 3;
            quickSetActivityData1.QuickSetTitle = "Dirty Ews";
            this.ewsSettings1.Value = quickSetActivityData1;
            // 
            // pluginActions1
            // 
            this.pluginActions1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pluginActions1.Location = new System.Drawing.Point(0, 0);
            this.pluginActions1.Name = "pluginActions1";
            this.pluginActions1.Size = new System.Drawing.Size(257, 218);
            this.pluginActions1.TabIndex = 0;
            this.pluginActions1.Value = HP.ScalableTest.Plugin.DirtyDevice.DirtyDeviceActionFlags.None;
            // 
            // DirtyDeviceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ActionSettingsTabControl);
            this.Controls.Add(this.pluginActions1);
            this.Name = "DirtyDeviceSettings";
            this.Size = new System.Drawing.Size(712, 221);
            this.ActionSettingsTabControl.ResumeLayout(false);
            this.DigitalSendNetworkFolderTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private PluginActionsPicker pluginActions1;
        private System.Windows.Forms.TabControl ActionSettingsTabControl;
        private System.Windows.Forms.TabPage DigitalSendNetworkFolderTabPage;
        private EwsSettings ewsSettings1;
        private DigitalSendOutputFolderSettings digitalSendOutputFolderSettings1;
    }
}
