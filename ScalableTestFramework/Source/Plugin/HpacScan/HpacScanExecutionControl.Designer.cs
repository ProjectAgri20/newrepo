namespace HP.ScalableTest.Plugin.HpacScan
{
    partial class HpacScanExecutionControl
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
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusRichTextBox = new System.Windows.Forms.RichTextBox();
            this.device_Label = new System.Windows.Forms.Label();
            this.activeDeviceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(3, 43);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 15);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Execution Status";
            // 
            // statusRichTextBox
            // 
            this.statusRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusRichTextBox.Location = new System.Drawing.Point(6, 71);
            this.statusRichTextBox.Name = "statusRichTextBox";
            this.statusRichTextBox.ReadOnly = true;
            this.statusRichTextBox.Size = new System.Drawing.Size(624, 324);
            this.statusRichTextBox.TabIndex = 1;
            this.statusRichTextBox.Text = "";
            this.statusRichTextBox.WordWrap = false;
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_Label.Location = new System.Drawing.Point(3, 14);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(47, 13);
            this.device_Label.TabIndex = 7;
            this.device_Label.Text = "Device";
            // 
            // activeDeviceLabel
            // 
            this.activeDeviceLabel.AutoSize = true;
            this.activeDeviceLabel.Location = new System.Drawing.Point(72, 14);
            this.activeDeviceLabel.Name = "activeDeviceLabel";
            this.activeDeviceLabel.Size = new System.Drawing.Size(29, 15);
            this.activeDeviceLabel.TabIndex = 8;
            this.activeDeviceLabel.Text = "N/A";
            // 
            // HpacScanExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.activeDeviceLabel);
            this.Controls.Add(this.device_Label);
            this.Controls.Add(this.statusRichTextBox);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpacScanExecutionControl";
            this.Size = new System.Drawing.Size(633, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Label device_Label;
        private System.Windows.Forms.Label activeDeviceLabel;
    }
}
