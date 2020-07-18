namespace HP.ScalableTest.Plugin.Vpn
{
    partial class VpnConfigurationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VpnConfigurationControl));
            this.disconnect_radioButton = new System.Windows.Forms.RadioButton();
            this.connect_radioButton = new System.Windows.Forms.RadioButton();
            this.vpn_comboBox = new System.Windows.Forms.ComboBox();
            this.vpn_label = new System.Windows.Forms.Label();
            this.note_groupBox = new System.Windows.Forms.GroupBox();
            this.note_textBox = new System.Windows.Forms.TextBox();
            this.vpn_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.note_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // disconnect_radioButton
            // 
            this.disconnect_radioButton.AutoSize = true;
            this.disconnect_radioButton.Location = new System.Drawing.Point(109, 67);
            this.disconnect_radioButton.Name = "disconnect_radioButton";
            this.disconnect_radioButton.Size = new System.Drawing.Size(161, 17);
            this.disconnect_radioButton.TabIndex = 9;
            this.disconnect_radioButton.TabStop = true;
            this.disconnect_radioButton.Text = "Disconnect from VPN Server";
            this.disconnect_radioButton.UseVisualStyleBackColor = true;
            // 
            // connect_radioButton
            // 
            this.connect_radioButton.AutoSize = true;
            this.connect_radioButton.Location = new System.Drawing.Point(109, 44);
            this.connect_radioButton.Name = "connect_radioButton";
            this.connect_radioButton.Size = new System.Drawing.Size(136, 17);
            this.connect_radioButton.TabIndex = 8;
            this.connect_radioButton.TabStop = true;
            this.connect_radioButton.Text = "Connect to VPN Server";
            this.connect_radioButton.UseVisualStyleBackColor = true;
            // 
            // vpn_comboBox
            // 
            this.vpn_comboBox.FormattingEnabled = true;
            this.vpn_comboBox.Location = new System.Drawing.Point(109, 17);
            this.vpn_comboBox.Name = "vpn_comboBox";
            this.vpn_comboBox.Size = new System.Drawing.Size(240, 21);
            this.vpn_comboBox.TabIndex = 7;
            // 
            // vpn_label
            // 
            this.vpn_label.AutoSize = true;
            this.vpn_label.Location = new System.Drawing.Point(17, 20);
            this.vpn_label.Name = "vpn_label";
            this.vpn_label.Size = new System.Drawing.Size(86, 13);
            this.vpn_label.TabIndex = 14;
            this.vpn_label.Text = "VPN Connection";
            // 
            // note_groupBox
            // 
            this.note_groupBox.Controls.Add(this.textBox1);
            this.note_groupBox.Controls.Add(this.note_textBox);
            this.note_groupBox.Location = new System.Drawing.Point(3, 99);
            this.note_groupBox.Name = "note_groupBox";
            this.note_groupBox.Size = new System.Drawing.Size(368, 142);
            this.note_groupBox.TabIndex = 15;
            this.note_groupBox.TabStop = false;
            this.note_groupBox.Text = "NOTE";
            // 
            // note_textBox
            // 
            this.note_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.note_textBox.Location = new System.Drawing.Point(10, 19);
            this.note_textBox.Multiline = true;
            this.note_textBox.Name = "note_textBox";
            this.note_textBox.ReadOnly = true;
            this.note_textBox.Size = new System.Drawing.Size(336, 54);
            this.note_textBox.TabIndex = 11;
            this.note_textBox.Text = resources.GetString("note_textBox.Text");
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(10, 79);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(336, 54);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Please define the server in plugin settings with the term \"Serverxx\" and store th" +
    "e configuration value as serverip;domain\\username;password";
            // 
            // VpnConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.note_groupBox);
            this.Controls.Add(this.vpn_label);
            this.Controls.Add(this.disconnect_radioButton);
            this.Controls.Add(this.connect_radioButton);
            this.Controls.Add(this.vpn_comboBox);
            this.Name = "VpnConfigurationControl";
            this.Size = new System.Drawing.Size(384, 244);
            this.note_groupBox.ResumeLayout(false);
            this.note_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton disconnect_radioButton;
        private System.Windows.Forms.RadioButton connect_radioButton;
        private System.Windows.Forms.ComboBox vpn_comboBox;
        private System.Windows.Forms.Label vpn_label;
        private System.Windows.Forms.GroupBox note_groupBox;
        private System.Windows.Forms.TextBox note_textBox;
       
        private Framework.UI.FieldValidator vpn_fieldValidator;
        private System.Windows.Forms.TextBox textBox1;
    }
}
