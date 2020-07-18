using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.WiFiDirect
{
    partial class WiFiDirectConfigurationControl
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
            this.print_PrintDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.printerDetails1 = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrinterDetails();
            this.apDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.ap2Model_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap1Model_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap3Model_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap2Vendor_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap1Vendor_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap3Vendor_ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.accessPoint3_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.accessPoint2_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.apVendor_Label = new System.Windows.Forms.Label();
            this.accessPoint1_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.label1 = new System.Windows.Forms.Label();
            this.radiusServerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.rootSha2_RadioButton = new System.Windows.Forms.RadioButton();
            this.rootSha1_RadioButton = new System.Windows.Forms.RadioButton();
            this.rootSha2_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.rootSha2Ip_Label = new System.Windows.Forms.Label();
            this.rootSha1_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.rootSha1Ip_Label = new System.Windows.Forms.Label();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.debug_CheckBox = new System.Windows.Forms.CheckBox();
            this.apDetails_GroupBox.SuspendLayout();
            this.radiusServerDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // testCaseDetails_GroupBox
            // 
            this.testCaseDetails_GroupBox.Size = new System.Drawing.Size(740, 330);
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(432, 383);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = null;
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(308, 90);
            this.sitemapVersionSelector.TabIndex = 34;
            // 
            // print_PrintDriverSelector
            // 
            this.print_PrintDriverSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.print_PrintDriverSelector.DriverModel = "";
            this.print_PrintDriverSelector.DriverPackagePath = "";
            this.print_PrintDriverSelector.Location = new System.Drawing.Point(426, 480);
            this.print_PrintDriverSelector.MinimumSize = new System.Drawing.Size(314, 99);
            this.print_PrintDriverSelector.Name = "print_PrintDriverSelector";
            this.print_PrintDriverSelector.PrinterFamily = null;
            this.print_PrintDriverSelector.PrinterName = null;
            this.print_PrintDriverSelector.Size = new System.Drawing.Size(314, 99);
            this.print_PrintDriverSelector.TabIndex = 36;
            // 
            // printerDetails1
            // 
            this.printerDetails1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.printerDetails1.HideExecuteOnInterface = true;
            this.printerDetails1.HideMacAddress = true;
            this.printerDetails1.HidePrimaryInterfaceAddress = false;
            this.printerDetails1.HidePrimaryInterfacePortNumber = false;
            this.printerDetails1.HideSecondaryInterfaceAddress = true;
            this.printerDetails1.HideSecondaryInterfacePortNumber = true;
            this.printerDetails1.HideWirelessInterfaceAddress = false;
            this.printerDetails1.HideWirelessInterfacePortNumber = true;
            this.printerDetails1.Location = new System.Drawing.Point(0, 385);
            this.printerDetails1.MacAddress = "";
            this.printerDetails1.Name = "printerDetails1";
            this.printerDetails1.PrimaryInterfaceAddress = "";
            this.printerDetails1.PrimaryInterfacePortNumber = 0;
            this.printerDetails1.PrinterFamily = HP.ScalableTest.PluginSupport.Connectivity.Printer.PrinterFamilies.VEP;
            this.printerDetails1.PrinterInterface = HP.ScalableTest.PluginSupport.Connectivity.Printer.InterfaceMode.Wireless;
            this.printerDetails1.PrinterInterfaceType = HP.ScalableTest.PluginSupport.Connectivity.Printer.ProductType.MultipleInterface;
            this.printerDetails1.SecondaryInterfaceAddress = "";
            this.printerDetails1.SecondaryInterfacePortNumber = 0;
            this.printerDetails1.Size = new System.Drawing.Size(415, 247);
            this.printerDetails1.TabIndex = 64;
            this.printerDetails1.WirelessAddress = "";
            this.printerDetails1.WirelessInterfacePortNumber = 0;
            // 
            // apDetails_GroupBox
            // 
            this.apDetails_GroupBox.Controls.Add(this.ap2Model_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap1Model_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap3Model_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap2Vendor_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap1Vendor_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap3Vendor_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.label2);
            this.apDetails_GroupBox.Controls.Add(this.accessPoint3_IpAddressControl);
            this.apDetails_GroupBox.Controls.Add(this.accessPoint2_IpAddressControl);
            this.apDetails_GroupBox.Controls.Add(this.apVendor_Label);
            this.apDetails_GroupBox.Controls.Add(this.accessPoint1_IpAddressControl);
            this.apDetails_GroupBox.Controls.Add(this.label1);
            this.apDetails_GroupBox.Location = new System.Drawing.Point(0, 650);
            this.apDetails_GroupBox.Name = "apDetails_GroupBox";
            this.apDetails_GroupBox.Size = new System.Drawing.Size(415, 181);
            this.apDetails_GroupBox.TabIndex = 65;
            this.apDetails_GroupBox.TabStop = false;
            this.apDetails_GroupBox.Text = "Access Point Details";
            // 
            // ap2Model_ComboBox
            // 
            this.ap2Model_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap2Model_ComboBox.FormattingEnabled = true;
            this.ap2Model_ComboBox.Location = new System.Drawing.Point(307, 103);
            this.ap2Model_ComboBox.Name = "ap2Model_ComboBox";
            this.ap2Model_ComboBox.Size = new System.Drawing.Size(62, 21);
            this.ap2Model_ComboBox.TabIndex = 19;
            // 
            // ap1Model_ComboBox
            // 
            this.ap1Model_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap1Model_ComboBox.FormattingEnabled = true;
            this.ap1Model_ComboBox.Location = new System.Drawing.Point(307, 67);
            this.ap1Model_ComboBox.Name = "ap1Model_ComboBox";
            this.ap1Model_ComboBox.Size = new System.Drawing.Size(62, 21);
            this.ap1Model_ComboBox.TabIndex = 18;
            // 
            // ap3Model_ComboBox
            // 
            this.ap3Model_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap3Model_ComboBox.Enabled = false;
            this.ap3Model_ComboBox.FormattingEnabled = true;
            this.ap3Model_ComboBox.Location = new System.Drawing.Point(307, 138);
            this.ap3Model_ComboBox.Name = "ap3Model_ComboBox";
            this.ap3Model_ComboBox.Size = new System.Drawing.Size(62, 21);
            this.ap3Model_ComboBox.TabIndex = 17;
            // 
            // ap2Vendor_ComboBox
            // 
            this.ap2Vendor_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap2Vendor_ComboBox.FormattingEnabled = true;
            this.ap2Vendor_ComboBox.Location = new System.Drawing.Point(184, 103);
            this.ap2Vendor_ComboBox.Name = "ap2Vendor_ComboBox";
            this.ap2Vendor_ComboBox.Size = new System.Drawing.Size(80, 21);
            this.ap2Vendor_ComboBox.TabIndex = 16;
            this.ap2Vendor_ComboBox.SelectedIndexChanged += new System.EventHandler(this.accessPointVendor_ComboBox_SelectedIndexChanged);
            // 
            // ap1Vendor_ComboBox
            // 
            this.ap1Vendor_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap1Vendor_ComboBox.FormattingEnabled = true;
            this.ap1Vendor_ComboBox.Location = new System.Drawing.Point(184, 67);
            this.ap1Vendor_ComboBox.Name = "ap1Vendor_ComboBox";
            this.ap1Vendor_ComboBox.Size = new System.Drawing.Size(80, 21);
            this.ap1Vendor_ComboBox.TabIndex = 15;
            this.ap1Vendor_ComboBox.SelectedIndexChanged += new System.EventHandler(this.accessPointVendor_ComboBox_SelectedIndexChanged);
            // 
            // ap3Vendor_ComboBox
            // 
            this.ap3Vendor_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap3Vendor_ComboBox.Enabled = false;
            this.ap3Vendor_ComboBox.FormattingEnabled = true;
            this.ap3Vendor_ComboBox.Location = new System.Drawing.Point(184, 138);
            this.ap3Vendor_ComboBox.Name = "ap3Vendor_ComboBox";
            this.ap3Vendor_ComboBox.Size = new System.Drawing.Size(80, 21);
            this.ap3Vendor_ComboBox.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Model:";
            // 
            // accessPoint3_IpAddressControl
            // 
            this.accessPoint3_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.accessPoint3_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.accessPoint3_IpAddressControl.Enabled = false;
            this.accessPoint3_IpAddressControl.Location = new System.Drawing.Point(18, 138);
            this.accessPoint3_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.accessPoint3_IpAddressControl.Name = "accessPoint3_IpAddressControl";
            this.accessPoint3_IpAddressControl.Size = new System.Drawing.Size(134, 20);
            this.accessPoint3_IpAddressControl.TabIndex = 10;
            this.accessPoint3_IpAddressControl.Text = "...";
            // 
            // accessPoint2_IpAddressControl
            // 
            this.accessPoint2_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.accessPoint2_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.accessPoint2_IpAddressControl.Location = new System.Drawing.Point(18, 103);
            this.accessPoint2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.accessPoint2_IpAddressControl.Name = "accessPoint2_IpAddressControl";
            this.accessPoint2_IpAddressControl.Size = new System.Drawing.Size(134, 20);
            this.accessPoint2_IpAddressControl.TabIndex = 6;
            this.accessPoint2_IpAddressControl.Text = "...";
            // 
            // apVendor_Label
            // 
            this.apVendor_Label.AutoSize = true;
            this.apVendor_Label.Location = new System.Drawing.Point(181, 38);
            this.apVendor_Label.Name = "apVendor_Label";
            this.apVendor_Label.Size = new System.Drawing.Size(44, 13);
            this.apVendor_Label.TabIndex = 4;
            this.apVendor_Label.Text = "Vendor:";
            // 
            // accessPoint1_IpAddressControl
            // 
            this.accessPoint1_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.accessPoint1_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.accessPoint1_IpAddressControl.Location = new System.Drawing.Point(18, 68);
            this.accessPoint1_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.accessPoint1_IpAddressControl.Name = "accessPoint1_IpAddressControl";
            this.accessPoint1_IpAddressControl.Size = new System.Drawing.Size(134, 20);
            this.accessPoint1_IpAddressControl.TabIndex = 2;
            this.accessPoint1_IpAddressControl.Text = "...";
            //this.accessPoint1_IpAddressControl.Validating += new System.ComponentModel.CancelEventHandler(this.accessPoint1_IpAddressControl_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address:";
            // 
            // radiusServerDetails_GroupBox
            // 
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2Ip_Label);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1Ip_Label);
            this.radiusServerDetails_GroupBox.Location = new System.Drawing.Point(426, 587);
            this.radiusServerDetails_GroupBox.Name = "radiusServerDetails_GroupBox";
            this.radiusServerDetails_GroupBox.Size = new System.Drawing.Size(314, 125);
            this.radiusServerDetails_GroupBox.TabIndex = 66;
            this.radiusServerDetails_GroupBox.TabStop = false;
            this.radiusServerDetails_GroupBox.Text = "Radius Servers";
            // 
            // rootSha2_RadioButton
            // 
            this.rootSha2_RadioButton.AutoSize = true;
            this.rootSha2_RadioButton.Location = new System.Drawing.Point(229, 28);
            this.rootSha2_RadioButton.Name = "rootSha2_RadioButton";
            this.rootSha2_RadioButton.Size = new System.Drawing.Size(79, 17);
            this.rootSha2_RadioButton.TabIndex = 1;
            this.rootSha2_RadioButton.TabStop = true;
            this.rootSha2_RadioButton.Text = "Root SHA2";
            this.rootSha2_RadioButton.UseVisualStyleBackColor = true;
            // 
            // rootSha1_RadioButton
            // 
            this.rootSha1_RadioButton.AutoSize = true;
            this.rootSha1_RadioButton.Checked = true;
            this.rootSha1_RadioButton.Location = new System.Drawing.Point(123, 28);
            this.rootSha1_RadioButton.Name = "rootSha1_RadioButton";
            this.rootSha1_RadioButton.Size = new System.Drawing.Size(79, 17);
            this.rootSha1_RadioButton.TabIndex = 0;
            this.rootSha1_RadioButton.TabStop = true;
            this.rootSha1_RadioButton.Text = "Root SHA1";
            this.rootSha1_RadioButton.UseVisualStyleBackColor = true;

            // 
            // rootSha2_IpAddressControl
            // 
            this.rootSha2_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha2_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha2_IpAddressControl.Enabled = false;
            this.rootSha2_IpAddressControl.Location = new System.Drawing.Point(127, 87);
            this.rootSha2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha2_IpAddressControl.Name = "rootSha2_IpAddressControl";
            this.rootSha2_IpAddressControl.Size = new System.Drawing.Size(164, 20);
            this.rootSha2_IpAddressControl.TabIndex = 4;
            this.rootSha2_IpAddressControl.Tag = "Root SHA2 Server IP";
            this.rootSha2_IpAddressControl.Text = "...";
            // 
            // rootSha2Ip_Label
            // 
            this.rootSha2Ip_Label.AutoSize = true;
            this.rootSha2Ip_Label.Location = new System.Drawing.Point(10, 90);
            this.rootSha2Ip_Label.Name = "rootSha2Ip_Label";
            this.rootSha2Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha2Ip_Label.TabIndex = 59;
            this.rootSha2Ip_Label.Text = "Root SHA2 Server IP:";
            // 
            // rootSha1_IpAddressControl
            // 
            this.rootSha1_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha1_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha1_IpAddressControl.Location = new System.Drawing.Point(127, 60);
            this.rootSha1_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha1_IpAddressControl.Name = "rootSha1_IpAddressControl";
            this.rootSha1_IpAddressControl.Size = new System.Drawing.Size(164, 20);
            this.rootSha1_IpAddressControl.TabIndex = 3;
            this.rootSha1_IpAddressControl.Tag = "Root SHA1 Server IP";
            this.rootSha1_IpAddressControl.Text = "...";
            // 
            // rootSha1Ip_Label
            // 
            this.rootSha1Ip_Label.AutoSize = true;
            this.rootSha1Ip_Label.Location = new System.Drawing.Point(10, 63);
            this.rootSha1Ip_Label.Name = "rootSha1Ip_Label";
            this.rootSha1Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha1Ip_Label.TabIndex = 57;
            this.rootSha1Ip_Label.Text = "Root SHA1 Server IP:";
            // 
            // switchDetailsControl
            // 
            this.switchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.switchDetailsControl.HidePortNumber = true;
            this.switchDetailsControl.Location = new System.Drawing.Point(426, 727);
            this.switchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.switchDetailsControl.Name = "switchDetailsControl";
            this.switchDetailsControl.Size = new System.Drawing.Size(314, 90);
            this.switchDetailsControl.SwitchIPAddress = "...";
            this.switchDetailsControl.SwitchPortNumber = 1;
            this.switchDetailsControl.TabIndex = 35;
            this.switchDetailsControl.ValidationRequired = false;
            // 
            // debug_CheckBox
            // 
            this.debug_CheckBox.AutoSize = true;
            this.debug_CheckBox.Location = new System.Drawing.Point(15, 838);
            this.debug_CheckBox.Name = "debug_CheckBox";
            this.debug_CheckBox.Size = new System.Drawing.Size(78, 17);
            this.debug_CheckBox.TabIndex = 67;
            this.debug_CheckBox.Text = "Debugging";
            this.debug_CheckBox.UseVisualStyleBackColor = true;
            this.debug_CheckBox.CheckedChanged += new System.EventHandler(this.debug_CheckBox_CheckedChanged);
            // 
            // WirelessConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.debug_CheckBox);
            this.Controls.Add(this.radiusServerDetails_GroupBox);
            this.Controls.Add(this.printerDetails1);
            this.Controls.Add(this.apDetails_GroupBox);
            this.Controls.Add(this.print_PrintDriverSelector);
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WiFiDirectConfigurationControl";
            this.Size = new System.Drawing.Size(1243, 859);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.Controls.SetChildIndex(this.print_PrintDriverSelector, 0);
            this.Controls.SetChildIndex(this.apDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.printerDetails1, 0);
            this.Controls.SetChildIndex(this.radiusServerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.debug_CheckBox, 0);
            this.apDetails_GroupBox.ResumeLayout(false);
            this.apDetails_GroupBox.PerformLayout();
            this.radiusServerDetails_GroupBox.ResumeLayout(false);
            this.radiusServerDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SitemapVersionSelector sitemapVersionSelector;
        private PrintDriverSelector print_PrintDriverSelector;
        private PrinterDetails printerDetails1;
        private System.Windows.Forms.GroupBox apDetails_GroupBox;
        private System.Windows.Forms.Label apVendor_Label;
        private Framework.UI.IPAddressControl accessPoint1_IpAddressControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox radiusServerDetails_GroupBox;
        private System.Windows.Forms.RadioButton rootSha2_RadioButton;
        private System.Windows.Forms.RadioButton rootSha1_RadioButton;
        private Framework.UI.IPAddressControl rootSha2_IpAddressControl;
        private System.Windows.Forms.Label rootSha2Ip_Label;
        private Framework.UI.IPAddressControl rootSha1_IpAddressControl;
        private System.Windows.Forms.Label rootSha1Ip_Label;
        private Framework.UI.IPAddressControl accessPoint3_IpAddressControl;
        private Framework.UI.IPAddressControl accessPoint2_IpAddressControl;
        private SwitchDetailsControl switchDetailsControl;
        private System.Windows.Forms.ComboBox ap2Model_ComboBox;
        private System.Windows.Forms.ComboBox ap1Model_ComboBox;
        private System.Windows.Forms.ComboBox ap3Model_ComboBox;
        private System.Windows.Forms.ComboBox ap2Vendor_ComboBox;
        private System.Windows.Forms.ComboBox ap1Vendor_ComboBox;
        private System.Windows.Forms.ComboBox ap3Vendor_ComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox debug_CheckBox;
    }
}
