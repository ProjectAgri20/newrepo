using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.ConnectivityPrint
{
    partial class ConnectivityPrintConfigurationControl
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
            this.connectivity_Label = new System.Windows.Forms.Label();
            this.wireless_RadioButton = new System.Windows.Forms.RadioButton();
            this.wired_RadioButton = new System.Windows.Forms.RadioButton();
            this.stateFull_CheckBox = new System.Windows.Forms.CheckBox();
            this.stateLess_CheckBox = new System.Windows.Forms.CheckBox();
            this.linkLocal_CheckBox = new System.Windows.Forms.CheckBox();
            this.ipv6AddressType_Label = new System.Windows.Forms.Label();
            this.ipv4Address_Label = new System.Windows.Forms.Label();
            this.ipv4Address_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.print_PrintDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.printDocuments_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentsPath_Label = new System.Windows.Forms.Label();
            this.documentsPath_ComboBox = new System.Windows.Forms.ComboBox();
            this.printSwitchDetails_SwtchDetails = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.information_Label = new System.Windows.Forms.Label();
            this.sitemaps_SitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.paperLessMode_Label = new System.Windows.Forms.Label();
            this.paperlessMode_CheckBox = new System.Windows.Forms.CheckBox();
            this.printerDetails_GroupBox.SuspendLayout();
            this.printDocuments_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.connectivity_Label);
            this.printerDetails_GroupBox.Controls.Add(this.wireless_RadioButton);
            this.printerDetails_GroupBox.Controls.Add(this.wired_RadioButton);
            this.printerDetails_GroupBox.Controls.Add(this.stateFull_CheckBox);
            this.printerDetails_GroupBox.Controls.Add(this.stateLess_CheckBox);
            this.printerDetails_GroupBox.Controls.Add(this.linkLocal_CheckBox);
            this.printerDetails_GroupBox.Controls.Add(this.ipv6AddressType_Label);
            this.printerDetails_GroupBox.Controls.Add(this.ipv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.ipv4Address_IpAddressControl);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(0, 372);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(362, 125);
            this.printerDetails_GroupBox.TabIndex = 8;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // connectivity_Label
            // 
            this.connectivity_Label.AutoSize = true;
            this.connectivity_Label.Location = new System.Drawing.Point(17, 98);
            this.connectivity_Label.Name = "connectivity_Label";
            this.connectivity_Label.Size = new System.Drawing.Size(68, 13);
            this.connectivity_Label.TabIndex = 8;
            this.connectivity_Label.Text = "Connectivity:";
            // 
            // wireless_RadioButton
            // 
            this.wireless_RadioButton.AutoSize = true;
            this.wireless_RadioButton.Location = new System.Drawing.Point(176, 96);
            this.wireless_RadioButton.Name = "wireless_RadioButton";
            this.wireless_RadioButton.Size = new System.Drawing.Size(65, 17);
            this.wireless_RadioButton.TabIndex = 7;
            this.wireless_RadioButton.Text = "Wireless";
            this.wireless_RadioButton.UseVisualStyleBackColor = true;
            // 
            // wired_RadioButton
            // 
            this.wired_RadioButton.AutoSize = true;
            this.wired_RadioButton.Checked = true;
            this.wired_RadioButton.Location = new System.Drawing.Point(118, 96);
            this.wired_RadioButton.Name = "wired_RadioButton";
            this.wired_RadioButton.Size = new System.Drawing.Size(53, 17);
            this.wired_RadioButton.TabIndex = 6;
            this.wired_RadioButton.TabStop = true;
            this.wired_RadioButton.Text = "Wired";
            this.wired_RadioButton.UseVisualStyleBackColor = true;
            // 
            // stateFull_CheckBox
            // 
            this.stateFull_CheckBox.AutoSize = true;
            this.stateFull_CheckBox.Location = new System.Drawing.Point(274, 67);
            this.stateFull_CheckBox.Name = "stateFull_CheckBox";
            this.stateFull_CheckBox.Size = new System.Drawing.Size(67, 17);
            this.stateFull_CheckBox.TabIndex = 5;
            this.stateFull_CheckBox.Text = "State full";
            this.stateFull_CheckBox.UseVisualStyleBackColor = true;
            // 
            // stateLess_CheckBox
            // 
            this.stateLess_CheckBox.AutoSize = true;
            this.stateLess_CheckBox.Location = new System.Drawing.Point(199, 67);
            this.stateLess_CheckBox.Name = "stateLess_CheckBox";
            this.stateLess_CheckBox.Size = new System.Drawing.Size(69, 17);
            this.stateLess_CheckBox.TabIndex = 4;
            this.stateLess_CheckBox.Text = "Stateless";
            this.stateLess_CheckBox.UseVisualStyleBackColor = true;
            // 
            // linkLocal_CheckBox
            // 
            this.linkLocal_CheckBox.AutoSize = true;
            this.linkLocal_CheckBox.Location = new System.Drawing.Point(118, 67);
            this.linkLocal_CheckBox.Name = "linkLocal_CheckBox";
            this.linkLocal_CheckBox.Size = new System.Drawing.Size(75, 17);
            this.linkLocal_CheckBox.TabIndex = 3;
            this.linkLocal_CheckBox.Text = "Link Local";
            this.linkLocal_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ipv6AddressType_Label
            // 
            this.ipv6AddressType_Label.AutoSize = true;
            this.ipv6AddressType_Label.Location = new System.Drawing.Point(20, 71);
            this.ipv6AddressType_Label.Name = "ipv6AddressType_Label";
            this.ipv6AddressType_Label.Size = new System.Drawing.Size(100, 13);
            this.ipv6AddressType_Label.TabIndex = 2;
            this.ipv6AddressType_Label.Text = "IPv6 Address Type:";
            // 
            // ipv4Address_Label
            // 
            this.ipv4Address_Label.AutoSize = true;
            this.ipv4Address_Label.Location = new System.Drawing.Point(17, 35);
            this.ipv4Address_Label.Name = "ipv4Address_Label";
            this.ipv4Address_Label.Size = new System.Drawing.Size(73, 13);
            this.ipv4Address_Label.TabIndex = 1;
            this.ipv4Address_Label.Text = "IPv4 Address:";
            // 
            // ipv4Address_IpAddressControl
            // 
            this.ipv4Address_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipv4Address_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipv4Address_IpAddressControl.Location = new System.Drawing.Point(118, 32);
            this.ipv4Address_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipv4Address_IpAddressControl.Name = "ipv4Address_IpAddressControl";
            this.ipv4Address_IpAddressControl.Size = new System.Drawing.Size(136, 20);
            this.ipv4Address_IpAddressControl.TabIndex = 0;
            this.ipv4Address_IpAddressControl.Text = "...";
            this.toolTip.SetToolTip(this.ipv4Address_IpAddressControl, "Enter IPv4 address of the printer");
            // 
            // print_PrintDriverSelector
            // 
            this.print_PrintDriverSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.print_PrintDriverSelector.DriverModel = "";
            this.print_PrintDriverSelector.DriverPackagePath = "";
            this.print_PrintDriverSelector.Location = new System.Drawing.Point(368, 375);
            this.print_PrintDriverSelector.MinimumSize = new System.Drawing.Size(314, 99);
            this.print_PrintDriverSelector.Name = "print_PrintDriverSelector";
            this.print_PrintDriverSelector.PrinterFamily = null;
            this.print_PrintDriverSelector.PrinterName = null;
            this.print_PrintDriverSelector.Size = new System.Drawing.Size(355, 122);
            this.print_PrintDriverSelector.TabIndex = 9;
            // 
            // printDocuments_GroupBox
            // 
            this.printDocuments_GroupBox.Controls.Add(this.documentsPath_Label);
            this.printDocuments_GroupBox.Controls.Add(this.documentsPath_ComboBox);
            this.printDocuments_GroupBox.Location = new System.Drawing.Point(3, 505);
            this.printDocuments_GroupBox.Name = "printDocuments_GroupBox";
            this.printDocuments_GroupBox.Size = new System.Drawing.Size(359, 84);
            this.printDocuments_GroupBox.TabIndex = 10;
            this.printDocuments_GroupBox.TabStop = false;
            this.printDocuments_GroupBox.Text = "Print Documents";
            // 
            // documentsPath_Label
            // 
            this.documentsPath_Label.AutoSize = true;
            this.documentsPath_Label.Location = new System.Drawing.Point(17, 35);
            this.documentsPath_Label.Name = "documentsPath_Label";
            this.documentsPath_Label.Size = new System.Drawing.Size(89, 13);
            this.documentsPath_Label.TabIndex = 2;
            this.documentsPath_Label.Text = "Documents Path:";
            // 
            // documentsPath_ComboBox
            // 
            this.documentsPath_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.documentsPath_ComboBox.FormattingEnabled = true;
            this.documentsPath_ComboBox.Location = new System.Drawing.Point(112, 32);
            this.documentsPath_ComboBox.Name = "documentsPath_ComboBox";
            this.documentsPath_ComboBox.Size = new System.Drawing.Size(189, 21);
            this.documentsPath_ComboBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.documentsPath_ComboBox, "Select print documents folder");
            // 
            // printSwitchDetails_SwtchDetails
            // 
            this.printSwitchDetails_SwtchDetails.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.printSwitchDetails_SwtchDetails.Location = new System.Drawing.Point(368, 505);
            this.printSwitchDetails_SwtchDetails.MinimumSize = new System.Drawing.Size(222, 84);
            this.printSwitchDetails_SwtchDetails.Name = "printSwitchDetails_SwtchDetails";
            this.printSwitchDetails_SwtchDetails.Size = new System.Drawing.Size(355, 84);
            this.printSwitchDetails_SwtchDetails.SwitchIPAddress = "...";
            this.printSwitchDetails_SwtchDetails.SwitchPortNumber = 1;
            this.printSwitchDetails_SwtchDetails.TabIndex = 11;
            this.printSwitchDetails_SwtchDetails.ValidationRequired = false;
            // 
            // information_Label
            // 
            this.information_Label.AutoSize = true;
            this.information_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.information_Label.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.information_Label.Location = new System.Drawing.Point(3, 708);
            this.information_Label.Name = "information_Label";
            this.information_Label.Size = new System.Drawing.Size(407, 13);
            this.information_Label.TabIndex = 12;
            this.information_Label.Text = "Note: Please make sure to enable WS Discovery option for the printer.";
            // 
            // sitemaps_SitemapVersionSelector
            // 
            this.sitemaps_SitemapVersionSelector.AutoSize = true;
            this.sitemaps_SitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemaps_SitemapVersionSelector.Location = new System.Drawing.Point(7, 596);
            this.sitemaps_SitemapVersionSelector.Name = "sitemaps_SitemapVersionSelector";
            this.sitemaps_SitemapVersionSelector.PrinterFamily = null;
            this.sitemaps_SitemapVersionSelector.PrinterName = null;
            this.sitemaps_SitemapVersionSelector.SitemapPath = "";
            this.sitemaps_SitemapVersionSelector.SitemapVersion = "";
            this.sitemaps_SitemapVersionSelector.Size = new System.Drawing.Size(355, 102);
            this.sitemaps_SitemapVersionSelector.TabIndex = 13;
            // 
            // paperLessMode_Label
            // 
            this.paperLessMode_Label.AutoSize = true;
            this.paperLessMode_Label.Location = new System.Drawing.Point(375, 622);
            this.paperLessMode_Label.Name = "paperLessMode_Label";
            this.paperLessMode_Label.Size = new System.Drawing.Size(86, 13);
            this.paperLessMode_Label.TabIndex = 14;
            this.paperLessMode_Label.Text = "Paperless Mode:";
            // 
            // paperlessMode_CheckBox
            // 
            this.paperlessMode_CheckBox.AutoSize = true;
            this.paperlessMode_CheckBox.Location = new System.Drawing.Point(467, 622);
            this.paperlessMode_CheckBox.Name = "paperlessMode_CheckBox";
            this.paperlessMode_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.paperlessMode_CheckBox.TabIndex = 15;
            this.paperlessMode_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ConnectivityPrintConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.paperlessMode_CheckBox);
            this.Controls.Add(this.paperLessMode_Label);
            this.Controls.Add(this.sitemaps_SitemapVersionSelector);
            this.Controls.Add(this.information_Label);
            this.Controls.Add(this.printSwitchDetails_SwtchDetails);
            this.Controls.Add(this.printDocuments_GroupBox);
            this.Controls.Add(this.print_PrintDriverSelector);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Name = "ConnectivityPrintConfigurationControl";
            this.Size = new System.Drawing.Size(743, 724);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.ConnectivityPrintConfigurationControl_Validating);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.print_PrintDriverSelector, 0);
            this.Controls.SetChildIndex(this.printDocuments_GroupBox, 0);
            this.Controls.SetChildIndex(this.printSwitchDetails_SwtchDetails, 0);
            this.Controls.SetChildIndex(this.information_Label, 0);
            this.Controls.SetChildIndex(this.sitemaps_SitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.paperLessMode_Label, 0);
            this.Controls.SetChildIndex(this.paperlessMode_CheckBox, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            this.printDocuments_GroupBox.ResumeLayout(false);
            this.printDocuments_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private System.Windows.Forms.CheckBox stateFull_CheckBox;
        private System.Windows.Forms.CheckBox stateLess_CheckBox;
        private System.Windows.Forms.CheckBox linkLocal_CheckBox;
        private System.Windows.Forms.Label ipv6AddressType_Label;
        private System.Windows.Forms.Label ipv4Address_Label;
        private Framework.UI.IPAddressControl ipv4Address_IpAddressControl;
        private System.Windows.Forms.Label connectivity_Label;
        private System.Windows.Forms.RadioButton wireless_RadioButton;
        private System.Windows.Forms.RadioButton wired_RadioButton;
        private PrintDriverSelector print_PrintDriverSelector;
        private System.Windows.Forms.GroupBox printDocuments_GroupBox;
        private System.Windows.Forms.Label documentsPath_Label;
        private System.Windows.Forms.ComboBox documentsPath_ComboBox;
        private SwitchDetailsControl printSwitchDetails_SwtchDetails;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label information_Label;
        private SitemapVersionSelector sitemaps_SitemapVersionSelector;
        private System.Windows.Forms.Label paperLessMode_Label;
        private System.Windows.Forms.CheckBox paperlessMode_CheckBox;

    }
}
