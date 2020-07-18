namespace HP.ScalableTest.Plugin.EWSFirmwarePerformance
{
    partial class EWSFirmwarePerformanceConfigurationControl
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
            this.groupBoxfirmware = new System.Windows.Forms.GroupBox();
            this.mapFirmware_Button = new System.Windows.Forms.Button();
            this.validatetimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.checkBoxValidate = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoBackup = new System.Windows.Forms.CheckBox();
            this.textBoxFirmwareFolder = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.validateFW_Checkbox = new System.Windows.Forms.CheckBox();
            this.firmwareInfo_GridView = new System.Windows.Forms.DataGridView();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.usage_Label = new System.Windows.Forms.Label();
            this.groupBoxfirmware.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.firmwareInfo_GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxfirmware
            // 
            this.groupBoxfirmware.Controls.Add(this.mapFirmware_Button);
            this.groupBoxfirmware.Controls.Add(this.validatetimeSpanControl);
            this.groupBoxfirmware.Controls.Add(this.checkBoxValidate);
            this.groupBoxfirmware.Controls.Add(this.checkBoxAutoBackup);
            this.groupBoxfirmware.Controls.Add(this.textBoxFirmwareFolder);
            this.groupBoxfirmware.Controls.Add(this.buttonBrowse);
            this.groupBoxfirmware.Location = new System.Drawing.Point(4, 254);
            this.groupBoxfirmware.Name = "groupBoxfirmware";
            this.groupBoxfirmware.Size = new System.Drawing.Size(737, 85);
            this.groupBoxfirmware.TabIndex = 18;
            this.groupBoxfirmware.TabStop = false;
            this.groupBoxfirmware.Text = "Firmware File";
            // 
            // mapFirmware_Button
            // 
            this.mapFirmware_Button.Location = new System.Drawing.Point(612, 21);
            this.mapFirmware_Button.Name = "mapFirmware_Button";
            this.mapFirmware_Button.Size = new System.Drawing.Size(98, 24);
            this.mapFirmware_Button.TabIndex = 7;
            this.mapFirmware_Button.Text = "Map Firmware";
            this.mapFirmware_Button.UseVisualStyleBackColor = true;
            this.mapFirmware_Button.Click += new System.EventHandler(this.mapFirmware_Button_Click);
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
            this.checkBoxValidate.Checked = true;
            this.checkBoxValidate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxValidate.Location = new System.Drawing.Point(136, 51);
            this.checkBoxValidate.Name = "checkBoxValidate";
            this.checkBoxValidate.Size = new System.Drawing.Size(197, 19);
            this.checkBoxValidate.TabIndex = 4;
            this.checkBoxValidate.Text = "Validate after completion within:";
            this.checkBoxValidate.UseVisualStyleBackColor = true;
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
            // textBoxFirmwareFolder
            // 
            this.textBoxFirmwareFolder.BackColor = System.Drawing.Color.White;
            this.textBoxFirmwareFolder.Location = new System.Drawing.Point(6, 22);
            this.textBoxFirmwareFolder.Name = "textBoxFirmwareFolder";
            this.textBoxFirmwareFolder.ReadOnly = true;
            this.textBoxFirmwareFolder.Size = new System.Drawing.Size(527, 23);
            this.textBoxFirmwareFolder.TabIndex = 1;
            this.textBoxFirmwareFolder.TextChanged += new System.EventHandler(this.textBoxFirmwareFolder_TextChanged);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.validateFW_Checkbox);
            this.groupBox2.Controls.Add(this.firmwareInfo_GridView);
            this.groupBox2.Location = new System.Drawing.Point(4, 354);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(737, 271);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Firmware Details";
            // 
            // validateFW_Checkbox
            // 
            this.validateFW_Checkbox.AutoSize = true;
            this.validateFW_Checkbox.Checked = true;
            this.validateFW_Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.validateFW_Checkbox.Location = new System.Drawing.Point(7, 22);
            this.validateFW_Checkbox.Name = "validateFW_Checkbox";
            this.validateFW_Checkbox.Size = new System.Drawing.Size(534, 19);
            this.validateFW_Checkbox.TabIndex = 7;
            this.validateFW_Checkbox.Text = "Validate Firmware (checks FW to make sure it\'s still the same as when the plugin " +
    "was configured)";
            this.validateFW_Checkbox.UseVisualStyleBackColor = true;
            // 
            // firmwareInfo_GridView
            // 
            this.firmwareInfo_GridView.AllowUserToAddRows = false;
            this.firmwareInfo_GridView.AllowUserToDeleteRows = false;
            this.firmwareInfo_GridView.Location = new System.Drawing.Point(7, 49);
            this.firmwareInfo_GridView.Name = "firmwareInfo_GridView";
            this.firmwareInfo_GridView.ReadOnly = true;
            this.firmwareInfo_GridView.Size = new System.Drawing.Size(730, 222);
            this.firmwareInfo_GridView.TabIndex = 0;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 46);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(737, 202);
            this.assetSelectionControl.TabIndex = 17;
            // 
            // usage_Label
            // 
            this.usage_Label.AutoSize = true;
            this.usage_Label.Location = new System.Drawing.Point(3, 11);
            this.usage_Label.Name = "usage_Label";
            this.usage_Label.Size = new System.Drawing.Size(526, 15);
            this.usage_Label.TabIndex = 20;
            this.usage_Label.Text = "1. Select Devices Before Pressing the Map Firmware Button.      2. Select as many" +
    " workers as devices";
            // 
            // EWSFirmwarePerformanceConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.usage_Label);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxfirmware);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EWSFirmwarePerformanceConfigurationControl";
            this.Size = new System.Drawing.Size(744, 630);
            this.groupBoxfirmware.ResumeLayout(false);
            this.groupBoxfirmware.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.firmwareInfo_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBoxfirmware;
        private Framework.UI.TimeSpanControl validatetimeSpanControl;
        private System.Windows.Forms.CheckBox checkBoxValidate;
        private System.Windows.Forms.CheckBox checkBoxAutoBackup;
        private System.Windows.Forms.TextBox textBoxFirmwareFolder;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView firmwareInfo_GridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn fWModelNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fWBundleVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firmwareRevisionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firmwareDateCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.CheckBox validateFW_Checkbox;
        private System.Windows.Forms.Label usage_Label;
        private System.Windows.Forms.Button mapFirmware_Button;
    }
}
