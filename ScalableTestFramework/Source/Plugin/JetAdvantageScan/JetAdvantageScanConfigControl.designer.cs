namespace HP.ScalableTest.Plugin.JetAdvantageScan
{
    partial class JetAdvantageScanConfigControl
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
            this.deviceId_Label = new System.Windows.Forms.Label();
            this.oxpdPassword_textBox = new System.Windows.Forms.TextBox();
            this.pluginDescription_Label = new System.Windows.Forms.Label();
            this.oxpdPassword_label = new System.Windows.Forms.Label();
            this.loginId_Label = new System.Windows.Forms.Label();
            this.titanLoginId_TextBox = new System.Windows.Forms.TextBox();
            this.password_label = new System.Windows.Forms.Label();
            this.titanPassword_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.Setting_groupBox = new System.Windows.Forms.GroupBox();
            this.filetype_comboBox = new System.Windows.Forms.ComboBox();
            this.filetype_label = new System.Windows.Forms.Label();
            this.duplexmode_comboBox = new System.Windows.Forms.ComboBox();
            this.duplexmode_label = new System.Windows.Forms.Label();
            this.orientation_label = new System.Windows.Forms.Label();
            this.paperSize_label = new System.Windows.Forms.Label();
            this.orientation_comboBox = new System.Windows.Forms.ComboBox();
            this.paperSize_comboBox = new System.Windows.Forms.ComboBox();
            this.loginPin_CheckBox = new System.Windows.Forms.CheckBox();
            this.loginPin_TextBox = new System.Windows.Forms.TextBox();
            this.pinDescription_label = new System.Windows.Forms.Label();
            this.loginPin_label = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.Setting_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceId_Label
            // 
            this.deviceId_Label.AutoSize = true;
            this.deviceId_Label.Location = new System.Drawing.Point(15, 9);
            this.deviceId_Label.Name = "deviceId_Label";
            this.deviceId_Label.Size = new System.Drawing.Size(88, 13);
            this.deviceId_Label.TabIndex = 0;
            this.deviceId_Label.Text = "Device Selection";
            // 
            // oxpdPassword_textBox
            // 
            this.oxpdPassword_textBox.Location = new System.Drawing.Point(119, 431);
            this.oxpdPassword_textBox.Name = "oxpdPassword_textBox";
            this.oxpdPassword_textBox.Size = new System.Drawing.Size(223, 20);
            this.oxpdPassword_textBox.TabIndex = 14;
            // 
            // pluginDescription_Label
            // 
            this.pluginDescription_Label.AutoSize = true;
            this.pluginDescription_Label.Location = new System.Drawing.Point(3, 395);
            this.pluginDescription_Label.Name = "pluginDescription_Label";
            this.pluginDescription_Label.Size = new System.Drawing.Size(389, 13);
            this.pluginDescription_Label.TabIndex = 11;
            this.pluginDescription_Label.Text = "This plugin will log into HPJetAdvantage enabled device and Scan the document";
            // 
            // oxpdPassword_label
            // 
            this.oxpdPassword_label.AutoSize = true;
            this.oxpdPassword_label.Location = new System.Drawing.Point(7, 434);
            this.oxpdPassword_label.Name = "oxpdPassword_label";
            this.oxpdPassword_label.Size = new System.Drawing.Size(87, 13);
            this.oxpdPassword_label.TabIndex = 10;
            this.oxpdPassword_label.Text = "OXPd Password:";
            // 
            // loginId_Label
            // 
            this.loginId_Label.AutoSize = true;
            this.loginId_Label.Location = new System.Drawing.Point(7, 469);
            this.loginId_Label.Name = "loginId_Label";
            this.loginId_Label.Size = new System.Drawing.Size(75, 13);
            this.loginId_Label.TabIndex = 15;
            this.loginId_Label.Text = "Titan Login Id:";
            // 
            // titanLoginId_TextBox
            // 
            this.titanLoginId_TextBox.Location = new System.Drawing.Point(119, 466);
            this.titanLoginId_TextBox.Name = "titanLoginId_TextBox";
            this.titanLoginId_TextBox.Size = new System.Drawing.Size(223, 20);
            this.titanLoginId_TextBox.TabIndex = 16;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(3, 499);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(83, 13);
            this.password_label.TabIndex = 17;
            this.password_label.Text = "Titan Password:";
            // 
            // titanPassword_TextBox
            // 
            this.titanPassword_TextBox.Location = new System.Drawing.Point(119, 496);
            this.titanPassword_TextBox.Name = "titanPassword_TextBox";
            this.titanPassword_TextBox.Size = new System.Drawing.Size(223, 20);
            this.titanPassword_TextBox.TabIndex = 18;
            this.titanPassword_TextBox.Text = "16CycleCross";
            // 
            // Setting_groupBox
            // 
            this.Setting_groupBox.Controls.Add(this.filetype_comboBox);
            this.Setting_groupBox.Controls.Add(this.filetype_label);
            this.Setting_groupBox.Controls.Add(this.duplexmode_comboBox);
            this.Setting_groupBox.Controls.Add(this.duplexmode_label);
            this.Setting_groupBox.Controls.Add(this.orientation_label);
            this.Setting_groupBox.Controls.Add(this.paperSize_label);
            this.Setting_groupBox.Controls.Add(this.orientation_comboBox);
            this.Setting_groupBox.Controls.Add(this.paperSize_comboBox);
            this.Setting_groupBox.Location = new System.Drawing.Point(18, 228);
            this.Setting_groupBox.Name = "Setting_groupBox";
            this.Setting_groupBox.Size = new System.Drawing.Size(499, 140);
            this.Setting_groupBox.TabIndex = 19;
            this.Setting_groupBox.TabStop = false;
            this.Setting_groupBox.Text = "Settings";
            // 
            // filetype_comboBox
            // 
            this.filetype_comboBox.FormattingEnabled = true;
            this.filetype_comboBox.Items.AddRange(new object[] {
            "Installed",
            "NotInstalled"});
            this.filetype_comboBox.Location = new System.Drawing.Point(243, 101);
            this.filetype_comboBox.Name = "filetype_comboBox";
            this.filetype_comboBox.Size = new System.Drawing.Size(156, 21);
            this.filetype_comboBox.TabIndex = 15;
            // 
            // filetype_label
            // 
            this.filetype_label.AutoSize = true;
            this.filetype_label.Location = new System.Drawing.Point(248, 85);
            this.filetype_label.Name = "filetype_label";
            this.filetype_label.Size = new System.Drawing.Size(50, 13);
            this.filetype_label.TabIndex = 14;
            this.filetype_label.Text = "File Type";
            this.filetype_label.Visible = false;
            // 
            // duplexmode_comboBox
            // 
            this.duplexmode_comboBox.FormattingEnabled = true;
            this.duplexmode_comboBox.Location = new System.Drawing.Point(9, 101);
            this.duplexmode_comboBox.Name = "duplexmode_comboBox";
            this.duplexmode_comboBox.Size = new System.Drawing.Size(156, 21);
            this.duplexmode_comboBox.TabIndex = 10;
            // 
            // duplexmode_label
            // 
            this.duplexmode_label.AutoSize = true;
            this.duplexmode_label.Location = new System.Drawing.Point(6, 85);
            this.duplexmode_label.Name = "duplexmode_label";
            this.duplexmode_label.Size = new System.Drawing.Size(33, 13);
            this.duplexmode_label.TabIndex = 9;
            this.duplexmode_label.Text = "Sides";
            // 
            // orientation_label
            // 
            this.orientation_label.AutoSize = true;
            this.orientation_label.Location = new System.Drawing.Point(240, 25);
            this.orientation_label.Name = "orientation_label";
            this.orientation_label.Size = new System.Drawing.Size(58, 13);
            this.orientation_label.TabIndex = 5;
            this.orientation_label.Text = "Orientation";
            // 
            // paperSize_label
            // 
            this.paperSize_label.AutoSize = true;
            this.paperSize_label.Location = new System.Drawing.Point(6, 25);
            this.paperSize_label.Name = "paperSize_label";
            this.paperSize_label.Size = new System.Drawing.Size(58, 13);
            this.paperSize_label.TabIndex = 4;
            this.paperSize_label.Text = "Paper Size";
            // 
            // orientation_comboBox
            // 
            this.orientation_comboBox.FormattingEnabled = true;
            this.orientation_comboBox.Location = new System.Drawing.Point(243, 41);
            this.orientation_comboBox.Name = "orientation_comboBox";
            this.orientation_comboBox.Size = new System.Drawing.Size(156, 21);
            this.orientation_comboBox.TabIndex = 3;
            // 
            // paperSize_comboBox
            // 
            this.paperSize_comboBox.FormattingEnabled = true;
            this.paperSize_comboBox.Location = new System.Drawing.Point(9, 41);
            this.paperSize_comboBox.Name = "paperSize_comboBox";
            this.paperSize_comboBox.Size = new System.Drawing.Size(156, 21);
            this.paperSize_comboBox.TabIndex = 0;
            // 
            // loginPin_CheckBox
            // 
            this.loginPin_CheckBox.AutoSize = true;
            this.loginPin_CheckBox.Location = new System.Drawing.Point(422, 430);
            this.loginPin_CheckBox.Name = "loginPin_CheckBox";
            this.loginPin_CheckBox.Size = new System.Drawing.Size(95, 17);
            this.loginPin_CheckBox.TabIndex = 21;
            this.loginPin_CheckBox.Text = "Use Login PIN";
            this.loginPin_CheckBox.UseVisualStyleBackColor = true;
            // 
            // loginPin_TextBox
            // 
            this.loginPin_TextBox.Enabled = false;
            this.loginPin_TextBox.Location = new System.Drawing.Point(417, 466);
            this.loginPin_TextBox.MaxLength = 8;
            this.loginPin_TextBox.Name = "loginPin_TextBox";
            this.loginPin_TextBox.ShortcutsEnabled = false;
            this.loginPin_TextBox.Size = new System.Drawing.Size(100, 20);
            this.loginPin_TextBox.TabIndex = 26;
            this.loginPin_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.loginPin_TextBox_KeyPress);
            // 
            // pinDescription_label
            // 
            this.pinDescription_label.AutoSize = true;
            this.pinDescription_label.Location = new System.Drawing.Point(385, 496);
            this.pinDescription_label.Name = "pinDescription_label";
            this.pinDescription_label.Size = new System.Drawing.Size(132, 13);
            this.pinDescription_label.TabIndex = 25;
            this.pinDescription_label.Text = "numeric(0-9), 8 chars Max.";
            // 
            // loginPin_label
            // 
            this.loginPin_label.AutoSize = true;
            this.loginPin_label.Location = new System.Drawing.Point(357, 469);
            this.loginPin_label.Name = "loginPin_label";
            this.loginPin_label.Size = new System.Drawing.Size(60, 13);
            this.loginPin_label.TabIndex = 24;
            this.loginPin_label.Text = "Login PIN :";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(18, 25);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(499, 197);
            this.assetSelectionControl.TabIndex = 27;
            // 
            // JetAdvantageScanConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.loginPin_TextBox);
            this.Controls.Add(this.pinDescription_label);
            this.Controls.Add(this.loginPin_label);
            this.Controls.Add(this.loginPin_CheckBox);
            this.Controls.Add(this.Setting_groupBox);
            this.Controls.Add(this.titanPassword_TextBox);
            this.Controls.Add(this.password_label);
            this.Controls.Add(this.titanLoginId_TextBox);
            this.Controls.Add(this.loginId_Label);
            this.Controls.Add(this.oxpdPassword_textBox);
            this.Controls.Add(this.pluginDescription_Label);
            this.Controls.Add(this.oxpdPassword_label);
            this.Controls.Add(this.deviceId_Label);
            this.Name = "JetAdvantageScanConfigControl";
            this.Size = new System.Drawing.Size(538, 534);
            this.Setting_groupBox.ResumeLayout(false);
            this.Setting_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label deviceId_Label;
        
        private System.Windows.Forms.TextBox oxpdPassword_textBox;
        private System.Windows.Forms.Label pluginDescription_Label;
        private System.Windows.Forms.Label oxpdPassword_label;
        private System.Windows.Forms.Label loginId_Label;
        private System.Windows.Forms.TextBox titanLoginId_TextBox;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.TextBox titanPassword_TextBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.ComboBox paperSize_comboBox;
        private System.Windows.Forms.ComboBox orientation_comboBox;
        private System.Windows.Forms.Label paperSize_label;
        private System.Windows.Forms.Label orientation_label;
        private System.Windows.Forms.Label duplexmode_label;
        private System.Windows.Forms.ComboBox duplexmode_comboBox;
        private System.Windows.Forms.Label filetype_label;
        private System.Windows.Forms.ComboBox filetype_comboBox;
        private System.Windows.Forms.GroupBox Setting_groupBox;
        private System.Windows.Forms.CheckBox loginPin_CheckBox;
        private System.Windows.Forms.TextBox loginPin_TextBox;
        private System.Windows.Forms.Label pinDescription_label;
        private System.Windows.Forms.Label loginPin_label;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
    }
}
