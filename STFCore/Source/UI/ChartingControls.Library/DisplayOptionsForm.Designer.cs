namespace HP.ScalableTest.UI.Charting
{
    partial class DisplayOptionsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.displayError_CheckBox = new System.Windows.Forms.CheckBox();
            this.displayOther_CheckBox = new System.Windows.Forms.CheckBox();
            this.displaySkipped_CheckBox = new System.Windows.Forms.CheckBox();
            this.displayFailed_CheckBox = new System.Windows.Forms.CheckBox();
            this.displayCompleted_CheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(236, 198);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 0;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(155, 198);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 5;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.displayError_CheckBox);
            this.groupBox1.Controls.Add(this.displayOther_CheckBox);
            this.groupBox1.Controls.Add(this.displaySkipped_CheckBox);
            this.groupBox1.Controls.Add(this.displayFailed_CheckBox);
            this.groupBox1.Controls.Add(this.displayCompleted_CheckBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Default Filters";
            // 
            // displayError_CheckBox
            // 
            this.displayError_CheckBox.AutoSize = true;
            this.displayError_CheckBox.Location = new System.Drawing.Point(20, 94);
            this.displayError_CheckBox.Name = "displayError_CheckBox";
            this.displayError_CheckBox.Size = new System.Drawing.Size(191, 17);
            this.displayError_CheckBox.TabIndex = 3;
            this.displayError_CheckBox.Text = "Display series with \"Error\" activities";
            this.displayError_CheckBox.UseVisualStyleBackColor = true;
            // 
            // displayOther_CheckBox
            // 
            this.displayOther_CheckBox.AutoSize = true;
            this.displayOther_CheckBox.Location = new System.Drawing.Point(20, 117);
            this.displayOther_CheckBox.Name = "displayOther_CheckBox";
            this.displayOther_CheckBox.Size = new System.Drawing.Size(117, 17);
            this.displayOther_CheckBox.TabIndex = 4;
            this.displayOther_CheckBox.Text = "Display other series";
            this.displayOther_CheckBox.UseVisualStyleBackColor = true;
            // 
            // displaySkipped_CheckBox
            // 
            this.displaySkipped_CheckBox.AutoSize = true;
            this.displaySkipped_CheckBox.Location = new System.Drawing.Point(20, 71);
            this.displaySkipped_CheckBox.Name = "displaySkipped_CheckBox";
            this.displaySkipped_CheckBox.Size = new System.Drawing.Size(208, 17);
            this.displaySkipped_CheckBox.TabIndex = 2;
            this.displaySkipped_CheckBox.Text = "Display series with \"Skipped\" activities";
            this.displaySkipped_CheckBox.UseVisualStyleBackColor = true;
            // 
            // displayFailed_CheckBox
            // 
            this.displayFailed_CheckBox.AutoSize = true;
            this.displayFailed_CheckBox.Location = new System.Drawing.Point(20, 48);
            this.displayFailed_CheckBox.Name = "displayFailed_CheckBox";
            this.displayFailed_CheckBox.Size = new System.Drawing.Size(197, 17);
            this.displayFailed_CheckBox.TabIndex = 1;
            this.displayFailed_CheckBox.Text = "Display series with \"Failed\" activities";
            this.displayFailed_CheckBox.UseVisualStyleBackColor = true;
            // 
            // displayCompleted_CheckBox
            // 
            this.displayCompleted_CheckBox.AutoSize = true;
            this.displayCompleted_CheckBox.Location = new System.Drawing.Point(20, 25);
            this.displayCompleted_CheckBox.Name = "displayCompleted_CheckBox";
            this.displayCompleted_CheckBox.Size = new System.Drawing.Size(219, 17);
            this.displayCompleted_CheckBox.TabIndex = 0;
            this.displayCompleted_CheckBox.Text = "Display series with \"Completed\" activities";
            this.displayCompleted_CheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Note: This will clear all series filters.";
            // 
            // DisplayOptionsForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(323, 233);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisplayOptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Display Options";
            this.Load += new System.EventHandler(this.DisplayOptionsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox displayOther_CheckBox;
        private System.Windows.Forms.CheckBox displaySkipped_CheckBox;
        private System.Windows.Forms.CheckBox displayFailed_CheckBox;
        private System.Windows.Forms.CheckBox displayCompleted_CheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox displayError_CheckBox;
    }
}