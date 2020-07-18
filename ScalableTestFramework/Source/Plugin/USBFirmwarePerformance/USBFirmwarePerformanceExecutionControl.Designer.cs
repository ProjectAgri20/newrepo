namespace HP.ScalableTest.Plugin.USBFirmwarePerformance
{
    partial class USBFirmwarePerformanceExecutionControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.value_label = new System.Windows.Forms.Label();
            this.device_textBox = new System.Windows.Forms.TextBox();
            this.device_label = new System.Windows.Forms.Label();
            this.flashProgress_label = new System.Windows.Forms.Label();
            this.flash_progressBar = new System.Windows.Forms.ProgressBar();
            this.postRevision_textBox = new System.Windows.Forms.TextBox();
            this.postRevision_label = new System.Windows.Forms.Label();
            this.currentRevision_textBox = new System.Windows.Forms.TextBox();
            this.currentRevision_label = new System.Windows.Forms.Label();
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.value_label);
            this.groupBox1.Controls.Add(this.device_textBox);
            this.groupBox1.Controls.Add(this.device_label);
            this.groupBox1.Controls.Add(this.flashProgress_label);
            this.groupBox1.Controls.Add(this.flash_progressBar);
            this.groupBox1.Controls.Add(this.postRevision_textBox);
            this.groupBox1.Controls.Add(this.postRevision_label);
            this.groupBox1.Controls.Add(this.currentRevision_textBox);
            this.groupBox1.Controls.Add(this.currentRevision_label);
            this.groupBox1.Location = new System.Drawing.Point(6, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(453, 161);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Firmware";
            // 
            // value_label
            // 
            this.value_label.AutoSize = true;
            this.value_label.Location = new System.Drawing.Point(425, 136);
            this.value_label.Name = "value_label";
            this.value_label.Size = new System.Drawing.Size(23, 15);
            this.value_label.TabIndex = 12;
            this.value_label.Text = "0%";
            // 
            // device_textBox
            // 
            this.device_textBox.BackColor = System.Drawing.Color.White;
            this.device_textBox.Location = new System.Drawing.Point(9, 37);
            this.device_textBox.Name = "device_textBox";
            this.device_textBox.ReadOnly = true;
            this.device_textBox.Size = new System.Drawing.Size(163, 23);
            this.device_textBox.TabIndex = 11;
            // 
            // device_label
            // 
            this.device_label.AutoSize = true;
            this.device_label.Location = new System.Drawing.Point(6, 19);
            this.device_label.Name = "device_label";
            this.device_label.Size = new System.Drawing.Size(42, 15);
            this.device_label.TabIndex = 10;
            this.device_label.Text = "Device";
            // 
            // flashProgress_label
            // 
            this.flashProgress_label.AutoSize = true;
            this.flashProgress_label.Location = new System.Drawing.Point(6, 114);
            this.flashProgress_label.Name = "flashProgress_label";
            this.flashProgress_label.Size = new System.Drawing.Size(82, 15);
            this.flashProgress_label.TabIndex = 9;
            this.flashProgress_label.Text = "Flash Progress";
            // 
            // flash_progressBar
            // 
            this.flash_progressBar.Location = new System.Drawing.Point(6, 132);
            this.flash_progressBar.Name = "flash_progressBar";
            this.flash_progressBar.Size = new System.Drawing.Size(413, 23);
            this.flash_progressBar.TabIndex = 8;
            // 
            // postRevision_textBox
            // 
            this.postRevision_textBox.BackColor = System.Drawing.Color.White;
            this.postRevision_textBox.Location = new System.Drawing.Point(213, 84);
            this.postRevision_textBox.Name = "postRevision_textBox";
            this.postRevision_textBox.ReadOnly = true;
            this.postRevision_textBox.Size = new System.Drawing.Size(171, 23);
            this.postRevision_textBox.TabIndex = 7;
            // 
            // postRevision_label
            // 
            this.postRevision_label.AutoSize = true;
            this.postRevision_label.Location = new System.Drawing.Point(210, 66);
            this.postRevision_label.Name = "postRevision_label";
            this.postRevision_label.Size = new System.Drawing.Size(129, 15);
            this.postRevision_label.TabIndex = 6;
            this.postRevision_label.Text = "Post Firmware Revision";
            // 
            // currentRevision_textBox
            // 
            this.currentRevision_textBox.BackColor = System.Drawing.Color.White;
            this.currentRevision_textBox.Location = new System.Drawing.Point(9, 84);
            this.currentRevision_textBox.Name = "currentRevision_textBox";
            this.currentRevision_textBox.ReadOnly = true;
            this.currentRevision_textBox.Size = new System.Drawing.Size(163, 23);
            this.currentRevision_textBox.TabIndex = 5;
            // 
            // currentRevision_label
            // 
            this.currentRevision_label.AutoSize = true;
            this.currentRevision_label.Location = new System.Drawing.Point(6, 66);
            this.currentRevision_label.Name = "currentRevision_label";
            this.currentRevision_label.Size = new System.Drawing.Size(103, 15);
            this.currentRevision_label.TabIndex = 4;
            this.currentRevision_label.Text = "Firmware Revision";
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.Location = new System.Drawing.Point(6, 204);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(448, 176);
            this.status_RichTextBox.TabIndex = 6;
            this.status_RichTextBox.Text = "";
            this.status_RichTextBox.WordWrap = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(3, 10);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 15);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "Execution Status";
            // 
            // USBFirmwarePerformanceExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.status_RichTextBox);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "USBFirmwarePerformanceExecutionControl";
            this.Size = new System.Drawing.Size(463, 423);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label value_label;
        private System.Windows.Forms.TextBox device_textBox;
        private System.Windows.Forms.Label device_label;
        private System.Windows.Forms.Label flashProgress_label;
        private System.Windows.Forms.ProgressBar flash_progressBar;
        private System.Windows.Forms.TextBox postRevision_textBox;
        private System.Windows.Forms.Label postRevision_label;
        private System.Windows.Forms.TextBox currentRevision_textBox;
        private System.Windows.Forms.Label currentRevision_label;
        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label statusLabel;
    }
}
