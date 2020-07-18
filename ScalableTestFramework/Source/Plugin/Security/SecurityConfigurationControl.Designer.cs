using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.Security
{
    partial class SecurityConfigurationControl
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
            this.sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.europaWirelessPortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.europaWiredPortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.europaWired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.europaWiredIP_Label = new System.Windows.Forms.Label();
            this.wiredIpv4Address_Label = new System.Windows.Forms.Label();
            this.wired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.europaWireless_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.europaWireless_Label = new System.Windows.Forms.Label();
            this.secondaryDHCP_Label = new System.Windows.Forms.Label();
            this.primaryDhcp_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.PrimaryDhcp_Label = new System.Windows.Forms.Label();
            this.SecondaryDhcp_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.serverDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.thirdDhcpServer_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.thirdDhcpServer_Label = new System.Windows.Forms.Label();
            this.security_SwitchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.security_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.printerDetails_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.europaWirelessPortNo_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.europaWiredPortNo_NumericUpDown)).BeginInit();
            this.serverDetails_GroupBox.SuspendLayout();
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
            this.sitemapVersionSelector.Location = new System.Drawing.Point(398, 524);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = null;
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(327, 90);
            this.sitemapVersionSelector.TabIndex = 9;
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.europaWirelessPortNo_NumericUpDown);
            this.printerDetails_GroupBox.Controls.Add(this.europaWiredPortNo_NumericUpDown);
            this.printerDetails_GroupBox.Controls.Add(this.wiredIpv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.europaWireless_Label);
            this.printerDetails_GroupBox.Controls.Add(this.europaWireless_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.europaWired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.europaWiredIP_Label);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(10, 383);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(383, 124);
            this.printerDetails_GroupBox.TabIndex = 0;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // europaWirelessPortNo_NumericUpDown
            // 
            this.europaWirelessPortNo_NumericUpDown.Enabled = false;
            this.europaWirelessPortNo_NumericUpDown.Location = new System.Drawing.Point(311, 92);
            this.europaWirelessPortNo_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.europaWirelessPortNo_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.europaWirelessPortNo_NumericUpDown.Name = "europaWirelessPortNo_NumericUpDown";
            this.europaWirelessPortNo_NumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.europaWirelessPortNo_NumericUpDown.TabIndex = 4;
            this.security_ToolTip.SetToolTip(this.europaWirelessPortNo_NumericUpDown, "Enter the wireless access point\'s port number");
            this.europaWirelessPortNo_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // europaWiredPortNo_NumericUpDown
            // 
            this.europaWiredPortNo_NumericUpDown.Enabled = false;
            this.europaWiredPortNo_NumericUpDown.Location = new System.Drawing.Point(311, 66);
            this.europaWiredPortNo_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.europaWiredPortNo_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.europaWiredPortNo_NumericUpDown.Name = "europaWiredPortNo_NumericUpDown";
            this.europaWiredPortNo_NumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.europaWiredPortNo_NumericUpDown.TabIndex = 2;
            this.security_ToolTip.SetToolTip(this.europaWiredPortNo_NumericUpDown, "Enter the secondary wired interface port number");
            this.europaWiredPortNo_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // europaWired_IpAddressControl
            // 
            this.europaWired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.europaWired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.europaWired_IpAddressControl.Enabled = false;
            this.europaWired_IpAddressControl.Location = new System.Drawing.Point(158, 65);
            this.europaWired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.europaWired_IpAddressControl.Name = "europaWired_IpAddressControl";
            this.europaWired_IpAddressControl.Size = new System.Drawing.Size(143, 20);
            this.europaWired_IpAddressControl.TabIndex = 1;
            this.europaWired_IpAddressControl.Tag = "Secondary Wired Interface IP ";
            this.europaWired_IpAddressControl.Text = "...";
            this.security_ToolTip.SetToolTip(this.europaWired_IpAddressControl, "Enter the secondary wired interface IP address");
            this.europaWired_IpAddressControl.TextChanged += new System.EventHandler(this.IPAddressControl_TextChanged);
            // 
            // europaWiredIP_Label
            // 
            this.europaWiredIP_Label.AutoSize = true;
            this.europaWiredIP_Label.Location = new System.Drawing.Point(21, 68);
            this.europaWiredIP_Label.Name = "europaWiredIP_Label";
            this.europaWiredIP_Label.Size = new System.Drawing.Size(137, 13);
            this.europaWiredIP_Label.TabIndex = 18;
            this.europaWiredIP_Label.Text = "Secondary Wired Interface:";
            this.europaWiredIP_Label.Visible = false;
            // 
            // wiredIpv4Address_Label
            // 
            this.wiredIpv4Address_Label.AutoSize = true;
            this.wiredIpv4Address_Label.Location = new System.Drawing.Point(21, 40);
            this.wiredIpv4Address_Label.Name = "wiredIpv4Address_Label";
            this.wiredIpv4Address_Label.Size = new System.Drawing.Size(120, 13);
            this.wiredIpv4Address_Label.TabIndex = 1;
            this.wiredIpv4Address_Label.Text = "Primary Wired Interface:";
            // 
            // wired_IpAddressControl
            // 
            this.wired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.wired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.wired_IpAddressControl.Location = new System.Drawing.Point(158, 37);
            this.wired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.wired_IpAddressControl.Name = "wired_IpAddressControl";
            this.wired_IpAddressControl.Size = new System.Drawing.Size(205, 20);
            this.wired_IpAddressControl.TabIndex = 0;
            this.wired_IpAddressControl.Text = "...";
            this.security_ToolTip.SetToolTip(this.wired_IpAddressControl, "Enter the primary wired interface IP address");
            this.wired_IpAddressControl.TextChanged += new System.EventHandler(this.IPAddressControl_TextChanged);
            // 
            // europaWireless_IpAddressControl
            // 
            this.europaWireless_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.europaWireless_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.europaWireless_IpAddressControl.Enabled = false;
            this.europaWireless_IpAddressControl.Location = new System.Drawing.Point(158, 91);
            this.europaWireless_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.europaWireless_IpAddressControl.Name = "europaWireless_IpAddressControl";
            this.europaWireless_IpAddressControl.Size = new System.Drawing.Size(143, 20);
            this.europaWireless_IpAddressControl.TabIndex = 3;
            this.europaWireless_IpAddressControl.Tag = "Wireless Interface IP";
            this.europaWireless_IpAddressControl.Text = "...";
            this.security_ToolTip.SetToolTip(this.europaWireless_IpAddressControl, "Enter the wireless interface IP address");
            this.europaWireless_IpAddressControl.TextChanged += new System.EventHandler(this.IPAddressControl_TextChanged);
            // 
            // europaWireless_Label
            // 
            this.europaWireless_Label.AutoSize = true;
            this.europaWireless_Label.Location = new System.Drawing.Point(21, 94);
            this.europaWireless_Label.Name = "europaWireless_Label";
            this.europaWireless_Label.Size = new System.Drawing.Size(95, 13);
            this.europaWireless_Label.TabIndex = 16;
            this.europaWireless_Label.Text = "Wireless Interface:";
            this.europaWireless_Label.Visible = false;
            // 
            // secondaryDHCP_Label
            // 
            this.secondaryDHCP_Label.AutoSize = true;
            this.secondaryDHCP_Label.Location = new System.Drawing.Point(22, 67);
            this.secondaryDHCP_Label.Name = "secondaryDHCP_Label";
            this.secondaryDHCP_Label.Size = new System.Drawing.Size(128, 13);
            this.secondaryDHCP_Label.TabIndex = 16;
            this.secondaryDHCP_Label.Text = "Secondary DHCP Server:";
            // 
            // primaryDhcp_IpAddressControl
            // 
            this.primaryDhcp_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.primaryDhcp_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryDhcp_IpAddressControl.Location = new System.Drawing.Point(156, 37);
            this.primaryDhcp_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.primaryDhcp_IpAddressControl.Name = "primaryDhcp_IpAddressControl";
            this.primaryDhcp_IpAddressControl.Size = new System.Drawing.Size(162, 20);
            this.primaryDhcp_IpAddressControl.TabIndex = 5;
            this.primaryDhcp_IpAddressControl.Text = "...";
            // 
            // PrimaryDhcp_Label
            // 
            this.PrimaryDhcp_Label.AutoSize = true;
            this.PrimaryDhcp_Label.Location = new System.Drawing.Point(22, 40);
            this.PrimaryDhcp_Label.Name = "PrimaryDhcp_Label";
            this.PrimaryDhcp_Label.Size = new System.Drawing.Size(111, 13);
            this.PrimaryDhcp_Label.TabIndex = 1;
            this.PrimaryDhcp_Label.Text = "Primary DHCP Server:";
            // 
            // SecondaryDhcp_IpAddressControl
            // 
            this.SecondaryDhcp_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.SecondaryDhcp_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SecondaryDhcp_IpAddressControl.Location = new System.Drawing.Point(156, 63);
            this.SecondaryDhcp_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.SecondaryDhcp_IpAddressControl.Name = "SecondaryDhcp_IpAddressControl";
            this.SecondaryDhcp_IpAddressControl.Size = new System.Drawing.Size(162, 20);
            this.SecondaryDhcp_IpAddressControl.TabIndex = 6;
            this.SecondaryDhcp_IpAddressControl.Text = "...";
            this.security_ToolTip.SetToolTip(this.SecondaryDhcp_IpAddressControl, "Enter the dhcp server address which provides secondary IP for the client.");
            // 
            // serverDetails_GroupBox
            // 
            this.serverDetails_GroupBox.Controls.Add(this.thirdDhcpServer_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.thirdDhcpServer_Label);
            this.serverDetails_GroupBox.Controls.Add(this.SecondaryDhcp_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.PrimaryDhcp_Label);
            this.serverDetails_GroupBox.Controls.Add(this.primaryDhcp_IpAddressControl);
            this.serverDetails_GroupBox.Controls.Add(this.secondaryDHCP_Label);
            this.serverDetails_GroupBox.Location = new System.Drawing.Point(399, 383);
            this.serverDetails_GroupBox.Name = "serverDetails_GroupBox";
            this.serverDetails_GroupBox.Size = new System.Drawing.Size(326, 124);
            this.serverDetails_GroupBox.TabIndex = 35;
            this.serverDetails_GroupBox.TabStop = false;
            this.serverDetails_GroupBox.Text = "Server Details";
            // 
            // thirdDhcpServer_IpAddressControl
            // 
            this.thirdDhcpServer_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.thirdDhcpServer_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.thirdDhcpServer_IpAddressControl.Enabled = false;
            this.thirdDhcpServer_IpAddressControl.Location = new System.Drawing.Point(156, 92);
            this.thirdDhcpServer_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.thirdDhcpServer_IpAddressControl.Name = "thirdDhcpServer_IpAddressControl";
            this.thirdDhcpServer_IpAddressControl.Size = new System.Drawing.Size(162, 20);
            this.thirdDhcpServer_IpAddressControl.TabIndex = 7;
            this.thirdDhcpServer_IpAddressControl.Text = "...";
            this.security_ToolTip.SetToolTip(this.thirdDhcpServer_IpAddressControl, "Enter the dhcp server IP which provides IP address for the wireless interface.");
            // 
            // thirdDhcpServer_Label
            // 
            this.thirdDhcpServer_Label.AutoSize = true;
            this.thirdDhcpServer_Label.Location = new System.Drawing.Point(22, 94);
            this.thirdDhcpServer_Label.Name = "thirdDhcpServer_Label";
            this.thirdDhcpServer_Label.Size = new System.Drawing.Size(101, 13);
            this.thirdDhcpServer_Label.TabIndex = 18;
            this.thirdDhcpServer_Label.Text = "Third DHCP Server:";
            // 
            // security_SwitchDetailsControl
            // 
            this.security_SwitchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.security_SwitchDetailsControl.Location = new System.Drawing.Point(10, 524);
            this.security_SwitchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.security_SwitchDetailsControl.Name = "security_SwitchDetailsControl";
            this.security_SwitchDetailsControl.Size = new System.Drawing.Size(383, 90);
            this.security_SwitchDetailsControl.SwitchIPAddress = "...";
            this.security_SwitchDetailsControl.SwitchPortNumber = 1;
            this.security_SwitchDetailsControl.TabIndex = 8;
            this.security_SwitchDetailsControl.ValidationRequired = true;
            // 
            // SecurityConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.security_SwitchDetailsControl);
            this.Controls.Add(this.serverDetails_GroupBox);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SecurityConfigurationControl";
            this.Size = new System.Drawing.Size(749, 791);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.serverDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.security_SwitchDetailsControl, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.europaWirelessPortNo_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.europaWiredPortNo_NumericUpDown)).EndInit();
            this.serverDetails_GroupBox.ResumeLayout(false);
            this.serverDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SitemapVersionSelector sitemapVersionSelector;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private Framework.UI.IPAddressControl europaWireless_IpAddressControl;
        private System.Windows.Forms.Label wiredIpv4Address_Label;
        private Framework.UI.IPAddressControl wired_IpAddressControl;
        private System.Windows.Forms.Label europaWireless_Label;
        private Framework.UI.IPAddressControl europaWired_IpAddressControl;
        private System.Windows.Forms.Label europaWiredIP_Label;
        private System.Windows.Forms.NumericUpDown europaWiredPortNo_NumericUpDown;
        private System.Windows.Forms.Label secondaryDHCP_Label;
        private Framework.UI.IPAddressControl primaryDhcp_IpAddressControl;
        private System.Windows.Forms.Label PrimaryDhcp_Label;
        private Framework.UI.IPAddressControl SecondaryDhcp_IpAddressControl;
        private System.Windows.Forms.GroupBox serverDetails_GroupBox;
        private System.Windows.Forms.NumericUpDown europaWirelessPortNo_NumericUpDown;
        private Framework.UI.IPAddressControl thirdDhcpServer_IpAddressControl;
        private System.Windows.Forms.Label thirdDhcpServer_Label;
        private SwitchDetailsControl security_SwitchDetailsControl;
        private System.Windows.Forms.ToolTip security_ToolTip;

    }
}
