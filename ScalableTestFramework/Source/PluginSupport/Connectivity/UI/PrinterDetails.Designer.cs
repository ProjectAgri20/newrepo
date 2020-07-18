using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    partial class PrinterDetails
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
			this.printerDetails_FieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
			this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
			this.productType_Label = new System.Windows.Forms.Label();
			this.wirelessInterfacePortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.multipleInterface_RadioButton = new System.Windows.Forms.RadioButton();
			this.singleInterface_RadioButton = new System.Windows.Forms.RadioButton();
			this.wirelessMacAddress_TextBox = new System.Windows.Forms.TextBox();
			this.secondaryInterfacePortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.primaryInterfacePortNo_NumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.wirelessAddress_Label = new System.Windows.Forms.Label();
			this.wirelessAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
			this.secondaryInterfaceAddress_Label = new System.Windows.Forms.Label();
			this.secondaryInterfaceAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
			this.wirelessMacAddress_Label = new System.Windows.Forms.Label();
			this.executeOn_Label = new System.Windows.Forms.Label();
			this.primaryAddress_Label = new System.Windows.Forms.Label();
			this.primaryAddress_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
			this.executeOn_GroupBox = new System.Windows.Forms.GroupBox();
			this.secondary_RadioButton = new System.Windows.Forms.RadioButton();
			this.wireless_RadioButton = new System.Windows.Forms.RadioButton();
			this.embedded_RadioButton = new System.Windows.Forms.RadioButton();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.printerDetails_GroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.wirelessInterfacePortNo_NumericUpDown)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.secondaryInterfacePortNo_NumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.primaryInterfacePortNo_NumericUpDown)).BeginInit();
			this.executeOn_GroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// printerDetails_GroupBox
			// 
			this.printerDetails_GroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.printerDetails_GroupBox.BackColor = System.Drawing.SystemColors.Control;
			this.printerDetails_GroupBox.Controls.Add(this.productType_Label);
			this.printerDetails_GroupBox.Controls.Add(this.wirelessInterfacePortNo_NumericUpDown);
			this.printerDetails_GroupBox.Controls.Add(this.groupBox1);
			this.printerDetails_GroupBox.Controls.Add(this.wirelessMacAddress_TextBox);
			this.printerDetails_GroupBox.Controls.Add(this.secondaryInterfacePortNo_NumericUpDown);
			this.printerDetails_GroupBox.Controls.Add(this.primaryInterfacePortNo_NumericUpDown);
			this.printerDetails_GroupBox.Controls.Add(this.wirelessAddress_Label);
			this.printerDetails_GroupBox.Controls.Add(this.wirelessAddress_IpAddressControl);
			this.printerDetails_GroupBox.Controls.Add(this.secondaryInterfaceAddress_Label);
			this.printerDetails_GroupBox.Controls.Add(this.secondaryInterfaceAddress_IpAddressControl);
			this.printerDetails_GroupBox.Controls.Add(this.wirelessMacAddress_Label);
			this.printerDetails_GroupBox.Controls.Add(this.executeOn_Label);
			this.printerDetails_GroupBox.Controls.Add(this.primaryAddress_Label);
			this.printerDetails_GroupBox.Controls.Add(this.primaryAddress_IPAddressControl);
			this.printerDetails_GroupBox.Controls.Add(this.executeOn_GroupBox);
			this.printerDetails_GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.printerDetails_GroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.printerDetails_GroupBox.Location = new System.Drawing.Point(0, 0);
			this.printerDetails_GroupBox.MinimumSize = new System.Drawing.Size(415, 245);
			this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
			this.printerDetails_GroupBox.Size = new System.Drawing.Size(415, 245);
			this.printerDetails_GroupBox.TabIndex = 5;
			this.printerDetails_GroupBox.TabStop = false;
			this.printerDetails_GroupBox.Text = "Printer Details";
			// 
			// productType_Label
			// 
			this.productType_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.productType_Label.AutoSize = true;
			this.productType_Label.Location = new System.Drawing.Point(22, 37);
			this.productType_Label.Name = "productType_Label";
			this.productType_Label.Size = new System.Drawing.Size(74, 13);
			this.productType_Label.TabIndex = 0;
			this.productType_Label.Text = "Product Type:";
			// 
			// wirelessInterfacePortNo_NumericUpDown
			// 
			this.wirelessInterfacePortNo_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wirelessInterfacePortNo_NumericUpDown.AutoSize = true;
			this.wirelessInterfacePortNo_NumericUpDown.Location = new System.Drawing.Point(353, 137);
			this.wirelessInterfacePortNo_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.wirelessInterfacePortNo_NumericUpDown.Name = "wirelessInterfacePortNo_NumericUpDown";
			this.wirelessInterfacePortNo_NumericUpDown.ReadOnly = true;
			this.wirelessInterfacePortNo_NumericUpDown.Size = new System.Drawing.Size(44, 20);
			this.wirelessInterfacePortNo_NumericUpDown.TabIndex = 29;
			this.toolTip.SetToolTip(this.wirelessInterfacePortNo_NumericUpDown, "Port number where wireless access point is connected");
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.multipleInterface_RadioButton);
			this.groupBox1.Controls.Add(this.singleInterface_RadioButton);
			this.groupBox1.Location = new System.Drawing.Point(176, 25);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(139, 30);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// multipleInterface_RadioButton
			// 
			this.multipleInterface_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.multipleInterface_RadioButton.AutoSize = true;
			this.multipleInterface_RadioButton.Location = new System.Drawing.Point(21, 10);
			this.multipleInterface_RadioButton.Name = "multipleInterface_RadioButton";
			this.multipleInterface_RadioButton.Size = new System.Drawing.Size(37, 17);
			this.multipleInterface_RadioButton.TabIndex = 2;
			this.multipleInterface_RadioButton.Tag = "Interface";
			this.multipleInterface_RadioButton.Text = "MI";
			this.multipleInterface_RadioButton.UseVisualStyleBackColor = true;
			this.multipleInterface_RadioButton.CheckedChanged += new System.EventHandler(this.multipleInterface_RadioButton_CheckedChanged);
			// 
			// singleInterface_RadioButton
			// 
			this.singleInterface_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.singleInterface_RadioButton.AutoSize = true;
			this.singleInterface_RadioButton.Location = new System.Drawing.Point(86, 10);
			this.singleInterface_RadioButton.Name = "singleInterface_RadioButton";
			this.singleInterface_RadioButton.Size = new System.Drawing.Size(35, 17);
			this.singleInterface_RadioButton.TabIndex = 3;
			this.singleInterface_RadioButton.Tag = "Interface";
			this.singleInterface_RadioButton.Text = "SI";
			this.singleInterface_RadioButton.UseVisualStyleBackColor = true;
			this.singleInterface_RadioButton.CheckedChanged += new System.EventHandler(this.multipleInterface_RadioButton_CheckedChanged);
			// 
			// wirelessMacAddress_TextBox
			// 
			this.wirelessMacAddress_TextBox.Location = new System.Drawing.Point(176, 172);
			this.wirelessMacAddress_TextBox.Name = "wirelessMacAddress_TextBox";
			this.wirelessMacAddress_TextBox.Size = new System.Drawing.Size(154, 20);
			this.wirelessMacAddress_TextBox.TabIndex = 26;
			this.wirelessMacAddress_TextBox.Tag = "Wireless mac address";
			this.wirelessMacAddress_TextBox.TextChanged += new System.EventHandler(this.wirelessMacAddress_TextBox_TextChanged);
			// 
			// secondaryInterfacePortNo_NumericUpDown
			// 
			this.secondaryInterfacePortNo_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondaryInterfacePortNo_NumericUpDown.AutoSize = true;
			this.secondaryInterfacePortNo_NumericUpDown.Location = new System.Drawing.Point(353, 102);
			this.secondaryInterfacePortNo_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.secondaryInterfacePortNo_NumericUpDown.Name = "secondaryInterfacePortNo_NumericUpDown";
			this.secondaryInterfacePortNo_NumericUpDown.ReadOnly = true;
			this.secondaryInterfacePortNo_NumericUpDown.Size = new System.Drawing.Size(44, 20);
			this.secondaryInterfacePortNo_NumericUpDown.TabIndex = 24;
			this.toolTip.SetToolTip(this.secondaryInterfacePortNo_NumericUpDown, "Port number where secondary wired interface is connected");
			// 
			// primaryInterfacePortNo_NumericUpDown
			// 
			this.primaryInterfacePortNo_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.primaryInterfacePortNo_NumericUpDown.AutoSize = true;
			this.primaryInterfacePortNo_NumericUpDown.Location = new System.Drawing.Point(353, 68);
			this.primaryInterfacePortNo_NumericUpDown.Name = "primaryInterfacePortNo_NumericUpDown";
			this.primaryInterfacePortNo_NumericUpDown.ReadOnly = true;
			this.primaryInterfacePortNo_NumericUpDown.Size = new System.Drawing.Size(44, 20);
			this.primaryInterfacePortNo_NumericUpDown.TabIndex = 23;
			this.primaryInterfacePortNo_NumericUpDown.Tag = "Primary interface port number";
			this.toolTip.SetToolTip(this.primaryInterfacePortNo_NumericUpDown, "Port number where primary wired interface is connected");
			// 
			// wirelessAddress_Label
			// 
			this.wirelessAddress_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wirelessAddress_Label.AutoSize = true;
			this.wirelessAddress_Label.Location = new System.Drawing.Point(22, 140);
			this.wirelessAddress_Label.Name = "wirelessAddress_Label";
			this.wirelessAddress_Label.Size = new System.Drawing.Size(91, 13);
			this.wirelessAddress_Label.TabIndex = 22;
			this.wirelessAddress_Label.Text = "Wireless Address:";
			// 
			// wirelessAddress_IpAddressControl
			// 
			this.wirelessAddress_IpAddressControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wirelessAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
			this.wirelessAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.wirelessAddress_IpAddressControl.Location = new System.Drawing.Point(176, 137);
			this.wirelessAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
			this.wirelessAddress_IpAddressControl.Name = "wirelessAddress_IpAddressControl";
			this.wirelessAddress_IpAddressControl.Size = new System.Drawing.Size(154, 20);
			this.wirelessAddress_IpAddressControl.TabIndex = 21;
			this.wirelessAddress_IpAddressControl.Tag = "Wireless interface IP address";
			this.wirelessAddress_IpAddressControl.Text = "...";
			this.toolTip.SetToolTip(this.wirelessAddress_IpAddressControl, "Wireless interface IP address");
			this.wirelessAddress_IpAddressControl.TextChanged += new System.EventHandler(this.primaryAddress_IPAddressControl_TextChanged);
			// 
			// secondaryInterfaceAddress_Label
			// 
			this.secondaryInterfaceAddress_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondaryInterfaceAddress_Label.AutoSize = true;
			this.secondaryInterfaceAddress_Label.Location = new System.Drawing.Point(22, 105);
			this.secondaryInterfaceAddress_Label.Name = "secondaryInterfaceAddress_Label";
			this.secondaryInterfaceAddress_Label.Size = new System.Drawing.Size(147, 13);
			this.secondaryInterfaceAddress_Label.TabIndex = 20;
			this.secondaryInterfaceAddress_Label.Text = "Secondary Interface Address:";
			// 
			// secondaryInterfaceAddress_IpAddressControl
			// 
			this.secondaryInterfaceAddress_IpAddressControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondaryInterfaceAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
			this.secondaryInterfaceAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.secondaryInterfaceAddress_IpAddressControl.Location = new System.Drawing.Point(176, 102);
			this.secondaryInterfaceAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
			this.secondaryInterfaceAddress_IpAddressControl.Name = "secondaryInterfaceAddress_IpAddressControl";
			this.secondaryInterfaceAddress_IpAddressControl.Size = new System.Drawing.Size(154, 20);
			this.secondaryInterfaceAddress_IpAddressControl.TabIndex = 19;
			this.secondaryInterfaceAddress_IpAddressControl.Tag = "Secondary interface IP address";
			this.secondaryInterfaceAddress_IpAddressControl.Text = "...";
			this.toolTip.SetToolTip(this.secondaryInterfaceAddress_IpAddressControl, "Secondary wired interface IP address");
			this.secondaryInterfaceAddress_IpAddressControl.TextChanged += new System.EventHandler(this.primaryAddress_IPAddressControl_TextChanged);
			// 
			// wirelessMacAddress_Label
			// 
			this.wirelessMacAddress_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wirelessMacAddress_Label.AutoSize = true;
			this.wirelessMacAddress_Label.Location = new System.Drawing.Point(22, 175);
			this.wirelessMacAddress_Label.Name = "wirelessMacAddress_Label";
			this.wirelessMacAddress_Label.Size = new System.Drawing.Size(115, 13);
			this.wirelessMacAddress_Label.TabIndex = 18;
			this.wirelessMacAddress_Label.Text = "Wireless Mac Address:";
			// 
			// executeOn_Label
			// 
			this.executeOn_Label.AutoSize = true;
			this.executeOn_Label.Location = new System.Drawing.Point(22, 212);
			this.executeOn_Label.Name = "executeOn_Label";
			this.executeOn_Label.Size = new System.Drawing.Size(66, 13);
			this.executeOn_Label.TabIndex = 14;
			this.executeOn_Label.Text = "Execute On:";
			// 
			// primaryAddress_Label
			// 
			this.primaryAddress_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.primaryAddress_Label.AutoSize = true;
			this.primaryAddress_Label.Location = new System.Drawing.Point(22, 70);
			this.primaryAddress_Label.Name = "primaryAddress_Label";
			this.primaryAddress_Label.Size = new System.Drawing.Size(130, 13);
			this.primaryAddress_Label.TabIndex = 4;
			this.primaryAddress_Label.Text = "Primary Interface Address:";
			// 
			// primaryAddress_IPAddressControl
			// 
			this.primaryAddress_IPAddressControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.primaryAddress_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
			this.primaryAddress_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.primaryAddress_IPAddressControl.Location = new System.Drawing.Point(176, 67);
			this.primaryAddress_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
			this.primaryAddress_IPAddressControl.Name = "primaryAddress_IPAddressControl";
			this.primaryAddress_IPAddressControl.Size = new System.Drawing.Size(154, 20);
			this.primaryAddress_IPAddressControl.TabIndex = 0;
			this.primaryAddress_IPAddressControl.Tag = "Primary interface IP address";
			this.primaryAddress_IPAddressControl.Text = "...";
			this.toolTip.SetToolTip(this.primaryAddress_IPAddressControl, "Primary wired interface IP address");
			this.primaryAddress_IPAddressControl.TextChanged += new System.EventHandler(this.primaryAddress_IPAddressControl_TextChanged);
			// 
			// executeOn_GroupBox
			// 
			this.executeOn_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.executeOn_GroupBox.Controls.Add(this.secondary_RadioButton);
			this.executeOn_GroupBox.Controls.Add(this.wireless_RadioButton);
			this.executeOn_GroupBox.Controls.Add(this.embedded_RadioButton);
			this.executeOn_GroupBox.Location = new System.Drawing.Point(179, 198);
			this.executeOn_GroupBox.Name = "executeOn_GroupBox";
			this.executeOn_GroupBox.Size = new System.Drawing.Size(226, 31);
			this.executeOn_GroupBox.TabIndex = 28;
			this.executeOn_GroupBox.TabStop = false;
			// 
			// secondary_RadioButton
			// 
			this.secondary_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.secondary_RadioButton.AutoSize = true;
			this.secondary_RadioButton.Location = new System.Drawing.Point(71, 10);
			this.secondary_RadioButton.Name = "secondary_RadioButton";
			this.secondary_RadioButton.Size = new System.Drawing.Size(76, 17);
			this.secondary_RadioButton.TabIndex = 3;
			this.secondary_RadioButton.Tag = "Connectivity";
			this.secondary_RadioButton.Text = "Secondary";
			this.secondary_RadioButton.UseVisualStyleBackColor = true;
			this.secondary_RadioButton.CheckedChanged += new System.EventHandler(this.embedded_RadioButton_CheckedChanged);
			// 
			// wireless_RadioButton
			// 
			this.wireless_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wireless_RadioButton.AutoSize = true;
			this.wireless_RadioButton.Location = new System.Drawing.Point(153, 10);
			this.wireless_RadioButton.Name = "wireless_RadioButton";
			this.wireless_RadioButton.Size = new System.Drawing.Size(65, 17);
			this.wireless_RadioButton.TabIndex = 2;
			this.wireless_RadioButton.Tag = "Connectivity";
			this.wireless_RadioButton.Text = "Wireless";
			this.wireless_RadioButton.UseVisualStyleBackColor = true;
			this.wireless_RadioButton.CheckedChanged += new System.EventHandler(this.embedded_RadioButton_CheckedChanged);
			// 
			// embedded_RadioButton
			// 
			this.embedded_RadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.embedded_RadioButton.AutoSize = true;
			this.embedded_RadioButton.Checked = true;
			this.embedded_RadioButton.Location = new System.Drawing.Point(5, 10);
			this.embedded_RadioButton.Name = "embedded_RadioButton";
			this.embedded_RadioButton.Size = new System.Drawing.Size(59, 17);
			this.embedded_RadioButton.TabIndex = 2;
			this.embedded_RadioButton.TabStop = true;
			this.embedded_RadioButton.Tag = "Connectivity";
			this.embedded_RadioButton.Text = "Primary";
			this.embedded_RadioButton.UseVisualStyleBackColor = true;
			this.embedded_RadioButton.CheckedChanged += new System.EventHandler(this.embedded_RadioButton_CheckedChanged);
			// 
			// PrinterDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.printerDetails_GroupBox);
			this.Name = "PrinterDetails";
			this.Size = new System.Drawing.Size(412, 243);
			this.printerDetails_GroupBox.ResumeLayout(false);
			this.printerDetails_GroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.wirelessInterfacePortNo_NumericUpDown)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.secondaryInterfacePortNo_NumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.primaryInterfacePortNo_NumericUpDown)).EndInit();
			this.executeOn_GroupBox.ResumeLayout(false);
			this.executeOn_GroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private Framework.UI.FieldValidator printerDetails_FieldValidator;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private System.Windows.Forms.Label productType_Label;
        private System.Windows.Forms.NumericUpDown wirelessInterfacePortNo_NumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton multipleInterface_RadioButton;
        private System.Windows.Forms.RadioButton singleInterface_RadioButton;
        private System.Windows.Forms.TextBox wirelessMacAddress_TextBox;
        private System.Windows.Forms.NumericUpDown secondaryInterfacePortNo_NumericUpDown;
        private System.Windows.Forms.NumericUpDown primaryInterfacePortNo_NumericUpDown;
        private System.Windows.Forms.Label wirelessAddress_Label;
        private IPAddressControl wirelessAddress_IpAddressControl;
        private System.Windows.Forms.Label secondaryInterfaceAddress_Label;
        private IPAddressControl secondaryInterfaceAddress_IpAddressControl;
        private System.Windows.Forms.Label wirelessMacAddress_Label;
        private System.Windows.Forms.Label executeOn_Label;
        private System.Windows.Forms.Label primaryAddress_Label;
        private IPAddressControl primaryAddress_IPAddressControl;
        private System.Windows.Forms.GroupBox executeOn_GroupBox;
        private System.Windows.Forms.RadioButton secondary_RadioButton;
        private System.Windows.Forms.RadioButton wireless_RadioButton;
        private System.Windows.Forms.RadioButton embedded_RadioButton;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
