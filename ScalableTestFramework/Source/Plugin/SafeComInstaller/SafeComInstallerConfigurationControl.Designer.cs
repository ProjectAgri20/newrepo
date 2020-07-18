namespace HP.ScalableTest.Plugin.SafeComInstaller
{
    partial class SafeComInstallerConfigurationControl
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
            this.safecom_groupBox = new System.Windows.Forms.GroupBox();
            this.safecom_serverComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.register_groupBox = new System.Windows.Forms.GroupBox();
            this.pin_label = new System.Windows.Forms.Label();
            this.userLogin_label = new System.Windows.Forms.Label();
            this.pincode_textBox = new System.Windows.Forms.TextBox();
            this.logon_textBox = new System.Windows.Forms.TextBox();
            this.safecomserver_label = new System.Windows.Forms.Label();
            this.tracking_groupBox = new System.Windows.Forms.GroupBox();
            this.chbSmartScan = new System.Windows.Forms.CheckBox();
            this.chbMail = new System.Windows.Forms.CheckBox();
            this.chbFax = new System.Windows.Forms.CheckBox();
            this.chbFolder = new System.Windows.Forms.CheckBox();
            this.chbJobStorageSave = new System.Windows.Forms.CheckBox();
            this.chbJobStoragePrint = new System.Windows.Forms.CheckBox();
            this.chbCopyC = new System.Windows.Forms.CheckBox();
            this.chbCopy = new System.Windows.Forms.CheckBox();
            this.chbPullPrint = new System.Windows.Forms.CheckBox();
            this.chbPosttrack = new System.Windows.Forms.CheckBox();
            this.print_groupBox = new System.Windows.Forms.GroupBox();
            this.chbLowTonerPreventPrinting = new System.Windows.Forms.CheckBox();
            this.chbPrintAllLIFO = new System.Windows.Forms.CheckBox();
            this.chbPrintAll = new System.Windows.Forms.CheckBox();
            this.chbHighSpeed = new System.Windows.Forms.CheckBox();
            this.login_groupBox = new System.Windows.Forms.GroupBox();
            this.prefill_label = new System.Windows.Forms.Label();
            this.login_label = new System.Windows.Forms.Label();
            this.domain_label = new System.Windows.Forms.Label();
            this.chbThirdEnable = new System.Windows.Forms.CheckBox();
            this.chbUsePin = new System.Windows.Forms.CheckBox();
            this.chbUseMaskUserCode = new System.Windows.Forms.CheckBox();
            this.prefilDomain = new System.Windows.Forms.ComboBox();
            this.devDomain = new System.Windows.Forms.TextBox();
            this.loginType = new System.Windows.Forms.ComboBox();
            this.tasks_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.browse_button = new System.Windows.Forms.Button();
            this.bundleFile_textBox = new System.Windows.Forms.TextBox();
            this.bundle_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.safecom_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.safecom_groupBox.SuspendLayout();
            this.register_groupBox.SuspendLayout();
            this.tracking_groupBox.SuspendLayout();
            this.print_groupBox.SuspendLayout();
            this.login_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // safecom_groupBox
            // 
            this.safecom_groupBox.Controls.Add(this.safecom_serverComboBox);
            this.safecom_groupBox.Controls.Add(this.register_groupBox);
            this.safecom_groupBox.Controls.Add(this.safecomserver_label);
            this.safecom_groupBox.Controls.Add(this.tracking_groupBox);
            this.safecom_groupBox.Controls.Add(this.print_groupBox);
            this.safecom_groupBox.Controls.Add(this.login_groupBox);
            this.safecom_groupBox.Controls.Add(this.tasks_comboBox);
            this.safecom_groupBox.Controls.Add(this.label2);
            this.safecom_groupBox.Controls.Add(this.label1);
            this.safecom_groupBox.Controls.Add(this.browse_button);
            this.safecom_groupBox.Controls.Add(this.bundleFile_textBox);
            this.safecom_groupBox.Location = new System.Drawing.Point(3, 246);
            this.safecom_groupBox.Name = "safecom_groupBox";
            this.safecom_groupBox.Size = new System.Drawing.Size(656, 388);
            this.safecom_groupBox.TabIndex = 1;
            this.safecom_groupBox.TabStop = false;
            this.safecom_groupBox.Text = "Safecom Adminstration";
            // 
            // safecom_serverComboBox
            // 
            this.safecom_serverComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safecom_serverComboBox.Location = new System.Drawing.Point(8, 89);
            this.safecom_serverComboBox.Name = "safecom_serverComboBox";
            this.safecom_serverComboBox.Size = new System.Drawing.Size(210, 23);
            this.safecom_serverComboBox.TabIndex = 16;
            // 
            // register_groupBox
            // 
            this.register_groupBox.Controls.Add(this.pin_label);
            this.register_groupBox.Controls.Add(this.userLogin_label);
            this.register_groupBox.Controls.Add(this.pincode_textBox);
            this.register_groupBox.Controls.Add(this.logon_textBox);
            this.register_groupBox.Location = new System.Drawing.Point(233, 66);
            this.register_groupBox.Name = "register_groupBox";
            this.register_groupBox.Size = new System.Drawing.Size(387, 66);
            this.register_groupBox.TabIndex = 15;
            this.register_groupBox.TabStop = false;
            this.register_groupBox.Text = "Registeration Credentials";
            // 
            // pin_label
            // 
            this.pin_label.AutoSize = true;
            this.pin_label.Location = new System.Drawing.Point(184, 19);
            this.pin_label.Name = "pin_label";
            this.pin_label.Size = new System.Drawing.Size(60, 15);
            this.pin_label.TabIndex = 3;
            this.pin_label.Text = "PIN Code:";
            // 
            // userLogin_label
            // 
            this.userLogin_label.AutoSize = true;
            this.userLogin_label.Location = new System.Drawing.Point(8, 19);
            this.userLogin_label.Name = "userLogin_label";
            this.userLogin_label.Size = new System.Drawing.Size(70, 15);
            this.userLogin_label.TabIndex = 2;
            this.userLogin_label.Text = "User Logon:";
            // 
            // pincode_textBox
            // 
            this.pincode_textBox.Location = new System.Drawing.Point(187, 37);
            this.pincode_textBox.Name = "pincode_textBox";
            this.pincode_textBox.Size = new System.Drawing.Size(170, 23);
            this.pincode_textBox.TabIndex = 1;
            // 
            // logon_textBox
            // 
            this.logon_textBox.Location = new System.Drawing.Point(11, 37);
            this.logon_textBox.Name = "logon_textBox";
            this.logon_textBox.Size = new System.Drawing.Size(170, 23);
            this.logon_textBox.TabIndex = 0;
            // 
            // safecomserver_label
            // 
            this.safecomserver_label.AutoSize = true;
            this.safecomserver_label.Location = new System.Drawing.Point(11, 71);
            this.safecomserver_label.Name = "safecomserver_label";
            this.safecomserver_label.Size = new System.Drawing.Size(90, 15);
            this.safecomserver_label.TabIndex = 14;
            this.safecomserver_label.Text = "SafeCom Server";
            // 
            // tracking_groupBox
            // 
            this.tracking_groupBox.Controls.Add(this.chbSmartScan);
            this.tracking_groupBox.Controls.Add(this.chbMail);
            this.tracking_groupBox.Controls.Add(this.chbFax);
            this.tracking_groupBox.Controls.Add(this.chbFolder);
            this.tracking_groupBox.Controls.Add(this.chbJobStorageSave);
            this.tracking_groupBox.Controls.Add(this.chbJobStoragePrint);
            this.tracking_groupBox.Controls.Add(this.chbCopyC);
            this.tracking_groupBox.Controls.Add(this.chbCopy);
            this.tracking_groupBox.Controls.Add(this.chbPullPrint);
            this.tracking_groupBox.Controls.Add(this.chbPosttrack);
            this.tracking_groupBox.Location = new System.Drawing.Point(420, 138);
            this.tracking_groupBox.Name = "tracking_groupBox";
            this.tracking_groupBox.Size = new System.Drawing.Size(200, 234);
            this.tracking_groupBox.TabIndex = 7;
            this.tracking_groupBox.TabStop = false;
            this.tracking_groupBox.Text = "Tracking Settings";
            // 
            // chbSmartScan
            // 
            this.chbSmartScan.AutoSize = true;
            this.chbSmartScan.Checked = true;
            this.chbSmartScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSmartScan.Location = new System.Drawing.Point(102, 47);
            this.chbSmartScan.Name = "chbSmartScan";
            this.chbSmartScan.Size = new System.Drawing.Size(85, 19);
            this.chbSmartScan.TabIndex = 19;
            this.chbSmartScan.Text = "Smart Scan";
            this.chbSmartScan.UseVisualStyleBackColor = true;
            // 
            // chbMail
            // 
            this.chbMail.AutoSize = true;
            this.chbMail.Location = new System.Drawing.Point(102, 22);
            this.chbMail.Name = "chbMail";
            this.chbMail.Size = new System.Drawing.Size(60, 19);
            this.chbMail.TabIndex = 18;
            this.chbMail.Text = "E-mail";
            this.chbMail.UseVisualStyleBackColor = true;
            // 
            // chbFax
            // 
            this.chbFax.AutoSize = true;
            this.chbFax.Location = new System.Drawing.Point(6, 204);
            this.chbFax.Name = "chbFax";
            this.chbFax.Size = new System.Drawing.Size(43, 19);
            this.chbFax.TabIndex = 17;
            this.chbFax.Text = "Fax";
            this.chbFax.UseVisualStyleBackColor = true;
            // 
            // chbFolder
            // 
            this.chbFolder.AutoSize = true;
            this.chbFolder.Location = new System.Drawing.Point(6, 178);
            this.chbFolder.Name = "chbFolder";
            this.chbFolder.Size = new System.Drawing.Size(59, 19);
            this.chbFolder.TabIndex = 16;
            this.chbFolder.Text = "Folder";
            this.chbFolder.UseVisualStyleBackColor = true;
            // 
            // chbJobStorageSave
            // 
            this.chbJobStorageSave.AutoSize = true;
            this.chbJobStorageSave.Location = new System.Drawing.Point(6, 152);
            this.chbJobStorageSave.Name = "chbJobStorageSave";
            this.chbJobStorageSave.Size = new System.Drawing.Size(150, 19);
            this.chbJobStorageSave.TabIndex = 15;
            this.chbJobStorageSave.Text = "Save to Device Memory";
            this.chbJobStorageSave.UseVisualStyleBackColor = true;
            // 
            // chbJobStoragePrint
            // 
            this.chbJobStoragePrint.AutoSize = true;
            this.chbJobStoragePrint.Location = new System.Drawing.Point(6, 126);
            this.chbJobStoragePrint.Name = "chbJobStoragePrint";
            this.chbJobStoragePrint.Size = new System.Drawing.Size(183, 19);
            this.chbJobStoragePrint.TabIndex = 14;
            this.chbJobStoragePrint.Text = "Retrieve from Device Memory";
            this.chbJobStoragePrint.UseVisualStyleBackColor = true;
            // 
            // chbCopyC
            // 
            this.chbCopyC.AutoSize = true;
            this.chbCopyC.Location = new System.Drawing.Point(6, 100);
            this.chbCopyC.Name = "chbCopyC";
            this.chbCopyC.Size = new System.Drawing.Size(84, 19);
            this.chbCopyC.TabIndex = 13;
            this.chbCopyC.Text = "Color copy";
            this.chbCopyC.UseVisualStyleBackColor = true;
            // 
            // chbCopy
            // 
            this.chbCopy.AutoSize = true;
            this.chbCopy.Location = new System.Drawing.Point(6, 74);
            this.chbCopy.Name = "chbCopy";
            this.chbCopy.Size = new System.Drawing.Size(54, 19);
            this.chbCopy.TabIndex = 12;
            this.chbCopy.Text = "Copy";
            this.chbCopy.UseVisualStyleBackColor = true;
            // 
            // chbPullPrint
            // 
            this.chbPullPrint.AutoSize = true;
            this.chbPullPrint.Checked = true;
            this.chbPullPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPullPrint.Location = new System.Drawing.Point(6, 48);
            this.chbPullPrint.Name = "chbPullPrint";
            this.chbPullPrint.Size = new System.Drawing.Size(74, 19);
            this.chbPullPrint.TabIndex = 11;
            this.chbPullPrint.Text = "Pull Print";
            this.chbPullPrint.UseVisualStyleBackColor = true;
            // 
            // chbPosttrack
            // 
            this.chbPosttrack.AutoSize = true;
            this.chbPosttrack.Checked = true;
            this.chbPosttrack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPosttrack.Location = new System.Drawing.Point(6, 22);
            this.chbPosttrack.Name = "chbPosttrack";
            this.chbPosttrack.Size = new System.Drawing.Size(78, 19);
            this.chbPosttrack.TabIndex = 10;
            this.chbPosttrack.Text = "Post track";
            this.chbPosttrack.UseVisualStyleBackColor = true;
            // 
            // print_groupBox
            // 
            this.print_groupBox.Controls.Add(this.chbLowTonerPreventPrinting);
            this.print_groupBox.Controls.Add(this.chbPrintAllLIFO);
            this.print_groupBox.Controls.Add(this.chbPrintAll);
            this.print_groupBox.Controls.Add(this.chbHighSpeed);
            this.print_groupBox.Location = new System.Drawing.Point(233, 138);
            this.print_groupBox.Name = "print_groupBox";
            this.print_groupBox.Size = new System.Drawing.Size(181, 234);
            this.print_groupBox.TabIndex = 6;
            this.print_groupBox.TabStop = false;
            this.print_groupBox.Text = "Print Settings";
            // 
            // chbLowTonerPreventPrinting
            // 
            this.chbLowTonerPreventPrinting.AutoSize = true;
            this.chbLowTonerPreventPrinting.Location = new System.Drawing.Point(6, 103);
            this.chbLowTonerPreventPrinting.Name = "chbLowTonerPreventPrinting";
            this.chbLowTonerPreventPrinting.Size = new System.Drawing.Size(164, 19);
            this.chbLowTonerPreventPrinting.TabIndex = 12;
            this.chbLowTonerPreventPrinting.Text = "Prevent low toner printing";
            this.chbLowTonerPreventPrinting.UseVisualStyleBackColor = true;
            // 
            // chbPrintAllLIFO
            // 
            this.chbPrintAllLIFO.AutoSize = true;
            this.chbPrintAllLIFO.Location = new System.Drawing.Point(6, 76);
            this.chbPrintAllLIFO.Name = "chbPrintAllLIFO";
            this.chbPrintAllLIFO.Size = new System.Drawing.Size(118, 19);
            this.chbPrintAllLIFO.TabIndex = 11;
            this.chbPrintAllLIFO.Text = "Show newest first";
            this.chbPrintAllLIFO.UseVisualStyleBackColor = true;
            // 
            // chbPrintAll
            // 
            this.chbPrintAll.AutoSize = true;
            this.chbPrintAll.Location = new System.Drawing.Point(6, 49);
            this.chbPrintAll.Name = "chbPrintAll";
            this.chbPrintAll.Size = new System.Drawing.Size(109, 19);
            this.chbPrintAll.TabIndex = 10;
            this.chbPrintAll.Text = "Print all at login";
            this.chbPrintAll.UseVisualStyleBackColor = true;
            // 
            // chbHighSpeed
            // 
            this.chbHighSpeed.AutoSize = true;
            this.chbHighSpeed.Location = new System.Drawing.Point(6, 22);
            this.chbHighSpeed.Name = "chbHighSpeed";
            this.chbHighSpeed.Size = new System.Drawing.Size(114, 19);
            this.chbHighSpeed.TabIndex = 9;
            this.chbHighSpeed.Text = "High speed print";
            this.chbHighSpeed.UseVisualStyleBackColor = true;
            // 
            // login_groupBox
            // 
            this.login_groupBox.Controls.Add(this.prefill_label);
            this.login_groupBox.Controls.Add(this.login_label);
            this.login_groupBox.Controls.Add(this.domain_label);
            this.login_groupBox.Controls.Add(this.chbThirdEnable);
            this.login_groupBox.Controls.Add(this.chbUsePin);
            this.login_groupBox.Controls.Add(this.chbUseMaskUserCode);
            this.login_groupBox.Controls.Add(this.prefilDomain);
            this.login_groupBox.Controls.Add(this.devDomain);
            this.login_groupBox.Controls.Add(this.loginType);
            this.login_groupBox.Location = new System.Drawing.Point(8, 129);
            this.login_groupBox.Name = "login_groupBox";
            this.login_groupBox.Size = new System.Drawing.Size(219, 243);
            this.login_groupBox.TabIndex = 5;
            this.login_groupBox.TabStop = false;
            this.login_groupBox.Text = "Login Settings";
            // 
            // prefill_label
            // 
            this.prefill_label.AutoSize = true;
            this.prefill_label.Location = new System.Drawing.Point(6, 112);
            this.prefill_label.Name = "prefill_label";
            this.prefill_label.Size = new System.Drawing.Size(87, 15);
            this.prefill_label.TabIndex = 13;
            this.prefill_label.Text = "Pre-fill Domain";
            // 
            // login_label
            // 
            this.login_label.AutoSize = true;
            this.login_label.Location = new System.Drawing.Point(6, 23);
            this.login_label.Name = "login_label";
            this.login_label.Size = new System.Drawing.Size(82, 15);
            this.login_label.TabIndex = 8;
            this.login_label.Text = "Login Method";
            // 
            // domain_label
            // 
            this.domain_label.AutoSize = true;
            this.domain_label.Location = new System.Drawing.Point(6, 68);
            this.domain_label.Name = "domain_label";
            this.domain_label.Size = new System.Drawing.Size(90, 15);
            this.domain_label.TabIndex = 12;
            this.domain_label.Text = "Default Domain";
            // 
            // chbThirdEnable
            // 
            this.chbThirdEnable.AutoSize = true;
            this.chbThirdEnable.Location = new System.Drawing.Point(6, 209);
            this.chbThirdEnable.Name = "chbThirdEnable";
            this.chbThirdEnable.Size = new System.Drawing.Size(204, 19);
            this.chbThirdEnable.TabIndex = 11;
            this.chbThirdEnable.Text = "Enable Third Party Authentication";
            this.chbThirdEnable.UseVisualStyleBackColor = true;
            // 
            // chbUsePin
            // 
            this.chbUsePin.AutoSize = true;
            this.chbUsePin.Checked = true;
            this.chbUsePin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbUsePin.Location = new System.Drawing.Point(6, 182);
            this.chbUsePin.Name = "chbUsePin";
            this.chbUsePin.Size = new System.Drawing.Size(125, 19);
            this.chbUsePin.TabIndex = 10;
            this.chbUsePin.Text = "Log in without PIN";
            this.chbUsePin.UseVisualStyleBackColor = true;
            // 
            // chbUseMaskUserCode
            // 
            this.chbUseMaskUserCode.AutoSize = true;
            this.chbUseMaskUserCode.Location = new System.Drawing.Point(6, 157);
            this.chbUseMaskUserCode.Name = "chbUseMaskUserCode";
            this.chbUseMaskUserCode.Size = new System.Drawing.Size(99, 19);
            this.chbUseMaskUserCode.TabIndex = 8;
            this.chbUseMaskUserCode.Text = "Mask ID Code";
            this.chbUseMaskUserCode.UseVisualStyleBackColor = true;
            // 
            // prefilDomain
            // 
            this.prefilDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.prefilDomain.FormattingEnabled = true;
            this.prefilDomain.Location = new System.Drawing.Point(6, 128);
            this.prefilDomain.Name = "prefilDomain";
            this.prefilDomain.Size = new System.Drawing.Size(139, 23);
            this.prefilDomain.TabIndex = 9;
            // 
            // devDomain
            // 
            this.devDomain.Location = new System.Drawing.Point(6, 86);
            this.devDomain.Name = "devDomain";
            this.devDomain.Size = new System.Drawing.Size(139, 23);
            this.devDomain.TabIndex = 0;
            // 
            // loginType
            // 
            this.loginType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loginType.FormattingEnabled = true;
            this.loginType.Location = new System.Drawing.Point(6, 41);
            this.loginType.Name = "loginType";
            this.loginType.Size = new System.Drawing.Size(139, 23);
            this.loginType.TabIndex = 8;
            // 
            // tasks_comboBox
            // 
            this.tasks_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tasks_comboBox.FormattingEnabled = true;
            this.tasks_comboBox.Location = new System.Drawing.Point(9, 37);
            this.tasks_comboBox.Name = "tasks_comboBox";
            this.tasks_comboBox.Size = new System.Drawing.Size(184, 23);
            this.tasks_comboBox.TabIndex = 4;
            this.tasks_comboBox.SelectedIndexChanged += new System.EventHandler(this.tasks_comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Administration Task";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Safecom Bundle File";
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(571, 37);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(75, 23);
            this.browse_button.TabIndex = 1;
            this.browse_button.Text = "Browse...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // bundleFile_textBox
            // 
            this.bundleFile_textBox.BackColor = System.Drawing.Color.White;
            this.bundleFile_textBox.Location = new System.Drawing.Point(233, 37);
            this.bundleFile_textBox.Name = "bundleFile_textBox";
            this.bundleFile_textBox.ReadOnly = true;
            this.bundleFile_textBox.Size = new System.Drawing.Size(335, 23);
            this.bundleFile_textBox.TabIndex = 0;
            // 
            // safecom_assetSelectionControl
            // 
            this.safecom_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safecom_assetSelectionControl.Location = new System.Drawing.Point(3, 12);
            this.safecom_assetSelectionControl.Name = "safecom_assetSelectionControl";
            this.safecom_assetSelectionControl.Size = new System.Drawing.Size(656, 228);
            this.safecom_assetSelectionControl.TabIndex = 0;
            // 
            // SafeComInstallerConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.safecom_groupBox);
            this.Controls.Add(this.safecom_assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SafeComInstallerConfigurationControl";
            this.Size = new System.Drawing.Size(688, 637);
            this.safecom_groupBox.ResumeLayout(false);
            this.safecom_groupBox.PerformLayout();
            this.register_groupBox.ResumeLayout(false);
            this.register_groupBox.PerformLayout();
            this.tracking_groupBox.ResumeLayout(false);
            this.tracking_groupBox.PerformLayout();
            this.print_groupBox.ResumeLayout(false);
            this.print_groupBox.PerformLayout();
            this.login_groupBox.ResumeLayout(false);
            this.login_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl safecom_assetSelectionControl;
        private System.Windows.Forms.GroupBox safecom_groupBox;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox bundleFile_textBox;
        private System.Windows.Forms.OpenFileDialog bundle_openFileDialog;
        private System.Windows.Forms.ComboBox tasks_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox tracking_groupBox;
        private System.Windows.Forms.GroupBox print_groupBox;
        private System.Windows.Forms.GroupBox login_groupBox;
        private System.Windows.Forms.ComboBox loginType;
        private System.Windows.Forms.ComboBox prefilDomain;
        private System.Windows.Forms.TextBox devDomain;
        private System.Windows.Forms.CheckBox chbSmartScan;
        private System.Windows.Forms.CheckBox chbMail;
        private System.Windows.Forms.CheckBox chbFax;
        private System.Windows.Forms.CheckBox chbFolder;
        private System.Windows.Forms.CheckBox chbJobStorageSave;
        private System.Windows.Forms.CheckBox chbJobStoragePrint;
        private System.Windows.Forms.CheckBox chbCopyC;
        private System.Windows.Forms.CheckBox chbCopy;
        private System.Windows.Forms.CheckBox chbPullPrint;
        private System.Windows.Forms.CheckBox chbPosttrack;
        private System.Windows.Forms.CheckBox chbLowTonerPreventPrinting;
        private System.Windows.Forms.CheckBox chbPrintAllLIFO;
        private System.Windows.Forms.CheckBox chbPrintAll;
        private System.Windows.Forms.CheckBox chbHighSpeed;
        private System.Windows.Forms.Label prefill_label;
        private System.Windows.Forms.Label login_label;
        private System.Windows.Forms.Label domain_label;
        private System.Windows.Forms.CheckBox chbThirdEnable;
        private System.Windows.Forms.CheckBox chbUsePin;
        private System.Windows.Forms.CheckBox chbUseMaskUserCode;
        private System.Windows.Forms.Label safecomserver_label;
        private System.Windows.Forms.GroupBox register_groupBox;
        private System.Windows.Forms.Label pin_label;
        private System.Windows.Forms.Label userLogin_label;
        private System.Windows.Forms.TextBox pincode_textBox;
        private System.Windows.Forms.TextBox logon_textBox;
        private Framework.UI.ServerComboBox safecom_serverComboBox;
    }
}
