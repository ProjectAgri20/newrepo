using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.IPSecurity
{
    partial class IPSecurityConfigurationControl
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
            this.europa_RadioButton = new System.Windows.Forms.RadioButton();
            this.wireless_RadioButton = new System.Windows.Forms.RadioButton();
            this.wired_RadioButton = new System.Windows.Forms.RadioButton();
            this.europaWired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.europaWiredIP_Label = new System.Windows.Forms.Label();
            this.wirelessIpv4_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.wiredIpv4Address_Label = new System.Windows.Forms.Label();
            this.wiredIpv4_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.wirelessIpv4Address_Label = new System.Windows.Forms.Label();
            this.serverDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.primaryDhcpAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.primaryDhcpAddress_Label = new System.Windows.Forms.Label();
            this.kerberosServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.secondDHCPServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.kerberosServerIP_Label = new System.Windows.Forms.Label();
            this.secondDHCPServerIP_Label = new System.Windows.Forms.Label();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.windowsSecondaryIP_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.windowsSecondary_lbl = new System.Windows.Forms.Label();
            this.linuxFedoraClient_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.linuxFedoraClientIP_lbl = new System.Windows.Forms.Label();
            this.print_PrintDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.message_CheckBox = new System.Windows.Forms.CheckBox();
            this.printerDetails_GroupBox.SuspendLayout();
            this.serverDetails_GroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // testCaseDetails_GroupBox
            // 
            this.testCaseDetails_GroupBox.Size = new System.Drawing.Size(725, 330);
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(362, 383);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = null;
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(355, 114);
            this.sitemapVersionSelector.TabIndex = 34;
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.europa_RadioButton);
            this.printerDetails_GroupBox.Controls.Add(this.wireless_RadioButton);
            this.printerDetails_GroupBox.Controls.Add(this.wired_RadioButton);
            this.printerDetails_GroupBox.Controls.Add(this.europaWired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.europaWiredIP_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessIpv4_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.wirelessIpv4Address_Label);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(10, 383);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(345, 143);
            this.printerDetails_GroupBox.TabIndex = 31;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // europa_RadioButton
            // 
            this.europa_RadioButton.AutoSize = true;
            this.europa_RadioButton.Location = new System.Drawing.Point(232, 110);
            this.europa_RadioButton.Name = "europa_RadioButton";
            this.europa_RadioButton.Size = new System.Drawing.Size(104, 17);
            this.europa_RadioButton.TabIndex = 21;
            this.europa_RadioButton.TabStop = true;
            this.europa_RadioButton.Text = "Europa Interface";
            this.europa_RadioButton.UseVisualStyleBackColor = true;
            // 
            // wireless_RadioButton
            // 
            this.wireless_RadioButton.AutoSize = true;
            this.wireless_RadioButton.Location = new System.Drawing.Point(116, 110);
            this.wireless_RadioButton.Name = "wireless_RadioButton";
            this.wireless_RadioButton.Size = new System.Drawing.Size(110, 17);
            this.wireless_RadioButton.TabIndex = 20;
            this.wireless_RadioButton.TabStop = true;
            this.wireless_RadioButton.Text = "Wireless Interface";
            this.wireless_RadioButton.UseVisualStyleBackColor = true;
            // 
            // wired_RadioButton
            // 
            this.wired_RadioButton.AutoSize = true;
            this.wired_RadioButton.Location = new System.Drawing.Point(12, 110);
            this.wired_RadioButton.Name = "wired_RadioButton";
            this.wired_RadioButton.Size = new System.Drawing.Size(98, 17);
            this.wired_RadioButton.TabIndex = 19;
            this.wired_RadioButton.TabStop = true;
            this.wired_RadioButton.Text = "Wired Interface";
            this.wired_RadioButton.UseVisualStyleBackColor = true;
            // 
            // europaWired_IpAddressControl
            // 
            this.europaWired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.europaWired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.europaWired_IpAddressControl.Location = new System.Drawing.Point(146, 80);
            this.europaWired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.europaWired_IpAddressControl.Name = "europaWired_IpAddressControl";
            this.europaWired_IpAddressControl.Size = new System.Drawing.Size(173, 20);
            this.europaWired_IpAddressControl.TabIndex = 2;
            this.europaWired_IpAddressControl.Text = "...";
            this.europaWired_IpAddressControl.TextChanged += new System.EventHandler(this.IPAddressControl_TextChanged);
            // 
            // europaWiredIP_Label
            // 
            this.europaWiredIP_Label.AutoSize = true;
            this.europaWiredIP_Label.Location = new System.Drawing.Point(23, 84);
            this.europaWiredIP_Label.Name = "europaWiredIP_Label";
            this.europaWiredIP_Label.Size = new System.Drawing.Size(88, 13);
            this.europaWiredIP_Label.TabIndex = 18;
            this.europaWiredIP_Label.Text = "Europa Wired IP:";
            // 
            // wirelessIpv4_IpAddressControl
            // 
            this.wirelessIpv4_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wirelessIpv4_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wirelessIpv4_IpAddressControl.Location = new System.Drawing.Point(145, 53);
            this.wirelessIpv4_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wirelessIpv4_IpAddressControl.Name = "wirelessIpv4_IpAddressControl";
            this.wirelessIpv4_IpAddressControl.Size = new System.Drawing.Size(173, 20);
            this.wirelessIpv4_IpAddressControl.TabIndex = 1;
            this.wirelessIpv4_IpAddressControl.Text = "...";
            this.wirelessIpv4_IpAddressControl.TextChanged += new System.EventHandler(this.IPAddressControl_TextChanged);
            // 
            // wiredIpv4Address_Label
            // 
            this.wiredIpv4Address_Label.AutoSize = true;
            this.wiredIpv4Address_Label.Location = new System.Drawing.Point(22, 30);
            this.wiredIpv4Address_Label.Name = "wiredIpv4Address_Label";
            this.wiredIpv4Address_Label.Size = new System.Drawing.Size(51, 13);
            this.wiredIpv4Address_Label.TabIndex = 1;
            this.wiredIpv4Address_Label.Text = "Wired IP:";
            // 
            // wiredIpv4_IpAddressControl
            // 
            this.wiredIpv4_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wiredIpv4_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wiredIpv4_IpAddressControl.Location = new System.Drawing.Point(145, 27);
            this.wiredIpv4_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wiredIpv4_IpAddressControl.Name = "wiredIpv4_IpAddressControl";
            this.wiredIpv4_IpAddressControl.Size = new System.Drawing.Size(173, 20);
            this.wiredIpv4_IpAddressControl.TabIndex = 0;
            this.wiredIpv4_IpAddressControl.Text = "...";
            this.wiredIpv4_IpAddressControl.TextChanged += new System.EventHandler(this.IPAddressControl_TextChanged);
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
            this.serverDetails_GroupBox.Controls.Add(this.kerberosServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.secondDHCPServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.kerberosServerIP_Label);
            this.serverDetails_GroupBox.Controls.Add(this.secondDHCPServerIP_Label);
            this.serverDetails_GroupBox.Location = new System.Drawing.Point(10, 652);
            this.serverDetails_GroupBox.Name = "serverDetails_GroupBox";
            this.serverDetails_GroupBox.Size = new System.Drawing.Size(345, 119);
            this.serverDetails_GroupBox.TabIndex = 33;
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
            this.primaryDhcpAddress_IpAddressControl.TabIndex = 7;
            this.primaryDhcpAddress_IpAddressControl.Text = "...";
            this.primaryDhcpAddress_IpAddressControl.TextChanged += new System.EventHandler(this.ipAddressControl_TextChanged);
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
            // kerberosServer_IpAddressControl
            // 
            this.kerberosServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.kerberosServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.kerberosServer_IpAddressControl.Location = new System.Drawing.Point(147, 81);
            this.kerberosServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.kerberosServer_IpAddressControl.Name = "kerberosServer_IpAddressControl";
            this.kerberosServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.kerberosServer_IpAddressControl.TabIndex = 9;
            this.kerberosServer_IpAddressControl.Text = "...";
            this.kerberosServer_IpAddressControl.TextChanged += new System.EventHandler(this.ipAddressControl_TextChanged);
            // 
            // secondDHCPServer_IpAddressControl
            // 
            this.secondDHCPServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondDHCPServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondDHCPServer_IpAddressControl.Location = new System.Drawing.Point(147, 54);
            this.secondDHCPServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondDHCPServer_IpAddressControl.Name = "secondDHCPServer_IpAddressControl";
            this.secondDHCPServer_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.secondDHCPServer_IpAddressControl.TabIndex = 8;
            this.secondDHCPServer_IpAddressControl.Text = "...";
            this.secondDHCPServer_IpAddressControl.TextChanged += new System.EventHandler(this.ipAddressControl_TextChanged);
            // 
            // kerberosServerIP_Label
            // 
            this.kerberosServerIP_Label.AutoSize = true;
            this.kerberosServerIP_Label.Location = new System.Drawing.Point(22, 84);
            this.kerberosServerIP_Label.Name = "kerberosServerIP_Label";
            this.kerberosServerIP_Label.Size = new System.Drawing.Size(99, 13);
            this.kerberosServerIP_Label.TabIndex = 61;
            this.kerberosServerIP_Label.Text = "Kerberos Server IP:";
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
            this.switchDetailsControl.Location = new System.Drawing.Point(362, 540);
            this.switchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.switchDetailsControl.Name = "switchDetailsControl";
            this.switchDetailsControl.Size = new System.Drawing.Size(355, 98);
            this.switchDetailsControl.SwitchIPAddress = "...";
            this.switchDetailsControl.SwitchPortNumber = 1;
            this.switchDetailsControl.TabIndex = 35;
            this.switchDetailsControl.ValidationRequired = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.windowsSecondaryIP_IpAddressControl);
            this.groupBox1.Controls.Add(this.windowsSecondary_lbl);
            this.groupBox1.Controls.Add(this.linuxFedoraClient_IpAddressControl);
            this.groupBox1.Controls.Add(this.linuxFedoraClientIP_lbl);
            this.groupBox1.Location = new System.Drawing.Point(10, 540);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 98);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Details";
            // 
            // windowsSecondaryIP_IpAddressControl
            // 
            this.windowsSecondaryIP_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.windowsSecondaryIP_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.windowsSecondaryIP_IpAddressControl.Location = new System.Drawing.Point(147, 27);
            this.windowsSecondaryIP_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.windowsSecondaryIP_IpAddressControl.Name = "windowsSecondaryIP_IpAddressControl";
            this.windowsSecondaryIP_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.windowsSecondaryIP_IpAddressControl.TabIndex = 4;
            this.windowsSecondaryIP_IpAddressControl.Text = "...";
            this.windowsSecondaryIP_IpAddressControl.TextChanged += new System.EventHandler(this.ipAddressControl_TextChanged);
            // 
            // windowsSecondary_lbl
            // 
            this.windowsSecondary_lbl.AutoSize = true;
            this.windowsSecondary_lbl.Location = new System.Drawing.Point(22, 30);
            this.windowsSecondary_lbl.Name = "windowsSecondary_lbl";
            this.windowsSecondary_lbl.Size = new System.Drawing.Size(121, 13);
            this.windowsSecondary_lbl.TabIndex = 64;
            this.windowsSecondary_lbl.Text = "Windows Secondary IP:";
            // 
            // linuxFedoraClient_IpAddressControl
            // 
            this.linuxFedoraClient_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.linuxFedoraClient_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.linuxFedoraClient_IpAddressControl.Location = new System.Drawing.Point(147, 54);
            this.linuxFedoraClient_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.linuxFedoraClient_IpAddressControl.Name = "linuxFedoraClient_IpAddressControl";
            this.linuxFedoraClient_IpAddressControl.Size = new System.Drawing.Size(176, 20);
            this.linuxFedoraClient_IpAddressControl.TabIndex = 5;
            this.linuxFedoraClient_IpAddressControl.Text = "...";
            this.linuxFedoraClient_IpAddressControl.TextChanged += new System.EventHandler(this.ipAddressControl_TextChanged);
            // 
            // linuxFedoraClientIP_lbl
            // 
            this.linuxFedoraClientIP_lbl.AutoSize = true;
            this.linuxFedoraClientIP_lbl.Location = new System.Drawing.Point(22, 57);
            this.linuxFedoraClientIP_lbl.Name = "linuxFedoraClientIP_lbl";
            this.linuxFedoraClientIP_lbl.Size = new System.Drawing.Size(113, 13);
            this.linuxFedoraClientIP_lbl.TabIndex = 9;
            this.linuxFedoraClientIP_lbl.Text = "Linux Fedora Client IP:";
            // 
            // print_PrintDriverSelector
            // 
            this.print_PrintDriverSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.print_PrintDriverSelector.DriverModel = "";
            this.print_PrintDriverSelector.DriverPackagePath = "";
            this.print_PrintDriverSelector.Location = new System.Drawing.Point(362, 654);
            this.print_PrintDriverSelector.MinimumSize = new System.Drawing.Size(314, 99);
            this.print_PrintDriverSelector.Name = "print_PrintDriverSelector";
            this.print_PrintDriverSelector.PrinterFamily = null;
            this.print_PrintDriverSelector.PrinterName = null;
            this.print_PrintDriverSelector.Size = new System.Drawing.Size(355, 117);
            this.print_PrintDriverSelector.TabIndex = 36;
            // 
            // message_CheckBox
            // 
            this.message_CheckBox.AutoSize = true;
            this.message_CheckBox.Location = new System.Drawing.Point(15, 806);
            this.message_CheckBox.Name = "message_CheckBox";
            this.message_CheckBox.Size = new System.Drawing.Size(90, 17);
            this.message_CheckBox.TabIndex = 63;
            this.message_CheckBox.Text = "Message Box";
            this.message_CheckBox.UseVisualStyleBackColor = true;
            this.message_CheckBox.CheckedChanged += new System.EventHandler(this.message_CheckBox_CheckedChanged);
            // 
            // IPSecurityConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.message_CheckBox);
            this.Controls.Add(this.print_PrintDriverSelector);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.serverDetails_GroupBox);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "IPSecurityConfigurationControl";
            this.Size = new System.Drawing.Size(749, 883);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.serverDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.print_PrintDriverSelector, 0);
            this.Controls.SetChildIndex(this.message_CheckBox, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            this.serverDetails_GroupBox.ResumeLayout(false);
            this.serverDetails_GroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SitemapVersionSelector sitemapVersionSelector;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private Framework.UI.IPAddressControl wirelessIpv4_IpAddressControl;
        private System.Windows.Forms.Label wiredIpv4Address_Label;
        private Framework.UI.IPAddressControl wiredIpv4_IpAddressControl;
        private System.Windows.Forms.Label wirelessIpv4Address_Label;
        private Framework.UI.IPAddressControl europaWired_IpAddressControl;
        private System.Windows.Forms.Label europaWiredIP_Label;
        private System.Windows.Forms.GroupBox serverDetails_GroupBox;
        private Framework.UI.IPAddressControl primaryDhcpAddress_IpAddressControl;
        private System.Windows.Forms.Label primaryDhcpAddress_Label;
        private Framework.UI.IPAddressControl kerberosServer_IpAddressControl;
        private Framework.UI.IPAddressControl secondDHCPServer_IpAddressControl;
        private System.Windows.Forms.Label kerberosServerIP_Label;
        private System.Windows.Forms.Label secondDHCPServerIP_Label;
        private SwitchDetailsControl switchDetailsControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private Framework.UI.IPAddressControl windowsSecondaryIP_IpAddressControl;
        private System.Windows.Forms.Label windowsSecondary_lbl;
        private Framework.UI.IPAddressControl linuxFedoraClient_IpAddressControl;
        private System.Windows.Forms.Label linuxFedoraClientIP_lbl;
        private PrintDriverSelector print_PrintDriverSelector;
        private System.Windows.Forms.CheckBox message_CheckBox;
        private System.Windows.Forms.RadioButton europa_RadioButton;
        private System.Windows.Forms.RadioButton wireless_RadioButton;
        private System.Windows.Forms.RadioButton wired_RadioButton;
    }
}
