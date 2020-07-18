namespace HP.ScalableTest.Plugin.DirtyDevice
{
    partial class DirtyDeviceExecutionControl
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
            this.status_Label = new System.Windows.Forms.Label();
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.activeDeviceLabel = new System.Windows.Forms.Label();
            this.device_Label = new System.Windows.Forms.Label();
            this.action_Label = new System.Windows.Forms.Label();
            this.labelDocumentProcessAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_Label
            // 
            this.status_Label.AutoSize = true;
            this.status_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_Label.Location = new System.Drawing.Point(3, 56);
            this.status_Label.Name = "status_Label";
            this.status_Label.Size = new System.Drawing.Size(103, 13);
            this.status_Label.TabIndex = 13;
            this.status_Label.Text = "Execution Status";
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.status_RichTextBox.Location = new System.Drawing.Point(3, 72);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(539, 243);
            this.status_RichTextBox.TabIndex = 12;
            this.status_RichTextBox.Text = "";
            this.status_RichTextBox.WordWrap = false;
            // 
            // activeDeviceLabel
            // 
            this.activeDeviceLabel.AutoSize = true;
            this.activeDeviceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeDeviceLabel.Location = new System.Drawing.Point(138, 9);
            this.activeDeviceLabel.Name = "activeDeviceLabel";
            this.activeDeviceLabel.Size = new System.Drawing.Size(27, 13);
            this.activeDeviceLabel.TabIndex = 11;
            this.activeDeviceLabel.Text = "N/A";
            // 
            // device_Label
            // 
            this.device_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_Label.Location = new System.Drawing.Point(25, 9);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(104, 13);
            this.device_Label.TabIndex = 10;
            this.device_Label.Text = "Device";
            this.device_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // action_Label
            // 
            this.action_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.action_Label.Location = new System.Drawing.Point(25, 32);
            this.action_Label.Name = "action_Label";
            this.action_Label.Size = new System.Drawing.Size(104, 13);
            this.action_Label.TabIndex = 14;
            this.action_Label.Text = "Action";
            this.action_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDocumentProcessAction
            // 
            this.labelDocumentProcessAction.AutoSize = true;
            this.labelDocumentProcessAction.Location = new System.Drawing.Point(138, 32);
            this.labelDocumentProcessAction.Name = "labelDocumentProcessAction";
            this.labelDocumentProcessAction.Size = new System.Drawing.Size(29, 13);
            this.labelDocumentProcessAction.TabIndex = 15;
            this.labelDocumentProcessAction.Text = "TBD";
            // 
            // DirtyDeviceExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelDocumentProcessAction);
            this.Controls.Add(this.action_Label);
            this.Controls.Add(this.status_Label);
            this.Controls.Add(this.status_RichTextBox);
            this.Controls.Add(this.activeDeviceLabel);
            this.Controls.Add(this.device_Label);
            this.Name = "DirtyDeviceExecutionControl";
            this.Size = new System.Drawing.Size(545, 318);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label status_Label;
        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label activeDeviceLabel;
        private System.Windows.Forms.Label device_Label;
        private System.Windows.Forms.Label action_Label;
        private System.Windows.Forms.Label labelDocumentProcessAction;

    }
}
