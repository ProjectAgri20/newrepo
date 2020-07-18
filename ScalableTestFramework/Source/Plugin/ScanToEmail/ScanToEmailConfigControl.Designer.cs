namespace HP.ScalableTest.Plugin.ScanToEmail
{
    partial class ScanToEmailConfigControl
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
            this.radioButton_ScanToEmail = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.scanoptions_groupBox = new System.Windows.Forms.GroupBox();
            this.ScanOptions_Button = new System.Windows.Forms.Button();
            this.ExecutionSettingGroup = new System.Windows.Forms.GroupBox();
            this.RestrictRadioButton = new System.Windows.Forms.RadioButton();
            this.GenerateRadioButton = new System.Windows.Forms.RadioButton();
            this.OptionalRadioButton = new System.Windows.Forms.RadioButton();
            this.useOcr_CheckBox = new System.Windows.Forms.CheckBox();
            this.quickSet_GroupBox = new System.Windows.Forms.GroupBox();
            this.quickset_Label = new System.Windows.Forms.Label();
            this.quickSet_TextBox = new System.Windows.Forms.TextBox();
            this.launchFromApp_RadioButton = new System.Windows.Forms.RadioButton();
            this.launchFromHome_RadioButton = new System.Windows.Forms.RadioButton();
            this.scanDestination_GroupBox = new System.Windows.Forms.GroupBox();
            this.quickSet_RadioButton = new System.Windows.Forms.RadioButton();
            this.emailApp_RadioButton = new System.Windows.Forms.RadioButton();
            this.email_GroupBox = new System.Windows.Forms.GroupBox();
            this.emailDestination_Label = new System.Windows.Forms.Label();
            this.addressSource_comboBox = new System.Windows.Forms.ComboBox();
            this.addressSource_label = new System.Windows.Forms.Label();
            this.email_ComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pageCount_GroupBox = new System.Windows.Forms.GroupBox();
            this.pageCount_Label = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pageCountDescrption_Label = new System.Windows.Forms.Label();
            this.skipDelay_GroupBox = new System.Windows.Forms.GroupBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.loggingOptions_TabPage = new System.Windows.Forms.TabPage();
            this.digitalSendService_GroupBox = new System.Windows.Forms.GroupBox();
            this.usesDigitalSendServer_CheckBox = new System.Windows.Forms.CheckBox();
            this.digitalSendServer_TextBox = new System.Windows.Forms.TextBox();
            this.digitalSendServerName_Label = new System.Windows.Forms.Label();
            this.logging_Label = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.panel1.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.scanoptions_groupBox.SuspendLayout();
            this.ExecutionSettingGroup.SuspendLayout();
            this.quickSet_GroupBox.SuspendLayout();
            this.scanDestination_GroupBox.SuspendLayout();
            this.email_GroupBox.SuspendLayout();
            this.pageCount_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.skipDelay_GroupBox.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.loggingOptions_TabPage.SuspendLayout();
            this.digitalSendService_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_Authentication);
            this.panel1.Controls.Add(this.scanoptions_groupBox);
            this.panel1.Controls.Add(this.ExecutionSettingGroup);
            this.panel1.Controls.Add(this.useOcr_CheckBox);
            this.panel1.Controls.Add(this.quickSet_GroupBox);
            this.panel1.Controls.Add(this.scanDestination_GroupBox);
            this.panel1.Controls.Add(this.email_GroupBox);
            this.panel1.Controls.Add(this.pageCount_GroupBox);
            this.panel1.Controls.Add(this.skipDelay_GroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(817, 312);
            this.panel1.TabIndex = 49;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ScanToEmail);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(10, 159);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(654, 49);
            this.groupBox_Authentication.TabIndex = 95;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_ScanToEmail
            // 
            this.radioButton_ScanToEmail.AutoSize = true;
            this.radioButton_ScanToEmail.Checked = true;
            this.radioButton_ScanToEmail.Location = new System.Drawing.Point(116, 20);
            this.radioButton_ScanToEmail.Name = "radioButton_ScanToEmail";
            this.radioButton_ScanToEmail.Size = new System.Drawing.Size(92, 19);
            this.radioButton_ScanToEmail.TabIndex = 93;
            this.radioButton_ScanToEmail.TabStop = true;
            this.radioButton_ScanToEmail.Text = "ScanToEmail";
            this.radioButton_ScanToEmail.UseVisualStyleBackColor = true;
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
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(404, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(264, 22);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // scanoptions_groupBox
            // 
            this.scanoptions_groupBox.Controls.Add(this.ScanOptions_Button);
            this.scanoptions_groupBox.Location = new System.Drawing.Point(600, 0);
            this.scanoptions_groupBox.Name = "scanoptions_groupBox";
            this.scanoptions_groupBox.Size = new System.Drawing.Size(151, 59);
            this.scanoptions_groupBox.TabIndex = 56;
            this.scanoptions_groupBox.TabStop = false;
            this.scanoptions_groupBox.Text = "Scan Options";
            // 
            // ScanOptions_Button
            // 
            this.ScanOptions_Button.Location = new System.Drawing.Point(15, 19);
            this.ScanOptions_Button.Name = "ScanOptions_Button";
            this.ScanOptions_Button.Size = new System.Drawing.Size(114, 34);
            this.ScanOptions_Button.TabIndex = 0;
            this.ScanOptions_Button.Text = "Scan Options";
            this.ScanOptions_Button.UseVisualStyleBackColor = true;
            this.ScanOptions_Button.Click += new System.EventHandler(this.ScanOptions_Button_Click);
            // 
            // ExecutionSettingGroup
            // 
            this.ExecutionSettingGroup.Controls.Add(this.RestrictRadioButton);
            this.ExecutionSettingGroup.Controls.Add(this.GenerateRadioButton);
            this.ExecutionSettingGroup.Controls.Add(this.OptionalRadioButton);
            this.ExecutionSettingGroup.Location = new System.Drawing.Point(220, 4);
            this.ExecutionSettingGroup.Name = "ExecutionSettingGroup";
            this.ExecutionSettingGroup.Size = new System.Drawing.Size(363, 44);
            this.ExecutionSettingGroup.TabIndex = 51;
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
            // useOcr_CheckBox
            // 
            this.useOcr_CheckBox.AutoSize = true;
            this.useOcr_CheckBox.Location = new System.Drawing.Point(17, 134);
            this.useOcr_CheckBox.Name = "useOcr_CheckBox";
            this.useOcr_CheckBox.Size = new System.Drawing.Size(110, 19);
            this.useOcr_CheckBox.TabIndex = 14;
            this.useOcr_CheckBox.Text = "Use OCR for job";
            this.useOcr_CheckBox.UseVisualStyleBackColor = true;
            this.useOcr_CheckBox.CheckedChanged += new System.EventHandler(this.useOcr_CheckBox_CheckedChanged);
            // 
            // quickSet_GroupBox
            // 
            this.quickSet_GroupBox.Controls.Add(this.quickset_Label);
            this.quickSet_GroupBox.Controls.Add(this.quickSet_TextBox);
            this.quickSet_GroupBox.Controls.Add(this.launchFromApp_RadioButton);
            this.quickSet_GroupBox.Controls.Add(this.launchFromHome_RadioButton);
            this.quickSet_GroupBox.Location = new System.Drawing.Point(7, 54);
            this.quickSet_GroupBox.Name = "quickSet_GroupBox";
            this.quickSet_GroupBox.Size = new System.Drawing.Size(647, 77);
            this.quickSet_GroupBox.TabIndex = 11;
            this.quickSet_GroupBox.TabStop = false;
            this.quickSet_GroupBox.Text = "Quickset";
            this.quickSet_GroupBox.Visible = false;
            // 
            // quickset_Label
            // 
            this.quickset_Label.AutoSize = true;
            this.quickset_Label.Location = new System.Drawing.Point(7, 19);
            this.quickset_Label.Name = "quickset_Label";
            this.quickset_Label.Size = new System.Drawing.Size(95, 15);
            this.quickset_Label.TabIndex = 59;
            this.quickset_Label.Text = "Named Quickset";
            // 
            // quickSet_TextBox
            // 
            this.quickSet_TextBox.Location = new System.Drawing.Point(122, 16);
            this.quickSet_TextBox.Name = "quickSet_TextBox";
            this.quickSet_TextBox.Size = new System.Drawing.Size(355, 23);
            this.quickSet_TextBox.TabIndex = 58;
            // 
            // launchFromApp_RadioButton
            // 
            this.launchFromApp_RadioButton.AutoSize = true;
            this.launchFromApp_RadioButton.Checked = true;
            this.launchFromApp_RadioButton.Location = new System.Drawing.Point(10, 45);
            this.launchFromApp_RadioButton.Name = "launchFromApp_RadioButton";
            this.launchFromApp_RadioButton.Size = new System.Drawing.Size(152, 19);
            this.launchFromApp_RadioButton.TabIndex = 57;
            this.launchFromApp_RadioButton.TabStop = true;
            this.launchFromApp_RadioButton.Text = "Launch From Email App";
            this.launchFromApp_RadioButton.UseVisualStyleBackColor = true;
            // 
            // launchFromHome_RadioButton
            // 
            this.launchFromHome_RadioButton.AutoSize = true;
            this.launchFromHome_RadioButton.Location = new System.Drawing.Point(186, 45);
            this.launchFromHome_RadioButton.Name = "launchFromHome_RadioButton";
            this.launchFromHome_RadioButton.Size = new System.Drawing.Size(129, 19);
            this.launchFromHome_RadioButton.TabIndex = 56;
            this.launchFromHome_RadioButton.Text = "Launch from Home";
            this.launchFromHome_RadioButton.UseVisualStyleBackColor = true;
            // 
            // scanDestination_GroupBox
            // 
            this.scanDestination_GroupBox.Controls.Add(this.quickSet_RadioButton);
            this.scanDestination_GroupBox.Controls.Add(this.emailApp_RadioButton);
            this.scanDestination_GroupBox.Location = new System.Drawing.Point(7, 3);
            this.scanDestination_GroupBox.Name = "scanDestination_GroupBox";
            this.scanDestination_GroupBox.Size = new System.Drawing.Size(193, 45);
            this.scanDestination_GroupBox.TabIndex = 50;
            this.scanDestination_GroupBox.TabStop = false;
            this.scanDestination_GroupBox.Text = "Scan Destination";
            // 
            // quickSet_RadioButton
            // 
            this.quickSet_RadioButton.AutoSize = true;
            this.quickSet_RadioButton.Location = new System.Drawing.Point(111, 20);
            this.quickSet_RadioButton.Name = "quickSet_RadioButton";
            this.quickSet_RadioButton.Size = new System.Drawing.Size(71, 19);
            this.quickSet_RadioButton.TabIndex = 1;
            this.quickSet_RadioButton.Text = "Quickset";
            this.quickSet_RadioButton.UseVisualStyleBackColor = true;
            this.quickSet_RadioButton.CheckedChanged += new System.EventHandler(this.emailApp_RadioButton_CheckedChanged);
            // 
            // emailApp_RadioButton
            // 
            this.emailApp_RadioButton.AutoSize = true;
            this.emailApp_RadioButton.Checked = true;
            this.emailApp_RadioButton.Location = new System.Drawing.Point(15, 20);
            this.emailApp_RadioButton.Name = "emailApp_RadioButton";
            this.emailApp_RadioButton.Size = new System.Drawing.Size(82, 19);
            this.emailApp_RadioButton.TabIndex = 0;
            this.emailApp_RadioButton.TabStop = true;
            this.emailApp_RadioButton.Text = "Email  APP";
            this.emailApp_RadioButton.UseVisualStyleBackColor = true;
            this.emailApp_RadioButton.CheckedChanged += new System.EventHandler(this.emailApp_RadioButton_CheckedChanged);
            // 
            // email_GroupBox
            // 
            this.email_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.email_GroupBox.Controls.Add(this.emailDestination_Label);
            this.email_GroupBox.Controls.Add(this.addressSource_comboBox);
            this.email_GroupBox.Controls.Add(this.addressSource_label);
            this.email_GroupBox.Controls.Add(this.email_ComboBox);
            this.email_GroupBox.Controls.Add(this.label1);
            this.email_GroupBox.Location = new System.Drawing.Point(7, 59);
            this.email_GroupBox.Name = "email_GroupBox";
            this.email_GroupBox.Size = new System.Drawing.Size(651, 69);
            this.email_GroupBox.TabIndex = 11;
            this.email_GroupBox.TabStop = false;
            this.email_GroupBox.Text = "Email Settings";
            // 
            // emailDestination_Label
            // 
            this.emailDestination_Label.AutoSize = true;
            this.emailDestination_Label.Location = new System.Drawing.Point(6, 19);
            this.emailDestination_Label.Name = "emailDestination_Label";
            this.emailDestination_Label.Size = new System.Drawing.Size(99, 15);
            this.emailDestination_Label.TabIndex = 6;
            this.emailDestination_Label.Text = "Email Destination";
            // 
            // addressSource_comboBox
            // 
            this.addressSource_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.addressSource_comboBox.FormattingEnabled = true;
            this.addressSource_comboBox.Items.AddRange(new object[] {
            "Default",
            "Email Addressbook",
            "LDAP Addressbook"});
            this.addressSource_comboBox.Location = new System.Drawing.Point(477, 19);
            this.addressSource_comboBox.Name = "addressSource_comboBox";
            this.addressSource_comboBox.Size = new System.Drawing.Size(168, 23);
            this.addressSource_comboBox.TabIndex = 9;
            // 
            // addressSource_label
            // 
            this.addressSource_label.AutoSize = true;
            this.addressSource_label.Location = new System.Drawing.Point(389, 22);
            this.addressSource_label.Name = "addressSource_label";
            this.addressSource_label.Size = new System.Drawing.Size(88, 15);
            this.addressSource_label.TabIndex = 10;
            this.addressSource_label.Text = "Address Source";
            // 
            // email_ComboBox
            // 
            this.email_ComboBox.FormattingEnabled = true;
            this.email_ComboBox.Location = new System.Drawing.Point(111, 19);
            this.email_ComboBox.Name = "email_ComboBox";
            this.email_ComboBox.Size = new System.Drawing.Size(192, 23);
            this.email_ComboBox.Sorted = true;
            this.email_ComboBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "@domain.test";
            // 
            // pageCount_GroupBox
            // 
            this.pageCount_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_GroupBox.Controls.Add(this.pageCount_Label);
            this.pageCount_GroupBox.Controls.Add(this.pageCount_NumericUpDown);
            this.pageCount_GroupBox.Controls.Add(this.pageCountDescrption_Label);
            this.pageCount_GroupBox.Location = new System.Drawing.Point(0, 206);
            this.pageCount_GroupBox.Name = "pageCount_GroupBox";
            this.pageCount_GroupBox.Size = new System.Drawing.Size(296, 100);
            this.pageCount_GroupBox.TabIndex = 4;
            this.pageCount_GroupBox.TabStop = false;
            this.pageCount_GroupBox.Text = "Page Count";
            // 
            // pageCount_Label
            // 
            this.pageCount_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_Label.AutoSize = true;
            this.pageCount_Label.Location = new System.Drawing.Point(37, 71);
            this.pageCount_Label.Name = "pageCount_Label";
            this.pageCount_Label.Size = new System.Drawing.Size(69, 15);
            this.pageCount_Label.TabIndex = 13;
            this.pageCount_Label.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(118, 69);
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
            // pageCountDescrption_Label
            // 
            this.pageCountDescrption_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageCountDescrption_Label.Location = new System.Drawing.Point(7, 18);
            this.pageCountDescrption_Label.Name = "pageCountDescrption_Label";
            this.pageCountDescrption_Label.Size = new System.Drawing.Size(282, 57);
            this.pageCountDescrption_Label.TabIndex = 11;
            this.pageCountDescrption_Label.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // skipDelay_GroupBox
            // 
            this.skipDelay_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipDelay_GroupBox.Controls.Add(this.lockTimeoutControl);
            this.skipDelay_GroupBox.Location = new System.Drawing.Point(302, 206);
            this.skipDelay_GroupBox.Name = "skipDelay_GroupBox";
            this.skipDelay_GroupBox.Size = new System.Drawing.Size(362, 100);
            this.skipDelay_GroupBox.TabIndex = 2;
            this.skipDelay_GroupBox.TabStop = false;
            this.skipDelay_GroupBox.Text = "Skip Delay";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Controls.Add(this.loggingOptions_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(831, 500);
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
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(823, 472);
            this.scanConfiguration_TabPage.TabIndex = 0;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 315);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(817, 154);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // loggingOptions_TabPage
            // 
            this.loggingOptions_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.loggingOptions_TabPage.Controls.Add(this.digitalSendService_GroupBox);
            this.loggingOptions_TabPage.Controls.Add(this.logging_Label);
            this.loggingOptions_TabPage.Location = new System.Drawing.Point(4, 24);
            this.loggingOptions_TabPage.Name = "loggingOptions_TabPage";
            this.loggingOptions_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingOptions_TabPage.Size = new System.Drawing.Size(823, 472);
            this.loggingOptions_TabPage.TabIndex = 1;
            this.loggingOptions_TabPage.Text = "Logging Options";
            // 
            // digitalSendService_GroupBox
            // 
            this.digitalSendService_GroupBox.Controls.Add(this.usesDigitalSendServer_CheckBox);
            this.digitalSendService_GroupBox.Controls.Add(this.digitalSendServer_TextBox);
            this.digitalSendService_GroupBox.Controls.Add(this.digitalSendServerName_Label);
            this.digitalSendService_GroupBox.Location = new System.Drawing.Point(9, 43);
            this.digitalSendService_GroupBox.Name = "digitalSendService_GroupBox";
            this.digitalSendService_GroupBox.Size = new System.Drawing.Size(317, 104);
            this.digitalSendService_GroupBox.TabIndex = 12;
            this.digitalSendService_GroupBox.TabStop = false;
            this.digitalSendService_GroupBox.Text = "Digital Send Service";
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
            // logging_Label
            // 
            this.logging_Label.AutoSize = true;
            this.logging_Label.Location = new System.Drawing.Point(6, 13);
            this.logging_Label.Name = "logging_Label";
            this.logging_Label.Size = new System.Drawing.Size(649, 15);
            this.logging_Label.TabIndex = 1;
            this.logging_Label.Text = "The options selected here will modify the data that is logged by this activity, b" +
    "ut will not affect the parameters of the scan.";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 22);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 0;
            // 
            // ScanToEmailConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToEmailConfigControl";
            this.Size = new System.Drawing.Size(831, 500);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.scanoptions_groupBox.ResumeLayout(false);
            this.ExecutionSettingGroup.ResumeLayout(false);
            this.ExecutionSettingGroup.PerformLayout();
            this.quickSet_GroupBox.ResumeLayout(false);
            this.quickSet_GroupBox.PerformLayout();
            this.scanDestination_GroupBox.ResumeLayout(false);
            this.scanDestination_GroupBox.PerformLayout();
            this.email_GroupBox.ResumeLayout(false);
            this.email_GroupBox.PerformLayout();
            this.pageCount_GroupBox.ResumeLayout(false);
            this.pageCount_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.skipDelay_GroupBox.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.PerformLayout();
            this.digitalSendService_GroupBox.ResumeLayout(false);
            this.digitalSendService_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox skipDelay_GroupBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox pageCount_GroupBox;
        private System.Windows.Forms.Label pageCountDescrption_Label;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label pageCount_Label;
        private System.Windows.Forms.Label emailDestination_Label;
        private System.Windows.Forms.ComboBox email_ComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private System.Windows.Forms.TabPage loggingOptions_TabPage;
        private System.Windows.Forms.Label logging_Label;
        private System.Windows.Forms.GroupBox digitalSendService_GroupBox;
        private System.Windows.Forms.CheckBox usesDigitalSendServer_CheckBox;
        private System.Windows.Forms.TextBox digitalSendServer_TextBox;
        private System.Windows.Forms.Label digitalSendServerName_Label;
        private System.Windows.Forms.Label addressSource_label;
        private System.Windows.Forms.ComboBox addressSource_comboBox;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox email_GroupBox;
        private System.Windows.Forms.CheckBox useOcr_CheckBox;
        private System.Windows.Forms.GroupBox quickSet_GroupBox;
        private System.Windows.Forms.Label quickset_Label;
        private System.Windows.Forms.TextBox quickSet_TextBox;
        private System.Windows.Forms.RadioButton launchFromApp_RadioButton;
        private System.Windows.Forms.RadioButton launchFromHome_RadioButton;
        private System.Windows.Forms.GroupBox scanDestination_GroupBox;
        private System.Windows.Forms.RadioButton quickSet_RadioButton;
        private System.Windows.Forms.RadioButton emailApp_RadioButton;
        private System.Windows.Forms.GroupBox ExecutionSettingGroup;
        private System.Windows.Forms.RadioButton RestrictRadioButton;
        private System.Windows.Forms.RadioButton GenerateRadioButton;
        private System.Windows.Forms.RadioButton OptionalRadioButton;
        private System.Windows.Forms.GroupBox scanoptions_groupBox;
        private System.Windows.Forms.Button ScanOptions_Button;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_ScanToEmail;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
