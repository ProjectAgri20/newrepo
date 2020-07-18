using HP.ScalableTest.Plugin.CtcBase.Controls;

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
            this.ipconfigPlugin_SitemapVersionSelector = new HP.ScalableTest.Plugin.CtcBase.Controls.SitemapVersionSelector();
            this.ipconfig_SwitchDetailsControl = new HP.ScalableTest.Plugin.CtcBase.Controls.SwitchDetailsControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.secondDHCPServer_IpAddressControl = new HP.ScalableTest.UI.IPAddressControl();
            this.linuxServer_IpAddressControl = new HP.ScalableTest.UI.IPAddressControl();
            this.ssid_TextBox = new System.Windows.Forms.TextBox();
            this.primaryDhcpAddress_IpAddressControl = new HP.ScalableTest.UI.IPAddressControl();
            this.serverDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.primaryDhcpAddress_Label = new System.Windows.Forms.Label();
            this.linuxServerIP_Label = new System.Windows.Forms.Label();
            this.secondDHCPServerIP_Label = new System.Windows.Forms.Label();
            this.wirelessDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.ssidName_Label = new System.Windows.Forms.Label();
            this.printerDetails1 = new HP.ScalableTest.Plugin.CtcBase.Controls.PrinterDetails();
            this.serverDetails_GroupBox.SuspendLayout();
            this.wirelessDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipconfigPlugin_SitemapVersionSelector
            // 
            this.ipconfigPlugin_SitemapVersionSelector.AutoSize = true;
            this.ipconfigPlugin_SitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ipconfigPlugin_SitemapVersionSelector.Location = new System.Drawing.Point(467, 379);
            this.ipconfigPlugin_SitemapVersionSelector.Margin = new System.Windows.Forms.Padding(4);
            this.ipconfigPlugin_SitemapVersionSelector.Name = "ipconfigPlugin_SitemapVersionSelector";
            this.ipconfigPlugin_SitemapVersionSelector.PrinterFamily = null;
            this.ipconfigPlugin_SitemapVersionSelector.PrinterName = null;
            this.ipconfigPlugin_SitemapVersionSelector.SitemapVersion = "";
            this.ipconfigPlugin_SitemapVersionSelector.Size = new System.Drawing.Size(235, 83);
            this.ipconfigPlugin_SitemapVersionSelector.TabIndex = 7;
            // 
            // ipconfig_SwitchDetailsControl
            // 
            this.ipconfig_SwitchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ipconfig_SwitchDetailsControl.Location = new System.Drawing.Point(467, 470);
            this.ipconfig_SwitchDetailsControl.Margin = new System.Windows.Forms.Padding(4);
            this.ipconfig_SwitchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.ipconfig_SwitchDetailsControl.Name = "ipconfig_SwitchDetailsControl";
            this.ipconfig_SwitchDetailsControl.Size = new System.Drawing.Size(240, 90);
            this.ipconfig_SwitchDetailsControl.SwitchIPAddress = "...";
            this.ipconfig_SwitchDetailsControl.SwitchPortNumber = 1;
            this.ipconfig_SwitchDetailsControl.TabIndex = 8;
            this.toolTip1.SetToolTip(this.ipconfig_SwitchDetailsControl, "Enter the switch IP address and the port no. where the printer is connected.");
            this.ipconfig_SwitchDetailsControl.ValidationRequired = true;
            // 
            // secondDHCPServer_IpAddressControl
            // 
            this.secondDHCPServer_IpAddressControl.AllowInternalTab = false;
            this.secondDHCPServer_IpAddressControl.AutoHeight = true;
            this.secondDHCPServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondDHCPServer_IpAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.secondDHCPServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondDHCPServer_IpAddressControl.Location = new System.Drawing.Point(147, 61);
            this.secondDHCPServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondDHCPServer_IpAddressControl.Name = "secondDHCPServer_IpAddressControl";
            this.secondDHCPServer_IpAddressControl.ReadOnly = false;
            this.secondDHCPServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.secondDHCPServer_IpAddressControl.TabIndex = 4;
            this.secondDHCPServer_IpAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.secondDHCPServer_IpAddressControl, "Enter valid ping-able router ip on which the printer is configured.");
            this.secondDHCPServer_IpAddressControl.TextChanged += new System.EventHandler(this.iPAddressControl_TextChanged);
            // 
            // linuxServer_IpAddressControl
            // 
            this.linuxServer_IpAddressControl.AllowInternalTab = false;
            this.linuxServer_IpAddressControl.AutoHeight = true;
            this.linuxServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.linuxServer_IpAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linuxServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.linuxServer_IpAddressControl.Location = new System.Drawing.Point(147, 96);
            this.linuxServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.linuxServer_IpAddressControl.Name = "linuxServer_IpAddressControl";
            this.linuxServer_IpAddressControl.ReadOnly = false;
            this.linuxServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.linuxServer_IpAddressControl.TabIndex = 5;
            this.linuxServer_IpAddressControl.Text = "...";
            this.toolTip1.SetToolTip(this.linuxServer_IpAddressControl, "Enter valid ping-able router ip on which the printer is configured.");
            this.linuxServer_IpAddressControl.TextChanged += new System.EventHandler(this.iPAddressControl_TextChanged);
            // 
            // ssid_TextBox
            // 
            this.ssid_TextBox.Location = new System.Drawing.Point(95, 26);
            this.ssid_TextBox.Name = "ssid_TextBox";
            this.ssid_TextBox.Size = new System.Drawing.Size(145, 20);
            this.ssid_TextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ssid_TextBox, "Enter the SSID of the access point for which the wireless network is configured.");
            this.ssid_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ssid_TextBox_Validating);
            // 
            // primaryDhcpAddress_IpAddressControl
            // 
            this.primaryDhcpAddress_IpAddressControl.AllowInternalTab = false;
            this.primaryDhcpAddress_IpAddressControl.AutoHeight = true;
            this.primaryDhcpAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.primaryDhcpAddress_IpAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.primaryDhcpAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryDhcpAddress_IpAddressControl.Location = new System.Drawing.Point(147, 27);
            this.primaryDhcpAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.primaryDhcpAddress_IpAddressControl.Name = "primaryDhcpAddress_IpAddressControl";
            this.primaryDhcpAddress_IpAddressControl.ReadOnly = false;
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
            this.serverDetails_GroupBox.Location = new System.Drawing.Point(0, 640);
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
            this.wirelessDetails_GroupBox.Location = new System.Drawing.Point(467, 567);
            this.wirelessDetails_GroupBox.Name = "wirelessDetails_GroupBox";
            this.wirelessDetails_GroupBox.Size = new System.Drawing.Size(256, 62);
            this.wirelessDetails_GroupBox.TabIndex = 7;
            this.wirelessDetails_GroupBox.TabStop = false;
            this.wirelessDetails_GroupBox.Text = "WirelessInterface Details";
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
            // printerDetails1
            // 
            this.printerDetails1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.printerDetails1.Connectivity = HP.ScalableTest.Plugin.CtcBase.ConnectivityType.Wired;
            this.printerDetails1.HideSecondaryWiredInterfaceInput = false;
            this.printerDetails1.HideWirelessInterfaceInputs = false;
            this.printerDetails1.Location = new System.Drawing.Point(0, 379);
            this.printerDetails1.Name = "printerDetails1";
            this.printerDetails1.PrimaryInterfaceAddress = "";
            this.printerDetails1.PrimaryInterfacePortNumber = -1;
            this.printerDetails1.PrinterFamily = HP.ScalableTest.Printer.PrinterFamilies.VEP;
            this.printerDetails1.PrinterInterface = HP.ScalableTest.Plugin.CtcBase.Controls.InterfaceType.None;
            this.printerDetails1.SecondaryInterfaceAddress = "";
            this.printerDetails1.SecondaryInterfacePortNumber = -1;
            this.printerDetails1.Size = new System.Drawing.Size(460, 240);
            this.printerDetails1.TabIndex = 9;
            this.printerDetails1.WirelessAccessPointPortNumber = -1;
            this.printerDetails1.WirelessAddress = "";
            this.printerDetails1.WirelessMacAddress = "";
            // 
            // IPConfigurationConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.printerDetails1);
            this.Controls.Add(this.serverDetails_GroupBox);
            this.Controls.Add(this.ipconfig_SwitchDetailsControl);
            this.Controls.Add(this.wirelessDetails_GroupBox);
            this.Controls.Add(this.ipconfigPlugin_SitemapVersionSelector);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IPConfigurationConfigurationControl";
            this.Size = new System.Drawing.Size(731, 798);
            this.Controls.SetChildIndex(this.ipconfigPlugin_SitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.wirelessDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.ipconfig_SwitchDetailsControl, 0);
            this.Controls.SetChildIndex(this.serverDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.printerDetails1, 0);
            this.serverDetails_GroupBox.ResumeLayout(false);
            this.serverDetails_GroupBox.PerformLayout();
            this.wirelessDetails_GroupBox.ResumeLayout(false);
            this.wirelessDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
		private SitemapVersionSelector ipconfigPlugin_SitemapVersionSelector;
        private SwitchDetailsControl ipconfig_SwitchDetailsControl;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox serverDetails_GroupBox;
        private UI.IPAddressControl linuxServer_IpAddressControl;
        private UI.IPAddressControl secondDHCPServer_IpAddressControl;
        private System.Windows.Forms.Label linuxServerIP_Label;
        private System.Windows.Forms.Label secondDHCPServerIP_Label;
		private System.Windows.Forms.GroupBox wirelessDetails_GroupBox;
		private System.Windows.Forms.TextBox ssid_TextBox;
		private System.Windows.Forms.Label ssidName_Label;
		private UI.IPAddressControl primaryDhcpAddress_IpAddressControl;
		private System.Windows.Forms.Label primaryDhcpAddress_Label;
        private PrinterDetails printerDetails1;
    }
}
