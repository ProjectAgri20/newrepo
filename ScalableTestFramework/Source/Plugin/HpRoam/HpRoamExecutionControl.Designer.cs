namespace HP.ScalableTest.Plugin.HpRoam
{
    partial class HpRoamExecutionControl
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
            this.labelDocumentProcessAction = new System.Windows.Forms.Label();
            this.action_Label = new System.Windows.Forms.Label();
            this.solution_Label = new System.Windows.Forms.Label();
            this.activeDevice_Label = new System.Windows.Forms.Label();
            this.device_Label = new System.Windows.Forms.Label();
            this.statusRichTextBox = new System.Windows.Forms.RichTextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.activeSession_Label = new System.Windows.Forms.Label();
            this.session_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDocumentProcessAction
            // 
            this.labelDocumentProcessAction.AutoSize = true;
            this.labelDocumentProcessAction.Location = new System.Drawing.Point(141, 76);
            this.labelDocumentProcessAction.Name = "labelDocumentProcessAction";
            this.labelDocumentProcessAction.Size = new System.Drawing.Size(29, 15);
            this.labelDocumentProcessAction.TabIndex = 27;
            this.labelDocumentProcessAction.Text = "TBD";
            // 
            // action_Label
            // 
            this.action_Label.AutoSize = true;
            this.action_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.action_Label.Location = new System.Drawing.Point(34, 76);
            this.action_Label.Name = "action_Label";
            this.action_Label.Size = new System.Drawing.Size(101, 13);
            this.action_Label.TabIndex = 25;
            this.action_Label.Text = "Print Job Action:";
            this.action_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // solution_Label
            // 
            this.solution_Label.AutoSize = true;
            this.solution_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solution_Label.Location = new System.Drawing.Point(4, 8);
            this.solution_Label.Name = "solution_Label";
            this.solution_Label.Size = new System.Drawing.Size(132, 13);
            this.solution_Label.TabIndex = 26;
            this.solution_Label.Text = "HP Roam Pull Printing";
            // 
            // activeDevice_Label
            // 
            this.activeDevice_Label.AutoSize = true;
            this.activeDevice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeDevice_Label.Location = new System.Drawing.Point(141, 53);
            this.activeDevice_Label.Name = "activeDevice_Label";
            this.activeDevice_Label.Size = new System.Drawing.Size(29, 13);
            this.activeDevice_Label.TabIndex = 24;
            this.activeDevice_Label.Text = "TBD";
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_Label.Location = new System.Drawing.Point(85, 53);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(51, 13);
            this.device_Label.TabIndex = 23;
            this.device_Label.Text = "Device:";
            // 
            // statusRichTextBox
            // 
            this.statusRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusRichTextBox.Location = new System.Drawing.Point(7, 120);
            this.statusRichTextBox.Name = "statusRichTextBox";
            this.statusRichTextBox.ReadOnly = true;
            this.statusRichTextBox.Size = new System.Drawing.Size(499, 239);
            this.statusRichTextBox.TabIndex = 22;
            this.statusRichTextBox.Text = "";
            this.statusRichTextBox.WordWrap = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(4, 102);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(100, 15);
            this.statusLabel.TabIndex = 21;
            this.statusLabel.Text = "Execution Status";
            // 
            // activeSession_Label
            // 
            this.activeSession_Label.AutoSize = true;
            this.activeSession_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeSession_Label.Location = new System.Drawing.Point(141, 28);
            this.activeSession_Label.Name = "activeSession_Label";
            this.activeSession_Label.Size = new System.Drawing.Size(29, 13);
            this.activeSession_Label.TabIndex = 29;
            this.activeSession_Label.Text = "TBD";
            // 
            // session_Label
            // 
            this.session_Label.AutoSize = true;
            this.session_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.session_Label.Location = new System.Drawing.Point(69, 28);
            this.session_Label.Name = "session_Label";
            this.session_Label.Size = new System.Drawing.Size(66, 13);
            this.session_Label.TabIndex = 28;
            this.session_Label.Text = "SessionId:";
            // 
            // HpRoamExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.activeSession_Label);
            this.Controls.Add(this.session_Label);
            this.Controls.Add(this.labelDocumentProcessAction);
            this.Controls.Add(this.action_Label);
            this.Controls.Add(this.solution_Label);
            this.Controls.Add(this.activeDevice_Label);
            this.Controls.Add(this.device_Label);
            this.Controls.Add(this.statusRichTextBox);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpRoamExecutionControl";
            this.Size = new System.Drawing.Size(511, 366);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDocumentProcessAction;
        private System.Windows.Forms.Label action_Label;
        private System.Windows.Forms.Label solution_Label;
        private System.Windows.Forms.Label activeDevice_Label;
        private System.Windows.Forms.Label device_Label;
        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label activeSession_Label;
        private System.Windows.Forms.Label session_Label;
    }
}
