namespace HP.ScalableTest.Plugin.ScanToFolder
{
    partial class ScanToFolderConfigControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_ScanToFolder = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.scanoptions_groupBox = new System.Windows.Forms.GroupBox();
            this.ScanOptions_Button = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.applyCredential_Checkbox = new System.Windows.Forms.CheckBox();
            this.useOcr_CheckBox = new System.Windows.Forms.CheckBox();
            this.folder_TextBox = new System.Windows.Forms.TextBox();
            this.quickSet_TextBox = new System.Windows.Forms.TextBox();
            this.quickSet_RadioButton = new System.Windows.Forms.RadioButton();
            this.networkFolder_RadioButton = new System.Windows.Forms.RadioButton();
            this.ExecutionSettingGroup = new System.Windows.Forms.GroupBox();
            this.RestrictRadioButton = new System.Windows.Forms.RadioButton();
            this.GenerateRadioButton = new System.Windows.Forms.RadioButton();
            this.OptionalRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.loggingOptions_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.usesDigitalSendServer_CheckBox = new System.Windows.Forms.CheckBox();
            this.digitalSendServer_TextBox = new System.Windows.Forms.TextBox();
            this.digitalSendServerName_Label = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.destinations_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.multipleFoldersExact_RadioButton = new System.Windows.Forms.RadioButton();
            this.multipleFoldersAuto_RadioButton = new System.Windows.Forms.RadioButton();
            this.singleFolder_RadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.panel1.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.scanoptions_groupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.ExecutionSettingGroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.loggingOptions_TabPage.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.destinations_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_Authentication);
            this.panel1.Controls.Add(this.scanoptions_groupBox);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.ExecutionSettingGroup);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 360);
            this.panel1.TabIndex = 0;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ScanToFolder);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 168);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(658, 49);
            this.groupBox_Authentication.TabIndex = 96;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_ScanToFolder
            // 
            this.radioButton_ScanToFolder.AutoSize = true;
            this.radioButton_ScanToFolder.Checked = true;
            this.radioButton_ScanToFolder.Location = new System.Drawing.Point(116, 20);
            this.radioButton_ScanToFolder.Name = "radioButton_ScanToFolder";
            this.radioButton_ScanToFolder.Size = new System.Drawing.Size(96, 19);
            this.radioButton_ScanToFolder.TabIndex = 93;
            this.radioButton_ScanToFolder.TabStop = true;
            this.radioButton_ScanToFolder.Text = "ScanToFolder";
            this.radioButton_ScanToFolder.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(61, 19);
            this.radioButton_SignInButton.TabIndex = 92;
            this.radioButton_SignInButton.Text = "Sign In";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(436, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(296, 21);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // scanoptions_groupBox
            // 
            this.scanoptions_groupBox.Controls.Add(this.ScanOptions_Button);
            this.scanoptions_groupBox.Location = new System.Drawing.Point(439, 113);
            this.scanoptions_groupBox.Name = "scanoptions_groupBox";
            this.scanoptions_groupBox.Size = new System.Drawing.Size(227, 56);
            this.scanoptions_groupBox.TabIndex = 55;
            this.scanoptions_groupBox.TabStop = false;
            this.scanoptions_groupBox.Text = "Scan Options";
            // 
            // ScanOptions_Button
            // 
            this.ScanOptions_Button.Location = new System.Drawing.Point(63, 22);
            this.ScanOptions_Button.Name = "ScanOptions_Button";
            this.ScanOptions_Button.Size = new System.Drawing.Size(114, 34);
            this.ScanOptions_Button.TabIndex = 0;
            this.ScanOptions_Button.Text = "Scan Options";
            this.ScanOptions_Button.UseVisualStyleBackColor = true;
            this.ScanOptions_Button.Click += new System.EventHandler(this.ScanPreferences_Button_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.applyCredential_Checkbox);
            this.groupBox3.Controls.Add(this.useOcr_CheckBox);
            this.groupBox3.Controls.Add(this.folder_TextBox);
            this.groupBox3.Controls.Add(this.quickSet_TextBox);
            this.groupBox3.Controls.Add(this.quickSet_RadioButton);
            this.groupBox3.Controls.Add(this.networkFolder_RadioButton);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(0, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(738, 104);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scan Destination";
            // 
            // applyCredential_Checkbox
            // 
            this.applyCredential_Checkbox.AutoSize = true;
            this.applyCredential_Checkbox.Location = new System.Drawing.Point(514, 28);
            this.applyCredential_Checkbox.Name = "applyCredential_Checkbox";
            this.applyCredential_Checkbox.Size = new System.Drawing.Size(198, 19);
            this.applyCredential_Checkbox.TabIndex = 53;
            this.applyCredential_Checkbox.Text = "Apply Credentials on Verification";
            this.applyCredential_Checkbox.UseVisualStyleBackColor = true;
            // 
            // useOcr_CheckBox
            // 
            this.useOcr_CheckBox.AutoSize = true;
            this.useOcr_CheckBox.Location = new System.Drawing.Point(514, 52);
            this.useOcr_CheckBox.Name = "useOcr_CheckBox";
            this.useOcr_CheckBox.Size = new System.Drawing.Size(110, 19);
            this.useOcr_CheckBox.TabIndex = 52;
            this.useOcr_CheckBox.Text = "Use OCR for job";
            this.useOcr_CheckBox.UseVisualStyleBackColor = true;
            this.useOcr_CheckBox.CheckedChanged += new System.EventHandler(this.useOcr_CheckBox_CheckedChanged);
            // 
            // folder_TextBox
            // 
            this.folder_TextBox.Location = new System.Drawing.Point(141, 26);
            this.folder_TextBox.Name = "folder_TextBox";
            this.folder_TextBox.Size = new System.Drawing.Size(355, 23);
            this.folder_TextBox.TabIndex = 51;
            // 
            // quickSet_TextBox
            // 
            this.quickSet_TextBox.Enabled = false;
            this.quickSet_TextBox.Location = new System.Drawing.Point(141, 63);
            this.quickSet_TextBox.Name = "quickSet_TextBox";
            this.quickSet_TextBox.Size = new System.Drawing.Size(355, 23);
            this.quickSet_TextBox.TabIndex = 50;
            // 
            // quickSet_RadioButton
            // 
            this.quickSet_RadioButton.AutoSize = true;
            this.quickSet_RadioButton.Location = new System.Drawing.Point(21, 64);
            this.quickSet_RadioButton.Name = "quickSet_RadioButton";
            this.quickSet_RadioButton.Size = new System.Drawing.Size(114, 19);
            this.quickSet_RadioButton.TabIndex = 49;
            this.quickSet_RadioButton.TabStop = true;
            this.quickSet_RadioButton.Text = "Named QuickSet";
            this.quickSet_RadioButton.UseVisualStyleBackColor = true;
            // 
            // networkFolder_RadioButton
            // 
            this.networkFolder_RadioButton.AutoSize = true;
            this.networkFolder_RadioButton.Checked = true;
            this.networkFolder_RadioButton.Location = new System.Drawing.Point(21, 27);
            this.networkFolder_RadioButton.Name = "networkFolder_RadioButton";
            this.networkFolder_RadioButton.Size = new System.Drawing.Size(106, 19);
            this.networkFolder_RadioButton.TabIndex = 48;
            this.networkFolder_RadioButton.TabStop = true;
            this.networkFolder_RadioButton.Text = "Network Folder";
            this.networkFolder_RadioButton.UseVisualStyleBackColor = true;
            this.networkFolder_RadioButton.CheckedChanged += new System.EventHandler(this.networkFolder_RadioButton_CheckedChanged);
            // 
            // ExecutionSettingGroup
            // 
            this.ExecutionSettingGroup.Controls.Add(this.RestrictRadioButton);
            this.ExecutionSettingGroup.Controls.Add(this.GenerateRadioButton);
            this.ExecutionSettingGroup.Controls.Add(this.OptionalRadioButton);
            this.ExecutionSettingGroup.Location = new System.Drawing.Point(3, 107);
            this.ExecutionSettingGroup.Name = "ExecutionSettingGroup";
            this.ExecutionSettingGroup.Size = new System.Drawing.Size(424, 62);
            this.ExecutionSettingGroup.TabIndex = 53;
            this.ExecutionSettingGroup.TabStop = false;
            this.ExecutionSettingGroup.Text = "Use Image Preview";
            // 
            // RestrictRadioButton
            // 
            this.RestrictRadioButton.AutoSize = true;
            this.RestrictRadioButton.Location = new System.Drawing.Point(247, 21);
            this.RestrictRadioButton.Name = "RestrictRadioButton";
            this.RestrictRadioButton.Size = new System.Drawing.Size(108, 19);
            this.RestrictRadioButton.TabIndex = 2;
            this.RestrictRadioButton.Text = "Restrict Preview";
            this.RestrictRadioButton.UseVisualStyleBackColor = true;
            // 
            // GenerateRadioButton
            // 
            this.GenerateRadioButton.AutoSize = true;
            this.GenerateRadioButton.Location = new System.Drawing.Point(105, 21);
            this.GenerateRadioButton.Name = "GenerateRadioButton";
            this.GenerateRadioButton.Size = new System.Drawing.Size(116, 19);
            this.GenerateRadioButton.TabIndex = 1;
            this.GenerateRadioButton.Text = "Generate Preview";
            this.GenerateRadioButton.UseVisualStyleBackColor = true;
            // 
            // OptionalRadioButton
            // 
            this.OptionalRadioButton.AutoSize = true;
            this.OptionalRadioButton.Checked = true;
            this.OptionalRadioButton.Location = new System.Drawing.Point(17, 21);
            this.OptionalRadioButton.Name = "OptionalRadioButton";
            this.OptionalRadioButton.Size = new System.Drawing.Size(71, 19);
            this.OptionalRadioButton.TabIndex = 0;
            this.OptionalRadioButton.TabStop = true;
            this.OptionalRadioButton.Text = "Optional";
            this.OptionalRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.pageCount_NumericUpDown);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(0, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 135);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Page Count";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(119, 98);
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
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 57);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.lockTimeoutControl);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(302, 219);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(359, 135);
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
            this.label2.Size = new System.Drawing.Size(345, 57);
            this.label2.TabIndex = 10;
            this.label2.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Controls.Add(this.loggingOptions_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(790, 650);
            this.tabControl.TabIndex = 48;
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.scanConfiguration_TabPage.Controls.Add(this.assetSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel1);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(782, 622);
            this.scanConfiguration_TabPage.TabIndex = 0;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 363);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(776, 256);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // loggingOptions_TabPage
            // 
            this.loggingOptions_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.loggingOptions_TabPage.Controls.Add(this.groupBox7);
            this.loggingOptions_TabPage.Controls.Add(this.groupBox1);
            this.loggingOptions_TabPage.Controls.Add(this.label1);
            this.loggingOptions_TabPage.Location = new System.Drawing.Point(4, 24);
            this.loggingOptions_TabPage.Name = "loggingOptions_TabPage";
            this.loggingOptions_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingOptions_TabPage.Size = new System.Drawing.Size(782, 550);
            this.loggingOptions_TabPage.TabIndex = 1;
            this.loggingOptions_TabPage.Text = "Logging Options";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.usesDigitalSendServer_CheckBox);
            this.groupBox7.Controls.Add(this.digitalSendServer_TextBox);
            this.groupBox7.Controls.Add(this.digitalSendServerName_Label);
            this.groupBox7.Location = new System.Drawing.Point(9, 196);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(317, 104);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Digital Send Service";
            // 
            // usesDigitalSendServer_CheckBox
            // 
            this.usesDigitalSendServer_CheckBox.AutoSize = true;
            this.usesDigitalSendServer_CheckBox.Checked = true;
            this.usesDigitalSendServer_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.usesDigitalSendServer_CheckBox.Location = new System.Drawing.Point(21, 27);
            this.usesDigitalSendServer_CheckBox.Name = "usesDigitalSendServer_CheckBox";
            this.usesDigitalSendServer_CheckBox.Size = new System.Drawing.Size(245, 19);
            this.usesDigitalSendServer_CheckBox.TabIndex = 2;
            this.usesDigitalSendServer_CheckBox.Text = "Activity will use Digital Send Service (DSS)";
            this.usesDigitalSendServer_CheckBox.UseVisualStyleBackColor = true;
            this.usesDigitalSendServer_CheckBox.CheckedChanged += new System.EventHandler(this.usesDigitalSendServer_CheckBox_CheckedChanged);
            // 
            // digitalSendServer_TextBox
            // 
            this.digitalSendServer_TextBox.Location = new System.Drawing.Point(98, 62);
            this.digitalSendServer_TextBox.Name = "digitalSendServer_TextBox";
            this.digitalSendServer_TextBox.Size = new System.Drawing.Size(202, 23);
            this.digitalSendServer_TextBox.TabIndex = 1;
            // 
            // digitalSendServerName_Label
            // 
            this.digitalSendServerName_Label.AutoSize = true;
            this.digitalSendServerName_Label.Location = new System.Drawing.Point(18, 65);
            this.digitalSendServerName_Label.Name = "digitalSendServerName_Label";
            this.digitalSendServerName_Label.Size = new System.Drawing.Size(74, 15);
            this.digitalSendServerName_Label.TabIndex = 0;
            this.digitalSendServerName_Label.Text = "Server Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.destinations_NumericUpDown);
            this.groupBox1.Controls.Add(this.multipleFoldersExact_RadioButton);
            this.groupBox1.Controls.Add(this.multipleFoldersAuto_RadioButton);
            this.groupBox1.Controls.Add(this.singleFolder_RadioButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(502, 147);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Destination Type";
            // 
            // destinations_NumericUpDown
            // 
            this.destinations_NumericUpDown.Enabled = false;
            this.destinations_NumericUpDown.Location = new System.Drawing.Point(323, 101);
            this.destinations_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.destinations_NumericUpDown.Name = "destinations_NumericUpDown";
            this.destinations_NumericUpDown.Size = new System.Drawing.Size(76, 23);
            this.destinations_NumericUpDown.TabIndex = 2;
            this.destinations_NumericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // multipleFoldersExact_RadioButton
            // 
            this.multipleFoldersExact_RadioButton.AutoSize = true;
            this.multipleFoldersExact_RadioButton.Location = new System.Drawing.Point(21, 101);
            this.multipleFoldersExact_RadioButton.Name = "multipleFoldersExact_RadioButton";
            this.multipleFoldersExact_RadioButton.Size = new System.Drawing.Size(296, 19);
            this.multipleFoldersExact_RadioButton.TabIndex = 1;
            this.multipleFoldersExact_RadioButton.Text = "Multiple folder destinations - Record exact number:";
            this.multipleFoldersExact_RadioButton.UseVisualStyleBackColor = true;
            this.multipleFoldersExact_RadioButton.CheckedChanged += new System.EventHandler(this.multipleFoldersExact_RadioButton_CheckedChanged);
            // 
            // multipleFoldersAuto_RadioButton
            // 
            this.multipleFoldersAuto_RadioButton.AutoSize = true;
            this.multipleFoldersAuto_RadioButton.Location = new System.Drawing.Point(21, 64);
            this.multipleFoldersAuto_RadioButton.Name = "multipleFoldersAuto_RadioButton";
            this.multipleFoldersAuto_RadioButton.Size = new System.Drawing.Size(438, 19);
            this.multipleFoldersAuto_RadioButton.TabIndex = 1;
            this.multipleFoldersAuto_RadioButton.Text = "Multiple folder destinations - Automatically detect number from control panel";
            this.multipleFoldersAuto_RadioButton.UseVisualStyleBackColor = true;
            // 
            // singleFolder_RadioButton
            // 
            this.singleFolder_RadioButton.AutoSize = true;
            this.singleFolder_RadioButton.Checked = true;
            this.singleFolder_RadioButton.Location = new System.Drawing.Point(21, 27);
            this.singleFolder_RadioButton.Name = "singleFolder_RadioButton";
            this.singleFolder_RadioButton.Size = new System.Drawing.Size(153, 19);
            this.singleFolder_RadioButton.TabIndex = 0;
            this.singleFolder_RadioButton.TabStop = true;
            this.singleFolder_RadioButton.Text = "Single folder destination";
            this.singleFolder_RadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(649, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "The options selected here will modify the data that is logged by this activity, b" +
    "ut will not affect the parameters of the scan.";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 76);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(296, 53);
            this.lockTimeoutControl.TabIndex = 97;
            // 
            // ScanToFolderConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToFolderConfigControl";
            this.Size = new System.Drawing.Size(790, 650);
            this.panel1.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.scanoptions_groupBox.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ExecutionSettingGroup.ResumeLayout(false);
            this.ExecutionSettingGroup.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.destinations_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox quickSet_TextBox;
        private System.Windows.Forms.RadioButton quickSet_RadioButton;
        private System.Windows.Forms.RadioButton networkFolder_RadioButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private System.Windows.Forms.TabPage loggingOptions_TabPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton multipleFoldersAuto_RadioButton;
        private System.Windows.Forms.RadioButton singleFolder_RadioButton;
        private System.Windows.Forms.RadioButton multipleFoldersExact_RadioButton;
        private System.Windows.Forms.NumericUpDown destinations_NumericUpDown;
        private System.Windows.Forms.TextBox folder_TextBox;
        private System.Windows.Forms.CheckBox useOcr_CheckBox;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox digitalSendServer_TextBox;
        private System.Windows.Forms.Label digitalSendServerName_Label;
        private System.Windows.Forms.CheckBox usesDigitalSendServer_CheckBox;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox ExecutionSettingGroup;
        private System.Windows.Forms.RadioButton RestrictRadioButton;
        private System.Windows.Forms.RadioButton GenerateRadioButton;
        private System.Windows.Forms.RadioButton OptionalRadioButton;
        private System.Windows.Forms.CheckBox applyCredential_Checkbox;
        private System.Windows.Forms.GroupBox scanoptions_groupBox;
        private System.Windows.Forms.Button ScanOptions_Button;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_ScanToFolder;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
