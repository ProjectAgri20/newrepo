namespace HP.ScalableTest.Plugin.CertificateInstaller
{
    partial class CertificateInstallerConfigurationControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.certificate_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.certificates_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.FileBrowse = new System.Windows.Forms.Button();
            this.Certificate_Label = new System.Windows.Forms.Label();
            this.certificateName_Textbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.browser_label = new System.Windows.Forms.Label();
            this.browser_comboBox = new System.Windows.Forms.ComboBox();
            this.Intermediate_CA = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InstallVM_CA = new System.Windows.Forms.CheckBox();
            this.InstallPrinter_CA = new System.Windows.Forms.RadioButton();
            this.DeletePrinter_CA = new System.Windows.Forms.RadioButton();
            this.certificate_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // certificate_assetSelectionControl
            // 
            this.certificate_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.certificate_assetSelectionControl.Location = new System.Drawing.Point(7, 19);
            this.certificate_assetSelectionControl.Name = "certificate_assetSelectionControl";
            this.certificate_assetSelectionControl.Size = new System.Drawing.Size(590, 136);
            this.certificate_assetSelectionControl.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.certificate_assetSelectionControl);
            this.groupBox5.Location = new System.Drawing.Point(15, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(603, 161);
            this.groupBox5.TabIndex = 53;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Printer Details";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.certificates_checkedListBox);
            this.groupBox4.Controls.Add(this.FileBrowse);
            this.groupBox4.Controls.Add(this.Certificate_Label);
            this.groupBox4.Controls.Add(this.certificateName_Textbox);
            this.groupBox4.Location = new System.Drawing.Point(15, 269);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(603, 179);
            this.groupBox4.TabIndex = 49;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Select CA certificate for installation:";
            // 
            // certificates_checkedListBox
            // 
            this.certificates_checkedListBox.FormattingEnabled = true;
            this.certificates_checkedListBox.Location = new System.Drawing.Point(7, 19);
            this.certificates_checkedListBox.Name = "certificates_checkedListBox";
            this.certificates_checkedListBox.Size = new System.Drawing.Size(491, 109);
            this.certificates_checkedListBox.TabIndex = 10;
            this.certificates_checkedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Certificates_CheckChanged);
            // 
            // FileBrowse
            // 
            this.FileBrowse.Location = new System.Drawing.Point(504, 144);
            this.FileBrowse.Name = "FileBrowse";
            this.FileBrowse.Size = new System.Drawing.Size(60, 23);
            this.FileBrowse.TabIndex = 12;
            this.FileBrowse.Text = "Upload";
            this.FileBrowse.UseVisualStyleBackColor = true;
            // 
            // Certificate_Label
            // 
            this.Certificate_Label.AutoSize = true;
            this.Certificate_Label.Location = new System.Drawing.Point(2, 147);
            this.Certificate_Label.Name = "Certificate_Label";
            this.Certificate_Label.Size = new System.Drawing.Size(88, 13);
            this.Certificate_Label.TabIndex = 1;
            this.Certificate_Label.Text = "Certificate Name:";
            // 
            // certificateName_Textbox
            // 
            this.certificateName_Textbox.Location = new System.Drawing.Point(91, 144);
            this.certificateName_Textbox.Name = "certificateName_Textbox";
            this.certificateName_Textbox.Size = new System.Drawing.Size(407, 20);
            this.certificateName_Textbox.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(588, 41);
            this.label2.TabIndex = 1;
            this.label2.Text = "Check whether to install or delete CA certificate on printer. Provide the asset I" +
    "D of the device and select the certificate. Upload new certificate to Store if c" +
    "ertificate not available in list.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(15, 454);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(603, 64);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            // 
            // browser_label
            // 
            this.browser_label.AutoSize = true;
            this.browser_label.Location = new System.Drawing.Point(220, 26);
            this.browser_label.Name = "browser_label";
            this.browser_label.Size = new System.Drawing.Size(48, 13);
            this.browser_label.TabIndex = 47;
            this.browser_label.Text = "Browser:";
            // 
            // browser_comboBox
            // 
            this.browser_comboBox.Items.AddRange(new object[] {
            "IE7",
            "IE8",
            "IE9",
            "Firefox",
            "Chrome",
            "Opera",
            "Safari"});
            this.browser_comboBox.Location = new System.Drawing.Point(274, 23);
            this.browser_comboBox.Name = "browser_comboBox";
            this.browser_comboBox.Size = new System.Drawing.Size(107, 21);
            this.browser_comboBox.TabIndex = 5;
            // 
            // Intermediate_CA
            // 
            this.Intermediate_CA.AutoSize = true;
            this.Intermediate_CA.Location = new System.Drawing.Point(30, 42);
            this.Intermediate_CA.Name = "Intermediate_CA";
            this.Intermediate_CA.Size = new System.Drawing.Size(129, 17);
            this.Intermediate_CA.TabIndex = 4;
            this.Intermediate_CA.Text = "Allow Intermediate CA";
            this.Intermediate_CA.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.InstallVM_CA);
            this.groupBox2.Controls.Add(this.browser_label);
            this.groupBox2.Controls.Add(this.browser_comboBox);
            this.groupBox2.Controls.Add(this.Intermediate_CA);
            this.groupBox2.Controls.Add(this.InstallPrinter_CA);
            this.groupBox2.Controls.Add(this.DeletePrinter_CA);
            this.groupBox2.Location = new System.Drawing.Point(15, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(603, 84);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Printer Certificates";
            // 
            // InstallVM_CA
            // 
            this.InstallVM_CA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallVM_CA.AutoSize = true;
            this.InstallVM_CA.Location = new System.Drawing.Point(223, 61);
            this.InstallVM_CA.Name = "InstallVM_CA";
            this.InstallVM_CA.Size = new System.Drawing.Size(230, 17);
            this.InstallVM_CA.TabIndex = 6;
            this.InstallVM_CA.Text = "Install CA certificate on Client VM (Optional)";
            this.InstallVM_CA.UseVisualStyleBackColor = true;
            // 
            // InstallPrinter_CA
            // 
            this.InstallPrinter_CA.AutoSize = true;
            this.InstallPrinter_CA.Checked = true;
            this.InstallPrinter_CA.Location = new System.Drawing.Point(7, 19);
            this.InstallPrinter_CA.Name = "InstallPrinter_CA";
            this.InstallPrinter_CA.Size = new System.Drawing.Size(167, 17);
            this.InstallPrinter_CA.TabIndex = 2;
            this.InstallPrinter_CA.TabStop = true;
            this.InstallPrinter_CA.Text = "Install CA Certificate on Printer";
            this.InstallPrinter_CA.UseVisualStyleBackColor = true;
            // 
            // DeletePrinter_CA
            // 
            this.DeletePrinter_CA.AutoSize = true;
            this.DeletePrinter_CA.Location = new System.Drawing.Point(7, 61);
            this.DeletePrinter_CA.Name = "DeletePrinter_CA";
            this.DeletePrinter_CA.Size = new System.Drawing.Size(169, 17);
            this.DeletePrinter_CA.TabIndex = 3;
            this.DeletePrinter_CA.TabStop = true;
            this.DeletePrinter_CA.Text = "Delete CA certificate on printer";
            this.DeletePrinter_CA.UseVisualStyleBackColor = true;
            // 
            // CertificateInstallerConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "CertificateInstallerConfigurationControl";
            this.Size = new System.Drawing.Size(640, 525);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.AssetSelectionControl certificate_assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckedListBox certificates_checkedListBox;
        private System.Windows.Forms.Button FileBrowse;
        private System.Windows.Forms.Label Certificate_Label;
        private System.Windows.Forms.TextBox certificateName_Textbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label browser_label;
        private System.Windows.Forms.ComboBox browser_comboBox;
        private System.Windows.Forms.CheckBox Intermediate_CA;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton InstallPrinter_CA;
        private System.Windows.Forms.RadioButton DeletePrinter_CA;
        private System.Windows.Forms.CheckBox InstallVM_CA;
        private Framework.UI.FieldValidator certificate_fieldValidator;
    }
}