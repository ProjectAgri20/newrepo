using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.WebProxy
{
    partial class WebProxyConfigurationControl
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
            this.wiredIPv4Address_Label = new System.Windows.Forms.Label();
            this.wiredIPv4_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.proxyServerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.secureProxyPort_TextBox = new System.Windows.Forms.TextBox();
            this.unsecureProxyPort_TextBox = new System.Windows.Forms.TextBox();
            this.cURLPath_TextBox = new System.Windows.Forms.TextBox();
            this.secureProxyPassword_TextBox = new System.Windows.Forms.TextBox();
            this.secureProxyUsername_TextBox = new System.Windows.Forms.TextBox();
            this.wpadServer_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.wpadServerAddress_Label = new System.Windows.Forms.Label();
            this.cURLPath_Label = new System.Windows.Forms.Label();
            this.secureProxyPassword_Label = new System.Windows.Forms.Label();
            this.unsecureProxy_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.unsecureProxyAddress_Label = new System.Windows.Forms.Label();
            this.secureProxy_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.secureProxyUsername_Label = new System.Windows.Forms.Label();
            this.secureProxyAddress_Label = new System.Windows.Forms.Label();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.dhcpServerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.secondaryDHCPServerAddress_Label = new System.Windows.Forms.Label();
            this.primaryDHCPServer_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.primaryDHCPServerAddress_Label = new System.Windows.Forms.Label();
            this.secondaryDHCPServer_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.printerDetails_GroupBox.SuspendLayout();
            this.proxyServerDetails_GroupBox.SuspendLayout();
            this.dhcpServerDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(362, 382);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = null;
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(355, 84);
            this.sitemapVersionSelector.TabIndex = 30;
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.wiredIPv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIPv4_IPAddressControl);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(7, 382);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(337, 67);
            this.printerDetails_GroupBox.TabIndex = 31;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // wiredIPv4Address_Label
            // 
            this.wiredIPv4Address_Label.AutoSize = true;
            this.wiredIPv4Address_Label.Location = new System.Drawing.Point(22, 30);
            this.wiredIPv4Address_Label.Name = "wiredIPv4Address_Label";
            this.wiredIPv4Address_Label.Size = new System.Drawing.Size(61, 13);
            this.wiredIPv4Address_Label.TabIndex = 1;
            this.wiredIPv4Address_Label.Text = "IP Address:";
            // 
            // wiredIPv4_IPAddressControl
            // 
            this.wiredIPv4_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wiredIPv4_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wiredIPv4_IPAddressControl.Location = new System.Drawing.Point(154, 27);
            this.wiredIPv4_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wiredIPv4_IPAddressControl.Name = "wiredIPv4_IPAddressControl";
            this.wiredIPv4_IPAddressControl.Size = new System.Drawing.Size(122, 20);
            this.wiredIPv4_IPAddressControl.TabIndex = 0;
            this.wiredIPv4_IPAddressControl.Text = "...";
            this.wiredIPv4_IPAddressControl.TextChanged += new System.EventHandler(this.wiredIPv4_IPAddressControl_TextChanged);
            // 
            // proxyServerDetails_GroupBox
            // 
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxyPort_TextBox);
            this.proxyServerDetails_GroupBox.Controls.Add(this.unsecureProxyPort_TextBox);
            this.proxyServerDetails_GroupBox.Controls.Add(this.cURLPath_TextBox);
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxyPassword_TextBox);
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxyUsername_TextBox);
            this.proxyServerDetails_GroupBox.Controls.Add(this.wpadServer_IPAddressControl);
            this.proxyServerDetails_GroupBox.Controls.Add(this.wpadServerAddress_Label);
            this.proxyServerDetails_GroupBox.Controls.Add(this.cURLPath_Label);
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxyPassword_Label);
            this.proxyServerDetails_GroupBox.Controls.Add(this.unsecureProxy_IPAddressControl);
            this.proxyServerDetails_GroupBox.Controls.Add(this.unsecureProxyAddress_Label);
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxy_IPAddressControl);
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxyUsername_Label);
            this.proxyServerDetails_GroupBox.Controls.Add(this.secureProxyAddress_Label);
            this.proxyServerDetails_GroupBox.Location = new System.Drawing.Point(7, 466);
            this.proxyServerDetails_GroupBox.Name = "proxyServerDetails_GroupBox";
            this.proxyServerDetails_GroupBox.Size = new System.Drawing.Size(337, 214);
            this.proxyServerDetails_GroupBox.TabIndex = 32;
            this.proxyServerDetails_GroupBox.TabStop = false;
            this.proxyServerDetails_GroupBox.Text = "Web Proxy Details";
            // 
            // secureProxyPort_TextBox
            // 
            this.secureProxyPort_TextBox.Location = new System.Drawing.Point(293, 53);
            this.secureProxyPort_TextBox.Name = "secureProxyPort_TextBox";
            this.secureProxyPort_TextBox.Size = new System.Drawing.Size(33, 20);
            this.secureProxyPort_TextBox.TabIndex = 75;
            // 
            // unsecureProxyPort_TextBox
            // 
            this.unsecureProxyPort_TextBox.Location = new System.Drawing.Point(293, 27);
            this.unsecureProxyPort_TextBox.Name = "unsecureProxyPort_TextBox";
            this.unsecureProxyPort_TextBox.Size = new System.Drawing.Size(33, 20);
            this.unsecureProxyPort_TextBox.TabIndex = 74;
            // 
            // cURLPath_TextBox
            // 
            this.cURLPath_TextBox.Location = new System.Drawing.Point(153, 145);
            this.cURLPath_TextBox.Name = "cURLPath_TextBox";
            this.cURLPath_TextBox.Size = new System.Drawing.Size(176, 20);
            this.cURLPath_TextBox.TabIndex = 73;
            // 
            // secureProxyPassword_TextBox
            // 
            this.secureProxyPassword_TextBox.Location = new System.Drawing.Point(120, 111);
            this.secureProxyPassword_TextBox.Name = "secureProxyPassword_TextBox";
            this.secureProxyPassword_TextBox.Size = new System.Drawing.Size(123, 20);
            this.secureProxyPassword_TextBox.TabIndex = 71;
            // 
            // secureProxyUsername_TextBox
            // 
            this.secureProxyUsername_TextBox.Location = new System.Drawing.Point(120, 85);
            this.secureProxyUsername_TextBox.Name = "secureProxyUsername_TextBox";
            this.secureProxyUsername_TextBox.Size = new System.Drawing.Size(123, 20);
            this.secureProxyUsername_TextBox.TabIndex = 70;
            // 
            // wpadServer_IPAddressControl
            // 
            this.wpadServer_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wpadServer_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wpadServer_IPAddressControl.Location = new System.Drawing.Point(153, 174);
            this.wpadServer_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wpadServer_IPAddressControl.Name = "wpadServer_IPAddressControl";
            this.wpadServer_IPAddressControl.Size = new System.Drawing.Size(123, 20);
            this.wpadServer_IPAddressControl.TabIndex = 68;
            this.wpadServer_IPAddressControl.Text = "...";
            // 
            // wpadServerAddress_Label
            // 
            this.wpadServerAddress_Label.AutoSize = true;
            this.wpadServerAddress_Label.Location = new System.Drawing.Point(22, 181);
            this.wpadServerAddress_Label.Name = "wpadServerAddress_Label";
            this.wpadServerAddress_Label.Size = new System.Drawing.Size(77, 13);
            this.wpadServerAddress_Label.TabIndex = 67;
            this.wpadServerAddress_Label.Text = "WPAD Server:";
            // 
            // cURLPath_Label
            // 
            this.cURLPath_Label.AutoSize = true;
            this.cURLPath_Label.Location = new System.Drawing.Point(22, 152);
            this.cURLPath_Label.Name = "cURLPath_Label";
            this.cURLPath_Label.Size = new System.Drawing.Size(38, 13);
            this.cURLPath_Label.TabIndex = 66;
            this.cURLPath_Label.Text = "cURL:";
            // 
            // secureProxyPassword_Label
            // 
            this.secureProxyPassword_Label.AutoSize = true;
            this.secureProxyPassword_Label.Location = new System.Drawing.Point(45, 114);
            this.secureProxyPassword_Label.Name = "secureProxyPassword_Label";
            this.secureProxyPassword_Label.Size = new System.Drawing.Size(56, 13);
            this.secureProxyPassword_Label.TabIndex = 65;
            this.secureProxyPassword_Label.Text = "Password:";
            // 
            // unsecureProxy_IPAddressControl
            // 
            this.unsecureProxy_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.unsecureProxy_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.unsecureProxy_IPAddressControl.Location = new System.Drawing.Point(153, 27);
            this.unsecureProxy_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.unsecureProxy_IPAddressControl.Name = "unsecureProxy_IPAddressControl";
            this.unsecureProxy_IPAddressControl.Size = new System.Drawing.Size(123, 20);
            this.unsecureProxy_IPAddressControl.TabIndex = 3;
            this.unsecureProxy_IPAddressControl.Text = "...";
            // 
            // unsecureProxyAddress_Label
            // 
            this.unsecureProxyAddress_Label.AutoSize = true;
            this.unsecureProxyAddress_Label.Location = new System.Drawing.Point(22, 30);
            this.unsecureProxyAddress_Label.Name = "unsecureProxyAddress_Label";
            this.unsecureProxyAddress_Label.Size = new System.Drawing.Size(125, 13);
            this.unsecureProxyAddress_Label.TabIndex = 64;
            this.unsecureProxyAddress_Label.Text = "Proxy Server (Unsecure):";
            // 
            // secureProxy_IPAddressControl
            // 
            this.secureProxy_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secureProxy_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secureProxy_IPAddressControl.Location = new System.Drawing.Point(153, 53);
            this.secureProxy_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secureProxy_IPAddressControl.Name = "secureProxy_IPAddressControl";
            this.secureProxy_IPAddressControl.Size = new System.Drawing.Size(123, 20);
            this.secureProxy_IPAddressControl.TabIndex = 4;
            this.secureProxy_IPAddressControl.Text = "...";
            // 
            // secureProxyUsername_Label
            // 
            this.secureProxyUsername_Label.AutoSize = true;
            this.secureProxyUsername_Label.Location = new System.Drawing.Point(43, 88);
            this.secureProxyUsername_Label.Name = "secureProxyUsername_Label";
            this.secureProxyUsername_Label.Size = new System.Drawing.Size(58, 13);
            this.secureProxyUsername_Label.TabIndex = 9;
            this.secureProxyUsername_Label.Text = "Username:";
            // 
            // secureProxyAddress_Label
            // 
            this.secureProxyAddress_Label.AutoSize = true;
            this.secureProxyAddress_Label.Location = new System.Drawing.Point(22, 57);
            this.secureProxyAddress_Label.Name = "secureProxyAddress_Label";
            this.secureProxyAddress_Label.Size = new System.Drawing.Size(113, 13);
            this.secureProxyAddress_Label.TabIndex = 9;
            this.secureProxyAddress_Label.Text = "Proxy Server (Secure):";
            // 
            // switchDetailsControl
            // 
            this.switchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.switchDetailsControl.HidePortNumber = false;
            this.switchDetailsControl.Location = new System.Drawing.Point(362, 590);
            this.switchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.switchDetailsControl.Name = "switchDetailsControl";
            this.switchDetailsControl.Size = new System.Drawing.Size(355, 90);
            this.switchDetailsControl.SwitchIPAddress = "...";
            this.switchDetailsControl.SwitchPortNumber = 1;
            this.switchDetailsControl.TabIndex = 33;
            this.switchDetailsControl.ValidationRequired = false;
            // 
            // dhcpServerDetails_GroupBox
            // 
            this.dhcpServerDetails_GroupBox.Controls.Add(this.secondaryDHCPServerAddress_Label);
            this.dhcpServerDetails_GroupBox.Controls.Add(this.primaryDHCPServer_IPAddressControl);
            this.dhcpServerDetails_GroupBox.Controls.Add(this.primaryDHCPServerAddress_Label);
            this.dhcpServerDetails_GroupBox.Controls.Add(this.secondaryDHCPServer_IPAddressControl);
            this.dhcpServerDetails_GroupBox.Location = new System.Drawing.Point(362, 485);
            this.dhcpServerDetails_GroupBox.Name = "dhcpServerDetails_GroupBox";
            this.dhcpServerDetails_GroupBox.Size = new System.Drawing.Size(355, 82);
            this.dhcpServerDetails_GroupBox.TabIndex = 65;
            this.dhcpServerDetails_GroupBox.TabStop = false;
            this.dhcpServerDetails_GroupBox.Text = "DHCP Server Details";
            // 
            // secondaryDHCPServerAddress_Label
            // 
            this.secondaryDHCPServerAddress_Label.AutoSize = true;
            this.secondaryDHCPServerAddress_Label.Location = new System.Drawing.Point(22, 55);
            this.secondaryDHCPServerAddress_Label.Name = "secondaryDHCPServerAddress_Label";
            this.secondaryDHCPServerAddress_Label.Size = new System.Drawing.Size(98, 13);
            this.secondaryDHCPServerAddress_Label.TabIndex = 65;
            this.secondaryDHCPServerAddress_Label.Text = "Secondary Server :";
            // 
            // primaryDHCPServer_IPAddressControl
            // 
            this.primaryDHCPServer_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.primaryDHCPServer_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryDHCPServer_IPAddressControl.Location = new System.Drawing.Point(147, 27);
            this.primaryDHCPServer_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.primaryDHCPServer_IPAddressControl.Name = "primaryDHCPServer_IPAddressControl";
            this.primaryDHCPServer_IPAddressControl.Size = new System.Drawing.Size(176, 20);
            this.primaryDHCPServer_IPAddressControl.TabIndex = 3;
            this.primaryDHCPServer_IPAddressControl.Text = "...";
            // 
            // primaryDHCPServerAddress_Label
            // 
            this.primaryDHCPServerAddress_Label.AutoSize = true;
            this.primaryDHCPServerAddress_Label.Location = new System.Drawing.Point(22, 30);
            this.primaryDHCPServerAddress_Label.Name = "primaryDHCPServerAddress_Label";
            this.primaryDHCPServerAddress_Label.Size = new System.Drawing.Size(84, 13);
            this.primaryDHCPServerAddress_Label.TabIndex = 64;
            this.primaryDHCPServerAddress_Label.Text = "Primary  Server :";
            // 
            // secondaryDHCPServer_IPAddressControl
            // 
            this.secondaryDHCPServer_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondaryDHCPServer_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondaryDHCPServer_IPAddressControl.Location = new System.Drawing.Point(147, 54);
            this.secondaryDHCPServer_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondaryDHCPServer_IPAddressControl.Name = "secondaryDHCPServer_IPAddressControl";
            this.secondaryDHCPServer_IPAddressControl.Size = new System.Drawing.Size(176, 20);
            this.secondaryDHCPServer_IPAddressControl.TabIndex = 4;
            this.secondaryDHCPServer_IPAddressControl.Text = "...";
            // 
            // WebProxyConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sitemapVersionSelector);
            this.Controls.Add(this.dhcpServerDetails_GroupBox);
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.proxyServerDetails_GroupBox);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Name = "WebProxyConfigurationControl";
            this.Size = new System.Drawing.Size(724, 730);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.proxyServerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.Controls.SetChildIndex(this.dhcpServerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            this.proxyServerDetails_GroupBox.ResumeLayout(false);
            this.proxyServerDetails_GroupBox.PerformLayout();
            this.dhcpServerDetails_GroupBox.ResumeLayout(false);
            this.dhcpServerDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SitemapVersionSelector sitemapVersionSelector;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private System.Windows.Forms.Label wiredIPv4Address_Label;
        private Framework.UI.IPAddressControl wiredIPv4_IPAddressControl;
        private System.Windows.Forms.GroupBox proxyServerDetails_GroupBox;
        private Framework.UI.IPAddressControl unsecureProxy_IPAddressControl;
        private System.Windows.Forms.Label unsecureProxyAddress_Label;
        private Framework.UI.IPAddressControl secureProxy_IPAddressControl;
        private System.Windows.Forms.Label secureProxyAddress_Label;
        private SwitchDetailsControl switchDetailsControl;
        private System.Windows.Forms.GroupBox dhcpServerDetails_GroupBox;
        private Framework.UI.IPAddressControl primaryDHCPServer_IPAddressControl;
        private System.Windows.Forms.Label primaryDHCPServerAddress_Label;
        private Framework.UI.IPAddressControl secondaryDHCPServer_IPAddressControl;
        private System.Windows.Forms.Label secureProxyUsername_Label;
        private System.Windows.Forms.TextBox cURLPath_TextBox;
        private System.Windows.Forms.TextBox secureProxyPassword_TextBox;
        private System.Windows.Forms.TextBox secureProxyUsername_TextBox;
        private Framework.UI.IPAddressControl wpadServer_IPAddressControl;
        private System.Windows.Forms.Label wpadServerAddress_Label;
        private System.Windows.Forms.Label cURLPath_Label;
        private System.Windows.Forms.Label secureProxyPassword_Label;
        private System.Windows.Forms.Label secondaryDHCPServerAddress_Label;
        private System.Windows.Forms.TextBox secureProxyPort_TextBox;
        private System.Windows.Forms.TextBox unsecureProxyPort_TextBox;
    }
}
