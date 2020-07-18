using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.Wireless
{
	partial class WirelessConfigurationControl
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
            this.print_PrintDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.printerDetails1 = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrinterDetails();
            this.apDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.ap1Password_TextBox = new System.Windows.Forms.TextBox();
            this.ap2Password_TextBox = new System.Windows.Forms.TextBox();
            this.ap2Username_TextBox = new System.Windows.Forms.TextBox();
            this.ap1Username_TextBox = new System.Windows.Forms.TextBox();
            this.password_Label = new System.Windows.Forms.Label();
            this.userName_Label = new System.Windows.Forms.Label();
            this.ap2PortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ap1PortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.portNo_Label = new System.Windows.Forms.Label();
            this.frequency_ComboBox = new System.Windows.Forms.ComboBox();
            this.frequency_Label = new System.Windows.Forms.Label();
            this.ap2Model_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap1Model_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap2Vendor_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap1Vendor_ComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ap2_IpAddressControl = new Framework.UI.IPAddressControl();
            this.apVendor_Label = new System.Windows.Forms.Label();
            this.ap1_IpAddressControl = new Framework.UI.IPAddressControl();
            this.label1 = new System.Windows.Forms.Label();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.debug_CheckBox = new System.Windows.Forms.CheckBox();
            this.radiusServerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.rootSha2_RadioButton = new System.Windows.Forms.RadioButton();
            this.rootSha1_RadioButton = new System.Windows.Forms.RadioButton();
            this.rootSha2_IpAddressControl = new Framework.UI.IPAddressControl();
            this.rootSha2Ip_Label = new System.Windows.Forms.Label();
            this.rootSha1_IpAddressControl = new Framework.UI.IPAddressControl();
            this.rootSha1Ip_Label = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.powerSwitch_GroupBox = new System.Windows.Forms.GroupBox();
            this.powerSwitchIp_Label = new System.Windows.Forms.Label();
            this.powerSwitch_IpAddressControl = new Framework.UI.IPAddressControl();
            this.ap3Password_TextBox = new System.Windows.Forms.TextBox();
            this.ap3Username_TextBox = new System.Windows.Forms.TextBox();
            this.ap3PortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ap3Model_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap3Vendor_ComboBox = new System.Windows.Forms.ComboBox();
            this.ap3_IpAddressControl = new Framework.UI.IPAddressControl();
            this.apDetails_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ap2PortNo_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ap1PortNo_NumericUpDown)).BeginInit();
            this.radiusServerDetails_GroupBox.SuspendLayout();
            this.powerSwitch_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ap3PortNo_NumericUpDown)).BeginInit();
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
            this.sitemapVersionSelector.Location = new System.Drawing.Point(426, 383);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = "InkJet";
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(314, 90);
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
            this.apDetails_GroupBox.Controls.Add(this.ap3Password_TextBox);
            this.apDetails_GroupBox.Controls.Add(this.ap3Username_TextBox);
            this.apDetails_GroupBox.Controls.Add(this.ap3PortNo_NumericUpDown);
            this.apDetails_GroupBox.Controls.Add(this.ap3Model_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap3Vendor_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap3_IpAddressControl);
            this.apDetails_GroupBox.Controls.Add(this.ap1Password_TextBox);
            this.apDetails_GroupBox.Controls.Add(this.ap2Password_TextBox);
            this.apDetails_GroupBox.Controls.Add(this.ap2Username_TextBox);
            this.apDetails_GroupBox.Controls.Add(this.ap1Username_TextBox);
            this.apDetails_GroupBox.Controls.Add(this.password_Label);
            this.apDetails_GroupBox.Controls.Add(this.userName_Label);
            this.apDetails_GroupBox.Controls.Add(this.ap2PortNo_NumericUpDown);
            this.apDetails_GroupBox.Controls.Add(this.ap1PortNo_NumericUpDown);
            this.apDetails_GroupBox.Controls.Add(this.portNo_Label);
            this.apDetails_GroupBox.Controls.Add(this.frequency_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.frequency_Label);
            this.apDetails_GroupBox.Controls.Add(this.ap2Model_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap1Model_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap2Vendor_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.ap1Vendor_ComboBox);
            this.apDetails_GroupBox.Controls.Add(this.label2);
            this.apDetails_GroupBox.Controls.Add(this.ap2_IpAddressControl);
            this.apDetails_GroupBox.Controls.Add(this.apVendor_Label);
            this.apDetails_GroupBox.Controls.Add(this.ap1_IpAddressControl);
            this.apDetails_GroupBox.Controls.Add(this.label1);
            this.apDetails_GroupBox.Location = new System.Drawing.Point(0, 769);
            this.apDetails_GroupBox.Name = "apDetails_GroupBox";
            this.apDetails_GroupBox.Size = new System.Drawing.Size(740, 178);
            this.apDetails_GroupBox.TabIndex = 65;
            this.apDetails_GroupBox.TabStop = false;
            this.apDetails_GroupBox.Text = "Access Point Details";
            // 
            // ap1Password_TextBox
            // 
            this.ap1Password_TextBox.Location = new System.Drawing.Point(568, 83);
            this.ap1Password_TextBox.Name = "ap1Password_TextBox";
            this.ap1Password_TextBox.Size = new System.Drawing.Size(100, 20);
            this.ap1Password_TextBox.TabIndex = 34;
            this.ap1Password_TextBox.Text = "1iso*help";
            // 
            // ap2Password_TextBox
            // 
            this.ap2Password_TextBox.Location = new System.Drawing.Point(568, 115);
            this.ap2Password_TextBox.Name = "ap2Password_TextBox";
            this.ap2Password_TextBox.Size = new System.Drawing.Size(100, 20);
            this.ap2Password_TextBox.TabIndex = 33;
            this.ap2Password_TextBox.Text = "1iso*help";
            // 
            // ap2Username_TextBox
            // 
            this.ap2Username_TextBox.Location = new System.Drawing.Point(429, 114);
            this.ap2Username_TextBox.Name = "ap2Username_TextBox";
            this.ap2Username_TextBox.Size = new System.Drawing.Size(100, 20);
            this.ap2Username_TextBox.TabIndex = 32;
            this.ap2Username_TextBox.Text = "admin";
            // 
            // ap1Username_TextBox
            // 
            this.ap1Username_TextBox.Location = new System.Drawing.Point(429, 83);
            this.ap1Username_TextBox.Name = "ap1Username_TextBox";
            this.ap1Username_TextBox.Size = new System.Drawing.Size(100, 20);
            this.ap1Username_TextBox.TabIndex = 31;
            this.ap1Username_TextBox.Text = "admin";
            // 
            // password_Label
            // 
            this.password_Label.AutoSize = true;
            this.password_Label.Location = new System.Drawing.Point(605, 54);
            this.password_Label.Name = "password_Label";
            this.password_Label.Size = new System.Drawing.Size(53, 13);
            this.password_Label.TabIndex = 30;
            this.password_Label.Text = "Password";
            // 
            // userName_Label
            // 
            this.userName_Label.AutoSize = true;
            this.userName_Label.Location = new System.Drawing.Point(443, 54);
            this.userName_Label.Name = "userName_Label";
            this.userName_Label.Size = new System.Drawing.Size(60, 13);
            this.userName_Label.TabIndex = 29;
            this.userName_Label.Text = "UserName:";
            // 
            // ap2PortNo_NumericUpDown
            // 
            this.ap2PortNo_NumericUpDown.Location = new System.Drawing.Point(351, 115);
            this.ap2PortNo_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ap2PortNo_NumericUpDown.Name = "ap2PortNo_NumericUpDown";
            this.ap2PortNo_NumericUpDown.Size = new System.Drawing.Size(42, 20);
            this.ap2PortNo_NumericUpDown.TabIndex = 28;
            // 
            // ap1PortNo_NumericUpDown
            // 
            this.ap1PortNo_NumericUpDown.Location = new System.Drawing.Point(350, 84);
            this.ap1PortNo_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ap1PortNo_NumericUpDown.Name = "ap1PortNo_NumericUpDown";
            this.ap1PortNo_NumericUpDown.Size = new System.Drawing.Size(42, 20);
            this.ap1PortNo_NumericUpDown.TabIndex = 27;
            // 
            // portNo_Label
            // 
            this.portNo_Label.AutoSize = true;
            this.portNo_Label.Location = new System.Drawing.Point(347, 54);
            this.portNo_Label.Name = "portNo_Label";
            this.portNo_Label.Size = new System.Drawing.Size(46, 13);
            this.portNo_Label.TabIndex = 26;
            this.portNo_Label.Text = "Port No:";
            // 
            // frequency_ComboBox
            // 
            this.frequency_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.frequency_ComboBox.FormattingEnabled = true;
            this.frequency_ComboBox.Items.AddRange(new object[] {
            "2.4 GHz",
            "5 GHz"});
            this.frequency_ComboBox.Location = new System.Drawing.Point(660, 13);
            this.frequency_ComboBox.Name = "frequency_ComboBox";
            this.frequency_ComboBox.Size = new System.Drawing.Size(58, 21);
            this.frequency_ComboBox.TabIndex = 25;
            // 
            // frequency_Label
            // 
            this.frequency_Label.AutoSize = true;
            this.frequency_Label.Location = new System.Drawing.Point(588, 16);
            this.frequency_Label.Name = "frequency_Label";
            this.frequency_Label.Size = new System.Drawing.Size(60, 13);
            this.frequency_Label.TabIndex = 24;
            this.frequency_Label.Text = "Frequency:";
            // 
            // ap2Model_ComboBox
            // 
            this.ap2Model_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap2Model_ComboBox.FormattingEnabled = true;
            this.ap2Model_ComboBox.Location = new System.Drawing.Point(265, 114);
            this.ap2Model_ComboBox.Name = "ap2Model_ComboBox";
            this.ap2Model_ComboBox.Size = new System.Drawing.Size(62, 21);
            this.ap2Model_ComboBox.TabIndex = 19;
            // 
            // ap1Model_ComboBox
            // 
            this.ap1Model_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap1Model_ComboBox.FormattingEnabled = true;
            this.ap1Model_ComboBox.Location = new System.Drawing.Point(265, 83);
            this.ap1Model_ComboBox.Name = "ap1Model_ComboBox";
            this.ap1Model_ComboBox.Size = new System.Drawing.Size(62, 21);
            this.ap1Model_ComboBox.TabIndex = 18;
            // 
            // ap2Vendor_ComboBox
            // 
            this.ap2Vendor_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap2Vendor_ComboBox.FormattingEnabled = true;
            this.ap2Vendor_ComboBox.Location = new System.Drawing.Point(171, 114);
            this.ap2Vendor_ComboBox.Name = "ap2Vendor_ComboBox";
            this.ap2Vendor_ComboBox.Size = new System.Drawing.Size(80, 21);
            this.ap2Vendor_ComboBox.TabIndex = 16;
            this.ap2Vendor_ComboBox.SelectedIndexChanged += new System.EventHandler(this.accessPointVendor_ComboBox_SelectedIndexChanged);
            // 
            // ap1Vendor_ComboBox
            // 
            this.ap1Vendor_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap1Vendor_ComboBox.FormattingEnabled = true;
            this.ap1Vendor_ComboBox.Location = new System.Drawing.Point(171, 83);
            this.ap1Vendor_ComboBox.Name = "ap1Vendor_ComboBox";
            this.ap1Vendor_ComboBox.Size = new System.Drawing.Size(80, 21);
            this.ap1Vendor_ComboBox.TabIndex = 15;
            this.ap1Vendor_ComboBox.SelectedIndexChanged += new System.EventHandler(this.accessPointVendor_ComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Model:";
            // 
            // ap2_IpAddressControl
            // 
            this.ap2_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ap2_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ap2_IpAddressControl.Location = new System.Drawing.Point(18, 114);
            this.ap2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ap2_IpAddressControl.Name = "ap2_IpAddressControl";
            this.ap2_IpAddressControl.Size = new System.Drawing.Size(134, 20);
            this.ap2_IpAddressControl.TabIndex = 6;
            this.ap2_IpAddressControl.Text = "...";
            // 
            // apVendor_Label
            // 
            this.apVendor_Label.AutoSize = true;
            this.apVendor_Label.Location = new System.Drawing.Point(168, 54);
            this.apVendor_Label.Name = "apVendor_Label";
            this.apVendor_Label.Size = new System.Drawing.Size(44, 13);
            this.apVendor_Label.TabIndex = 4;
            this.apVendor_Label.Text = "Vendor:";
            // 
            // ap1_IpAddressControl
            // 
            this.ap1_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ap1_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ap1_IpAddressControl.Location = new System.Drawing.Point(18, 84);
            this.ap1_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ap1_IpAddressControl.Name = "ap1_IpAddressControl";
            this.ap1_IpAddressControl.Size = new System.Drawing.Size(134, 20);
            this.ap1_IpAddressControl.TabIndex = 2;
            this.ap1_IpAddressControl.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address:";
            // 
            // switchDetailsControl
            // 
            this.switchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.switchDetailsControl.HidePortNumber = true;
            this.switchDetailsControl.Location = new System.Drawing.Point(426, 585);
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
            this.debug_CheckBox.Location = new System.Drawing.Point(0, 947);
            this.debug_CheckBox.Name = "debug_CheckBox";
            this.debug_CheckBox.Size = new System.Drawing.Size(78, 17);
            this.debug_CheckBox.TabIndex = 67;
            this.debug_CheckBox.Text = "Debugging";
            this.debug_CheckBox.UseVisualStyleBackColor = true;
            this.debug_CheckBox.CheckedChanged += new System.EventHandler(this.debug_CheckBox_CheckedChanged);
            // 
            // radiusServerDetails_GroupBox
            // 
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2Ip_Label);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1Ip_Label);
            this.radiusServerDetails_GroupBox.Location = new System.Drawing.Point(0, 638);
            this.radiusServerDetails_GroupBox.Name = "radiusServerDetails_GroupBox";
            this.radiusServerDetails_GroupBox.Size = new System.Drawing.Size(415, 125);
            this.radiusServerDetails_GroupBox.TabIndex = 68;
            this.radiusServerDetails_GroupBox.TabStop = false;
            this.radiusServerDetails_GroupBox.Text = "Radius Servers";
            // 
            // rootSha2_RadioButton
            // 
            this.rootSha2_RadioButton.AutoSize = true;
            this.rootSha2_RadioButton.Location = new System.Drawing.Point(254, 28);
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
            this.rootSha1_RadioButton.Location = new System.Drawing.Point(152, 28);
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
            this.rootSha2_IpAddressControl.Location = new System.Drawing.Point(152, 87);
            this.rootSha2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha2_IpAddressControl.Name = "rootSha2_IpAddressControl";
            this.rootSha2_IpAddressControl.Size = new System.Drawing.Size(199, 20);
            this.rootSha2_IpAddressControl.TabIndex = 4;
            this.rootSha2_IpAddressControl.Tag = "Root SHA2 Server IP";
            this.rootSha2_IpAddressControl.Text = "...";
            // 
            // rootSha2Ip_Label
            // 
            this.rootSha2Ip_Label.AutoSize = true;
            this.rootSha2Ip_Label.Location = new System.Drawing.Point(17, 90);
            this.rootSha2Ip_Label.Name = "rootSha2Ip_Label";
            this.rootSha2Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha2Ip_Label.TabIndex = 59;
            this.rootSha2Ip_Label.Text = "Root SHA2 Server IP:";
            // 
            // rootSha1_IpAddressControl
            // 
            this.rootSha1_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha1_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha1_IpAddressControl.Location = new System.Drawing.Point(152, 60);
            this.rootSha1_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha1_IpAddressControl.Name = "rootSha1_IpAddressControl";
            this.rootSha1_IpAddressControl.Size = new System.Drawing.Size(199, 20);
            this.rootSha1_IpAddressControl.TabIndex = 3;
            this.rootSha1_IpAddressControl.Tag = "Root SHA1 Server IP";
            this.rootSha1_IpAddressControl.Text = "...";
            // 
            // rootSha1Ip_Label
            // 
            this.rootSha1Ip_Label.AutoSize = true;
            this.rootSha1Ip_Label.Location = new System.Drawing.Point(18, 63);
            this.rootSha1Ip_Label.Name = "rootSha1Ip_Label";
            this.rootSha1Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha1Ip_Label.TabIndex = 57;
            this.rootSha1Ip_Label.Text = "Root SHA1 Server IP:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // powerSwitch_GroupBox
            // 
            this.powerSwitch_GroupBox.Controls.Add(this.powerSwitch_IpAddressControl);
            this.powerSwitch_GroupBox.Controls.Add(this.powerSwitchIp_Label);
            this.powerSwitch_GroupBox.Location = new System.Drawing.Point(426, 696);
            this.powerSwitch_GroupBox.Name = "powerSwitch_GroupBox";
            this.powerSwitch_GroupBox.Size = new System.Drawing.Size(314, 60);
            this.powerSwitch_GroupBox.TabIndex = 69;
            this.powerSwitch_GroupBox.TabStop = false;
            this.powerSwitch_GroupBox.Text = "Power Switch";
            // 
            // powerSwitchIp_Label
            // 
            this.powerSwitchIp_Label.AutoSize = true;
            this.powerSwitchIp_Label.Location = new System.Drawing.Point(12, 33);
            this.powerSwitchIp_Label.Name = "powerSwitchIp_Label";
            this.powerSwitchIp_Label.Size = new System.Drawing.Size(64, 13);
            this.powerSwitchIp_Label.TabIndex = 59;
            this.powerSwitchIp_Label.Text = " IP Address:";
            // 
            // powerSwitch_IpAddressControl
            // 
            this.powerSwitch_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.powerSwitch_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.powerSwitch_IpAddressControl.Location = new System.Drawing.Point(82, 30);
            this.powerSwitch_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.powerSwitch_IpAddressControl.Name = "powerSwitch_IpAddressControl";
            this.powerSwitch_IpAddressControl.Size = new System.Drawing.Size(160, 20);
            this.powerSwitch_IpAddressControl.TabIndex = 4;
            this.powerSwitch_IpAddressControl.Tag = "Root SHA2 Server IP";
            this.powerSwitch_IpAddressControl.Text = "...";
            // 
            // ap3Password_TextBox
            // 
            this.ap3Password_TextBox.Location = new System.Drawing.Point(568, 147);
            this.ap3Password_TextBox.Name = "ap3Password_TextBox";
            this.ap3Password_TextBox.Size = new System.Drawing.Size(100, 20);
            this.ap3Password_TextBox.TabIndex = 40;
            this.ap3Password_TextBox.Text = "1iso*help";
            // 
            // ap3Username_TextBox
            // 
            this.ap3Username_TextBox.Location = new System.Drawing.Point(429, 146);
            this.ap3Username_TextBox.Name = "ap3Username_TextBox";
            this.ap3Username_TextBox.Size = new System.Drawing.Size(100, 20);
            this.ap3Username_TextBox.TabIndex = 39;
            this.ap3Username_TextBox.Text = "admin";
            // 
            // ap3PortNo_NumericUpDown
            // 
            this.ap3PortNo_NumericUpDown.Location = new System.Drawing.Point(351, 147);
            this.ap3PortNo_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ap3PortNo_NumericUpDown.Name = "ap3PortNo_NumericUpDown";
            this.ap3PortNo_NumericUpDown.Size = new System.Drawing.Size(42, 20);
            this.ap3PortNo_NumericUpDown.TabIndex = 38;
            // 
            // ap3Model_ComboBox
            // 
            this.ap3Model_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap3Model_ComboBox.FormattingEnabled = true;
            this.ap3Model_ComboBox.Location = new System.Drawing.Point(265, 146);
            this.ap3Model_ComboBox.Name = "ap3Model_ComboBox";
            this.ap3Model_ComboBox.Size = new System.Drawing.Size(62, 21);
            this.ap3Model_ComboBox.TabIndex = 37;
            // 
            // ap3Vendor_ComboBox
            // 
            this.ap3Vendor_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ap3Vendor_ComboBox.FormattingEnabled = true;
            this.ap3Vendor_ComboBox.Location = new System.Drawing.Point(171, 146);
            this.ap3Vendor_ComboBox.Name = "ap3Vendor_ComboBox";
            this.ap3Vendor_ComboBox.Size = new System.Drawing.Size(80, 21);
            this.ap3Vendor_ComboBox.TabIndex = 36;
            this.ap3Vendor_ComboBox.SelectedIndexChanged += new System.EventHandler(this.accessPointVendor_ComboBox_SelectedIndexChanged);
            // 
            // ap3_IpAddressControl
            // 
            this.ap3_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ap3_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ap3_IpAddressControl.Location = new System.Drawing.Point(18, 146);
            this.ap3_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ap3_IpAddressControl.Name = "ap3_IpAddressControl";
            this.ap3_IpAddressControl.Size = new System.Drawing.Size(134, 20);
            this.ap3_IpAddressControl.TabIndex = 35;
            this.ap3_IpAddressControl.Text = "...";
            // 
            // WirelessConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.powerSwitch_GroupBox);
            this.Controls.Add(this.radiusServerDetails_GroupBox);
            this.Controls.Add(this.debug_CheckBox);
            this.Controls.Add(this.printerDetails1);
            this.Controls.Add(this.apDetails_GroupBox);
            this.Controls.Add(this.print_PrintDriverSelector);
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WirelessConfigurationControl";
            this.Size = new System.Drawing.Size(751, 964);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.Controls.SetChildIndex(this.print_PrintDriverSelector, 0);
            this.Controls.SetChildIndex(this.apDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.printerDetails1, 0);
            this.Controls.SetChildIndex(this.debug_CheckBox, 0);
            this.Controls.SetChildIndex(this.radiusServerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.powerSwitch_GroupBox, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.apDetails_GroupBox.ResumeLayout(false);
            this.apDetails_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ap2PortNo_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ap1PortNo_NumericUpDown)).EndInit();
            this.radiusServerDetails_GroupBox.ResumeLayout(false);
            this.radiusServerDetails_GroupBox.PerformLayout();
            this.powerSwitch_GroupBox.ResumeLayout(false);
            this.powerSwitch_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ap3PortNo_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private SitemapVersionSelector sitemapVersionSelector;
		private PrintDriverSelector print_PrintDriverSelector;
		private PrinterDetails printerDetails1;
        private System.Windows.Forms.GroupBox apDetails_GroupBox;
        private System.Windows.Forms.Label apVendor_Label;
        private Framework.UI.IPAddressControl ap1_IpAddressControl;
        private System.Windows.Forms.Label label1;
		private Framework.UI.IPAddressControl ap2_IpAddressControl;
		private SwitchDetailsControl switchDetailsControl;
		private System.Windows.Forms.ComboBox ap2Model_ComboBox;
		private System.Windows.Forms.ComboBox ap1Model_ComboBox;
		private System.Windows.Forms.ComboBox ap2Vendor_ComboBox;
		private System.Windows.Forms.ComboBox ap1Vendor_ComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox debug_CheckBox;
        private System.Windows.Forms.ComboBox frequency_ComboBox;
        private System.Windows.Forms.Label frequency_Label;
        private System.Windows.Forms.Label portNo_Label;
        private System.Windows.Forms.NumericUpDown ap2PortNo_NumericUpDown;
        private System.Windows.Forms.NumericUpDown ap1PortNo_NumericUpDown;
        private System.Windows.Forms.GroupBox radiusServerDetails_GroupBox;
        private System.Windows.Forms.RadioButton rootSha2_RadioButton;
        private System.Windows.Forms.RadioButton rootSha1_RadioButton;
        private Framework.UI.IPAddressControl rootSha2_IpAddressControl;
        private System.Windows.Forms.Label rootSha2Ip_Label;
        private Framework.UI.IPAddressControl rootSha1_IpAddressControl;
        private System.Windows.Forms.Label rootSha1Ip_Label;
        private System.Windows.Forms.Label password_Label;
        private System.Windows.Forms.Label userName_Label;
        private System.Windows.Forms.TextBox ap1Password_TextBox;
        private System.Windows.Forms.TextBox ap2Password_TextBox;
        private System.Windows.Forms.TextBox ap2Username_TextBox;
        private System.Windows.Forms.TextBox ap1Username_TextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox powerSwitch_GroupBox;
        private Framework.UI.IPAddressControl powerSwitch_IpAddressControl;
        private System.Windows.Forms.Label powerSwitchIp_Label;
        private System.Windows.Forms.TextBox ap3Password_TextBox;
        private System.Windows.Forms.TextBox ap3Username_TextBox;
        private System.Windows.Forms.NumericUpDown ap3PortNo_NumericUpDown;
        private System.Windows.Forms.ComboBox ap3Model_ComboBox;
        private System.Windows.Forms.ComboBox ap3Vendor_ComboBox;
        private Framework.UI.IPAddressControl ap3_IpAddressControl;
    }
}
