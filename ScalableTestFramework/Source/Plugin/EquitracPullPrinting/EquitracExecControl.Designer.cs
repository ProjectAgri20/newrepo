namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    partial class EquitracExecControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.status_Label = new System.Windows.Forms.Label();
            this.activeDevice_Label = new System.Windows.Forms.Label();
            this.device_Label = new System.Windows.Forms.Label();
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.solution_Label = new System.Windows.Forms.Label();
            this.label_sessionId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDocumentProcessAction
            // 
            this.labelDocumentProcessAction.AutoSize = true;
            this.labelDocumentProcessAction.Location = new System.Drawing.Point(139, 57);
            this.labelDocumentProcessAction.Name = "labelDocumentProcessAction";
            this.labelDocumentProcessAction.Size = new System.Drawing.Size(29, 13);
            this.labelDocumentProcessAction.TabIndex = 21;
            this.labelDocumentProcessAction.Text = "TBD";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Document Action";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // status_Label
            // 
            this.status_Label.AutoSize = true;
            this.status_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_Label.Location = new System.Drawing.Point(4, 81);
            this.status_Label.Name = "status_Label";
            this.status_Label.Size = new System.Drawing.Size(103, 13);
            this.status_Label.TabIndex = 18;
            this.status_Label.Text = "Execution Status";
            // 
            // activeDevice_Label
            // 
            this.activeDevice_Label.AutoSize = true;
            this.activeDevice_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activeDevice_Label.Location = new System.Drawing.Point(139, 33);
            this.activeDevice_Label.Name = "activeDevice_Label";
            this.activeDevice_Label.Size = new System.Drawing.Size(27, 13);
            this.activeDevice_Label.TabIndex = 17;
            this.activeDevice_Label.Text = "N/A";
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_Label.Location = new System.Drawing.Point(83, 33);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(47, 13);
            this.device_Label.TabIndex = 16;
            this.device_Label.Text = "Device";
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.status_RichTextBox.Location = new System.Drawing.Point(0, 97);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(539, 254);
            this.status_RichTextBox.TabIndex = 23;
            this.status_RichTextBox.Text = "";
            // 
            // solution_Label
            // 
            this.solution_Label.AutoSize = true;
            this.solution_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solution_Label.Location = new System.Drawing.Point(5, 10);
            this.solution_Label.Name = "solution_Label";
            this.solution_Label.Size = new System.Drawing.Size(175, 13);
            this.solution_Label.TabIndex = 24;
            this.solution_Label.Text = "Equitrac : Follow-You Printing";
            // 
            // label_sessionId
            // 
            this.label_sessionId.AutoSize = true;
            this.label_sessionId.Location = new System.Drawing.Point(186, 10);
            this.label_sessionId.Name = "label_sessionId";
            this.label_sessionId.Size = new System.Drawing.Size(58, 13);
            this.label_sessionId.TabIndex = 25;
            this.label_sessionId.Text = "Session ID";
            // 
            // EquitracExecControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_sessionId);
            this.Controls.Add(this.solution_Label);
            this.Controls.Add(this.status_RichTextBox);
            this.Controls.Add(this.labelDocumentProcessAction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.status_Label);
            this.Controls.Add(this.activeDevice_Label);
            this.Controls.Add(this.device_Label);
            this.Name = "EquitracExecControl";
            this.Size = new System.Drawing.Size(545, 355);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDocumentProcessAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label status_Label;
        private System.Windows.Forms.Label activeDevice_Label;
        private System.Windows.Forms.Label device_Label;
        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label solution_Label;
        private System.Windows.Forms.Label label_sessionId;
    }
}
