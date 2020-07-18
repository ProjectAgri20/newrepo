using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.NetworkNamingServices
{
    partial class NetworkNamingServiceConfigurationControl
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
            this.sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.secondPrinter_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.secondPrinterIP_Label = new System.Windows.Forms.Label();
            this.wirelessIpv4_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.wiredIpv4Address_Label = new System.Windows.Forms.Label();
            this.wiredIpv4_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.wirelessIpv4Address_Label = new System.Windows.Forms.Label();
            this.serverDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.primaryDhcpAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.primaryDhcpAddress_Label = new System.Windows.Forms.Label();
            this.linuxServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.secondDHCPServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.linuxServerIP_Label = new System.Windows.Forms.Label();
            this.secondDHCPServerIP_Label = new System.Windows.Forms.Label();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.printerDetails_GroupBox.SuspendLayout();
            this.serverDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(362, 423);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = null;
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(355, 121);
            this.sitemapVersionSelector.TabIndex = 30;
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.secondPrinter_IPAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.secondPrinterIP_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessIpv4_IPAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4_IPAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessIpv4Address_Label);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(10, 423);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(337, 121);
            this.printerDetails_GroupBox.TabIndex = 31;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // secondPrinter_IPAddressControl
            // 
            this.secondPrinter_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondPrinter_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondPrinter_IPAddressControl.Location = new System.Drawing.Point(146, 80);
            this.secondPrinter_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondPrinter_IPAddressControl.Name = "secondPrinter_IPAddressControl";
            this.secondPrinter_IPAddressControl.Size = new System.Drawing.Size(173, 20);
            this.secondPrinter_IPAddressControl.TabIndex = 17;
            this.secondPrinter_IPAddressControl.Text = "...";
            this.secondPrinter_IPAddressControl.TextChanged += new System.EventHandler(this.wiredIpv4_IPAddressControl_TextChanged);
            this.secondPrinter_IPAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
            // 
            // secondPrinterIP_Label
            // 
            this.secondPrinterIP_Label.AutoSize = true;
            this.secondPrinterIP_Label.Location = new System.Drawing.Point(23, 84);
            this.secondPrinterIP_Label.Name = "secondPrinterIP_Label";
            this.secondPrinterIP_Label.Size = new System.Drawing.Size(93, 13);
            this.secondPrinterIP_Label.TabIndex = 18;
            this.secondPrinterIP_Label.Text = "Second Printer IP:";
            // 
            // wirelessIpv4_IPAddressControl
            // 
            this.wirelessIpv4_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wirelessIpv4_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wirelessIpv4_IPAddressControl.Location = new System.Drawing.Point(145, 53);
            this.wirelessIpv4_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wirelessIpv4_IPAddressControl.Name = "wirelessIpv4_IPAddressControl";
            this.wirelessIpv4_IPAddressControl.Size = new System.Drawing.Size(173, 20);
            this.wirelessIpv4_IPAddressControl.TabIndex = 1;
            this.wirelessIpv4_IPAddressControl.Text = "...";
            this.wirelessIpv4_IPAddressControl.TextChanged += new System.EventHandler(this.wiredIpv4_IPAddressControl_TextChanged);
            this.wirelessIpv4_IPAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
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
            this.wiredIpv4_IPAddressControl.TextChanged += new System.EventHandler(this.wiredIpv4_IPAddressControl_TextChanged);
            this.wiredIpv4_IPAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
            // 
            // wirelessIpv4Address_Label
            // 
            this.wirelessIpv4Address_Label.AutoSize = true;
            this.wirelessIpv4Address_Label.Location = new System.Drawing.Point(22, 57);
            this.wirelessIpv4Address_Label.Name = "wirelessIpv4Address_Label";
            this.wirelessIpv4Address_Label.Size = new System.Drawing.Size(63, 13);
            this.wirelessIpv4Address_Label.TabIndex = 16;
            this.wirelessIpv4Address_Label.Text = "Wireless IP:";
            // 
            // serverDetails_GroupBox
            // 
            this.serverDetails_GroupBox.Controls.Add(this.primaryDhcpAddress_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.primaryDhcpAddress_Label);
            this.serverDetails_GroupBox.Controls.Add(this.linuxServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.secondDHCPServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.linuxServerIP_Label);
            this.serverDetails_GroupBox.Controls.Add(this.secondDHCPServerIP_Label);
            this.serverDetails_GroupBox.Location = new System.Drawing.Point(10, 552);
            this.serverDetails_GroupBox.Name = "serverDetails_GroupBox";
            this.serverDetails_GroupBox.Size = new System.Drawing.Size(339, 119);
            this.serverDetails_GroupBox.TabIndex = 32;
            this.serverDetails_GroupBox.TabStop = false;
            this.serverDetails_GroupBox.Text = "DHCP Server Details";
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
            this.primaryDhcpAddress_IpAddressControl.TextChanged += new System.EventHandler(this.wiredIpv4_IPAddressControl_TextChanged);
            this.primaryDhcpAddress_IpAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
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
            // linuxServer_IpAddressControl
            // 
            this.linuxServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.linuxServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.linuxServer_IpAddressControl.Location = new System.Drawing.Point(147, 81);
            this.linuxServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.linuxServer_IpAddressControl.Name = "linuxServer_IpAddressControl";
            this.linuxServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.linuxServer_IpAddressControl.TabIndex = 5;
            this.linuxServer_IpAddressControl.Text = "...";
            this.linuxServer_IpAddressControl.TextChanged += new System.EventHandler(this.wiredIpv4_IPAddressControl_TextChanged);
            this.linuxServer_IpAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
            // 
            // secondDHCPServer_IpAddressControl
            // 
            this.secondDHCPServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondDHCPServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondDHCPServer_IpAddressControl.Location = new System.Drawing.Point(147, 54);
            this.secondDHCPServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondDHCPServer_IpAddressControl.Name = "secondDHCPServer_IpAddressControl";
            this.secondDHCPServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.secondDHCPServer_IpAddressControl.TabIndex = 4;
            this.secondDHCPServer_IpAddressControl.Text = "...";
            this.secondDHCPServer_IpAddressControl.TextChanged += new System.EventHandler(this.wiredIpv4_IPAddressControl_TextChanged);
            this.secondDHCPServer_IpAddressControl.Validated += new System.EventHandler(this.wiredIpv4_IPAddressControl_Validated);
            // 
            // linuxServerIP_Label
            // 
            this.linuxServerIP_Label.AutoSize = true;
            this.linuxServerIP_Label.Location = new System.Drawing.Point(22, 84);
            this.linuxServerIP_Label.Name = "linuxServerIP_Label";
            this.linuxServerIP_Label.Size = new System.Drawing.Size(69, 13);
            this.linuxServerIP_Label.TabIndex = 61;
            this.linuxServerIP_Label.Text = "Linux Server:";
            // 
            // secondDHCPServerIP_Label
            // 
            this.secondDHCPServerIP_Label.AutoSize = true;
            this.secondDHCPServerIP_Label.Location = new System.Drawing.Point(22, 57);
            this.secondDHCPServerIP_Label.Name = "secondDHCPServerIP_Label";
            this.secondDHCPServerIP_Label.Size = new System.Drawing.Size(95, 13);
            this.secondDHCPServerIP_Label.TabIndex = 9;
            this.secondDHCPServerIP_Label.Text = "Secondary Server:";
            // 
            // switchDetailsControl
            // 
            this.switchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.switchDetailsControl.Location = new System.Drawing.Point(362, 552);
            this.switchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.switchDetailsControl.Name = "switchDetailsControl";
            this.switchDetailsControl.Size = new System.Drawing.Size(355, 119);
            this.switchDetailsControl.SwitchIPAddress = "...";
            this.switchDetailsControl.SwitchPortNumber = 1;
            this.switchDetailsControl.TabIndex = 33;
            this.switchDetailsControl.ValidationRequired = false;
            // 
            // NetworkNamingServiceConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.serverDetails_GroupBox);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Name = "NetworkNamingServiceConfigurationControl";
            this.Size = new System.Drawing.Size(723, 701);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.serverDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            this.serverDetails_GroupBox.ResumeLayout(false);
            this.serverDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SitemapVersionSelector sitemapVersionSelector;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private Framework.UI.IPAddressControl wirelessIpv4_IPAddressControl;
        private System.Windows.Forms.Label wiredIpv4Address_Label;
        private Framework.UI.IPAddressControl wiredIpv4_IPAddressControl;
        private System.Windows.Forms.Label wirelessIpv4Address_Label;
        private Framework.UI.IPAddressControl secondPrinter_IPAddressControl;
        private System.Windows.Forms.Label secondPrinterIP_Label;
        private System.Windows.Forms.GroupBox serverDetails_GroupBox;
        private Framework.UI.IPAddressControl primaryDhcpAddress_IpAddressControl;
        private System.Windows.Forms.Label primaryDhcpAddress_Label;
        private Framework.UI.IPAddressControl linuxServer_IpAddressControl;
        private Framework.UI.IPAddressControl secondDHCPServer_IpAddressControl;
        private System.Windows.Forms.Label linuxServerIP_Label;
        private System.Windows.Forms.Label secondDHCPServerIP_Label;
        private SwitchDetailsControl switchDetailsControl;


    }
}
