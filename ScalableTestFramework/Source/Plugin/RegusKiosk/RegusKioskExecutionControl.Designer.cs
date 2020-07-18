namespace HP.ScalableTest.Plugin.RegusKiosk
{
    partial class RegusKioskExecutionControl
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
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.activeDeviceLabel = new System.Windows.Forms.Label();
            this.activejobTypelabel = new System.Windows.Forms.Label();
            this.jobTypelabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.Location = new System.Drawing.Point(9, 76);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(409, 227);
            this.status_RichTextBox.TabIndex = 3;
            this.status_RichTextBox.Text = "";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(8, 53);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(125, 20);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Execution Status";
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceLabel.Location = new System.Drawing.Point(8, 18);
            this.deviceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(55, 20);
            this.deviceLabel.TabIndex = 5;
            this.deviceLabel.Text = "Device";
            // 
            // activeDeviceLabel
            // 
            this.activeDeviceLabel.AutoSize = true;
            this.activeDeviceLabel.Location = new System.Drawing.Point(79, 18);
            this.activeDeviceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.activeDeviceLabel.Name = "activeDeviceLabel";
            this.activeDeviceLabel.Size = new System.Drawing.Size(31, 17);
            this.activeDeviceLabel.TabIndex = 6;
            this.activeDeviceLabel.Text = "N/A";
            // 
            // activejobTypelabel
            // 
            this.activejobTypelabel.AutoSize = true;
            this.activejobTypelabel.Location = new System.Drawing.Point(338, 18);
            this.activejobTypelabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.activejobTypelabel.Name = "activejobTypelabel";
            this.activejobTypelabel.Size = new System.Drawing.Size(31, 17);
            this.activejobTypelabel.TabIndex = 8;
            this.activejobTypelabel.Text = "N/A";
            // 
            // jobTypelabel
            // 
            this.jobTypelabel.AutoSize = true;
            this.jobTypelabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobTypelabel.Location = new System.Drawing.Point(239, 18);
            this.jobTypelabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.jobTypelabel.Name = "jobTypelabel";
            this.jobTypelabel.Size = new System.Drawing.Size(71, 20);
            this.jobTypelabel.TabIndex = 7;
            this.jobTypelabel.Text = "Job Type";
            // 
            // RegusKioskExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.activejobTypelabel);
            this.Controls.Add(this.jobTypelabel);
            this.Controls.Add(this.activeDeviceLabel);
            this.Controls.Add(this.deviceLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.status_RichTextBox);
            this.Name = "KioskExecutionControl";
            this.Size = new System.Drawing.Size(437, 335);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Label activeDeviceLabel;
        private System.Windows.Forms.Label activejobTypelabel;
        private System.Windows.Forms.Label jobTypelabel;
    }
}
