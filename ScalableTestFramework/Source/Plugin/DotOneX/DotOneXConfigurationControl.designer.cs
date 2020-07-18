using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.DotOneX
{
    /// <summary>
    /// Edit control for 802.1X plug-in
    /// </summary>
    partial class DotOneXConfigurationControl
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
            this.radiusServerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.subSha2_RadioButton = new System.Windows.Forms.RadioButton();
            this.rootSha2_RadioButton = new System.Windows.Forms.RadioButton();
            this.rootSha1_RadioButton = new System.Windows.Forms.RadioButton();
            this.subSha2_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.subSha2Ip_label = new System.Windows.Forms.Label();
            this.rootSha2_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.rootSha2Ip_Label = new System.Windows.Forms.Label();
            this.rootSha1_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.rootSha1Ip_Label = new System.Windows.Forms.Label();
            this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.europaPortNumber_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.secondaryWired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.europaWiredIP_Label = new System.Windows.Forms.Label();
            this.primaryWired_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.printerIp_Label = new System.Windows.Forms.Label();
            this.sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.switchDetailsControl = new HP.ScalableTest.PluginSupport.Connectivity.UI.SwitchDetailsControl();
            this.validation_GroupBox = new System.Windows.Forms.GroupBox();
            this.validatePrint_CheckBox = new System.Windows.Forms.CheckBox();
            this.printDriverSelector1 = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.packetCaptureMachineDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.packetCaptureMachine_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.packetCaptureMachine_Label = new System.Windows.Forms.Label();
            this.radiusServerDetails_GroupBox.SuspendLayout();
            this.printerDetails_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.europaPortNumber_NumericUpDown)).BeginInit();
            this.validation_GroupBox.SuspendLayout();
            this.packetCaptureMachineDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // testCaseDetails_GroupBox
            // 
            this.testCaseDetails_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testCaseDetails_GroupBox.Size = new System.Drawing.Size(718, 338);
            // 
            // radiusServerDetails_GroupBox
            // 
            this.radiusServerDetails_GroupBox.Controls.Add(this.subSha2_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_RadioButton);
            this.radiusServerDetails_GroupBox.Controls.Add(this.subSha2_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.subSha2Ip_label);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha2Ip_Label);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1_IpAddressControl);
            this.radiusServerDetails_GroupBox.Controls.Add(this.rootSha1Ip_Label);
            this.radiusServerDetails_GroupBox.Location = new System.Drawing.Point(10, 514);
            this.radiusServerDetails_GroupBox.Name = "radiusServerDetails_GroupBox";
            this.radiusServerDetails_GroupBox.Size = new System.Drawing.Size(346, 154);
            this.radiusServerDetails_GroupBox.TabIndex = 50;
            this.radiusServerDetails_GroupBox.TabStop = false;
            this.radiusServerDetails_GroupBox.Text = "Radius Servers";
            // 
            // subSha2_RadioButton
            // 
            this.subSha2_RadioButton.AutoSize = true;
            this.subSha2_RadioButton.Enabled = false;
            this.subSha2_RadioButton.Location = new System.Drawing.Point(242, 28);
            this.subSha2_RadioButton.Name = "subSha2_RadioButton";
            this.subSha2_RadioButton.Size = new System.Drawing.Size(75, 17);
            this.subSha2_RadioButton.TabIndex = 2;
            this.subSha2_RadioButton.TabStop = true;
            this.subSha2_RadioButton.Text = "Sub SHA2";
            this.subSha2_RadioButton.UseVisualStyleBackColor = true;
            this.subSha2_RadioButton.CheckedChanged += new System.EventHandler(this.RadiusServer_CheckedChanged);
            // 
            // rootSha2_RadioButton
            // 
            this.rootSha2_RadioButton.AutoSize = true;
            this.rootSha2_RadioButton.Location = new System.Drawing.Point(127, 28);
            this.rootSha2_RadioButton.Name = "rootSha2_RadioButton";
            this.rootSha2_RadioButton.Size = new System.Drawing.Size(79, 17);
            this.rootSha2_RadioButton.TabIndex = 1;
            this.rootSha2_RadioButton.TabStop = true;
            this.rootSha2_RadioButton.Text = "Root SHA2";
            this.rootSha2_RadioButton.UseVisualStyleBackColor = true;
            this.rootSha2_RadioButton.CheckedChanged += new System.EventHandler(this.RadiusServer_CheckedChanged);
            // 
            // rootSha1_RadioButton
            // 
            this.rootSha1_RadioButton.AutoSize = true;
            this.rootSha1_RadioButton.Checked = true;
            this.rootSha1_RadioButton.Location = new System.Drawing.Point(21, 28);
            this.rootSha1_RadioButton.Name = "rootSha1_RadioButton";
            this.rootSha1_RadioButton.Size = new System.Drawing.Size(79, 17);
            this.rootSha1_RadioButton.TabIndex = 0;
            this.rootSha1_RadioButton.TabStop = true;
            this.rootSha1_RadioButton.Text = "Root SHA1";
            this.rootSha1_RadioButton.UseVisualStyleBackColor = true;
            this.rootSha1_RadioButton.CheckedChanged += new System.EventHandler(this.RadiusServer_CheckedChanged);
            // 
            // subSha2_IpAddressControl
            // 
            this.subSha2_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.subSha2_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.subSha2_IpAddressControl.Enabled = false;
            this.subSha2_IpAddressControl.Location = new System.Drawing.Point(138, 113);
            this.subSha2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.subSha2_IpAddressControl.Name = "subSha2_IpAddressControl";
            this.subSha2_IpAddressControl.Size = new System.Drawing.Size(190, 20);
            this.subSha2_IpAddressControl.TabIndex = 5;
            this.subSha2_IpAddressControl.Tag = "Sub SHA2 Server IP";
            this.subSha2_IpAddressControl.Text = "...";
            // 
            // subSha2Ip_label
            // 
            this.subSha2Ip_label.AutoSize = true;
            this.subSha2Ip_label.Location = new System.Drawing.Point(20, 116);
            this.subSha2Ip_label.Name = "subSha2Ip_label";
            this.subSha2Ip_label.Size = new System.Drawing.Size(107, 13);
            this.subSha2Ip_label.TabIndex = 61;
            this.subSha2Ip_label.Text = "Sub SHA2 Server IP:";
            // 
            // rootSha2_IpAddressControl
            // 
            this.rootSha2_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha2_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha2_IpAddressControl.Enabled = false;
            this.rootSha2_IpAddressControl.Location = new System.Drawing.Point(138, 87);
            this.rootSha2_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha2_IpAddressControl.Name = "rootSha2_IpAddressControl";
            this.rootSha2_IpAddressControl.Size = new System.Drawing.Size(190, 20);
            this.rootSha2_IpAddressControl.TabIndex = 4;
            this.rootSha2_IpAddressControl.Tag = "Root SHA2 Server IP";
            this.rootSha2_IpAddressControl.Text = "...";
            // 
            // rootSha2Ip_Label
            // 
            this.rootSha2Ip_Label.AutoSize = true;
            this.rootSha2Ip_Label.Location = new System.Drawing.Point(20, 90);
            this.rootSha2Ip_Label.Name = "rootSha2Ip_Label";
            this.rootSha2Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha2Ip_Label.TabIndex = 59;
            this.rootSha2Ip_Label.Text = "Root SHA2 Server IP:";
            // 
            // rootSha1_IpAddressControl
            // 
            this.rootSha1_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.rootSha1_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rootSha1_IpAddressControl.Location = new System.Drawing.Point(138, 60);
            this.rootSha1_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.rootSha1_IpAddressControl.Name = "rootSha1_IpAddressControl";
            this.rootSha1_IpAddressControl.Size = new System.Drawing.Size(190, 20);
            this.rootSha1_IpAddressControl.TabIndex = 3;
            this.rootSha1_IpAddressControl.Tag = "Root SHA1 Server IP";
            this.rootSha1_IpAddressControl.Text = "...";
            // 
            // rootSha1Ip_Label
            // 
            this.rootSha1Ip_Label.AutoSize = true;
            this.rootSha1Ip_Label.Location = new System.Drawing.Point(20, 63);
            this.rootSha1Ip_Label.Name = "rootSha1Ip_Label";
            this.rootSha1Ip_Label.Size = new System.Drawing.Size(111, 13);
            this.rootSha1Ip_Label.TabIndex = 57;
            this.rootSha1Ip_Label.Text = "Root SHA1 Server IP:";
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.europaPortNumber_NumericUpDown);
            this.printerDetails_GroupBox.Controls.Add(this.secondaryWired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.europaWiredIP_Label);
            this.printerDetails_GroupBox.Controls.Add(this.primaryWired_IpAddressControl);
            this.printerDetails_GroupBox.Controls.Add(this.printerIp_Label);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(10, 410);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(346, 98);
            this.printerDetails_GroupBox.TabIndex = 52;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // europaPortNumber_NumericUpDown
            // 
            this.europaPortNumber_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.europaPortNumber_NumericUpDown.AutoSize = true;
            this.europaPortNumber_NumericUpDown.Enabled = false;
            this.europaPortNumber_NumericUpDown.Location = new System.Drawing.Point(283, 64);
            this.europaPortNumber_NumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.europaPortNumber_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.europaPortNumber_NumericUpDown.MaximumSize = new System.Drawing.Size(35, 0);
            this.europaPortNumber_NumericUpDown.MinimumSize = new System.Drawing.Size(45, 0);
            this.europaPortNumber_NumericUpDown.Name = "europaPortNumber_NumericUpDown";
            this.europaPortNumber_NumericUpDown.ReadOnly = true;
            this.europaPortNumber_NumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.europaPortNumber_NumericUpDown.TabIndex = 62;
            // 
            // secondaryWired_IpAddressControl
            // 
            this.secondaryWired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.secondaryWired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.secondaryWired_IpAddressControl.Enabled = false;
            this.secondaryWired_IpAddressControl.Location = new System.Drawing.Point(120, 64);
            this.secondaryWired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.secondaryWired_IpAddressControl.Name = "secondaryWired_IpAddressControl";
            this.secondaryWired_IpAddressControl.Size = new System.Drawing.Size(153, 20);
            this.secondaryWired_IpAddressControl.TabIndex = 57;
            this.secondaryWired_IpAddressControl.Tag = "Secondary Wired Interface IP Address";
            this.secondaryWired_IpAddressControl.Text = "...";
            // 
            // europaWiredIP_Label
            // 
            this.europaWiredIP_Label.AutoSize = true;
            this.europaWiredIP_Label.Location = new System.Drawing.Point(12, 68);
            this.europaWiredIP_Label.Name = "europaWiredIP_Label";
            this.europaWiredIP_Label.Size = new System.Drawing.Size(105, 13);
            this.europaWiredIP_Label.TabIndex = 59;
            this.europaWiredIP_Label.Text = "Secondary Wired IP:";
            // 
            // primaryWired_IpAddressControl
            // 
            this.primaryWired_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.primaryWired_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.primaryWired_IpAddressControl.Location = new System.Drawing.Point(120, 35);
            this.primaryWired_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.primaryWired_IpAddressControl.Name = "primaryWired_IpAddressControl";
            this.primaryWired_IpAddressControl.Size = new System.Drawing.Size(208, 20);
            this.primaryWired_IpAddressControl.TabIndex = 56;
            this.primaryWired_IpAddressControl.Tag = "Primary Wired Interface IP Address";
            this.primaryWired_IpAddressControl.Text = "...";
            this.primaryWired_IpAddressControl.TextChanged += new System.EventHandler(this.printerIp_IpAddressControl_TextChanged);
            // 
            // printerIp_Label
            // 
            this.printerIp_Label.AutoSize = true;
            this.printerIp_Label.Location = new System.Drawing.Point(12, 38);
            this.printerIp_Label.Name = "printerIp_Label";
            this.printerIp_Label.Size = new System.Drawing.Size(88, 13);
            this.printerIp_Label.TabIndex = 55;
            this.printerIp_Label.Text = "Primary Wired IP:";
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(362, 410);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = null;
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(356, 98);
            this.sitemapVersionSelector.TabIndex = 54;
            // 
            // switchDetailsControl
            // 
            this.switchDetailsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.switchDetailsControl.HidePortNumber = false;
            this.switchDetailsControl.Location = new System.Drawing.Point(362, 514);
            this.switchDetailsControl.MinimumSize = new System.Drawing.Size(240, 90);
            this.switchDetailsControl.Name = "switchDetailsControl";
            this.switchDetailsControl.Size = new System.Drawing.Size(353, 90);
            this.switchDetailsControl.SwitchIPAddress = "...";
            this.switchDetailsControl.SwitchPortNumber = 1;
            this.switchDetailsControl.TabIndex = 55;
            this.switchDetailsControl.ValidationRequired = true;
            // 
            // validation_GroupBox
            // 
            this.validation_GroupBox.Controls.Add(this.validatePrint_CheckBox);
            this.validation_GroupBox.Location = new System.Drawing.Point(362, 724);
            this.validation_GroupBox.Name = "validation_GroupBox";
            this.validation_GroupBox.Size = new System.Drawing.Size(355, 49);
            this.validation_GroupBox.TabIndex = 56;
            this.validation_GroupBox.TabStop = false;
            this.validation_GroupBox.Text = "Validation";
            // 
            // validatePrint_CheckBox
            // 
            this.validatePrint_CheckBox.AutoSize = true;
            this.validatePrint_CheckBox.Checked = true;
            this.validatePrint_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.validatePrint_CheckBox.Location = new System.Drawing.Point(65, 21);
            this.validatePrint_CheckBox.Name = "validatePrint_CheckBox";
            this.validatePrint_CheckBox.Size = new System.Drawing.Size(87, 17);
            this.validatePrint_CheckBox.TabIndex = 0;
            this.validatePrint_CheckBox.Text = "Validate print";
            this.validatePrint_CheckBox.UseVisualStyleBackColor = true;
            this.validatePrint_CheckBox.CheckedChanged += new System.EventHandler(this.validatePrint_CheckBox_CheckedChanged);
            // 
            // printDriverSelector1
            // 
            this.printDriverSelector1.DriverModel = "";
            this.printDriverSelector1.DriverPackagePath = "";
            this.printDriverSelector1.Location = new System.Drawing.Point(362, 612);
            this.printDriverSelector1.MinimumSize = new System.Drawing.Size(314, 99);
            this.printDriverSelector1.Name = "printDriverSelector1";
            this.printDriverSelector1.PrinterFamily = null;
            this.printDriverSelector1.PrinterName = null;
            this.printDriverSelector1.Size = new System.Drawing.Size(353, 105);
            this.printDriverSelector1.TabIndex = 57;
            // 
            // packetCaptureMachineDetails_GroupBox
            // 
            this.packetCaptureMachineDetails_GroupBox.Controls.Add(this.packetCaptureMachine_IpAddressControl);
            this.packetCaptureMachineDetails_GroupBox.Controls.Add(this.packetCaptureMachine_Label);
            this.packetCaptureMachineDetails_GroupBox.Location = new System.Drawing.Point(10, 674);
            this.packetCaptureMachineDetails_GroupBox.Name = "packetCaptureMachineDetails_GroupBox";
            this.packetCaptureMachineDetails_GroupBox.Size = new System.Drawing.Size(346, 99);
            this.packetCaptureMachineDetails_GroupBox.TabIndex = 58;
            this.packetCaptureMachineDetails_GroupBox.TabStop = false;
            this.packetCaptureMachineDetails_GroupBox.Text = "Packet Capture Details";
            // 
            // packetCaptureMachine_IpAddressControl
            // 
            this.packetCaptureMachine_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.packetCaptureMachine_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.packetCaptureMachine_IpAddressControl.Location = new System.Drawing.Point(157, 36);
            this.packetCaptureMachine_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.packetCaptureMachine_IpAddressControl.Name = "packetCaptureMachine_IpAddressControl";
            this.packetCaptureMachine_IpAddressControl.Size = new System.Drawing.Size(174, 20);
            this.packetCaptureMachine_IpAddressControl.TabIndex = 58;
            this.packetCaptureMachine_IpAddressControl.Tag = "";
            this.packetCaptureMachine_IpAddressControl.Text = "...";
            // 
            // packetCaptureMachine_Label
            // 
            this.packetCaptureMachine_Label.AutoSize = true;
            this.packetCaptureMachine_Label.Location = new System.Drawing.Point(15, 39);
            this.packetCaptureMachine_Label.Name = "packetCaptureMachine_Label";
            this.packetCaptureMachine_Label.Size = new System.Drawing.Size(144, 13);
            this.packetCaptureMachine_Label.TabIndex = 57;
            this.packetCaptureMachine_Label.Text = "Packet Capture Machine IP: ";
            // 
            // DotOneXConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.packetCaptureMachineDetails_GroupBox);
            this.Controls.Add(this.printDriverSelector1);
            this.Controls.Add(this.validation_GroupBox);
            this.Controls.Add(this.switchDetailsControl);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Controls.Add(this.radiusServerDetails_GroupBox);
            this.Name = "DotOneXConfigurationControl";
            this.Size = new System.Drawing.Size(734, 813);
            this.Controls.SetChildIndex(this.radiusServerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.switchDetailsControl, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.validation_GroupBox, 0);
            this.Controls.SetChildIndex(this.printDriverSelector1, 0);
            this.Controls.SetChildIndex(this.packetCaptureMachineDetails_GroupBox, 0);
            this.radiusServerDetails_GroupBox.ResumeLayout(false);
            this.radiusServerDetails_GroupBox.PerformLayout();
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.europaPortNumber_NumericUpDown)).EndInit();
            this.validation_GroupBox.ResumeLayout(false);
            this.validation_GroupBox.PerformLayout();
            this.packetCaptureMachineDetails_GroupBox.ResumeLayout(false);
            this.packetCaptureMachineDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox radiusServerDetails_GroupBox;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
        private SitemapVersionSelector sitemapVersionSelector;
        private Framework.UI.IPAddressControl subSha2_IpAddressControl;
        private System.Windows.Forms.Label subSha2Ip_label;
        private Framework.UI.IPAddressControl rootSha2_IpAddressControl;
        private System.Windows.Forms.Label rootSha2Ip_Label;
        private Framework.UI.IPAddressControl rootSha1_IpAddressControl;
        private System.Windows.Forms.Label rootSha1Ip_Label;
        private Framework.UI.IPAddressControl primaryWired_IpAddressControl;
        private System.Windows.Forms.Label printerIp_Label;
        private SwitchDetailsControl switchDetailsControl;
        private System.Windows.Forms.RadioButton subSha2_RadioButton;
        private System.Windows.Forms.RadioButton rootSha2_RadioButton;
        private System.Windows.Forms.RadioButton rootSha1_RadioButton;
        private System.Windows.Forms.GroupBox validation_GroupBox;
        private System.Windows.Forms.CheckBox validatePrint_CheckBox;
        private Framework.UI.IPAddressControl secondaryWired_IpAddressControl;
        private System.Windows.Forms.Label europaWiredIP_Label;
        private System.Windows.Forms.NumericUpDown europaPortNumber_NumericUpDown;
        private PrintDriverSelector printDriverSelector1;
        private System.Windows.Forms.GroupBox packetCaptureMachineDetails_GroupBox;
        private Framework.UI.IPAddressControl packetCaptureMachine_IpAddressControl;
        private System.Windows.Forms.Label packetCaptureMachine_Label;
    }
}
