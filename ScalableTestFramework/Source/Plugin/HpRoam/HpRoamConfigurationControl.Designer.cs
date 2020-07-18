namespace HP.ScalableTest.Plugin.HpRoam
{
    partial class HpRoamConfigurationControl
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
            this.tabControlHpac = new System.Windows.Forms.TabControl();
            this.tabPagePullPrinting = new System.Windows.Forms.TabPage();
            this.groupBox_Phone = new System.Windows.Forms.GroupBox();
            this.textBox_Description = new System.Windows.Forms.TextBox();
            this.textBox_AssetId = new System.Windows.Forms.TextBox();
            this.button_PhoneSelect = new System.Windows.Forms.Button();
            this.textBox_PhoneId = new System.Windows.Forms.TextBox();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_PullPrintConfig = new System.Windows.Forms.GroupBox();
            this.groupBox_PullPrintType = new System.Windows.Forms.GroupBox();
            this.radioButton_PullPrintPhone = new System.Windows.Forms.RadioButton();
            this.radioButton_PullPrintOxpd = new System.Windows.Forms.RadioButton();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintAll = new System.Windows.Forms.RadioButton();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.radioButton_RoamApp = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.groupBoxDeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabPagePrinting = new System.Windows.Forms.TabPage();
            this.printServerNotificationcheckBox = new System.Windows.Forms.CheckBox();
            this.label_DelayAfterPrint = new System.Windows.Forms.Label();
            this.numericUpDown_DelayAfterPrint = new System.Windows.Forms.NumericUpDown();
            this.groupBoxDocSend = new System.Windows.Forms.GroupBox();
            this.radioButton_PrintViaWebClient = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintViaPhone = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintViaDriver = new System.Windows.Forms.RadioButton();
            this.groupboxDocSelection = new System.Windows.Forms.GroupBox();
            this.label_documentToPush = new System.Windows.Forms.Label();
            this.textBox_PhoneDocument = new System.Windows.Forms.TextBox();
            this.shuffle_CheckBox = new System.Windows.Forms.CheckBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.label_PullPrintDelay = new System.Windows.Forms.Label();
            this.numericUpDown_PullPrintDelay = new System.Windows.Forms.NumericUpDown();
            this.tabControlHpac.SuspendLayout();
            this.tabPagePullPrinting.SuspendLayout();
            this.groupBox_Phone.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBox_PullPrintConfig.SuspendLayout();
            this.groupBox_PullPrintType.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBoxDeviceConfiguration.SuspendLayout();
            this.tabPagePrinting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DelayAfterPrint)).BeginInit();
            this.groupBoxDocSend.SuspendLayout();
            this.groupboxDocSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PullPrintDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlHpac
            // 
            this.tabControlHpac.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlHpac.Controls.Add(this.tabPagePullPrinting);
            this.tabControlHpac.Controls.Add(this.tabPagePrinting);
            this.tabControlHpac.Location = new System.Drawing.Point(4, 6);
            this.tabControlHpac.Name = "tabControlHpac";
            this.tabControlHpac.SelectedIndex = 0;
            this.tabControlHpac.Size = new System.Drawing.Size(719, 546);
            this.tabControlHpac.TabIndex = 2;
            // 
            // tabPagePullPrinting
            // 
            this.tabPagePullPrinting.AutoScroll = true;
            this.tabPagePullPrinting.Controls.Add(this.groupBox_Phone);
            this.tabPagePullPrinting.Controls.Add(this.groupBoxMemoryPofile);
            this.tabPagePullPrinting.Controls.Add(this.groupBox_PullPrintConfig);
            this.tabPagePullPrinting.Controls.Add(this.groupBox_Authentication);
            this.tabPagePullPrinting.Controls.Add(this.groupBoxDeviceConfiguration);
            this.tabPagePullPrinting.Location = new System.Drawing.Point(4, 24);
            this.tabPagePullPrinting.Name = "tabPagePullPrinting";
            this.tabPagePullPrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePullPrinting.Size = new System.Drawing.Size(711, 518);
            this.tabPagePullPrinting.TabIndex = 0;
            this.tabPagePullPrinting.Text = "PullPrinting";
            this.tabPagePullPrinting.UseVisualStyleBackColor = true;
            // 
            // groupBox_Phone
            // 
            this.groupBox_Phone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Phone.Controls.Add(this.textBox_Description);
            this.groupBox_Phone.Controls.Add(this.textBox_AssetId);
            this.groupBox_Phone.Controls.Add(this.button_PhoneSelect);
            this.groupBox_Phone.Controls.Add(this.textBox_PhoneId);
            this.groupBox_Phone.Location = new System.Drawing.Point(8, 198);
            this.groupBox_Phone.Name = "groupBox_Phone";
            this.groupBox_Phone.Size = new System.Drawing.Size(697, 62);
            this.groupBox_Phone.TabIndex = 5;
            this.groupBox_Phone.TabStop = false;
            this.groupBox_Phone.Text = "Phone Selection";
            // 
            // textBox_Description
            // 
            this.textBox_Description.Enabled = false;
            this.textBox_Description.Location = new System.Drawing.Point(497, 26);
            this.textBox_Description.Name = "textBox_Description";
            this.textBox_Description.ReadOnly = true;
            this.textBox_Description.Size = new System.Drawing.Size(120, 23);
            this.textBox_Description.TabIndex = 14;
            // 
            // textBox_AssetId
            // 
            this.textBox_AssetId.Enabled = false;
            this.textBox_AssetId.Location = new System.Drawing.Point(9, 26);
            this.textBox_AssetId.Name = "textBox_AssetId";
            this.textBox_AssetId.ReadOnly = true;
            this.textBox_AssetId.Size = new System.Drawing.Size(120, 23);
            this.textBox_AssetId.TabIndex = 13;
            // 
            // button_PhoneSelect
            // 
            this.button_PhoneSelect.Enabled = false;
            this.button_PhoneSelect.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_PhoneSelect.Location = new System.Drawing.Point(623, 25);
            this.button_PhoneSelect.Name = "button_PhoneSelect";
            this.button_PhoneSelect.Size = new System.Drawing.Size(31, 25);
            this.button_PhoneSelect.TabIndex = 12;
            this.button_PhoneSelect.Text = "...";
            this.button_PhoneSelect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_PhoneSelect.UseVisualStyleBackColor = true;
            this.button_PhoneSelect.Click += new System.EventHandler(this.Button_PhoneSelect_Click);
            // 
            // textBox_PhoneId
            // 
            this.textBox_PhoneId.Enabled = false;
            this.textBox_PhoneId.Location = new System.Drawing.Point(135, 26);
            this.textBox_PhoneId.Name = "textBox_PhoneId";
            this.textBox_PhoneId.ReadOnly = true;
            this.textBox_PhoneId.Size = new System.Drawing.Size(356, 23);
            this.textBox_PhoneId.TabIndex = 10;
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(392, 381);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(313, 103);
            this.groupBoxMemoryPofile.TabIndex = 4;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(6, 26);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(307, 84);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // groupBox_PullPrintConfig
            // 
            this.groupBox_PullPrintConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PullPrintConfig.Controls.Add(this.numericUpDown_PullPrintDelay);
            this.groupBox_PullPrintConfig.Controls.Add(this.label_PullPrintDelay);
            this.groupBox_PullPrintConfig.Controls.Add(this.groupBox_PullPrintType);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_PrintAll);
            this.groupBox_PullPrintConfig.Location = new System.Drawing.Point(8, 270);
            this.groupBox_PullPrintConfig.Name = "groupBox_PullPrintConfig";
            this.groupBox_PullPrintConfig.Size = new System.Drawing.Size(697, 103);
            this.groupBox_PullPrintConfig.TabIndex = 3;
            this.groupBox_PullPrintConfig.TabStop = false;
            this.groupBox_PullPrintConfig.Text = "Pull Print Configuration";
            // 
            // groupBox_PullPrintType
            // 
            this.groupBox_PullPrintType.Controls.Add(this.radioButton_PullPrintPhone);
            this.groupBox_PullPrintType.Controls.Add(this.radioButton_PullPrintOxpd);
            this.groupBox_PullPrintType.Location = new System.Drawing.Point(9, 16);
            this.groupBox_PullPrintType.Name = "groupBox_PullPrintType";
            this.groupBox_PullPrintType.Size = new System.Drawing.Size(135, 81);
            this.groupBox_PullPrintType.TabIndex = 10;
            this.groupBox_PullPrintType.TabStop = false;
            // 
            // radioButton_PullPrintPhone
            // 
            this.radioButton_PullPrintPhone.AutoSize = true;
            this.radioButton_PullPrintPhone.Location = new System.Drawing.Point(37, 46);
            this.radioButton_PullPrintPhone.Name = "radioButton_PullPrintPhone";
            this.radioButton_PullPrintPhone.Size = new System.Drawing.Size(59, 19);
            this.radioButton_PullPrintPhone.TabIndex = 4;
            this.radioButton_PullPrintPhone.Text = "Phone";
            this.radioButton_PullPrintPhone.UseVisualStyleBackColor = true;
            this.radioButton_PullPrintPhone.CheckedChanged += new System.EventHandler(this.radioButton_PullPrintPhone_CheckedChanged);
            // 
            // radioButton_PullPrintOxpd
            // 
            this.radioButton_PullPrintOxpd.AutoSize = true;
            this.radioButton_PullPrintOxpd.Checked = true;
            this.radioButton_PullPrintOxpd.Location = new System.Drawing.Point(37, 21);
            this.radioButton_PullPrintOxpd.Name = "radioButton_PullPrintOxpd";
            this.radioButton_PullPrintOxpd.Size = new System.Drawing.Size(55, 19);
            this.radioButton_PullPrintOxpd.TabIndex = 3;
            this.radioButton_PullPrintOxpd.TabStop = true;
            this.radioButton_PullPrintOxpd.Text = "OXPd";
            this.radioButton_PullPrintOxpd.UseVisualStyleBackColor = true;
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(187, 78);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Delete.TabIndex = 9;
            this.radioButton_Delete.TabStop = true;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            this.radioButton_Delete.CheckedChanged += new System.EventHandler(this.radioButton_PullPrintOperation_CheckedChanged);
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(187, 49);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(50, 19);
            this.radioButton_Print.TabIndex = 2;
            this.radioButton_Print.TabStop = true;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            this.radioButton_Print.CheckedChanged += new System.EventHandler(this.radioButton_PullPrintOperation_CheckedChanged);
            // 
            // radioButton_PrintAll
            // 
            this.radioButton_PrintAll.AutoSize = true;
            this.radioButton_PrintAll.Checked = true;
            this.radioButton_PrintAll.Location = new System.Drawing.Point(187, 22);
            this.radioButton_PrintAll.Name = "radioButton_PrintAll";
            this.radioButton_PrintAll.Size = new System.Drawing.Size(65, 19);
            this.radioButton_PrintAll.TabIndex = 1;
            this.radioButton_PrintAll.TabStop = true;
            this.radioButton_PrintAll.Text = "Print all";
            this.radioButton_PrintAll.UseVisualStyleBackColor = true;
            this.radioButton_PrintAll.CheckedChanged += new System.EventHandler(this.radioButton_PullPrintOperation_CheckedChanged);
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.radioButton_RoamApp);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Location = new System.Drawing.Point(8, 381);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(378, 103);
            this.groupBox_Authentication.TabIndex = 2;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(18, 62);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 23;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(155, 59);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(216, 23);
            this.comboBox_AuthProvider.TabIndex = 2;
            this.comboBox_AuthProvider.ValueMember = "Key";
            this.comboBox_AuthProvider.SelectedIndexChanged += new System.EventHandler(this.comboBox_AuthProvider_SelectedIndexChanged);
            // 
            // radioButton_RoamApp
            // 
            this.radioButton_RoamApp.AutoSize = true;
            this.radioButton_RoamApp.Location = new System.Drawing.Point(170, 31);
            this.radioButton_RoamApp.Name = "radioButton_RoamApp";
            this.radioButton_RoamApp.Size = new System.Drawing.Size(75, 19);
            this.radioButton_RoamApp.TabIndex = 1;
            this.radioButton_RoamApp.Text = "HP Roam";
            this.radioButton_RoamApp.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(21, 31);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(100, 19);
            this.radioButton_SignInButton.TabIndex = 0;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In Button";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // groupBoxDeviceConfiguration
            // 
            this.groupBoxDeviceConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDeviceConfiguration.Controls.Add(this.lockTimeoutControl);
            this.groupBoxDeviceConfiguration.Controls.Add(this.assetSelectionControl);
            this.groupBoxDeviceConfiguration.Location = new System.Drawing.Point(5, 6);
            this.groupBoxDeviceConfiguration.Name = "groupBoxDeviceConfiguration";
            this.groupBoxDeviceConfiguration.Size = new System.Drawing.Size(700, 177);
            this.groupBoxDeviceConfiguration.TabIndex = 1;
            this.groupBoxDeviceConfiguration.TabStop = false;
            this.groupBoxDeviceConfiguration.Text = "Device Configuration";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 119);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 19);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(688, 94);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabPagePrinting
            // 
            this.tabPagePrinting.AutoScroll = true;
            this.tabPagePrinting.Controls.Add(this.printServerNotificationcheckBox);
            this.tabPagePrinting.Controls.Add(this.label_DelayAfterPrint);
            this.tabPagePrinting.Controls.Add(this.numericUpDown_DelayAfterPrint);
            this.tabPagePrinting.Controls.Add(this.groupBoxDocSend);
            this.tabPagePrinting.Controls.Add(this.groupboxDocSelection);
            this.tabPagePrinting.Location = new System.Drawing.Point(4, 24);
            this.tabPagePrinting.Name = "tabPagePrinting";
            this.tabPagePrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePrinting.Size = new System.Drawing.Size(711, 518);
            this.tabPagePrinting.TabIndex = 1;
            this.tabPagePrinting.Text = "Printing";
            this.tabPagePrinting.UseVisualStyleBackColor = true;
            // 
            // printServerNotificationcheckBox
            // 
            this.printServerNotificationcheckBox.AutoSize = true;
            this.printServerNotificationcheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.printServerNotificationcheckBox.Location = new System.Drawing.Point(325, 63);
            this.printServerNotificationcheckBox.Name = "printServerNotificationcheckBox";
            this.printServerNotificationcheckBox.Size = new System.Drawing.Size(174, 19);
            this.printServerNotificationcheckBox.TabIndex = 64;
            this.printServerNotificationcheckBox.Text = "Use Print Server Notification";
            this.printServerNotificationcheckBox.UseVisualStyleBackColor = true;
            this.printServerNotificationcheckBox.CheckedChanged += new System.EventHandler(this.PrintServerNotificationcheckBox_CheckedChanged);
            // 
            // label_DelayAfterPrint
            // 
            this.label_DelayAfterPrint.AutoSize = true;
            this.label_DelayAfterPrint.Location = new System.Drawing.Point(298, 33);
            this.label_DelayAfterPrint.Name = "label_DelayAfterPrint";
            this.label_DelayAfterPrint.Size = new System.Drawing.Size(145, 15);
            this.label_DelayAfterPrint.TabIndex = 63;
            this.label_DelayAfterPrint.Text = "Delay after print (seconds)";
            // 
            // numericUpDown_DelayAfterPrint
            // 
            this.numericUpDown_DelayAfterPrint.Location = new System.Drawing.Point(449, 29);
            this.numericUpDown_DelayAfterPrint.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown_DelayAfterPrint.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_DelayAfterPrint.Name = "numericUpDown_DelayAfterPrint";
            this.numericUpDown_DelayAfterPrint.Size = new System.Drawing.Size(46, 23);
            this.numericUpDown_DelayAfterPrint.TabIndex = 62;
            this.numericUpDown_DelayAfterPrint.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // groupBoxDocSend
            // 
            this.groupBoxDocSend.Controls.Add(this.radioButton_PrintViaWebClient);
            this.groupBoxDocSend.Controls.Add(this.radioButton_PrintViaPhone);
            this.groupBoxDocSend.Controls.Add(this.radioButton_PrintViaDriver);
            this.groupBoxDocSend.Location = new System.Drawing.Point(13, 29);
            this.groupBoxDocSend.Name = "groupBoxDocSend";
            this.groupBoxDocSend.Size = new System.Drawing.Size(264, 128);
            this.groupBoxDocSend.TabIndex = 1;
            this.groupBoxDocSend.TabStop = false;
            this.groupBoxDocSend.Text = "Document Send Methodology";
            // 
            // radioButton_PrintViaWebClient
            // 
            this.radioButton_PrintViaWebClient.AutoSize = true;
            this.radioButton_PrintViaWebClient.Enabled = false;
            this.radioButton_PrintViaWebClient.Location = new System.Drawing.Point(19, 85);
            this.radioButton_PrintViaWebClient.Name = "radioButton_PrintViaWebClient";
            this.radioButton_PrintViaWebClient.Size = new System.Drawing.Size(136, 19);
            this.radioButton_PrintViaWebClient.TabIndex = 2;
            this.radioButton_PrintViaWebClient.TabStop = true;
            this.radioButton_PrintViaWebClient.Tag = "WebClient";
            this.radioButton_PrintViaWebClient.Text = "HP Roam Web Client";
            this.radioButton_PrintViaWebClient.UseVisualStyleBackColor = true;
            this.radioButton_PrintViaWebClient.CheckedChanged += new System.EventHandler(this.CheckedChanged_RoamDocSend);
            // 
            // radioButton_PrintViaPhone
            // 
            this.radioButton_PrintViaPhone.AutoSize = true;
            this.radioButton_PrintViaPhone.Location = new System.Drawing.Point(19, 59);
            this.radioButton_PrintViaPhone.Name = "radioButton_PrintViaPhone";
            this.radioButton_PrintViaPhone.Size = new System.Drawing.Size(158, 19);
            this.radioButton_PrintViaPhone.TabIndex = 1;
            this.radioButton_PrintViaPhone.TabStop = true;
            this.radioButton_PrintViaPhone.Tag = "Android";
            this.radioButton_PrintViaPhone.Text = "HP Roam Android Phone";
            this.radioButton_PrintViaPhone.UseVisualStyleBackColor = true;
            this.radioButton_PrintViaPhone.CheckedChanged += new System.EventHandler(this.CheckedChanged_RoamDocSend);
            // 
            // radioButton_PrintViaDriver
            // 
            this.radioButton_PrintViaDriver.AutoSize = true;
            this.radioButton_PrintViaDriver.Checked = true;
            this.radioButton_PrintViaDriver.Location = new System.Drawing.Point(19, 33);
            this.radioButton_PrintViaDriver.Name = "radioButton_PrintViaDriver";
            this.radioButton_PrintViaDriver.Size = new System.Drawing.Size(161, 19);
            this.radioButton_PrintViaDriver.TabIndex = 0;
            this.radioButton_PrintViaDriver.TabStop = true;
            this.radioButton_PrintViaDriver.Tag = "Windows";
            this.radioButton_PrintViaDriver.Text = "HP Roam Windows Driver";
            this.radioButton_PrintViaDriver.UseVisualStyleBackColor = true;
            this.radioButton_PrintViaDriver.CheckedChanged += new System.EventHandler(this.CheckedChanged_RoamDocSend);
            // 
            // groupboxDocSelection
            // 
            this.groupboxDocSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxDocSelection.Controls.Add(this.label_documentToPush);
            this.groupboxDocSelection.Controls.Add(this.textBox_PhoneDocument);
            this.groupboxDocSelection.Controls.Add(this.shuffle_CheckBox);
            this.groupboxDocSelection.Controls.Add(this.documentSelectionControl);
            this.groupboxDocSelection.Location = new System.Drawing.Point(7, 172);
            this.groupboxDocSelection.Name = "groupboxDocSelection";
            this.groupboxDocSelection.Size = new System.Drawing.Size(664, 340);
            this.groupboxDocSelection.TabIndex = 0;
            this.groupboxDocSelection.TabStop = false;
            this.groupboxDocSelection.Text = "Document Selection";
            // 
            // label_documentToPush
            // 
            this.label_documentToPush.AutoSize = true;
            this.label_documentToPush.Enabled = false;
            this.label_documentToPush.Location = new System.Drawing.Point(265, 25);
            this.label_documentToPush.Name = "label_documentToPush";
            this.label_documentToPush.Size = new System.Drawing.Size(175, 15);
            this.label_documentToPush.TabIndex = 3;
            this.label_documentToPush.Text = "Document to push from phone:";
            // 
            // textBox_PhoneDocument
            // 
            this.textBox_PhoneDocument.Enabled = false;
            this.textBox_PhoneDocument.Location = new System.Drawing.Point(446, 22);
            this.textBox_PhoneDocument.Name = "textBox_PhoneDocument";
            this.textBox_PhoneDocument.Size = new System.Drawing.Size(212, 23);
            this.textBox_PhoneDocument.TabIndex = 2;
            // 
            // shuffle_CheckBox
            // 
            this.shuffle_CheckBox.AutoSize = true;
            this.shuffle_CheckBox.Location = new System.Drawing.Point(6, 32);
            this.shuffle_CheckBox.Name = "shuffle_CheckBox";
            this.shuffle_CheckBox.Size = new System.Drawing.Size(197, 19);
            this.shuffle_CheckBox.TabIndex = 1;
            this.shuffle_CheckBox.Text = "Shuffle document printing order";
            this.shuffle_CheckBox.UseVisualStyleBackColor = true;
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(6, 57);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(658, 272);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // label_PullPrintDelay
            // 
            this.label_PullPrintDelay.Location = new System.Drawing.Point(293, 35);
            this.label_PullPrintDelay.Name = "label_PullPrintDelay";
            this.label_PullPrintDelay.Size = new System.Drawing.Size(236, 20);
            this.label_PullPrintDelay.TabIndex = 12;
            this.label_PullPrintDelay.Text = "Delay before Print button click (seconds)";
            this.label_PullPrintDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDown_PullPrintDelay
            // 
            this.numericUpDown_PullPrintDelay.Location = new System.Drawing.Point(518, 37);
            this.numericUpDown_PullPrintDelay.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown_PullPrintDelay.Name = "numericUpDown_PullPrintDelay";
            this.numericUpDown_PullPrintDelay.Size = new System.Drawing.Size(46, 23);
            this.numericUpDown_PullPrintDelay.TabIndex = 63;
            this.numericUpDown_PullPrintDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // HpRoamConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlHpac);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpRoamConfigurationControl";
            this.Size = new System.Drawing.Size(726, 555);
            this.tabControlHpac.ResumeLayout(false);
            this.tabPagePullPrinting.ResumeLayout(false);
            this.groupBox_Phone.ResumeLayout(false);
            this.groupBox_Phone.PerformLayout();
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBox_PullPrintConfig.ResumeLayout(false);
            this.groupBox_PullPrintConfig.PerformLayout();
            this.groupBox_PullPrintType.ResumeLayout(false);
            this.groupBox_PullPrintType.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBoxDeviceConfiguration.ResumeLayout(false);
            this.tabPagePrinting.ResumeLayout(false);
            this.tabPagePrinting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DelayAfterPrint)).EndInit();
            this.groupBoxDocSend.ResumeLayout(false);
            this.groupBoxDocSend.PerformLayout();
            this.groupboxDocSelection.ResumeLayout(false);
            this.groupboxDocSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PullPrintDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl tabControlHpac;
        private System.Windows.Forms.TabPage tabPagePullPrinting;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.GroupBox groupBox_PullPrintConfig;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.RadioButton radioButton_PrintAll;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.RadioButton radioButton_RoamApp;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.GroupBox groupBoxDeviceConfiguration;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.TabPage tabPagePrinting;
        private System.Windows.Forms.GroupBox groupboxDocSelection;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private System.Windows.Forms.CheckBox shuffle_CheckBox;
        private System.Windows.Forms.GroupBox groupBoxDocSend;
        private System.Windows.Forms.RadioButton radioButton_PrintViaDriver;
        private System.Windows.Forms.RadioButton radioButton_PrintViaWebClient;
        private System.Windows.Forms.RadioButton radioButton_PrintViaPhone;
        private System.Windows.Forms.CheckBox printServerNotificationcheckBox;
        private System.Windows.Forms.Label label_DelayAfterPrint;
        private System.Windows.Forms.NumericUpDown numericUpDown_DelayAfterPrint;
        private System.Windows.Forms.GroupBox groupBox_Phone;
        private System.Windows.Forms.Button button_PhoneSelect;
        private System.Windows.Forms.TextBox textBox_PhoneId;
        private System.Windows.Forms.TextBox textBox_Description;
        private System.Windows.Forms.TextBox textBox_AssetId;
        private System.Windows.Forms.TextBox textBox_PhoneDocument;
        private System.Windows.Forms.Label label_documentToPush;
        private System.Windows.Forms.GroupBox groupBox_PullPrintType;
        private System.Windows.Forms.RadioButton radioButton_PullPrintPhone;
        private System.Windows.Forms.RadioButton radioButton_PullPrintOxpd;
        private System.Windows.Forms.Label label_PullPrintDelay;
        private System.Windows.Forms.NumericUpDown numericUpDown_PullPrintDelay;
    }
}
