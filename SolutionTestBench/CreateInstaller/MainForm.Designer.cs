namespace CreateInstaller
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelLabel = new System.Windows.Forms.Label();
            this.textBox_Label = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox_Build = new System.Windows.Forms.GroupBox();
            this.clientCheckBox = new System.Windows.Forms.CheckBox();
            this.serverCheckBox = new System.Windows.Forms.CheckBox();
            this.label_StatusDisplay = new System.Windows.Forms.Label();
            this.label_Status = new System.Windows.Forms.Label();
            this.label_WorkingPath = new System.Windows.Forms.Label();
            this.textBox_WorkingPath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBox_Sign = new System.Windows.Forms.CheckBox();
            this.checkBox_Package = new System.Windows.Forms.CheckBox();
            this.groupBox_SignPackage = new System.Windows.Forms.GroupBox();
            this.textBox_InstallerPath = new System.Windows.Forms.TextBox();
            this.label_InstallerPath = new System.Windows.Forms.Label();
            this.checkBox_Build = new System.Windows.Forms.CheckBox();
            this.checkBox_SignPackage = new System.Windows.Forms.CheckBox();
            this.button_ViewPackage = new System.Windows.Forms.Button();
            this.listBox_BuildConfig = new System.Windows.Forms.CheckedListBox();
            this.label_BuildConfig = new System.Windows.Forms.Label();
            this.checkBox_Authenticate = new System.Windows.Forms.CheckBox();
            this.groupBox_Build.SuspendLayout();
            this.groupBox_SignPackage.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLabel
            // 
            this.labelLabel.Location = new System.Drawing.Point(16, 50);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(106, 21);
            this.labelLabel.TabIndex = 0;
            this.labelLabel.Text = "Package Label";
            this.labelLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_Label
            // 
            this.textBox_Label.Location = new System.Drawing.Point(125, 48);
            this.textBox_Label.Name = "textBox_Label";
            this.textBox_Label.Size = new System.Drawing.Size(232, 21);
            this.textBox_Label.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(383, 468);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "Start";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(489, 468);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Exit";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBox_Build
            // 
            this.groupBox_Build.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Build.Controls.Add(this.clientCheckBox);
            this.groupBox_Build.Controls.Add(this.serverCheckBox);
            this.groupBox_Build.Location = new System.Drawing.Point(30, 165);
            this.groupBox_Build.Name = "groupBox_Build";
            this.groupBox_Build.Size = new System.Drawing.Size(559, 80);
            this.groupBox_Build.TabIndex = 9;
            this.groupBox_Build.TabStop = false;
            // 
            // clientCheckBox
            // 
            this.clientCheckBox.AutoSize = true;
            this.clientCheckBox.Checked = true;
            this.clientCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.clientCheckBox.Location = new System.Drawing.Point(95, 45);
            this.clientCheckBox.Name = "clientCheckBox";
            this.clientCheckBox.Size = new System.Drawing.Size(134, 19);
            this.clientCheckBox.TabIndex = 11;
            this.clientCheckBox.Text = "Build Client Installer";
            this.clientCheckBox.UseVisualStyleBackColor = true;
            // 
            // serverCheckBox
            // 
            this.serverCheckBox.AutoSize = true;
            this.serverCheckBox.Checked = true;
            this.serverCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.serverCheckBox.Location = new System.Drawing.Point(95, 20);
            this.serverCheckBox.Name = "serverCheckBox";
            this.serverCheckBox.Size = new System.Drawing.Size(138, 19);
            this.serverCheckBox.TabIndex = 10;
            this.serverCheckBox.Text = "Build Server Installer";
            this.serverCheckBox.UseVisualStyleBackColor = true;
            // 
            // label_StatusDisplay
            // 
            this.label_StatusDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_StatusDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_StatusDisplay.Location = new System.Drawing.Point(97, 439);
            this.label_StatusDisplay.Name = "label_StatusDisplay";
            this.label_StatusDisplay.Size = new System.Drawing.Size(492, 18);
            this.label_StatusDisplay.TabIndex = 11;
            // 
            // label_Status
            // 
            this.label_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_Status.AutoSize = true;
            this.label_Status.Location = new System.Drawing.Point(12, 439);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(72, 15);
            this.label_Status.TabIndex = 12;
            this.label_Status.Text = "Build Status";
            // 
            // label_WorkingPath
            // 
            this.label_WorkingPath.Location = new System.Drawing.Point(16, 23);
            this.label_WorkingPath.Name = "label_WorkingPath";
            this.label_WorkingPath.Size = new System.Drawing.Size(106, 21);
            this.label_WorkingPath.TabIndex = 0;
            this.label_WorkingPath.Text = "Working Path";
            this.label_WorkingPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_WorkingPath
            // 
            this.textBox_WorkingPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_WorkingPath.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_WorkingPath.Location = new System.Drawing.Point(125, 21);
            this.textBox_WorkingPath.Name = "textBox_WorkingPath";
            this.textBox_WorkingPath.ReadOnly = true;
            this.textBox_WorkingPath.Size = new System.Drawing.Size(464, 21);
            this.textBox_WorkingPath.TabIndex = 1;
            // 
            // checkBox_Sign
            // 
            this.checkBox_Sign.AutoSize = true;
            this.checkBox_Sign.Checked = true;
            this.checkBox_Sign.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Sign.Location = new System.Drawing.Point(95, 77);
            this.checkBox_Sign.Name = "checkBox_Sign";
            this.checkBox_Sign.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox_Sign.Size = new System.Drawing.Size(117, 19);
            this.checkBox_Sign.TabIndex = 13;
            this.checkBox_Sign.Text = "Sign Assemblies";
            this.checkBox_Sign.UseVisualStyleBackColor = true;
            this.checkBox_Sign.CheckedChanged += new System.EventHandler(this.Sign_CheckedChanged);
            // 
            // checkBox_Package
            // 
            this.checkBox_Package.AutoSize = true;
            this.checkBox_Package.Checked = true;
            this.checkBox_Package.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Package.Location = new System.Drawing.Point(95, 102);
            this.checkBox_Package.Name = "checkBox_Package";
            this.checkBox_Package.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox_Package.Size = new System.Drawing.Size(119, 19);
            this.checkBox_Package.TabIndex = 14;
            this.checkBox_Package.Text = "Create Packages";
            this.checkBox_Package.UseVisualStyleBackColor = true;
            // 
            // groupBox_SignPackage
            // 
            this.groupBox_SignPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_SignPackage.Controls.Add(this.checkBox_Authenticate);
            this.groupBox_SignPackage.Controls.Add(this.textBox_InstallerPath);
            this.groupBox_SignPackage.Controls.Add(this.label_InstallerPath);
            this.groupBox_SignPackage.Controls.Add(this.checkBox_Sign);
            this.groupBox_SignPackage.Controls.Add(this.checkBox_Package);
            this.groupBox_SignPackage.Enabled = false;
            this.groupBox_SignPackage.Location = new System.Drawing.Point(30, 254);
            this.groupBox_SignPackage.Name = "groupBox_SignPackage";
            this.groupBox_SignPackage.Size = new System.Drawing.Size(559, 141);
            this.groupBox_SignPackage.TabIndex = 15;
            this.groupBox_SignPackage.TabStop = false;
            // 
            // textBox_InstallerPath
            // 
            this.textBox_InstallerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_InstallerPath.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_InstallerPath.Location = new System.Drawing.Point(95, 25);
            this.textBox_InstallerPath.Name = "textBox_InstallerPath";
            this.textBox_InstallerPath.ReadOnly = true;
            this.textBox_InstallerPath.Size = new System.Drawing.Size(458, 21);
            this.textBox_InstallerPath.TabIndex = 16;
            // 
            // label_InstallerPath
            // 
            this.label_InstallerPath.Location = new System.Drawing.Point(12, 28);
            this.label_InstallerPath.Name = "label_InstallerPath";
            this.label_InstallerPath.Size = new System.Drawing.Size(97, 18);
            this.label_InstallerPath.TabIndex = 15;
            this.label_InstallerPath.Text = "Source Path";
            // 
            // checkBox_Build
            // 
            this.checkBox_Build.AutoSize = true;
            this.checkBox_Build.Checked = true;
            this.checkBox_Build.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Build.Location = new System.Drawing.Point(12, 163);
            this.checkBox_Build.Name = "checkBox_Build";
            this.checkBox_Build.Size = new System.Drawing.Size(54, 19);
            this.checkBox_Build.TabIndex = 16;
            this.checkBox_Build.Text = "Build";
            this.checkBox_Build.UseVisualStyleBackColor = true;
            this.checkBox_Build.CheckedChanged += new System.EventHandler(this.checkBox_Build_CheckedChanged);
            // 
            // checkBox_SignPackage
            // 
            this.checkBox_SignPackage.AutoSize = true;
            this.checkBox_SignPackage.Location = new System.Drawing.Point(12, 251);
            this.checkBox_SignPackage.Name = "checkBox_SignPackage";
            this.checkBox_SignPackage.Size = new System.Drawing.Size(212, 19);
            this.checkBox_SignPackage.TabIndex = 17;
            this.checkBox_SignPackage.Text = "Sign and Package (Parnter Portal)";
            this.checkBox_SignPackage.UseVisualStyleBackColor = true;
            this.checkBox_SignPackage.CheckedChanged += new System.EventHandler(this.checkBox_SignPackage_CheckedChanged);
            // 
            // button_ViewPackage
            // 
            this.button_ViewPackage.Location = new System.Drawing.Point(363, 48);
            this.button_ViewPackage.Name = "button_ViewPackage";
            this.button_ViewPackage.Size = new System.Drawing.Size(30, 23);
            this.button_ViewPackage.TabIndex = 18;
            this.button_ViewPackage.Text = ". . .";
            this.button_ViewPackage.UseVisualStyleBackColor = true;
            this.button_ViewPackage.Click += new System.EventHandler(this.button_ViewPackage_Click);
            // 
            // listBox_BuildConfig
            // 
            this.listBox_BuildConfig.CheckOnClick = true;
            this.listBox_BuildConfig.FormattingEnabled = true;
            this.listBox_BuildConfig.Location = new System.Drawing.Point(125, 75);
            this.listBox_BuildConfig.Name = "listBox_BuildConfig";
            this.listBox_BuildConfig.Size = new System.Drawing.Size(232, 84);
            this.listBox_BuildConfig.TabIndex = 20;
            // 
            // label_BuildConfig
            // 
            this.label_BuildConfig.Location = new System.Drawing.Point(22, 71);
            this.label_BuildConfig.Name = "label_BuildConfig";
            this.label_BuildConfig.Size = new System.Drawing.Size(97, 18);
            this.label_BuildConfig.TabIndex = 19;
            this.label_BuildConfig.Text = "Configuration";
            this.label_BuildConfig.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBox_Authenticate
            // 
            this.checkBox_Authenticate.AutoSize = true;
            this.checkBox_Authenticate.Location = new System.Drawing.Point(95, 52);
            this.checkBox_Authenticate.Name = "checkBox_Authenticate";
            this.checkBox_Authenticate.Size = new System.Drawing.Size(93, 19);
            this.checkBox_Authenticate.TabIndex = 17;
            this.checkBox_Authenticate.Text = "Authenticate";
            this.checkBox_Authenticate.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 503);
            this.Controls.Add(this.checkBox_Build);
            this.Controls.Add(this.listBox_BuildConfig);
            this.Controls.Add(this.label_BuildConfig);
            this.Controls.Add(this.button_ViewPackage);
            this.Controls.Add(this.checkBox_SignPackage);
            this.Controls.Add(this.groupBox_SignPackage);
            this.Controls.Add(this.labelLabel);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.textBox_Label);
            this.Controls.Add(this.label_StatusDisplay);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textBox_WorkingPath);
            this.Controls.Add(this.label_WorkingPath);
            this.Controls.Add(this.groupBox_Build);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "STB Installer Builder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox_Build.ResumeLayout(false);
            this.groupBox_Build.PerformLayout();
            this.groupBox_SignPackage.ResumeLayout(false);
            this.groupBox_SignPackage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLabel;
        private System.Windows.Forms.TextBox textBox_Label;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox_Build;
        private System.Windows.Forms.CheckBox clientCheckBox;
        private System.Windows.Forms.CheckBox serverCheckBox;
        private System.Windows.Forms.Label label_StatusDisplay;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Label label_WorkingPath;
        private System.Windows.Forms.TextBox textBox_WorkingPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox checkBox_Sign;
        private System.Windows.Forms.CheckBox checkBox_Package;
        private System.Windows.Forms.GroupBox groupBox_SignPackage;
        private System.Windows.Forms.Label label_InstallerPath;
        private System.Windows.Forms.CheckBox checkBox_Build;
        private System.Windows.Forms.CheckBox checkBox_SignPackage;
        private System.Windows.Forms.TextBox textBox_InstallerPath;
        private System.Windows.Forms.Button button_ViewPackage;
        private System.Windows.Forms.CheckedListBox listBox_BuildConfig;
        private System.Windows.Forms.Label label_BuildConfig;
        private System.Windows.Forms.CheckBox checkBox_Authenticate;
    }
}