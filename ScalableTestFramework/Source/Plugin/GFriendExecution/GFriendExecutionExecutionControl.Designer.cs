namespace HP.ScalableTest.Plugin.GFriendExecution
{
    partial class GFriendExecutionExecutionControl
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
            this.output_Label = new System.Windows.Forms.Label();
            this.output_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.dut_Label = new System.Windows.Forms.Label();
            this.session_Label = new System.Windows.Forms.Label();
            this.sessionId_value_label = new System.Windows.Forms.Label();
            this.dut_value_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // output_Label
            // 
            this.output_Label.AutoSize = true;
            this.output_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output_Label.Location = new System.Drawing.Point(3, 37);
            this.output_Label.Name = "output_Label";
            this.output_Label.Size = new System.Drawing.Size(105, 15);
            this.output_Label.TabIndex = 0;
            this.output_Label.Text = "Execution Output";
            // 
            // output_RichTextBox
            // 
            this.output_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output_RichTextBox.Location = new System.Drawing.Point(6, 55);
            this.output_RichTextBox.Name = "output_RichTextBox";
            this.output_RichTextBox.ReadOnly = true;
            this.output_RichTextBox.Size = new System.Drawing.Size(611, 326);
            this.output_RichTextBox.TabIndex = 1;
            this.output_RichTextBox.Text = "";
            this.output_RichTextBox.WordWrap = false;
            // 
            // dut_Label
            // 
            this.dut_Label.AutoSize = true;
            this.dut_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dut_Label.Location = new System.Drawing.Point(275, 10);
            this.dut_Label.Name = "dut_Label";
            this.dut_Label.Size = new System.Drawing.Size(118, 13);
            this.dut_Label.TabIndex = 15;
            this.dut_Label.Text = "Device Under Test:";
            // 
            // session_Label
            // 
            this.session_Label.AutoSize = true;
            this.session_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.session_Label.Location = new System.Drawing.Point(6, 10);
            this.session_Label.Name = "session_Label";
            this.session_Label.Size = new System.Drawing.Size(72, 13);
            this.session_Label.TabIndex = 14;
            this.session_Label.Text = "Session ID:";
            // 
            // sessionId_value_label
            // 
            this.sessionId_value_label.AutoSize = true;
            this.sessionId_value_label.Location = new System.Drawing.Point(84, 9);
            this.sessionId_value_label.Name = "sessionId_value_label";
            this.sessionId_value_label.Size = new System.Drawing.Size(57, 15);
            this.sessionId_value_label.TabIndex = 16;
            this.sessionId_value_label.Text = "SessionID";
            // 
            // dut_value_label
            // 
            this.dut_value_label.AutoSize = true;
            this.dut_value_label.Location = new System.Drawing.Point(399, 9);
            this.dut_value_label.Name = "dut_value_label";
            this.dut_value_label.Size = new System.Drawing.Size(36, 15);
            this.dut_value_label.TabIndex = 17;
            this.dut_value_label.Text = "None";
            // 
            // GFriendExecutionExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dut_value_label);
            this.Controls.Add(this.sessionId_value_label);
            this.Controls.Add(this.dut_Label);
            this.Controls.Add(this.session_Label);
            this.Controls.Add(this.output_RichTextBox);
            this.Controls.Add(this.output_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GFriendExecutionExecutionControl";
            this.Size = new System.Drawing.Size(623, 384);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label output_Label;
        private System.Windows.Forms.RichTextBox output_RichTextBox;
        private System.Windows.Forms.Label dut_Label;
        private System.Windows.Forms.Label session_Label;
        private System.Windows.Forms.Label sessionId_value_label;
        private System.Windows.Forms.Label dut_value_label;
    }
}
