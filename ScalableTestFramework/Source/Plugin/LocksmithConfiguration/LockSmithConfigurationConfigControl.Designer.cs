namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    partial class LockSmithConfigurationConfigControl
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
            this.locksmithConfiguration_Panel = new System.Windows.Forms.Panel();
            this.globalCredentials_GroupBox = new System.Windows.Forms.GroupBox();
            this.ewsAdminPassword_TextBox = new System.Windows.Forms.TextBox();
            this.ewsAdminPassword_Label = new System.Windows.Forms.Label();
            this.tasekType_GroupBox = new System.Windows.Forms.GroupBox();
            this.assessAndRemediate_RadioButton = new System.Windows.Forms.RadioButton();
            this.assessOnly_RadioButton = new System.Windows.Forms.RadioButton();
            this.groupConfiguration_GroupBox = new System.Windows.Forms.GroupBox();
            this.groupName_TextBox = new System.Windows.Forms.TextBox();
            this.groupName_Label = new System.Windows.Forms.Label();
            this.browser_GroupBox = new System.Windows.Forms.GroupBox();
            this.browserType_ComboBox = new System.Windows.Forms.ComboBox();
            this.browser_Label = new System.Windows.Forms.Label();
            this.activity_GroupBox = new System.Windows.Forms.GroupBox();
            this.generateReports_CheckBox = new System.Windows.Forms.CheckBox();
            this.importAndApplyPolicy_CheckBox = new System.Windows.Forms.CheckBox();
            this.printerDiscovery_CheckBox = new System.Windows.Forms.CheckBox();
            this.serverDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.locksmithServer_Label = new System.Windows.Forms.Label();
            this.lockSmith_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.adminUser_Label = new System.Windows.Forms.Label();
            this.user_TextBox = new System.Windows.Forms.TextBox();
            this.adminPassword_Label = new System.Windows.Forms.Label();
            this.password_TextBox = new System.Windows.Forms.TextBox();
            this.deviceDiscovery_GroupBox = new System.Windows.Forms.GroupBox();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.addFromFile_GroupBox = new System.Windows.Forms.GroupBox();
            this.deviceListPath_Label = new System.Windows.Forms.Label();
            this.ipAddressFileBrowse_button = new System.Windows.Forms.Button();
            this.ipAddressFile_textBox = new System.Windows.Forms.TextBox();
            this.manualDiscovery_GroupBox = new System.Windows.Forms.GroupBox();
            this.ipAddress_Label = new System.Windows.Forms.Label();
            this.manualIPAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.automaticDiscovery_GroupBox = new System.Windows.Forms.GroupBox();
            this.automaticEndIPAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.automaticStartIPAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.endIPAddress_Label = new System.Windows.Forms.Label();
            this.startIPAddress_Label = new System.Windows.Forms.Label();
            this.networkHop_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ipRange_RadioButton = new System.Windows.Forms.RadioButton();
            this.numberOfNetworkHops_RadioButton = new System.Windows.Forms.RadioButton();
            this.discoveryType_Label = new System.Windows.Forms.Label();
            this.discoveryType_ComboBox = new System.Windows.Forms.ComboBox();
            this.policyConfiguration_GroupBox = new System.Windows.Forms.GroupBox();
            this.policyPath_TextBox = new System.Windows.Forms.TextBox();
            this.validatePolicyPath_CheckBox = new System.Windows.Forms.CheckBox();
            this.existingPolicyName_CheckBox = new System.Windows.Forms.CheckBox();
            this.existingPolicyName_TextBox = new System.Windows.Forms.TextBox();
            this.PolicyPath_Label = new System.Windows.Forms.Label();
            this.policyPassword_TextBox = new System.Windows.Forms.TextBox();
            this.policyPassword_Label = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lockSmith_FieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.locksmithConfiguration_Panel.SuspendLayout();
            this.globalCredentials_GroupBox.SuspendLayout();
            this.tasekType_GroupBox.SuspendLayout();
            this.groupConfiguration_GroupBox.SuspendLayout();
            this.browser_GroupBox.SuspendLayout();
            this.activity_GroupBox.SuspendLayout();
            this.serverDetails_GroupBox.SuspendLayout();
            this.deviceDiscovery_GroupBox.SuspendLayout();
            this.addFromFile_GroupBox.SuspendLayout();
            this.manualDiscovery_GroupBox.SuspendLayout();
            this.automaticDiscovery_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.networkHop_NumericUpDown)).BeginInit();
            this.policyConfiguration_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // locksmithConfiguration_Panel
            // 
            this.locksmithConfiguration_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.locksmithConfiguration_Panel.AutoScroll = true;
            this.locksmithConfiguration_Panel.Controls.Add(this.globalCredentials_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.tasekType_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.groupConfiguration_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.browser_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.activity_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.serverDetails_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.deviceDiscovery_GroupBox);
            this.locksmithConfiguration_Panel.Controls.Add(this.policyConfiguration_GroupBox);
            this.locksmithConfiguration_Panel.Location = new System.Drawing.Point(3, 3);
            this.locksmithConfiguration_Panel.Name = "locksmithConfiguration_Panel";
            this.locksmithConfiguration_Panel.Size = new System.Drawing.Size(818, 737);
            this.locksmithConfiguration_Panel.TabIndex = 65;
            // 
            // globalCredentials_GroupBox
            // 
            this.globalCredentials_GroupBox.Controls.Add(this.ewsAdminPassword_TextBox);
            this.globalCredentials_GroupBox.Controls.Add(this.ewsAdminPassword_Label);
            this.globalCredentials_GroupBox.Location = new System.Drawing.Point(287, 12);
            this.globalCredentials_GroupBox.Name = "globalCredentials_GroupBox";
            this.globalCredentials_GroupBox.Size = new System.Drawing.Size(248, 71);
            this.globalCredentials_GroupBox.TabIndex = 65;
            this.globalCredentials_GroupBox.TabStop = false;
            this.globalCredentials_GroupBox.Text = "Global Credentials";
            // 
            // ewsAdminPassword_TextBox
            // 
            this.ewsAdminPassword_TextBox.Location = new System.Drawing.Point(128, 30);
            this.ewsAdminPassword_TextBox.Name = "ewsAdminPassword_TextBox";
            this.ewsAdminPassword_TextBox.PasswordChar = '*';
            this.ewsAdminPassword_TextBox.Size = new System.Drawing.Size(106, 20);
            this.ewsAdminPassword_TextBox.TabIndex = 2;
            // 
            // ewsAdminPassword_Label
            // 
            this.ewsAdminPassword_Label.AutoSize = true;
            this.ewsAdminPassword_Label.Location = new System.Drawing.Point(9, 33);
            this.ewsAdminPassword_Label.Name = "ewsAdminPassword_Label";
            this.ewsAdminPassword_Label.Size = new System.Drawing.Size(113, 13);
            this.ewsAdminPassword_Label.TabIndex = 55;
            this.ewsAdminPassword_Label.Text = "EWS Admin Password";
            // 
            // tasekType_GroupBox
            // 
            this.tasekType_GroupBox.Controls.Add(this.assessAndRemediate_RadioButton);
            this.tasekType_GroupBox.Controls.Add(this.assessOnly_RadioButton);
            this.tasekType_GroupBox.Enabled = false;
            this.tasekType_GroupBox.Location = new System.Drawing.Point(346, 89);
            this.tasekType_GroupBox.Name = "tasekType_GroupBox";
            this.tasekType_GroupBox.Size = new System.Drawing.Size(189, 129);
            this.tasekType_GroupBox.TabIndex = 65;
            this.tasekType_GroupBox.TabStop = false;
            this.tasekType_GroupBox.Text = "Task Type";
            // 
            // assessAndRemediate_RadioButton
            // 
            this.assessAndRemediate_RadioButton.AutoSize = true;
            this.assessAndRemediate_RadioButton.Location = new System.Drawing.Point(35, 74);
            this.assessAndRemediate_RadioButton.Name = "assessAndRemediate_RadioButton";
            this.assessAndRemediate_RadioButton.Size = new System.Drawing.Size(133, 17);
            this.assessAndRemediate_RadioButton.TabIndex = 10;
            this.assessAndRemediate_RadioButton.Text = "Assess and Remediate";
            this.assessAndRemediate_RadioButton.UseVisualStyleBackColor = true;
            // 
            // assessOnly_RadioButton
            // 
            this.assessOnly_RadioButton.AutoSize = true;
            this.assessOnly_RadioButton.Checked = true;
            this.assessOnly_RadioButton.Location = new System.Drawing.Point(35, 51);
            this.assessOnly_RadioButton.Name = "assessOnly_RadioButton";
            this.assessOnly_RadioButton.Size = new System.Drawing.Size(82, 17);
            this.assessOnly_RadioButton.TabIndex = 9;
            this.assessOnly_RadioButton.TabStop = true;
            this.assessOnly_RadioButton.Text = "Assess Only";
            this.assessOnly_RadioButton.UseVisualStyleBackColor = true;
            // 
            // groupConfiguration_GroupBox
            // 
            this.groupConfiguration_GroupBox.Controls.Add(this.groupName_TextBox);
            this.groupConfiguration_GroupBox.Controls.Add(this.groupName_Label);
            this.groupConfiguration_GroupBox.Location = new System.Drawing.Point(541, 12);
            this.groupConfiguration_GroupBox.Name = "groupConfiguration_GroupBox";
            this.groupConfiguration_GroupBox.Size = new System.Drawing.Size(270, 71);
            this.groupConfiguration_GroupBox.TabIndex = 64;
            this.groupConfiguration_GroupBox.TabStop = false;
            this.groupConfiguration_GroupBox.Text = "Group Configuration";
            // 
            // groupName_TextBox
            // 
            this.groupName_TextBox.Location = new System.Drawing.Point(109, 30);
            this.groupName_TextBox.Name = "groupName_TextBox";
            this.groupName_TextBox.Size = new System.Drawing.Size(127, 20);
            this.groupName_TextBox.TabIndex = 3;
            // 
            // groupName_Label
            // 
            this.groupName_Label.AutoSize = true;
            this.groupName_Label.Location = new System.Drawing.Point(36, 33);
            this.groupName_Label.Name = "groupName_Label";
            this.groupName_Label.Size = new System.Drawing.Size(67, 13);
            this.groupName_Label.TabIndex = 55;
            this.groupName_Label.Text = "Group Name";
            // 
            // browser_GroupBox
            // 
            this.browser_GroupBox.Controls.Add(this.browserType_ComboBox);
            this.browser_GroupBox.Controls.Add(this.browser_Label);
            this.browser_GroupBox.Location = new System.Drawing.Point(6, 12);
            this.browser_GroupBox.Name = "browser_GroupBox";
            this.browser_GroupBox.Size = new System.Drawing.Size(275, 71);
            this.browser_GroupBox.TabIndex = 64;
            this.browser_GroupBox.TabStop = false;
            this.browser_GroupBox.Text = "Browser";
            // 
            // browserType_ComboBox
            // 
            this.browserType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.browserType_ComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.browserType_ComboBox.FormattingEnabled = true;
            this.browserType_ComboBox.Location = new System.Drawing.Point(90, 27);
            this.browserType_ComboBox.Name = "browserType_ComboBox";
            this.browserType_ComboBox.Size = new System.Drawing.Size(155, 21);
            this.browserType_ComboBox.TabIndex = 1;
            // 
            // browser_Label
            // 
            this.browser_Label.AutoSize = true;
            this.browser_Label.Location = new System.Drawing.Point(6, 27);
            this.browser_Label.Name = "browser_Label";
            this.browser_Label.Size = new System.Drawing.Size(78, 13);
            this.browser_Label.TabIndex = 0;
            this.browser_Label.Text = "Select Browser";
            // 
            // activity_GroupBox
            // 
            this.activity_GroupBox.Controls.Add(this.generateReports_CheckBox);
            this.activity_GroupBox.Controls.Add(this.importAndApplyPolicy_CheckBox);
            this.activity_GroupBox.Controls.Add(this.printerDiscovery_CheckBox);
            this.activity_GroupBox.Location = new System.Drawing.Point(541, 89);
            this.activity_GroupBox.Name = "activity_GroupBox";
            this.activity_GroupBox.Size = new System.Drawing.Size(270, 129);
            this.activity_GroupBox.TabIndex = 63;
            this.activity_GroupBox.TabStop = false;
            this.activity_GroupBox.Text = "Activity";
            // 
            // generateReports_CheckBox
            // 
            this.generateReports_CheckBox.AutoSize = true;
            this.generateReports_CheckBox.Location = new System.Drawing.Point(39, 98);
            this.generateReports_CheckBox.Name = "generateReports_CheckBox";
            this.generateReports_CheckBox.Size = new System.Drawing.Size(110, 17);
            this.generateReports_CheckBox.TabIndex = 4;
            this.generateReports_CheckBox.Text = "Generate Reports";
            this.generateReports_CheckBox.UseVisualStyleBackColor = true;
            // 
            // importAndApplyPolicy_CheckBox
            // 
            this.importAndApplyPolicy_CheckBox.AutoSize = true;
            this.importAndApplyPolicy_CheckBox.Location = new System.Drawing.Point(39, 68);
            this.importAndApplyPolicy_CheckBox.Name = "importAndApplyPolicy_CheckBox";
            this.importAndApplyPolicy_CheckBox.Size = new System.Drawing.Size(136, 17);
            this.importAndApplyPolicy_CheckBox.TabIndex = 3;
            this.importAndApplyPolicy_CheckBox.Text = "Import and Apply Policy";
            this.importAndApplyPolicy_CheckBox.UseVisualStyleBackColor = true;
            this.importAndApplyPolicy_CheckBox.CheckedChanged += new System.EventHandler(this.importAndApplyPolicy_CheckBox_CheckedChanged);
            // 
            // printerDiscovery_CheckBox
            // 
            this.printerDiscovery_CheckBox.AutoSize = true;
            this.printerDiscovery_CheckBox.Location = new System.Drawing.Point(39, 38);
            this.printerDiscovery_CheckBox.Name = "printerDiscovery_CheckBox";
            this.printerDiscovery_CheckBox.Size = new System.Drawing.Size(106, 17);
            this.printerDiscovery_CheckBox.TabIndex = 2;
            this.printerDiscovery_CheckBox.Text = "Printer Discovery";
            this.printerDiscovery_CheckBox.UseVisualStyleBackColor = true;
            this.printerDiscovery_CheckBox.CheckedChanged += new System.EventHandler(this.printerDiscovery_CheckBox_CheckedChanged);
            // 
            // serverDetails_GroupBox
            // 
            this.serverDetails_GroupBox.Controls.Add(this.locksmithServer_Label);
            this.serverDetails_GroupBox.Controls.Add(this.lockSmith_ServerComboBox);
            this.serverDetails_GroupBox.Controls.Add(this.adminUser_Label);
            this.serverDetails_GroupBox.Controls.Add(this.user_TextBox);
            this.serverDetails_GroupBox.Controls.Add(this.adminPassword_Label);
            this.serverDetails_GroupBox.Controls.Add(this.password_TextBox);
            this.serverDetails_GroupBox.Location = new System.Drawing.Point(6, 89);
            this.serverDetails_GroupBox.Name = "serverDetails_GroupBox";
            this.serverDetails_GroupBox.Size = new System.Drawing.Size(334, 129);
            this.serverDetails_GroupBox.TabIndex = 59;
            this.serverDetails_GroupBox.TabStop = false;
            this.serverDetails_GroupBox.Text = "Server Details";
            // 
            // locksmithServer_Label
            // 
            this.locksmithServer_Label.AutoSize = true;
            this.locksmithServer_Label.Location = new System.Drawing.Point(25, 38);
            this.locksmithServer_Label.Name = "locksmithServer_Label";
            this.locksmithServer_Label.Size = new System.Drawing.Size(89, 13);
            this.locksmithServer_Label.TabIndex = 0;
            this.locksmithServer_Label.Text = "Locksmith Server";
            // 
            // lockSmith_ServerComboBox
            // 
            this.lockSmith_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockSmith_ServerComboBox.Location = new System.Drawing.Point(120, 32);
            this.lockSmith_ServerComboBox.Name = "lockSmith_ServerComboBox";
            this.lockSmith_ServerComboBox.Size = new System.Drawing.Size(155, 23);
            this.lockSmith_ServerComboBox.TabIndex = 1;
            // 
            // adminUser_Label
            // 
            this.adminUser_Label.AutoSize = true;
            this.adminUser_Label.Location = new System.Drawing.Point(25, 70);
            this.adminUser_Label.Name = "adminUser_Label";
            this.adminUser_Label.Size = new System.Drawing.Size(58, 13);
            this.adminUser_Label.TabIndex = 50;
            this.adminUser_Label.Text = "User name";
            // 
            // user_TextBox
            // 
            this.user_TextBox.Location = new System.Drawing.Point(120, 67);
            this.user_TextBox.Name = "user_TextBox";
            this.user_TextBox.Size = new System.Drawing.Size(155, 20);
            this.user_TextBox.TabIndex = 52;
            // 
            // adminPassword_Label
            // 
            this.adminPassword_Label.AutoSize = true;
            this.adminPassword_Label.Location = new System.Drawing.Point(25, 99);
            this.adminPassword_Label.Name = "adminPassword_Label";
            this.adminPassword_Label.Size = new System.Drawing.Size(53, 13);
            this.adminPassword_Label.TabIndex = 51;
            this.adminPassword_Label.Text = "Password";
            // 
            // password_TextBox
            // 
            this.password_TextBox.Location = new System.Drawing.Point(120, 98);
            this.password_TextBox.Name = "password_TextBox";
            this.password_TextBox.PasswordChar = '*';
            this.password_TextBox.Size = new System.Drawing.Size(155, 20);
            this.password_TextBox.TabIndex = 53;
            // 
            // deviceDiscovery_GroupBox
            // 
            this.deviceDiscovery_GroupBox.Controls.Add(this.assetSelectionControl);
            this.deviceDiscovery_GroupBox.Controls.Add(this.addFromFile_GroupBox);
            this.deviceDiscovery_GroupBox.Controls.Add(this.manualDiscovery_GroupBox);
            this.deviceDiscovery_GroupBox.Controls.Add(this.automaticDiscovery_GroupBox);
            this.deviceDiscovery_GroupBox.Controls.Add(this.discoveryType_Label);
            this.deviceDiscovery_GroupBox.Controls.Add(this.discoveryType_ComboBox);
            this.deviceDiscovery_GroupBox.Enabled = false;
            this.deviceDiscovery_GroupBox.Location = new System.Drawing.Point(6, 373);
            this.deviceDiscovery_GroupBox.Name = "deviceDiscovery_GroupBox";
            this.deviceDiscovery_GroupBox.Size = new System.Drawing.Size(805, 357);
            this.deviceDiscovery_GroupBox.TabIndex = 62;
            this.deviceDiscovery_GroupBox.TabStop = false;
            this.deviceDiscovery_GroupBox.Text = "Device Discovery";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Enabled = false;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 204);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(799, 147);
            this.assetSelectionControl.TabIndex = 66;
            // 
            // addFromFile_GroupBox
            // 
            this.addFromFile_GroupBox.Controls.Add(this.deviceListPath_Label);
            this.addFromFile_GroupBox.Controls.Add(this.ipAddressFileBrowse_button);
            this.addFromFile_GroupBox.Controls.Add(this.ipAddressFile_textBox);
            this.addFromFile_GroupBox.Enabled = false;
            this.addFromFile_GroupBox.Location = new System.Drawing.Point(445, 19);
            this.addFromFile_GroupBox.Name = "addFromFile_GroupBox";
            this.addFromFile_GroupBox.Size = new System.Drawing.Size(354, 52);
            this.addFromFile_GroupBox.TabIndex = 56;
            this.addFromFile_GroupBox.TabStop = false;
            this.addFromFile_GroupBox.Text = "Add From File";
            // 
            // deviceListPath_Label
            // 
            this.deviceListPath_Label.AutoSize = true;
            this.deviceListPath_Label.Location = new System.Drawing.Point(6, 23);
            this.deviceListPath_Label.Name = "deviceListPath_Label";
            this.deviceListPath_Label.Size = new System.Drawing.Size(85, 13);
            this.deviceListPath_Label.TabIndex = 2;
            this.deviceListPath_Label.Text = "Device List Path";
            // 
            // ipAddressFileBrowse_button
            // 
            this.ipAddressFileBrowse_button.Location = new System.Drawing.Point(273, 19);
            this.ipAddressFileBrowse_button.Name = "ipAddressFileBrowse_button";
            this.ipAddressFileBrowse_button.Size = new System.Drawing.Size(75, 23);
            this.ipAddressFileBrowse_button.TabIndex = 60;
            this.ipAddressFileBrowse_button.Text = "Browse";
            this.ipAddressFileBrowse_button.UseVisualStyleBackColor = true;
            this.ipAddressFileBrowse_button.Click += new System.EventHandler(this.ipAddressFileBrowse_button_Click);
            // 
            // ipAddressFile_textBox
            // 
            this.ipAddressFile_textBox.Location = new System.Drawing.Point(96, 20);
            this.ipAddressFile_textBox.Name = "ipAddressFile_textBox";
            this.ipAddressFile_textBox.ReadOnly = true;
            this.ipAddressFile_textBox.Size = new System.Drawing.Size(171, 20);
            this.ipAddressFile_textBox.TabIndex = 60;
            // 
            // manualDiscovery_GroupBox
            // 
            this.manualDiscovery_GroupBox.Controls.Add(this.ipAddress_Label);
            this.manualDiscovery_GroupBox.Controls.Add(this.manualIPAddress_IpAddressControl);
            this.manualDiscovery_GroupBox.Location = new System.Drawing.Point(524, 78);
            this.manualDiscovery_GroupBox.Name = "manualDiscovery_GroupBox";
            this.manualDiscovery_GroupBox.Size = new System.Drawing.Size(275, 120);
            this.manualDiscovery_GroupBox.TabIndex = 3;
            this.manualDiscovery_GroupBox.TabStop = false;
            this.manualDiscovery_GroupBox.Text = "Manual Discovery";
            // 
            // ipAddress_Label
            // 
            this.ipAddress_Label.AutoSize = true;
            this.ipAddress_Label.Location = new System.Drawing.Point(56, 62);
            this.ipAddress_Label.Name = "ipAddress_Label";
            this.ipAddress_Label.Size = new System.Drawing.Size(58, 13);
            this.ipAddress_Label.TabIndex = 61;
            this.ipAddress_Label.Text = "IP Address";
            // 
            // manualIPAddress_IpAddressControl
            // 
            this.manualIPAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.manualIPAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.manualIPAddress_IpAddressControl.Location = new System.Drawing.Point(120, 59);
            this.manualIPAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.manualIPAddress_IpAddressControl.Name = "manualIPAddress_IpAddressControl";
            this.manualIPAddress_IpAddressControl.Size = new System.Drawing.Size(116, 20);
            this.manualIPAddress_IpAddressControl.TabIndex = 2;
            // 
            // automaticDiscovery_GroupBox
            // 
            this.automaticDiscovery_GroupBox.Controls.Add(this.automaticEndIPAddress_IpAddressControl);
            this.automaticDiscovery_GroupBox.Controls.Add(this.automaticStartIPAddress_IpAddressControl);
            this.automaticDiscovery_GroupBox.Controls.Add(this.endIPAddress_Label);
            this.automaticDiscovery_GroupBox.Controls.Add(this.startIPAddress_Label);
            this.automaticDiscovery_GroupBox.Controls.Add(this.networkHop_NumericUpDown);
            this.automaticDiscovery_GroupBox.Controls.Add(this.ipRange_RadioButton);
            this.automaticDiscovery_GroupBox.Controls.Add(this.numberOfNetworkHops_RadioButton);
            this.automaticDiscovery_GroupBox.Location = new System.Drawing.Point(0, 78);
            this.automaticDiscovery_GroupBox.Name = "automaticDiscovery_GroupBox";
            this.automaticDiscovery_GroupBox.Size = new System.Drawing.Size(518, 120);
            this.automaticDiscovery_GroupBox.TabIndex = 2;
            this.automaticDiscovery_GroupBox.TabStop = false;
            this.automaticDiscovery_GroupBox.Text = "Automatic Discovery";
            // 
            // automaticEndIPAddress_IpAddressControl
            // 
            this.automaticEndIPAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.automaticEndIPAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.automaticEndIPAddress_IpAddressControl.Location = new System.Drawing.Point(303, 85);
            this.automaticEndIPAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.automaticEndIPAddress_IpAddressControl.Name = "automaticEndIPAddress_IpAddressControl";
            this.automaticEndIPAddress_IpAddressControl.Size = new System.Drawing.Size(103, 20);
            this.automaticEndIPAddress_IpAddressControl.TabIndex = 8;
            // 
            // automaticStartIPAddress_IpAddressControl
            // 
            this.automaticStartIPAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.automaticStartIPAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.automaticStartIPAddress_IpAddressControl.Location = new System.Drawing.Point(134, 85);
            this.automaticStartIPAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.automaticStartIPAddress_IpAddressControl.Name = "automaticStartIPAddress_IpAddressControl";
            this.automaticStartIPAddress_IpAddressControl.Size = new System.Drawing.Size(103, 20);
            this.automaticStartIPAddress_IpAddressControl.TabIndex = 7;
            // 
            // endIPAddress_Label
            // 
            this.endIPAddress_Label.AutoSize = true;
            this.endIPAddress_Label.Location = new System.Drawing.Point(300, 66);
            this.endIPAddress_Label.Name = "endIPAddress_Label";
            this.endIPAddress_Label.Size = new System.Drawing.Size(80, 13);
            this.endIPAddress_Label.TabIndex = 4;
            this.endIPAddress_Label.Text = "End IP Address";
            // 
            // startIPAddress_Label
            // 
            this.startIPAddress_Label.AutoSize = true;
            this.startIPAddress_Label.Location = new System.Drawing.Point(131, 66);
            this.startIPAddress_Label.Name = "startIPAddress_Label";
            this.startIPAddress_Label.Size = new System.Drawing.Size(83, 13);
            this.startIPAddress_Label.TabIndex = 3;
            this.startIPAddress_Label.Text = "Start IP Address";
            // 
            // networkHop_NumericUpDown
            // 
            this.networkHop_NumericUpDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.networkHop_NumericUpDown.InterceptArrowKeys = false;
            this.networkHop_NumericUpDown.Location = new System.Drawing.Point(174, 22);
            this.networkHop_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.networkHop_NumericUpDown.Name = "networkHop_NumericUpDown";
            this.networkHop_NumericUpDown.ReadOnly = true;
            this.networkHop_NumericUpDown.Size = new System.Drawing.Size(184, 20);
            this.networkHop_NumericUpDown.TabIndex = 2;
            this.networkHop_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ipRange_RadioButton
            // 
            this.ipRange_RadioButton.AutoSize = true;
            this.ipRange_RadioButton.Location = new System.Drawing.Point(13, 85);
            this.ipRange_RadioButton.Name = "ipRange_RadioButton";
            this.ipRange_RadioButton.Size = new System.Drawing.Size(57, 17);
            this.ipRange_RadioButton.TabIndex = 1;
            this.ipRange_RadioButton.TabStop = true;
            this.ipRange_RadioButton.Text = "Range";
            this.ipRange_RadioButton.UseVisualStyleBackColor = true;
            this.ipRange_RadioButton.CheckedChanged += new System.EventHandler(this.iPRange_RadioButton_CheckedChanged);
            // 
            // numberOfNetworkHops_RadioButton
            // 
            this.numberOfNetworkHops_RadioButton.AutoSize = true;
            this.numberOfNetworkHops_RadioButton.Location = new System.Drawing.Point(13, 22);
            this.numberOfNetworkHops_RadioButton.Name = "numberOfNetworkHops_RadioButton";
            this.numberOfNetworkHops_RadioButton.Size = new System.Drawing.Size(147, 17);
            this.numberOfNetworkHops_RadioButton.TabIndex = 0;
            this.numberOfNetworkHops_RadioButton.TabStop = true;
            this.numberOfNetworkHops_RadioButton.Text = "Number Of Network Hops";
            this.numberOfNetworkHops_RadioButton.UseVisualStyleBackColor = true;
            this.numberOfNetworkHops_RadioButton.CheckedChanged += new System.EventHandler(this.numberOfNetworkHops_RadioButton_CheckedChanged);
            // 
            // discoveryType_Label
            // 
            this.discoveryType_Label.AutoSize = true;
            this.discoveryType_Label.Location = new System.Drawing.Point(10, 43);
            this.discoveryType_Label.Name = "discoveryType_Label";
            this.discoveryType_Label.Size = new System.Drawing.Size(81, 13);
            this.discoveryType_Label.TabIndex = 1;
            this.discoveryType_Label.Text = "Discovery Type";
            // 
            // discoveryType_ComboBox
            // 
            this.discoveryType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.discoveryType_ComboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.discoveryType_ComboBox.FormattingEnabled = true;
            this.discoveryType_ComboBox.Location = new System.Drawing.Point(97, 40);
            this.discoveryType_ComboBox.Name = "discoveryType_ComboBox";
            this.discoveryType_ComboBox.Size = new System.Drawing.Size(210, 21);
            this.discoveryType_ComboBox.TabIndex = 0;
            this.discoveryType_ComboBox.SelectedIndexChanged += new System.EventHandler(this.discoveryType_ComboBox_SelectedIndexChanged);
            // 
            // policyConfiguration_GroupBox
            // 
            this.policyConfiguration_GroupBox.Controls.Add(this.policyPath_TextBox);
            this.policyConfiguration_GroupBox.Controls.Add(this.validatePolicyPath_CheckBox);
            this.policyConfiguration_GroupBox.Controls.Add(this.existingPolicyName_CheckBox);
            this.policyConfiguration_GroupBox.Controls.Add(this.existingPolicyName_TextBox);
            this.policyConfiguration_GroupBox.Controls.Add(this.PolicyPath_Label);
            this.policyConfiguration_GroupBox.Controls.Add(this.policyPassword_TextBox);
            this.policyConfiguration_GroupBox.Controls.Add(this.policyPassword_Label);
            this.policyConfiguration_GroupBox.Enabled = false;
            this.policyConfiguration_GroupBox.Location = new System.Drawing.Point(6, 224);
            this.policyConfiguration_GroupBox.Name = "policyConfiguration_GroupBox";
            this.policyConfiguration_GroupBox.Size = new System.Drawing.Size(805, 143);
            this.policyConfiguration_GroupBox.TabIndex = 60;
            this.policyConfiguration_GroupBox.TabStop = false;
            this.policyConfiguration_GroupBox.Text = "Policy Configuration";
            // 
            // policyPath_TextBox
            // 
            this.policyPath_TextBox.Location = new System.Drawing.Point(103, 57);
            this.policyPath_TextBox.Name = "policyPath_TextBox";
            this.policyPath_TextBox.Size = new System.Drawing.Size(264, 20);
            this.policyPath_TextBox.TabIndex = 54;
            // 
            // validatePolicyPath_CheckBox
            // 
            this.validatePolicyPath_CheckBox.Location = new System.Drawing.Point(414, 25);
            this.validatePolicyPath_CheckBox.Name = "validatePolicyPath_CheckBox";
            this.validatePolicyPath_CheckBox.Size = new System.Drawing.Size(122, 24);
            this.validatePolicyPath_CheckBox.TabIndex = 55;
            this.validatePolicyPath_CheckBox.Text = "Apply Network Credentials";
            // 
            // existingPolicyName_CheckBox
            // 
            this.existingPolicyName_CheckBox.AutoSize = true;
            this.existingPolicyName_CheckBox.Location = new System.Drawing.Point(9, 29);
            this.existingPolicyName_CheckBox.Name = "existingPolicyName_CheckBox";
            this.existingPolicyName_CheckBox.Size = new System.Drawing.Size(85, 17);
            this.existingPolicyName_CheckBox.TabIndex = 5;
            this.existingPolicyName_CheckBox.Text = "Policy Name";
            this.existingPolicyName_CheckBox.UseVisualStyleBackColor = true;
            this.existingPolicyName_CheckBox.CheckedChanged += new System.EventHandler(this.existingPolicyName_CheckBox_CheckedChanged);
            // 
            // existingPolicyName_TextBox
            // 
            this.existingPolicyName_TextBox.Enabled = false;
            this.existingPolicyName_TextBox.Location = new System.Drawing.Point(103, 27);
            this.existingPolicyName_TextBox.Name = "existingPolicyName_TextBox";
            this.existingPolicyName_TextBox.Size = new System.Drawing.Size(127, 20);
            this.existingPolicyName_TextBox.TabIndex = 56;
            // 
            // PolicyPath_Label
            // 
            this.PolicyPath_Label.AutoSize = true;
            this.PolicyPath_Label.Location = new System.Drawing.Point(6, 60);
            this.PolicyPath_Label.Name = "PolicyPath_Label";
            this.PolicyPath_Label.Size = new System.Drawing.Size(60, 13);
            this.PolicyPath_Label.TabIndex = 56;
            this.PolicyPath_Label.Text = "Policy Path";
            // 
            // policyPassword_TextBox
            // 
            this.policyPassword_TextBox.Location = new System.Drawing.Point(103, 89);
            this.policyPassword_TextBox.Name = "policyPassword_TextBox";
            this.policyPassword_TextBox.PasswordChar = '*';
            this.policyPassword_TextBox.Size = new System.Drawing.Size(264, 20);
            this.policyPassword_TextBox.TabIndex = 58;
            // 
            // policyPassword_Label
            // 
            this.policyPassword_Label.AutoSize = true;
            this.policyPassword_Label.Location = new System.Drawing.Point(6, 89);
            this.policyPassword_Label.Name = "policyPassword_Label";
            this.policyPassword_Label.Size = new System.Drawing.Size(84, 13);
            this.policyPassword_Label.TabIndex = 57;
            this.policyPassword_Label.Text = "Policy Password";
            // 
            // LockSmithConfigurationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.locksmithConfiguration_Panel);
            this.Name = "LockSmithConfigurationConfigControl";
            this.Size = new System.Drawing.Size(824, 743);
            this.locksmithConfiguration_Panel.ResumeLayout(false);
            this.globalCredentials_GroupBox.ResumeLayout(false);
            this.globalCredentials_GroupBox.PerformLayout();
            this.tasekType_GroupBox.ResumeLayout(false);
            this.tasekType_GroupBox.PerformLayout();
            this.groupConfiguration_GroupBox.ResumeLayout(false);
            this.groupConfiguration_GroupBox.PerformLayout();
            this.browser_GroupBox.ResumeLayout(false);
            this.browser_GroupBox.PerformLayout();
            this.activity_GroupBox.ResumeLayout(false);
            this.activity_GroupBox.PerformLayout();
            this.serverDetails_GroupBox.ResumeLayout(false);
            this.serverDetails_GroupBox.PerformLayout();
            this.deviceDiscovery_GroupBox.ResumeLayout(false);
            this.deviceDiscovery_GroupBox.PerformLayout();
            this.addFromFile_GroupBox.ResumeLayout(false);
            this.addFromFile_GroupBox.PerformLayout();
            this.manualDiscovery_GroupBox.ResumeLayout(false);
            this.manualDiscovery_GroupBox.PerformLayout();
            this.automaticDiscovery_GroupBox.ResumeLayout(false);
            this.automaticDiscovery_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.networkHop_NumericUpDown)).EndInit();
            this.policyConfiguration_GroupBox.ResumeLayout(false);
            this.policyConfiguration_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator lockSmith_FieldValidator;
        private System.Windows.Forms.Panel locksmithConfiguration_Panel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox tasekType_GroupBox;
        private System.Windows.Forms.RadioButton assessAndRemediate_RadioButton;
        private System.Windows.Forms.RadioButton assessOnly_RadioButton;
        private System.Windows.Forms.GroupBox groupConfiguration_GroupBox;
        private System.Windows.Forms.TextBox groupName_TextBox;
        private System.Windows.Forms.Label groupName_Label;
        private System.Windows.Forms.GroupBox browser_GroupBox;
        private System.Windows.Forms.ComboBox browserType_ComboBox;
        private System.Windows.Forms.Label browser_Label;
        private System.Windows.Forms.GroupBox activity_GroupBox;
        private System.Windows.Forms.CheckBox generateReports_CheckBox;
        private System.Windows.Forms.CheckBox importAndApplyPolicy_CheckBox;
        private System.Windows.Forms.CheckBox printerDiscovery_CheckBox;
        private System.Windows.Forms.GroupBox serverDetails_GroupBox;
        private System.Windows.Forms.Label locksmithServer_Label;
        private Framework.UI.ServerComboBox lockSmith_ServerComboBox;
        private System.Windows.Forms.Label adminUser_Label;
        private System.Windows.Forms.TextBox user_TextBox;
        private System.Windows.Forms.Label adminPassword_Label;
        private System.Windows.Forms.TextBox password_TextBox;
        private System.Windows.Forms.GroupBox deviceDiscovery_GroupBox;
        private System.Windows.Forms.GroupBox addFromFile_GroupBox;
        private System.Windows.Forms.Label deviceListPath_Label;
        private System.Windows.Forms.Button ipAddressFileBrowse_button;
        private System.Windows.Forms.TextBox ipAddressFile_textBox;
        private System.Windows.Forms.GroupBox manualDiscovery_GroupBox;
        private System.Windows.Forms.Label ipAddress_Label;
        private Framework.UI.IPAddressControl manualIPAddress_IpAddressControl;
        private System.Windows.Forms.GroupBox automaticDiscovery_GroupBox;
        private Framework.UI.IPAddressControl automaticEndIPAddress_IpAddressControl;
        private Framework.UI.IPAddressControl automaticStartIPAddress_IpAddressControl;
        private System.Windows.Forms.Label endIPAddress_Label;
        private System.Windows.Forms.Label startIPAddress_Label;
        private System.Windows.Forms.NumericUpDown networkHop_NumericUpDown;
        private System.Windows.Forms.RadioButton ipRange_RadioButton;
        private System.Windows.Forms.RadioButton numberOfNetworkHops_RadioButton;
        private System.Windows.Forms.Label discoveryType_Label;
        private System.Windows.Forms.ComboBox discoveryType_ComboBox;
        private System.Windows.Forms.GroupBox policyConfiguration_GroupBox;
        private System.Windows.Forms.CheckBox existingPolicyName_CheckBox;
        private System.Windows.Forms.TextBox existingPolicyName_TextBox;
        private System.Windows.Forms.Label PolicyPath_Label;
        private System.Windows.Forms.TextBox policyPassword_TextBox;
        private System.Windows.Forms.Label policyPassword_Label;
        private System.Windows.Forms.CheckBox validatePolicyPath_CheckBox;
        private System.Windows.Forms.TextBox policyPath_TextBox;
        private System.Windows.Forms.GroupBox globalCredentials_GroupBox;
        private System.Windows.Forms.TextBox ewsAdminPassword_TextBox;
        private System.Windows.Forms.Label ewsAdminPassword_Label;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
    }
}
