using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.CertificateManagement
{
    /// <summary>
    /// Edit control for CertificateManagement plug-in
    /// </summary>
    partial class CertificateManagementConfigurationControl
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
            this.radiusServerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.rootSha2_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.rootSha2Ip_Label = new System.Windows.Forms.Label();
            this.rootSha1_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.rootSha1Ip_Label = new System.Windows.Forms.Label();
            this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.serpentPortNumber_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.europaPortNumber_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.wireless_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.serpentine_lbl = new System.Windows.Forms.Label();
            this.secondaryWired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.europaWiredIP_Label = new System.Windows.Forms.Label();
            this.primaryWired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.printerIp_Label = new System.Windows.Forms.Label();
            this.sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.printDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.radiusServerDetails_GroupBox.SuspendLayout();
            this.printerDetails_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serpentPortNumber_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.europaPortNumber_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // testCaseDetails_GroupBox
            // 
            this.testCaseDetails_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testCaseDetails_GroupBox.Size = new System.Drawing.Size(718, 338);
            // 
            // radiusServerDetails_GroupBox
            // 
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2Ip_Label);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1Ip_Label);
            this.radiusServerDetails_GroupBox.Location = new System.Drawing.Point(10, 551);
            this.radiusServerDetails_GroupBox.Name = "radiusServerDetails_GroupBox";
            this.radiusServerDetails_GroupBox.Size = new System.Drawing.Size(346, 90);
            this.radiusServerDetails_GroupBox.TabIndex = 50;
            this.radiusServerDetails_GroupBox.TabStop = false;
            this.radiusServerDetails_GroupBox.Text = "Radius Servers";
            // 
            // rootSha2_IpAddressControl
            // 
            this.rootSha2_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha2_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha2_IpAddressControl.Enabled = false;
            this.rootSha2_IpAddressControl.Location = new System.Drawing.Point(138, 56);
            this.rootSha2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha2_IpAddressControl.Name = "rootSha2_IpAddressControl";
            this.rootSha2_IpAddressControl.Size = new System.Drawing.Size(190, 20);
            this.rootSha2_IpAddressControl.TabIndex = 4;
            this.rootSha2_IpAddressControl.Tag = "Root SHA2 Server IP";
            this.rootSha2_IpAddressControl.Text = "...";
            // 
            // rootSha2Ip_Label
            // 
            this.rootSha2Ip_Label.AutoSize = true;
            this.rootSha2Ip_Label.Location = new System.Drawing.Point(20, 59);
            this.rootSha2Ip_Label.Name = "rootSha2Ip_Label";
            this.rootSha2Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha2Ip_Label.TabIndex = 59;
            this.rootSha2Ip_Label.Text = "Root SHA2 Server IP:";
            // 
            // rootSha1_IpAddressControl
            // 
            this.rootSha1_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha1_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha1_IpAddressControl.Location = new System.Drawing.Point(138, 29);
            this.rootSha1_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha1_IpAddressControl.Name = "rootSha1_IpAddressControl";
            this.rootSha1_IpAddressControl.Size = new System.Drawing.Size(190, 20);
            this.rootSha1_IpAddressControl.TabIndex = 3;
            this.rootSha1_IpAddressControl.Tag = "Root SHA1 Server IP";
            this.rootSha1_IpAddressControl.Text = "...";
            // 
            // rootSha1Ip_Label
            // 
            this.rootSha1Ip_Label.AutoSize = true;
            this.rootSha1Ip_Label.Location = new System.Drawing.Point(20, 32);
            this.rootSha1Ip_Label.Name = "rootSha1Ip_Label";
            this.rootSha1Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha1Ip_Label.TabIndex = 57;
            this.rootSha1Ip_Label.Text = "Root SHA1 Server IP:";
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.serpentPortNumber_NumericUpDown);
            this.printerDetails_GroupBox.Controls.Add(this.europaPortNumber_NumericUpDown);
            this.printerDetails_GroupBox.Controls.Add(this.wireless_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.serpentine_lbl);
            this.printerDetails_GroupBox.Controls.Add(this.secondaryWired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.europaWiredIP_Label);
            this.printerDetails_GroupBox.Controls.Add(this.primaryWired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.printerIp_Label);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(10, 410);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(346, 128);
            this.printerDetails_GroupBox.TabIndex = 52;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // serpentPortNumber_NumericUpDown
            // 
            this.serpentPortNumber_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serpentPortNumber_NumericUpDown.AutoSize = true;
            this.serpentPortNumber_NumericUpDown.Enabled = false;
            this.serpentPortNumber_NumericUpDown.Location = new System.Drawing.Point(283, 95);
            this.serpentPortNumber_NumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.serpentPortNumber_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.serpentPortNumber_NumericUpDown.MaximumSize = new System.Drawing.Size(35, 0);
            this.serpentPortNumber_NumericUpDown.MinimumSize = new System.Drawing.Size(45, 0);
            this.serpentPortNumber_NumericUpDown.Name = "serpentPortNumber_NumericUpDown";
            this.serpentPortNumber_NumericUpDown.ReadOnly = true;
            this.serpentPortNumber_NumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.serpentPortNumber_NumericUpDown.TabIndex = 63;
            // 
            // europaPortNumber_NumericUpDown
            // 
            this.europaPortNumber_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.europaPortNumber_NumericUpDown.AutoSize = true;
            this.europaPortNumber_NumericUpDown.Enabled = false;
            this.europaPortNumber_NumericUpDown.Location = new System.Drawing.Point(283, 64);
            this.europaPortNumber_NumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.europaPortNumber_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.europaPortNumber_NumericUpDown.MaximumSize = new System.Drawing.Size(35, 0);
            this.europaPortNumber_NumericUpDown.MinimumSize = new System.Drawing.Size(45, 0);
            this.europaPortNumber_NumericUpDown.Name = "europaPortNumber_NumericUpDown";
            this.europaPortNumber_NumericUpDown.ReadOnly = true;
            this.europaPortNumber_NumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.europaPortNumber_NumericUpDown.TabIndex = 62;
            // 
            // wireless_IpAddressControl
            // 
            this.wireless_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wireless_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wireless_IpAddressControl.Enabled = false;
            this.wireless_IpAddressControl.Location = new System.Drawing.Point(120, 94);
            this.wireless_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wireless_IpAddressControl.Name = "wireless_IpAddressControl";
            this.wireless_IpAddressControl.Size = new System.Drawing.Size(153, 20);
            this.wireless_IpAddressControl.TabIndex = 58;
            this.wireless_IpAddressControl.Tag = "Wireless IP Address";
            this.wireless_IpAddressControl.Text = "...";
            // 
            // serpentine_lbl
            // 
            this.serpentine_lbl.AutoSize = true;
            this.serpentine_lbl.Location = new System.Drawing.Point(12, 97);
            this.serpentine_lbl.Name = "serpentine_lbl";
            this.serpentine_lbl.Size = new System.Drawing.Size(63, 13);
            this.serpentine_lbl.TabIndex = 60;
            this.serpentine_lbl.Text = "Wireless IP:";
            // 
            // secondaryWired_IpAddressControl
            // 
            this.secondaryWired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondaryWired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondaryWired_IpAddressControl.Enabled = false;
            this.secondaryWired_IpAddressControl.Location = new System.Drawing.Point(120, 64);
            this.secondaryWired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondaryWired_IpAddressControl.Name = "secondaryWired_IpAddressControl";
            this.secondaryWired_IpAddressControl.Size = new System.Drawing.Size(153, 20);
            this.secondaryWired_IpAddressControl.TabIndex = 57;
            this.secondaryWired_IpAddressControl.Tag = "Secondary Wired Interface IP Address";
            this.secondaryWired_IpAddressControl.Text = "...";
            // 
            // europaWiredIP_Label
            // 
            this.europaWiredIP_Label.AutoSize = true;
            this.europaWiredIP_Label.Location = new System.Drawing.Point(12, 68);
            this.europaWiredIP_Label.Name = "europaWiredIP_Label";
            this.europaWiredIP_Label.Size = new System.Drawing.Size(105, 13);
            this.europaWiredIP_Label.TabIndex = 59;
            this.europaWiredIP_Label.Text = "Secondary Wired IP:";
            // 
            // primaryWired_IpAddressControl
            // 
            this.primaryWired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.primaryWired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryWired_IpAddressControl.Location = new System.Drawing.Point(120, 35);
            this.primaryWired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.primaryWired_IpAddressControl.Name = "primaryWired_IpAddressControl";
            this.primaryWired_IpAddressControl.Size = new System.Drawing.Size(208, 20);
            this.primaryWired_IpAddressControl.TabIndex = 56;
            this.primaryWired_IpAddressControl.Tag = "Primary Wired Interface IP Address";
            this.primaryWired_IpAddressControl.Text = "...";
            this.primaryWired_IpAddressControl.TextChanged += new System.EventHandler(this.printerIp_IpAddressControl_TextChanged);
            // 
            // printerIp_Label
            // 
            this.printerIp_Label.AutoSize = true;
            this.printerIp_Label.Location = new System.Drawing.Point(12, 38);
            this.printerIp_Label.Name = "printerIp_Label";
            this.printerIp_Label.Size = new System.Drawing.Size(88, 13);
            this.printerIp_Label.TabIndex = 55;
            this.printerIp_Label.Text = "Primary Wired IP:";
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(362, 410);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = "InkJet";
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(352, 128);
            this.sitemapVersionSelector.TabIndex = 54;
            // 
            // switchDetailsControl
            // 
            this.switchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.switchDetailsControl.HidePortNumber = false;
            this.switchDetailsControl.Location = new System.Drawing.Point(361, 551);
            this.switchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.switchDetailsControl.Name = "switchDetailsControl";
            this.switchDetailsControl.Size = new System.Drawing.Size(353, 90);
            this.switchDetailsControl.SwitchIPAddress = "...";
            this.switchDetailsControl.SwitchPortNumber = 1;
            this.switchDetailsControl.TabIndex = 55;
            this.switchDetailsControl.ValidationRequired = true;
            // 
            // printDriverSelector
            // 
            this.printDriverSelector.DriverModel = "";
            this.printDriverSelector.DriverPackagePath = "";
            this.printDriverSelector.Location = new System.Drawing.Point(361, 659);
            this.printDriverSelector.MinimumSize = new System.Drawing.Size(314, 99);
            this.printDriverSelector.Name = "printDriverSelector";
            this.printDriverSelector.PrinterFamily = null;
            this.printDriverSelector.PrinterName = null;
            this.printDriverSelector.Size = new System.Drawing.Size(353, 99);
            this.printDriverSelector.TabIndex = 14;
            // 
            // CertificateManagementConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.printDriverSelector);
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Controls.Add(this.radiusServerDetails_GroupBox);
            this.Name = "CertificateManagementConfigurationControl";
            this.Size = new System.Drawing.Size(734, 788);
            this.Controls.SetChildIndex(this.radiusServerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.printDriverSelector, 0);
            this.radiusServerDetails_GroupBox.ResumeLayout(false);
            this.radiusServerDetails_GroupBox.PerformLayout();
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serpentPortNumber_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.europaPortNumber_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox radiusServerDetails_GroupBox;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private SitemapVersionSelector sitemapVersionSelector;
        private Framework.UI.IPAddressControl rootSha2_IpAddressControl;
        private System.Windows.Forms.Label rootSha2Ip_Label;
        private Framework.UI.IPAddressControl rootSha1_IpAddressControl;
        private System.Windows.Forms.Label rootSha1Ip_Label;
        private Framework.UI.IPAddressControl primaryWired_IpAddressControl;
        private System.Windows.Forms.Label printerIp_Label;
        private SwitchDetailsControl switchDetailsControl;
        private Framework.UI.IPAddressControl wireless_IpAddressControl;
        private System.Windows.Forms.Label serpentine_lbl;
        private Framework.UI.IPAddressControl secondaryWired_IpAddressControl;
        private System.Windows.Forms.Label europaWiredIP_Label;
        private System.Windows.Forms.NumericUpDown serpentPortNumber_NumericUpDown;
        private System.Windows.Forms.NumericUpDown europaPortNumber_NumericUpDown;
        private PrintDriverSelector printDriverSelector;
    }
}
