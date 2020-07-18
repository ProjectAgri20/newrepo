namespace HP.ScalableTest.Plugin.SafeQPullPrinting
{
    partial class SafeQPullPrintingExecutionControl
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
            this.label_safeqPullPrinting = new System.Windows.Forms.Label();
            this.label_sessionId = new System.Windows.Forms.Label();
            this.label_device = new System.Windows.Forms.Label();
            this.label_pullPrintAction = new System.Windows.Forms.Label();
            this.label_activeDevice = new System.Windows.Forms.Label();
            this.label_documentAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(3, 85);
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
            this.statusRichTextBox.Location = new System.Drawing.Point(3, 103);
            this.statusRichTextBox.Name = "statusRichTextBox";
            this.statusRichTextBox.ReadOnly = true;
            this.statusRichTextBox.Size = new System.Drawing.Size(621, 295);
            this.statusRichTextBox.TabIndex = 1;
            this.statusRichTextBox.Text = "";
            this.statusRichTextBox.WordWrap = false;
            // 
            // label_safeqPullPrinting
            // 
            this.label_safeqPullPrinting.AutoSize = true;
            this.label_safeqPullPrinting.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_safeqPullPrinting.Location = new System.Drawing.Point(3, 9);
            this.label_safeqPullPrinting.Name = "label_safeqPullPrinting";
            this.label_safeqPullPrinting.Size = new System.Drawing.Size(111, 15);
            this.label_safeqPullPrinting.TabIndex = 2;
            this.label_safeqPullPrinting.Text = "SafeQ Pull Printing";
            // 
            // label_sessionId
            // 
            this.label_sessionId.AutoSize = true;
            this.label_sessionId.Location = new System.Drawing.Point(120, 9);
            this.label_sessionId.Name = "label_sessionId";
            this.label_sessionId.Size = new System.Drawing.Size(60, 15);
            this.label_sessionId.TabIndex = 3;
            this.label_sessionId.Text = "Session ID";
            // 
            // label_device
            // 
            this.label_device.AutoSize = true;
            this.label_device.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_device.Location = new System.Drawing.Point(68, 35);
            this.label_device.Name = "label_device";
            this.label_device.Size = new System.Drawing.Size(46, 15);
            this.label_device.TabIndex = 4;
            this.label_device.Text = "Device";
            // 
            // label_pullPrintAction
            // 
            this.label_pullPrintAction.AutoSize = true;
            this.label_pullPrintAction.Location = new System.Drawing.Point(120, 60);
            this.label_pullPrintAction.Name = "label_pullPrintAction";
            this.label_pullPrintAction.Size = new System.Drawing.Size(42, 15);
            this.label_pullPrintAction.TabIndex = 10;
            this.label_pullPrintAction.Text = "Action";
            // 
            // label_activeDevice
            // 
            this.label_activeDevice.AutoSize = true;
            this.label_activeDevice.Location = new System.Drawing.Point(120, 35);
            this.label_activeDevice.Name = "label_activeDevice";
            this.label_activeDevice.Size = new System.Drawing.Size(29, 15);
            this.label_activeDevice.TabIndex = 9;
            this.label_activeDevice.Text = "N/A";
            // 
            // label_documentAction
            // 
            this.label_documentAction.AutoSize = true;
            this.label_documentAction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_documentAction.Location = new System.Drawing.Point(9, 60);
            this.label_documentAction.Name = "label_documentAction";
            this.label_documentAction.Size = new System.Drawing.Size(105, 15);
            this.label_documentAction.TabIndex = 8;
            this.label_documentAction.Text = "Document Action";
            // 
            // SafeQPullPrintingExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_pullPrintAction);
            this.Controls.Add(this.label_activeDevice);
            this.Controls.Add(this.label_documentAction);
            this.Controls.Add(this.label_device);
            this.Controls.Add(this.label_sessionId);
            this.Controls.Add(this.label_safeqPullPrinting);
            this.Controls.Add(this.statusRichTextBox);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SafeQPullPrintingExecutionControl";
            this.Size = new System.Drawing.Size(633, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Label label_safeqPullPrinting;
        private System.Windows.Forms.Label label_sessionId;
        private System.Windows.Forms.Label label_device;
        private System.Windows.Forms.Label label_pullPrintAction;
        private System.Windows.Forms.Label label_activeDevice;
        private System.Windows.Forms.Label label_documentAction;
    }
}
