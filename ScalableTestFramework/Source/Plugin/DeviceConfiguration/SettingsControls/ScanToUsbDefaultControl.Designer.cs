namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class ScanToUsbDefaultControl
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
            this.folder_Label = new System.Windows.Forms.Label();
            this.enable_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.usbSettings_Label = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.scanSettingsUserControl = new HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls.ScanSettingsUserControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fileSettingsControl = new HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls.FileSettingsControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // folder_Label
            // 
            this.folder_Label.AutoSize = true;
            this.folder_Label.Location = new System.Drawing.Point(87, 39);
            this.folder_Label.Name = "folder_Label";
            this.folder_Label.Size = new System.Drawing.Size(112, 13);
            this.folder_Label.TabIndex = 46;
            this.folder_Label.Text = "Enable Scan To USB:";
            // 
            // enable_ComboBox
            // 
            this.enable_ComboBox.AutoSize = true;
            this.enable_ComboBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.enable_ComboBox.Location = new System.Drawing.Point(212, 32);
            this.enable_ComboBox.Name = "enable_ComboBox";
            this.enable_ComboBox.Size = new System.Drawing.Size(338, 27);
            this.enable_ComboBox.TabIndex = 45;
            // 
            // usbSettings_Label
            // 
            this.usbSettings_Label.AutoSize = true;
            this.usbSettings_Label.Location = new System.Drawing.Point(284, 3);
            this.usbSettings_Label.Name = "usbSettings_Label";
            this.usbSettings_Label.Size = new System.Drawing.Size(114, 13);
            this.usbSettings_Label.TabIndex = 44;
            this.usbSettings_Label.Text = "Scan To USB Settings";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scanSettingsUserControl);
            this.groupBox1.Location = new System.Drawing.Point(3, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 423);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Settings";
            // 
            // scanSettingsUserControl
            // 
            this.scanSettingsUserControl.Location = new System.Drawing.Point(4, 14);
            this.scanSettingsUserControl.Name = "scanSettingsUserControl";
            this.scanSettingsUserControl.Size = new System.Drawing.Size(340, 400);
            this.scanSettingsUserControl.TabIndex = 48;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fileSettingsControl);
            this.groupBox2.Location = new System.Drawing.Point(357, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 423);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Settings";
            // 
            // fileSettingsControl
            // 
            this.fileSettingsControl.Location = new System.Drawing.Point(6, 19);
            this.fileSettingsControl.Name = "fileSettingsControl";
            this.fileSettingsControl.Size = new System.Drawing.Size(338, 385);
            this.fileSettingsControl.TabIndex = 0;
            // 
            // ScanToUsbDefaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.folder_Label);
            this.Controls.Add(this.enable_ComboBox);
            this.Controls.Add(this.usbSettings_Label);
            this.Name = "ScanToUsbDefaultControl";
            this.Size = new System.Drawing.Size(710, 490);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label folder_Label;
        public ChoiceComboControl enable_ComboBox;
        private System.Windows.Forms.Label usbSettings_Label;
        private System.Windows.Forms.GroupBox groupBox1;
        private ScanSettingsUserControl scanSettingsUserControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private FileSettingsControl fileSettingsControl;
    }
}
