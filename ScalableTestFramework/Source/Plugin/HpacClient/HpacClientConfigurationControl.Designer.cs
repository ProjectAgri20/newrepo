namespace HP.ScalableTest.Plugin.HpacClient
{
    partial class HpacClientConfigurationControl
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
            this.serverComboBoxHpac = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.serverIplabel = new System.Windows.Forms.Label();
            this.textBoxInstallerPath = new System.Windows.Forms.TextBox();
            this.installerPathButton = new System.Windows.Forms.Button();
            this.lprQueuelabel = new System.Windows.Forms.Label();
            this.lprQueuetextBox = new System.Windows.Forms.TextBox();
            this.driverInstallGroupBox = new System.Windows.Forms.GroupBox();
            this.checkBoxDefaultPrinter = new System.Windows.Forms.CheckBox();
            this.groupBoxQueueConfiguration = new System.Windows.Forms.GroupBox();
            this.lprTipLabel = new System.Windows.Forms.Label();
            this.groupBoxDriverSelection = new System.Windows.Forms.GroupBox();
            this.printDriverSelectionControl = new HP.ScalableTest.Framework.UI.PrintDriverSelectionControl();
            this.groupBoxSpool = new System.Windows.Forms.GroupBox();
            this.labelSpoolingPrintDocuments = new System.Windows.Forms.Label();
            this.radioButtonPrintImmediately = new System.Windows.Forms.RadioButton();
            this.radioButtonPrintAfterSpooling = new System.Windows.Forms.RadioButton();
            this.groupBoxBidi = new System.Windows.Forms.GroupBox();
            this.labelBidiCommunication = new System.Windows.Forms.Label();
            this.radioButtonDisableBidi = new System.Windows.Forms.RadioButton();
            this.radioButtonEnableBidi = new System.Windows.Forms.RadioButton();
            this.hpacClientInstallGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBoxServerName = new System.Windows.Forms.GroupBox();
            this.textBoxJAServerName = new System.Windows.Forms.TextBox();
            this.textBoxIPMServerName = new System.Windows.Forms.TextBox();
            this.textBoxPullPrintServerName = new System.Windows.Forms.TextBox();
            this.labelPullPrintServerName = new System.Windows.Forms.Label();
            this.labelIpmServerName = new System.Windows.Forms.Label();
            this.labelJaServerName = new System.Windows.Forms.Label();
            this.groupBoxHpacConfig = new System.Windows.Forms.GroupBox();
            this.checkBoxJobStorage = new System.Windows.Forms.CheckBox();
            this.checkBoxDelegate = new System.Windows.Forms.CheckBox();
            this.checkBoxIpm = new System.Windows.Forms.CheckBox();
            this.checkBoxQuota = new System.Windows.Forms.CheckBox();
            this.clientInstallerPathlabel = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.checkBoxInstallClient = new System.Windows.Forms.CheckBox();
            this.driverInstallGroupBox.SuspendLayout();
            this.groupBoxQueueConfiguration.SuspendLayout();
            this.groupBoxDriverSelection.SuspendLayout();
            this.groupBoxSpool.SuspendLayout();
            this.groupBoxBidi.SuspendLayout();
            this.hpacClientInstallGroupBox.SuspendLayout();
            this.groupBoxServerName.SuspendLayout();
            this.groupBoxHpacConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverComboBoxHpac
            // 
            this.serverComboBoxHpac.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverComboBoxHpac.Location = new System.Drawing.Point(108, 16);
            this.serverComboBoxHpac.Margin = new System.Windows.Forms.Padding(1);
            this.serverComboBoxHpac.Name = "serverComboBoxHpac";
            this.serverComboBoxHpac.Size = new System.Drawing.Size(426, 24);
            this.serverComboBoxHpac.TabIndex = 6;
            // 
            // serverIplabel
            // 
            this.serverIplabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverIplabel.AutoSize = true;
            this.serverIplabel.Location = new System.Drawing.Point(29, 27);
            this.serverIplabel.Name = "serverIplabel";
            this.serverIplabel.Size = new System.Drawing.Size(73, 13);
            this.serverIplabel.TabIndex = 5;
            this.serverIplabel.Text = "HPAC Server ";
            // 
            // textBoxInstallerPath
            // 
            this.textBoxInstallerPath.Location = new System.Drawing.Point(182, 30);
            this.textBoxInstallerPath.Name = "textBoxInstallerPath";
            this.textBoxInstallerPath.Size = new System.Drawing.Size(332, 20);
            this.textBoxInstallerPath.TabIndex = 21;
            // 
            // installerPathButton
            // 
            this.installerPathButton.Location = new System.Drawing.Point(524, 27);
            this.installerPathButton.Name = "installerPathButton";
            this.installerPathButton.Size = new System.Drawing.Size(47, 23);
            this.installerPathButton.TabIndex = 22;
            this.installerPathButton.Text = "...";
            this.installerPathButton.UseVisualStyleBackColor = true;
            this.installerPathButton.Click += new System.EventHandler(this.InstallerPathButtonClick);
            // 
            // lprQueuelabel
            // 
            this.lprQueuelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lprQueuelabel.AutoSize = true;
            this.lprQueuelabel.Location = new System.Drawing.Point(10, 47);
            this.lprQueuelabel.Name = "lprQueuelabel";
            this.lprQueuelabel.Size = new System.Drawing.Size(94, 13);
            this.lprQueuelabel.TabIndex = 7;
            this.lprQueuelabel.Text = "LPR Queue Name";
            // 
            // lprQueuetextBox
            // 
            this.lprQueuetextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lprQueuetextBox.Location = new System.Drawing.Point(107, 44);
            this.lprQueuetextBox.Margin = new System.Windows.Forms.Padding(1);
            this.lprQueuetextBox.Name = "lprQueuetextBox";
            this.lprQueuetextBox.Size = new System.Drawing.Size(426, 20);
            this.lprQueuetextBox.TabIndex = 8;
            // 
            // driverInstallGroupBox
            // 
            this.driverInstallGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.driverInstallGroupBox.CausesValidation = false;
            this.driverInstallGroupBox.Controls.Add(this.checkBoxDefaultPrinter);
            this.driverInstallGroupBox.Controls.Add(this.groupBoxQueueConfiguration);
            this.driverInstallGroupBox.Controls.Add(this.groupBoxDriverSelection);
            this.driverInstallGroupBox.Controls.Add(this.groupBoxSpool);
            this.driverInstallGroupBox.Controls.Add(this.groupBoxBidi);
            this.driverInstallGroupBox.Location = new System.Drawing.Point(25, 13);
            this.driverInstallGroupBox.Name = "driverInstallGroupBox";
            this.driverInstallGroupBox.Size = new System.Drawing.Size(689, 349);
            this.driverInstallGroupBox.TabIndex = 1;
            this.driverInstallGroupBox.TabStop = false;
            this.driverInstallGroupBox.Text = "Driver Install with LPR Queue";
            // 
            // checkBoxDefaultPrinter
            // 
            this.checkBoxDefaultPrinter.AutoSize = true;
            this.checkBoxDefaultPrinter.Location = new System.Drawing.Point(22, 197);
            this.checkBoxDefaultPrinter.Name = "checkBoxDefaultPrinter";
            this.checkBoxDefaultPrinter.Size = new System.Drawing.Size(126, 17);
            this.checkBoxDefaultPrinter.TabIndex = 10;
            this.checkBoxDefaultPrinter.Text = "Set as Default Printer";
            this.checkBoxDefaultPrinter.UseVisualStyleBackColor = true;
            // 
            // groupBoxQueueConfiguration
            // 
            this.groupBoxQueueConfiguration.Controls.Add(this.serverComboBoxHpac);
            this.groupBoxQueueConfiguration.Controls.Add(this.lprQueuetextBox);
            this.groupBoxQueueConfiguration.Controls.Add(this.serverIplabel);
            this.groupBoxQueueConfiguration.Controls.Add(this.lprQueuelabel);
            this.groupBoxQueueConfiguration.Controls.Add(this.lprTipLabel);
            this.groupBoxQueueConfiguration.Location = new System.Drawing.Point(62, 108);
            this.groupBoxQueueConfiguration.Margin = new System.Windows.Forms.Padding(1);
            this.groupBoxQueueConfiguration.Name = "groupBoxQueueConfiguration";
            this.groupBoxQueueConfiguration.Size = new System.Drawing.Size(548, 85);
            this.groupBoxQueueConfiguration.TabIndex = 4;
            this.groupBoxQueueConfiguration.TabStop = false;
            this.groupBoxQueueConfiguration.Text = "LPR Queue Settings";
            // 
            // lprTipLabel
            // 
            this.lprTipLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lprTipLabel.AutoSize = true;
            this.lprTipLabel.Location = new System.Drawing.Point(432, 67);
            this.lprTipLabel.Name = "lprTipLabel";
            this.lprTipLabel.Size = new System.Drawing.Size(101, 13);
            this.lprTipLabel.TabIndex = 9;
            this.lprTipLabel.Text = "(as in HPAC Server)";
            // 
            // groupBoxDriverSelection
            // 
            this.groupBoxDriverSelection.Controls.Add(this.printDriverSelectionControl);
            this.groupBoxDriverSelection.Location = new System.Drawing.Point(62, 19);
            this.groupBoxDriverSelection.Name = "groupBoxDriverSelection";
            this.groupBoxDriverSelection.Padding = new System.Windows.Forms.Padding(1);
            this.groupBoxDriverSelection.Size = new System.Drawing.Size(548, 83);
            this.groupBoxDriverSelection.TabIndex = 2;
            this.groupBoxDriverSelection.TabStop = false;
            this.groupBoxDriverSelection.Text = "Driver Selection";
            // 
            // printDriverSelectionControl
            // 
            this.printDriverSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.printDriverSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printDriverSelectionControl.Location = new System.Drawing.Point(19, 17);
            this.printDriverSelectionControl.Name = "printDriverSelectionControl";
            this.printDriverSelectionControl.Size = new System.Drawing.Size(515, 62);
            this.printDriverSelectionControl.TabIndex = 3;
            // 
            // groupBoxSpool
            // 
            this.groupBoxSpool.Controls.Add(this.labelSpoolingPrintDocuments);
            this.groupBoxSpool.Controls.Add(this.radioButtonPrintImmediately);
            this.groupBoxSpool.Controls.Add(this.radioButtonPrintAfterSpooling);
            this.groupBoxSpool.Location = new System.Drawing.Point(365, 225);
            this.groupBoxSpool.Name = "groupBoxSpool";
            this.groupBoxSpool.Size = new System.Drawing.Size(301, 116);
            this.groupBoxSpool.TabIndex = 14;
            this.groupBoxSpool.TabStop = false;
            this.groupBoxSpool.Text = "Spool Print Documents";
            // 
            // labelSpoolingPrintDocuments
            // 
            this.labelSpoolingPrintDocuments.AutoSize = true;
            this.labelSpoolingPrintDocuments.Location = new System.Drawing.Point(21, 70);
            this.labelSpoolingPrintDocuments.MaximumSize = new System.Drawing.Size(280, 70);
            this.labelSpoolingPrintDocuments.Name = "labelSpoolingPrintDocuments";
            this.labelSpoolingPrintDocuments.Size = new System.Drawing.Size(228, 26);
            this.labelSpoolingPrintDocuments.TabIndex = 17;
            this.labelSpoolingPrintDocuments.Text = "NOTE : Start printing after last page spooled is recommended when using HP AC IPM" +
    "";
            // 
            // radioButtonPrintImmediately
            // 
            this.radioButtonPrintImmediately.AutoSize = true;
            this.radioButtonPrintImmediately.Location = new System.Drawing.Point(24, 42);
            this.radioButtonPrintImmediately.Name = "radioButtonPrintImmediately";
            this.radioButtonPrintImmediately.Size = new System.Drawing.Size(143, 17);
            this.radioButtonPrintImmediately.TabIndex = 16;
            this.radioButtonPrintImmediately.TabStop = true;
            this.radioButtonPrintImmediately.Text = "Start Printing Immediately";
            this.radioButtonPrintImmediately.UseVisualStyleBackColor = true;
            // 
            // radioButtonPrintAfterSpooling
            // 
            this.radioButtonPrintAfterSpooling.AutoSize = true;
            this.radioButtonPrintAfterSpooling.Location = new System.Drawing.Point(24, 19);
            this.radioButtonPrintAfterSpooling.Name = "radioButtonPrintAfterSpooling";
            this.radioButtonPrintAfterSpooling.Size = new System.Drawing.Size(207, 17);
            this.radioButtonPrintAfterSpooling.TabIndex = 15;
            this.radioButtonPrintAfterSpooling.TabStop = true;
            this.radioButtonPrintAfterSpooling.Text = "Start Printing after last page is Spooled";
            this.radioButtonPrintAfterSpooling.UseVisualStyleBackColor = true;
            // 
            // groupBoxBidi
            // 
            this.groupBoxBidi.Controls.Add(this.labelBidiCommunication);
            this.groupBoxBidi.Controls.Add(this.radioButtonDisableBidi);
            this.groupBoxBidi.Controls.Add(this.radioButtonEnableBidi);
            this.groupBoxBidi.Location = new System.Drawing.Point(22, 225);
            this.groupBoxBidi.Name = "groupBoxBidi";
            this.groupBoxBidi.Size = new System.Drawing.Size(317, 116);
            this.groupBoxBidi.TabIndex = 10;
            this.groupBoxBidi.TabStop = false;
            this.groupBoxBidi.Text = "Bidirectional Communication";
            // 
            // labelBidiCommunication
            // 
            this.labelBidiCommunication.AutoSize = true;
            this.labelBidiCommunication.Location = new System.Drawing.Point(18, 70);
            this.labelBidiCommunication.MaximumSize = new System.Drawing.Size(280, 70);
            this.labelBidiCommunication.Name = "labelBidiCommunication";
            this.labelBidiCommunication.Size = new System.Drawing.Size(275, 39);
            this.labelBidiCommunication.TabIndex = 13;
            this.labelBidiCommunication.Text = "NOTE : Disabling bi-directional support  is recommended because driver is not com" +
    "municating directly to a printer but with HPAC Server";
            // 
            // radioButtonDisableBidi
            // 
            this.radioButtonDisableBidi.AutoSize = true;
            this.radioButtonDisableBidi.Location = new System.Drawing.Point(21, 42);
            this.radioButtonDisableBidi.Name = "radioButtonDisableBidi";
            this.radioButtonDisableBidi.Size = new System.Drawing.Size(159, 17);
            this.radioButtonDisableBidi.TabIndex = 12;
            this.radioButtonDisableBidi.TabStop = true;
            this.radioButtonDisableBidi.Text = "Disable bidirectional Support";
            this.radioButtonDisableBidi.UseVisualStyleBackColor = true;
            // 
            // radioButtonEnableBidi
            // 
            this.radioButtonEnableBidi.AutoSize = true;
            this.radioButtonEnableBidi.Location = new System.Drawing.Point(21, 19);
            this.radioButtonEnableBidi.Name = "radioButtonEnableBidi";
            this.radioButtonEnableBidi.Size = new System.Drawing.Size(157, 17);
            this.radioButtonEnableBidi.TabIndex = 11;
            this.radioButtonEnableBidi.TabStop = true;
            this.radioButtonEnableBidi.Text = "Enable bidirectional Support";
            this.radioButtonEnableBidi.UseVisualStyleBackColor = true;
            // 
            // hpacClientInstallGroupBox
            // 
            this.hpacClientInstallGroupBox.Controls.Add(this.groupBoxServerName);
            this.hpacClientInstallGroupBox.Controls.Add(this.groupBoxHpacConfig);
            this.hpacClientInstallGroupBox.Controls.Add(this.clientInstallerPathlabel);
            this.hpacClientInstallGroupBox.Controls.Add(this.textBoxInstallerPath);
            this.hpacClientInstallGroupBox.Controls.Add(this.installerPathButton);
            this.hpacClientInstallGroupBox.Location = new System.Drawing.Point(25, 403);
            this.hpacClientInstallGroupBox.Name = "hpacClientInstallGroupBox";
            this.hpacClientInstallGroupBox.Size = new System.Drawing.Size(689, 192);
            this.hpacClientInstallGroupBox.TabIndex = 19;
            this.hpacClientInstallGroupBox.TabStop = false;
            this.hpacClientInstallGroupBox.Text = "HPAC Client Installation";
            // 
            // groupBoxServerName
            // 
            this.groupBoxServerName.Controls.Add(this.textBoxJAServerName);
            this.groupBoxServerName.Controls.Add(this.textBoxIPMServerName);
            this.groupBoxServerName.Controls.Add(this.textBoxPullPrintServerName);
            this.groupBoxServerName.Controls.Add(this.labelPullPrintServerName);
            this.groupBoxServerName.Controls.Add(this.labelIpmServerName);
            this.groupBoxServerName.Controls.Add(this.labelJaServerName);
            this.groupBoxServerName.Location = new System.Drawing.Point(22, 69);
            this.groupBoxServerName.Name = "groupBoxServerName";
            this.groupBoxServerName.Size = new System.Drawing.Size(496, 103);
            this.groupBoxServerName.TabIndex = 23;
            this.groupBoxServerName.TabStop = false;
            this.groupBoxServerName.Text = "Server Name Configuration";
            // 
            // textBoxJAServerName
            // 
            this.textBoxJAServerName.Location = new System.Drawing.Point(143, 19);
            this.textBoxJAServerName.Name = "textBoxJAServerName";
            this.textBoxJAServerName.Size = new System.Drawing.Size(343, 20);
            this.textBoxJAServerName.TabIndex = 25;
            // 
            // textBoxIPMServerName
            // 
            this.textBoxIPMServerName.Location = new System.Drawing.Point(143, 45);
            this.textBoxIPMServerName.Name = "textBoxIPMServerName";
            this.textBoxIPMServerName.Size = new System.Drawing.Size(343, 20);
            this.textBoxIPMServerName.TabIndex = 27;
            // 
            // textBoxPullPrintServerName
            // 
            this.textBoxPullPrintServerName.Location = new System.Drawing.Point(143, 71);
            this.textBoxPullPrintServerName.Name = "textBoxPullPrintServerName";
            this.textBoxPullPrintServerName.Size = new System.Drawing.Size(343, 20);
            this.textBoxPullPrintServerName.TabIndex = 29;
            // 
            // labelPullPrintServerName
            // 
            this.labelPullPrintServerName.AutoSize = true;
            this.labelPullPrintServerName.Location = new System.Drawing.Point(15, 78);
            this.labelPullPrintServerName.Name = "labelPullPrintServerName";
            this.labelPullPrintServerName.Size = new System.Drawing.Size(111, 13);
            this.labelPullPrintServerName.TabIndex = 28;
            this.labelPullPrintServerName.Text = "HPAC PullPrint Server";
            // 
            // labelIpmServerName
            // 
            this.labelIpmServerName.AutoSize = true;
            this.labelIpmServerName.Location = new System.Drawing.Point(34, 52);
            this.labelIpmServerName.Name = "labelIpmServerName";
            this.labelIpmServerName.Size = new System.Drawing.Size(92, 13);
            this.labelIpmServerName.TabIndex = 26;
            this.labelIpmServerName.Text = "HPAC IPM Server";
            // 
            // labelJaServerName
            // 
            this.labelJaServerName.AutoSize = true;
            this.labelJaServerName.Location = new System.Drawing.Point(41, 27);
            this.labelJaServerName.Name = "labelJaServerName";
            this.labelJaServerName.Size = new System.Drawing.Size(85, 13);
            this.labelJaServerName.TabIndex = 24;
            this.labelJaServerName.Text = "HPAC JA Server";
            // 
            // groupBoxHpacConfig
            // 
            this.groupBoxHpacConfig.Controls.Add(this.checkBoxJobStorage);
            this.groupBoxHpacConfig.Controls.Add(this.checkBoxDelegate);
            this.groupBoxHpacConfig.Controls.Add(this.checkBoxIpm);
            this.groupBoxHpacConfig.Controls.Add(this.checkBoxQuota);
            this.groupBoxHpacConfig.Location = new System.Drawing.Point(524, 69);
            this.groupBoxHpacConfig.Name = "groupBoxHpacConfig";
            this.groupBoxHpacConfig.Size = new System.Drawing.Size(142, 114);
            this.groupBoxHpacConfig.TabIndex = 30;
            this.groupBoxHpacConfig.TabStop = false;
            this.groupBoxHpacConfig.Text = "HPAC Configuration";
            // 
            // checkBoxJobStorage
            // 
            this.checkBoxJobStorage.AutoSize = true;
            this.checkBoxJobStorage.Location = new System.Drawing.Point(16, 88);
            this.checkBoxJobStorage.Name = "checkBoxJobStorage";
            this.checkBoxJobStorage.Size = new System.Drawing.Size(112, 17);
            this.checkBoxJobStorage.TabIndex = 34;
            this.checkBoxJobStorage.Text = "Local Job Storage";
            this.checkBoxJobStorage.UseVisualStyleBackColor = true;
            // 
            // checkBoxDelegate
            // 
            this.checkBoxDelegate.AutoSize = true;
            this.checkBoxDelegate.Location = new System.Drawing.Point(16, 65);
            this.checkBoxDelegate.Name = "checkBoxDelegate";
            this.checkBoxDelegate.Size = new System.Drawing.Size(69, 17);
            this.checkBoxDelegate.TabIndex = 33;
            this.checkBoxDelegate.Text = "Delegate";
            this.checkBoxDelegate.UseVisualStyleBackColor = true;
            // 
            // checkBoxIpm
            // 
            this.checkBoxIpm.AutoSize = true;
            this.checkBoxIpm.Location = new System.Drawing.Point(16, 42);
            this.checkBoxIpm.Name = "checkBoxIpm";
            this.checkBoxIpm.Size = new System.Drawing.Size(45, 17);
            this.checkBoxIpm.TabIndex = 32;
            this.checkBoxIpm.Text = "IPM";
            this.checkBoxIpm.UseVisualStyleBackColor = true;
            // 
            // checkBoxQuota
            // 
            this.checkBoxQuota.AutoSize = true;
            this.checkBoxQuota.Location = new System.Drawing.Point(16, 19);
            this.checkBoxQuota.Name = "checkBoxQuota";
            this.checkBoxQuota.Size = new System.Drawing.Size(55, 17);
            this.checkBoxQuota.TabIndex = 31;
            this.checkBoxQuota.Text = "Quota";
            this.checkBoxQuota.UseVisualStyleBackColor = true;
            // 
            // clientInstallerPathlabel
            // 
            this.clientInstallerPathlabel.AutoSize = true;
            this.clientInstallerPathlabel.Location = new System.Drawing.Point(33, 33);
            this.clientInstallerPathlabel.Name = "clientInstallerPathlabel";
            this.clientInstallerPathlabel.Size = new System.Drawing.Size(129, 13);
            this.clientInstallerPathlabel.TabIndex = 20;
            this.clientInstallerPathlabel.Text = "HPAC Client Installer Path";
            // 
            // checkBoxInstallClient
            // 
            this.checkBoxInstallClient.AutoSize = true;
            this.checkBoxInstallClient.Location = new System.Drawing.Point(25, 380);
            this.checkBoxInstallClient.Name = "checkBoxInstallClient";
            this.checkBoxInstallClient.Size = new System.Drawing.Size(114, 17);
            this.checkBoxInstallClient.TabIndex = 18;
            this.checkBoxInstallClient.Text = "Install HPAC Client";
            this.checkBoxInstallClient.UseVisualStyleBackColor = true;
            this.checkBoxInstallClient.CheckedChanged += new System.EventHandler(this.checkBoxInstallClient_CheckedChanged);
            // 
            // HpacClientConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxInstallClient);
            this.Controls.Add(this.hpacClientInstallGroupBox);
            this.Controls.Add(this.driverInstallGroupBox);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "HpacClientConfigurationControl";
            this.Size = new System.Drawing.Size(738, 608);
            this.driverInstallGroupBox.ResumeLayout(false);
            this.driverInstallGroupBox.PerformLayout();
            this.groupBoxQueueConfiguration.ResumeLayout(false);
            this.groupBoxQueueConfiguration.PerformLayout();
            this.groupBoxDriverSelection.ResumeLayout(false);
            this.groupBoxSpool.ResumeLayout(false);
            this.groupBoxSpool.PerformLayout();
            this.groupBoxBidi.ResumeLayout(false);
            this.groupBoxBidi.PerformLayout();
            this.hpacClientInstallGroupBox.ResumeLayout(false);
            this.hpacClientInstallGroupBox.PerformLayout();
            this.groupBoxServerName.ResumeLayout(false);
            this.groupBoxServerName.PerformLayout();
            this.groupBoxHpacConfig.ResumeLayout(false);
            this.groupBoxHpacConfig.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.ServerComboBox serverComboBoxHpac;
        private System.Windows.Forms.Label serverIplabel;
        private System.Windows.Forms.TextBox textBoxInstallerPath;
        private System.Windows.Forms.Button installerPathButton;
        private System.Windows.Forms.Label lprQueuelabel;
        private System.Windows.Forms.TextBox lprQueuetextBox;
        private System.Windows.Forms.GroupBox driverInstallGroupBox;
        private System.Windows.Forms.Label lprTipLabel;
        private System.Windows.Forms.GroupBox hpacClientInstallGroupBox;
        private System.Windows.Forms.GroupBox groupBoxServerName;
        private System.Windows.Forms.TextBox textBoxJAServerName;
        private System.Windows.Forms.TextBox textBoxIPMServerName;
        private System.Windows.Forms.TextBox textBoxPullPrintServerName;
        private System.Windows.Forms.Label labelPullPrintServerName;
        private System.Windows.Forms.Label labelIpmServerName;
        private System.Windows.Forms.Label labelJaServerName;
        private System.Windows.Forms.GroupBox groupBoxHpacConfig;
        private System.Windows.Forms.Label clientInstallerPathlabel;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.CheckBox checkBoxJobStorage;
        private System.Windows.Forms.CheckBox checkBoxDelegate;
        private System.Windows.Forms.CheckBox checkBoxIpm;
        private System.Windows.Forms.CheckBox checkBoxQuota;
        private System.Windows.Forms.GroupBox groupBoxSpool;
        private System.Windows.Forms.RadioButton radioButtonPrintImmediately;
        private System.Windows.Forms.RadioButton radioButtonPrintAfterSpooling;
        private System.Windows.Forms.GroupBox groupBoxBidi;
        private System.Windows.Forms.RadioButton radioButtonDisableBidi;
        private System.Windows.Forms.RadioButton radioButtonEnableBidi;
        private System.Windows.Forms.Label labelSpoolingPrintDocuments;
        private System.Windows.Forms.Label labelBidiCommunication;
        private Framework.UI.PrintDriverSelectionControl printDriverSelectionControl;
        private System.Windows.Forms.CheckBox checkBoxInstallClient;
        private System.Windows.Forms.GroupBox groupBoxQueueConfiguration;
        private System.Windows.Forms.GroupBox groupBoxDriverSelection;
        private System.Windows.Forms.CheckBox checkBoxDefaultPrinter;
    }
}
