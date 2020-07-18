namespace HP.ScalableTest.Plugin.ActivityOutcome
{
    partial class ActivityOutcomeConfigControl
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
            this.groupBoxOutcome = new System.Windows.Forms.GroupBox();
            this.success_RadioButton = new System.Windows.Forms.RadioButton();
            this.skip_RadioButton = new System.Windows.Forms.RadioButton();
            this.fail_RadioButton = new System.Windows.Forms.RadioButton();
            this.randomOutcome_CheckBox = new System.Windows.Forms.CheckBox();
            this.error_radioButton = new System.Windows.Forms.RadioButton();
            this.groupBoxOutcome.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxOutcome
            // 
            this.groupBoxOutcome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutcome.Controls.Add(this.error_radioButton);
            this.groupBoxOutcome.Controls.Add(this.success_RadioButton);
            this.groupBoxOutcome.Controls.Add(this.skip_RadioButton);
            this.groupBoxOutcome.Controls.Add(this.fail_RadioButton);
            this.groupBoxOutcome.Location = new System.Drawing.Point(3, 3);
            this.groupBoxOutcome.Name = "groupBoxOutcome";
            this.groupBoxOutcome.Size = new System.Drawing.Size(374, 120);
            this.groupBoxOutcome.TabIndex = 2;
            this.groupBoxOutcome.TabStop = false;
            this.groupBoxOutcome.Text = "Activity Outcome";
            // 
            // success_RadioButton
            // 
            this.success_RadioButton.AutoSize = true;
            this.success_RadioButton.Location = new System.Drawing.Point(14, 19);
            this.success_RadioButton.Name = "success_RadioButton";
            this.success_RadioButton.Size = new System.Drawing.Size(61, 19);
            this.success_RadioButton.TabIndex = 0;
            this.success_RadioButton.TabStop = true;
            this.success_RadioButton.Text = "Passed";
            this.success_RadioButton.UseVisualStyleBackColor = true;
            // 
            // skip_RadioButton
            // 
            this.skip_RadioButton.AutoSize = true;
            this.skip_RadioButton.Location = new System.Drawing.Point(14, 44);
            this.skip_RadioButton.Name = "skip_RadioButton";
            this.skip_RadioButton.Size = new System.Drawing.Size(67, 19);
            this.skip_RadioButton.TabIndex = 0;
            this.skip_RadioButton.TabStop = true;
            this.skip_RadioButton.Text = "Skipped";
            this.skip_RadioButton.UseVisualStyleBackColor = true;
            // 
            // fail_RadioButton
            // 
            this.fail_RadioButton.AutoSize = true;
            this.fail_RadioButton.Location = new System.Drawing.Point(14, 69);
            this.fail_RadioButton.Name = "fail_RadioButton";
            this.fail_RadioButton.Size = new System.Drawing.Size(56, 19);
            this.fail_RadioButton.TabIndex = 0;
            this.fail_RadioButton.TabStop = true;
            this.fail_RadioButton.Text = "Failed";
            this.fail_RadioButton.UseVisualStyleBackColor = true;
            // 
            // randomOutcome_CheckBox
            // 
            this.randomOutcome_CheckBox.AutoSize = true;
            this.randomOutcome_CheckBox.Location = new System.Drawing.Point(17, 132);
            this.randomOutcome_CheckBox.Name = "randomOutcome_CheckBox";
            this.randomOutcome_CheckBox.Size = new System.Drawing.Size(124, 19);
            this.randomOutcome_CheckBox.TabIndex = 3;
            this.randomOutcome_CheckBox.Text = "Random Outcome";
            this.randomOutcome_CheckBox.UseVisualStyleBackColor = true;
            this.randomOutcome_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBoxRandomCheckedChanged);
            // 
            // error_radioButton
            // 
            this.error_radioButton.AutoSize = true;
            this.error_radioButton.Location = new System.Drawing.Point(14, 94);
            this.error_radioButton.Name = "error_radioButton";
            this.error_radioButton.Size = new System.Drawing.Size(50, 19);
            this.error_radioButton.TabIndex = 0;
            this.error_radioButton.TabStop = true;
            this.error_radioButton.Text = "Error";
            this.error_radioButton.UseVisualStyleBackColor = true;
            // 
            // ActivityOutcomeConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.randomOutcome_CheckBox);
            this.Controls.Add(this.groupBoxOutcome);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActivityOutcomeConfigControl";
            this.Size = new System.Drawing.Size(380, 154);
            this.groupBoxOutcome.ResumeLayout(false);
            this.groupBoxOutcome.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOutcome;
        private System.Windows.Forms.RadioButton success_RadioButton;
        private System.Windows.Forms.RadioButton skip_RadioButton;
        private System.Windows.Forms.RadioButton fail_RadioButton;
        private System.Windows.Forms.CheckBox randomOutcome_CheckBox;
        private System.Windows.Forms.RadioButton error_radioButton;
    }
}
