namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class FolderDefaultControl
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
            this.FolderSettings_Label = new System.Windows.Forms.Label();
            this.folder_Label = new System.Windows.Forms.Label();
            this.accesstype_label = new System.Windows.Forms.Label();
            this.cropping_label = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.scanSettingsUserControl = new HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls.ScanSettingsUserControl();
            this.cropping_choiceComboControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fileSettingsControl1 = new HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls.FileSettingsControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.folder_choiceComboControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.enable_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // FolderSettings_Label
            // 
            this.FolderSettings_Label.AutoSize = true;
            this.FolderSettings_Label.Location = new System.Drawing.Point(272, 0);
            this.FolderSettings_Label.Name = "FolderSettings_Label";
            this.FolderSettings_Label.Size = new System.Drawing.Size(121, 13);
            this.FolderSettings_Label.TabIndex = 26;
            this.FolderSettings_Label.Text = "Scan To Folder Settings";
            // 
            // folder_Label
            // 
            this.folder_Label.AutoSize = true;
            this.folder_Label.Location = new System.Drawing.Point(75, 27);
            this.folder_Label.Name = "folder_Label";
            this.folder_Label.Size = new System.Drawing.Size(119, 13);
            this.folder_Label.TabIndex = 43;
            this.folder_Label.Text = "Enable Scan To Folder:";
            // 
            // accesstype_label
            // 
            this.accesstype_label.AutoSize = true;
            this.accesstype_label.Location = new System.Drawing.Point(98, 60);
            this.accesstype_label.Name = "accesstype_label";
            this.accesstype_label.Size = new System.Drawing.Size(96, 13);
            this.accesstype_label.TabIndex = 45;
            this.accesstype_label.Text = "Folder Acces Type";
            // 
            // cropping_label
            // 
            this.cropping_label.AutoSize = true;
            this.cropping_label.Location = new System.Drawing.Point(6, 416);
            this.cropping_label.Name = "cropping_label";
            this.cropping_label.Size = new System.Drawing.Size(83, 13);
            this.cropping_label.TabIndex = 46;
            this.cropping_label.Text = "Cropping Option";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scanSettingsUserControl);
            this.groupBox1.Controls.Add(this.cropping_choiceComboControl);
            this.groupBox1.Controls.Add(this.cropping_label);
            this.groupBox1.Location = new System.Drawing.Point(0, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 471);
            this.groupBox1.TabIndex = 50;
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
            // cropping_choiceComboControl
            // 
            this.cropping_choiceComboControl.AutoSize = true;
            this.cropping_choiceComboControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cropping_choiceComboControl.Location = new System.Drawing.Point(6, 432);
            this.cropping_choiceComboControl.Name = "cropping_choiceComboControl";
            this.cropping_choiceComboControl.Size = new System.Drawing.Size(338, 27);
            this.cropping_choiceComboControl.TabIndex = 47;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fileSettingsControl1);
            this.groupBox2.Location = new System.Drawing.Point(356, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 471);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Settings";
            // 
            // fileSettingsControl1
            // 
            this.fileSettingsControl1.Location = new System.Drawing.Point(6, 19);
            this.fileSettingsControl1.Name = "fileSettingsControl1";
            this.fileSettingsControl1.Size = new System.Drawing.Size(338, 446);
            this.fileSettingsControl1.TabIndex = 0;
            // 
            // folder_choiceComboControl
            // 
            this.folder_choiceComboControl.AutoSize = true;
            this.folder_choiceComboControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.folder_choiceComboControl.Location = new System.Drawing.Point(200, 53);
            this.folder_choiceComboControl.Name = "folder_choiceComboControl";
            this.folder_choiceComboControl.Size = new System.Drawing.Size(338, 27);
            this.folder_choiceComboControl.TabIndex = 44;
            // 
            // enable_ComboBox
            // 
            this.enable_ComboBox.AutoSize = true;
            this.enable_ComboBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.enable_ComboBox.Location = new System.Drawing.Point(200, 20);
            this.enable_ComboBox.Name = "enable_ComboBox";
            this.enable_ComboBox.Size = new System.Drawing.Size(338, 27);
            this.enable_ComboBox.TabIndex = 42;
            // 
            // FolderDefaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.accesstype_label);
            this.Controls.Add(this.folder_choiceComboControl);
            this.Controls.Add(this.folder_Label);
            this.Controls.Add(this.enable_ComboBox);
            this.Controls.Add(this.FolderSettings_Label);
            this.Name = "FolderDefaultControl";
            this.Size = new System.Drawing.Size(710, 565);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FolderSettings_Label;
        private System.Windows.Forms.Label folder_Label;
        public ChoiceComboControl enable_ComboBox;
        private System.Windows.Forms.Label accesstype_label;
        public ChoiceComboControl folder_choiceComboControl;
        private ChoiceComboControl cropping_choiceComboControl;
        private System.Windows.Forms.Label cropping_label;
        private System.Windows.Forms.GroupBox groupBox1;
        private ScanSettingsUserControl scanSettingsUserControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private FileSettingsControl fileSettingsControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
