using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    partial class IPConfigurationConfigurationControl
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
            this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.wirelessMacAddress_TextBox = new System.Windows.Forms.TextBox();
            this.wirelessMac_Label = new System.Windows.Forms.Label();
            this.connectivity_Label = new System.Windows.Forms.Label();
            this.radioButton_Wireless = new System.Windows.Forms.RadioButton();
            this.wirelessIpv4_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.radioButton_Wired = new System.Windows.Forms.RadioButton();
            this.wiredIpv4Address_Label = new System.Windows.Forms.Label();
            this.wiredIpv4_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.wirelessIpv4Address_Label = new System.Windows.Forms.Label();
            this.ipconfigPlugin_SitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.ipconfig_SwitchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.secondDHCPServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.linuxServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.ssid_TextBox = new System.Windows.Forms.TextBox();
            this.primaryDhcpAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.serverDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.primaryDhcpAddress_Label = new System.Windows.Forms.Label();
            this.linuxServerIP_Label = new System.Windows.Forms.Label();
            this.secondDHCPServerIP_Label = new System.Windows.Forms.Label();
            this.wirelessDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.ssidName_Label = new System.Windows.Forms.Label();
            this.printerDetails_GroupBox.SuspendLayout();
            this.serverDetails_GroupBox.SuspendLayout();
            this.wirelessDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.wirelessMacAddress_TextBox);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessMac_Label);
            this.printerDetails_GroupBox.Controls.Add(this.connectivity_Label);
            this.printerDetails_GroupBox.Controls.Add(this.radioButton_Wireless);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessIpv4_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.radioButton_Wired);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4_IPAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessIpv4Address_Label);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(5, 373);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(337, 133);
            this.printerDetails_GroupBox.TabIndex = 3;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // wirelessMacAddress_TextBox
            // 
            this.wirelessMacAddress_TextBox.Location = new System.Drawing.Point(145, 58);
            this.wirelessMacAddress_TextBox.Name = "wirelessMacAddress_TextBox";
            this.wirelessMacAddress_TextBox.Size = new System.Drawing.Size(173, 20);
            this.wirelessMacAddress_TextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.wirelessMacAddress_TextBox, "Enter the wireless MAC Address.");
            this.wirelessMacAddress_TextBox.Visible = false;
            this.wirelessMacAddress_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.wirelessMacAddress_TextBox_Validating);
            // 
            // wirelessMac_Label
            // 
            this.wirelessMac_Label.AutoSize = true;
            this.wirelessMac_Label.Location = new System.Drawing.Point(22, 62);
            this.wirelessMac_Label.Name = "wirelessMac_Label";
            this.wirelessMac_Label.Size = new System.Drawing.Size(74, 13);
            this.wirelessMac_Label.TabIndex = 18;
            this.wirelessMac_Label.Text = "Wireless Mac:";
            this.wirelessMac_Label.Visible = false;
            // 
            // connectivity_Label
            // 
            this.connectivity_Label.AutoSize = true;
            this.connectivity_Label.Location = new System.Drawing.Point(22, 94);
            this.connectivity_Label.Name = "connectivity_Label";
            this.connectivity_Label.Size = new System.Drawing.Size(68, 13);
            this.connectivity_Label.TabIndex = 14;
            this.connectivity_Label.Text = "Connectivity:";
            // 
            // radioButton_Wireless
            // 
            this.radioButton_Wireless.AutoSize = true;
            this.radioButton_Wireless.Location = new System.Drawing.Point(232, 94);
            this.radioButton_Wireless.Name = "radioButton_Wireless";
            this.radioButton_Wireless.Size = new System.Drawing.Size(65, 17);
            this.radioButton_Wireless.TabIndex = 2;
            this.radioButton_Wireless.TabStop = true;
            this.radioButton_Wireless.Text = "Wireless";
            this.radioButton_Wireless.UseVisualStyleBackColor = true;
            // 
            // wirelessIpv4_IpAddressControl
            // 
            this.wirelessIpv4_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wirelessIpv4_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wirelessIpv4_IpAddressControl.Location = new System.Drawing.Point(145, 58);
            this.wirelessIpv4_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wirelessIpv4_IpAddressControl.Name = "wirelessIpv4_IpAddressControl";
            this.wirelessIpv4_IpAddressControl.Size = new System.Drawing.Size(173, 20);
            this.wirelessIpv4_IpAddressControl.TabIndex = 1;
            this.wirelessIpv4_IpAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.wirelessIpv4_IpAddressControl, "Enter a valid wireless IPv4 address.");
            // 
            // radioButton_Wired
            // 
            this.radioButton_Wired.AutoSize = true;
            this.radioButton_Wired.Location = new System.Drawing.Point(145, 94);
            this.radioButton_Wired.Name = "radioButton_Wired";
            this.radioButton_Wired.Size = new System.Drawing.Size(53, 17);
            this.radioButton_Wired.TabIndex = 2;
            this.radioButton_Wired.TabStop = true;
            this.radioButton_Wired.Text = "Wired";
            this.toolTip1.SetToolTip(this.radioButton_Wired, "Choose the appropriate connectivity.");
            this.radioButton_Wired.UseVisualStyleBackColor = true;
            this.radioButton_Wired.CheckedChanged += new System.EventHandler(this.radioButton_Wired_CheckedChanged);
            // 
            // wiredIpv4Address_Label
            // 
            this.wiredIpv4Address_Label.AutoSize = true;
            this.wiredIpv4Address_Label.Location = new System.Drawing.Point(22, 30);
            this.wiredIpv4Address_Label.Name = "wiredIpv4Address_Label";
            this.wiredIpv4Address_Label.Size = new System.Drawing.Size(61, 13);
            this.wiredIpv4Address_Label.TabIndex = 1;
            this.wiredIpv4Address_Label.Text = "IP Address:";
            // 
            // wiredIpv4_IPAddressControl
            // 
            this.wiredIpv4_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wiredIpv4_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wiredIpv4_IPAddressControl.Location = new System.Drawing.Point(145, 27);
            this.wiredIpv4_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wiredIpv4_IPAddressControl.Name = "wiredIpv4_IPAddressControl";
            this.wiredIpv4_IPAddressControl.Size = new System.Drawing.Size(173, 20);
            this.wiredIpv4_IPAddressControl.TabIndex = 0;
            this.wiredIpv4_IPAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.wiredIpv4_IPAddressControl, "Enter a valid IPv4 address.");
            this.wiredIpv4_IPAddressControl.TextChanged += new System.EventHandler(this.iPAddressControl_TextChanged);
            this.wiredIpv4_IPAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
            // 
            // wirelessIpv4Address_Label
            // 
            this.wirelessIpv4Address_Label.AutoSize = true;
            this.wirelessIpv4Address_Label.Location = new System.Drawing.Point(22, 62);
            this.wirelessIpv4Address_Label.Name = "wirelessIpv4Address_Label";
            this.wirelessIpv4Address_Label.Size = new System.Drawing.Size(63, 13);
            this.wirelessIpv4Address_Label.TabIndex = 16;
            this.wirelessIpv4Address_Label.Text = "Wireless IP:";
            // 
            // ipconfigPlugin_SitemapVersionSelector
            // 
            this.ipconfigPlugin_SitemapVersionSelector.AutoSize = true;
            this.ipconfigPlugin_SitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ipconfigPlugin_SitemapVersionSelector.Location = new System.Drawing.Point(349, 373);
            this.ipconfigPlugin_SitemapVersionSelector.Margin = new System.Windows.Forms.Padding(4);
            this.ipconfigPlugin_SitemapVersionSelector.Name = "ipconfigPlugin_SitemapVersionSelector";
            this.ipconfigPlugin_SitemapVersionSelector.PrinterFamily = null;
            this.ipconfigPlugin_SitemapVersionSelector.PrinterName = null;
            this.ipconfigPlugin_SitemapVersionSelector.SitemapPath = "";
            this.ipconfigPlugin_SitemapVersionSelector.SitemapVersion = "";
            this.ipconfigPlugin_SitemapVersionSelector.Size = new System.Drawing.Size(374, 97);
            this.ipconfigPlugin_SitemapVersionSelector.TabIndex = 7;
            // 
            // ipconfig_SwitchDetailsControl
            // 
            this.ipconfig_SwitchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ipconfig_SwitchDetailsControl.Location = new System.Drawing.Point(352, 484);
            this.ipconfig_SwitchDetailsControl.Margin = new System.Windows.Forms.Padding(4);
            this.ipconfig_SwitchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.ipconfig_SwitchDetailsControl.Name = "ipconfig_SwitchDetailsControl";
            this.ipconfig_SwitchDetailsControl.Size = new System.Drawing.Size(371, 96);
            this.ipconfig_SwitchDetailsControl.SwitchIPAddress = "...";
            this.ipconfig_SwitchDetailsControl.SwitchPortNumber = 1;
            this.ipconfig_SwitchDetailsControl.TabIndex = 8;
            this.toolTip1.SetToolTip(this.ipconfig_SwitchDetailsControl, "Enter the switch IP address and the port no. where the printer is connected.");
            this.ipconfig_SwitchDetailsControl.ValidationRequired = true;
            // 
            // secondDHCPServer_IpAddressControl
            // 
            this.secondDHCPServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondDHCPServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondDHCPServer_IpAddressControl.Location = new System.Drawing.Point(147, 61);
            this.secondDHCPServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondDHCPServer_IpAddressControl.Name = "secondDHCPServer_IpAddressControl";
            this.secondDHCPServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.secondDHCPServer_IpAddressControl.TabIndex = 4;
            this.secondDHCPServer_IpAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.secondDHCPServer_IpAddressControl, "Enter valid ping-able router ip on which the printer is configured.");
            this.secondDHCPServer_IpAddressControl.TextChanged += new System.EventHandler(this.iPAddressControl_TextChanged);
            // 
            // linuxServer_IpAddressControl
            // 
            this.linuxServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.linuxServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.linuxServer_IpAddressControl.Location = new System.Drawing.Point(147, 96);
            this.linuxServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.linuxServer_IpAddressControl.Name = "linuxServer_IpAddressControl";
            this.linuxServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.linuxServer_IpAddressControl.TabIndex = 5;
            this.linuxServer_IpAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.linuxServer_IpAddressControl, "Enter valid ping-able router ip on which the printer is configured.");
            this.linuxServer_IpAddressControl.TextChanged += new System.EventHandler(this.iPAddressControl_TextChanged);
            // 
            // ssid_TextBox
            // 
            this.ssid_TextBox.Location = new System.Drawing.Point(145, 26);
            this.ssid_TextBox.Name = "ssid_TextBox";
            this.ssid_TextBox.Size = new System.Drawing.Size(176, 20);
            this.ssid_TextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ssid_TextBox, "Enter the SSID of the access point for which the wireless network is configured.");
            this.ssid_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ssid_TextBox_Validating);
            // 
            // primaryDhcpAddress_IpAddressControl
            // 
            this.primaryDhcpAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.primaryDhcpAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryDhcpAddress_IpAddressControl.Location = new System.Drawing.Point(147, 27);
            this.primaryDhcpAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.primaryDhcpAddress_IpAddressControl.Name = "primaryDhcpAddress_IpAddressControl";
            this.primaryDhcpAddress_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.primaryDhcpAddress_IpAddressControl.TabIndex = 3;
            this.primaryDhcpAddress_IpAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.primaryDhcpAddress_IpAddressControl, "Enter valid ping-able router ip on which the printer is configured.");
            this.primaryDhcpAddress_IpAddressControl.TextChanged += new System.EventHandler(this.iPAddressControl_TextChanged);
            // 
            // serverDetails_GroupBox
            // 
            this.serverDetails_GroupBox.Controls.Add(this.primaryDhcpAddress_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.primaryDhcpAddress_Label);
            this.serverDetails_GroupBox.Controls.Add(this.linuxServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.secondDHCPServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.linuxServerIP_Label);
            this.serverDetails_GroupBox.Controls.Add(this.secondDHCPServerIP_Label);
            this.serverDetails_GroupBox.Location = new System.Drawing.Point(3, 512);
            this.serverDetails_GroupBox.Name = "serverDetails_GroupBox";
            this.serverDetails_GroupBox.Size = new System.Drawing.Size(339, 153);
            this.serverDetails_GroupBox.TabIndex = 8;
            this.serverDetails_GroupBox.TabStop = false;
            this.serverDetails_GroupBox.Text = "DHCP Server Details";
            // 
            // primaryDhcpAddress_Label
            // 
            this.primaryDhcpAddress_Label.AutoSize = true;
            this.primaryDhcpAddress_Label.Location = new System.Drawing.Point(22, 30);
            this.primaryDhcpAddress_Label.Name = "primaryDhcpAddress_Label";
            this.primaryDhcpAddress_Label.Size = new System.Drawing.Size(84, 13);
            this.primaryDhcpAddress_Label.TabIndex = 64;
            this.primaryDhcpAddress_Label.Text = "Primary  Server :";
            // 
            // linuxServerIP_Label
            // 
            this.linuxServerIP_Label.AutoSize = true;
            this.linuxServerIP_Label.Location = new System.Drawing.Point(22, 99);
            this.linuxServerIP_Label.Name = "linuxServerIP_Label";
            this.linuxServerIP_Label.Size = new System.Drawing.Size(69, 13);
            this.linuxServerIP_Label.TabIndex = 61;
            this.linuxServerIP_Label.Text = "Linux Server:";
            // 
            // secondDHCPServerIP_Label
            // 
            this.secondDHCPServerIP_Label.AutoSize = true;
            this.secondDHCPServerIP_Label.Location = new System.Drawing.Point(22, 64);
            this.secondDHCPServerIP_Label.Name = "secondDHCPServerIP_Label";
            this.secondDHCPServerIP_Label.Size = new System.Drawing.Size(95, 13);
            this.secondDHCPServerIP_Label.TabIndex = 9;
            this.secondDHCPServerIP_Label.Text = "Secondary Server:";
            // 
            // wirelessDetails_GroupBox
            // 
            this.wirelessDetails_GroupBox.Controls.Add(this.ssid_TextBox);
            this.wirelessDetails_GroupBox.Controls.Add(this.ssidName_Label);
            this.wirelessDetails_GroupBox.Location = new System.Drawing.Point(352, 599);
            this.wirelessDetails_GroupBox.Name = "wirelessDetails_GroupBox";
            this.wirelessDetails_GroupBox.Size = new System.Drawing.Size(365, 66);
            this.wirelessDetails_GroupBox.TabIndex = 7;
            this.wirelessDetails_GroupBox.TabStop = false;
            this.wirelessDetails_GroupBox.Text = "Wireless Details";
            // 
            // ssidName_Label
            // 
            this.ssidName_Label.AutoSize = true;
            this.ssidName_Label.Location = new System.Drawing.Point(20, 29);
            this.ssidName_Label.Name = "ssidName_Label";
            this.ssidName_Label.Size = new System.Drawing.Size(66, 13);
            this.ssidName_Label.TabIndex = 61;
            this.ssidName_Label.Text = "SSID Name:";
            // 
            // IPConfigurationConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.serverDetails_GroupBox);
            this.Controls.Add(this.ipconfig_SwitchDetailsControl);
            this.Controls.Add(this.wirelessDetails_GroupBox);
            this.Controls.Add(this.ipconfigPlugin_SitemapVersionSelector);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IPConfigurationConfigurationControl";
            this.Size = new System.Drawing.Size(734, 714);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.ipconfigPlugin_SitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.wirelessDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.ipconfig_SwitchDetailsControl, 0);
            this.Controls.SetChildIndex(this.serverDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            this.serverDetails_GroupBox.ResumeLayout(false);
            this.serverDetails_GroupBox.PerformLayout();
            this.wirelessDetails_GroupBox.ResumeLayout(false);
            this.wirelessDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private System.Windows.Forms.Label wiredIpv4Address_Label;
        private Framework.UI.IPAddressControl wiredIpv4_IPAddressControl;
		private SitemapVersionSelector ipconfigPlugin_SitemapVersionSelector;
        private SwitchDetailsControl ipconfig_SwitchDetailsControl;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton radioButton_Wireless;
		private System.Windows.Forms.RadioButton radioButton_Wired;
        private System.Windows.Forms.Label connectivity_Label;
        private System.Windows.Forms.GroupBox serverDetails_GroupBox;
        private Framework.UI.IPAddressControl linuxServer_IpAddressControl;
        private Framework.UI.IPAddressControl secondDHCPServer_IpAddressControl;
        private System.Windows.Forms.Label linuxServerIP_Label;
        private System.Windows.Forms.Label secondDHCPServerIP_Label;
		private System.Windows.Forms.Label wirelessIpv4Address_Label;
		private Framework.UI.IPAddressControl wirelessIpv4_IpAddressControl;
		private System.Windows.Forms.GroupBox wirelessDetails_GroupBox;
		private System.Windows.Forms.TextBox ssid_TextBox;
		private System.Windows.Forms.Label ssidName_Label;
		private System.Windows.Forms.TextBox wirelessMacAddress_TextBox;
		private System.Windows.Forms.Label wirelessMac_Label;
		private Framework.UI.IPAddressControl primaryDhcpAddress_IpAddressControl;
		private System.Windows.Forms.Label primaryDhcpAddress_Label;
    }
}
