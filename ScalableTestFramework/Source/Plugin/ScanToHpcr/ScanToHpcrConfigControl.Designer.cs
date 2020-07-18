namespace HP.ScalableTest.Plugin.ScanToHpcr
{
    partial class ScanToHpcrConfigControl
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
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDevice = new System.Windows.Forms.RadioButton();
            this.radioButtonHpcr = new System.Windows.Forms.RadioButton();
            this.checkBoxImagePreview = new System.Windows.Forms.CheckBox();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.distribution_textbox = new System.Windows.Forms.TextBox();
            this.lblDestinationName = new System.Windows.Forms.Label();
            this.destination_textBox = new System.Windows.Forms.TextBox();
            this.labelDistribution = new System.Windows.Forms.Label();
            this.labelSelectHpcrApp = new System.Windows.Forms.Label();
            this.HpcrApps_comboBox = new System.Windows.Forms.ComboBox();
            this.hpcrScan_label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.scanConfiguration_TabPage.Controls.Add(this.label_AuthMethod);
            this.scanConfiguration_TabPage.Controls.Add(this.comboBox_AuthProvider);
            this.scanConfiguration_TabPage.Controls.Add(this.groupBoxMemoryPofile);
            this.scanConfiguration_TabPage.Controls.Add(this.groupBox1);
            this.scanConfiguration_TabPage.Controls.Add(this.checkBoxImagePreview);
            this.scanConfiguration_TabPage.Controls.Add(this.assetSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel1);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(667, 687);
            this.scanConfiguration_TabPage.TabIndex = 0;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(329, 269);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 57;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(472, 266);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 56;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(3, 577);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(658, 101);
            this.groupBoxMemoryPofile.TabIndex = 55;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDevice);
            this.groupBox1.Controls.Add(this.radioButtonHpcr);
            this.groupBox1.Location = new System.Drawing.Point(158, 243);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 61);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authentication";
            // 
            // radioButtonDevice
            // 
            this.radioButtonDevice.AutoSize = true;
            this.radioButtonDevice.Checked = true;
            this.radioButtonDevice.Location = new System.Drawing.Point(22, 15);
            this.radioButtonDevice.Name = "radioButtonDevice";
            this.radioButtonDevice.Size = new System.Drawing.Size(61, 19);
            this.radioButtonDevice.TabIndex = 1;
            this.radioButtonDevice.TabStop = true;
            this.radioButtonDevice.Text = "Sign In";
            this.radioButtonDevice.UseVisualStyleBackColor = true;
            // 
            // radioButtonHpcr
            // 
            this.radioButtonHpcr.AutoSize = true;
            this.radioButtonHpcr.Location = new System.Drawing.Point(22, 36);
            this.radioButtonHpcr.Name = "radioButtonHpcr";
            this.radioButtonHpcr.Size = new System.Drawing.Size(113, 19);
            this.radioButtonHpcr.TabIndex = 0;
            this.radioButtonHpcr.Text = "HP CR Workflow";
            this.radioButtonHpcr.UseVisualStyleBackColor = true;
            // 
            // checkBoxImagePreview
            // 
            this.checkBoxImagePreview.AutoSize = true;
            this.checkBoxImagePreview.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxImagePreview.Location = new System.Drawing.Point(43, 265);
            this.checkBoxImagePreview.Name = "checkBoxImagePreview";
            this.checkBoxImagePreview.Size = new System.Drawing.Size(109, 19);
            this.checkBoxImagePreview.TabIndex = 52;
            this.checkBoxImagePreview.Text = "Image Preview: ";
            this.checkBoxImagePreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxImagePreview.UseVisualStyleBackColor = true;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 317);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(661, 260);
            this.assetSelectionControl.TabIndex = 50;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.distribution_textbox);
            this.panel1.Controls.Add(this.lblDestinationName);
            this.panel1.Controls.Add(this.destination_textBox);
            this.panel1.Controls.Add(this.labelDistribution);
            this.panel1.Controls.Add(this.labelSelectHpcrApp);
            this.panel1.Controls.Add(this.HpcrApps_comboBox);
            this.panel1.Controls.Add(this.hpcrScan_label);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(661, 234);
            this.panel1.TabIndex = 49;
            // 
            // distribution_textbox
            // 
            this.distribution_textbox.Enabled = false;
            this.distribution_textbox.Location = new System.Drawing.Point(445, 4);
            this.distribution_textbox.Name = "distribution_textbox";
            this.distribution_textbox.Size = new System.Drawing.Size(156, 23);
            this.distribution_textbox.TabIndex = 15;
            // 
            // lblDestinationName
            // 
            this.lblDestinationName.AutoSize = true;
            this.lblDestinationName.Location = new System.Drawing.Point(317, 37);
            this.lblDestinationName.Name = "lblDestinationName";
            this.lblDestinationName.Size = new System.Drawing.Size(100, 15);
            this.lblDestinationName.TabIndex = 14;
            this.lblDestinationName.Text = "Destination name";
            // 
            // destination_textBox
            // 
            this.destination_textBox.Enabled = false;
            this.destination_textBox.Location = new System.Drawing.Point(445, 34);
            this.destination_textBox.Name = "destination_textBox";
            this.destination_textBox.Size = new System.Drawing.Size(156, 23);
            this.destination_textBox.TabIndex = 13;
            // 
            // labelDistribution
            // 
            this.labelDistribution.AutoSize = true;
            this.labelDistribution.Location = new System.Drawing.Point(317, 7);
            this.labelDistribution.Name = "labelDistribution";
            this.labelDistribution.Size = new System.Drawing.Size(102, 15);
            this.labelDistribution.TabIndex = 12;
            this.labelDistribution.Text = "Distribution name";
            // 
            // labelSelectHpcrApp
            // 
            this.labelSelectHpcrApp.AutoSize = true;
            this.labelSelectHpcrApp.Location = new System.Drawing.Point(15, 15);
            this.labelSelectHpcrApp.Name = "labelSelectHpcrApp";
            this.labelSelectHpcrApp.Size = new System.Drawing.Size(97, 15);
            this.labelSelectHpcrApp.TabIndex = 11;
            this.labelSelectHpcrApp.Text = "Select HPCR App";
            // 
            // HpcrApps_comboBox
            // 
            this.HpcrApps_comboBox.BackColor = System.Drawing.SystemColors.Window;
            this.HpcrApps_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HpcrApps_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.HpcrApps_comboBox.FormattingEnabled = true;
            this.HpcrApps_comboBox.Location = new System.Drawing.Point(118, 10);
            this.HpcrApps_comboBox.Name = "HpcrApps_comboBox";
            this.HpcrApps_comboBox.Size = new System.Drawing.Size(158, 23);
            this.HpcrApps_comboBox.TabIndex = 10;
            this.HpcrApps_comboBox.SelectedIndexChanged += new System.EventHandler(this.HpcrApps_comboBox_SelectedIndexChanged);
            // 
            // hpcrScan_label
            // 
            this.hpcrScan_label.AutoSize = true;
            this.hpcrScan_label.Location = new System.Drawing.Point(133, 48);
            this.hpcrScan_label.Name = "hpcrScan_label";
            this.hpcrScan_label.Size = new System.Drawing.Size(139, 15);
            this.hpcrScan_label.TabIndex = 9;
            this.hpcrScan_label.Text = "Uses HPCR \"Scan To Me\"";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.pageCount_NumericUpDown);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(7, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 147);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Page Count";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(118, 67);
            this.pageCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pageCount_NumericUpDown.Name = "pageCount_NumericUpDown";
            this.pageCount_NumericUpDown.Size = new System.Drawing.Size(107, 23);
            this.pageCount_NumericUpDown.TabIndex = 12;
            this.pageCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(290, 53);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.lockTimeoutControl);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(317, 74);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(341, 156);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Skip Delay";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(7, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(327, 67);
            this.label2.TabIndex = 10;
            this.label2.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(675, 715);
            this.tabControl.TabIndex = 50;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 88);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(22, 16);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(397, 84);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // ScanToHpcrConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToHpcrConfigControl";
            this.Size = new System.Drawing.Size(675, 718);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.scanConfiguration_TabPage.PerformLayout();
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox distribution_textbox;
        private System.Windows.Forms.Label lblDestinationName;
        private System.Windows.Forms.TextBox destination_textBox;
        private System.Windows.Forms.Label labelDistribution;
        private System.Windows.Forms.Label labelSelectHpcrApp;
        private System.Windows.Forms.ComboBox HpcrApps_comboBox;
        private System.Windows.Forms.Label hpcrScan_label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.CheckBox checkBoxImagePreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private System.Windows.Forms.RadioButton radioButtonDevice;
        private System.Windows.Forms.RadioButton radioButtonHpcr;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
