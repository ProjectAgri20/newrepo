namespace HP.ScalableTest.Plugin.Blueprint
{
    partial class BlueprintExecutionControl
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
            this.labelDocumentProcessAction = new System.Windows.Forms.Label();
            this.action_Label = new System.Windows.Forms.Label();
            this.solution_Label = new System.Windows.Forms.Label();
            this.activeDevice_Label = new System.Windows.Forms.Label();
            this.device_Label = new System.Windows.Forms.Label();
            this.label_sessionId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(3, 106);
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
            this.statusRichTextBox.Location = new System.Drawing.Point(6, 124);
            this.statusRichTextBox.Name = "statusRichTextBox";
            this.statusRichTextBox.ReadOnly = true;
            this.statusRichTextBox.Size = new System.Drawing.Size(499, 239);
            this.statusRichTextBox.TabIndex = 1;
            this.statusRichTextBox.Text = "";
            this.statusRichTextBox.WordWrap = false;
            // 
            // labelDocumentProcessAction
            // 
            this.labelDocumentProcessAction.AutoSize = true;
            this.labelDocumentProcessAction.Location = new System.Drawing.Point(138, 59);
            this.labelDocumentProcessAction.Name = "labelDocumentProcessAction";
            this.labelDocumentProcessAction.Size = new System.Drawing.Size(29, 15);
            this.labelDocumentProcessAction.TabIndex = 20;
            this.labelDocumentProcessAction.Text = "TBD";
            // 
            // action_Label
            // 
            this.action_Label.AutoSize = true;
            this.action_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.action_Label.Location = new System.Drawing.Point(25, 59);
            this.action_Label.Name = "action_Label";
            this.action_Label.Size = new System.Drawing.Size(104, 13);
            this.action_Label.TabIndex = 18;
            this.action_Label.Text = "Document Action";
            this.action_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // solution_Label
            // 
            this.solution_Label.AutoSize = true;
            this.solution_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solution_Label.Location = new System.Drawing.Point(3, 12);
            this.solution_Label.Name = "solution_Label";
            this.solution_Label.Size = new System.Drawing.Size(129, 13);
            this.solution_Label.TabIndex = 19;
            this.solution_Label.Text = "Blueprint Pull Printing";
            // 
            // activeDevice_Label
            // 
            this.activeDevice_Label.AutoSize = true;
            this.activeDevice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeDevice_Label.Location = new System.Drawing.Point(138, 36);
            this.activeDevice_Label.Name = "activeDevice_Label";
            this.activeDevice_Label.Size = new System.Drawing.Size(27, 13);
            this.activeDevice_Label.TabIndex = 17;
            this.activeDevice_Label.Text = "N/A";
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_Label.Location = new System.Drawing.Point(82, 36);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(47, 13);
            this.device_Label.TabIndex = 16;
            this.device_Label.Text = "Device";
            // 
            // label_sessionId
            // 
            this.label_sessionId.AutoSize = true;
            this.label_sessionId.Location = new System.Drawing.Point(138, 12);
            this.label_sessionId.Name = "label_sessionId";
            this.label_sessionId.Size = new System.Drawing.Size(60, 15);
            this.label_sessionId.TabIndex = 26;
            this.label_sessionId.Text = "Session ID";
            // 
            // BlueprintExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_sessionId);
            this.Controls.Add(this.labelDocumentProcessAction);
            this.Controls.Add(this.action_Label);
            this.Controls.Add(this.solution_Label);
            this.Controls.Add(this.activeDevice_Label);
            this.Controls.Add(this.device_Label);
            this.Controls.Add(this.statusRichTextBox);
            this.Controls.Add(this.statusLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BlueprintExecutionControl";
            this.Size = new System.Drawing.Size(511, 366);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Label labelDocumentProcessAction;
        private System.Windows.Forms.Label action_Label;
        private System.Windows.Forms.Label solution_Label;
        private System.Windows.Forms.Label activeDevice_Label;
        private System.Windows.Forms.Label device_Label;
        private System.Windows.Forms.Label label_sessionId;
    }
}
