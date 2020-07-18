namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    partial class LinkExecutionControl
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
            this.deviceLabel = new System.Windows.Forms.Label();
            this.activeDeviceLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusRichTextBox = new System.Windows.Forms.RichTextBox();
            this.appNamelabel = new System.Windows.Forms.Label();
            this.activeappNamelabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceLabel.Location = new System.Drawing.Point(3, 11);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(46, 15);
            this.deviceLabel.TabIndex = 0;
            this.deviceLabel.Text = "Device";
            // 
            // activeDeviceLabel
            // 
            this.activeDeviceLabel.AutoSize = true;
            this.activeDeviceLabel.Location = new System.Drawing.Point(68, 11);
            this.activeDeviceLabel.Name = "activeDeviceLabel";
            this.activeDeviceLabel.Size = new System.Drawing.Size(29, 15);
            this.activeDeviceLabel.TabIndex = 1;
            this.activeDeviceLabel.Text = "N/A";
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
            this.statusRichTextBox.Size = new System.Drawing.Size(511, 246);
            this.statusRichTextBox.TabIndex = 2;
            this.statusRichTextBox.Text = "";
            // 
            // appNamelabel
            // 
            this.appNamelabel.AutoSize = true;
            this.appNamelabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appNamelabel.Location = new System.Drawing.Point(155, 11);
            this.appNamelabel.Name = "appNamelabel";
            this.appNamelabel.Size = new System.Drawing.Size(65, 15);
            this.appNamelabel.TabIndex = 3;
            this.appNamelabel.Text = "App Name";
            // 
            // activeappNamelabel
            // 
            this.activeappNamelabel.AutoSize = true;
            this.activeappNamelabel.Location = new System.Drawing.Point(243, 11);
            this.activeappNamelabel.Name = "activeappNamelabel";
            this.activeappNamelabel.Size = new System.Drawing.Size(29, 15);
            this.activeappNamelabel.TabIndex = 4;
            this.activeappNamelabel.Text = "N/A";
            // 
            // LinkExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.activeappNamelabel);
            this.Controls.Add(this.appNamelabel);
            this.Controls.Add(this.statusRichTextBox);
            this.Controls.Add(this.activeDeviceLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.deviceLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "LinkExecutionControl";
            this.Size = new System.Drawing.Size(520, 320);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Label activeDeviceLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Label appNamelabel;
        private System.Windows.Forms.Label activeappNamelabel;
    }
}
