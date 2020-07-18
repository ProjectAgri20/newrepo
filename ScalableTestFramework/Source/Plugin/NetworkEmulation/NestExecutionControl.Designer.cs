namespace HP.ScalableTest.Plugin.NetworkEmulation
{
    partial class NestExecutionControl
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
            this.nest_groupBox = new System.Windows.Forms.GroupBox();
            this.nic_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.error_textBox = new System.Windows.Forms.TextBox();
            this.packetloss_textBox = new System.Windows.Forms.TextBox();
            this.latency_textBox = new System.Windows.Forms.TextBox();
            this.bandwidth_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.networkprofile_textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nest_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // nest_groupBox
            // 
            this.nest_groupBox.Controls.Add(this.networkprofile_textBox);
            this.nest_groupBox.Controls.Add(this.label6);
            this.nest_groupBox.Controls.Add(this.nic_textBox);
            this.nest_groupBox.Controls.Add(this.label5);
            this.nest_groupBox.Controls.Add(this.error_textBox);
            this.nest_groupBox.Controls.Add(this.packetloss_textBox);
            this.nest_groupBox.Controls.Add(this.latency_textBox);
            this.nest_groupBox.Controls.Add(this.bandwidth_textBox);
            this.nest_groupBox.Controls.Add(this.label4);
            this.nest_groupBox.Controls.Add(this.label3);
            this.nest_groupBox.Controls.Add(this.label2);
            this.nest_groupBox.Controls.Add(this.label1);
            this.nest_groupBox.Location = new System.Drawing.Point(29, 24);
            this.nest_groupBox.Name = "nest_groupBox";
            this.nest_groupBox.Size = new System.Drawing.Size(405, 223);
            this.nest_groupBox.TabIndex = 0;
            this.nest_groupBox.TabStop = false;
            this.nest_groupBox.Text = "Network Emulation";
            // 
            // nic_textBox
            // 
            this.nic_textBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.nic_textBox.Location = new System.Drawing.Point(122, 190);
            this.nic_textBox.Name = "nic_textBox";
            this.nic_textBox.ReadOnly = true;
            this.nic_textBox.Size = new System.Drawing.Size(234, 20);
            this.nic_textBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Network Card";
            // 
            // error_textBox
            // 
            this.error_textBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.error_textBox.Location = new System.Drawing.Point(122, 157);
            this.error_textBox.Name = "error_textBox";
            this.error_textBox.ReadOnly = true;
            this.error_textBox.Size = new System.Drawing.Size(234, 20);
            this.error_textBox.TabIndex = 7;
            // 
            // packetloss_textBox
            // 
            this.packetloss_textBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.packetloss_textBox.Location = new System.Drawing.Point(122, 124);
            this.packetloss_textBox.Name = "packetloss_textBox";
            this.packetloss_textBox.ReadOnly = true;
            this.packetloss_textBox.Size = new System.Drawing.Size(234, 20);
            this.packetloss_textBox.TabIndex = 6;
            // 
            // latency_textBox
            // 
            this.latency_textBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.latency_textBox.Location = new System.Drawing.Point(122, 91);
            this.latency_textBox.Name = "latency_textBox";
            this.latency_textBox.ReadOnly = true;
            this.latency_textBox.Size = new System.Drawing.Size(234, 20);
            this.latency_textBox.TabIndex = 5;
            // 
            // bandwidth_textBox
            // 
            this.bandwidth_textBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.bandwidth_textBox.Location = new System.Drawing.Point(122, 58);
            this.bandwidth_textBox.Name = "bandwidth_textBox";
            this.bandwidth_textBox.ReadOnly = true;
            this.bandwidth_textBox.Size = new System.Drawing.Size(234, 20);
            this.bandwidth_textBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Error";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Packet Loss";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Latency";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bandwidth";
            // 
            // networkprofile_textBox
            // 
            this.networkprofile_textBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.networkprofile_textBox.Location = new System.Drawing.Point(122, 25);
            this.networkprofile_textBox.Name = "networkprofile_textBox";
            this.networkprofile_textBox.ReadOnly = true;
            this.networkprofile_textBox.Size = new System.Drawing.Size(234, 20);
            this.networkprofile_textBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Profile";
            // 
            // NESTActivityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nest_groupBox);
            this.Name = "NESTActivityControl";
            this.Size = new System.Drawing.Size(487, 273);
            this.nest_groupBox.ResumeLayout(false);
            this.nest_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox nest_groupBox;
        private System.Windows.Forms.TextBox error_textBox;
        private System.Windows.Forms.TextBox packetloss_textBox;
        private System.Windows.Forms.TextBox latency_textBox;
        private System.Windows.Forms.TextBox bandwidth_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nic_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox networkprofile_textBox;
        private System.Windows.Forms.Label label6;
    }
}
