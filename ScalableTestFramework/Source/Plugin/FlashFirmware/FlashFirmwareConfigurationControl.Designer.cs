namespace HP.ScalableTest.Plugin.FlashFirmware
{
    partial class FlashFirmwareConfigurationControl
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
            this.textBoxFirmwareFile = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.checkBoxAutoBackup = new System.Windows.Forms.CheckBox();
            this.groupBoxfirmware = new System.Windows.Forms.GroupBox();
            this.validatetimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.checkBoxValidate = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxModelName = new System.Windows.Forms.TextBox();
            this.labelFutureSmart = new System.Windows.Forms.Label();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.textBoxRevision = new System.Windows.Forms.TextBox();
            this.labelRevision = new System.Windows.Forms.Label();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.method_groupBox = new System.Windows.Forms.GroupBox();
            this.port_label = new System.Windows.Forms.Label();
            this.port_comboBox = new System.Windows.Forms.ComboBox();
            this.bios_radioButton = new System.Windows.Forms.RadioButton();
            this.controlPanel_radioButton = new System.Windows.Forms.RadioButton();
            this.ews_radioButton = new System.Windows.Forms.RadioButton();
            this.firmware_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBoxUpgrade = new System.Windows.Forms.GroupBox();
            this.radioButtonUpgrade = new System.Windows.Forms.RadioButton();
            this.radioButtonDowngrade = new System.Windows.Forms.RadioButton();
            this.groupBoxfirmware.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.method_groupBox.SuspendLayout();
            this.groupBoxUpgrade.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFirmwareFile
            // 
            this.textBoxFirmwareFile.BackColor = System.Drawing.Color.White;
            this.textBoxFirmwareFile.Location = new System.Drawing.Point(6, 22);
            this.textBoxFirmwareFile.Name = "textBoxFirmwareFile";
            this.textBoxFirmwareFile.ReadOnly = true;
            this.textBoxFirmwareFile.Size = new System.Drawing.Size(527, 23);
            this.textBoxFirmwareFile.TabIndex = 1;
            this.textBoxFirmwareFile.TextChanged += new System.EventHandler(this.textBoxFirmwareFile_TextChanged);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(539, 22);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(67, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // checkBoxAutoBackup
            // 
            this.checkBoxAutoBackup.AutoSize = true;
            this.checkBoxAutoBackup.Checked = true;
            this.checkBoxAutoBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoBackup.Location = new System.Drawing.Point(6, 51);
            this.checkBoxAutoBackup.Name = "checkBoxAutoBackup";
            this.checkBoxAutoBackup.Size = new System.Drawing.Size(124, 19);
            this.checkBoxAutoBackup.TabIndex = 3;
            this.checkBoxAutoBackup.Text = "Automatic Backup";
            this.checkBoxAutoBackup.UseVisualStyleBackColor = true;
            // 
            // groupBoxfirmware
            // 
            this.groupBoxfirmware.Controls.Add(this.validatetimeSpanControl);
            this.groupBoxfirmware.Controls.Add(this.checkBoxValidate);
            this.groupBoxfirmware.Controls.Add(this.checkBoxAutoBackup);
            this.groupBoxfirmware.Controls.Add(this.textBoxFirmwareFile);
            this.groupBoxfirmware.Controls.Add(this.buttonBrowse);
            this.groupBoxfirmware.Location = new System.Drawing.Point(3, 290);
            this.groupBoxfirmware.Name = "groupBoxfirmware";
            this.groupBoxfirmware.Size = new System.Drawing.Size(737, 85);
            this.groupBoxfirmware.TabIndex = 4;
            this.groupBoxfirmware.TabStop = false;
            this.groupBoxfirmware.Text = "Firmware File";
            // 
            // validatetimeSpanControl
            // 
            this.validatetimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.validatetimeSpanControl.Location = new System.Drawing.Point(336, 51);
            this.validatetimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.validatetimeSpanControl.Name = "validatetimeSpanControl";
            this.validatetimeSpanControl.Size = new System.Drawing.Size(95, 25);
            this.validatetimeSpanControl.TabIndex = 6;
            // 
            // checkBoxValidate
            // 
            this.checkBoxValidate.AutoSize = true;
            this.checkBoxValidate.Location = new System.Drawing.Point(136, 51);
            this.checkBoxValidate.Name = "checkBoxValidate";
            this.checkBoxValidate.Size = new System.Drawing.Size(197, 19);
            this.checkBoxValidate.TabIndex = 4;
            this.checkBoxValidate.Text = "Validate after completion within:";
            this.checkBoxValidate.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxModelName);
            this.groupBox2.Controls.Add(this.labelFutureSmart);
            this.groupBox2.Controls.Add(this.textBoxDate);
            this.groupBox2.Controls.Add(this.labelDate);
            this.groupBox2.Controls.Add(this.textBoxRevision);
            this.groupBox2.Controls.Add(this.labelRevision);
            this.groupBox2.Controls.Add(this.textBoxVersion);
            this.groupBox2.Controls.Add(this.labelVersion);
            this.groupBox2.Location = new System.Drawing.Point(3, 453);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(737, 102);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Firmware Details";
            // 
            // textBoxModelName
            // 
            this.textBoxModelName.BackColor = System.Drawing.Color.White;
            this.textBoxModelName.Location = new System.Drawing.Point(9, 46);
            this.textBoxModelName.Name = "textBoxModelName";
            this.textBoxModelName.ReadOnly = true;
            this.textBoxModelName.Size = new System.Drawing.Size(180, 23);
            this.textBoxModelName.TabIndex = 7;
            // 
            // labelFutureSmart
            // 
            this.labelFutureSmart.AutoSize = true;
            this.labelFutureSmart.Location = new System.Drawing.Point(6, 28);
            this.labelFutureSmart.Name = "labelFutureSmart";
            this.labelFutureSmart.Size = new System.Drawing.Size(76, 15);
            this.labelFutureSmart.TabIndex = 6;
            this.labelFutureSmart.Text = "Model Name";
            // 
            // textBoxDate
            // 
            this.textBoxDate.BackColor = System.Drawing.Color.White;
            this.textBoxDate.Location = new System.Drawing.Point(561, 46);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.ReadOnly = true;
            this.textBoxDate.Size = new System.Drawing.Size(134, 23);
            this.textBoxDate.TabIndex = 5;
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(563, 28);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(114, 15);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "Firmware Date Code";
            // 
            // textBoxRevision
            // 
            this.textBoxRevision.BackColor = System.Drawing.Color.White;
            this.textBoxRevision.Location = new System.Drawing.Point(397, 46);
            this.textBoxRevision.Name = "textBoxRevision";
            this.textBoxRevision.ReadOnly = true;
            this.textBoxRevision.Size = new System.Drawing.Size(134, 23);
            this.textBoxRevision.TabIndex = 3;
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.Location = new System.Drawing.Point(398, 28);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Size = new System.Drawing.Size(103, 15);
            this.labelRevision.TabIndex = 2;
            this.labelRevision.Text = "Firmware Revision";
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.BackColor = System.Drawing.Color.White;
            this.textBoxVersion.Location = new System.Drawing.Point(219, 46);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.ReadOnly = true;
            this.textBoxVersion.Size = new System.Drawing.Size(148, 23);
            this.textBoxVersion.TabIndex = 1;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(220, 28);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(137, 15);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Firmware Bundle Version";
            // 
            // method_groupBox
            // 
            this.method_groupBox.Controls.Add(this.groupBoxUpgrade);
            this.method_groupBox.Controls.Add(this.port_label);
            this.method_groupBox.Controls.Add(this.port_comboBox);
            this.method_groupBox.Controls.Add(this.bios_radioButton);
            this.method_groupBox.Controls.Add(this.controlPanel_radioButton);
            this.method_groupBox.Controls.Add(this.ews_radioButton);
            this.method_groupBox.Location = new System.Drawing.Point(3, 380);
            this.method_groupBox.Name = "method_groupBox";
            this.method_groupBox.Size = new System.Drawing.Size(737, 69);
            this.method_groupBox.TabIndex = 6;
            this.method_groupBox.TabStop = false;
            this.method_groupBox.Text = "Firmware Update Method";
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(295, 13);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(97, 15);
            this.port_label.TabIndex = 5;
            this.port_label.Text = "Telnet/COM Port";
            // 
            // port_comboBox
            // 
            this.port_comboBox.FormattingEnabled = true;
            this.port_comboBox.Location = new System.Drawing.Point(296, 31);
            this.port_comboBox.Name = "port_comboBox";
            this.port_comboBox.Size = new System.Drawing.Size(96, 23);
            this.port_comboBox.TabIndex = 4;
            // 
            // bios_radioButton
            // 
            this.bios_radioButton.AutoSize = true;
            this.bios_radioButton.Location = new System.Drawing.Point(208, 31);
            this.bios_radioButton.Name = "bios_radioButton";
            this.bios_radioButton.Size = new System.Drawing.Size(82, 19);
            this.bios_radioButton.TabIndex = 2;
            this.bios_radioButton.TabStop = true;
            this.bios_radioButton.Text = "USB - Bios ";
            this.bios_radioButton.UseVisualStyleBackColor = true;
            // 
            // controlPanel_radioButton
            // 
            this.controlPanel_radioButton.AutoSize = true;
            this.controlPanel_radioButton.Location = new System.Drawing.Point(73, 31);
            this.controlPanel_radioButton.Name = "controlPanel_radioButton";
            this.controlPanel_radioButton.Size = new System.Drawing.Size(129, 19);
            this.controlPanel_radioButton.TabIndex = 1;
            this.controlPanel_radioButton.TabStop = true;
            this.controlPanel_radioButton.Text = "USB - Control Panel";
            this.controlPanel_radioButton.UseVisualStyleBackColor = true;
            // 
            // ews_radioButton
            // 
            this.ews_radioButton.AutoSize = true;
            this.ews_radioButton.Checked = true;
            this.ews_radioButton.Location = new System.Drawing.Point(9, 31);
            this.ews_radioButton.Name = "ews_radioButton";
            this.ews_radioButton.Size = new System.Drawing.Size(48, 19);
            this.ews_radioButton.TabIndex = 0;
            this.ews_radioButton.TabStop = true;
            this.ews_radioButton.Text = "EWS";
            this.ews_radioButton.UseVisualStyleBackColor = true;
            // 
            // firmware_assetSelectionControl
            // 
            this.firmware_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firmware_assetSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.firmware_assetSelectionControl.Name = "firmware_assetSelectionControl";
            this.firmware_assetSelectionControl.Size = new System.Drawing.Size(737, 265);
            this.firmware_assetSelectionControl.TabIndex = 0;
            // 
            // groupBoxUpgrade
            // 
            this.groupBoxUpgrade.Controls.Add(this.radioButtonDowngrade);
            this.groupBoxUpgrade.Controls.Add(this.radioButtonUpgrade);
            this.groupBoxUpgrade.Location = new System.Drawing.Point(444, 13);
            this.groupBoxUpgrade.Name = "groupBoxUpgrade";
            this.groupBoxUpgrade.Size = new System.Drawing.Size(287, 50);
            this.groupBoxUpgrade.TabIndex = 6;
            this.groupBoxUpgrade.TabStop = false;
            // 
            // radioButtonUpgrade
            // 
            this.radioButtonUpgrade.AutoSize = true;
            this.radioButtonUpgrade.Checked = true;
            this.radioButtonUpgrade.Location = new System.Drawing.Point(6, 19);
            this.radioButtonUpgrade.Name = "radioButtonUpgrade";
            this.radioButtonUpgrade.Size = new System.Drawing.Size(70, 19);
            this.radioButtonUpgrade.TabIndex = 0;
            this.radioButtonUpgrade.TabStop = true;
            this.radioButtonUpgrade.Text = "Upgrade";
            this.radioButtonUpgrade.UseVisualStyleBackColor = true;
            // 
            // radioButtonDowngrade
            // 
            this.radioButtonDowngrade.AutoSize = true;
            this.radioButtonDowngrade.Location = new System.Drawing.Point(117, 18);
            this.radioButtonDowngrade.Name = "radioButtonDowngrade";
            this.radioButtonDowngrade.Size = new System.Drawing.Size(86, 19);
            this.radioButtonDowngrade.TabIndex = 1;
            this.radioButtonDowngrade.TabStop = true;
            this.radioButtonDowngrade.Text = "Downgrade";
            this.radioButtonDowngrade.UseVisualStyleBackColor = true;
            // 
            // FlashFirmwareConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.method_groupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxfirmware);
            this.Controls.Add(this.firmware_assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FlashFirmwareConfigurationControl";
            this.Size = new System.Drawing.Size(743, 560);
            this.groupBoxfirmware.ResumeLayout(false);
            this.groupBoxfirmware.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.method_groupBox.ResumeLayout(false);
            this.method_groupBox.PerformLayout();
            this.groupBoxUpgrade.ResumeLayout(false);
            this.groupBoxUpgrade.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl firmware_assetSelectionControl;
        private System.Windows.Forms.TextBox textBoxFirmwareFile;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.CheckBox checkBoxAutoBackup;
        private System.Windows.Forms.GroupBox groupBoxfirmware;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxModelName;
        private System.Windows.Forms.Label labelFutureSmart;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.TextBox textBoxRevision;
        private System.Windows.Forms.Label labelRevision;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.CheckBox checkBoxValidate;
        private Framework.UI.TimeSpanControl validatetimeSpanControl;
        private System.Windows.Forms.GroupBox method_groupBox;
        private System.Windows.Forms.RadioButton bios_radioButton;
        private System.Windows.Forms.RadioButton controlPanel_radioButton;
        private System.Windows.Forms.RadioButton ews_radioButton;
        private System.Windows.Forms.ComboBox port_comboBox;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.GroupBox groupBoxUpgrade;
        private System.Windows.Forms.RadioButton radioButtonDowngrade;
        private System.Windows.Forms.RadioButton radioButtonUpgrade;
    }
}
