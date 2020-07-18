using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.Firewall
{
    partial class FirewallConfigurationControl
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
            this.ipv4Address_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.firewall_SitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stateFull_CheckBox = new System.Windows.Forms.CheckBox();
            this.stateLess_CheckBox = new System.Windows.Forms.CheckBox();
            this.linkLocal_CheckBox = new System.Windows.Forms.CheckBox();
            this.ipv6AddressType_Label = new System.Windows.Forms.Label();
            this.wirelessMacAddress_TextBox = new System.Windows.Forms.TextBox();
            this.wirelessMac_Label = new System.Windows.Forms.Label();
            this.connectivity_Label = new System.Windows.Forms.Label();
            this.radioButton_Wireless = new System.Windows.Forms.RadioButton();
            this.radioButton_Wired = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.firewall_PrintDriverDetails = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.driverFolder_BrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.firewall_SwitchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.wirelessDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.ssid_TextBox = new System.Windows.Forms.TextBox();
            this.ssidName_Label = new System.Windows.Forms.Label();
            this.v4_CheckBox = new System.Windows.Forms.CheckBox();
            this.debug_GroupBox = new System.Windows.Forms.GroupBox();
            this.debug_CheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.wirelessDetails_GroupBox.SuspendLayout();
            this.debug_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipv4Address_IPAddressControl
            // 
            this.ipv4Address_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipv4Address_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipv4Address_IPAddressControl.Location = new System.Drawing.Point(102, 28);
            this.ipv4Address_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipv4Address_IPAddressControl.Name = "ipv4Address_IPAddressControl";
            this.ipv4Address_IPAddressControl.Size = new System.Drawing.Size(267, 20);
            this.ipv4Address_IPAddressControl.TabIndex = 6;
            this.ipv4Address_IPAddressControl.Text = "...";
            // 
            // firewall_SitemapVersionSelector
            // 
            this.firewall_SitemapVersionSelector.AutoSize = true;
            this.firewall_SitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.firewall_SitemapVersionSelector.Location = new System.Drawing.Point(409, 388);
            this.firewall_SitemapVersionSelector.Name = "firewall_SitemapVersionSelector";
            this.firewall_SitemapVersionSelector.PrinterFamily = null;
            this.firewall_SitemapVersionSelector.PrinterName = null;
            this.firewall_SitemapVersionSelector.SitemapPath = "";
            this.firewall_SitemapVersionSelector.SitemapVersion = "";
            this.firewall_SitemapVersionSelector.Size = new System.Drawing.Size(308, 102);
            this.firewall_SitemapVersionSelector.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.v4_CheckBox);
            this.groupBox1.Controls.Add(this.stateFull_CheckBox);
            this.groupBox1.Controls.Add(this.stateLess_CheckBox);
            this.groupBox1.Controls.Add(this.linkLocal_CheckBox);
            this.groupBox1.Controls.Add(this.ipv6AddressType_Label);
            this.groupBox1.Controls.Add(this.wirelessMacAddress_TextBox);
            this.groupBox1.Controls.Add(this.wirelessMac_Label);
            this.groupBox1.Controls.Add(this.connectivity_Label);
            this.groupBox1.Controls.Add(this.radioButton_Wireless);
            this.groupBox1.Controls.Add(this.radioButton_Wired);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ipv4Address_IPAddressControl);
            this.groupBox1.Location = new System.Drawing.Point(20, 388);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 172);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Printer Details";
            // 
            // stateFull_CheckBox
            // 
            this.stateFull_CheckBox.AutoSize = true;
            this.stateFull_CheckBox.Location = new System.Drawing.Point(310, 98);
            this.stateFull_CheckBox.Name = "stateFull_CheckBox";
            this.stateFull_CheckBox.Size = new System.Drawing.Size(67, 17);
            this.stateFull_CheckBox.TabIndex = 34;
            this.stateFull_CheckBox.Text = "State full";
            this.stateFull_CheckBox.UseVisualStyleBackColor = true;
            // 
            // stateLess_CheckBox
            // 
            this.stateLess_CheckBox.AutoSize = true;
            this.stateLess_CheckBox.Location = new System.Drawing.Point(235, 98);
            this.stateLess_CheckBox.Name = "stateLess_CheckBox";
            this.stateLess_CheckBox.Size = new System.Drawing.Size(69, 17);
            this.stateLess_CheckBox.TabIndex = 33;
            this.stateLess_CheckBox.Text = "Stateless";
            this.stateLess_CheckBox.UseVisualStyleBackColor = true;
            // 
            // linkLocal_CheckBox
            // 
            this.linkLocal_CheckBox.AutoSize = true;
            this.linkLocal_CheckBox.Location = new System.Drawing.Point(158, 98);
            this.linkLocal_CheckBox.Name = "linkLocal_CheckBox";
            this.linkLocal_CheckBox.Size = new System.Drawing.Size(75, 17);
            this.linkLocal_CheckBox.TabIndex = 32;
            this.linkLocal_CheckBox.Text = "Link Local";
            this.linkLocal_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ipv6AddressType_Label
            // 
            this.ipv6AddressType_Label.AutoSize = true;
            this.ipv6AddressType_Label.Location = new System.Drawing.Point(12, 99);
            this.ipv6AddressType_Label.Name = "ipv6AddressType_Label";
            this.ipv6AddressType_Label.Size = new System.Drawing.Size(47, 13);
            this.ipv6AddressType_Label.TabIndex = 31;
            this.ipv6AddressType_Label.Text = "IP Type:";
            // 
            // wirelessMacAddress_TextBox
            // 
            this.wirelessMacAddress_TextBox.Location = new System.Drawing.Point(101, 66);
            this.wirelessMacAddress_TextBox.Name = "wirelessMacAddress_TextBox";
            this.wirelessMacAddress_TextBox.Size = new System.Drawing.Size(268, 20);
            this.wirelessMacAddress_TextBox.TabIndex = 26;
            this.wirelessMacAddress_TextBox.Visible = false;
            this.wirelessMacAddress_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.wirelessMacAddress_TextBox_Validating);
            // 
            // wirelessMac_Label
            // 
            this.wirelessMac_Label.AutoSize = true;
            this.wirelessMac_Label.Location = new System.Drawing.Point(12, 69);
            this.wirelessMac_Label.Name = "wirelessMac_Label";
            this.wirelessMac_Label.Size = new System.Drawing.Size(62, 13);
            this.wirelessMac_Label.TabIndex = 30;
            this.wirelessMac_Label.Text = "Wired Mac:";
            this.wirelessMac_Label.Visible = false;
            // 
            // connectivity_Label
            // 
            this.connectivity_Label.AutoSize = true;
            this.connectivity_Label.Location = new System.Drawing.Point(12, 135);
            this.connectivity_Label.Name = "connectivity_Label";
            this.connectivity_Label.Size = new System.Drawing.Size(68, 13);
            this.connectivity_Label.TabIndex = 29;
            this.connectivity_Label.Text = "Connectivity:";
            // 
            // radioButton_Wireless
            // 
            this.radioButton_Wireless.AutoSize = true;
            this.radioButton_Wireless.Location = new System.Drawing.Point(168, 133);
            this.radioButton_Wireless.Name = "radioButton_Wireless";
            this.radioButton_Wireless.Size = new System.Drawing.Size(65, 17);
            this.radioButton_Wireless.TabIndex = 27;
            this.radioButton_Wireless.TabStop = true;
            this.radioButton_Wireless.Text = "Wireless";
            this.radioButton_Wireless.UseVisualStyleBackColor = true;
            // 
            // radioButton_Wired
            // 
            this.radioButton_Wired.AutoSize = true;
            this.radioButton_Wired.Location = new System.Drawing.Point(102, 133);
            this.radioButton_Wired.Name = "radioButton_Wired";
            this.radioButton_Wired.Size = new System.Drawing.Size(53, 17);
            this.radioButton_Wired.TabIndex = 28;
            this.radioButton_Wired.TabStop = true;
            this.radioButton_Wired.Text = "Wired";
            this.radioButton_Wired.UseVisualStyleBackColor = true;
            this.radioButton_Wired.CheckedChanged += new System.EventHandler(this.radioButton_Wired_CheckedChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "IPv4 Address:";
            // 
            // firewall_PrintDriverDetails
            // 
            this.firewall_PrintDriverDetails.DriverModel = "";
            this.firewall_PrintDriverDetails.DriverPackagePath = "";
            this.firewall_PrintDriverDetails.Location = new System.Drawing.Point(21, 568);
            this.firewall_PrintDriverDetails.MinimumSize = new System.Drawing.Size(314, 99);
            this.firewall_PrintDriverDetails.Name = "firewall_PrintDriverDetails";
            this.firewall_PrintDriverDetails.PrinterFamily = null;
            this.firewall_PrintDriverDetails.PrinterName = null;
            this.firewall_PrintDriverDetails.Size = new System.Drawing.Size(382, 99);
            this.firewall_PrintDriverDetails.TabIndex = 11;
            // 
            // firewall_SwitchDetailsControl
            // 
            this.firewall_SwitchDetailsControl.HidePortNumber = false;
            this.firewall_SwitchDetailsControl.Location = new System.Drawing.Point(409, 500);
            this.firewall_SwitchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.firewall_SwitchDetailsControl.Name = "firewall_SwitchDetailsControl";
            this.firewall_SwitchDetailsControl.Size = new System.Drawing.Size(308, 90);
            this.firewall_SwitchDetailsControl.SwitchIPAddress = "...";
            this.firewall_SwitchDetailsControl.SwitchPortNumber = 1;
            this.firewall_SwitchDetailsControl.TabIndex = 9;
            this.firewall_SwitchDetailsControl.ValidationRequired = false;
            // 
            // wirelessDetails_GroupBox
            // 
            this.wirelessDetails_GroupBox.Controls.Add(this.ssid_TextBox);
            this.wirelessDetails_GroupBox.Controls.Add(this.ssidName_Label);
            this.wirelessDetails_GroupBox.Location = new System.Drawing.Point(409, 601);
            this.wirelessDetails_GroupBox.Name = "wirelessDetails_GroupBox";
            this.wirelessDetails_GroupBox.Size = new System.Drawing.Size(308, 66);
            this.wirelessDetails_GroupBox.TabIndex = 10;
            this.wirelessDetails_GroupBox.TabStop = false;
            this.wirelessDetails_GroupBox.Text = "Wireless Details";
            // 
            // ssid_TextBox
            // 
            this.ssid_TextBox.Location = new System.Drawing.Point(90, 26);
            this.ssid_TextBox.Name = "ssid_TextBox";
            this.ssid_TextBox.Size = new System.Drawing.Size(175, 20);
            this.ssid_TextBox.TabIndex = 6;
            this.ssid_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ssid_TextBox_Validating);
            // 
            // ssidName_Label
            // 
            this.ssidName_Label.AutoSize = true;
            this.ssidName_Label.Location = new System.Drawing.Point(18, 29);
            this.ssidName_Label.Name = "ssidName_Label";
            this.ssidName_Label.Size = new System.Drawing.Size(66, 13);
            this.ssidName_Label.TabIndex = 61;
            this.ssidName_Label.Text = "SSID Name:";
            // 
            // v4_CheckBox
            // 
            this.v4_CheckBox.AutoSize = true;
            this.v4_CheckBox.Location = new System.Drawing.Point(101, 99);
            this.v4_CheckBox.Name = "v4_CheckBox";
            this.v4_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.v4_CheckBox.TabIndex = 35;
            this.v4_CheckBox.Text = "IP v4";
            this.v4_CheckBox.UseVisualStyleBackColor = true;
            // 
            // debug_GroupBox
            // 
            this.debug_GroupBox.Controls.Add(this.debug_CheckBox);
            this.debug_GroupBox.Location = new System.Drawing.Point(21, 673);
            this.debug_GroupBox.Name = "debug_GroupBox";
            this.debug_GroupBox.Size = new System.Drawing.Size(382, 63);
            this.debug_GroupBox.TabIndex = 12;
            this.debug_GroupBox.TabStop = false;
            this.debug_GroupBox.Text = "Debug Options";
            // 
            // debug_CheckBox
            // 
            this.debug_CheckBox.AutoSize = true;
            this.debug_CheckBox.Location = new System.Drawing.Point(26, 28);
            this.debug_CheckBox.Name = "debug_CheckBox";
            this.debug_CheckBox.Size = new System.Drawing.Size(128, 17);
            this.debug_CheckBox.TabIndex = 0;
            this.debug_CheckBox.Text = "Enable Debug Option";
            this.debug_CheckBox.UseVisualStyleBackColor = true;
            // 
            // FirewallConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.debug_GroupBox);
            this.Controls.Add(this.wirelessDetails_GroupBox);
            this.Controls.Add(this.firewall_SwitchDetailsControl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.firewall_PrintDriverDetails);
            this.Controls.Add(this.firewall_SitemapVersionSelector);
            this.Name = "FirewallConfigurationControl";
            this.Size = new System.Drawing.Size(756, 736);
            this.Controls.SetChildIndex(this.firewall_SitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.firewall_PrintDriverDetails, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.firewall_SwitchDetailsControl, 0);
            this.Controls.SetChildIndex(this.wirelessDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.debug_GroupBox, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.wirelessDetails_GroupBox.ResumeLayout(false);
            this.wirelessDetails_GroupBox.PerformLayout();
            this.debug_GroupBox.ResumeLayout(false);
            this.debug_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.IPAddressControl ipv4Address_IPAddressControl;
        private SitemapVersionSelector firewall_SitemapVersionSelector;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog driverFolder_BrowserDialog;
        private System.Windows.Forms.TextBox wirelessMacAddress_TextBox;
        private System.Windows.Forms.Label wirelessMac_Label;
        private System.Windows.Forms.Label connectivity_Label;
        private System.Windows.Forms.RadioButton radioButton_Wireless;
        private System.Windows.Forms.RadioButton radioButton_Wired;
        private SwitchDetailsControl firewall_SwitchDetailsControl;
        private System.Windows.Forms.GroupBox wirelessDetails_GroupBox;
        private System.Windows.Forms.TextBox ssid_TextBox;
        private System.Windows.Forms.Label ssidName_Label;
        private PrintDriverSelector firewall_PrintDriverDetails;
        private System.Windows.Forms.CheckBox stateFull_CheckBox;
        private System.Windows.Forms.CheckBox stateLess_CheckBox;
        private System.Windows.Forms.CheckBox linkLocal_CheckBox;
        private System.Windows.Forms.Label ipv6AddressType_Label;
        private System.Windows.Forms.CheckBox v4_CheckBox;
        private System.Windows.Forms.GroupBox debug_GroupBox;
        private System.Windows.Forms.CheckBox debug_CheckBox;
    }
}
