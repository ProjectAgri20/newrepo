namespace HP.ScalableTest.Plugin.AutoStore
{
    partial class AutoStoreConfigControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_configuration = new System.Windows.Forms.TabPage();
            this.checkBox_ImagePreview = new System.Windows.Forms.CheckBox();
            this.checkBox_OCR = new System.Windows.Forms.CheckBox();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AutoStoreRadioButton = new System.Windows.Forms.RadioButton();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelSelectAutoStoreApp = new System.Windows.Forms.Label();
            this.AutoStoreApps_comboBox = new System.Windows.Forms.ComboBox();
            this.AutoStoreScan_label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.tabControl.SuspendLayout();
            this.tabPage_configuration.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_configuration);
            this.tabControl.Location = new System.Drawing.Point(4, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(710, 556);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage_configuration
            // 
            this.tabPage_configuration.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_configuration.Controls.Add(this.checkBox_ImagePreview);
            this.tabPage_configuration.Controls.Add(this.checkBox_OCR);
            this.tabPage_configuration.Controls.Add(this.groupBoxMemoryPofile);
            this.tabPage_configuration.Controls.Add(this.groupBox1);
            this.tabPage_configuration.Controls.Add(this.assetSelectionControl);
            this.tabPage_configuration.Controls.Add(this.panel1);
            this.tabPage_configuration.Location = new System.Drawing.Point(4, 24);
            this.tabPage_configuration.Name = "tabPage_configuration";
            this.tabPage_configuration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_configuration.Size = new System.Drawing.Size(702, 528);
            this.tabPage_configuration.TabIndex = 0;
            this.tabPage_configuration.Text = "AutoStore Configuration";
            // 
            // checkBox_ImagePreview
            // 
            this.checkBox_ImagePreview.AutoSize = true;
            this.checkBox_ImagePreview.Location = new System.Drawing.Point(367, 195);
            this.checkBox_ImagePreview.Name = "checkBox_ImagePreview";
            this.checkBox_ImagePreview.Size = new System.Drawing.Size(103, 19);
            this.checkBox_ImagePreview.TabIndex = 59;
            this.checkBox_ImagePreview.Text = "Image Preview";
            this.checkBox_ImagePreview.UseVisualStyleBackColor = true;
            // 
            // checkBox_OCR
            // 
            this.checkBox_OCR.AutoSize = true;
            this.checkBox_OCR.Location = new System.Drawing.Point(207, 195);
            this.checkBox_OCR.Name = "checkBox_OCR";
            this.checkBox_OCR.Size = new System.Drawing.Size(111, 19);
            this.checkBox_OCR.TabIndex = 57;
            this.checkBox_OCR.Text = "Use OCR for Job";
            this.checkBox_OCR.UseVisualStyleBackColor = true;
            this.checkBox_OCR.CheckedChanged += new System.EventHandler(this.checkBox_OCR_CheckedChanged);
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(6, 404);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(350, 116);
            this.groupBoxMemoryPofile.TabIndex = 56;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AutoStoreRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 51);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authentication";
            // 
            // AutoStoreRadioButton
            // 
            this.AutoStoreRadioButton.AutoSize = true;
            this.AutoStoreRadioButton.Checked = true;
            this.AutoStoreRadioButton.Enabled = false;
            this.AutoStoreRadioButton.Location = new System.Drawing.Point(27, 26);
            this.AutoStoreRadioButton.Name = "AutoStoreRadioButton";
            this.AutoStoreRadioButton.Size = new System.Drawing.Size(132, 19);
            this.AutoStoreRadioButton.TabIndex = 0;
            this.AutoStoreRadioButton.TabStop = true;
            this.AutoStoreRadioButton.Text = "AutoStore Workflow";
            this.AutoStoreRadioButton.UseVisualStyleBackColor = true;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 244);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(661, 156);
            this.assetSelectionControl.TabIndex = 51;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.labelSelectAutoStoreApp);
            this.panel1.Controls.Add(this.AutoStoreApps_comboBox);
            this.panel1.Controls.Add(this.AutoStoreScan_label);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(696, 170);
            this.panel1.TabIndex = 50;
            // 
            // labelSelectAutoStoreApp
            // 
            this.labelSelectAutoStoreApp.AutoSize = true;
            this.labelSelectAutoStoreApp.Location = new System.Drawing.Point(15, 15);
            this.labelSelectAutoStoreApp.Name = "labelSelectAutoStoreApp";
            this.labelSelectAutoStoreApp.Size = new System.Drawing.Size(119, 15);
            this.labelSelectAutoStoreApp.TabIndex = 11;
            this.labelSelectAutoStoreApp.Text = "Select AutoStore App";
            // 
            // AutoStoreApps_comboBox
            // 
            this.AutoStoreApps_comboBox.BackColor = System.Drawing.SystemColors.Window;
            this.AutoStoreApps_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AutoStoreApps_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AutoStoreApps_comboBox.FormattingEnabled = true;
            this.AutoStoreApps_comboBox.Location = new System.Drawing.Point(140, 12);
            this.AutoStoreApps_comboBox.Name = "AutoStoreApps_comboBox";
            this.AutoStoreApps_comboBox.Size = new System.Drawing.Size(158, 23);
            this.AutoStoreApps_comboBox.TabIndex = 10;
            this.AutoStoreApps_comboBox.SelectedIndexChanged += new System.EventHandler(this.AutoStoreApps_comboBox_SelectedIndexChanged);
            // 
            // AutoStoreScan_label
            // 
            this.AutoStoreScan_label.AutoSize = true;
            this.AutoStoreScan_label.Location = new System.Drawing.Point(338, 15);
            this.AutoStoreScan_label.Name = "AutoStoreScan_label";
            this.AutoStoreScan_label.Size = new System.Drawing.Size(123, 15);
            this.AutoStoreScan_label.TabIndex = 9;
            this.AutoStoreScan_label.Text = "Uses AutoStore Folder";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.pageCount_NumericUpDown);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(0, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 93);
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
            this.groupBox5.Location = new System.Drawing.Point(310, 61);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(348, 99);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Lock Timeout";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(28, 26);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 60;
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(15, 23);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(329, 87);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // AutoStoreConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AutoStoreConfigControl";
            this.Size = new System.Drawing.Size(710, 556);
            this.tabControl.ResumeLayout(false);
            this.tabPage_configuration.ResumeLayout(false);
            this.tabPage_configuration.PerformLayout();
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabPage tabPage_configuration;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelSelectAutoStoreApp;
        private System.Windows.Forms.ComboBox AutoStoreApps_comboBox;
        private System.Windows.Forms.Label AutoStoreScan_label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabControl tabControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton AutoStoreRadioButton;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private System.Windows.Forms.CheckBox checkBox_OCR;
        private System.Windows.Forms.CheckBox checkBox_ImagePreview;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
    }
}
