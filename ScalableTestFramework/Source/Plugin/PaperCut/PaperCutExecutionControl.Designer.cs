namespace HP.ScalableTest.Plugin.PaperCut
{
    partial class PaperCutExecutionControl
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
            this.label_PullPrintAction = new System.Windows.Forms.Label();
            this.label_Action = new System.Windows.Forms.Label();
            this.solution_Label = new System.Windows.Forms.Label();
            this.label_Status = new System.Windows.Forms.Label();
            this.richTextBox_Status = new System.Windows.Forms.RichTextBox();
            this.label_ActiveDevice = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
            this.label_SessionId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_PullPrintAction
            // 
            this.label_PullPrintAction.AutoSize = true;
            this.label_PullPrintAction.Location = new System.Drawing.Point(138, 51);
            this.label_PullPrintAction.Name = "label_PullPrintAction";
            this.label_PullPrintAction.Size = new System.Drawing.Size(29, 13);
            this.label_PullPrintAction.TabIndex = 22;
            this.label_PullPrintAction.Text = "TBD";
            // 
            // label_Action
            // 
            this.label_Action.AutoSize = true;
            this.label_Action.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Action.Location = new System.Drawing.Point(25, 51);
            this.label_Action.Name = "label_Action";
            this.label_Action.Size = new System.Drawing.Size(104, 13);
            this.label_Action.TabIndex = 20;
            this.label_Action.Text = "Document Action";
            this.label_Action.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // solution_Label
            // 
            this.solution_Label.AutoSize = true;
            this.solution_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solution_Label.Location = new System.Drawing.Point(3, 4);
            this.solution_Label.Name = "solution_Label";
            this.solution_Label.Size = new System.Drawing.Size(131, 13);
            this.solution_Label.TabIndex = 21;
            this.solution_Label.Text = "PaperCut Pull Printing";
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Status.Location = new System.Drawing.Point(3, 75);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(103, 13);
            this.label_Status.TabIndex = 19;
            this.label_Status.Text = "Execution Status";
            // 
            // richTextBox_Status
            // 
            this.richTextBox_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Status.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox_Status.Location = new System.Drawing.Point(3, 91);
            this.richTextBox_Status.Name = "richTextBox_Status";
            this.richTextBox_Status.ReadOnly = true;
            this.richTextBox_Status.Size = new System.Drawing.Size(627, 304);
            this.richTextBox_Status.TabIndex = 18;
            this.richTextBox_Status.Text = "";
            // 
            // label_ActiveDevice
            // 
            this.label_ActiveDevice.AutoSize = true;
            this.label_ActiveDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ActiveDevice.Location = new System.Drawing.Point(138, 28);
            this.label_ActiveDevice.Name = "label_ActiveDevice";
            this.label_ActiveDevice.Size = new System.Drawing.Size(27, 13);
            this.label_ActiveDevice.TabIndex = 17;
            this.label_ActiveDevice.Text = "N/A";
            // 
            // label_Device
            // 
            this.label_Device.AutoSize = true;
            this.label_Device.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Device.Location = new System.Drawing.Point(82, 28);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(47, 13);
            this.label_Device.TabIndex = 16;
            this.label_Device.Text = "Device";
            // 
            // label_SessionId
            // 
            this.label_SessionId.AutoSize = true;
            this.label_SessionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SessionId.Location = new System.Drawing.Point(140, 4);
            this.label_SessionId.Name = "label_SessionId";
            this.label_SessionId.Size = new System.Drawing.Size(68, 13);
            this.label_SessionId.TabIndex = 23;
            this.label_SessionId.Text = "SESSION ID";
            // 
            // PaperCutPullPrintingExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_SessionId);
            this.Controls.Add(this.label_PullPrintAction);
            this.Controls.Add(this.label_Action);
            this.Controls.Add(this.solution_Label);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.richTextBox_Status);
            this.Controls.Add(this.label_ActiveDevice);
            this.Controls.Add(this.label_Device);
            this.Name = "PaperCutPullPrintingExecutionControl";
            this.Size = new System.Drawing.Size(633, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_PullPrintAction;
        private System.Windows.Forms.Label label_Action;
        private System.Windows.Forms.Label solution_Label;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.RichTextBox richTextBox_Status;
        private System.Windows.Forms.Label label_ActiveDevice;
        private System.Windows.Forms.Label label_Device;
        private System.Windows.Forms.Label label_SessionId;
    }
}
