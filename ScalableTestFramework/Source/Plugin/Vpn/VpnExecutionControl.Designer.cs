namespace HP.ScalableTest.Plugin.Vpn
{
    partial class VpnExecutionControl
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
            this.currentVPN_label = new System.Windows.Forms.Label();
            this.vpnname_textBox = new System.Windows.Forms.TextBox();
            this.action_groupBox = new System.Windows.Forms.GroupBox();
            this.status_textBox = new System.Windows.Forms.TextBox();
            this.status_label = new System.Windows.Forms.Label();
            this.log_textBox = new System.Windows.Forms.TextBox();
            this.log_label = new System.Windows.Forms.Label();
            this.action_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentVPN_label
            // 
            this.currentVPN_label.AutoSize = true;
            this.currentVPN_label.Location = new System.Drawing.Point(22, 19);
            this.currentVPN_label.Name = "currentVPN_label";
            this.currentVPN_label.Size = new System.Drawing.Size(66, 13);
            this.currentVPN_label.TabIndex = 4;
            this.currentVPN_label.Text = "Current VPN";
            // 
            // vpnname_textBox
            // 
            this.vpnname_textBox.BackColor = System.Drawing.Color.White;
            this.vpnname_textBox.Location = new System.Drawing.Point(94, 16);
            this.vpnname_textBox.Name = "vpnname_textBox";
            this.vpnname_textBox.ReadOnly = true;
            this.vpnname_textBox.Size = new System.Drawing.Size(182, 20);
            this.vpnname_textBox.TabIndex = 5;
            // 
            // action_groupBox
            // 
            this.action_groupBox.Controls.Add(this.status_textBox);
            this.action_groupBox.Controls.Add(this.status_label);
            this.action_groupBox.Controls.Add(this.currentVPN_label);
            this.action_groupBox.Controls.Add(this.vpnname_textBox);
            this.action_groupBox.Location = new System.Drawing.Point(50, 14);
            this.action_groupBox.Name = "action_groupBox";
            this.action_groupBox.Size = new System.Drawing.Size(352, 87);
            this.action_groupBox.TabIndex = 8;
            this.action_groupBox.TabStop = false;
            this.action_groupBox.Text = "Activity";
            // 
            // status_textBox
            // 
            this.status_textBox.Location = new System.Drawing.Point(94, 51);
            this.status_textBox.Name = "status_textBox";
            this.status_textBox.Size = new System.Drawing.Size(182, 20);
            this.status_textBox.TabIndex = 7;
            // 
            // status_label
            // 
            this.status_label.AutoSize = true;
            this.status_label.Location = new System.Drawing.Point(22, 51);
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(37, 13);
            this.status_label.TabIndex = 6;
            this.status_label.Text = "Status";
            // 
            // log_textBox
            // 
            this.log_textBox.BackColor = System.Drawing.Color.White;
            this.log_textBox.Location = new System.Drawing.Point(50, 131);
            this.log_textBox.Multiline = true;
            this.log_textBox.Name = "log_textBox";
            this.log_textBox.ReadOnly = true;
            this.log_textBox.Size = new System.Drawing.Size(352, 65);
            this.log_textBox.TabIndex = 9;
            // 
            // log_label
            // 
            this.log_label.AutoSize = true;
            this.log_label.Location = new System.Drawing.Point(47, 115);
            this.log_label.Name = "log_label";
            this.log_label.Size = new System.Drawing.Size(25, 13);
            this.log_label.TabIndex = 10;
            this.log_label.Text = "Log";
            // 
            // VpnExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.log_label);
            this.Controls.Add(this.log_textBox);
            this.Controls.Add(this.action_groupBox);
            this.Name = "VPNExecutionControl";
            this.Size = new System.Drawing.Size(458, 199);
            this.action_groupBox.ResumeLayout(false);
            this.action_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label currentVPN_label;
        private System.Windows.Forms.TextBox vpnname_textBox;
        private System.Windows.Forms.GroupBox action_groupBox;
        private System.Windows.Forms.TextBox status_textBox;
        private System.Windows.Forms.Label status_label;
        private System.Windows.Forms.TextBox log_textBox;
        private System.Windows.Forms.Label log_label;
    }
}
