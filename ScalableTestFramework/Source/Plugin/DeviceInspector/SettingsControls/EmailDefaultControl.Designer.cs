using HP.ScalableTest.Plugin.DeviceInspector.FieldControls;
namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    partial class EmailDefaultControl
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
            this.email_Label = new System.Windows.Forms.Label();
            this.serverName_Label = new System.Windows.Forms.Label();
            this.from_Label = new System.Windows.Forms.Label();
            this.defaultFrom_Label = new System.Windows.Forms.Label();
            this.to_Label = new System.Windows.Forms.Label();
            this.originalSize_Label = new System.Windows.Forms.Label();
            this.originalSides_Label = new System.Windows.Forms.Label();
            this.imagePreview_Label = new System.Windows.Forms.Label();
            this.fileType_Label = new System.Windows.Forms.Label();
            this.resolution_Label = new System.Windows.Forms.Label();
            this.EmailSettings_Label = new System.Windows.Forms.Label();
            this.port_label = new System.Windows.Forms.Label();
            this.fileSplit_Label = new System.Windows.Forms.Label();
            this.defaultTo_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.resolution_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.fileType_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.imagePreview_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.originalSides_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.originalSize_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.defaultFrom_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.from_TextBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceTextControl();
            this.enable_ComboBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.ChoiceComboControl();
            this.outgoingSMTPServer_ChoiceBox = new HP.ScalableTest.Plugin.DeviceInspector.FieldControls.FourChoiceControl();
            this.ssl_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // email_Label
            // 
            this.email_Label.AutoSize = true;
            this.email_Label.Location = new System.Drawing.Point(25, 47);
            this.email_Label.Name = "email_Label";
            this.email_Label.Size = new System.Drawing.Size(115, 13);
            this.email_Label.TabIndex = 11;
            this.email_Label.Text = "Enable Scan To Email:";
            // 
            // serverName_Label
            // 
            this.serverName_Label.AutoSize = true;
            this.serverName_Label.Location = new System.Drawing.Point(30, 366);
            this.serverName_Label.Name = "serverName_Label";
            this.serverName_Label.Size = new System.Drawing.Size(72, 13);
            this.serverName_Label.TabIndex = 12;
            this.serverName_Label.Text = "Server Name:";
            // 
            // from_Label
            // 
            this.from_Label.AutoSize = true;
            this.from_Label.Location = new System.Drawing.Point(25, 80);
            this.from_Label.Name = "from_Label";
            this.from_Label.Size = new System.Drawing.Size(33, 13);
            this.from_Label.TabIndex = 13;
            this.from_Label.Text = "From:";
            // 
            // defaultFrom_Label
            // 
            this.defaultFrom_Label.AutoSize = true;
            this.defaultFrom_Label.Location = new System.Drawing.Point(25, 113);
            this.defaultFrom_Label.Name = "defaultFrom_Label";
            this.defaultFrom_Label.Size = new System.Drawing.Size(70, 13);
            this.defaultFrom_Label.TabIndex = 14;
            this.defaultFrom_Label.Text = "Default From:";
            // 
            // to_Label
            // 
            this.to_Label.AutoSize = true;
            this.to_Label.Location = new System.Drawing.Point(25, 142);
            this.to_Label.Name = "to_Label";
            this.to_Label.Size = new System.Drawing.Size(23, 13);
            this.to_Label.TabIndex = 15;
            this.to_Label.Text = "To:";
            // 
            // originalSize_Label
            // 
            this.originalSize_Label.AutoSize = true;
            this.originalSize_Label.Location = new System.Drawing.Point(25, 174);
            this.originalSize_Label.Name = "originalSize_Label";
            this.originalSize_Label.Size = new System.Drawing.Size(68, 13);
            this.originalSize_Label.TabIndex = 16;
            this.originalSize_Label.Text = "Original Size:";
            // 
            // originalSides_Label
            // 
            this.originalSides_Label.AutoSize = true;
            this.originalSides_Label.Location = new System.Drawing.Point(25, 210);
            this.originalSides_Label.Name = "originalSides_Label";
            this.originalSides_Label.Size = new System.Drawing.Size(74, 13);
            this.originalSides_Label.TabIndex = 17;
            this.originalSides_Label.Text = "Original Sides:";
            // 
            // imagePreview_Label
            // 
            this.imagePreview_Label.AutoSize = true;
            this.imagePreview_Label.Location = new System.Drawing.Point(25, 249);
            this.imagePreview_Label.Name = "imagePreview_Label";
            this.imagePreview_Label.Size = new System.Drawing.Size(80, 13);
            this.imagePreview_Label.TabIndex = 18;
            this.imagePreview_Label.Text = "Image Preview:";
            // 
            // fileType_Label
            // 
            this.fileType_Label.AutoSize = true;
            this.fileType_Label.Location = new System.Drawing.Point(25, 276);
            this.fileType_Label.Name = "fileType_Label";
            this.fileType_Label.Size = new System.Drawing.Size(53, 13);
            this.fileType_Label.TabIndex = 19;
            this.fileType_Label.Text = "File Type:";
            // 
            // resolution_Label
            // 
            this.resolution_Label.AutoSize = true;
            this.resolution_Label.Location = new System.Drawing.Point(25, 306);
            this.resolution_Label.Name = "resolution_Label";
            this.resolution_Label.Size = new System.Drawing.Size(60, 13);
            this.resolution_Label.TabIndex = 20;
            this.resolution_Label.Text = "Resolution:";
            // 
            // EmailSettings_Label
            // 
            this.EmailSettings_Label.AutoSize = true;
            this.EmailSettings_Label.Location = new System.Drawing.Point(261, 10);
            this.EmailSettings_Label.Name = "EmailSettings_Label";
            this.EmailSettings_Label.Size = new System.Drawing.Size(73, 13);
            this.EmailSettings_Label.TabIndex = 21;
            this.EmailSettings_Label.Text = "Email Settings";
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(30, 393);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(29, 13);
            this.port_label.TabIndex = 23;
            this.port_label.Text = "Port:";
            // 
            // fileSplit_Label
            // 
            this.fileSplit_Label.AutoSize = true;
            this.fileSplit_Label.Location = new System.Drawing.Point(30, 419);
            this.fileSplit_Label.Name = "fileSplit_Label";
            this.fileSplit_Label.Size = new System.Drawing.Size(72, 13);
            this.fileSplit_Label.TabIndex = 24;
            this.fileSplit_Label.Text = "File Split Size:";
            // 
            // defaultTo_ComboBox
            // 
            this.defaultTo_ComboBox.Location = new System.Drawing.Point(146, 136);
            this.defaultTo_ComboBox.Name = "defaultTo_ComboBox";
            this.defaultTo_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.defaultTo_ComboBox.TabIndex = 4;
            // 
            // resolution_ComboBox
            // 
            this.resolution_ComboBox.Location = new System.Drawing.Point(146, 297);
            this.resolution_ComboBox.Name = "resolution_ComboBox";
            this.resolution_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.resolution_ComboBox.TabIndex = 9;
            // 
            // fileType_ComboBox
            // 
            this.fileType_ComboBox.Location = new System.Drawing.Point(146, 264);
            this.fileType_ComboBox.Name = "fileType_ComboBox";
            this.fileType_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.fileType_ComboBox.TabIndex = 8;
            // 
            // imagePreview_ComboBox
            // 
            this.imagePreview_ComboBox.Location = new System.Drawing.Point(146, 234);
            this.imagePreview_ComboBox.Name = "imagePreview_ComboBox";
            this.imagePreview_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.imagePreview_ComboBox.TabIndex = 7;
            // 
            // originalSides_ComboBox
            // 
            this.originalSides_ComboBox.Location = new System.Drawing.Point(146, 201);
            this.originalSides_ComboBox.Name = "originalSides_ComboBox";
            this.originalSides_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.originalSides_ComboBox.TabIndex = 6;
            // 
            // originalSize_ComboBox
            // 
            this.originalSize_ComboBox.Location = new System.Drawing.Point(146, 168);
            this.originalSize_ComboBox.Name = "originalSize_ComboBox";
            this.originalSize_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.originalSize_ComboBox.TabIndex = 5;
            // 
            // defaultFrom_ComboBox
            // 
            this.defaultFrom_ComboBox.Location = new System.Drawing.Point(146, 71);
            this.defaultFrom_ComboBox.Name = "defaultFrom_ComboBox";
            this.defaultFrom_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.defaultFrom_ComboBox.TabIndex = 5;
            // 
            // from_TextBox
            // 
            this.from_TextBox.Location = new System.Drawing.Point(146, 104);
            this.from_TextBox.Name = "from_TextBox";
            this.from_TextBox.Size = new System.Drawing.Size(361, 26);
            this.from_TextBox.TabIndex = 3;
            // 
            // enable_ComboBox
            // 
            this.enable_ComboBox.Location = new System.Drawing.Point(146, 40);
            this.enable_ComboBox.Name = "enable_ComboBox";
            this.enable_ComboBox.Size = new System.Drawing.Size(363, 27);
            this.enable_ComboBox.TabIndex = 0;
            // 
            // outgoingSMTPServer_ChoiceBox
            // 
            this.outgoingSMTPServer_ChoiceBox.Location = new System.Drawing.Point(146, 342);
            this.outgoingSMTPServer_ChoiceBox.Name = "outgoingSMTPServer_ChoiceBox";
            this.outgoingSMTPServer_ChoiceBox.Size = new System.Drawing.Size(346, 133);
            this.outgoingSMTPServer_ChoiceBox.TabIndex = 25;
            // 
            // ssl_label
            // 
            this.ssl_label.AutoSize = true;
            this.ssl_label.Location = new System.Drawing.Point(25, 446);
            this.ssl_label.Name = "ssl_label";
            this.ssl_label.Size = new System.Drawing.Size(52, 13);
            this.ssl_label.TabIndex = 26;
            this.ssl_label.Text = "Use SSL:";
            // 
            // EmailDefaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ssl_label);
            this.Controls.Add(this.outgoingSMTPServer_ChoiceBox);
            this.Controls.Add(this.defaultTo_ComboBox);
            this.Controls.Add(this.fileSplit_Label);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.EmailSettings_Label);
            this.Controls.Add(this.resolution_Label);
            this.Controls.Add(this.fileType_Label);
            this.Controls.Add(this.imagePreview_Label);
            this.Controls.Add(this.originalSides_Label);
            this.Controls.Add(this.originalSize_Label);
            this.Controls.Add(this.to_Label);
            this.Controls.Add(this.defaultFrom_Label);
            this.Controls.Add(this.from_Label);
            this.Controls.Add(this.serverName_Label);
            this.Controls.Add(this.email_Label);
            this.Controls.Add(this.resolution_ComboBox);
            this.Controls.Add(this.fileType_ComboBox);
            this.Controls.Add(this.imagePreview_ComboBox);
            this.Controls.Add(this.originalSides_ComboBox);
            this.Controls.Add(this.originalSize_ComboBox);
            this.Controls.Add(this.defaultFrom_ComboBox);
            this.Controls.Add(this.from_TextBox);
            this.Controls.Add(this.enable_ComboBox);
            this.Name = "EmailDefaultControl";
            this.Size = new System.Drawing.Size(551, 478);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ChoiceComboControl enable_ComboBox;
        public ChoiceTextControl from_TextBox;
        public ChoiceComboControl defaultFrom_ComboBox;
        public ChoiceComboControl originalSize_ComboBox;
        public ChoiceComboControl originalSides_ComboBox;
        public ChoiceComboControl imagePreview_ComboBox;
        public ChoiceComboControl fileType_ComboBox;
        public ChoiceComboControl resolution_ComboBox;
        private System.Windows.Forms.Label email_Label;
        private System.Windows.Forms.Label serverName_Label;
        private System.Windows.Forms.Label from_Label;
        private System.Windows.Forms.Label defaultFrom_Label;
        private System.Windows.Forms.Label to_Label;
        private System.Windows.Forms.Label originalSize_Label;
        private System.Windows.Forms.Label originalSides_Label;
        private System.Windows.Forms.Label imagePreview_Label;
        private System.Windows.Forms.Label fileType_Label;
        private System.Windows.Forms.Label resolution_Label;
        private System.Windows.Forms.Label EmailSettings_Label;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label fileSplit_Label;
        private ChoiceComboControl defaultTo_ComboBox;
        private FourChoiceControl outgoingSMTPServer_ChoiceBox;
        private System.Windows.Forms.Label ssl_label;
    }
}
