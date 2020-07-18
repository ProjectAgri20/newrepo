namespace HP.ScalableTest.Plugin.PaperCutAgentless
{
    partial class PaperCutAgentlessExecutionControl
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
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.label_SessionId = new System.Windows.Forms.Label();
            this.label_PullPrintAction = new System.Windows.Forms.Label();
            this.label_Action = new System.Windows.Forms.Label();
            this.solution_Label = new System.Windows.Forms.Label();
            this.label_ActiveDevice = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(3, 75);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 15);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Execution Status";
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Status.Location = new System.Drawing.Point(3, 91);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.ReadOnly = true;
            this.richTextBox_Status.Size = new System.Drawing.Size(621, 304);
            this.richTextBox_Status.TabIndex = 1;
            this.richTextBox_Status.Text = "";
            this.richTextBox_Status.WordWrap = false;
            // 
            // label_SessionId
            // 
            this.label_SessionId.AutoSize = true;
            this.label_SessionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SessionId.Location = new System.Drawing.Point(195, 7);
            this.label_SessionId.Name = "label_SessionId";
            this.label_SessionId.Size = new System.Drawing.Size(68, 13);
            this.label_SessionId.TabIndex = 29;
            this.label_SessionId.Text = "SESSION ID";
            // 
            // label_PullPrintAction
            // 
            this.label_PullPrintAction.AutoSize = true;
            this.label_PullPrintAction.Location = new System.Drawing.Point(194, 54);
            this.label_PullPrintAction.Name = "label_PullPrintAction";
            this.label_PullPrintAction.Size = new System.Drawing.Size(29, 15);
            this.label_PullPrintAction.TabIndex = 28;
            this.label_PullPrintAction.Text = "TBD";
            // 
            // label_Action
            // 
            this.label_Action.AutoSize = true;
            this.label_Action.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Action.Location = new System.Drawing.Point(84, 54);
            this.label_Action.Name = "label_Action";
            this.label_Action.Size = new System.Drawing.Size(104, 13);
            this.label_Action.TabIndex = 26;
            this.label_Action.Text = "Document Action";
            this.label_Action.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // solution_Label
            // 
            this.solution_Label.AutoSize = true;
            this.solution_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solution_Label.Location = new System.Drawing.Point(3, 7);
            this.solution_Label.Name = "solution_Label";
            this.solution_Label.Size = new System.Drawing.Size(186, 13);
            this.solution_Label.TabIndex = 27;
            this.solution_Label.Text = "PaperCutAgentless Pull Printing";
            // 
            // label_ActiveDevice
            // 
            this.label_ActiveDevice.AutoSize = true;
            this.label_ActiveDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ActiveDevice.Location = new System.Drawing.Point(194, 31);
            this.label_ActiveDevice.Name = "label_ActiveDevice";
            this.label_ActiveDevice.Size = new System.Drawing.Size(27, 13);
            this.label_ActiveDevice.TabIndex = 25;
            this.label_ActiveDevice.Text = "N/A";
            // 
            // label_Device
            // 
            this.label_Device.AutoSize = true;
            this.label_Device.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Device.Location = new System.Drawing.Point(141, 31);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(47, 13);
            this.label_Device.TabIndex = 24;
            this.label_Device.Text = "Device";
            // 
            // PaperCutAgentlessExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_SessionId);
            this.Controls.Add(this.label_PullPrintAction);
            this.Controls.Add(this.label_Action);
            this.Controls.Add(this.solution_Label);
            this.Controls.Add(this.label_ActiveDevice);
            this.Controls.Add(this.label_Device);
            this.Controls.Add(this.richTextBox_Status);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PaperCutAgentlessExecutionControl";
            this.Size = new System.Drawing.Size(633, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox richTextBox_Status;
        private System.Windows.Forms.Label label_SessionId;
        private System.Windows.Forms.Label label_PullPrintAction;
        private System.Windows.Forms.Label label_Action;
        private System.Windows.Forms.Label solution_Label;
        private System.Windows.Forms.Label label_ActiveDevice;
        private System.Windows.Forms.Label label_Device;
    }
}
