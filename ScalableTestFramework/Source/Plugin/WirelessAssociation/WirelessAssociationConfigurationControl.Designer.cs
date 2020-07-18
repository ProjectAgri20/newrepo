namespace HP.ScalableTest.Plugin.WirelessAssociation
{
    partial class WirelessAssociationConfigurationControl
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
            this.PowerCycle_checkBox = new System.Windows.Forms.CheckBox();
            this.wireless_groupBox = new System.Windows.Forms.GroupBox();
            this.passphrase_label = new System.Windows.Forms.Label();
            this.authentication_label = new System.Windows.Forms.Label();
            this.Hex_textBox = new System.Windows.Forms.TextBox();
            this.SSID_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.passPhrase_textBox = new System.Windows.Forms.TextBox();
            this.WPA2Authentication_comboBox = new System.Windows.Forms.ComboBox();
            this.ssid_label = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.wireless_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.label1 = new System.Windows.Forms.Label();
            this.wireless_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.wireless_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // PowerCycle_checkBox
            // 
            this.PowerCycle_checkBox.AutoSize = true;
            this.PowerCycle_checkBox.Location = new System.Drawing.Point(43, 153);
            this.PowerCycle_checkBox.Name = "PowerCycle_checkBox";
            this.PowerCycle_checkBox.Size = new System.Drawing.Size(129, 19);
            this.PowerCycle_checkBox.TabIndex = 47;
            this.PowerCycle_checkBox.Text = "PowerCycle Printer";
            this.PowerCycle_checkBox.UseVisualStyleBackColor = true;
            // 
            // wireless_groupBox
            // 
            this.wireless_groupBox.Controls.Add(this.passphrase_label);
            this.wireless_groupBox.Controls.Add(this.authentication_label);
            this.wireless_groupBox.Controls.Add(this.Hex_textBox);
            this.wireless_groupBox.Controls.Add(this.SSID_textBox);
            this.wireless_groupBox.Controls.Add(this.label7);
            this.wireless_groupBox.Controls.Add(this.passPhrase_textBox);
            this.wireless_groupBox.Controls.Add(this.WPA2Authentication_comboBox);
            this.wireless_groupBox.Controls.Add(this.ssid_label);
            this.wireless_groupBox.Controls.Add(this.label4);
            this.wireless_groupBox.Location = new System.Drawing.Point(43, 178);
            this.wireless_groupBox.Name = "wireless_groupBox";
            this.wireless_groupBox.Size = new System.Drawing.Size(566, 186);
            this.wireless_groupBox.TabIndex = 67;
            this.wireless_groupBox.TabStop = false;
            this.wireless_groupBox.Text = "Wireless Access Point Details";
            // 
            // passphrase_label
            // 
            this.passphrase_label.AutoSize = true;
            this.passphrase_label.Location = new System.Drawing.Point(339, 91);
            this.passphrase_label.Name = "passphrase_label";
            this.passphrase_label.Size = new System.Drawing.Size(0, 15);
            this.passphrase_label.TabIndex = 56;
            // 
            // authentication_label
            // 
            this.authentication_label.AutoSize = true;
            this.authentication_label.Location = new System.Drawing.Point(57, 39);
            this.authentication_label.Name = "authentication_label";
            this.authentication_label.Size = new System.Drawing.Size(84, 15);
            this.authentication_label.TabIndex = 48;
            this.authentication_label.Text = "Authentication";
            // 
            // Hex_textBox
            // 
            this.Hex_textBox.Enabled = false;
            this.Hex_textBox.Location = new System.Drawing.Point(152, 117);
            this.Hex_textBox.Name = "Hex_textBox";
            this.Hex_textBox.Size = new System.Drawing.Size(171, 21);
            this.Hex_textBox.TabIndex = 53;
            // 
            // SSID_textBox
            // 
            this.SSID_textBox.Location = new System.Drawing.Point(152, 64);
            this.SSID_textBox.Name = "SSID_textBox";
            this.SSID_textBox.Size = new System.Drawing.Size(171, 21);
            this.SSID_textBox.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 15);
            this.label7.TabIndex = 51;
            this.label7.Text = "Hex Passphrase";
            // 
            // passPhrase_textBox
            // 
            this.passPhrase_textBox.Location = new System.Drawing.Point(152, 90);
            this.passPhrase_textBox.Name = "passPhrase_textBox";
            this.passPhrase_textBox.Size = new System.Drawing.Size(171, 21);
            this.passPhrase_textBox.TabIndex = 1;
            this.passPhrase_textBox.TextChanged += new System.EventHandler(this.ConvertTo64HexPassphrase);
            // 
            // WPA2Authentication_comboBox
            // 
            this.WPA2Authentication_comboBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.WPA2Authentication_comboBox.FormattingEnabled = true;
            this.WPA2Authentication_comboBox.Items.AddRange(new object[] {
            "WPA2 with AES",
            "WPA2 with 64HEX",
            "WPA2 with AUTO"});
            this.WPA2Authentication_comboBox.Location = new System.Drawing.Point(152, 36);
            this.WPA2Authentication_comboBox.Name = "WPA2Authentication_comboBox";
            this.WPA2Authentication_comboBox.Size = new System.Drawing.Size(171, 23);
            this.WPA2Authentication_comboBox.TabIndex = 49;
            this.WPA2Authentication_comboBox.SelectedIndexChanged += new System.EventHandler(this.WPA2Authentication_comboBox_SelectedIndexChanged);
            // 
            // ssid_label
            // 
            this.ssid_label.AutoSize = true;
            this.ssid_label.Location = new System.Drawing.Point(106, 66);
            this.ssid_label.Name = "ssid_label";
            this.ssid_label.Size = new System.Drawing.Size(35, 15);
            this.ssid_label.TabIndex = 2;
            this.ssid_label.Text = "SSID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "PassPhrase";
            // 
            // wireless_assetSelectionControl
            // 
            this.wireless_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wireless_assetSelectionControl.Location = new System.Drawing.Point(43, 3);
            this.wireless_assetSelectionControl.Name = "wireless_assetSelectionControl";
            this.wireless_assetSelectionControl.Size = new System.Drawing.Size(566, 144);
            this.wireless_assetSelectionControl.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(425, 15);
            this.label1.TabIndex = 57;
            this.label1.Text = "Note: Only the first device will be selected for the test, choose only one device" +
    ".";
            // 
            // WirelessAssociationConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PowerCycle_checkBox);
            this.Controls.Add(this.wireless_assetSelectionControl);
            this.Controls.Add(this.wireless_groupBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WirelessAssociationConfigurationControl";
            this.Size = new System.Drawing.Size(675, 390);
            this.wireless_groupBox.ResumeLayout(false);
            this.wireless_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox PowerCycle_checkBox;
        private System.Windows.Forms.GroupBox wireless_groupBox;
        private System.Windows.Forms.Label passphrase_label;
        private System.Windows.Forms.Label authentication_label;
        private System.Windows.Forms.TextBox Hex_textBox;
        private System.Windows.Forms.TextBox SSID_textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox passPhrase_textBox;
        private System.Windows.Forms.ComboBox WPA2Authentication_comboBox;
        private System.Windows.Forms.Label ssid_label;
        private System.Windows.Forms.Label label4;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl wireless_assetSelectionControl;
        private System.Windows.Forms.Label label1;
        private Framework.UI.FieldValidator wireless_fieldValidator;
    }
}
